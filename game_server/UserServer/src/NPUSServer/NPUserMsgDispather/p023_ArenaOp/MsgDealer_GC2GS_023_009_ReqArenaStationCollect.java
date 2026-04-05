package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import CommonEnum.ESpecialItemType;
import GC2GS.p023_ArenaOp.GC2GS_023_009_ReqArenaStationCollect;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Station;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

public class MsgDealer_GC2GS_023_009_ReqArenaStationCollect extends NPUserMsgDealer<GC2GS_023_009_ReqArenaStationCollect>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_009_ReqArenaStationCollect _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //查找处理器
        SpecialItemDealer_Station stationDealer = userData.getSpecialItemComponent().getDealer(ESpecialItemType.ARENA_STATION, SpecialItemDealer_Station.class);
        if (stationDealer == null)
        {
            _commiter.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.STATION_UPGRADE);

        //执行逻辑
        Result result = stationDealer.collect(context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        //展示推送
        userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        _commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_009_RetArenaStationCollect());
    }
}
