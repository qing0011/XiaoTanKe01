// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:34249,y:32342,varname:node_9361,prsc:2|emission-4337-OUT,alpha-4320-OUT,clip-5696-OUT;n:type:ShaderForge.SFN_Tex2d,id:6592,x:33631,y:32371,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_6592,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3610,x:31837,y:32622,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:node_3610,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Blend,id:4337,x:33948,y:32371,varname:node_4337,prsc:2,blmd:5,clmp:True|SRC-6592-RGB,DST-4660-OUT;n:type:ShaderForge.SFN_Multiply,id:4660,x:33631,y:32556,varname:node_4660,prsc:2|A-4528-RGB,B-5696-OUT,C-2304-OUT;n:type:ShaderForge.SFN_Color,id:4528,x:33307,y:32375,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_4528,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:1,c4:1;n:type:ShaderForge.SFN_Step,id:5696,x:32804,y:32941,varname:node_5696,prsc:2|A-3610-R,B-6133-OUT;n:type:ShaderForge.SFN_Slider,id:6133,x:31639,y:33178,ptovrint:False,ptlb:Synthesis,ptin:_Synthesis,varname:node_6133,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-1,cur:-1,max:2;n:type:ShaderForge.SFN_Tex2d,id:3430,x:32624,y:33760,ptovrint:False,ptlb:Detail,ptin:_Detail,varname:node_3430,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9195,x:31815,y:33469,ptovrint:False,ptlb:Wangge,ptin:_Wangge,varname:node_9195,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_RemapRange,id:8088,x:32141,y:33248,varname:node_8088,prsc:2,frmn:0,frmx:2,tomn:0,tomx:5|IN-6133-OUT;n:type:ShaderForge.SFN_RemapRange,id:4772,x:32097,y:32827,varname:node_4772,prsc:2,frmn:0,frmx:1,tomn:1,tomx:6|IN-3610-R;n:type:ShaderForge.SFN_Subtract,id:3406,x:32621,y:33520,varname:node_3406,prsc:2|A-4772-OUT,B-8088-OUT;n:type:ShaderForge.SFN_Multiply,id:3640,x:32899,y:33518,varname:node_3640,prsc:2|A-3430-R,B-3406-OUT;n:type:ShaderForge.SFN_Clamp01,id:2304,x:33397,y:33209,varname:node_2304,prsc:2|IN-9751-OUT;n:type:ShaderForge.SFN_Add,id:9751,x:33152,y:33360,varname:node_9751,prsc:2|A-3640-OUT,B-5162-OUT;n:type:ShaderForge.SFN_Subtract,id:5162,x:32624,y:33252,varname:node_5162,prsc:2|A-1290-OUT,B-8088-OUT;n:type:ShaderForge.SFN_RemapRange,id:1290,x:32141,y:33468,varname:node_1290,prsc:2,frmn:0,frmx:1,tomn:1,tomx:4|IN-9195-R;n:type:ShaderForge.SFN_Multiply,id:4320,x:33948,y:32570,varname:node_4320,prsc:2|A-6592-A,B-5696-OUT,C-9850-A;n:type:ShaderForge.SFN_VertexColor,id:9850,x:33611,y:32947,varname:node_9850,prsc:2;proporder:6592-3610-4528-3430-9195-6133;pass:END;sub:END;*/

Shader "Shader Forge/Card01" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Alpha ("Alpha", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,1,1)
        _Detail ("Detail", 2D) = "white" {}
        _Wangge ("Wangge", 2D) = "white" {}
        _Synthesis ("Synthesis", Range(-1, 2)) = -1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            uniform float4 _Color;
            uniform float _Synthesis;
            uniform sampler2D _Detail; uniform float4 _Detail_ST;
            uniform sampler2D _Wangge; uniform float4 _Wangge_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
                float node_5696 = step(_Alpha_var.r,_Synthesis);
                clip(node_5696 - 0.5);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _Detail_var = tex2D(_Detail,TRANSFORM_TEX(i.uv0, _Detail));
                float node_8088 = (_Synthesis*2.5+0.0);
                float4 _Wangge_var = tex2D(_Wangge,TRANSFORM_TEX(i.uv0, _Wangge));
                float3 emissive = saturate(max(_MainTex_var.rgb,(_Color.rgb*node_5696*saturate(((_Detail_var.r*((_Alpha_var.r*5.0+1.0)-node_8088))+((_Wangge_var.r*3.0+1.0)-node_8088))))));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_MainTex_var.a*node_5696*i.vertexColor.a));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            uniform float _Synthesis;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
                float node_5696 = step(_Alpha_var.r,_Synthesis);
                clip(node_5696 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
