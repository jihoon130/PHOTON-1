Shader "Custom/2"
{
    Properties
    {

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal", 2D) = "bump" {}

		_RimColor("Rim Color" , Color) = (1,1,1,1)
		_RimPow("Rim Power" , Range(0,10)) = 5

		_SpecCol("Specular Color" , Color) = (1,1,1,1)
		_SpecPow("Specular Power" , Range(0,200)) = 100

		_LinePower("Outline Power" , Range(0,10)) = 1
		_LineColor("Line Color" , Color) = (1,1,1,1)

		
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			cull front

			CGPROGRAM
			#pragma surface surf eee vertex:vert noshadow
			#pragma target 3.0

			sampler2D _MainTex;

			float _LinePower;
			float4 _LineColor;

			struct Input
			{
				float2 uv_MainTex;
			};

			void vert(inout appdata_full v)
			{
				v.vertex.xyz = v.vertex.xyz + v.normal*(0.01 * _LinePower);
			}


			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb * _LineColor;
				o.Alpha = c.a;
			}

			float4 Lightingeee(SurfaceOutput s , float3 lightDir, float atten)
			{
				float4 sdf;

				sdf.rgb = s.Albedo;
				sdf.a = s.Alpha;
				return sdf;
			}
			ENDCG

			cull back
			CGPROGRAM

			#pragma surface surf mylight noambient

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _BumpMap;

			fixed4 _RimColor;
			float _RimPow;
			fixed4 _SpecCol;
			float _SpecPow;
			float _Gloss;

			struct Input
			{
				float2 uv_MainTex;
			};


			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				fixed4 n = tex2D(_BumpMap, IN.uv_MainTex);
				o.Normal = UnpackNormal(n);

				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}

			float4 Lightingmylight(SurfaceOutput s, float3 lightDir, float3 viewDir , float atten)
			{
			float ndotl = dot(s.Normal , lightDir) * 0.5 + 0.5;

			if (ndotl > 0.5)
			{
			ndotl = 1;
			}
			else
			{
			ndotl = 0.2;
			}


			float rim = dot(viewDir , s.Normal);
			rim = 1 - rim;
			rim = pow(rim , _RimPow);
			float3 rimcol = rim * _RimColor.rgb;

			float3 h = normalize(viewDir + lightDir);
			float spec = saturate(dot(s.Normal , h));
			spec = pow(spec , _SpecPow * 100);
			spec = ceil(spec * 3) / 3;
			float3 speccol = spec * _SpecCol.rgb;

			float4 finalColor;
			finalColor.rgb = s.Albedo * ndotl + speccol + rimcol;
			finalColor.a = s.Alpha;

			return finalColor;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
