using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 公会协作主界面
    /// </summary>
    public class GGUIMonoGuildCooperateMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("排行榜按钮")]
        public GameObject btnRank;
        [ALHeader("日志按钮")]
        public GameObject btnLog;
        [ALHeader("地图按钮")]
        public GameObject btnMap;
        [ALHeader("上一个区域按钮")]
        public GameObject btnPrevious;
        [ALHeader("下一个区域按钮")]
        public GameObject btnNext;
        [ALHeader("区域名称")]
        public Text txtName;
        [ALHeader("重置倒计时")]
        public Text txtCD;
        [ALHeader("区域背景页面父节点")]
        public Transform bgParent;
        [ALHeader("区域未解锁时需要显示的GO列表")]
        public List<GameObject> goLockShowList;
        [ALHeader("区域未解锁时需要隐藏的GO列表")]
        public List<GameObject> goLockHideList;
        [ALHeader("某个区域有奖励时显示的GO列表")]
        public List<GameObject> goAreaHaveRewardShowList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4932); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4932); } }
    }
}
