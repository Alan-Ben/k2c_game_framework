using System;

namespace GOE
{
    /// <summary>
    /// 冲榜提示信息
    /// </summary>
    public class ActivityRankRushChangeTipInfo
    {
        //活动实例ID
        private long _m_lActivityInstanceId;
        //冲榜ID
        private long _m_lRankRushId;
        //上次排名
        private long _m_lLastRanking;
        //当前排名
        private long _m_lCurRanking;
        //tip触发时间戳
        private long _m_lTriggerTimeMs;

        /// <summary>
        /// 活动实例id
        /// </summary>
        public long activityInstanceId { get => _m_lActivityInstanceId; }
        /// <summary>
        /// 冲榜id
        /// </summary>
        public long rankRushId { get => _m_lRankRushId; }
        /// <summary>
        /// 上次排名
        /// </summary>
        public long lastRanking { get => _m_lLastRanking; }
        /// <summary>
        /// 当前排名
        /// </summary>
        public long curRanking { get => _m_lCurRanking; }
        /// <summary>
        /// tip触发时间戳
        /// </summary>
        public long triggerTimeMs { get => _m_lTriggerTimeMs; }

        public ActivityRankRushChangeTipInfo(long _activityInstanceId, long _rankRushId, long _lastRanking, long _curRanking)
        {
            updateInfo(_activityInstanceId, _rankRushId, _lastRanking, _curRanking);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_activityInstanceId"></param>
        /// <param name="_rankRushId"></param>
        /// <param name="_lastRanking"></param>
        /// <param name="_curRanking"></param>
        public void updateInfo(long _activityInstanceId, long _rankRushId, long _lastRanking, long _curRanking)
        {
            _m_lActivityInstanceId = _activityInstanceId;
            _m_lRankRushId = _rankRushId;
            if(_m_lLastRanking == 0)
                _m_lLastRanking = _lastRanking;
            _m_lCurRanking = _curRanking;

            _m_lTriggerTimeMs = FpsAndPingMgr.instance.serverTimeTag + (long)(GRefdataCoreMgr.instance.npGeneral.rank_rush_ranking_chg_tip_delay_show_time_sec * 1000);
        }
    }
}
