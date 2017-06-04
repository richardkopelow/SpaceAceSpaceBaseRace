﻿Shader "Unlit/StarShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FadeMap ("Fade Map", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _FadeMap;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = mul(unity_ObjectToWorld, v.vertex)/6;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float2 worldPos = i.uv;
				i.uv = fmod(i.uv,1);
				if (i.uv.x < 0)
				{
					i.uv.x += 1;
				}
				if (i.uv.y < 0)
				{
					i.uv.y += 1;
				}
				fixed4 col = tex2D(_MainTex, i.uv) * abs(tex2D(_FadeMap, i.uv).r - ((_SinTime.w)/2*0.6+0.7) + sin(worldPos.x+_Time.y)/3);
				return col;
			}
			ENDCG
		}
	}
}
