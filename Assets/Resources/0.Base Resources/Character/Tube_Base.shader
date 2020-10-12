Shader "Custom/rimLight2"
{
	Properties
	{
		_MainTex("Albedo RGB",2D) = "white" {}

		[HDR]
		_RimColor("Rim Color", color) = (1, 1, 1, 1)
		_RimPower("Rim Power(Inside)", float) = 1
		_Blinking("Rim Power(Outside)", float) = 1

		_BumpMap("Normal Map", 2D) = "bump" {}
		_BumpPower("Bump Power", float) = 1

		[HDR]
		_SpecCol("Specular Color", Color) = (1,1,1,1)
		_SpecPow("Specular Power", float) = 50

		[HDR]
		_FspecCol("Fake Specular Color", Color) = (1,1,1,1)
		_FspecPow("Fake Specular Power", float) = 50

	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" "IgnoreProjector" = "True" "Opaque" = "Transparent" }
		LOD 200

		Pass{
		zwrite on
		ColorMask 0
			}

		UsePass	"Transparent/Diffuse/FORWARD"

		CGPROGRAM
#pragma surface surf  Test alpha:blend
#pragma target 3.0

	sampler2D _MainTex;
	sampler2D _BumpMap;

	struct Input
	{
		float2 uv_MainTex;
		float2 uv_BumpMap;

		float3 viewDir;
		float3 lightDir;
	};

	float4 _RimColor;
	float4 _SpecCol;
	float4 _FspecCol;

	float _RimPower, _RimBold;
	float _BumpPower, _Blinking;
	float _SpecPow, _CeilPower, _FspecPow;

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		
		// Normal
		fixed3 n = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		n = float3(n.r * _BumpPower, n.g * _BumpPower, n.b);
		o.Normal = normalize(n);

		// Rim Light
		float rim = dot(normalize(o.Normal), normalize(IN.viewDir));
		rim = pow(1 - rim, _RimPower);


		o.Emission = _RimColor.rgb;
		o.Albedo = c.rgb * 1.2;
		o.Alpha = saturate((rim) * _Blinking);

	}

	float4 LightingTest(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
	{
		float NdotL = saturate(dot(s.Normal, viewDir));

		// Specular
		float3 h = normalize(viewDir + lightDir); // 뷰 벡터와 라이트 벡터를 더하고 Normalize
		float spec = saturate(dot(s.Normal, h)); // Normalize한 중간 벡터를 노말 벡터와 함께 Dot 연산
		spec = saturate(pow(spec, _SpecPow));
		

		// Fake Specular
		float3 fspec = saturate(dot(viewDir, s.Normal));
		fspec = saturate(pow(fspec, _FspecPow) * _FspecCol.rgb);

		 //Final
		float4 finalColor;
		finalColor.rgb = s.Albedo * (NdotL + spec * _SpecCol.rgb + fspec);
		finalColor.a = s.Alpha;

		return finalColor;
	}


	ENDCG
	}
		FallBack "Legacy Shaders/Transparent/Diffuse/FORWARD"
}

