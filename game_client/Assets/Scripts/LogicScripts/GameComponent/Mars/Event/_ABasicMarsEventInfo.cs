using Common.MarsObj;

namespace GOE
{
    public class _ABasicMarsEventInfo : _IMarsEventInfo
    {
        private long _m_lEventRefId;
        private MarsEventRefObj _m_rEventRefObj;
        private NPPlayerBuffRefObj _m_rBuffRefObj;
        
        public _ABasicMarsEventInfo(MarsEventRefObj _eventRefObj)
        {
            _m_lEventRefId = _eventRefObj?.id ?? 0;
            _m_rEventRefObj = _eventRefObj;
        }
        
        public long eventRefId { get { return _m_lEventRefId; } }

        public MarsEventRefObj eventRefObj
        {
            get
            {
                if(_m_rEventRefObj == null || _m_rEventRefObj.id != _m_lEventRefId)
                    _m_rEventRefObj = GRefdataCoreMgr.instance.marsEventRefCore.getRef(_m_lEventRefId);
                return _m_rEventRefObj;
            }
        }
        
        public long buffId { get { return eventRefObj?.buff_id ?? 0; } }
        public NPPlayerBuffRefObj buffRefObj {
            get
            {
                if(_m_rBuffRefObj == null || _m_rBuffRefObj.id != buffId)
                    _m_rBuffRefObj = GRefdataCoreMgr.instance.playerBuffMap.getRef(buffId);
                return _m_rBuffRefObj;
            }
        }

        public NPPlayerBuffInfo buffInfo
        {
            get
            {
                return NPPlayer.instance.playerBuffComp.lookup(buffId);
            }
        }
    }
}