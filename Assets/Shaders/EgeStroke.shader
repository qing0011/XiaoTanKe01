Shader "Unlit/EgeStroke"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _Transparent("Transparent Scale", Float) = 0
        _FillColor("FillColor Color", Color) = (0.4338235,0.4377282,1,0)
        _outColor("OutColor Color", Color) = (0.4338235,0.4377282,1,0)
            
        _IntersectionColor("Intersection Color", Color) = (0.4338235,0.4377282,1,0)
        _ClipEdge0("ClipEdge0", Float) = 1
        _ClipEdge("ClipEdge", Float) = 1
        _StrokeScale("_StrokeScale", Float) = 0
        _EdgeScale("_EdgeScale", Float) = 0.01
        _EdgeRim("_EdgeRim", Float) = 0.1
            
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest LEqual
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(2)
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Transparent, _ClipEdge, _ClipEdge0, _StrokeScale,_EdgeScale,_EdgeRim;
            float4 _FillColor, _IntersectionColor, _outColor;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 c = _FillColor;
                float rate = lerp(1+ _StrokeScale, 1, col.a) * step(0.1, col.a);
                float distort = 0;// sin(i.uv.y * 10) * 0.0013;
                float pos  =  _ClipEdge + distort - i.uv.x;
                float pos2 = i.uv.x- (_ClipEdge0+ distort);
                rate += smoothstep(_EdgeScale, 0, pos)*(1+ _EdgeRim);
                rate += smoothstep(_EdgeScale, 0, pos2)*(1+ _EdgeRim);
                rate *= step(i.uv.x, _ClipEdge + distort) * step(_ClipEdge0 + distort, i.uv.x)*step(0.1, col.a);
                float alpha = rate * _Transparent;
                c.rgb = lerp(_IntersectionColor.rgb, _FillColor.rgb, rate * alpha);
                c.rgb = lerp(_outColor, c.rgb, step(0.01, alpha));
                if (alpha < _Transparent)
                {
                    alpha = _outColor.a ;
                }
                c.a = alpha;
                UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(1, 1, 1, 1));
                return c;
            }
            ENDCG
        }
    }
}
