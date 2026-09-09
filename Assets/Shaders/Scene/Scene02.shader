Shader "Custom/Test/Scene02"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}

		_BumpScale("Scale", Float) = 1.0
		[Normal] _BumpMap("Normal Map", 2D) = "bump" {}

		_GlossMapScale("Smoothness Scale", Range(0.0, 1.0)) = 1.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}

		// Blending state
		[HideInInspector] _Mode("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend("__src", Float) = 1.0
		[HideInInspector] _DstBlend("__dst", Float) = 0.0
		[HideInInspector] _ZWrite("__zw", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		Blend[_SrcBlend][_DstBlend]
		ZWrite[_ZWrite]

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

		half _BumpScale;
		sampler2D _BumpMap;

		half _GlossMapScale;
		sampler2D _MetallicGlossMap;

		half _OcclusionStrength;
		sampler2D _OcclusionMap;

		fixed4 _EmissionColor;
		sampler2D _EmissionMap;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormalWithScale(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
            // Metallic and smoothness come from slider variables
			half2 mg = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			mg.g *= _GlossMapScale;
            o.Metallic = mg.g;
            o.Smoothness = mg.r;
			o.Occlusion = tex2D(_OcclusionMap, IN.uv_MainTex) * _OcclusionStrength;
			o.Emission = tex2D(_EmissionMap, IN.uv_MainTex).rgb * _EmissionColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
