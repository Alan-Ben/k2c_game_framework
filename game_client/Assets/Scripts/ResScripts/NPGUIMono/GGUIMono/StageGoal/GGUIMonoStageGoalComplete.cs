using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标小阶段完成弹窗
    /// </summary>
    public class GGUIMonoStageGoalComplete : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("小阶段序号")]
        public Text txtStageNum;
        [ALHeader("小阶段标题")]
        public Text txtStageTitle;
        [ALHeader("下一个小阶段序号")]
        public Text txtNextStageNum;
        [ALHeader("下一个小阶段标题")]
        public Text txtNextStageTitle;
        [ALHeader("没有下一个阶段时显示和隐藏的列表")]
        public List<GameObject> noNextStageShowList;
        public List<GameObject> noNextStageHideList;
        [ALHeader("小阶段图片")]
        public RawImage imgStage;
        [ALHeader("奖励物品列表")]
        public NPGGUIMonoCommonItemContainer monoRewardItemContainer;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5010); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5010); } }
    }
}