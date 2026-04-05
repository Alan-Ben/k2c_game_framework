package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import GC2GS.p024_DungeonOp.GC2GS_024_005_ReqMiddayDungeonBoxCanDraw;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

/**
 * 查询午间副本宝箱是否可以领取
 *
 * 功能：检查指定宝箱是否可以被当前玩家领取，并返回剩余可领取次数
 */
public class MsgDealer_GC2GS_024_005_ReqMiddayDungeonBoxCanDraw extends NPUserMsgDealer<GC2GS_024_005_ReqMiddayDungeonBoxCanDraw>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_005_ReqMiddayDungeonBoxCanDraw _msg)
    {
        // 获取用户对象
        NPUSUserData userData = _commiter.getUserData();

        // 检查宝箱是否可领取
        boolean canDraw = getUSServer().getMiddayDungeonMgr().getBoxMgr().checkBoxCanDraw(_msg.getDbId(), userData.getCid());

        // 获取宝箱剩余可领取次数
        int remainDrawCount = getUSServer().getMiddayDungeonMgr().getBoxMgr().getBoxRemainDrawCount(_msg.getDbId());

        // 返回结果
        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_005_RetMiddayDungeonBoxCanDraw(canDraw, remainDrawCount));
    }
}
