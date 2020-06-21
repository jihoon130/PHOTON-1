Shader "Custom/Lambert"
{
    Properties
    {
	[HDR] _TexCol("Rim Color", Color) = (1,1,1,1)
		 _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal", 2D) = "bump" {}
		_BumpPow("Normal Power", float) = 1
	[HDR] _RimCol("Rim Color", Color) = (1,1,1,1)
		_RimPow("Rim Power", float) = 2
	[HDR]	_SpecCol("Specular Color", Color) = (1,1,1,1)
		_SpecPow("Specular Size", float) = 50
	[HDR]	_FspecCol("Fake Specular Color", Color) = (1,1,1,1)
		_FspecPow("Fake Specular Size", float) = 30
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Test fullforwardshadows //noambient
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
		float4 _TexCol;
		float4 _RimCol;
		float4 _SpecCol;
		float4 _FspecCol;
		float _RimPow;
		float _BumpPow;
		float _SpecPow;
		float _FspecPow;

        struct Input
        {
            float2 uv_MainTex;
			float3 viewDir;
			float3 lightDir;
        };
		
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			// Normal
            fixed3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			n = float3(n.r * _BumpPow, n.g * _BumpPow, n.b); // Normal 세기 더 강하게 
			o.Normal = normalize(n);

			// Rim Light
			float rim = saturate(pow((1 - dot(o.Normal, IN.viewDir)), _RimPow));

			// Final
			//o.Emission = (c.a * 0.5) + rim * _RimCol.rgb);
            o.Albedo = c.rgb * _TexCol.rgb + rim * _RimCol.rgb;
			o.Alpha = c.a;
        }
		

		float4 LightingTest(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {

			float ndotL = saturate(dot(s.Normal, lightDir));

			// Specular
			float3 h = normalize(viewDir + lightDir); // 뷰 벡터와 라이트 벡터를 더하고 Normalize
			float spec = saturate(dot(s.Normal, h)); // Normalize한 중간 벡터를 노말 벡터와 함께 Dot 연산
			spec = saturate(pow(spec, _SpecPow))* _SpecCol.rgb;

			// Fake Specular
			float3 fspec = saturate(dot(viewDir, s.Normal));
			fspec = saturate(pow(fspec, _FspecPow) * _FspecCol.rgb) * 0.5;

			// Fake Shadow? 사실 그림자 넣고 싶었어요!
			float shade = saturate(dot(viewDir, s.Normal));
			shade = pow(shade, 5);

			

			// Final				
			float4 finalColor;
			finalColor.rgb = s.Albedo * (ndotL + spec +  fspec)* atten;//* shade * atten;
			finalColor.a = s.Alpha;

			return finalColor;
		}

        ENDCG
    }
    FallBack "Diffuse"
}

