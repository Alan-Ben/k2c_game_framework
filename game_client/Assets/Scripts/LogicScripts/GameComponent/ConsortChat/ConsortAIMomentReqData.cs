using System.Collections.Generic;

namespace GOE
{
    public class ConsortAIMomentReqData
    {
        public long consortId;
        public long momentInstanceId;
        public bool isMomentContent;
        public List<Common.Common_AiChatMessage> msgList;
        public ConsortAIMomentReqData(long _consortId, long _momentInstanceId, bool _isMomentContent, List<Common.Common_AiChatMessage> _msgList)
        {
            consortId = _consortId;
            momentInstanceId = _momentInstanceId;
            isMomentContent = _isMomentContent;
            msgList = _msgList;
        }
    }
}