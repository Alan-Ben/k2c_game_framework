using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 本地推送设置列表item
    /// </summary>
    public class GGUIMonoGameSettingLocalPushContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("开关名")]
        public Text txtName;
        [ALHeader("开关")]
        public NPGGUIMonoCommonTab monoMainSwitch;
    }
}
