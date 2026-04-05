using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 伙伴替换藏品列表item
    /// </summary>
    public class GGUIMonoHeroEquipChangeGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("藏品item")]
        public GGUIMonoEquipCommonItem monoEquipItem;
        [ALHeader("实力加成值")]
        public Text txtPower;
        [ALHeader("佩戴按钮")]
        public GameObject btnWear;
        [ALHeader("替换按钮")]
        public GameObject btnReplace;
        [ALHeader("未佩戴或者是更高品质可佩戴红点")]
        public GameObject goWearRedTip;
    }
}