Shader "Custom/chowderShader" {
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
			};
			sampler2D _MainTex;

			v2f vert(appdata v)
			{
				v2f o;
 				o.vertex=UnityObjectToClipPos(v.vertex);		
				return o;
			}


			float4 frag(v2f i):SV_TARGET
			{
	
				return tex2D(_MainTex,((i.vertex.xy*10)/_ScreenParams.xy));
			}
			ENDCG
		}
		
	}
	FallBack "Diffuse"
}
