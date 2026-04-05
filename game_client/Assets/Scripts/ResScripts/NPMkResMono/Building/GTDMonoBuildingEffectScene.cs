using UnityEngine;

namespace GOE
{
    public class GTDMonoBuildingEffectScene : MonoBehaviour
    {
        [ALHeader("场景动画")]
        public Animation anim;
        [ALHeader("场景动画名")]
        public string animName;
        [ALHeader("相机运镜终点")]
        public Transform cameraEnd;
        [ALHeader("相机运镜总时长")]
        public float cameraDuration;
    }
}
