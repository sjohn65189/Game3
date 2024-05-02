Shader "HealthBarShader/HealthBar"
{
    Properties
    {
        _Health("Health", Range(0, 1)) = 0
        _LowColor("Low Health Color", Color) = (1, 0, 0, 1)
        _HighColor("High Health Color", Color) = (0, 1, 0, 1)
        _MainTex("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Health;
            fixed4 _LowColor;
            fixed4 _HighColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.x > _Health)
                {
                //    return fixed4(1, 1, 1, 1);
                    discard;
                }
                return lerp(_LowColor, _HighColor, _Health);
            }
            ENDCG
        }
    }
}
