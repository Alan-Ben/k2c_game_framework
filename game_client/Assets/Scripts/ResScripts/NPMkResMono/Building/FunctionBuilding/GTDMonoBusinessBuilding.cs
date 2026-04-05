using UnityEngine;

namespace GOE
{
    public class GTDMonoBusinessBuilding : MonoBehaviour
    {
        [ALHeader("名字的跟随目标")]
        public Transform nameHudTarget;
        [ALHeader("上飘收益的跟随目标")]
        public Transform earningsHudTarget;
        [ALHeader("点击进入的对象")]
        public GTDCommonPosClickMono clickMono;
    }
}