using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 阶段目标任务奖励按钮状态
    /// </summary>
    public enum EStageGoalTaskRewardBtnState
    {
        [InspectorName("LOCK（未解锁）")]
        LOCK,
        [InspectorName("CAN_NOT_GET_REWARD（已解锁不可领取奖励）")]
        CAN_NOT_GET_REWARD,
        [InspectorName("CAN_GET_REWARD（已解锁可领取奖励）")]
        CAN_GET_REWARD,
    }

    public class GGUIMonoStageGoalPageTask : _AALBasicUIWndMono
    {
        [ALHeader("背景图")]
        public RawImage imgStageGoalBg;
        [ALHeader("大阶段序号和标题")]
        public Text stageBigNumAndTitleTxt;
        [ALHeader("小阶段序号，标题和图片")] 
        public Text stageNumTxt;
        public Text stageTitleTxt;
        public RawImage imgStageIcon;
        [ALHeader("下一个小阶段序号，标题")]
        public Text nextStageNumTxt;
        public Text nextStageTitleTxt;
        [ALHeader("没有下一个阶段时显示和隐藏的列表")]
        public List<GameObject> noNextStageShowList;
        public List<GameObject> noNextStageHideList;
        [ALHeader("这个阶段会解锁什么的列表，以及列表为空时隐藏的内容")]
        public NPGGUIMonoCommonItemContainer unlockItemContainer;
        public List<GameObject> listUnlockNothingHide;
        [ALHeader("这个阶段的奖励")]
        public NPGGUIMonoCommonItemContainer rewardItemContainer;
        [ALHeader("获取奖励按钮和文本")]
        public GameObject btnGetReward;
        public Text btnGetRewardTxt;
        public Text btnGetRewardTxt2;
        [ALHeader("按钮领奖状态控制")]
        public List<NPCommonEnumStatInfo<EStageGoalTaskRewardBtnState>> getBtnRewardState;
        [ALHeader("下一阶解锁描述")]
        public Text txtUnlockDesc;
        [ALHeader("下一阶段解锁倒计时文本")]
        public Text txtUnclockCDDesc;
        [ALHeader("没有服务器开启天数解锁条件时显隐的列表")]
        public List<GameObject> goNoServerDayCDShowList;
        public List<GameObject> goNoServerDayCDHideList;
        [ALHeader("任务列表的父节点")]
        public Transform transTaskRoot;
        [ALHeader("所有阶段全部完成显示的GoList")]
        public List<GameObject> allDoneShowGoList;
        [ALHeader("所有阶段全部完成隐藏的GoList")]
        public List<GameObject> allDoneHideGoList;
        [ALInfo("任务进度相关表现配置")]
        [ALHeader("任务进度条")]
        public NPGGUIMonoProgress monoTaskProgress;
        [ALHeader("任务进度文本")]
        public Text txtTaskProgress;
        [ALHeader("任务进度数值变化时间秒")]
        public float taskProgressChgTime = 0.5f;
        [ALHeader("首次打开界面时的动画")]
        public CommonAnimationSingleInfo aniFirstShow;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5002); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5002); } }

        public void setAllDone(bool _allDone)
        {
            ALUGUICommon.setGameObjEnable(allDoneShowGoList, false);
            ALUGUICommon.setGameObjEnable(allDoneHideGoList, false);
            ALUGUICommon.setGameObjEnable(_allDone ? allDoneShowGoList : allDoneHideGoList, true);
        }
    }
}