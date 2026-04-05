package NPUSServer.NPUserMsgDispather.p013_HeroOp;

import GC2GS.p013_HeroOp.GC2GS_013_023_ReqEquipDisassemble;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_013_HeroOp;

public class MsgDealer_GC2GS_013_023_ReqEquipDisassemble extends NPUserMsgDealer<GC2GS_013_023_ReqEquipDisassemble>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_013_023_ReqEquipDisassemble _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData == null)
            return;

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.EQUIP_DISASSEMBLE);

        Result result = _committer.getUserData().getEquipComponent().disassemble(_msg.getDbList(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.getUserData().sendMsgToGC(context.getCollector().toProto());

        _committer.commitSucRes(US2GCWriter_013_HeroOp.make_023_RetEquipDisassemble());
    }
}