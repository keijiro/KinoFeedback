Shader "Unlit/Feedback"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"{}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float2 _MainTex_TexelSize;

    half4 _Color;
    float2 _Offset;
    float4 _Rotation;
    float _Scale;

    v2f_img vert_feedback(appdata_img v)
    {
        v2f_img o;
        float4 pos = mul(UNITY_MATRIX_MVP, v.vertex);
        pos.z = 1;
        o.pos = pos;
        o.uv = v.texcoord;
        return o;
    }

    half4 frag_feedback(v2f_img i) : SV_Target
    {
        float2 uv = (i.uv - 0.5) * _Scale;
        uv = float2(dot(uv, _Rotation.xy), dot(uv, _Rotation.zw));
        uv += 0.5 + _Offset;
        half4 col = tex2D(_MainTex, uv);
        return col * _Color;
    }

    ENDCG

    SubShader
    {
        Pass
        {
            ZTest LEqual
            Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_feedback
            #pragma fragment frag_feedback
            ENDCG
        }
    }
}
