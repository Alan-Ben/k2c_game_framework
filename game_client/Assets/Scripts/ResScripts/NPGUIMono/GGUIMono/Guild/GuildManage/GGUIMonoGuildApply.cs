using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟申请界面
    /// </summary>
    public class GGUIMonoGuildApply : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("快速加入按钮")]
        public GameObject btnQuickJoin;
        [ALHeader("创建联盟")]
        public GameObject btnCreate;
        [ALHeader("更多联盟")]
        public GameObject btnMore;
        [ALHeader("加入联盟CD")]
        public NPGGUIMonoCommonCountDown monoJoinCD;
        [ALHeader("首次加入奖励列表")]
        public NPGGUIMonoCommonItemContainer monoItemContainer;
        [ALHeader("有首次加入奖励时需要展示的GO列表")]
        public List<GameObject> goHaveFirstJoinRewardShowList;
        [ALHeader("有首次加入奖励时需要隐藏的GO列表")]
        public List<GameObject> goHaveFirstJoinRewardHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4900); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4900); } }
    }
}
