// Shader created with Shader Forge v1.38 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33111,y:33469,varname:node_9361,prsc:2|emission-2331-OUT,custl-2791-OUT,alpha-7460-OUT,voffset-1148-OUT;n:type:ShaderForge.SFN_Tex2d,id:3192,x:32420,y:33434,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_node_3192,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6282-OUT;n:type:ShaderForge.SFN_Color,id:5450,x:32420,y:33631,ptovrint:False,ptlb:Color,ptin:_Color,varname:_node_5450,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_VertexColor,id:6470,x:32420,y:33808,varname:node_6470,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2791,x:32730,y:33533,varname:node_2791,prsc:2|A-3192-RGB,B-5450-RGB,C-6470-RGB;n:type:ShaderForge.SFN_Multiply,id:7460,x:32730,y:33722,varname:node_7460,prsc:2|A-3192-A,B-5450-A,C-6470-A,D-1622-R,E-4611-OUT;n:type:ShaderForge.SFN_Time,id:8739,x:31228,y:33157,varname:node_8739,prsc:2;n:type:ShaderForge.SFN_Add,id:3019,x:31867,y:33296,varname:node_3019,prsc:2|A-1155-OUT,B-7763-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:2161,x:31228,y:33055,ptovrint:False,ptlb:MainTex_U_Speed,ptin:_MainTex_U_Speed,varname:_node_2161,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9179,x:31228,y:33345,ptovrint:False,ptlb:MainTex_V_Speed,ptin:_MainTex_V_Speed,varname:_node_9179,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:4885,x:31471,y:33042,varname:node_4885,prsc:2|A-2161-OUT,B-8739-T;n:type:ShaderForge.SFN_Multiply,id:2727,x:31471,y:33270,varname:node_2727,prsc:2|A-8739-T,B-9179-OUT;n:type:ShaderForge.SFN_Append,id:1155,x:31653,y:33149,varname:node_1155,prsc:2|A-4885-OUT,B-2727-OUT;n:type:ShaderForge.SFN_Tex2d,id:1622,x:32420,y:33981,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:_node_3192_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_TexCoord,id:7763,x:31657,y:33389,varname:node_7763,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:7684,x:31478,y:33601,ptovrint:False,ptlb:UVDistortionTex,ptin:_UVDistortionTex,varname:node_7684,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c193e00489518a540a7661ddb8b18a37,ntxv:0,isnm:False|UVIN-3335-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7872,x:31659,y:33790,ptovrint:False,ptlb:UVDistortionIntensity,ptin:_UVDistortionIntensity,varname:node_7872,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Multiply,id:6967,x:31866,y:33601,varname:node_6967,prsc:2|A-3106-OUT,B-7872-OUT;n:type:ShaderForge.SFN_Add,id:6282,x:32184,y:33434,varname:node_6282,prsc:2|A-3019-OUT,B-6967-OUT;n:type:ShaderForge.SFN_TexCoord,id:7539,x:31080,y:33697,varname:node_7539,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:4748,x:30655,y:33510,varname:node_4748,prsc:2;n:type:ShaderForge.SFN_Add,id:3335,x:31283,y:33601,varname:node_3335,prsc:2|A-5058-OUT,B-7539-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:2406,x:30655,y:33408,ptovrint:False,ptlb:UVDistortionTex_U_Speed,ptin:_UVDistortionTex_U_Speed,varname:_U_Speed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9677,x:30655,y:33698,ptovrint:False,ptlb:UVDistortionTex_V_Speed,ptin:_UVDistortionTex_V_Speed,varname:_V_Speed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:1120,x:30898,y:33395,varname:node_1120,prsc:2|A-2406-OUT,B-4748-T;n:type:ShaderForge.SFN_Multiply,id:232,x:30898,y:33623,varname:node_232,prsc:2|A-4748-T,B-9677-OUT;n:type:ShaderForge.SFN_Append,id:5058,x:31080,y:33502,varname:node_5058,prsc:2|A-1120-OUT,B-232-OUT;n:type:ShaderForge.SFN_ComponentMask,id:3106,x:31659,y:33601,varname:node_3106,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7684-RGB;n:type:ShaderForge.SFN_Tex2d,id:352,x:31844,y:34612,ptovrint:False,ptlb:VertexMoveTex,ptin:_VertexMoveTex,varname:node_352,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-5195-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8756,x:31844,y:34849,ptovrint:False,ptlb:R_Multiplier,ptin:_R_Multiplier,varname:node_8756,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3237,x:32158,y:34633,varname:node_3237,prsc:2|A-352-R,B-8756-OUT;n:type:ShaderForge.SFN_ValueProperty,id:97,x:31844,y:34955,ptovrint:False,ptlb:G_Multiplier,ptin:_G_Multiplier,varname:_node_8756_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:7225,x:32158,y:34802,varname:node_7225,prsc:2|A-352-G,B-97-OUT;n:type:ShaderForge.SFN_Multiply,id:535,x:32158,y:34967,varname:node_535,prsc:2|A-352-B,B-8499-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8499,x:31844,y:35062,ptovrint:False,ptlb:B_Multiplier,ptin:_B_Multiplier,varname:_node_8756_copy_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Append,id:1148,x:32436,y:34687,varname:node_1148,prsc:2|A-3237-OUT,B-7225-OUT,C-535-OUT;n:type:ShaderForge.SFN_Time,id:5843,x:30951,y:34473,varname:node_5843,prsc:2;n:type:ShaderForge.SFN_Add,id:5195,x:31590,y:34612,varname:node_5195,prsc:2|A-3428-OUT,B-6614-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:7759,x:30951,y:34371,ptovrint:False,ptlb:VertexMoveTex_U_Speed,ptin:_VertexMoveTex_U_Speed,varname:_MainTex_U_Speed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:2782,x:30951,y:34661,ptovrint:False,ptlb:VertexMoveTex_V_Speed,ptin:_VertexMoveTex_V_Speed,varname:_MainTex_V_Speed_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:5498,x:31194,y:34358,varname:node_5498,prsc:2|A-7759-OUT,B-5843-T;n:type:ShaderForge.SFN_Multiply,id:4396,x:31194,y:34586,varname:node_4396,prsc:2|A-5843-T,B-2782-OUT;n:type:ShaderForge.SFN_Append,id:3428,x:31376,y:34465,varname:node_3428,prsc:2|A-5498-OUT,B-4396-OUT;n:type:ShaderForge.SFN_TexCoord,id:6614,x:31380,y:34705,varname:node_6614,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:8306,x:32420,y:32885,ptovrint:False,ptlb:EmissionTex,ptin:_EmissionTex,varname:node_8306,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-7867-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9192,x:32420,y:33298,ptovrint:False,ptlb:EmissionIntensity,ptin:_EmissionIntensity,varname:node_9192,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:2331,x:32731,y:33168,varname:node_2331,prsc:2|A-8306-RGB,B-6746-RGB,C-9192-OUT;n:type:ShaderForge.SFN_Color,id:6746,x:32420,y:33086,ptovrint:False,ptlb:EmissionColor,ptin:_EmissionColor,varname:node_6746,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Fresnel,id:4679,x:31817,y:34335,varname:node_4679,prsc:2|EXP-7533-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7533,x:31638,y:34355,ptovrint:False,ptlb:FresnelExp,ptin:_FresnelExp,varname:node_7533,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_OneMinus,id:3722,x:32202,y:34335,varname:node_3722,prsc:2|IN-7952-OUT;n:type:ShaderForge.SFN_Clamp01,id:7952,x:32011,y:34335,varname:node_7952,prsc:2|IN-4679-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:4611,x:32420,y:34219,ptovrint:False,ptlb:Frensel,ptin:_Frensel,varname:node_4611,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-1916-OUT,B-3722-OUT;n:type:ShaderForge.SFN_Vector1,id:1916,x:32202,y:34219,varname:node_1916,prsc:2,v1:1;n:type:ShaderForge.SFN_Time,id:131,x:31575,y:32746,varname:node_131,prsc:2;n:type:ShaderForge.SFN_Add,id:7867,x:32214,y:32885,varname:node_7867,prsc:2|A-8476-OUT,B-7563-UVOUT;n:type:ShaderForge.SFN_Multiply,id:1412,x:31818,y:32631,varname:node_1412,prsc:2|A-6932-OUT,B-131-T;n:type:ShaderForge.SFN_Multiply,id:5944,x:31818,y:32859,varname:node_5944,prsc:2|A-131-T,B-7186-OUT;n:type:ShaderForge.SFN_Append,id:8476,x:32000,y:32738,varname:node_8476,prsc:2|A-1412-OUT,B-5944-OUT;n:type:ShaderForge.SFN_TexCoord,id:7563,x:32004,y:32978,varname:node_7563,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_ValueProperty,id:6932,x:31575,y:32647,ptovrint:False,ptlb:EmissionTex_U_Speed,ptin:_EmissionTex_U_Speed,varname:node_6932,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:7186,x:31575,y:32927,ptovrint:False,ptlb:EmissionTex_V_Speed,ptin:_EmissionTex_V_Speed,varname:node_7186,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:5450-3192-2161-9179-1622-7684-7872-2406-9677-352-8756-97-8499-7759-2782-8306-6746-9192-6932-7186-4611-7533;pass:END;sub:END;*/

Shader "Shader Forge/Blend_VertexMove" {
    Properties {
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTex_U_Speed ("MainTex_U_Speed", Float ) = 0
        _MainTex_V_Speed ("MainTex_V_Speed", Float ) = 0
        _Mask ("Mask", 2D) = "white" {}
        _UVDistortionTex ("UVDistortionTex", 2D) = "white" {}
        _UVDistortionIntensity ("UVDistortionIntensity", Float ) = 0.2
        _UVDistortionTex_U_Speed ("UVDistortionTex_U_Speed", Float ) = 0
        _UVDistortionTex_V_Speed ("UVDistortionTex_V_Speed", Float ) = 0
        _VertexMoveTex ("VertexMoveTex", 2D) = "white" {}
        _R_Multiplier ("R_Multiplier", Float ) = 0
        _G_Multiplier ("G_Multiplier", Float ) = 0
        _B_Multiplier ("B_Multiplier", Float ) = 0
        _VertexMoveTex_U_Speed ("VertexMoveTex_U_Speed", Float ) = 0
        _VertexMoveTex_V_Speed ("VertexMoveTex_V_Speed", Float ) = 0
        _EmissionTex ("EmissionTex", 2D) = "white" {}
        _EmissionColor ("EmissionColor", Color) = (0.5,0.5,0.5,1)
        _EmissionIntensity ("EmissionIntensity", Float ) = 0
        _EmissionTex_U_Speed ("EmissionTex_U_Speed", Float ) = 0
        _EmissionTex_V_Speed ("EmissionTex_V_Speed", Float ) = 0
        [MaterialToggle] _Frensel ("Frensel", Float ) = 1
        _FresnelExp ("FresnelExp", Float ) = 0
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
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float _MainTex_U_Speed;
            uniform float _MainTex_V_Speed;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _UVDistortionTex; uniform float4 _UVDistortionTex_ST;
            uniform float _UVDistortionIntensity;
            uniform float _UVDistortionTex_U_Speed;
            uniform float _UVDistortionTex_V_Speed;
            uniform sampler2D _VertexMoveTex; uniform float4 _VertexMoveTex_ST;
            uniform float _R_Multiplier;
            uniform float _G_Multiplier;
            uniform float _B_Multiplier;
            uniform float _VertexMoveTex_U_Speed;
            uniform float _VertexMoveTex_V_Speed;
            uniform sampler2D _EmissionTex; uniform float4 _EmissionTex_ST;
            uniform float _EmissionIntensity;
            uniform float4 _EmissionColor;
            uniform float _FresnelExp;
            uniform fixed _Frensel;
            uniform float _EmissionTex_U_Speed;
            uniform float _EmissionTex_V_Speed;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_5843 = _Time;
                float2 node_5195 = (float2((_VertexMoveTex_U_Speed*node_5843.g),(node_5843.g*_VertexMoveTex_V_Speed))+o.uv0);
                float4 _VertexMoveTex_var = tex2Dlod(_VertexMoveTex,float4(TRANSFORM_TEX(node_5195, _VertexMoveTex),0.0,0));
                v.vertex.xyz += float3((_VertexMoveTex_var.r*_R_Multiplier),(_VertexMoveTex_var.g*_G_Multiplier),(_VertexMoveTex_var.b*_B_Multiplier));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_131 = _Time;
                float2 node_7867 = (float2((_EmissionTex_U_Speed*node_131.g),(node_131.g*_EmissionTex_V_Speed))+i.uv0);
                float4 _EmissionTex_var = tex2D(_EmissionTex,TRANSFORM_TEX(node_7867, _EmissionTex));
                float3 emissive = (_EmissionTex_var.rgb*_EmissionColor.rgb*_EmissionIntensity);
                float4 node_8739 = _Time;
                float4 node_4748 = _Time;
                float2 node_3335 = (float2((_UVDistortionTex_U_Speed*node_4748.g),(node_4748.g*_UVDistortionTex_V_Speed))+i.uv0);
                float4 _UVDistortionTex_var = tex2D(_UVDistortionTex,TRANSFORM_TEX(node_3335, _UVDistortionTex));
                float2 node_6282 = ((float2((_MainTex_U_Speed*node_8739.g),(node_8739.g*_MainTex_V_Speed))+i.uv0)+(_UVDistortionTex_var.rgb.rg*_UVDistortionIntensity));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_6282, _MainTex));
                float3 finalColor = emissive + (_MainTex_var.rgb*_Color.rgb*i.vertexColor.rgb);
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                return fixed4(finalColor,(_MainTex_var.a*_Color.a*i.vertexColor.a*_Mask_var.r*lerp( 1.0, (1.0 - saturate(pow(1.0-max(0,dot(normalDirection, viewDirection)),_FresnelExp))), _Frensel )));
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal 
            #pragma target 3.0
            uniform sampler2D _VertexMoveTex; uniform float4 _VertexMoveTex_ST;
            uniform float _R_Multiplier;
            uniform float _G_Multiplier;
            uniform float _B_Multiplier;
            uniform float _VertexMoveTex_U_Speed;
            uniform float _VertexMoveTex_V_Speed;
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
                float4 node_5843 = _Time;
                float2 node_5195 = (float2((_VertexMoveTex_U_Speed*node_5843.g),(node_5843.g*_VertexMoveTex_V_Speed))+o.uv0);
                float4 _VertexMoveTex_var = tex2Dlod(_VertexMoveTex,float4(TRANSFORM_TEX(node_5195, _VertexMoveTex),0.0,0));
                v.vertex.xyz += float3((_VertexMoveTex_var.r*_R_Multiplier),(_VertexMoveTex_var.g*_G_Multiplier),(_VertexMoveTex_var.b*_B_Multiplier));
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
