using UnityEngine;

namespace GOE
{
    public class GTDMonoAnecdoteEvent : MonoBehaviour
    {
        [ALHeader("Hud 的跟随目标")]
        public Transform hudTarget;
        [ALHeader("点击按钮")]
        public GTDCommonPosClickMono clickMono;
    }
}