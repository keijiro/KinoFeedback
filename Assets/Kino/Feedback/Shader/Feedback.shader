//
// Kino/Feedback - framebuffer feedback effect for Unity
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
Shader "Hidden/Kino/Feedback"
{
    Properties
    {
        _MainTex("", 2D) = "black"{}
        _FeedbackTex("", 2D) = "black"{}
        _Color("", Color) = (1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D_float _CameraDepthTexture;

    sampler2D _MainTex;
    sampler2D _FeedbackTex;

    half4 _Color;
    float2 _Offset;
    float4 _Rotation;
    float _Scale;

    fixed4 frag_feedback(v2f_img i) : SV_Target
    {
        float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);

        #if defined(UNITY_REVERSED_Z)
        fixed sel = d < 0.0001;
        #else
        fixed sel = d > 0.9999;
        #endif

        float2 uv = (i.uv - 0.5) * _Scale;
        uv = float2(dot(uv, _Rotation.xy), dot(uv, _Rotation.zw));
        uv += 0.5 + _Offset;

        fixed4 sc = tex2D(_MainTex, i.uv);
        fixed4 fb = tex2D(_FeedbackTex, uv) * _Color;

        return lerp(sc, fb, sel);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag_feedback
            #pragma target 3.0
            ENDCG
        }
    }
}
