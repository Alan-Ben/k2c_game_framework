using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 大阶段解锁界面
    /// </summary>
    public class GGUIMonoStageGoalBigStepUnlock : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("阶段图片")]
        public RawImage stageImg;
        [ALHeader("阶段标题")]
        public Text txtTitle;
        [ALHeader("阶段编号")]
        public Text stageNumTxt;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5003); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5003); } }
    }
}