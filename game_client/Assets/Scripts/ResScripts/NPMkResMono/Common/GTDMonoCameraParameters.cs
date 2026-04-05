using UnityEngine;

namespace GOE
{
    [ExecuteInEditMode]
    public class GTDMonoCameraParameters : MonoBehaviour
    {
        [ALHeader("相机的相关参数")]
        public Vector3 cameraPos;
        public Vector3 cameraFocusPos;
        public float cameraFieldOfView;
        public float cameraOrthographicSize;
        
        [ALHeader("预览用的相机")]
        public Camera previewCamera;

#if UNITY_EDITOR
        public void Update()
        {
            if (previewCamera != null)
            {
                previewCamera.transform.position = cameraPos;
                previewCamera.transform.LookAt(cameraFocusPos);
                previewCamera.fieldOfView = cameraFieldOfView;
                previewCamera.orthographicSize = cameraOrthographicSize;
            }   
        }
#endif
    }
}