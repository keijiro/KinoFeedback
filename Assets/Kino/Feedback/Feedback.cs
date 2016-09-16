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
using UnityEngine;
using UnityEngine.Rendering;

namespace Kino
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class Feedback : MonoBehaviour
    {
        #region Editable properties

        /// Feedback color
        public Color color {
            get { return _color; }
            set { _color = value; }
        }

        [SerializeField]
        Color _color = Color.white;

        /// Horizontal offset for feedback
        public float offsetX {
            get { return _offsetX; }
            set { _offsetX = value; }
        }

        [SerializeField, Range(-1, 1)]
        float _offsetX = 0;

        /// Vertical offset for feedback
        public float offsetY {
            get { return _offsetY; }
            set { _offsetY = value; }
        }

        [SerializeField, Range(-1, 1)]
        float _offsetY = 0;

        /// Center-axis rotation for feedback
        public float rotation {
            get { return _rotation; }
            set { _rotation = value; }
        }

        [SerializeField, Range(-5, 5)]
        float _rotation = 0;

        /// Scale factor for feedback
        public float scale {
            get { return _scale; }
            set { _scale = value; }
        }

        [SerializeField, Range(0.95f, 1.05f)]
        float _scale = 1;

        /// Disables bilinear filter
        public bool jaggies {
            get { return _jaggies; }
            set { _jaggies = value; }
        }

        [SerializeField]
        bool _jaggies = false;

        #endregion

        #region Private members

        [SerializeField] Shader _shader;

        Material _material;
        RenderTexture _delayBuffer;

        // 2d rotation matrix
        Vector4 rotationMatrixAsVector {
            get {
                var angle = -Mathf.Deg2Rad * _rotation;
                var sin = Mathf.Sin(angle);
                var cos = Mathf.Cos(angle);
                return new Vector4(cos, sin, -sin, cos);
            }
        }

        #endregion

        #region MonoBehaviour functions

        void OnEnable()
        {
            GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
        }

        void OnDestroy()
        {
            if (_delayBuffer != null)
                RenderTexture.ReleaseTemporary(_delayBuffer);

            if (_material != null)
                if (Application.isPlaying)
                    Destroy(_material);
                else
                    DestroyImmediate(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Lazy initialization of the postprocessing material
            if (_material == null) {
                _material = new Material(Shader.Find("Hidden/Kino/Feedback"));
                _material.hideFlags = HideFlags.HideAndDontSave;
            }

            // Feedback from the previous frame (note: this could be null).
            if (_delayBuffer != null)
                _delayBuffer.filterMode = _jaggies ? FilterMode.Point : FilterMode.Bilinear;
            _material.SetTexture("_FeedbackTex", _delayBuffer);

            // Filter parameters
            _material.SetColor("_Color", _color);
            _material.SetVector("_Offset", new Vector2(_offsetX, _offsetY) * -0.05f);
            _material.SetVector("_Rotation", rotationMatrixAsVector);
            _material.SetFloat("_Scale", 2 - _scale);

            // Apply the filter and get the result in a newly allocated RT.
            var tempRT = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, tempRT, _material, 0);

            // Replace the delay buffer with the filter result.
            if (_delayBuffer != null)
                RenderTexture.ReleaseTemporary(_delayBuffer);
            _delayBuffer = tempRT;

            // Copy the result to the destination.
            Graphics.Blit(tempRT, destination);
        }

        #endregion
    }
}
