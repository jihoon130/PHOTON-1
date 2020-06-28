Shader "Custom/Shad_AddtiveHDR"
{
    Properties
    {
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		blend SrcAlpha One
		cull off

        CGPROGRAM
        #pragma surface surf Lambert alpha:blend

        #pragma target 3.0

        sampler2D _MainTex;

		float4 _Color;

        struct Input
        {
            float2 uv_MainTex;
			float4 color : COLOR;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Emission = c.rgb * IN.color.rgb;
            o.Alpha = c.a * IN.color.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
