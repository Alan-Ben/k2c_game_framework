using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 竞技场主界面
    /// </summary>
    public class GGUIMonoArenaMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("贸易站按钮")]
        public GameObject btnStation;
        [ALHeader("谈判按钮")]
        public GameObject btnRandomAttack;
        [ALHeader("增加谈判次数按钮")]
        public GameObject btnAddCount;
        [ALHeader("排行榜按钮")]
        public GameObject btnRank;
        [ALHeader("战报按钮")]
        public GameObject btnReport;
        [ALHeader("设置按钮")]
        public GameObject btnSetting;
        [ALHeader("名人榜按钮")]
        public GameObject btnCelebrityList;
        [ALHeader("名人榜描述")]
        public Text txtCelebrityListDesc;
        [ALHeader("贸易站等级")]
        public Text txtStationLevel;
        [ALHeader("剩余谈判次数")]
        public Text txtAttackCount;
        [ALHeader("贸易站资源收集附加窗口")]
        public GGUIMonoArenaStationCollection monoStationCollection;
        [ALHeader("名人榜列表")]
        public GGUIMonoArenaSubCelebrity monoSubCelebrity;
        [ALHeader("没有名人榜时需要显示的GO列表")]
        public List<GameObject> goNoCelebrityShowList;
        [ALHeader("没有名人榜时需要隐藏的GO列表")]
        public List<GameObject> goNoCelebrityHideList;
        [ALHeader("名人榜描述切换时间")]
        public int switchCelebrityDescTimeSec = 3;
        [ALHeader("滚动展示名人榜前n条信息")]
        public int showCelebrityDescRanking = 10;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5200); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5200); } }
    }
}
