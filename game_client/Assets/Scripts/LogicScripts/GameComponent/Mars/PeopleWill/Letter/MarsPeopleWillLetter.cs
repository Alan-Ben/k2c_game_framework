using Common.MarsObj;

namespace GOE
{
    /// <summary>
    /// 民意信件数据
    /// </summary>
    public class MarsPeopleWillLetter : _IMarsPeopleWillLetter
    {
        private long _m_lInstanceId;//信件实例id
        private long _m_lRefId;//信件配表id
        private MarsPeopleLetterRefObj _m_refObj;//信件配表数据
        private long _m_lNpcRefId;//npc配表id
        private NPNPCRefObj _m_npcRefObj;//npc配表数据
        private bool _m_bIsDealed;//是否已处理

        public MarsPeopleWillLetter(Mars_Letter _serverLetterInfo)
        {
            update(_serverLetterInfo);
        }
        
        public long instanceId { get { return _m_lInstanceId; } }
        public long refId { get { return _m_lRefId; } }

        public MarsPeopleLetterRefObj refObj
        {
            get
            {
                if(_m_refObj == null || _m_refObj.id != _m_lRefId)
                    _m_refObj = GRefdataCoreMgr.instance.marsPeopleLetterRefCore.getRef(_m_lRefId);
                return _m_refObj;
            }
        }

        public NPNPCRefObj npcRefObj
        {
            get
            {
                if(_m_npcRefObj == null || _m_npcRefObj.id != _m_lNpcRefId)
                    _m_npcRefObj = GRefdataCoreMgr.instance.npcRefCore.getRef(_m_lNpcRefId);
                return _m_npcRefObj;
            }
        }
        
        /// <summary>
        /// 是否已处理(服务端标识)
        /// </summary>
        public bool isDealed { get { return _m_bIsDealed; } }

        /// <summary>
        /// 是否达成完成条件
        /// </summary>
        public bool resolveCondEnable
        {
            get
            {
                MarsPeopleLetterRefObj letterRefObj = refObj;
                if (letterRefObj == null)
                    return false;
                
                return letterRefObj.resolveCond == null || letterRefObj.resolveCond.isNoConditionOrEnable(null);
            }
        }
        
        public void update(Mars_Letter _serverLetterInfo)
        {
            if(_serverLetterInfo == null)
                return;
            
            _m_lInstanceId = _serverLetterInfo.getId();
            _m_lRefId = _serverLetterInfo.getLetterId();
            _m_lNpcRefId = _serverLetterInfo.getNpcId();
            _m_bIsDealed = _serverLetterInfo.getIsDealed();
        }

        /// <summary>
        /// 设置为已处理
        /// </summary>
        public void setDealed()
        {
            _m_bIsDealed = true;
        }
    }
}