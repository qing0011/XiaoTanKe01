// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE_depth"
{
	Properties
	{
		_Depth_strength("Depth_strength", Range( -1 , 1)) = 0
		_Depth_Map("Depth_Map", 2D) = "white" {}
		_main("main", 2D) = "white" {}
		_Main_Color_Strength("Main_Color_Strength", Float) = 0
		_Opacity_texture("Opacity_texture", 2D) = "white" {}
		_Dissolve_texture("Dissolve_texture", 2D) = "white" {}
		_Opacity("Opacity", Range( 0 , 1)) = 1
		_Dissolve("Dissolve", Range( 0 , 1)) = 0
		_Dissolve_soft("Dissolve_soft", Range( 0 , 1)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Speed_UV("Speed_UV", Vector) = (1,1,0.2,0.2)
		_Emission_Color("Emission_Color", Color) = (1,0.5215687,0,0)
		_Color1("Color 1", Color) = (1,0,0,0)
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Move_U_speed("Move_U_speed", Float) = 0.1
		_Move_V_speed("Move_V_speed", Float) = 0
		_Main_Color("Main_Color", Color) = (1,1,1,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			float3 worldNormal;
			float3 worldPos;
		};

		uniform sampler2D _main;
		uniform float _Move_U_speed;
		uniform float _Move_V_speed;
		uniform sampler2D _Depth_Map;
		uniform float4 _Depth_Map_ST;
		uniform float _Depth_strength;
		uniform float _Main_Color_Strength;
		uniform float4 _Main_Color;
		uniform sampler2D _TextureSample2;
		SamplerState sampler_TextureSample2;
		uniform float4 _Color1;
		uniform sampler2D _TextureSample1;
		uniform float4 _Speed_UV;
		uniform float4 _Emission_Color;
		uniform sampler2D _Opacity_texture;
		SamplerState sampler_Opacity_texture;
		uniform float _Opacity;
		uniform float _Dissolve_soft;
		uniform sampler2D _Dissolve_texture;
		SamplerState sampler_Dissolve_texture;
		uniform float4 _Dissolve_texture_ST;
		uniform float _Dissolve;


		inline float2 POM( sampler2D heightMap, float2 uvs, float2 dx, float2 dy, float3 normalWorld, float3 viewWorld, float3 viewDirTan, int minSamples, int maxSamples, float parallax, float refPlane, float2 tilling, float2 curv, int index )
		{
			float3 result = 0;
			int stepIndex = 0;
			int numSteps = ( int )lerp( (float)maxSamples, (float)minSamples, saturate( dot( normalWorld, viewWorld ) ) );
			float layerHeight = 1.0 / numSteps;
			float2 plane = parallax * ( viewDirTan.xy / viewDirTan.z );
			uvs.xy += refPlane * plane;
			float2 deltaTex = -plane * layerHeight;
			float2 prevTexOffset = 0;
			float prevRayZ = 1.0f;
			float prevHeight = 0.0f;
			float2 currTexOffset = deltaTex;
			float currRayZ = 1.0f - layerHeight;
			float currHeight = 0.0f;
			float intersection = 0;
			float2 finalTexOffset = 0;
			while ( stepIndex < numSteps + 1 )
			{
			 	currHeight = tex2Dgrad( heightMap, uvs + currTexOffset, dx, dy ).r;
			 	if ( currHeight > currRayZ )
			 	{
			 	 	stepIndex = numSteps + 1;
			 	}
			 	else
			 	{
			 	 	stepIndex++;
			 	 	prevTexOffset = currTexOffset;
			 	 	prevRayZ = currRayZ;
			 	 	prevHeight = currHeight;
			 	 	currTexOffset += deltaTex;
			 	 	currRayZ -= layerHeight;
			 	}
			}
			int sectionSteps = 10;
			int sectionIndex = 0;
			float newZ = 0;
			float newHeight = 0;
			while ( sectionIndex < sectionSteps )
			{
			 	intersection = ( prevHeight - prevRayZ ) / ( prevHeight - currHeight + currRayZ - prevRayZ );
			 	finalTexOffset = prevTexOffset + intersection * deltaTex;
			 	newZ = prevRayZ - intersection * layerHeight;
			 	newHeight = tex2Dgrad( heightMap, uvs + finalTexOffset, dx, dy ).r;
			 	if ( newHeight > newZ )
			 	{
			 	 	currTexOffset = finalTexOffset;
			 	 	currHeight = newHeight;
			 	 	currRayZ = newZ;
			 	 	deltaTex = intersection * deltaTex;
			 	 	layerHeight = intersection * layerHeight;
			 	}
			 	else
			 	{
			 	 	prevTexOffset = finalTexOffset;
			 	 	prevHeight = newHeight;
			 	 	prevRayZ = newZ;
			 	 	deltaTex = ( 1 - intersection ) * deltaTex;
			 	 	layerHeight = ( 1 - intersection ) * layerHeight;
			 	}
			 	sectionIndex++;
			}
			return uvs.xy + finalTexOffset;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 appendResult93 = (float2(_Move_U_speed , _Move_V_speed));
			float2 uv_Depth_Map = i.uv_texcoord * _Depth_Map_ST.xy + _Depth_Map_ST.zw;
			float2 panner90 = ( 1.0 * _Time.y * appendResult93 + uv_Depth_Map);
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float2 OffsetPOM1 = POM( _Depth_Map, panner90, ddx(panner90), ddy(panner90), ase_worldNormal, ase_worldViewDir, i.viewDir, 60, 60, _Depth_strength, 0.5, _Depth_Map_ST.xy, float2(10,10), 0 );
			float2 DepthUV10 = OffsetPOM1;
			float4 Albedo24 = ( tex2D( _main, DepthUV10 ) * _Main_Color_Strength * _Main_Color );
			o.Albedo = Albedo24.rgb;
			float4 tex2DNode74 = tex2D( _TextureSample2, DepthUV10 );
			float2 temp_output_55_0 = (DepthUV10*1.0 + 0.0);
			float2 break57 = temp_output_55_0;
			float2 appendResult58 = (float2(length( temp_output_55_0 ) , ( atan2( break57.y , break57.x ) / UNITY_PI )));
			float2 appendResult63 = (float2(_Speed_UV.x , _Speed_UV.y));
			float2 appendResult64 = (float2(_Speed_UV.z , _Speed_UV.w));
			float4 lerpResult72 = lerp( ( tex2DNode74.r * _Color1 ) , ( tex2D( _TextureSample1, (appendResult58*appendResult63 + ( appendResult64 * _Time.y )) ) * _Emission_Color ) , tex2DNode74.r);
			float4 Emission77 = lerpResult72;
			o.Emission = Emission77.rgb;
			float2 uv_Dissolve_texture = i.uv_texcoord * _Dissolve_texture_ST.xy + _Dissolve_texture_ST.zw;
			float smoothstepResult44 = smoothstep( _Dissolve_soft , ( 1.0 - _Dissolve_soft ) , ( tex2D( _Dissolve_texture, uv_Dissolve_texture ).r + 1.0 + ( _Dissolve * -2.0 ) ));
			float Alpha51 = ( tex2D( _Opacity_texture, DepthUV10 ).a * _Opacity * smoothstepResult44 );
			o.Alpha = Alpha51;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
2055;7;1906;1004;-229.7825;816.1166;2.197855;True;True
Node;AmplifyShaderEditor.CommentaryNode;32;-1335.035,-149.3343;Inherit;False;1197.629;831.6237;Depth;10;10;1;90;14;17;9;93;91;92;18;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-1246.182,-32.67921;Inherit;False;Property;_Move_U_speed;Move_U_speed;15;0;Create;True;0;0;False;0;False;0.1;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-1236.801,63.39022;Inherit;False;Property;_Move_V_speed;Move_V_speed;16;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;18;-1055.284,144.9619;Inherit;True;Property;_Depth_Map;Depth_Map;2;0;Create;True;0;0;False;0;False;a430725aaff788348b75219f493a11d0;739796ce0ca879f4fbb28bb29458e097;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DynamicAppendNode;93;-955.98,35.33591;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1076.558,-110.8418;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;14;-952.8369,493.1765;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;17;-1131.509,347.1691;Inherit;False;Property;_Depth_strength;Depth_strength;0;0;Create;True;0;0;False;0;False;0;0.125;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;90;-781.6599,-36.53721;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ParallaxOcclusionMappingNode;1;-610.4666,139.2057;Inherit;False;0;60;False;-1;60;False;-1;10;0.02;0.5;False;1,1;False;10,10;7;0;FLOAT2;0,0;False;1;SAMPLER2D;;False;2;FLOAT;0.02;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT2;0,0;False;6;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-355.8554,139.6966;Inherit;True;DepthUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;53;-567.9482,1946.831;Inherit;False;3288.625;877.3567;Emission;23;77;72;75;69;71;73;68;62;76;66;63;58;67;64;60;56;65;61;59;57;55;54;74;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-457.988,2105.567;Inherit;False;10;DepthUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;55;-225.9073,2152.721;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;57;-100.2241,2412.269;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ATan2OpNode;59;224.7759,2437.269;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PiNode;61;157.7759,2604.269;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;65;471.7757,2570.269;Inherit;False;Property;_Speed_UV;Speed_UV;11;0;Create;True;0;0;False;0;False;1,1,0.2,0.2;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;56;122.0926,2159.721;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;37;152.3134,621.717;Inherit;False;1309.347;851.8842;Alpha;14;51;50;49;47;46;45;41;44;43;42;40;38;39;48;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;67;761.7758,2715.269;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;60;446.7758,2441.269;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;64;761.7758,2501.269;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;983.776,2565.269;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;63;754.7758,2342.269;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;541.7758,2143.269;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;49;213.065,1346.248;Inherit;False;Constant;_Float3;Float 3;10;0;Create;True;0;0;False;0;False;-2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;165.065,1218.248;Inherit;False;Property;_Dissolve;Dissolve;8;0;Create;True;0;0;False;0;False;0;0.536;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;432.065,1346.248;Inherit;False;Property;_Dissolve_soft;Dissolve_soft;9;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;19;358.7255,-157.9532;Inherit;False;938.019;556.2693;Albedo;6;143;24;22;23;20;21;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;62;941.7759,2164.269;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;1,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;76;1237.461,2031.341;Inherit;False;10;DepthUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;460.065,1216.248;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;42;186.065,916.2478;Inherit;True;Property;_Dissolve_texture;Dissolve_texture;6;0;Create;True;0;0;False;0;False;-1;None;5c4bb25ea6794b14d8ee1239c126f828;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;297.065,1122.248;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;588.065,964.2478;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;1710.516,2459.869;Inherit;False;Property;_Color1;Color 1;13;0;Create;True;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;74;1546.019,2004.4;Inherit;True;Property;_TextureSample2;Texture Sample 2;14;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;68;1236.776,2179.269;Inherit;True;Property;_TextureSample1;Texture Sample 1;10;0;Create;True;0;0;False;0;False;-1;None;df1bf130274da0041a3bc7eb62930de1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;21;397.7021,-89.0987;Inherit;False;10;DepthUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;45;635.065,1139.248;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;71;1406.776,2457.269;Inherit;False;Property;_Emission_Color;Emission_Color;12;0;Create;True;0;0;False;0;False;1,0.5215687,0,0;0.3584906,0.1328608,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;38;197.065,741.2478;Inherit;False;10;DepthUV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;23;679.8232,95.1445;Inherit;False;Property;_Main_Color_Strength;Main_Color_Strength;4;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;1711.776,2272.269;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;143;661.9743,196.6351;Inherit;False;Property;_Main_Color;Main_Color;17;0;Create;True;0;0;False;0;False;1,1,1,1;0.7333333,0.5882353,0.5176471,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;20;590.0949,-108.408;Inherit;True;Property;_main;main;3;0;Create;True;0;0;False;0;False;-1;None;739796ce0ca879f4fbb28bb29458e097;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;706.733,899.8674;Inherit;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;1929.428,2059.218;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;39;516.065,703.2478;Inherit;True;Property;_Opacity_texture;Opacity_texture;5;0;Create;True;0;0;False;0;False;-1;None;739796ce0ca879f4fbb28bb29458e097;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;44;861.065,988.2478;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;903.8232,-89.85554;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;72;2146.698,2243.532;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;996.0651,745.2478;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;77;2456.891,2249.495;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;24;1072.823,-90.85557;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;1213.522,767.238;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;1415.447,-276.1485;Inherit;False;24;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;1458.927,142.0886;Inherit;False;51;Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;78;1415.742,-91.436;Inherit;False;77;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1759.217,-205.162;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ASE_depth;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;93;0;91;0
WireConnection;93;1;92;0
WireConnection;9;2;18;0
WireConnection;90;0;9;0
WireConnection;90;2;93;0
WireConnection;1;0;90;0
WireConnection;1;1;18;0
WireConnection;1;2;17;0
WireConnection;1;3;14;0
WireConnection;10;0;1;0
WireConnection;55;0;54;0
WireConnection;57;0;55;0
WireConnection;59;0;57;1
WireConnection;59;1;57;0
WireConnection;56;0;55;0
WireConnection;60;0;59;0
WireConnection;60;1;61;0
WireConnection;64;0;65;3
WireConnection;64;1;65;4
WireConnection;66;0;64;0
WireConnection;66;1;67;0
WireConnection;63;0;65;1
WireConnection;63;1;65;2
WireConnection;58;0;56;0
WireConnection;58;1;60;0
WireConnection;62;0;58;0
WireConnection;62;1;63;0
WireConnection;62;2;66;0
WireConnection;47;0;48;0
WireConnection;47;1;49;0
WireConnection;43;0;42;1
WireConnection;43;1;46;0
WireConnection;43;2;47;0
WireConnection;74;1;76;0
WireConnection;68;1;62;0
WireConnection;45;0;50;0
WireConnection;69;0;68;0
WireConnection;69;1;71;0
WireConnection;20;1;21;0
WireConnection;75;0;74;1
WireConnection;75;1;73;0
WireConnection;39;1;38;0
WireConnection;44;0;43;0
WireConnection;44;1;50;0
WireConnection;44;2;45;0
WireConnection;22;0;20;0
WireConnection;22;1;23;0
WireConnection;22;2;143;0
WireConnection;72;0;75;0
WireConnection;72;1;69;0
WireConnection;72;2;74;1
WireConnection;40;0;39;4
WireConnection;40;1;41;0
WireConnection;40;2;44;0
WireConnection;77;0;72;0
WireConnection;24;0;22;0
WireConnection;51;0;40;0
WireConnection;0;0;31;0
WireConnection;0;2;78;0
WireConnection;0;9;52;0
ASEEND*/
//CHKSM=1E7ECDC311255534106BEC285C4CA83B8959A51C