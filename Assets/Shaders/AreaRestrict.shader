Shader "Custom/AreaRestrict"
{
    Properties
    {
        _MainColor("_MainColor",Color) = (1,1,1,1)
        _Shape("_Shape",Int)=0
    }
    SubShader
    {
        // No culling or depth
        Tags { "Queue" = "Transparent+1"}
        ZWrite Off

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ CIRCLE
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent:TANGENT;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
            //float4 _MainColor;
            UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor)
            UNITY_DEFINE_INSTANCED_PROP(int, _Shape)
            UNITY_INSTANCING_BUFFER_END(Props)
            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);                
                o.uv = v.uv;
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                fixed4 col = UNITY_ACCESS_INSTANCED_PROP(Props, _MainColor);
                int shape= UNITY_ACCESS_INSTANCED_PROP(Props, _Shape);
                if (shape == 1)
                {
                    float2 uv = i.uv.xy - float2(0.5, 0.5);
                    if (length(uv) > 0.51) discard;
                    float rate = length(uv) / 0.5;
                    col.a = lerp(1, 0, 1 - (rate * rate));
                }
                else 
                {
                    float2 uv = i.uv.xy - float2(0.5, 0.5);
                    float rx = fmod(uv.x, 0.4);
                    float ry = fmod(uv.y, 0.4);
                    float mx = step(0.4, abs(uv.x));
                    float my = step(0.4, abs(uv.y));
                    float alpha = 1 - mx * my * step(0.1, length(half2(rx, ry)));
                    float rate = length(uv) / 0.5;
                    col.a = lerp(1, 0, 1 - (rate * rate)) * alpha * 0.8;
                }
                return col;
            }
            ENDCG
        }
    }
}
