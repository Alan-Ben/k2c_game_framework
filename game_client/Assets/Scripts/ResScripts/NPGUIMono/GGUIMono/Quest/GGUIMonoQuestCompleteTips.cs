using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 任务完成提示界面
    /// </summary>
    public class GGUIMonoQuestCompleteTips : _AALBasicUIWndMono
    {
        [ALHeader("任务名")]
        public Text txtName;
        [ALHeader("延时关闭时间（秒）")]
        public float delayCloseTime = 4f;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2403); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2403); } }
    }
}
