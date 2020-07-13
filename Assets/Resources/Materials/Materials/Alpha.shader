Shader "Custom/alpha"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_AlphaTube ("Opacity", Range(0,1)) = 0.5
	}
		SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200

		zwrite on
		ColorMask 0 // Pass


		CGPROGRAM
#pragma surface surf Lambert 


		sampler2D _MainTex;

	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;

		o.Alpha = c.a;
	}
	ENDCG



		CGPROGRAM
#pragma surface surf Lambert alpha:blend


		sampler2D _MainTex;
	float _AlphaTube;	

	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;

		o.Alpha = _AlphaTube;
	}
	ENDCG
	}
		FallBack "Transparent"
}