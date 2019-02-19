Shader "Unlit/InterFrameShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		ZTest Always
		ZWrite Off

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
			};
			
			sampler2D _Overlay;
			sampler2D _MainTex;
			sampler2D _KeyFrame;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				v.uv = float2(1.0 - v.uv.x, 1.0 - v.uv.y);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				clip(tex2D(_Overlay, i.uv).a - 0.5);

				// sample the texture
				float3 col = tex2D(_KeyFrame, i.uv).rgb + (tex2D(_MainTex, i.uv).rgb - 0.5) * 2.0;
				return float4(col.r, col.g, col.b, 0.0);
			}
			ENDCG
		}
	}
}
