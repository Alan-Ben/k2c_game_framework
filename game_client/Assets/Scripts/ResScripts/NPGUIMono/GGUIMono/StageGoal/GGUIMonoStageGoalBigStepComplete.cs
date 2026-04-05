using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标完成大阶段展示界面
    /// </summary>
    public class GGUIMonoStageGoalBigStepComplete : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("阶段图片")]
        public RawImage imgStage;
        [ALHeader("阶段标题")]
        public Text txtTitle;
        [ALHeader("阶段编号")]
        public Text txtStageNum;
        [ALHeader("阶段完成时间")]
        public Text txtStageCompleteTime;
        [ALHeader("获得奖励的物品列表")]
        public NPGGUIMonoCommonItemContainer itemContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5009); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5009); } }
    }
}