Shader "Custom/Terrain01"
{
    Properties
    {
		_MaskTex("Mask (RGBA)", 2D) = "red" {}

        _MainTexR ("Albedo R (RGB)", 2D) = "white" {}
		_ColorR("Color R", Color) = (1,1,1,1)
		_MetallicGlossMapR("MetallicGlossMap R (RG)", 2D) = "grey" {}
		_SmoothnessR("_Smoothness R", Range(0, 1)) = 1
		[Normal] _BumpMapR("Normal Map R", 2D) = "bump" {}
		_BumpMapScaleR("BumpMap R Scale", float) = 1

        _MainTexG ("Albedo G (RGB)", 2D) = "white" {}
		_ColorG("Color G", Color) = (1,1,1,1)
		_MetallicGlossMapG("MetallicGlossMap G (RG)", 2D) = "grey" {}
		_SmoothnessG("_Smoothness G", Range(0, 1)) = 1
		[Normal] _BumpMapG("Normal Map G", 2D) = "bump" {}
		_BumpMapScaleG("BumpMap G Scale", float) = 1

        _MainTexB ("Albedo B (RGB)", 2D) = "white" {}
		_ColorB("Color B", Color) = (1,1,1,1)
		_MetallicGlossMapB("MetallicGlossMap B (RG)", 2D) = "grey" {}
		_SmoothnessB("_Smoothness B", Range(0, 1)) = 1
		[Normal] _BumpMapB("Normal Map B", 2D) = "bump" {}
		_BumpMapScaleB("BumpMap B Scale", float) = 1

        _MainTexA ("Albedo A (RGB)", 2D) = "white" {}
		_ColorA("Color A", Color) = (1,1,1,1)
		_MetallicGlossMapA("MetallicGlossMap A (RG)", 2D) = "grey" {}
		_SmoothnessA("_Smoothness A", Range(0, 1)) = 1
		[Normal] _BumpMapA("Normal Map A", 2D) = "bump" {}
		_BumpMapScaleA("BumpMap A Scale", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#include "Lighting.cginc"
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

		sampler2D _MaskTex;

		sampler2D _MainTexR;
		float4 _MainTexR_ST;
		half3 _ColorR;

		sampler2D _MainTexG;
		float4 _MainTexG_ST;
		half3 _ColorG;

		sampler2D _MainTexB;
		float4 _MainTexB_ST;
		half3 _ColorB;

		sampler2D _MainTexA;
		float4 _MainTexA_ST;
		half3 _ColorA;

		sampler2D _MetallicGlossMapR;
		float4 _MetallicGlossMapR_ST;
		half _SmoothnessR;

		sampler2D _MetallicGlossMapG;
		float4 _MetallicGlossMapG_ST;
		half _SmoothnessG;

		sampler2D _MetallicGlossMapB;
		float4 _MetallicGlossMapB_ST;
		half _SmoothnessB;

		sampler2D _MetallicGlossMapA;
		float4 _MetallicGlossMapA_ST;
		half _SmoothnessA;

		sampler2D _BumpMapR;
		float4 _BumpMapR_ST;
		half _BumpMapScaleR;

		sampler2D _BumpMapG;
		float4 _BumpMapG_ST;
		half _BumpMapScaleG;

		sampler2D _BumpMapB;
		float4 _BumpMapB_ST;
		half _BumpMapScaleB;

		sampler2D _BumpMapA;
		float4 _BumpMapA_ST;
		half _BumpMapScaleA;

        struct Input
        {
            float2 uv_MaskTex;
        };

        half _Glossiness;
        half _Metallic;
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
            half4 mask = tex2D (_MaskTex, IN.uv_MaskTex);
			//o.Albedo = mask.rgb;
			//return;
			half3 albedoR = tex2D(_MainTexR, TRANSFORM_TEX(IN.uv_MaskTex, _MainTexR)).rgb * _ColorR;
			half3 albedoG = tex2D(_MainTexG, TRANSFORM_TEX(IN.uv_MaskTex, _MainTexG)).rgb * _ColorG;
			half3 albedoB = tex2D(_MainTexB, TRANSFORM_TEX(IN.uv_MaskTex, _MainTexB)).rgb * _ColorB;
			half3 albedoA = tex2D(_MainTexA, TRANSFORM_TEX(IN.uv_MaskTex, _MainTexA)).rgb * _ColorA;
            o.Albedo = albedoR * mask.r + albedoG * mask.g + albedoB * mask.b + albedoA * mask.a;
			//return;

			half3 MSR = tex2D(_MetallicGlossMapR, TRANSFORM_TEX(IN.uv_MaskTex, _MetallicGlossMapR)).rgb;
			half3 MSG = tex2D(_MetallicGlossMapG, TRANSFORM_TEX(IN.uv_MaskTex, _MetallicGlossMapG)).rgb;
			half3 MSB = tex2D(_MetallicGlossMapB, TRANSFORM_TEX(IN.uv_MaskTex, _MetallicGlossMapB)).rgb;
			half3 MSA = tex2D(_MetallicGlossMapA, TRANSFORM_TEX(IN.uv_MaskTex, _MetallicGlossMapA)).rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = MSR.r * mask.r + MSG.r * mask.g + MSB.r * mask.b + MSA.r * mask.a;
            o.Smoothness = MSR.g * mask.r * _SmoothnessR 
						 + MSG.g * mask.g * _SmoothnessG
						 + MSB.g * mask.b * _SmoothnessB
						 + MSA.g * mask.a * _SmoothnessA;

			half3 normalR = UnpackNormalWithScale(tex2D(_BumpMapR, TRANSFORM_TEX(IN.uv_MaskTex, _BumpMapR)), _BumpMapScaleR);
			half3 normalG = UnpackNormalWithScale(tex2D(_BumpMapG, TRANSFORM_TEX(IN.uv_MaskTex, _BumpMapG)), _BumpMapScaleG);
			half3 normalB = UnpackNormalWithScale(tex2D(_BumpMapB, TRANSFORM_TEX(IN.uv_MaskTex, _BumpMapB)), _BumpMapScaleB);
			half3 normalA = UnpackNormalWithScale(tex2D(_BumpMapA, TRANSFORM_TEX(IN.uv_MaskTex, _BumpMapA)), _BumpMapScaleA);
			half3 normal = normalR * mask.r + normalG * mask.g + normalB * mask.b + normalA * mask.a;
			
			o.Normal = normalize(normal);
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
	CustomEditor "TerrainShaderGUI"
}
