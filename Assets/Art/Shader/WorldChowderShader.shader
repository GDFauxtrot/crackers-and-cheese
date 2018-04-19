Shader "Unlit/WorldChowderShader"
{
	Properties {
		_MainTex ("ScreenSpace Texture", 2D) = "white" {}
		_Color("playerColor",color)=(1,1,1,1)
		_Xmovement("X movement",float)=1.0	
		_Ymovement("Y movement",float)=1.0	

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
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
				float4 worldPos : TEXCOORD0;
				float4 screenPos: TEXCOORD1;

			};
			sampler2D _MainTex;
			float4 _Color;
		

			v2f vert(appdata v)
			{
				v2f o;
				o.worldPos = mul (unity_ObjectToWorld, v.vertex);
 				o.vertex=UnityObjectToClipPos(v.vertex);
				o.screenPos=ComputeScreenPos(v.vertex);		
				return o;
			}

			float _Xmovement;
			float _Ymovement;
			float4 frag(v2f i):SV_TARGET
			{		
				//i.worldPos*=.9;
				return tex2D(_MainTex,float2(i.vertex.x+_Time.w*10*_Xmovement,i.vertex.y+_Time.w*10*_Ymovement)/(_ScreenParams.xy))+_Color;
			}
			ENDCG
		}
		
	}
	FallBack "Diffuse"
}
