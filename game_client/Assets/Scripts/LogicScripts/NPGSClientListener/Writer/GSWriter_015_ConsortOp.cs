namespace GOE
{
    public static class GSWriter_015_ConsortOp
    {
        /// <summary>
        /// 请求升级羁绊等级
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_001_ReqUpgradeFettersLvl make_001_ReqUpgradeFettersLvl(long _consortId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_001_ReqUpgradeFettersLvl(_consortId);
        }
        
        /// <summary>
        /// 家人-领悟经营技能等级
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skillId"></param>
        /// <param name="_isAdvanced"></param>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_002_ReqUnderstandBusinessSkill make_002_ReqUnderstandBusinessSkill(long _consortId, long _skillId, bool _isAdvanced)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_002_ReqUnderstandBusinessSkill(_consortId, _skillId, _isAdvanced);
        }
        
        /// <summary>
        /// 请求升级加护技能等级
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skillId"></param>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_003_ReqUpgradeBlessSkill make_003_ReqUpgradeBlessSkill(long _consortId, long _skillId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_003_ReqUpgradeBlessSkill(_consortId, _skillId);
        }
        
        /// <summary>
        /// 请求随机邀约
        /// </summary>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_004_ReqCallRand make_004_ReqCallRand()
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_004_ReqCallRand();
        }
        
        /// <summary>
        /// 请求一键邀约
        /// </summary>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_005_ReqCallAkey make_005_ReqCallAkey()
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_005_ReqCallAkey();
        }
        
        /// <summary>
        /// 请求指定邀约
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_consortTravelId"></param>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_006_ReqCallAppoint make_006_ReqCallAppoint(long _consortId, long _consortTravelId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_006_ReqCallAppoint(_consortId, _consortTravelId);
        }
        
        /// <summary>
        /// 设置妃子皮肤
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_007_ReqSetCurSkin make_007_ReqSetCurSkin(long _consortId, long _skinId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_007_ReqSetCurSkin(_consortId, _skinId);
        }
        
        /// <summary>
        /// 获取家人周期内的指定出游次数
        /// </summary>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_008_ReqGetRoundTravelCount make_008_ReqGetRoundTravelCount(long _travelId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_008_ReqGetRoundTravelCount(_travelId);
        }
        
        /// <summary>
        /// 获取某一妃子的所有经营技能数据
        /// </summary>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_009_ReqGetAllBusinessSkill make_009_ReqGetAllBusinessSkill(long _consortId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_009_ReqGetAllBusinessSkill(_consortId);
        }

        /// <summary>
        /// 请求升级星辉等级
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_010_ReqUpgradeHaloLvl make_010_ReqUpgradeHaloLvl(long _consortId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_010_ReqUpgradeHaloLvl(_consortId);
        }
        
        /// <summary>
        /// 请求领取CG奖励
        /// </summary>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_011_ReqGetCgUnlockReward make_011_ReqGetCgUnlockReward(long _cgId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_011_ReqGetCgUnlockReward(_cgId);
        }
        
        /// <summary>
        /// 请求解锁皮肤
        /// </summary>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_012_ReqUnlockSkin make_012_ReqUnlockSkin(long _skinId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_012_ReqUnlockSkin(_skinId);
        }
        
        /// <summary>
        /// 请求解锁星辉
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public static GC2GS.p015_ConsortOp.GC2GS_015_013_ReqUnlockHalo make_013_ReqUnlockHalo(long _consortId)
        {
            return new GC2GS.p015_ConsortOp.GC2GS_015_013_ReqUnlockHalo(_consortId);
        }
    }
}
