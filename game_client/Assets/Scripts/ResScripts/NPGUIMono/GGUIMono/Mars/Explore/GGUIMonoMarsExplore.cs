using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    // 火星探索 窗口 Mono
    public class GGUIMonoMarsExplore : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("探索等级")]
        public Text txtExploreLevel;
        [ALHeader("升级进度")]
        public Text txtLevelUpProcess;
        public Slider sldLevelUpProcess;
        [ALHeader("等级详情按钮")]
        public GameObject btnLevelDetail;
        [ALHeader("队伍迷你信息")]
        public GGUIMonoMarsExploreTeamMiniInfo monoTeamMiniInfo;
        [ALHeader("满级时显示隐藏的内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;
        [ALHeader("探索次数相关内容")]
        public GGUIMonoCommonLazyCDCountResume monoExploreLazyCD;
        [ALHeader("探索按钮")]
        public GameObject btnExplore;
        [ALHeader("记录按钮")]
        public GameObject btnRecord;
        [ALHeader("矿分享按钮")]
        public GameObject btnMineShare;


        public void setLevelMax(bool _isLevelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_isLevelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7400); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7400); } }
    }
}
