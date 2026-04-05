using UnityEngine;

namespace GOE
{
    public class GTDMonoStageGoalBuilding : MonoBehaviour
    {
        [ALHeader("名字的跟随目标")]
        public Transform nameHudTarget;
        [ALHeader("点击进入的对象")]
        public GTDCommonPosClickMono clickMono;
        [ALHeader("聚焦到建筑时所需的时间")]
        public float focusFadeTime = 0.5f;
    }
}