Shader "Custom/Character_Shader_0713"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
		[HDR]
		_TopColor("Top Color", Color) = (0,0,0,0)
		[HDR]	
		_UnderColor("Under Color", Color) = (0.3,0.1,0.15,1)

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal", 2D) = "bump"{}

		/*
		_RimColor("Rim Color",Color) = (1,1,1,1)
		_RimArea("Rim Area", float) = 1
		_RimPower("Rim Power", float) = 1
		*/

		[HDR]
		_DamagedColor("Damaged Color",Color) = (5,0,0,1)
		[Toggle]
		_Damaged("Damaged(0~1)",float) = 0

		[HDR]
		_RespawnColor("Respawn Color", Color) = (5,5,0,1)
		[Toggle]
		_Respawn("Respawn(0~1)", float) = 0

		_Glossiness("Smoothness", Range(0,1)) = 0.6


  
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

		fixed4 _DamagedColor, _RespawnColor;
		float _Damaged, _Respawn;
		
		
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			// Texture and Normal
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;	
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
 
			// World Normal
			float3 worldNor = WorldNormalVector(IN, o.Normal);

			// Rim Light
			float rim = dot(IN.viewDir, o.Normal);
			rim = pow(1 - rim, _RimArea);
			rim *= saturate(worldNor.y) * _RimPower;

			// Damaged Light
			float dma = dot(IN.viewDir, o.Normal);
			dma = pow(1 - dma, 2);
			dma *= dma * _Damaged;
			dma = saturate(dma);

			// Respawn Light
			float res = dot(IN.viewDir, o.Normal);
			res = pow(1 - res, 2);
			res *= res * _Respawn;
			res = saturate(res);

			// Smoothness			
			o.Smoothness = _Glossiness;

			// Upper, Lower Light
			float3 lowerColor = (1 - saturate(o.Normal.y)) * _UnderColor * o.Albedo;
			float3 upperColor = (saturate(o.Normal.y)) * _TopColor * o.Albedo;



			// Final
			o.Emission = lowerColor + upperColor + (dma * _DamagedColor) + (res * _RespawnColor);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
