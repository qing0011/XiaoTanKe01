// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:32759,y:32420,varname:node_9361,prsc:2|alpha-3299-OUT,refract-9268-OUT;n:type:ShaderForge.SFN_Tex2d,id:9146,x:32024,y:32523,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_9146,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-9501-OUT;n:type:ShaderForge.SFN_Append,id:6580,x:32226,y:32540,varname:node_6580,prsc:2|A-9146-R,B-9146-G;n:type:ShaderForge.SFN_Multiply,id:9268,x:32445,y:32540,varname:node_9268,prsc:2|A-6580-OUT,B-9146-A,C-8867-A,D-3175-OUT;n:type:ShaderForge.SFN_VertexColor,id:8867,x:32226,y:32708,varname:node_8867,prsc:2;n:type:ShaderForge.SFN_Slider,id:3175,x:32069,y:32917,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:node_3175,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Vector1,id:3299,x:32445,y:32416,varname:node_3299,prsc:2,v1:0;n:type:ShaderForge.SFN_TexCoord,id:9545,x:31603,y:32624,varname:node_9545,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:2748,x:31171,y:32464,varname:node_2748,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:6821,x:31171,y:32339,ptovrint:False,ptlb:U_Speed,ptin:_U_Speed,varname:node_6821,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8876,x:31171,y:32657,ptovrint:False,ptlb:V_Speed,ptin:_V_Speed,varname:_node_6821_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3734,x:31387,y:32380,varname:node_3734,prsc:2|A-6821-OUT,B-2748-T;n:type:ShaderForge.SFN_Multiply,id:8325,x:31387,y:32563,varname:node_8325,prsc:2|A-2748-T,B-8876-OUT;n:type:ShaderForge.SFN_Append,id:9623,x:31603,y:32459,varname:node_9623,prsc:2|A-3734-OUT,B-8325-OUT;n:type:ShaderForge.SFN_Add,id:9501,x:31823,y:32523,varname:node_9501,prsc:2|A-9623-OUT,B-9545-UVOUT;proporder:9146-3175-6821-8876;pass:END;sub:END;*/

Shader "Shader Forge/Distortion03" {
    Properties {
        _Texture ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 0.5
        _U_Speed ("U_Speed", Float ) = 0
        _V_Speed ("V_Speed", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _Texture; uniform float4 _Texture_ST;
            uniform float _Intensity;
            uniform float _U_Speed;
            uniform float _V_Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 node_2748 = _Time;
                float2 node_9501 = (float2((_U_Speed*node_2748.g),(node_2748.g*_V_Speed))+i.uv0);
                float4 _Texture_var = tex2D(_Texture,TRANSFORM_TEX(node_9501, _Texture));
                float2 sceneUVs = (i.projPos.xy / i.projPos.w) + (float2(_Texture_var.r,_Texture_var.g)*_Texture_var.a*i.vertexColor.a*_Intensity);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                return fixed4(lerp(sceneColor.rgb, finalColor,0.0),1);
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
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
