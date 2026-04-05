package NPUSServer.NPUserMsgDispather.p024_DungeonOp;

import Common.DungeonObj.EveningDungeon_BossInfo;
import GC2GS.p024_DungeonOp.GC2GS_024_012_ReqEveningDungeonBossInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_024_DungeonOp;

public class MsgDealer_GC2GS_024_012_ReqEveningDungeonBossInfo extends NPUserMsgDealer<GC2GS_024_012_ReqEveningDungeonBossInfo>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_024_012_ReqEveningDungeonBossInfo _msg)
    {
        EveningDungeon_BossInfo bossInfo = getUSServer().getEveningDungeonMgr().getInfo().makeBossInfo();
        _commiter.commitSucRes(US2GCWriter_024_DungeonOp.make_012_RetEveningDungeonBossInfo(bossInfo));
    }
}
