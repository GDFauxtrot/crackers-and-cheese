Shader "Custom/NewPlayerShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_internalColor("color for overlap",color)=(1,1,1,1)
		_speed("speed of scrolling",Range(0,100))=1.0
		_size("size of pattern",Float )=0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
		};

		
		fixed4 _Color;
		fixed4 _internalColor;
		float _speed,_size;
		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			
			o.Albedo = _Color.rgb;
			// Metallic and smoothness come from slider variables
		 	float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
          	screenUV *= float2(8,6);
			screenUV+=_Time.x*_speed;
			screenUV*=_size;
			o.Emission=tex2D(_MainTex,screenUV).rgb*_internalColor;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
