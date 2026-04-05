using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 骑士推荐入口点的Mono
    /// </summary>
    public class GTDHeroRecommendPointMono : MonoBehaviour
    {
        [ALHeader("队列ID ")]
        public int queueId;
        [ALHeader("GO加载的父节点")]
        public Transform goParent;
        [ALHeader("UI跟随的节点")]
        public Transform followParent;
        [ALHeader("功能入口点的点击脚本")]
        public GTDHomeEntryPointClickMono clickMono;
    }
}