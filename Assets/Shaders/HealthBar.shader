Shader "HealthBarShader/HealthBar"
{
    Properties
    {
        _Health("Health", Range(0, 1)) = 0
        _LowColor("Low Health Color", Color) = (1, 0, 0, 1)
        _HighColor("High Health Color", Color) = (0, 1, 0, 1)

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
/*
            // make fog work
            #pragma multi_compile_fog
*/

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
            //    UNITY_FOG_COORDS(1)
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
/*
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
*/
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                if (i.uv.x > _Health)
                {
                    return 1;
                }
                return lerp(_LowColor, _HighColor, _Health);
/*
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
*/
            }
            ENDCG
        }
    }
}
