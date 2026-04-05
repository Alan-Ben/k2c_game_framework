package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import GC2GS.p024_DungeonOp.GC2GS_024_004_ReqMiddayDungeonDrawBox;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

public class MsgDealer_GC2GS_024_004_ReqMiddayDungeonDrawBox extends NPUserMsgDealer<GC2GS_024_004_ReqMiddayDungeonDrawBox>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_004_ReqMiddayDungeonDrawBox _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MIDDAY_DUNGEON_BOX_DRAW);
        Result result = getUSServer().getMiddayDungeonMgr().getBoxMgr().drawBox(_msg.getDbId(), userData, context);
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto(ENpRewardShowType.TIP));

        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_004_RetMiddayDungeonDrawBox());
    }
}
