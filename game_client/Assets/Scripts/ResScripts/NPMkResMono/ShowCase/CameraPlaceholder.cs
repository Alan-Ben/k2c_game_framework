using System;
using ALPackage;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOE
{
    /// <summary>
    /// 摄像头信息存储对象
    /// </summary>
    [Serializable]
    public class CameraData
    {
        public Vector3 localPosition;
        [FormerlySerializedAs("localEuler")] public Vector3 euler;
        public Vector3 localScale;
        [ALHeader("       Nothing == Don't clear")]
        [ALHeader("       Depth == Depth only")]
        [ALHeader("       Color == Solid Color")]
        [ALHeader("提示：Clear Flags与相机的Clear Flags的对应关系")]
        public CameraClearFlags clearFlags;
        public Color backgroundColor;
        public int cullingMask;
        public bool orthographic;
        public float orthographicSize;
        public float fieldOfView;
        public bool physicalCamera;
        public float nearClipPlane;
        public float farClipPlane;
        public Rect viewportRect;
        public float depth;
        public RenderingPath renderingPath;
        public bool occlusionCulling;
        public bool allowHDR;
        public bool allowMSAA;
        public bool allowDynamicResolution;
        public int targetDisplay;

        //根据摄像头信息保存到数据类
        public void setDataFrom(Camera _camera)
        {
            Transform tempTsf = _camera.transform;
            localPosition = tempTsf.localPosition;
            euler = tempTsf.eulerAngles;
            localScale = tempTsf.localScale;
            
            clearFlags = _camera.clearFlags;
            backgroundColor = _camera.backgroundColor;
            cullingMask = _camera.cullingMask;
            orthographic = _camera.orthographic;
            orthographicSize = _camera.orthographicSize;
            fieldOfView = _camera.fieldOfView;
            physicalCamera = _camera.usePhysicalProperties;
            nearClipPlane = _camera.nearClipPlane;
            farClipPlane = _camera.farClipPlane;
            viewportRect = _camera.rect;
            depth = _camera.depth;
            renderingPath = _camera.renderingPath;
            occlusionCulling = _camera.useOcclusionCulling;
            allowHDR = _camera.allowHDR;
            allowMSAA = _camera.allowMSAA;
            allowDynamicResolution = _camera.allowDynamicResolution;
            targetDisplay = _camera.targetDisplay;
        }
    }
    
    /// <summary>
    /// 可以一键设置当前调好的摄像头信息
    /// </summary>
    public class CameraPlaceholder : MonoBehaviour
    {
        public CameraData cameraData = new CameraData();

        private void Awake()
        {
            Camera camera = GetComponent<Camera>();
            if(camera != null && camera.enabled)
            {
#if UNITY_EDITOR && NP_GAME
                Debug.LogError("Showcase模板上的摄像机没有关闭！请检查：" + transform.parent.name, gameObject);
#endif
                camera.enabled = false;
            }
        }
        
#if UNITY_EDITOR
        [MenuItem("CONTEXT/CameraPlaceholder/设置值")]
        static void SetValueByCamera()
        {
            CameraPlaceholder placeholder = Selection.activeGameObject.GetComponent<CameraPlaceholder>();
            Camera camera = Selection.activeGameObject.GetComponent<Camera>();
            if(placeholder != null && camera != null)
            {
                placeholder.cameraData.setDataFrom(camera);
                camera.enabled = false;
            }
            EditorUtility.SetDirty(placeholder);
        }
#endif
    }
}