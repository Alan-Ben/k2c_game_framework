using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 冲榜页签列表item
    /// </summary>
    public class GGUIMonoRankRushMultipleDetailTabContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("冲榜名称")]
        public Text txtName;
        [ALHeader("页签配置")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("未选中时文字颜色")]
        public Color unSelectTextColor = Color.black;
        [ALHeader("选中时文字颜色")]
        public Color selectTextColor = Color.yellow;
    }
}
