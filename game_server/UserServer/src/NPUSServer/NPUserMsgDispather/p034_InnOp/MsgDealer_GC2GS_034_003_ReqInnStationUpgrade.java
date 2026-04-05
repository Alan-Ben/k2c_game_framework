package NPUSServer.NPUserMsgDispather.p034_InnOp;

import GC2GS.p034_InnOp.GC2GS_034_003_ReqInnStationUpgrade;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.Station.InnStationInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;

public class MsgDealer_GC2GS_034_003_ReqInnStationUpgrade extends NPUserMsgDealer<GC2GS_034_003_ReqInnStationUpgrade>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_034_003_ReqInnStationUpgrade _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        InnStationInfo stationInfo = userData.getInnComponent().getStationMgr().lookupStation(_msg.getStationId());
        if (null == stationInfo)
        {
            _commiter.commitFailRes(InnErr.INN_STATION_NOT_FOUND.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.INN_BUILD_STATION);

        Result result = stationInfo.upgrade(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_034_InnOp.make_003_RetInnStationUpgrade());
    }
}
