Shader "Custom/Water2jp"
{
	Properties
	{
		_BumpMap("노말", 2D) = "bump" {}
		_BumpPower1("노말(1) 세기", float) = 0.4	
		_BumpPower2("노말(2) 세기", float) = 0.3

		_TimeCount1("노말(1) 흐름", float) = 0.03
		_TimeCount2("노말(2) 흐름", float) = 0.5

		_Cube("큐브맵", cube) = ""{}

		_WaveHeight("웨이브 높이", float) = 500
		_WaveTime("웨이브 속도", float) = 1.5

		_WaveDistortion("물 왜곡 세기", float) = 0.02

		[HDR]
		_RimCol("물 위 스펙큘러 색", Color) = (1,1,1,1)
		_RimPow("물 위 스펙큘러 세기", float) = 1500

		[HDR]
		_WaveCol("물보라 색", Color) = (1,1,1,1)
		_WaveArea("물보라 영역", float) = 1
		_WavePow("물보라 세기", float) = 1

		_DeepWaterColor ("깊은 물 칼라", color) = (0,0,1,1)
		_WaterDepth ("물깊이", Range(0,3)) = 0.5

	}
		SubShader
	{

		//Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Tags{ "RenderType" = "Opqaue"  "Queue" = "Geometry +1" }
		
		GrabPass{}

		LOD 200
//		zwrite off

		CGPROGRAM
#pragma surface surf Test vertex:vert noambient
#pragma target 3.0

	sampler2D _MainTex;
	sampler2D _BumpMap;
	sampler2D _GrabTexture;
	sampler2D _CameraDepthTexture;
	samplerCUBE _Cube;

	float _BumpPower1, _BumpPower2;
	float _TimeCount1, _TimeCount2;
	float _WaveHeight, _WaveTime,_WaveDistortion, _WaterDepth;

	float4 _RimCol, _WaveCol, _DeepWaterColor;
	float _RimPow, _WaveArea, _WavePow;


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

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

		// Normal
		fixed3 n1 = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap + (_Time.y * _TimeCount1)));
		fixed3 n2 = UnpackNormal(tex2D(_BumpMap, (IN.uv_BumpMap - (_Time.x * _TimeCount2)) * 2));
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
		float3 screenPosNorm = (IN.screenPos.xyz / (IN.screenPos.w));
		//float4 ScreenPos = float4(IN.screenPos.xyz, IN.screenPos.w );
		//float3 screenPosNorm = ScreenPos / ScreenPos.w;

		// GrapPass Texture 
		float4 grabtex = tex2D(_GrabTexture, screenPosNorm.xy + o.Normal.xy * _WaveDistortion);
		
		// Screen, Water Depth
		float screenDepth = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)))); // 깊이 텍스쳐를 스크린 좌표계를 이용해서 깊이값을 뽑아내고 그걸 리니어로 변환
		float planeDepth = (LinearEyeDepth(screenPosNorm.z));
		float waterDepth =  saturate((screenDepth - planeDepth) * _WaterDepth);

		// Result (투명은 grabtex, 불투명은 refltex)
		float3 grablerp = lerp(grabtex.rgb, _DeepWaterColor.rgb, waterDepth);
		o.Emission = (lerp(grablerp, refltex, rim) );// +((saturate(pow((1 - waterDepth), _WaveArea)) * _WavePow) * _WaveCol);
		o.Alpha = 1;
		//o.Emission = waterDepth;
	}


	float4 LightingTest(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
	{
		// Specular
		float3 H = normalize(lightDir + viewDir);
		float spec = saturate(dot(s.Normal, H));
		spec = pow(spec, _RimPow);
		float3 specColor = spec * 0.3	 * _RimCol.rgb * atten;

		// Reflection
		return float4(specColor, 1 + spec);
	}

	ENDCG
	}
		FallBack "Transparent"
}
