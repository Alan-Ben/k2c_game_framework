using Common.GachaObj;

namespace GOE
{
    /// <summary>
    /// 抽卡保底信息
    /// </summary>
    public class GachaGuaranteeInfo
    {
        /// <summary>
        /// 保底规则id
        /// </summary>
        private long _m_lGuaranteeId;
        
        /// <summary>
        /// 剩余次数
        /// </summary>
        private int _m_iRemianTimes;

        public long guaranteeId => _m_lGuaranteeId;
        public int remianTimes => _m_iRemianTimes;

        public GachaGuaranteeInfo(Gacha_GuaranteeInfo _serverInfo)
        {
            update(_serverInfo);
        }

        public void update(Gacha_GuaranteeInfo _serverInfo)
        {
            if(_serverInfo == null)
                return;
            
            _m_lGuaranteeId = _serverInfo.getGuaranteeId();
            updateRemainTimes(_serverInfo.getRemianTimes());
        }
        
        /// <summary>
        /// 更新剩余次数
        /// </summary>
        /// <param name="_iRemainTimes"></param>
        public void updateRemainTimes(int _iRemainTimes)
        {
            _m_iRemianTimes = _iRemainTimes;
        }
    }
}