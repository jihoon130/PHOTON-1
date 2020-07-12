Shader "Custom/Shader"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
		[HDR]
		_TopColor("Top Color", Color) = (1,1,1,1)
		[HDR]	
		_UnderColor("Under Color", Color) = (1,1,1,1)

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal", 2D) = "bump"{}

		_RimColor("Rim Color",Color) = (1,1,1,1)
		_RimArea("Rim Area", float) = 1
		_RimPower("Rim Power", float) = 1

		_Glossiness("Smoothness", Range(0,1)) = 0.5


  
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
        };

        half _Glossiness, _RimArea, _RimPower;
        fixed4 _Color, _UnderColor, _TopColor, _RimColor;
		
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;	
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
 
			float3 worldNor = WorldNormalVector(IN, o.Normal);

			float rim = dot(IN.viewDir, o.Normal);
			rim = pow(1 - rim, _RimArea);
			rim *= saturate(worldNor.y) * _RimPower;
			
			o.Smoothness = _Glossiness;

			float3 lowerColor = (1 - saturate(o.Normal.y)) * _UnderColor * o.Albedo;
			float3 upperColor = (saturate(o.Normal.y)) * _TopColor * o.Albedo;

		//	o.Emission = upperColor;
			o.Emission = lowerColor + upperColor +(rim* _RimColor);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
