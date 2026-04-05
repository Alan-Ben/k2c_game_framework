using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 对话历史回顾选项窗口
    /// </summary>
    public class GGUIMonoDialogueHistoryOption : _AALBasicUIWndMono
    {
        [ALHeader("名字")]
        public Text txtName;
        [ALHeader("选项列表")]
        public GGUIMonoDialogueHistoryOptionContainer monoOptionContainer;
    }
}

