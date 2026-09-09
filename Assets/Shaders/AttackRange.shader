Shader "Custom/AttackRange"
{
    Properties
    {
        _MainColor("_MainColor",Color) = (1,1,1,1)
        _minRange("_minRange",Range(0,1)) = 0.0
        _SectorAngle("_SectorAngle",Range(0,360)) = 360
        _SectorDir("_SectorDir",Range(0,360)) = 90

    }
    SubShader
    {
        // No culling or depth
        Tags { "Queue" = "Transparent+1"}
        ZWrite Off
        //ZTest always

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _MainColor;
            float _minRange = 0;
            float _SectorAngle = 0;
            float _SectorDir = 0;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _MainColor;
                float2 uv = i.uv.xy - float2(0.5,0.5);
                float dis = length(uv);
                float2 dir = normalize(uv);

                float rad = _SectorDir * 0.0174532924;
                float2 initDir = float2(cos(rad), sin(rad));
                float dt = dot(dir, initDir);
                float tardt = cos(_SectorAngle * 0.5 * 0.0174532924);
                float t = step(dis, 0.5)*step(_minRange, dis);                
                t = step(tardt, dt) * t;
                float st = smoothstep(0.4, 0.60, dis)
                    + smoothstep(_minRange+0.05, _minRange - 0.1, dis)
                    + smoothstep(tardt + 0.05, tardt - 0.1, dt) * step(-0.9, dt);
                col.a = (st+0.01) * t;//(1 - step(0.5, dis));
                return col;
            }
            ENDCG
        }
    }
}
