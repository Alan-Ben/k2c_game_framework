using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{    
    /// <summary>
    /// 每日任务状态
    /// </summary>
    public enum EDailyQuestState
    {
        CAN_GET,//可领取
        CAN_NOT_GET,//无法领取
        LOCK,//未解锁
        ALREADY_GET,//已领取
    }

    /// <summary>
    /// 每日任务状态
    /// </summary>
    [System.Serializable]
    public class GGUIDailyQuestState
    {
        [ALHeader("状态")]
        public EDailyQuestState state;
        [ALHeader("该状态需要显示的GO列表")]
        public List<GameObject> goShowList;
        [ALHeader("该状态需要隐藏的GO列表")]
        public List<GameObject> goHideList;
        [ALHeader("需要显示的文本颜色")]
        public Color txtColor;
    }

    public class GGUIMonoDailyQuestContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("任务名")]
        public Text txtName;
        [ALHeader("任务进度文本")]
        public Text txtProcess;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("领取奖励按钮")]
        public GameObject btnGetReward;
        [ALHeader("前往按钮")]
        public GameObject btnGoTo;
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer rewardItemContainer;

        [ALHeader("会先判断一键领取 再判断奖励状态")]
        [ALHeader("一键领取奖励按钮")]
        public GameObject btnOnceGetReward;
        [ALHeader("一键领取显示的GoList")]
        public List<GameObject> onceGetShowGoList;
        [ALHeader("一键领取隐藏的GoList")]
        public List<GameObject> onceGetHideGoList;
        [ALHeader("状态列表")]
        public List<GGUIDailyQuestState> stateList;
        [ALHeader("当前进度文本大小")]
        public long curProcessTextSize = 40;
        [ALHeader("粒子开始飞行位置")]
        public RectTransform particleStartTrans;

        [ALHeader("领取奖励需要播放的动画")]
        public Animation getRewardAni;
        [ALHeader("领取奖励需要播放的动画名")]
        public string getRewardAniName;
    }
}
