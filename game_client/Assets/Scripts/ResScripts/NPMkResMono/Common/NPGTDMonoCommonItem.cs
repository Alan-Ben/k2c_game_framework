using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个通用的 3d 物品 mono
    /// </summary>
    public class NPGTDMonoCommonItem : MonoBehaviour
    {
        [ALHeader("显示物品图标的 renderer")]
        public Renderer renderer;
        [ALHeader("品质特效父节点")]
        public Transform qualitySfxParent;
    }
}