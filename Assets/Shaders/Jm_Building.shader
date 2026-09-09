Shader "Custom/Building" {
    Properties {
		_MainColor("Main Color",Color) = (1,1,1,1)
		_MainTex ("Diffuse (RGB)", 2D) = "white" {}
		_LitMap ("LitMap (RGB)", 2D) = "white" {}
		_LitMult ("LitMult", Float) = 2

		_LightDir("Light Dir", Vector) = (45,45,45,0)
		_ShadowColor("Shadow Color", Color) = (0,0,0,1)
		_ShadowFalloff("Shadow Falloff", Range(0, 1)) = 0.1

		[MaterialToggle] _RIM_ON("_RIM_ON", Int) = 0
		_RimColor("RimColor", Color) = (1,1,1,1)
		_RimMask("Rim Mask",2D) = "white"{}
		_RimPower("RimPower", Range(0.000001, 10.0)) = 1.0
		_RimIntensity("RimIntensity", Range(0.000001, 10.0)) = 1.0
		[Toggle(_MICAI_ON)] _MICAI_ON("_MICAI_ON", Int) = 0
	    _Fre_color ("Fre_color", Color) = (0.5,0.5,0.5,1)
        _Fresmel ("Fresmel", Range(0, 2)) = 0

		_StencilOper ("Stencil Operation", Float) = 10
		_StencilWMask ("Stencil Write Mask", Float) = 200
		_StencilRMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
		_BaseAlpha ("_BaseAlpha",Float) = 1

		_ClipPos ("_ClipPos",Float) = -1
		_ClipEdgeWidth ("_ClipEdgeWidth",Float) = 0.1
		_ClipEdgeColor ("_ClipEdgeColor",Color) = (1,1,0,0)
    }    
	SubShader {
        LOD 850
        Pass {
            Name "BASESHADOW"
            Tags { "QUEUE" = "AlphaTest+1" "IGNOREPROJECTOR"="true" "RenderType"="Opaque" }
			//Cull Back
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag 
                #pragma multi_compile _MICAI_OFF _MICAI_ON
                #include "UnityCG.cginc"
                #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            	#pragma target 2.0

                uniform sampler2D _MainTex;
                uniform sampler2D _LitMap;
                uniform float _LitMult,_RIM_ON;
                float4 _MainColor;
				float _BaseAlpha;
				float _ClipPos,_ClipEdgeWidth;
				float4 _ClipEdgeColor;
                #if _MICAI_ON
					uniform float _Fresmel;
					uniform sampler2D _Diff; uniform float4 _Diff_ST;
					uniform float4 _Fre_color;
			    #endif

//				#if _RIM_ON
					fixed4 _RimColor;
					float _RimPower,_RimIntensity;
					sampler2D _RimMask;
//				#endif

                struct v2f { 
                    float4 pos : SV_POSITION;
                    half2 texcord0 : TEXCOORD0;
                    half3 texcord1 : TEXCOORD1;
//                    #if _RIM_ON
                    	float3 WorldNormal : TEXCOORD2;
                    	float3 WorldViewDir : TEXCOORD3;
//                    #endif
					#if _MICAI_ON
						float4 posWorld : TEXCOORD4;
						float3 normalDir : TEXCOORD5;
					#endif
					float4 wpos : TEXCOORD6;

                };

                v2f vert(appdata_tan v)
                {
                    v2f o = (v2f)0;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.texcord0 = v.texcoord.xy;
                    float4 vec4 = fixed4(1,1,1,1);
                    vec4.w = 0.0;
                    vec4.xyz = v.normal.xyz;
                    o.texcord1 = mul(UNITY_MATRIX_MV, vec4).xyz;
//				    #if _RIM_ON
						o.WorldNormal = mul(v.normal,(float3x3)unity_WorldToObject);
						float3 WorldPos = mul(unity_ObjectToWorld,v.vertex);
                		o.WorldViewDir = _WorldSpaceCameraPos.xyz - WorldPos;
//					#endif
					#if _MICAI_ON
					    o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
					    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				    #endif
				    o.wpos = mul(unity_ObjectToWorld, v.vertex);
                    return o;
                }

               
                float4 frag(v2f inData) : COLOR
                {
                    fixed4 colorOut = fixed4(1,1,1,1);
                    colorOut.xyz = tex2D(_MainTex, inData.texcord0).xyz * _MainColor.rgb;
                    float2 vec2 = normalize(inData.texcord1).xy * 0.5 + 0.5;
                    float4 LitMap = tex2D(_LitMap, vec2);
                    colorOut.xyz = (colorOut.xyz * (LitMap.xyz * _LitMult));
                    colorOut.w = 1;

                    #if _MICAI_ON
					    inData.normalDir = normalize(inData.normalDir);
						float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - inData.posWorld.xyz);
						float3 normalDirection = inData.normalDir;
						float node_1000 = ((1.0-max(0,dot(inData.normalDir, viewDirection)))*_Fresmel);
						float NdotL = max(0.0,normalDirection);
						float3 directDiffuse = max( 0.0, NdotL);
						float4 _Diff_var = tex2D(_Diff,TRANSFORM_TEX(inData.texcord0, _Diff));
						float3 diffuse = directDiffuse * _Diff_var.rgb;
						float3 node_786 = (node_1000*_Fre_color.rgb);
						float3 emissive = node_786;
						float3 finalColor = diffuse + emissive;
				    	colorOut.xyz = colorOut.xyz + finalColor;
					    colorOut = fixed4(colorOut.xyz,node_1000);
				   	#endif

//                    #if _RIM_ON
						fixed3 WorldNormal = normalize(inData.WorldNormal);
						float3 WorldViewDir = normalize(inData.WorldViewDir);
						float rim = 1 - max(0,dot(WorldViewDir,WorldNormal));
						fixed3 rimColor = _RimColor * pow(rim,_RimPower) * _RimIntensity;
						fixed3 RimMask = tex2D(_RimMask,inData.texcord0).xyz;
						fixed3 Col = colorOut.xyz + rimColor * RimMask.r * _RIM_ON;
						colorOut.xyz = Col;
						colorOut.w = _BaseAlpha;
//					#endif
					
					float clipv = step(inData.wpos.y,_ClipPos)-1;
					clip(clipv * step(0,_ClipPos));
					float t = saturate((_ClipPos-inData.wpos.y)/_ClipEdgeWidth);
					colorOut.rgb += lerp(_ClipEdgeColor, 0, t)*step(0,_ClipPos);
                    return colorOut;
                }
            ENDCG
        }
		
		Pass
		{
			Name "Shadow"
			Stencil
			{
				Ref 0
				Comp equal
				Pass [_StencilOper] 
				ReadMask [_StencilRMask]
	            WriteMask [_StencilWMask]
			}
	        ColorMask [_ColorMask]
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off
			Offset -1, 0

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag 
				#include "UnityCG.cginc"
				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float4 color : COLOR;
					float4 wpos:TEXCOORD0;
				};

				float4 _LightDir;
				float4 _ShadowColor;
				float _ShadowFalloff;
				float _BaseAlpha;
				float _ClipPos;
				float3 ShadowProjectPos(float4 vertPos)
				{
					float3 shadowPos;
					float3 worldPos = mul(unity_ObjectToWorld , vertPos).xyz;
					float3 lightDir = normalize(_LightDir.xyz);
					shadowPos.y = min(worldPos.y , _LightDir.w);
					shadowPos.xz = worldPos.xz - lightDir.xz * max(0 , worldPos.y - _LightDir.w) / lightDir.y;
					return shadowPos;
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.wpos = mul(unity_ObjectToWorld , v.vertex);
					float3 shadowPos = ShadowProjectPos(v.vertex);
					o.vertex = UnityWorldToClipPos(shadowPos);
					float3 center = float3(unity_ObjectToWorld[0].w , _LightDir.w , unity_ObjectToWorld[2].w);
					float falloff = 1 - saturate(distance(shadowPos , center) * _ShadowFalloff);
					o.color = _ShadowColor;
					o.color.a = falloff * _BaseAlpha;
					o.color.a = 0.6;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float clipv = step(i.wpos.y,_ClipPos)-1;
					clip(clipv*step(0,_ClipPos));
					return i.color;
				}
			ENDCG
		}
    }

	SubShader {
        LOD 650
        Pass {
            Name "BASESHADOW"
            Tags { "QUEUE" = "AlphaTest+1" "IGNOREPROJECTOR"="true" "RenderType"="Opaque" }
			//Cull Back
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile _MICAI_OFF _MICAI_ON
                #include "UnityCG.cginc"
                #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            	#pragma target 2.0

                uniform sampler2D _MainTex;
                uniform sampler2D _LitMap;
                uniform float _LitMult,_RIM_ON;
                float4 _MainColor;
				float _BaseAlpha;
				float _ClipPos,_ClipEdgeWidth;
				float4 _ClipEdgeColor;
//			    #if _RIM_ON
					fixed4 _RimColor;
					float _RimPower,_RimIntensity;
					sampler2D _RimMask;
//				#endif

				#if _MICAI_ON
				uniform float _Fresmel;
				uniform sampler2D _Diff; uniform float4 _Diff_ST;
				uniform float4 _Fre_color;
			    #endif

                struct v2f { 
                    float4 pos : SV_POSITION;
                    half2 texcord0 : TEXCOORD0;
                    half3 texcord1 : TEXCOORD1;
//					#if _RIM_ON
                    float3 WorldNormal : TEXCOORD2;
                   	float3 WorldViewDir : TEXCOORD3;
//                    #endif
					#if _MICAI_ON
						float4 posWorld : TEXCOORD4;
						float3 normalDir : TEXCOORD5;
					#endif
					float4 wpos:TEXCOORD6;
                };

                v2f vert(appdata_tan v)
                {
                    v2f o = (v2f)0;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.texcord0 = v.texcoord.xy;
                    float4 vec4 = fixed4(1,1,1,1);
                    vec4.w = 0.0;
                    vec4.xyz = v.normal.xyz;
                    o.texcord1 = mul(UNITY_MATRIX_MV, vec4).xyz;
//				    #if _RIM_ON
						o.WorldNormal = mul(v.normal,(float3x3)unity_WorldToObject);
						float3 WorldPos = mul(unity_ObjectToWorld,v.vertex);
                		o.WorldViewDir = _WorldSpaceCameraPos.xyz - WorldPos;
//					#endif
					#if _MICAI_ON
					    o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
					    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				    #endif
				    o.wpos = mul(unity_ObjectToWorld, v.vertex);
                    return o;
                }

                float4 frag(v2f inData) : COLOR
                {
                    fixed4 colorOut = fixed4(1,1,1,1);
                    colorOut.xyz = tex2D(_MainTex, inData.texcord0).xyz * _MainColor.rgb;
                    float2 vec2 = normalize(inData.texcord1).xy * 0.5 + 0.5;
                    float4 LitMap = tex2D(_LitMap, vec2);
                    colorOut.xyz = (colorOut.xyz * (LitMap.xyz * _LitMult));
                    colorOut.w = 1;

                    #if _MICAI_ON
					    inData.normalDir = normalize(inData.normalDir);
						float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - inData.posWorld.xyz);
						float3 normalDirection = inData.normalDir;
						float node_1000 = ((1.0-max(0,dot(inData.normalDir, viewDirection)))*_Fresmel);
						float NdotL = max(0.0,normalDirection);
						float3 directDiffuse = max( 0.0, NdotL);
						float4 _Diff_var = tex2D(_Diff,TRANSFORM_TEX(inData.texcord0, _Diff));
						float3 diffuse = directDiffuse * _Diff_var.rgb;
						float3 node_786 = (node_1000*_Fre_color.rgb);
						float3 emissive = node_786;
						float3 finalColor = diffuse + emissive;
				    	colorOut.xyz = colorOut.xyz + finalColor;
					    colorOut = fixed4(colorOut.xyz,node_1000);
				   	#endif

//                    #if _RIM_ON
						fixed3 WorldNormal = normalize(inData.WorldNormal);
						float3 WorldViewDir = normalize(inData.WorldViewDir);
						float rim = 1 - max(0,dot(WorldViewDir,WorldNormal));
						fixed3 rimColor = _RimColor * pow(rim,_RimPower) * _RimIntensity;
						fixed3 RimMask = tex2D(_RimMask,inData.texcord0).xyz;
						fixed3 Col = colorOut.xyz + rimColor * RimMask.r * _RIM_ON;
						colorOut.xyz = Col;
						colorOut.a = _BaseAlpha;
//					#endif

					float clipv = step(inData.wpos.y,_ClipPos)-1;
					clip(clipv*step(0,_ClipPos));
					float t = saturate((_ClipPos-inData.wpos.y)/_ClipEdgeWidth);
					colorOut.rgb += lerp(_ClipEdgeColor, 0, t)*step(0,_ClipPos);
                    return colorOut;
                }
            ENDCG
        }

		Pass
		{
			Name "Shadow"
			Stencil
			{
				Ref 0
				Comp equal
				Pass incrWrap
				Fail keep
				ZFail keep
			}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite off
			Offset -1, 0

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag 
				#include "UnityCG.cginc"
				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float4 color : COLOR;
					float4 wpos:TEXCOORD0;
				};

				float4 _LightDir;
				float4 _ShadowColor;
				float _ShadowFalloff;
				float _BaseAlpha;
				float _ClipPos;

				float3 ShadowProjectPos(float4 vertPos)
				{
					float3 shadowPos;
					float3 worldPos = mul(unity_ObjectToWorld , vertPos).xyz;
					float3 lightDir = normalize(_LightDir.xyz);
					shadowPos.y = min(worldPos.y , _LightDir.w);
					shadowPos.xz = worldPos.xz - lightDir.xz * max(0 , worldPos.y - _LightDir.w) / lightDir.y;
					return shadowPos;
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.wpos = mul(unity_ObjectToWorld , v.vertex);
					float3 shadowPos = ShadowProjectPos(v.vertex);
					o.vertex = UnityWorldToClipPos(shadowPos);
					float3 center = float3(unity_ObjectToWorld[0].w , _LightDir.w , unity_ObjectToWorld[2].w);
					float falloff = 1 - saturate(distance(shadowPos , center) * _ShadowFalloff);
					o.color = _ShadowColor;
					o.color.a *= falloff* _BaseAlpha;		
					o.color.a = 0.6;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float clipv = step(i.wpos.y,_ClipPos)-1;
					clip(clipv*step(0,_ClipPos));
					return i.color;
				}
			ENDCG
		}
    }

	SubShader {
		LOD 200
		Pass {
			Name "BASE"
			Tags { "QUEUE" = "AlphaTest+1" "IGNOREPROJECTOR"="true" "RenderType"="Opaque" }
			Cull Back
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _MICAI_OFF _MICAI_ON
			#include "UnityCG.cginc"
			#pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 2.0

			uniform sampler2D _MainTex;
			uniform sampler2D _LitMap;
			uniform float _LitMult,_RIM_ON;
			float4 _MainColor;
			float _BaseAlpha;
			float _ClipPos,_ClipEdgeWidth;
			float4 _ClipEdgeColor;
//			#if _RIM_ON
			fixed4 _RimColor;
			float _RimPower,_RimIntensity;
			sampler2D _RimMask;
//			#endif
			#if _MICAI_ON
				uniform float _Fresmel;
				uniform sampler2D _Diff; uniform float4 _Diff_ST;
				uniform float4 _Fre_color;
		    #endif

			struct v2f { 
				float4 pos : SV_POSITION;
				half2 texcord0 : TEXCOORD0;
				half3 texcord1 : TEXCOORD1;
//				#if _RIM_ON
				float3 WorldNormal : TEXCOORD2;
                float3 WorldViewDir : TEXCOORD3;
//				#endif
			    #if _MICAI_ON
					float4 posWorld : TEXCOORD4;
					float3 normalDir : TEXCOORD5;
				#endif
				float4 wpos:TEXCOORD6;
			};

			v2f vert(appdata_tan v)
			{
				v2f o = (v2f)0;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcord0 = v.texcoord.xy;
				float4 vec4 = fixed4(1,1,1,1);
                vec4.w = 0.0;
                vec4.xyz = v.normal.xyz;
                o.texcord1 = mul(UNITY_MATRIX_MV, vec4).xyz;
//				#if _RIM_ON
				o.WorldNormal = mul(v.normal,(float3x3)unity_WorldToObject);
				float3 WorldPos = mul(unity_ObjectToWorld,v.vertex);
                o.WorldViewDir = _WorldSpaceCameraPos.xyz - WorldPos;
//				#endif
				#if _MICAI_ON
				    o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
				    o.posWorld = mul(unity_ObjectToWorld, v.vertex);
			    #endif
			    o.wpos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			float4 frag(v2f inData) : COLOR
			{
				fixed4 colorOut;
				colorOut.xyz = tex2D(_MainTex, inData.texcord0).xyz * _MainColor.rgb;
				float2 vec2 = normalize(inData.texcord1).xy * 0.5 + 0.5;
				float4 LitMap = tex2D(_LitMap, vec2);
				colorOut.xyz = (colorOut.xyz * (LitMap.xyz * _LitMult));
				colorOut.w = 1;

				#if _MICAI_ON
				    inData.normalDir = normalize(inData.normalDir);
					float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - inData.posWorld.xyz);
					float3 normalDirection = inData.normalDir;
					float node_1000 = ((1.0-max(0,dot(inData.normalDir, viewDirection)))*_Fresmel);
					float NdotL = max(0.0,normalDirection);
					float3 directDiffuse = max( 0.0, NdotL);
					float4 _Diff_var = tex2D(_Diff,TRANSFORM_TEX(inData.texcord0, _Diff));
					float3 diffuse = directDiffuse * _Diff_var.rgb;
					float3 node_786 = (node_1000*_Fre_color.rgb);
					float3 emissive = node_786;
					float3 finalColor = diffuse + emissive;
			    	colorOut.xyz = colorOut.xyz + finalColor;
				    colorOut = fixed4(colorOut.xyz,node_1000);
			   	#endif

//				#if _RIM_ON
				fixed3 WorldNormal = normalize(inData.WorldNormal);
				float3 WorldViewDir = normalize(inData.WorldViewDir);
				float rim = 1 - max(0,dot(WorldViewDir,WorldNormal));
				fixed3 rimColor = _RimColor * pow(rim,_RimPower) * _RimIntensity;
				fixed3 RimMask = tex2D(_RimMask,inData.texcord0).xyz;
				fixed3 Col = colorOut.xyz + rimColor * RimMask.r * _RIM_ON;
				colorOut.xyz = Col;
//				#endif
				colorOut.a = _BaseAlpha;

				float clipv = step(inData.wpos.y,_ClipPos)-1;
				clip(clipv*step(0,_ClipPos));
				float t = saturate((_ClipPos-inData.wpos.y)/_ClipEdgeWidth);
				colorOut.rgb += lerp(_ClipEdgeColor, 0, t)*step(0,_ClipPos);

				return colorOut;
			}
		ENDCG
		}
	}
	FallBack "Diffuse"
}
