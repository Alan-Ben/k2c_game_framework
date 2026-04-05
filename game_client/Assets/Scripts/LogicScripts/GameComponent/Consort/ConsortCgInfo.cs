using Common.ConsortObj;

namespace GOE
{
    public class ConsortCgInfo
    {
        private long _m_lCgId;
        private bool _m_bRewarded;//是否已经领取奖励
        
        private ConsortCGRefObj _m_rConsortCGRefObj;//cg配表数据, 不要直接调用

        public ConsortCgInfo(Consort_CGInfo _serverCGInfo)
        {
            update(_serverCGInfo);
        }

        public long cgId { get { return _m_lCgId; } }
        
        public ConsortCGRefObj consortCgRefObj
        {
            get
            {
                if (_m_rConsortCGRefObj == null || _m_rConsortCGRefObj.cg_id != _m_lCgId)
                    _m_rConsortCGRefObj = GRefdataCoreMgr.instance.consortCGRefCore.getRef(_m_lCgId);

                if (_m_rConsortCGRefObj == null)
                {
                    Debug.LogError($"consort_cg表中获取不到cg_id为{_m_lCgId}的数据");
                }
                
                return _m_rConsortCGRefObj;
            }
        }
        
        public bool rewarded { get { return _m_bRewarded; } }
        
        public void update(Consort_CGInfo _serverCGInfo)
        {
            if(_serverCGInfo == null)
                return;

            _m_lCgId = _serverCGInfo.getCgId();
            _m_bRewarded = _serverCGInfo.getRewarded();
        }
        
        /// <summary>
        /// 设置是否领奖
        /// </summary>
        /// <param name="_bRewarded"></param>
        public void setRewarded(bool _bRewarded)
        {
            _m_bRewarded = _bRewarded;
        }
    }
}