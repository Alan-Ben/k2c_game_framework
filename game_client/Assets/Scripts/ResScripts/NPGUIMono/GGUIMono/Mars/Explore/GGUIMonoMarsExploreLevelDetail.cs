using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsExploreLevelDetail : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("等级文本")]
        public Text txtCurLevel;
        public Text txtNextLevel;
        [ALHeader("等级翻译key")]
        public string levelTranslateKey;
        
        [ALHeader("事件品质列表")]
        public GGUIMonoMarsExploreEventQualityContainer monoCurEventQualityContainer;
        public GGUIMonoMarsExploreEventQualityContainer monoNextEventQualityContainer;
        [ALHeader("探索次数累积上限")]
        public Text txtCurExploreCountLimit;
        public Text txtNextExploreCountLimit;
        [ALHeader("升级奖励列表")]
        public NPGGUIMonoCommonItemContainer monoUpgradeRewardContainer;
        [ALHeader("升级进度文本")]
        public Text txtUpgradeProcess;
        [ALHeader("满级时显示的内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;


        public void setLevelMax(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7401); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7401); } }
    }
}