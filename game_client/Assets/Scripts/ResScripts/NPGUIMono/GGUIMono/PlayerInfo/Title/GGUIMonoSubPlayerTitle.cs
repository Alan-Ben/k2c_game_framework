using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家称号附加窗口
    /// </summary>
    public class GGUIMonoSubPlayerTitle : _AALBasicUIWndMono
    {
        [ALHeader("底框或者固定称号加载父节点")]
        public Transform transParent;
        [ALHeader("组合称号文本")]
        public Text txtComboTitle;
    }
}
