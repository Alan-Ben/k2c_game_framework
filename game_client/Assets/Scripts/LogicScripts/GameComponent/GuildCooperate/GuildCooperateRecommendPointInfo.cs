using Common.GuildCooperateObj;

namespace GOE
{
    /// <summary>
    /// 联盟协作推荐奖励据点信息
    /// </summary>
    public class GuildCooperateRecommendPointInfo
    {
        //区域ID
        private long _m_lAreaId;
        //奖励点索引，从0开始
        private int _m_iIndex;

        /// <summary>
        /// 区域ID
        /// </summary>
        public long areaId { get { return _m_lAreaId; } }
        /// <summary>
        /// 奖励点索引，从0开始
        /// </summary>
        public int index { get { return _m_iIndex; } }


        public GuildCooperateRecommendPointInfo(GuildCooperate_RewardPointPos _info)
        {
            updateInfo(_info);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateInfo(GuildCooperate_RewardPointPos _info)
        {
            if (_info == null)
                return;

            _m_lAreaId = _info.getAreaId();
            _m_iIndex = _info.getIndex();
        }
    }
}