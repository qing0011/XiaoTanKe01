// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:6,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33082,y:32834,varname:node_9361,prsc:2|custl-6795-OUT;n:type:ShaderForge.SFN_Tex2d,id:5952,x:32513,y:32976,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_5952,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9887-UVOUT;n:type:ShaderForge.SFN_Color,id:3171,x:32513,y:33176,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_3171,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:6795,x:32819,y:33075,varname:node_6795,prsc:2|A-5952-RGB,B-3171-RGB,C-5952-A,D-3171-A,E-3437-OUT;n:type:ShaderForge.SFN_UVTile,id:9887,x:32286,y:32976,varname:node_9887,prsc:2|UVIN-1141-OUT,WDT-671-OUT,HGT-4962-OUT,TILE-4555-OUT;n:type:ShaderForge.SFN_TexCoord,id:2735,x:31194,y:32799,varname:node_2735,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ComponentMask,id:9005,x:31412,y:32799,varname:node_9005,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2735-UVOUT;n:type:ShaderForge.SFN_Append,id:1141,x:31839,y:32803,varname:node_1141,prsc:2|A-9005-R,B-2016-OUT;n:type:ShaderForge.SFN_ValueProperty,id:671,x:31839,y:32997,ptovrint:False,ptlb:Width,ptin:_Width,varname:node_671,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:1982,x:31839,y:33121,ptovrint:False,ptlb:Height,ptin:_Height,varname:_node_671_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Trunc,id:4555,x:32056,y:33237,varname:node_4555,prsc:2|IN-795-OUT;n:type:ShaderForge.SFN_OneMinus,id:2016,x:31631,y:32907,varname:node_2016,prsc:2|IN-9005-G;n:type:ShaderForge.SFN_Negate,id:4962,x:32056,y:33054,varname:node_4962,prsc:2|IN-1982-OUT;n:type:ShaderForge.SFN_ValueProperty,id:795,x:31839,y:33271,ptovrint:False,ptlb:Sequence,ptin:_Sequence,varname:_Height_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Vector1,id:3437,x:32513,y:33378,varname:node_3437,prsc:2,v1:1.5;proporder:3171-5952-671-1982-795;pass:END;sub:END;*/

Shader "Shader Forge/Add_Sequence_HighSort" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _Width ("Width", Float ) = 1
        _Height ("Height", Float ) = 1
        _Sequence ("Sequence", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZTest Always
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _Width;
            uniform float _Height;
            uniform float _Sequence;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
                float node_4555 = trunc(_Sequence);
                float2 node_9887_tc_rcp = float2(1.0,1.0)/float2( _Width, (-1*_Height) );
                float node_9887_ty = floor(node_4555 * node_9887_tc_rcp.x);
                float node_9887_tx = node_4555 - _Width * node_9887_ty;
                float2 node_9005 = i.uv0.rg;
                float2 node_9887 = (float2(node_9005.r,(1.0 - node_9005.g)) + float2(node_9887_tx, node_9887_ty)) * node_9887_tc_rcp;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_9887, _MainTex));
                float3 finalColor = (_MainTex_var.rgb*_Color.rgb*_MainTex_var.a*_Color.a*1.5);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
