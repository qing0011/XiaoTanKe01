// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32715,y:32767,varname:node_3138,prsc:2|emission-4947-RGB,alpha-4947-A;n:type:ShaderForge.SFN_TexCoord,id:2706,x:32033,y:32825,varname:node_2706,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Rotator,id:4693,x:32277,y:32867,varname:node_4693,prsc:2|UVIN-2706-UVOUT,ANG-7329-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1967,x:31772,y:33200,ptovrint:False,ptlb:Angle,ptin:_Angle,varname:node_1967,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:7329,x:32033,y:32999,varname:node_7329,prsc:2|A-2868-OUT,B-1967-OUT;n:type:ShaderForge.SFN_Tex2d,id:4947,x:32468,y:32867,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_4947,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4693-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:1818,x:31201,y:32913,varname:node_1818,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector2,id:7336,x:31201,y:33080,varname:node_7336,prsc:2,v1:0.5,v2:0.5;n:type:ShaderForge.SFN_Distance,id:7924,x:31426,y:32978,varname:node_7924,prsc:2|A-1818-UVOUT,B-7336-OUT;n:type:ShaderForge.SFN_Multiply,id:50,x:31595,y:32978,varname:node_50,prsc:2|A-7924-OUT,B-6495-OUT;n:type:ShaderForge.SFN_Vector1,id:6495,x:31426,y:33129,varname:node_6495,prsc:2,v1:2;n:type:ShaderForge.SFN_RemapRange,id:2868,x:31772,y:32978,varname:node_2868,prsc:2,frmn:0,frmx:1,tomn:1,tomx:0|IN-50-OUT;proporder:4947-1967;pass:END;sub:END;*/

Shader "Shader Forge/UVRotate01" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Angle ("Angle", Float ) = 0
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform float _Angle;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
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
////// Emissive:
                float node_4693_ang = (((distance(i.uv0,float2(0.5,0.5))*2.0)*-1.0+1.0)*_Angle);
                float node_4693_spd = 1.0;
                float node_4693_cos = cos(node_4693_spd*node_4693_ang);
                float node_4693_sin = sin(node_4693_spd*node_4693_ang);
                float2 node_4693_piv = float2(0.5,0.5);
                float2 node_4693 = (mul(i.uv0-node_4693_piv,float2x2( node_4693_cos, -node_4693_sin, node_4693_sin, node_4693_cos))+node_4693_piv);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_4693, _MainTex));
                float3 emissive = _MainTex_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,_MainTex_var.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
