package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import Common.DungeonObj.MiddayDungeon_SettleInfo;
import GC2GS.p024_DungeonOp.GC2GS_024_001_ReqMiddayDungeonAttack;
import NPCommon.ErrMain.Result.ResultOne;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

public class MsgDealer_GC2GS_024_001_ReqMiddayDungeonAttack extends NPUserMsgDealer<GC2GS_024_001_ReqMiddayDungeonAttack>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_001_ReqMiddayDungeonAttack _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MIDDAY_DUNGEON_ATTACK);

        ResultOne<MiddayDungeon_SettleInfo> attackResult = userData.getMiddayDungeonComponent().getDungeonInfo().attack(_msg.getHeroId(), context);
        if (!attackResult.isSucc())
        {
            _commiter.commitFailRes(attackResult.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_001_RetMiddayDungeonAttack(attackResult.getData()));
    }
}
