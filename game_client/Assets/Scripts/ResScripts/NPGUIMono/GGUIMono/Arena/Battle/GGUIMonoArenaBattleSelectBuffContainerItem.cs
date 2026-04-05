using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗选择增益道具列表item
    /// </summary>
    public class GGUIMonoArenaBattleSelectBuffContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("道具名称")]
        public Text txtItemName;
        [ALHeader("道具图标")]
        public RawImage imgItemIcon;
        [ALHeader("道具加成描述")]
        public Text txtItemDesc;
        [ALHeader("购买按钮")]
        public GameObject btnBuy;
        [ALHeader("购买消耗道具")]
        public NPGGUIMonoCommonItem monoCostItem;
    }
}
