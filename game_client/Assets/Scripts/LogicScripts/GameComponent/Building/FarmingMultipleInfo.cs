
using Common.BuildingObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 农场建筑暴击倍数信息
    /// </summary>
    public class FarmingMultipleInfo
    {
        //暴击次数
        private int _m_lCritCount;
        //上次刷新暴击次数时间 毫秒
        private long _m_lLastRefreshTimeMs;
        //点击次数
        private long _m_lClickNum;
        //随机种子
        private long _m_lRandomSeed;
        //随机数生成器
        private CSSyncRandom _m_random;

        /// <summary>
        /// 点击次数
        /// </summary>
        public long clickNum { get { return _m_random != null ? _m_random.operationCount : 0; } }

        public FarmingMultipleInfo(Building_FarmMultipleInfo _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(Building_FarmMultipleInfo _info)
        {
            if (_info == null)
                return;

            _m_lCritCount = _info.getCritCount();
            _m_lLastRefreshTimeMs = _info.getLastRefreshTimeMs();
            _m_lClickNum = _info.getClickNum();
            _m_lRandomSeed = _info.getRandomSeed();
            _m_random = new CSSyncRandom(_m_lRandomSeed, _m_lClickNum);
        }

        /// <summary>
        /// 获取触发倍数并增加点击次数
        /// </summary>
        public long addOpCountAndGetMutiple(long _buildingId, int _level)
        {
            //与上次刷新时间比较检查是否跨天，是则重置暴击次数
            int curDate = TimeUtil.getTimeByYYYYMM(FpsAndPingMgr.instance.serverTimeTag);
            int lastRefreshDate = TimeUtil.getTimeByYYYYMM(_m_lLastRefreshTimeMs);
            if (lastRefreshDate != curDate)
            {
                _m_lCritCount = 0;
                _m_lLastRefreshTimeMs = FpsAndPingMgr.instance.serverTimeTag;
            }

            FarmingBuildingLevelRefObj buildingLevelRef = GRefdataCoreMgr.instance.getFarmingBuildingLevelRefObj(_buildingId, _level);
            if (buildingLevelRef == null ||
                buildingLevelRef.multiple_weight_list == null ||
                buildingLevelRef.multiple_weight_list.IsEmpty ||
                _m_lCritCount >= GRefdataCoreMgr.instance.npGeneral.farming_building_trigger_multiple_daily_limit)
            {
                _m_random?.addOperationCount();
                return 1;
            }

            long multipleValue = buildingLevelRef.multiple_weight_list.RandomSelect(_m_random);

            //大于1倍说明触发暴击，暴击次数加1
            if (multipleValue > 1)
                _m_lCritCount++;

            if (multipleValue <= 0)
                return 1;
            else 
                return multipleValue;
        }

        /// <summary>
        /// 增加点击次数
        /// </summary>
        public void addOpCount()
        {
            _m_random?.addOperationCount();
        }
    }
}