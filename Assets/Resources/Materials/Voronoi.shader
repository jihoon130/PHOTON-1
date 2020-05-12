Shader "Custom/Voronoi"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0,1)
		_Color2("Color2", Color) = (0,0,0,1)
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	fixed4 _Color;
	fixed4 _Color2;

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);

		o.uv = v.uv;
		return o;
	}
	float2 random2(float2 p)
	{
		return frac(sin(float2(dot(p,float2(117.12,341.7)),dot(p,float2(269.5,123.3))))*43458.5453);
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = fixed4(0,0,0,1);
	float2 uv = i.uv;
	uv *= 40.0;
	float2 iuv = floor(uv);
	float2 fuv = frac(uv);
	float minDist = 3.0;
	for (int y = -1; y <= 1; y++)
	{
		for (int x = -1; x <= 1; x++)
		{
			float2 neighbor = float2(float(x), float(y));
			float2 pointv = random2(iuv + neighbor);
			pointv = 0.5 + 0.5*sin(_Time.z*0.25 + 6.2236*pointv);
			float2 diff = neighbor + pointv - fuv;
			float dist = length(diff);

			minDist = min(minDist, dist);
		}
	}

	col.rgb += (((minDist * minDist)* 0.4 *_Color) + _Color2);

	return col * 1.2;
	}
		ENDCG
	}
	}
}
