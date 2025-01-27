Shader "Custom/TentacleStrech"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _StretchMin ("Stretch Min Y", Float) = 0.0 // ˆø‚«L‚Î‚µŠJŽnˆÊ’u
        _StretchHeight ("Stretch Height", Float) = 0.5 // ˆø‚«L‚Î‚µ”ÍˆÍ
        _StretchX ("Stretch Scale X", Float) = 1.7 // x•ûŒü‚Ì”{—¦
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
            float _StretchMin; // ˆø‚«L‚Î‚µŠJŽnˆÊ’u
            float _StretchHeight; // ˆø‚«L‚Î‚µ‚Ì‚‚³
            float _StretchX; // x•ûŒü‚Ìˆø‚«L‚Î‚µ”{—¦

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
                float y = v.vertex.y; // ’¸“_‚ÌYÀ•W‚ðŽæ“¾

                // ˆø‚«L‚Î‚µ”ÍˆÍ“à‚È‚çX•ûŒü‚ÉƒXƒP[ƒ‹
                if (y >= _StretchMin && y <= (_StretchMin + _StretchHeight))
                {
                    v.vertex.x *= _StretchX; // X•ûŒü‚ðˆø‚«L‚Î‚µ
                }

                o.vertex = UnityObjectToClipPos(v.vertex); // À•W•ÏŠ·
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv); // ƒeƒNƒXƒ`ƒƒ‚ÌF‚ð‚»‚Ì‚Ü‚Üo—Í
            }
            ENDCG
        }
    }
}
