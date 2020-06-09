Shader "wooshader/woo_pencil"
{
    Properties
    {
        _MainTex ("메인 텍스쳐", 2D) = "white" {}
		_BumpTex ("노말맵", 2D) = "bump" {}

		[HDR] _RimColor ("림 라이트의 색상", color) = (1,1,1,1)
		_Rimpower ("림 라이트의 강도 조절", Range(10,1)) = 1  
		_Rimspeed ("깜빡임 속도", Range(0,20)) = 1
		

    }
    SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		CGPROGRAM
		#pragma surface surf Lambert alpha:blend
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _BumpTex;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_BumpTex;
			float3 viewDir;
			float3 worldPos;
		};

		float4 _RimColor;
		float _Rimpower;
		float _Rimspeed;
		


		void surf(Input IN, inout SurfaceOutput o)
		{
			float4 maintex = tex2D (_MainTex, IN.uv_MainTex);
			float3 bumptex = UnpackNormal (tex2D(_BumpTex, IN.uv_BumpTex));
			o.Normal = bumptex;
			

			float rim = saturate(dot(o.Normal, IN.viewDir));
			rim = pow(rim, _Rimpower);
			
			o.Emission = _RimColor.rgb * maintex;
			o.Alpha = rim * (sin(_Time.y * _Rimspeed)* -0.5+0.5);

		}
        ENDCG
    }
    FallBack "Diffuse"
}
