Shader "Unlit/BuildingGrids"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
        _MapGridTex("_MapGridTex", 2D) = "white" {}
        _GridLength("_GridLength", Float) = 120
        _GridWidth("_GridWidth", Float) = 24
        _UseColor("_UseColor",Color) = (1,1,1,1)
        _InterColor("_InterColor",Color) = (1,0,0,0)
        _ClipCol("_ClipCol",int) = 1
        _Alpha("_Alpha",Range(0,1)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        LOD 100
       ZWrite On //ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
       //Offset -1, 3
        Pass
        {
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _MapGridTex;
            float4 _MainTex_ST;
            float _GridWidth, _GridLength;
            fixed4 _UseColor, _InterColor;
            float _Alpha;
            int _ClipCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
              fixed4 gridCol = tex2D(_MapGridTex, i.uv);
              float2 uv = i.uv;
              uv.x = lerp(0, _GridLength, i.uv.x);
              uv.y = lerp(0, _GridWidth, i.uv.y);
              fixed4 col = tex2D(_MainTex, uv);
              int c = floor(uv.x);

             // 空位00，未占用01 占用10, 重叠11，
              float beUsed  = step(0.1, gridCol.g);
              float beInter = step(0.1, gridCol.r);//
              float beEmpty = 1-step(0.1,beUsed + beInter);
              col.a = smoothstep(0.06, 0.2, col.a) *  _Alpha; // 网格线加粗
              col.a = lerp(col.a, 0, step(_ClipCol,c)); //边界剔除
              fixed3 buildingCol = lerp(_UseColor.rgb, _InterColor.rgb, beInter);
              buildingCol = lerp(buildingCol, col.rgb, col.a);
              col.rgb = lerp(col.rgb, buildingCol, beUsed);

              col.a = lerp(col.a, col.a + _UseColor.a,beUsed);
              col.a=lerp(col.a, 0, beEmpty);
              UNITY_APPLY_FOG(i.fogCoord, col);
              return col;
            }
            ENDCG
        }
    }
}
