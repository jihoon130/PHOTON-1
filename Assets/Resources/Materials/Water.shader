Shader "Custom/Water"
{
    Properties
    {
		_BumpMap("노말", 2D) = "bump" {}
		_BumpPower1("노말(1) 세기", float) = 1
		_BumpPower2("노말(2) 세기", float) = 1

		_TimeCount1("노말(1) 흐름", float) = 1
		_TimeCount2("노말(2) 흐름", float) = 1
		
		_Cube("큐브맵", cube) = ""{}
		
		_WaveHeight("웨이브 높이", float) = 1
		_WaveTime("웨이브 속도", float) = 1

		_WaveDistortion("물 왜곡 세기", float) = 0.05

		[HDR]
		_RimCol("물 위 스펙큘러 색", Color) = (1,1,1,1)
		_RimPow("물 위 스펙큘러 세기", float) = 50

    }
    SubShader
    {

        Tags { "RenderType"="Opaque" }
		
		GrabPass{}

        LOD 200

		zwrite on

        CGPROGRAM
        #pragma surface surf Test vertex:vert noambient
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _GrabTexture;
        samplerCUBE _Cube;

		float _BumpPower1, _BumpPower2;
		float _TimeCount1, _TimeCount2;
		float _WaveHeight, _WaveTime,_WaveDistortion;

		float4 _RimCol;
		float _RimPow;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldRefl;
			float3 lightDir;
			float3 viewDir;
			float4 screenPos;

			INTERNAL_DATA
        };

		void vert(inout appdata_full v)
		{
			// 1 Wave
			v.vertex.z += cos(abs(v.texcoord.x * 2 - 1) * _WaveHeight + _Time.y * _WaveTime);
			// 2 Wave
			v.vertex.z += cos(abs(v.texcoord.y * 2 - 1) * _WaveHeight + _Time.y * _WaveTime);
		}
		
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
					
				// Normal
			fixed3 n1 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + (_Time.y * _TimeCount1)));
			fixed3 n2 = UnpackNormal(tex2D(_BumpMap, (IN.uv_BumpMap - (_Time.x * _TimeCount2))*2));
			n1 = float3(n1.r * _BumpPower1, n1.g * _BumpPower1, n1.b);
			n2 = float3(n2.r * _BumpPower2, n2.g * _BumpPower2, n2.b);
			float3 n = (n1 + n2) *0.5;
			o.Normal = normalize(n);

				// Rim(Fresnel)
			float rim = saturate(dot(o.Normal, IN.viewDir));
			rim = 1 - rim;
			rim = pow(rim, 3); 

				// CubeMap Texture
			float3 refltex = texCUBE(_Cube, WorldReflectionVector(IN, o.Normal));
			
				// ScreenPosition (Autographic)
			float2 screenPos = IN.screenPos.xyz / IN.screenPos.w;
		
				// GrapPass Texture 
			float3 grabtex = tex2D(_GrabTexture, screenPos.xy + o.Normal * _WaveDistortion);

				// Result (투명은 grabtex, 불투명은 refltex)
			o.Emission = saturate(lerp(grabtex, refltex, rim));

        }

		float4 LightingTest(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
		{

				// Specular
			float3 H = normalize(lightDir + viewDir);
			float spec = saturate(dot(s.Normal, H));
			spec = pow(spec, _RimPow);
			float3 specColor = spec * 0.3	 * _RimCol.rgb;

				// Reflection
			return float4(specColor, 1 + spec);

		}

		
        ENDCG
    }
    FallBack "Diffuse"
}
