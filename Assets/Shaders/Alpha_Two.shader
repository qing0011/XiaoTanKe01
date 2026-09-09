Shader "Cathy/Alpha_Two" 
{
	Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_AddColor ("Add Color", Range(0.5,1.5)) = 1
	}
	
	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		LOD 200
		Cull OFF
		
	CGPROGRAM
	#pragma surface surf Lambert alphatest:_Cutoff
	
	sampler2D _MainTex;
	fixed4 _Color;
	float _AddColor;
	
	struct Input {
		float2 uv_MainTex;
	};
	
	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb * _AddColor;
		o.Alpha = c.a;
	}
	ENDCG
	}

}