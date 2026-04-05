package NPUSServer.NPUSUserMgr.UserComp.WeekCardComp.Dealer;

import CommonEnum.EWeekCardSettleType;
import NPEnum.ENPFunctionType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp.PlayerLazyCD;
import NPUSServer.NPUSUserMgr.UserComp.WeekCardComp.WeekCardComponent;
import NPUSServer.NPUSUserMgr.UserComp.WeekCardComp._ADealPolicyWeekCardDealer;

import java.nio.ByteBuffer;

public class WeekCardDealer_ConsortRndCall extends _ADealPolicyWeekCardDealer
{
    public WeekCardDealer_ConsortRndCall(WeekCardComponent _comp)
    {
        super(_comp);
    }

    @Override
    protected ENPFunctionType getFuncType()
    {
        return ENPFunctionType.CONSORT;
    }

    @Override
    public EWeekCardSettleType getType()
    {
        return EWeekCardSettleType.CONSORT_RND_CALL;
    }

    @Override
    public void doOfflineCdSettle(NPPlayerContext _context)
    {
        PlayerLazyCD lazyCd = getUserData().getLazyCDComponent().ensureLazyCD(RefGeneral.Ref().consort_rand_call_cd);
        if (lazyCd == null)
            return;

        lazyCd.doOfflineSettle();
    }

    @Override
    public ByteBuffer settleOverflowCdDetail(long _startSettleTimeMs, long _endSettleTimeMs, NPPlayerContext _context)
    {
    	return null;
//        //检查消耗
//        PlayerLazyCD cd = getUserData().getLazyCDComponent().ensureLazyCD(RefGeneral.Ref().consort_rand_call_cd);
//        if (cd == null)
//            return null;
//
//        //扣除CD
//        int overflowCount = cd.useOfflineCd(_endSettleTimeMs, isLazy());
//        if (overflowCount <= 0)
//            return null;
//
//        WeekCard_SettleInfo_ConsortRndCall_Pack settleInfo = new WeekCard_SettleInfo_ConsortRndCall_Pack();
//
//        //调用组件的时间处理溢出CD值的逻辑
//        getUserData().getConsortComponent().settleOfflineOverflowCd(settleInfo, overflowCount, _context);
//        if (settleInfo.getList().isEmpty())
//            return null;
//
//        //发放获得的势力值
//        for (WeekCard_ConsortRndCallInfo consortRndCallInfo : settleInfo.getList())
//        {
//            ConsortInfo consort = getUserData().getConsortComponent().lookup(consortRndCallInfo.getConsortId());
//            if (consort == null)
//            {
//                USLog.error(getUserData().getUSServer(), "WeekCardDealer_ConsortRndCall settleOverflowCdDetail consort is null, cid:{} consortId:{}", getUserData().getCid(), consortRndCallInfo.getConsortId());
//                continue;
//            }
//
//            consort.incrSkillPoint(consortRndCallInfo.getSkillPoint(), _context);
//        }
//
//        return settleInfo.makePackage();
    }
}
