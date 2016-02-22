Shader "Unlit/Feedback"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"{}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Offset("Offset", Vector) = (0, 0, 0, 0)
        _Scale("Scale", Vector) = (1, 1, 1, 1)
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            half4 _Color;
            float2 _Offset;
            float2 _Scale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                float2 uv = o.vertex.xy / o.vertex.w;
                o.uv = (uv * _Scale + 1) * 0.5 + _Offset;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }

            ENDCG
        }
    }
}
