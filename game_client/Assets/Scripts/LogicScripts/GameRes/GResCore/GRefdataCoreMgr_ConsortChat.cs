using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        [NotNull] private  Dictionary<long, List<ConsortChatMomentsRefObj>> _m_consortMomentsRefDic = new Dictionary<long, List<ConsortChatMomentsRefObj>>();

        public int getConsortMomentRefCount(long _consortId)
        {
            if (_m_consortMomentsRefDic.TryGetValue(_consortId, out List<ConsortChatMomentsRefObj> momentRefList))
                if (momentRefList != null)
                    return momentRefList.Count;
            return 0;
        }
        public List<ConsortChatMomentsRefObj> getConsortMomentRefList(long _consortId)
        {
            if (_m_consortMomentsRefDic.TryGetValue(_consortId, out List<ConsortChatMomentsRefObj> momentRefList))
                return momentRefList;
            return null;
        }
        private void _initConsortChat()
        {
            if (consortChatMomentsRefCore != null && consortChatMomentsRefCore.refList != null)
                foreach (ConsortChatMomentsRefObj moment in consortChatMomentsRefCore.refList)
                {
                    if (moment != null)
                    {
                        if (_m_consortMomentsRefDic.TryGetValue(moment.consort_id,
                                out List<ConsortChatMomentsRefObj> momentRefList))
                        {
                            if (momentRefList != null) 
                                momentRefList.Add(moment);
                        }
                        else
                        {
                            momentRefList = new List<ConsortChatMomentsRefObj>();
                            momentRefList.Add(moment);
                            _m_consortMomentsRefDic.Add(moment.consort_id, momentRefList);
                        }
                    }
                }
        }
    }
}