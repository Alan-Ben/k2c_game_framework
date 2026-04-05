package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_045_ReqAddHeroRecoveryCount;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

/**
 * 恢复大臣使用次数处理器（通过道具）
 */
public class MsgDealer_GC2GS_032_045_ReqAddHeroRecoveryCount extends NPUserMsgDealer<GC2GS_032_045_ReqAddHeroRecoveryCount>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_045_ReqAddHeroRecoveryCount _msg)
    {
        if(_committer.getUserData().getGuildComponent().getGuildId() <= 0)
        {
            _committer.commitFailRes(GuildErr.MEMBER_NOT_FOUND.getCode());
            return ;
        }

        NPUSUserData userData = _committer.getUserData();

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_COOPERATE_RECOVER_HERO);

        // 消耗道具
        if (!userData.hasItem(RefGeneral.Ref().guild_cooperate_dispatch_time_reset_cost)
                || !userData.hasItem(ENPItemType.FIXED_CD, RefGeneral.Ref().guild_cooperate_dispatch_time_reset_limit_fix_cd_id, 1))
        {
            _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        if (!userData.spendItem(RefGeneral.Ref().guild_cooperate_dispatch_time_reset_cost, context)
                || !userData.spendItem(ENPItemType.FIXED_CD, RefGeneral.Ref().guild_cooperate_dispatch_time_reset_limit_fix_cd_id, 1, context))
        {
            _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        // 增加大臣恢复次数
        Result result = userData.getGuildCooperateComponent().addHeroRecoveryCount(_msg.getHeroId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_045_RetAddHeroRecoveryCount());
    }
}