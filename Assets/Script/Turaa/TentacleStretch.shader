Shader "Custom/TentacleStrech"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _StretchMin ("Stretch Min Y", Float) = 0.0 // �����L�΂��J�n�ʒu
        _StretchHeight ("Stretch Height", Float) = 0.5 // �����L�΂��͈�
        _StretchX ("Stretch Scale X", Float) = 1.7 // x�����̔{��
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
            float _StretchMin; // �����L�΂��J�n�ʒu
            float _StretchHeight; // �����L�΂��̍���
            float _StretchX; // x�����̈����L�΂��{��

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
                float y = v.vertex.y; // ���_��Y���W���擾

                // �����L�΂��͈͓��Ȃ�X�����ɃX�P�[��
                if (y >= _StretchMin && y <= (_StretchMin + _StretchHeight))
                {
                    v.vertex.x *= _StretchX; // X�����������L�΂�
                }

                o.vertex = UnityObjectToClipPos(v.vertex); // ���W�ϊ�
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv); // �e�N�X�`���̐F�����̂܂܏o��
            }
            ENDCG
        }
    }
}
