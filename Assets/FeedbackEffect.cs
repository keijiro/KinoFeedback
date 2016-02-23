using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class FeedbackEffect : MonoBehaviour
{
    #region Editable properties

    [SerializeField]
    bool _jaggies = false;

    [SerializeField]
    Color _color = Color.white;

    [SerializeField, Range(-1, 1)]
    float _offsetX = 0;

    [SerializeField, Range(-1, 1)]
    float _offsetY = 0;

    [SerializeField, Range(-5, 5)]
    float _rotation = 0;

    [SerializeField, Range(0.95f, 1.05f)]
    float _scale = 1;

    #endregion

    #region Private members

    [SerializeField] Shader _shader;

    // shader reference
    Shader shader {
        get {
            const string name = "Hidden/FeedbackEffect";
            return _shader ? _shader : Shader.Find(name);
        }
    }

    // assigned camera
    Camera assignedCamera {
        get { return GetComponent<Camera>(); }
    }

    Material _material;
    RenderTexture _previousFrame;
    CommandBuffer _commandBuffer;

    void SetUpResources()
    {
        var camera = assignedCamera;

        if (_material == null)
        {
            _material = new Material(shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        if (_previousFrame == null)
        {
            _previousFrame = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, 0, RenderTextureFormat.DefaultHDR);
        }

        if (_commandBuffer == null)
        {
            _commandBuffer = new CommandBuffer();
            _commandBuffer.name = "FeedbackEffect";

            _commandBuffer.Blit(_previousFrame as Texture, BuiltinRenderTextureType.CameraTarget, _material, 0);

            camera.AddCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
        }
    }

    #endregion

    void OnDisable()
    {
        if (_commandBuffer != null)
            assignedCamera.RemoveCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
        _commandBuffer = null;

        RenderTexture.ReleaseTemporary(_previousFrame);
        _previousFrame = null;

        if (_material != null) DestroyImmediate(_material);
        _material = null;
    }

    void Update()
    {
        var camera = assignedCamera;

        if (_previousFrame != null)
        {
            if (_previousFrame.width != camera.pixelWidth ||
                _previousFrame.height != camera.pixelHeight)
            {
                RenderTexture.ReleaseTemporary(_previousFrame);
                _previousFrame = null;
            }
        }

        if (_material != null)
        {
            _material.SetColor("_Color", _color);

            var offset = new Vector2(_offsetX, _offsetY) * -0.05f;
            _material.SetVector("_Offset", offset);

            var angle = -Mathf.Deg2Rad * _rotation;
            var sin = Mathf.Sin(angle);
            var cos = Mathf.Cos(angle);
            var rotation = new Vector4(cos, sin, -sin, cos);
            _material.SetVector("_Rotation", rotation);

            _material.SetFloat("_Scale", 2 - _scale);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUpResources();

        _previousFrame.filterMode = _jaggies ? FilterMode.Point : FilterMode.Bilinear;

        Graphics.Blit(source, _previousFrame);
        Graphics.Blit(source, destination);
    }
}
