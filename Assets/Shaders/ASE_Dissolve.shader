// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ASE_Dissolve"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Noise("Noise", 2D) = "white" {}
		_Dissolve_Color("Dissolve_Color", Color) = (1,1,1,0)
		_Dissolve_Color_strength("Dissolve_Color_strength", Float) = 1
		_Dissolve("Dissolve", Range( 0 , 1)) = 1
		_Edge("Edge", Range( 0 , 1)) = 0.01
		[Toggle(_VERTEX_COLOR_ON)] _Vertex_Color("Vertex_Color", Float) = 0
		[Toggle(_DISSOLVE_VERTEX_COLOR_ON)] _Dissolve_Vertex_Color("Dissolve_Vertex_Color", Float) = 0
		_Main_Color("Main_Color", Color) = (1,1,1,0)
		_Main("Main", 2D) = "white" {}
		_Main_Color_Strength("Main_Color_Strength", Float) = 1
		_Opacity("Opacity", Range( 0 , 1)) = 1
		[Toggle(_OPACITY_VERTEX_COLOR_ON)] _Opacity_vertex_color("Opacity_vertex_color", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha , SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _VERTEX_COLOR_ON
		#pragma shader_feature _DISSOLVE_VERTEX_COLOR_ON
		#pragma shader_feature _OPACITY_VERTEX_COLOR_ON
		#pragma surface surf Unlit keepalpha noshadow exclude_path:deferred noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Main;
		uniform float4 _Main_ST;
		uniform float _Main_Color_Strength;
		uniform float4 _Main_Color;
		uniform float _Edge;
		uniform float _Dissolve;
		uniform sampler2D _Noise;
		SamplerState sampler_Noise;
		uniform float4 _Noise_ST;
		uniform float4 _Dissolve_Color;
		uniform float _Dissolve_Color_strength;
		uniform float _Opacity;
		uniform float _Cutoff = 0.5;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_Main = i.uv_texcoord * _Main_ST.xy + _Main_ST.zw;
			#ifdef _VERTEX_COLOR_ON
				float staticSwitch112 = i.vertexColor.a;
			#else
				float staticSwitch112 = _Dissolve;
			#endif
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float4 tex2DNode1 = tex2D( _Noise, uv_Noise );
			float temp_output_93_0 = ( 1.0 - step( ( _Edge + staticSwitch112 ) , tex2DNode1.r ) );
			float3 appendResult118 = (float3(i.vertexColor.r , i.vertexColor.g , i.vertexColor.b));
			#ifdef _DISSOLVE_VERTEX_COLOR_ON
				float4 staticSwitch117 = float4( appendResult118 , 0.0 );
			#else
				float4 staticSwitch117 = _Dissolve_Color;
			#endif
			o.Emission = ( ( tex2D( _Main, uv_Main ) * _Main_Color_Strength * _Main_Color ) + ( ( temp_output_93_0 - ( 1.0 - step( staticSwitch112 , tex2DNode1.r ) ) ) * staticSwitch117 * _Dissolve_Color_strength ) ).rgb;
			#ifdef _OPACITY_VERTEX_COLOR_ON
				float staticSwitch122 = ( _Opacity * i.vertexColor.a );
			#else
				float staticSwitch122 = _Opacity;
			#endif
			o.Alpha = staticSwitch122;
			clip( temp_output_93_0 - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
2055;7;1906;1004;680.0913;54.59814;2.538463;True;True
Node;AmplifyShaderEditor.RangedFloatNode;101;-1048.846,1272.403;Inherit;False;Property;_Dissolve;Dissolve;4;0;Create;True;0;0;False;0;False;1;-0.79;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;111;-1047.899,1423.528;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;83;-858.6083,1052.521;Inherit;False;Property;_Edge;Edge;5;0;Create;True;0;0;False;0;False;0.01;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;112;-695.9133,1282.669;Inherit;False;Property;_Vertex_Color;Vertex_Color;6;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-644.5477,782.6539;Inherit;True;Property;_Noise;Noise;1;0;Create;True;0;0;False;0;False;-1;455b93176da849440bc6d8bda63e928c;455b93176da849440bc6d8bda63e928c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;70;-513.9543,1062.646;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;84;-267.5839,937.8049;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;88;-277.2593,1311.81;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;118;-784.4586,1565.238;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;87;-543.3106,1437.071;Inherit;False;Property;_Dissolve_Color;Dissolve_Color;2;0;Create;True;0;0;False;0;False;1,1,1,0;0,0.5748162,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;93;8.261004,1016.117;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;94;10.11273,1254.875;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;117;-205.8245,1600.538;Inherit;False;Property;_Dissolve_Vertex_Color;Dissolve_Vertex_Color;7;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;113;193.4469,1637.269;Inherit;False;Property;_Dissolve_Color_strength;Dissolve_Color_strength;3;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;258.328,470.2109;Inherit;False;Property;_Main_Color_Strength;Main_Color_Strength;10;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;96;221.1104,1209.897;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;102;21.05211,740.4483;Inherit;True;Property;_Main;Main;9;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;116;238.1226,570.9271;Inherit;False;Property;_Main_Color;Main_Color;8;0;Create;True;0;0;False;0;False;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;119;1536.411,784.5511;Inherit;False;Property;_Opacity;Opacity;11;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;519.2746,1290.588;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;561.8881,753.4584;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;1703.411,960.5511;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;122;1878.698,943.6113;Inherit;False;Property;_Opacity_vertex_color;Opacity_vertex_color;12;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;100;784.2778,895.2252;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;98;2277.029,855.8016;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;ASE_Dissolve;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;112;1;101;0
WireConnection;112;0;111;4
WireConnection;70;0;83;0
WireConnection;70;1;112;0
WireConnection;84;0;70;0
WireConnection;84;1;1;1
WireConnection;88;0;112;0
WireConnection;88;1;1;1
WireConnection;118;0;111;1
WireConnection;118;1;111;2
WireConnection;118;2;111;3
WireConnection;93;0;84;0
WireConnection;94;0;88;0
WireConnection;117;1;87;0
WireConnection;117;0;118;0
WireConnection;96;0;93;0
WireConnection;96;1;94;0
WireConnection;85;0;96;0
WireConnection;85;1;117;0
WireConnection;85;2;113;0
WireConnection;114;0;102;0
WireConnection;114;1;115;0
WireConnection;114;2;116;0
WireConnection;120;0;119;0
WireConnection;120;1;111;4
WireConnection;122;1;119;0
WireConnection;122;0;120;0
WireConnection;100;0;114;0
WireConnection;100;1;85;0
WireConnection;98;2;100;0
WireConnection;98;9;122;0
WireConnection;98;10;93;0
ASEEND*/
//CHKSM=A4DEE043A032617856CA9583738C93F8E41DE085