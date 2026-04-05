using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会内的席位的位置mono
    /// </summary>
    public class GTDDinnerMainPosItemMono : MonoBehaviour
    {
        [ALHeader("点击脚本")]
        public GTDCommonPosClickMono clickMono;
        [ALHeader("玩家加载父节点")]
        public Transform loadPlayerParent;
        [ALHeader("ui跟随节点")]
        public Transform uiFollowParent;
    }
}