// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33029,y:33291,varname:node_9361,prsc:2|custl-2791-OUT,olwid-2876-OUT,olcol-675-RGB;n:type:ShaderForge.SFN_Tex2d,id:3192,x:32420,y:33434,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_node_3192,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-3019-OUT;n:type:ShaderForge.SFN_Color,id:5450,x:32420,y:33631,ptovrint:False,ptlb:Color,ptin:_Color,varname:_node_5450,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:2791,x:32730,y:33533,varname:node_2791,prsc:2|A-3192-RGB,B-5450-RGB;n:type:ShaderForge.SFN_TexCoord,id:5265,x:31992,y:33660,varname:node_5265,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:8739,x:31567,y:33473,varname:node_8739,prsc:2;n:type:ShaderForge.SFN_Add,id:3019,x:32195,y:33564,varname:node_3019,prsc:2|A-1155-OUT,B-5265-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:2161,x:31567,y:33371,ptovrint:False,ptlb:U_Speed,ptin:_U_Speed,varname:_node_2161,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9179,x:31567,y:33661,ptovrint:False,ptlb:V_Speed,ptin:_V_Speed,varname:_node_9179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:4885,x:31810,y:33358,varname:node_4885,prsc:2|A-2161-OUT,B-8739-T;n:type:ShaderForge.SFN_Multiply,id:2727,x:31810,y:33586,varname:node_2727,prsc:2|A-8739-T,B-9179-OUT;n:type:ShaderForge.SFN_Append,id:1155,x:31992,y:33465,varname:node_1155,prsc:2|A-4885-OUT,B-2727-OUT;n:type:ShaderForge.SFN_Color,id:675,x:32730,y:33817,ptovrint:False,ptlb:Outline_Color,ptin:_Outline_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:2876,x:32730,y:33713,ptovrint:False,ptlb:Outline_Width,ptin:_Outline_Width,varname:node_2876,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:5450-3192-2161-9179-2876-675;pass:END;sub:END;*/

Shader "Shader Forge/Opaque_UVmove" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _U_Speed ("U_Speed", Float ) = 0
        _V_Speed ("V_Speed", Float ) = 0
        _Outline_Width ("Outline_Width", Float ) = 0
        _Outline_Color ("Outline_Color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _Outline_Color;
            uniform float _Outline_Width;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_Outline_Width,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_Outline_Color.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _U_Speed;
            uniform float _V_Speed;
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
                float4 node_8739 = _Time;
                float2 node_3019 = (float2((_U_Speed*node_8739.g),(node_8739.g*_V_Speed))+i.uv0);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_3019, _MainTex));
                float3 finalColor = (_MainTex_var.rgb*_Color.rgb);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
