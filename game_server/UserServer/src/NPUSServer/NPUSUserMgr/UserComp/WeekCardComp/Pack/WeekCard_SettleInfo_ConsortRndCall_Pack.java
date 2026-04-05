package NPUSServer.NPUSUserMgr.UserComp.WeekCardComp.Pack;

import Common.WeekCardObj.WeekCard_ConsortRndCallInfo;
import Common.WeekCardObj.WeekCard_SettleInfo_ConsortRndCall;

public class WeekCard_SettleInfo_ConsortRndCall_Pack extends WeekCard_SettleInfo_ConsortRndCall
{
    public WeekCard_ConsortRndCallInfo ensureWeekCardInfo(WeekCard_SettleInfo_ConsortRndCall _totalInfo, long _consortId)
    {
        WeekCard_ConsortRndCallInfo info;
        for (WeekCard_ConsortRndCallInfo tempInfo : _totalInfo.getList())
        {
            if (tempInfo.getConsortId() == _consortId)
                return tempInfo;
        }

        info = new WeekCard_ConsortRndCallInfo();
        info.setConsortId(_consortId);
        _totalInfo.addList(info);
        return info;
    }
}
