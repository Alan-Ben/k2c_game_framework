
using ChatPackage;
using NPEnum;
using GC2GS.p028_QuestOp;
using System.Collections.Generic;
using Common.QuestEnum;
using GS2GC.p028_QuestOp;

namespace GOE
{
    //任务相关
    public static class GSWriter_028_QuestOp
    {
        /// <summary>
        /// 接受任务
        /// </summary>
        public static GC2GS_028_001_ReqStartQuest make_001_ReqStartQuest(long _questId)
        {

            GC2GS_028_001_ReqStartQuest protocol = new GC2GS_028_001_ReqStartQuest(_questId);
            
            return protocol;
        }

        /// <summary>
        /// 完成任务
        /// </summary>
        public static GC2GS_028_002_ReqFinishQuest make_002_ReqFinishQuest(long _questId)
        {

            GC2GS_028_002_ReqFinishQuest protocol = new GC2GS_028_002_ReqFinishQuest(_questId);

            return protocol;
        }

        /// <summary>
        /// 放弃任务
        /// </summary>
        public static GC2GS_028_003_ReqDropQuest make_003_ReqDropQuest(long _questId)
        {

            GC2GS_028_003_ReqDropQuest protocol = new GC2GS_028_003_ReqDropQuest(_questId);

            return protocol;
        }

        /// <summary>
        /// 发送客户端触发的进度变更消息
        /// </summary>
        public static GC2GS_028_004_ReqAddClientTargetCount make_004_ReqAddClientTargetCount(long _questId, long _questStepId, long _questTargetId, int _addCount)
        {
            GC2GS_028_004_ReqAddClientTargetCount protocol = new GC2GS_028_004_ReqAddClientTargetCount(_questId, _questStepId, _questTargetId, _addCount);

            return protocol;
        }


        /// <summary>
        /// 完成日常周常任务
        /// </summary>
        public static GC2GS_028_010_ReqFinishDailyQuest make_010_ReqFinishDailyQuest(long _refreshSerial, long _questId)
        {

            GC2GS_028_010_ReqFinishDailyQuest protocol = new GC2GS_028_010_ReqFinishDailyQuest(_refreshSerial, _questId);

            return protocol;
        }

        /// <summary>
        /// 领取活跃度奖励
        /// </summary>
        public static GC2GS_028_011_ReqDrawActiveReward make_011_ReqDrawActiveReward(long _refreshSerial, long _rewardId)
        {

            GC2GS_028_011_ReqDrawActiveReward protocol = new GC2GS_028_011_ReqDrawActiveReward(_refreshSerial, _rewardId);

            return protocol;
        }

        /// <summary>
        /// 刷新定时任务
        /// </summary>
        public static GC2GS_028_012_ReqDailyQuestTryFresh make_012_ReqDailyQuestTryFresh(long _refreshSerial, EDailyQuestType _questType)
        {

            GC2GS_028_012_ReqDailyQuestTryFresh protocol = new GC2GS_028_012_ReqDailyQuestTryFresh(_refreshSerial, _questType);

            return protocol;
        }

        /// <summary>
        /// 一键领取日常任务奖励
        /// </summary>
        /// <param name="_refreshSerial"></param>
        /// <param name="_activeRewardIdList"></param>
        /// <param name="_questIdList"></param>
        /// <returns></returns>
        public static GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward make_013_ReqDailyQuestAKeyDrawFinishReward(long _refreshSerial, EDailyQuestType _type)
        {
            GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward protocol = new GC2GS_028_013_ReqDailyQuestAKeyDrawFinishReward(_refreshSerial, _type);
            return protocol;
        }


        /// <summary>
        /// 一键领取活跃度奖励
        /// </summary>
        /// <param name="_refreshSerial"></param>
        /// <param name="_activeRewardIdList"></param>
        /// <param name="_questIdList"></param>
        /// <returns></returns>
        public static GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward make_013_ReqDailyQuestAKeyDrawActiveReward(long _refreshSerial, EDailyQuestType _type)
        {
            GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward protocol = new GC2GS_028_014_ReqDailyQuestAKeyDrawActiveReward(_refreshSerial, _type);
            return protocol;
        }
    }
}