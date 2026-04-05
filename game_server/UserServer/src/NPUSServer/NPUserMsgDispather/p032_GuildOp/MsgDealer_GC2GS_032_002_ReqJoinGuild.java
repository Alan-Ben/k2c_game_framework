package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_002_ReqJoinGuild;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

public class MsgDealer_GC2GS_032_002_ReqJoinGuild extends NPUserMsgDealer<GC2GS_032_002_ReqJoinGuild>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_002_ReqJoinGuild _msg)
    {
        //检查玩家是否已经加入联盟
        if (_committer.getUserData().getGuildComponent().getGuildId() > 0)
        {
            _committer.commitFailRes(GuildErr.PLAYER_ALREADY_IN_GUILD.getCode());
            return;
        }

        //检查加入联盟CD
        if (!_committer.getUserData().getGuildComponent().checkJoinGuildCd())
        {
            _committer.commitFailRes(GuildErr.JOIN_GUILD_CD.getCode());
            return;
        }

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _msg.getGuildId(),
                _msg,
                _committer.getUserData().getGuildComponent().make_002_UserJoinInfo()
        );
    }
}
