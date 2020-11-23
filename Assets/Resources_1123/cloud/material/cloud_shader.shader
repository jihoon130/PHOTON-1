Shader "Custom/cloud_shader"
{
    Properties
	{
		_Color("color",color)=(1,1,1,1)
	   _MainTex("Albedo(RGB)",2D) = "white"{}
	_Cutoff("Alpha cutoff",Range(0,1))=0.5
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 200

        CGPROGRAM
      
        #pragma surface surf NoLighting alphatest:_Cutoff

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };
 

        void surf (Input IN, inout SurfaceOutput o)
        {

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
            o.Alpha = c.a;
        }

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

        ENDCG
    }
    FallBack "Transparent/Cutout/Diffuse"
}
