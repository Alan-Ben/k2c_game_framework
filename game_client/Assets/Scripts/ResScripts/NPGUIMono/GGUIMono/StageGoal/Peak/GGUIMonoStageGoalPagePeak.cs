using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标-时代之巅页面
    /// </summary>
    public class GGUIMonoStageGoalPagePeak : _AALBasicUIWndMono
    {
        [ALHeader("第一名玩家形象")]
        public GGUIMonoCommonShowCase monoFirstPlayer;
        [ALHeader("第一名玩家头像")]
        public NPGGUIMonoPlayerIcon monoFirstPlayerIcon;
        [ALHeader("第一名玩家小阶段标题")]
        public Text txtFirstPlayerSmallStage;
        [ALHeader("阶段列表")]
        public GGUIMonoStageGoalPagePeakGrid monoGrid;
        [ALHeader("有第一名玩家形象显示的GO列表")]
        public List<GameObject> goHaveTopPlayerShowList;
        [ALHeader("有第一名玩家形象隐藏的GO列表")]
        public List<GameObject> goHaveTopPlayerHideList;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5016); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5016); } }
    }
}