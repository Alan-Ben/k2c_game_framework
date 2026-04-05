using UnityEngine;

namespace GOE
{

    /// <summary>
    /// 集市店铺mono
    /// </summary>
    public class GTDMarketPosItemMono : MonoBehaviour
    {
        [ALHeader("集市店铺表的id ")]
        public long marketId;

        [ALHeader("UI跟随的节点")]
        public Transform followParent;

        [ALHeader("功能入口点的点击脚本")]
        public GTDCommonPosClickMono clickMono;

        [ALHeader("加载预制体位置")]
        public Transform loadResPos;
    }




}