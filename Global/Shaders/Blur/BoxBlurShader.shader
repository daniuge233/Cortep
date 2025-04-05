Shader "Custom/BoxBlurShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurStrength("Blur Strength", Range(0, 10)) = 1.0
        _BlurDirection("Blur Direction", Range(0, 360)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

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

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                float _BlurStrength;
                float _BlurDirection;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                half4 frag(v2f i) : SV_Target
                {
                    half2 blurDir = float2(cos(_BlurDirection), sin(_BlurDirection));
                    half4 sum = tex2D(_MainTex, i.uv) * (1.0 / 9.0); // ԭʼ����
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            half2 offset = half2(j, k) * _BlurStrength * _MainTex_TexelSize.xy;
                            sum += tex2D(_MainTex, i.uv + offset) * (1.0 / 9.0); // ����Ȩ��
                        }
                    }
                    return sum;
                }
                ENDCG
            }
        }
}
