Shader "Custom/Skybox"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MainTex2 ("Albedo (RGB)", 2D) = "white" {}

		_U("U", float) = 1
		_V("V", float) = 1

		_TimCon("Time", float) = 1
		_TimCon2("Time", float) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 200

//		zwrite off

        CGPROGRAM
           #pragma surface surf Standard alpha:blend //noambient
		   #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MainTex2;

		float4 _Color;
		float _U;
		float _V;
		float _TimCon;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MainTex2;
        };

		
        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 d = tex2D (_MainTex2, float2(IN.uv_MainTex2.x + _U + _Time.y * _TimCon, IN.uv_MainTex2.y + _V));

	      o.Emission = (d.rgb) * _Color;

	   	o.Alpha = saturate(d.a);

        }
        ENDCG
    }
    FallBack "Diffuse"
}
