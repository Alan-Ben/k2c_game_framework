using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标任务详情弹窗
    /// </summary>
    public class GGUIMonoStageGoalTaskInfo : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5008); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5008); } }
    }
}