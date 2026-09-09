// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE_vertex_move"
{
	Properties
	{
		_Main_Tex("Main_Tex", 2D) = "white" {}
		_Emission_Tex("Emission_Tex", 2D) = "white" {}
		_Emission_Mask("Emission_Mask", 2D) = "white" {}
		_Vertex_tex("Vertex_tex", 2D) = "white" {}
		_Vertex_shrength("Vertex_shrength", Float) = 2
		_Vertex_U_speed("Vertex_U_speed", Float) = 0
		_Vertex_V_speed("Vertex_V_speed", Float) = 0
		_Vertex_direction("Vertex_direction", Vector) = (1,1,1,0)
		_Vertex_Mask("Vertex_Mask", 2D) = "white" {}
		_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Emission_Color("Emission_Color", Color) = (1,1,1,0)
		_Emission_strength("Emission_strength", Float) = 1
		_Alpha_mask("Alpha_mask", 2D) = "white" {}
		_Main_U_speed("Main_U_speed", Float) = 0
		_Main_V_speed("Main_V_speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		ZTest Less
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _Vertex_shrength;
		uniform sampler2D _Vertex_tex;
		SamplerState sampler_Vertex_tex;
		uniform float _Vertex_U_speed;
		uniform float _Vertex_V_speed;
		uniform float4 _Vertex_tex_ST;
		uniform float3 _Vertex_direction;
		uniform sampler2D _Vertex_Mask;
		SamplerState sampler_Vertex_Mask;
		uniform float4 _Vertex_Mask_ST;
		uniform float4 _Main_Color;
		uniform sampler2D _Main_Tex;
		uniform float _Main_U_speed;
		uniform float _Main_V_speed;
		uniform float4 _Main_Tex_ST;
		uniform float4 _Emission_Color;
		uniform sampler2D _Emission_Tex;
		uniform float4 _Emission_Tex_ST;
		uniform float _Emission_strength;
		uniform sampler2D _Emission_Mask;
		uniform sampler2D _Alpha_mask;
		SamplerState sampler_Alpha_mask;
		uniform float4 _Alpha_mask_ST;
		SamplerState sampler_Main_Tex;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float2 appendResult12 = (float2(_Vertex_U_speed , _Vertex_V_speed));
			float2 uv_Vertex_tex = v.texcoord.xy * _Vertex_tex_ST.xy + _Vertex_tex_ST.zw;
			float2 panner8 = ( 1.0 * _Time.y * appendResult12 + uv_Vertex_tex);
			float2 uv_Vertex_Mask = v.texcoord * _Vertex_Mask_ST.xy + _Vertex_Mask_ST.zw;
			v.vertex.xyz += ( ase_vertexNormal * _Vertex_shrength * tex2Dlod( _Vertex_tex, float4( panner8, 0, 0.0) ).r * _Vertex_direction * tex2Dlod( _Vertex_Mask, float4( uv_Vertex_Mask, 0, 0.0) ).r );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 appendResult37 = (float2(_Main_U_speed , _Main_V_speed));
			float2 uv_Main_Tex = i.uv_texcoord * _Main_Tex_ST.xy + _Main_Tex_ST.zw;
			float2 panner34 = ( 1.0 * _Time.y * appendResult37 + uv_Main_Tex);
			float4 tex2DNode3 = tex2D( _Main_Tex, panner34 );
			o.Albedo = ( _Main_Color * tex2DNode3 ).rgb;
			float2 uv_Emission_Tex = i.uv_texcoord * _Emission_Tex_ST.xy + _Emission_Tex_ST.zw;
			float2 panner48 = ( 1.0 * _Time.y * appendResult37 + uv_Emission_Tex);
			o.Emission = ( _Emission_Color * tex2D( _Emission_Tex, panner48 ) * _Emission_strength * tex2D( _Emission_Mask, panner48 ) ).rgb;
			float2 uv_Alpha_mask = i.uv_texcoord * _Alpha_mask_ST.xy + _Alpha_mask_ST.zw;
			float smoothstepResult39 = smoothstep( 0.0 , 1.0 , tex2D( _Alpha_mask, uv_Alpha_mask ).r);
			o.Alpha = ( smoothstepResult39 * i.vertexColor.a * tex2DNode3.a );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
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
2055;1;1906;1010;1524.413;135.2059;1.026239;True;True
Node;AmplifyShaderEditor.RangedFloatNode;11;-1600.597,1553.766;Inherit;False;Property;_Vertex_V_speed;Vertex_V_speed;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1594.597,1420.766;Inherit;False;Property;_Vertex_U_speed;Vertex_U_speed;6;0;Create;True;0;0;False;0;False;0;-0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-1640.107,123.2823;Inherit;False;Property;_Main_U_speed;Main_U_speed;14;0;Create;True;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1641.107,221.2823;Inherit;False;Property;_Main_V_speed;Main_V_speed;15;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1442.091,-316.795;Inherit;False;0;3;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;37;-1423.107,153.2823;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;12;-1281.597,1474.766;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1618.597,1239.766;Inherit;False;0;7;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-1465.824,-39.10251;Inherit;False;0;44;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;48;-1182.824,206.8975;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;8;-1149.597,1249.766;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;34;-995.4664,-165.2029;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;30;-494.6266,512.497;Inherit;True;Property;_Alpha_mask;Alpha_mask;13;0;Create;True;0;0;False;0;False;-1;None;5c4bb25ea6794b14d8ee1239c126f828;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;4;-581.474,945.6304;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;13;-466.682,1326.513;Inherit;False;Property;_Vertex_direction;Vertex_direction;8;0;Create;True;0;0;False;0;False;1,1,1;1,1,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;14;-511.973,1593.536;Inherit;True;Property;_Vertex_Mask;Vertex_Mask;9;0;Create;True;0;0;False;0;False;-1;None;5c4bb25ea6794b14d8ee1239c126f828;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-727.5496,-127.5369;Inherit;True;Property;_Main_Tex;Main_Tex;1;0;Create;True;0;0;False;0;False;-1;None;3a1a5315918477942a77949d9254dd22;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-556.8364,1125.555;Inherit;False;Property;_Vertex_shrength;Vertex_shrength;5;0;Create;True;0;0;False;0;False;2;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;7;-847.5979,1229.766;Inherit;True;Property;_Vertex_tex;Vertex_tex;4;0;Create;True;0;0;False;0;False;-1;None;a236b5a1fe1ddef42b9195d717d512da;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;19;-721.0913,121.205;Inherit;False;Property;_Emission_Color;Emission_Color;11;0;Create;True;0;0;False;0;False;1,1,1,0;1,0.6755391,0.495283,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-409.0913,-286.795;Inherit;False;Property;_Main_Color;Main_Color;10;0;Create;True;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;39;-19.05127,398.3413;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-971.1188,483.6868;Inherit;True;Property;_Emission_Mask;Emission_Mask;3;0;Create;True;0;0;False;0;False;-1;None;3a1a5315918477942a77949d9254dd22;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;41;-78.04523,640.4768;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-702.0913,349.205;Inherit;False;Property;_Emission_strength;Emission_strength;12;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;44;-990.7523,269.899;Inherit;True;Property;_Emission_Tex;Emission_Tex;2;0;Create;True;0;0;False;0;False;-1;None;3a1a5315918477942a77949d9254dd22;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;302.2108,625.226;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-151.703,1038.963;Inherit;False;5;5;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-3.091309,-99.79501;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-29.09131,193.205;Inherit;True;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;419,4;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;ASE_vertex_move;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;2;False;-1;1;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;37;0;35;0
WireConnection;37;1;36;0
WireConnection;12;0;10;0
WireConnection;12;1;11;0
WireConnection;48;0;47;0
WireConnection;48;2;37;0
WireConnection;8;0;9;0
WireConnection;8;2;12;0
WireConnection;34;0;26;0
WireConnection;34;2;37;0
WireConnection;3;1;34;0
WireConnection;7;1;8;0
WireConnection;39;0;30;1
WireConnection;50;1;48;0
WireConnection;44;1;48;0
WireConnection;42;0;39;0
WireConnection;42;1;41;4
WireConnection;42;2;3;4
WireConnection;6;0;4;0
WireConnection;6;1;5;0
WireConnection;6;2;7;1
WireConnection;6;3;13;0
WireConnection;6;4;14;1
WireConnection;15;0;16;0
WireConnection;15;1;3;0
WireConnection;18;0;19;0
WireConnection;18;1;44;0
WireConnection;18;2;20;0
WireConnection;18;3;50;0
WireConnection;0;0;15;0
WireConnection;0;2;18;0
WireConnection;0;9;42;0
WireConnection;0;11;6;0
ASEEND*/
//CHKSM=2090F4763D03898F8C153A51A3E6190D5F5420E9