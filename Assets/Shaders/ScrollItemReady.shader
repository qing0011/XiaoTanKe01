Shader "Hidden/ScrollItemReady"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MulColor("_MulColor",Color) = (1,1,1,1)
        _AddColor("_AddColor",Color) = (1,1,1,1)

      _GrayR("Gray R", Range(0,1)) = 0.493
      _GrayG("Gray G", Range(0,1)) = 0.643
      _GrayB("Gray B", Range(0,1)) = 0.439
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed4 _MulColor, _AddColor;
            float _GrayR;
            float _GrayG;
            float _GrayB;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                //col *= _MulColor;
               /* col.r += lerp(-1, 1, _AddColor.r);
                col.g += lerp(-1, 1, _AddColor.g);
                col.b += lerp(-1, 1, _AddColor.b);*/
                float gray = col.rgb * float3(_GrayR, _GrayG, _GrayB);
                col.rgb = fixed3(gray, gray, gray)* _MulColor.r+col.rgb* _MulColor.g;
                return col;
            }
            ENDCG
        }
    }
}
