using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP页签列表item
    /// </summary>
    public class GGUIMonoVIPTabContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("页签图标")]
        public RawImage imgIcon;
        [ALHeader("页签背景")]
        public RawImage imgBg;
        [ALHeader("VIP等级")]
        public Text txtVIPLevel;
        [ALHeader("页签配置")]
        public NPGGUIMonoCommonTab monoTab;
        [ALHeader("未选中时文字颜色")]
        public Color unSelectTextColor = Color.black;
        [ALHeader("选中时文字颜色")]
        public Color selectTextColor = Color.yellow;
    }
}
