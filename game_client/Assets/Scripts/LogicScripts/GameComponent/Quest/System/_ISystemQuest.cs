using NPCommon;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 系统任务接口
    /// </summary>
    public interface _ISystemQuest
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        string questName { get; }
        /// <summary>
        /// 展示名称
        /// </summary>
        string showNameStr { get; }
        /// <summary>
        /// 当前计数
        /// </summary>
        long curCount { get; }
        /// <summary>
        /// 目标计数
        /// </summary>
        long targetCount { get; }
        /// <summary>
        /// 进度值格式化显示方式
        /// </summary>
        EValueFormatType processNumFormat { get; }
        /// <summary>
        /// 是否可以领取奖励
        /// </summary>
        bool canGetReward { get; }
        /// <summary>
        /// 奖励列表
        /// </summary>
        List<NPCommonCostItem> rewardItemList { get; }
        /// <summary>
        /// 处理领取奖励
        /// </summary>
        void dealGetReward(Action<List<NPCommon_ItemInfo>> _callback);
        /// <summary>
        /// 处理前往
        /// </summary>
        void dealGoTo();
    }
}
