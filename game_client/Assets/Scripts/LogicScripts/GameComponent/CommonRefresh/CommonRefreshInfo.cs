using Common.CommonFuncObj;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonRefreshInfo
    {
        private readonly long _m_id;
        private long _m_nextRefreshTimeMs;
        
        
        public CommonRefreshInfo([NotNull] CommonFunc_Refresh _serverInfo)
        {
            _m_id = _serverInfo.getId();
            _m_nextRefreshTimeMs = _serverInfo.getNextRefreshTimeMs();
        }
        
        
        public long id { get { return _m_id; } }
        public long refreshTimeMs { get { return _m_nextRefreshTimeMs; } }


        internal void _updateData(long _nextRefreshTimeMs)
        {
            _m_nextRefreshTimeMs = _nextRefreshTimeMs;
        }
    }
}