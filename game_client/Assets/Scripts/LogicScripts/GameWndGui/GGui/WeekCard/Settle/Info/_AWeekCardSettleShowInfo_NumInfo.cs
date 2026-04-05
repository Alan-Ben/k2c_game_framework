using Common.WeekCardObj;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 次数结算信息
    /// </summary>
    public abstract class _AWeekCardSettleShowInfo_NumInfo:_IWeekCardSettleShowInfo
    {
        private readonly WeekCard_SettleDetailInfo _m_info;
        protected WeekCard_SettleInfo_NumInfo _m_exInfo;

        public _AWeekCardSettleShowInfo_NumInfo(WeekCard_SettleDetailInfo _info)
        {
            _m_info = _info;
            _m_exInfo = new WeekCard_SettleInfo_NumInfo();
            _m_exInfo.readPackage(_m_info.getDetailInfo());
        }
        
        public EWeekCardSettleType getSettleType()
        {
            return _m_info.getType();
        }

        public abstract string getShowContent();
    }
}