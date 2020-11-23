Shader "water/Waterfall_zwrite_x" {
	Properties{

		_MainTex("폭포 텍스쳐", 2D) = "white" {}
		_Mainspeed("서브 텍스쳐 스피드", Range(0,20)) = 20
		_NainTex("폭포 서브 텍스쳐", 2D) = "white" {}
		_subspeed("서브 텍스쳐 스피드x", Range(0,20)) = 20
		_subspeedy("서브 텍스쳐 스피드y", Range(0,20)) = 20

		_SubTex("서브 텍스쳐", 2D) = "white" {}
		_BumpMap("NormalMap",2D) = "bump" {}
		_Color("색상", color) = (1,1,1,1)
		
			_SPecPower("스펙큘러 강도", Range(50,300)) = 150
		[HDR]_SPecColor("스펙큘러 색상", color) = (1,1,1,1)
		
		_WaveRange("x물결 범위", Range(0,50)) = 50
		_WaveRange1("y물결 범위", Range(0,50)) = 50
		_wavePower("x물결 높이", Range(0.01,0.1)) = 0.05
		_wavePower1("y물결 높이", Range(0.01,0.1)) = 0.05
		_wavespeed("x물결 스피드", Range(0,20)) = 20
		_wavespeed1("y물결 스피드", Range(0,20)) = 20
	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			blend SrcAlpha OneMinusSrcAlpha 
			

			CGPROGRAM
			#pragma surface surf WaterSpecular keepalpha vertex:vert 

			sampler2D _BumpMap;
			sampler2D _MainTex;
			sampler2D _NainTex;
			sampler2D _SubTex;
	

			float _SPecPower;
			float4 _SPecColor;
			float4 _Color;

			float _wavePower;
			float _wavePower1;
			float _WaveRange;
			float _WaveRange1;
			float _wavespeed;
			float _wavespeed1;
			float _Mainspeed;
			float _subspeed;
			float _subspeedy;


			void vert(inout appdata_full v) {

				float Xwave = cos(abs(v.texcoord.x * 2 - 1) * _WaveRange + (_Time.y*_wavespeed)) * _wavePower;
				float Ywave = sin(abs(v.texcoord.x * 2 - 1) * _WaveRange + (_Time.y*_wavespeed)) * _wavePower;

				float Zwave = cos(abs(v.texcoord.y * 2 - 1) * _WaveRange1 + (_Time.x*_wavespeed1)) * _wavePower1;
				float Wwave = sin(abs(v.texcoord.y * 2 - 1) * _WaveRange1 + (_Time.x*_wavespeed1)) * _wavePower1;

				v.vertex.z += (Xwave + Ywave + Zwave + Wwave) *0.25;
			}

			struct Input {
				float2 uv_BumpMap;
				float3 worldRefl;
				float2 uv_MainTex;
				float2 uv_NainTex;
				float2 uv_SubTex;
				float3 viewDir;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				
				float4 waterfalltex = tex2D(_MainTex ,float2(IN.uv_MainTex.x, IN.uv_MainTex.y + _Time.y * _Mainspeed)) * _Color;
				float4 cuttex = tex2D(_NainTex,float2(IN.uv_NainTex.x + _Time.y * _subspeed, IN.uv_NainTex.y + _Time.y * _subspeedy)) * _Color;
				float4 subtex = tex2D(_SubTex, IN.uv_SubTex) * _Color;
				

				float3 normal1 = UnpackNormal(tex2D(_BumpMap, float2 (IN.uv_BumpMap.x, IN.uv_BumpMap.y + _Time.y*1)));
			
				o.Normal = normal1;

				float rim = saturate(dot(o.Normal, IN.viewDir));
				rim = pow(1 - rim,2);

				//o.Emission = lerp(waterfalltex, subtex, 0.5) + rim;
				o.Albedo = subtex + rim;
				


				o.Alpha = (cuttex.a + waterfalltex.a) *0.5;

				if (o.Alpha > 0.7)
				{
					o.Alpha = 1;
				}
				else
				{
					o.Alpha = 0;
				}

				
			}

			float4 LightingWaterSpecular(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
			{
				float3 H = normalize(lightDir + viewDir);
				float spec = saturate(dot(H, s.Normal));
				spec = pow(spec, _SPecPower);

				float4 finalColor;
				finalColor.rgb = spec * _SPecColor.rgb;
				finalColor.a = s.Alpha + spec;

				return finalColor;
			}
			ENDCG
		}
			FallBack "Diffuse"
}