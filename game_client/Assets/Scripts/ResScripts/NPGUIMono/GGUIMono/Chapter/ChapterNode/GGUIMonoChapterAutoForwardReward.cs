using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoChapterAutoForwardReward : _AALBasicUIWndMono
    {
        [ALHeader("之前关卡")]
        public Text textOldChapter;
        [ALHeader("新关卡")]
        public Text textNewChapter;
        [ALHeader("消耗金币")]
        public Text textCostGold;
        [ALHeader("大臣经验")]
        public Text textHeroExp;
        [ALHeader("玩家经验")]
        public Text textPlayerExp;
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer itemContainer;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2131); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2131); } }
    }
}