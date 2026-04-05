package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_008_ReqChgGuildFlag;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_032_008_ReqChgGuildFlag extends NPUserMsgDealer<GC2GS_032_008_ReqChgGuildFlag>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_008_ReqChgGuildFlag _msg)
    {
        //检查道具是否足够
        if (!_committer.getUserData().hasCostItemList(RefGeneral.Ref().guild_change_flag_cost))
        {
            _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        //消耗道具
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHANGE_GUILD_FLAG);
        if (!_committer.getUserData().spendCostItemList(RefGeneral.Ref().guild_change_flag_cost, context))
        {
            _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        GuildOp_PlayerCname addInfo = new GuildOp_PlayerCname();
        addInfo.setCname(_committer.getUserData().getPlayerComponent().getName());

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo
        );
    }
}
