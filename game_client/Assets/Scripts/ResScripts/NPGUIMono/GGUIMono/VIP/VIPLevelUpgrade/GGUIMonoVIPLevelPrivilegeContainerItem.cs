using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// VIP等级特权描述列表item
    /// </summary>
    public class GGUIMonoVIPLevelPrivilegeContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("上个值")]
        public Text txtLastValue;
        [ALHeader("当前值")]
        public Text txtCurValue;
    }
}
