using UnityEngine;

namespace GOE
{
    public class GTDTravelMainPosItemInfo : MonoBehaviour
    {
        [ALHeader("加载父节点")]
        public Transform loadParent;

        [ALHeader("地点id")]
        public long posId;
    }
}