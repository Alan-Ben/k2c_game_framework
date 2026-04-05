package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import Common.DungeonObj.MiddayDungeon_DrawRecord;
import GC2GS.p024_DungeonOp.GC2GS_024_006_ReqMiddayDungeonBoxDrawRecord;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

import java.util.List;

/**
 * 查询午间副本宝箱领取记录
 *
 * 功能：返回指定宝箱的所有领取记录，包含领取玩家的CID和领取时间戳
 */
public class MsgDealer_GC2GS_024_006_ReqMiddayDungeonBoxDrawRecord extends NPUserMsgDealer<GC2GS_024_006_ReqMiddayDungeonBoxDrawRecord>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_006_ReqMiddayDungeonBoxDrawRecord _msg)
    {
        // 获取宝箱领取记录列表
        List<MiddayDungeon_DrawRecord> drawRecordList = getUSServer().getMiddayDungeonMgr()
                .getBoxMgr()
                .getBoxDrawRecordList(_msg.getDbId());

        // 返回结果
        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_006_RetMiddayDungeonBoxDrawRecord(drawRecordList));
    }
}
