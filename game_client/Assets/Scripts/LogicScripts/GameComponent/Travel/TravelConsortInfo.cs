using Common.TravelObj;

namespace GOE
{
    /// <summary>
    /// 游历妃子信息
    /// </summary>
    public class TravelConsortInfo
    {
        private long _m_lConsortId;//妃子id
        private int _m_iLike;//好感度
        
        private ConsortRefShowInfo _m_rConsortRefShowInfo;//妃子显示信息
        private TravelConsortRefObj _m_rTravelConsortRefObj;//游历妃子配表数据

        public TravelConsortInfo(Travel_Consort _serverTravelConsort)
        {
            updateInfo(_serverTravelConsort);
        }
        
        public long consortId { get => _m_lConsortId; }
        public int like { get => _m_iLike; }
        public ConsortRefShowInfo consortRefShowInfo
        {
            get
            {
                if (_m_rConsortRefShowInfo == null || _m_rConsortRefShowInfo.consortId != _m_lConsortId)
                    _m_rConsortRefShowInfo = new ConsortRefShowInfo(_m_lConsortId);

                return _m_rConsortRefShowInfo;
            }
        }
        
        public TravelConsortRefObj travelConsortRefObj
        {
            get
            {
                if (_m_rTravelConsortRefObj == null || _m_rTravelConsortRefObj.consort_id != _m_lConsortId)
                    _m_rTravelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(_m_lConsortId);

                return _m_rTravelConsortRefObj;
            }
        }

        public void updateInfo(Travel_Consort _serverTravelConsort)
        {
            if(_serverTravelConsort == null)
                return;
            
            _m_lConsortId = _serverTravelConsort.getConsortId();
            _m_iLike = _serverTravelConsort.getLike();
        }
    }
}