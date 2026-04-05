using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GTDMonoMarsExploreEventBase : MonoBehaviour
    {
        [ALHeader("跟随目标")]
        public Transform followTarget;
        [ALHeader("点击对象")]
        public GTDCommonPosClickMono monoClick;
    }
}
