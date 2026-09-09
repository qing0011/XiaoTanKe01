Shader "Custom/Transparent"
{
	Properties
	{
		_MainTex ("Particle Texture", 2D) = "white" {}
		_TpRimColor ("Rim Color", Color) = (1,1,1,1)
		_TpInnerColor ("Inner Color", Color) = (0,0,0,0.5)
		_TpInnerColorPower ("Inner Color Power", Range(0.0,1.0)) = 0.5
		_TpRimPower ("Rim Power", Range(0.0,5.0)) = 2.5
		_TpAlphaPower ("Alpha Rim Power", Range(0.0,8.0)) = 4.0
		_TpAllPower ("All Power", Range(0.0, 1)) = 1.0
		_TpInnerAlphaBase ("Inner Alpha Base", Range(0.3, 1.0)) = 0.3
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
 
		Pass
		{
			// 开启深度写入
			ZWrite On
			// 设置颜色通道的写掩码，0为不写入任何颜色
			ColorMask 0
		}
 
		CGPROGRAM
		#pragma surface surf Lambert alpha
	
		struct Input
		{
			float3 viewDir;
			float2 uv_MainTex;
			INTERNAL_DATA
		};
	
		sampler2D _MainTex;
		float4 _TpRimColor;
		float _TpRimPower;
		float _TpAlphaPower;
		float _AlphaMin;
		float _TpInnerColorPower;
		float _TpAllPower;
		float4 _TpInnerColor;
		float _TpInnerAlphaBase;
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			float4 col = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = col.rgb;
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = _TpRimColor.rgb * pow (rim, _TpRimPower)*_TpAllPower+(_TpInnerColor.rgb*2*_TpInnerColorPower);
			o.Alpha = (_TpInnerAlphaBase + (pow (rim, _TpAlphaPower))*_TpAllPower) * col.a;
		}
		ENDCG
	}
Fallback "VertexLit"
} 
