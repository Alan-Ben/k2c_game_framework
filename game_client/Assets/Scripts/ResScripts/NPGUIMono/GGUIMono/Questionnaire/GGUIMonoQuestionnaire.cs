using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 问卷调查弹窗
    /// </summary>
    public class GGUIMonoQuestionnaire : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("问卷调查活动时间")]
        public Text txtTime;
        [ALHeader("问卷调查倒计时")]
        public Text txtCD;
        [ALHeader("奖励列表")]
        public NPGGUIMonoCommonItemContainer monoRewardContainer;
        [ALHeader("同意前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;

        [ALHeader("可领取奖励时显示的GO列表")]
        public List<GameObject> goCanGetRewardShowList;
        [ALHeader("可领取奖励时隐藏的GO列表")]
        public List<GameObject> goCanGetRewardHideList;

        /************
        * 资源加载路径
        */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4600); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4600);} }
    }
}