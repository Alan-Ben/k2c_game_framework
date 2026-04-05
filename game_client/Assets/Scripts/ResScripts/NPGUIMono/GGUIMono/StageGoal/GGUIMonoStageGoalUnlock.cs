using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标小阶段解锁弹窗
    /// </summary>
    public class GGUIMonoStageGoalUnlock : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("小阶段标题")]
        public Text txtStageTitle;
        [ALHeader("小阶段图片")]
        public RawImage imgStage;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5004); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5004); } }
    }
}