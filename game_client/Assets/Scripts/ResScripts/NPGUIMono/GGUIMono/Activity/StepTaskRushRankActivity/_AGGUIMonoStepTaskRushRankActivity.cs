using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public enum EStepTaskRushRankActivityViewType
    {
        [InspectorName("阶段奖励")]
        STEP_REWARD,
        [InspectorName("任务列表")]
        TASK_LIST,
    }
    
    public class _AGGUIMonoStepTaskRushRankActivity : _AALBasicUIWndMono
    {
        // [ALHeader("活动名")]
        // public TextEx txtActivityName;
        
        [ALHeader("阶段奖励Tab")]
        public NPGGUIMonoCommonTab monoStepRewardTab;
        [ALHeader("阶段奖励Grid")]
        public GGUIMonoActivityStepRewardAllStepGrid monoStepRewardItemGrid;
        
        [ALHeader("任务列表Tab")]
        public NPGGUIMonoCommonTab monoTaskListTab;
        [ALHeader("任务列表Grid")]
        public GGUIMonoStepTaskRushRankActivityTaskGrid monoTaskItemGrid;
        
        [ALHeader("我的排名")]
        public TextEx txtMyRank;
        [ALHeader("我的排名Key(一个参数, 排名)")]
        public string txtMyRankKey;
        
        [ALHeader("我的分数")]
        public TextEx txtMyScore;
        [ALHeader("我的分数Key(两个参数, 1.排行分数名, 2.分数)")]
        public string txtMyScoreKey;
        
        [ALHeader("活动状态显示信息")]
        public List<NPCommonEnumStatMutexShowInfo<EActivityState>> activityStateShowInfo;
    }
}