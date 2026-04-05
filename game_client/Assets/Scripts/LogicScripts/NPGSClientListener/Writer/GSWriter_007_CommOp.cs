using System.Collections.Generic;
using GC2GS.p007_CommOp;

namespace GOE
{
    public static class GSWriter_007_CommOp
    {
        public static GC2GS.p007_CommOp.GC2GS_007_001_ReqDealRemoteEffect make_001_ReqDealRemoteEffect(long _clientSerialize, long _effectId)
        {
            GC2GS.p007_CommOp.GC2GS_007_001_ReqDealRemoteEffect protocol = new GC2GS.p007_CommOp.GC2GS_007_001_ReqDealRemoteEffect();
            protocol.setClientSerialize(_clientSerialize);
            protocol.setRefId(_effectId);
            return protocol;
        }
        
        /// <summary>
        /// 请求玩家领取离线奖励
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static GC2GS.p007_CommOp.GC2GS_007_020_ReqTakeOfflineReward make_020_ReqTakeOfflineReward(long _id)
        {
            GC2GS.p007_CommOp.GC2GS_007_020_ReqTakeOfflineReward protocol = new GC2GS.p007_CommOp.GC2GS_007_020_ReqTakeOfflineReward(_id);
            return protocol;
        }
        
        /// <summary>
        /// 请求玩家领取离线奖励
        /// </summary>
        /// <param name="_idList"></param>
        /// <returns></returns>
        public static GC2GS.p007_CommOp.GC2GS_007_021_ReqTakeOfflineRewardList make_021_ReqTakeOfflineRewardList(List<long> _idList)
        {
            GC2GS.p007_CommOp.GC2GS_007_021_ReqTakeOfflineRewardList protocol = new GC2GS.p007_CommOp.GC2GS_007_021_ReqTakeOfflineRewardList(_idList);
            return protocol;
        }

        public static GC2GS_007_005_ReqGenerateShareCode make_005_ReqGenerateShareCode(CommonEnum.EShareCodeType _type, byte[] _data)
        {
            return new GC2GS_007_005_ReqGenerateShareCode(_type, _data);
        }
        public static GC2GS_007_006_ReqShareCodeData make_006_ReqShareCodeData(CommonEnum.EShareCodeType _type, string _code)
        {
            return new GC2GS_007_006_ReqShareCodeData(_type, _code);
        }
        
        public static GC2GS_007_015_ReqDrawStageGoalFirstReachReward make_015_ReqDrawStageGoalFirstReachReward(long _bigStepId)
        {
            return new GC2GS_007_015_ReqDrawStageGoalFirstReachReward(_bigStepId);
        }
        
        public static GC2GS_007_016_ReqStageGoalFirstReachDetailInfo make_016_ReqStageGoalFirstReachDetailInfo(long _bigStepId)
        {
            return new GC2GS_007_016_ReqStageGoalFirstReachDetailInfo(_bigStepId);
        }
        
        public static GC2GS_007_017_ReqStageGoalFirstReachBaseInfo make_017_ReqStageGoalFirstReachBaseInfo()
        {
            return new GC2GS_007_017_ReqStageGoalFirstReachBaseInfo();
        }
        
        public static GC2GS_007_018_ReqTakeStageGoalSubTaskReward make_018_ReqTakeStageGoalSubTaskReward(long _taskId)
        {
            return new GC2GS_007_018_ReqTakeStageGoalSubTaskReward(_taskId);
        }
        
        public static GC2GS_007_019_ReqTakeStageGoalBigStepReward make_019_ReqTakeStageGoalBigStepReward(long _bigStep)
        {
            return new GC2GS_007_019_ReqTakeStageGoalBigStepReward(_bigStep);
        }
        
        public static GC2GS_007_025_ReqTakeStageGoalTaskReward make_025_ReqTakeStageGoalTaskReward(long _stage)
        {
            return new GC2GS_007_025_ReqTakeStageGoalTaskReward(_stage);
        }
        
        /// <summary>
        /// 请求抽卡
        /// </summary>
        /// <param name="_poolId">卡池id</param>
        /// <param name="_isTen">是否十抽</param>
        /// <returns></returns>
        public static GC2GS_007_026_ReqGachaRoll make_026_ReqGachaRoll(long _poolId, bool _isTen)
        {
            return new GC2GS_007_026_ReqGachaRoll(_poolId, _isTen);
        }
        
        /// <summary>
        /// 请求自身抽卡记录
        /// </summary>
        /// <param name="_poolId"></param>
        /// <returns></returns>
        public static GC2GS_007_027_ReqGachaRollRecord make_027_ReqGachaRollRecord(long _poolId)
        {
            return new GC2GS_007_027_ReqGachaRollRecord(_poolId);
        }
        
        /// <summary>
        /// 请求兑换
        /// </summary>
        /// <returns></returns>
        public static GC2GS_007_028_ReqRecruit make_028_ReqRecruit(long _id)
        {
            return new GC2GS_007_028_ReqRecruit(_id);
        }
        
        /// <summary>
        /// 请求抽卡公屏记录
        /// </summary>
        /// <param name="_poolId"></param>
        /// <param name="_dbId">抽卡记录数据id 查询这个id之后的数据</param>
        /// <returns></returns>
        public static GC2GS_007_029_ReqGachaPublicRollRecord make_029_ReqGachaPublicRollRecord(long _poolId, long _dbId)
        {
            return new GC2GS_007_029_ReqGachaPublicRollRecord(_poolId, _dbId);
        }
        
        /// <summary>
        /// 请求领取抽卡累计奖励
        /// </summary>
        /// <param name="_poolId"></param>
        /// <returns></returns>
        public static GC2GS_007_030_ReqDrawGachaCumulativeReward make_030_ReqDrawGachaCumulativeReward(long _poolId)
        {
            return new GC2GS_007_030_ReqDrawGachaCumulativeReward(_poolId);
        }
        
        /// <summary>
        /// 请求领奖
        /// </summary>
        public static GC2GS_007_022_ReqDrawTargetReward make_007_022_ReqDrawTargetReward(long _id)
        {
            return new GC2GS_007_022_ReqDrawTargetReward(_id);
        }
    }
}
