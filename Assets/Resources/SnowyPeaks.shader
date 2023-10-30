Shader "Custom/SnowyPeaks"
{
    Properties
    {
        _BaseTexture ("Base Texture", 2D) = "white" {}
        _SnowTexture ("Snow Texture", 2D) = "white" {}
        _SnowHeight ("Snow Height", float) = 22
    }
    SubShader
    {
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
                float height : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _BaseTexture;
            sampler2D _SnowTexture;
            float _SnowHeight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.height = v.vertex.y;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 baseColor = tex2D(_BaseTexture, i.uv);
                half4 snowColor = tex2D(_SnowTexture, i.uv);
                float t = smoothstep(_SnowHeight, _SnowHeight + 1.0, i.height);
                return lerp(baseColor, snowColor, t);
            }
            ENDCG
        }
    }
}
