using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    public class CommonTravelEventResultInfo : _ATravelEventResultInfo
    {
        public CommonTravelEventResultInfo(_ATravelEventInfo _eventInfo, List<NPCommon_ItemInfo> _rewardList, byte[] _extData, long _odlEarnings) : base(_eventInfo, _rewardList, _extData, _odlEarnings)
        {
        }
    }
}