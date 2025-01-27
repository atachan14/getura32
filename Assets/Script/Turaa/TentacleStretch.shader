Shader "Custom/TentacleStrech"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _StretchMin ("Stretch Min Y", Float) = 0.0 // 引き伸ばし開始位置
        _StretchHeight ("Stretch Height", Float) = 0.5 // 引き伸ばし範囲
        _StretchX ("Stretch Scale X", Float) = 1.7 // x方向の倍率
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _StretchMin; // 引き伸ばし開始位置
            float _StretchHeight; // 引き伸ばしの高さ
            float _StretchX; // x方向の引き伸ばし倍率

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                float y = v.vertex.y; // 頂点のY座標を取得

                // 引き伸ばし範囲内ならX方向にスケール
                if (y >= _StretchMin && y <= (_StretchMin + _StretchHeight))
                {
                    v.vertex.x *= _StretchX; // X方向を引き伸ばし
                }

                o.vertex = UnityObjectToClipPos(v.vertex); // 座標変換
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv); // テクスチャの色をそのまま出力
            }
            ENDCG
        }
    }
}
