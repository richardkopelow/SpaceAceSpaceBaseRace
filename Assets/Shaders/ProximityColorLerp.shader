Shader "Unlit/ProximityColorLerp"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_FarColor("Far Color", Color) = (0,0,1,1)
		_NearColor("Near Color", Color) = (1,0,0,1)
		_FarDistance("Far Distance", float) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

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

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float4 worldPos : POSITION1;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _FarColor;
				fixed4 _NearColor;
				float4 _TargetPosition;
				float _FarDistance;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);
					float dist = distance((0,0,0,0), i.worldPos);

					if (dist > _FarDistance)
					{
						return col * _FarColor;
					}
					col = col * lerp(_NearColor, _FarColor, dist / _FarDistance);
					return col;
				}
				ENDCG
			}
		}
}
