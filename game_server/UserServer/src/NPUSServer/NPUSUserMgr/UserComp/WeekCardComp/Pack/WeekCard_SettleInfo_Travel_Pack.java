package NPUSServer.NPUSUserMgr.UserComp.WeekCardComp.Pack;

import Common.WeekCardObj.WeekCard_SettleInfo_Travel;
import Common.WeekCardObj.WeekCard_TravelGotConsrtInfo;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.Util.Pair.WCGPairLong;
import NPCommon.Util.Pair.WCGPairLongList;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.USLog;

public class WeekCard_SettleInfo_Travel_Pack
{
    private NPItemCostCollector_nosafe itemCollector;
    private WCGPairLongList gotConsortLikeList;
    private WCGPairLongList unGetConsortLikeList;

    public WeekCard_SettleInfo_Travel_Pack()
    {
        itemCollector = new NPItemCostCollector_nosafe();
        gotConsortLikeList = new WCGPairLongList();
        unGetConsortLikeList = new WCGPairLongList();
    }

    public NPItemCostCollector_nosafe getItemCollector()
    {
        return itemCollector;
    }

    public void recordGotConsortIntimacy(long consortId, int intimacy)
    {
        WCGPairLong pair = gotConsortLikeList.ensure(consortId);
        pair.setSecond(pair.second() + intimacy);
    }

    public void recordUnGetConsortLike(long consortId, int likeValue)
    {
        WCGPairLong pair = unGetConsortLikeList.ensure(consortId);
        pair.setSecond(pair.second() + likeValue);
    }

    public WeekCard_SettleInfo_Travel dealAll(NPUSUserData _userData, NPPlayerContext _context)
    {
        WeekCard_SettleInfo_Travel proto = new WeekCard_SettleInfo_Travel();

        _userData.gainItemList(itemCollector.getItemList(), _context);

        for (WCGPairLong consortIntimacyInfo : gotConsortLikeList.getList())
        {
            ConsortInfo consort = _userData.getConsortComponent().lookup(consortIntimacyInfo.first());
            if (consort == null)
            {
                USLog.error(_userData.getUSServer(), "player:{} dealAll consort not found, consortId:{} intimacy:{}", _userData.getCid(), consortIntimacyInfo.first(), consortIntimacyInfo.second());
                continue;
            }

            consort.incrIntimacy(consortIntimacyInfo.second(), _context);
            proto.addGotConsrtlist(new WeekCard_TravelGotConsrtInfo(consortIntimacyInfo.first(), consortIntimacyInfo.second()));
        }

//        for (WCGPairLong unGetConsortLikeInfo : unGetConsortLikeList.getList())
//        {
//            int finalValue = _userData.getUngetConsortComponent().cmdIncrLike(unGetConsortLikeInfo.first(), (int) unGetConsortLikeInfo.second(), true, _context);
//            if (finalValue != 0)
//            {
//                proto.addUngetConsortlist(new WeekCard_TravelUngetConsortInfo(unGetConsortLikeInfo.first(), unGetConsortLikeInfo.second()));
//            }
//        }

        return proto;
    }
}
