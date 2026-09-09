// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33242,y:32711,varname:node_9361,prsc:2|custl-4445-OUT;n:type:ShaderForge.SFN_Tex2d,id:9381,x:32587,y:32642,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_9381,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:6640,x:32587,y:32877,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_6640,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_VertexColor,id:4990,x:32587,y:33091,varname:node_4990,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4445,x:32935,y:32772,varname:node_4445,prsc:2|A-9381-RGB,B-6640-RGB,C-4990-RGB,D-2984-OUT;n:type:ShaderForge.SFN_Multiply,id:2984,x:32935,y:32970,varname:node_2984,prsc:2|A-9381-A,B-6640-A,C-4990-A,D-5428-OUT;n:type:ShaderForge.SFN_Tex2d,id:3006,x:31777,y:33092,ptovrint:False,ptlb:DissolveTex,ptin:_DissolveTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:8599,x:31777,y:33339,ptovrint:False,ptlb:Softness,ptin:_Softness,varname:node_8599,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_TexCoord,id:6052,x:31777,y:33560,varname:node_6052,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Vector1,id:4310,x:31777,y:33451,varname:node_4310,prsc:2,v1:-1.5;n:type:ShaderForge.SFN_Lerp,id:994,x:32074,y:33451,varname:node_994,prsc:2|A-8599-OUT,B-4310-OUT,T-6052-U;n:type:ShaderForge.SFN_Multiply,id:3005,x:32074,y:33218,varname:node_3005,prsc:2|A-3006-R,B-8599-OUT;n:type:ShaderForge.SFN_Subtract,id:4962,x:32331,y:33336,varname:node_4962,prsc:2|A-3005-OUT,B-994-OUT;n:type:ShaderForge.SFN_Clamp01,id:5428,x:32587,y:33336,varname:node_5428,prsc:2|IN-4962-OUT;proporder:6640-9381-3006-8599;pass:END;sub:END;*/

Shader "Shader Forge/Add_Dissolve" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _DissolveTex ("DissolveTex", 2D) = "white" {}
        _Softness ("Softness", Float ) = 0
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
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform sampler2D _DissolveTex; uniform float4 _DissolveTex_ST;
            uniform float _Softness;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 texcoord1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _DissolveTex_var = tex2D(_DissolveTex,TRANSFORM_TEX(i.uv0, _DissolveTex));
                float3 finalColor = (_MainTex_var.rgb*_Color.rgb*i.vertexColor.rgb*(_MainTex_var.a*_Color.a*i.vertexColor.a*saturate(((_DissolveTex_var.r*_Softness)-lerp(_Softness,(-1.5),i.uv1.r)))));
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
