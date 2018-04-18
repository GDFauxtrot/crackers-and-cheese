Shader "Unlit/WorldChowderShader"
{
	Properties {
		_MainTex ("ScreenSpace Texture", 2D) = "white" {}
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		pass{
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			struct appdata
			{
				float4 vertex:POSITION;
			};
			struct v2f
			{
				float4 vertex:SV_POSITION;
				float3 worldPos : TEXCOORD0;

			};
			sampler2D _MainTex;

			v2f vert(appdata v)
			{
				v2f o;
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);
 				o.vertex=UnityObjectToClipPos(v.vertex);		
				return o;
			}


			float4 frag(v2f i):SV_TARGET
			{
	
				return tex2D(_MainTex,(i.worldPos.xy*i.worldPos.z));
			}
			ENDCG
		}
		
	}
	FallBack "Diffuse"
}
