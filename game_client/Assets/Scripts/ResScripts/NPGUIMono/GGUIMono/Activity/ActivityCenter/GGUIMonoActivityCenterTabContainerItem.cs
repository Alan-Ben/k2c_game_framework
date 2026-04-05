using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 活动中心页签列表item
    /// </summary>
    public class GGUIMonoActivityCenterTabContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("页签名称")]
        public Text txtTabName;
        [ALHeader("页签名称2")]
        public Text txtTabName2;
        [ALHeader("页签图标")]
        public RawImage imgIcon;
        [ALHeader("页签配置")]
        public NPGGUIMonoCommonTab monoTab;
    }
}
