package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import Common.NpChatObj.Common_ChatContent_GuildRecruit;
import GC2GS.p032_GuildOp.GC2GS_032_027_ReqOpenRecruit;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;

public class MsgDealer_GC2GS_032_027_ReqOpenRecruit extends NPUserMsgDealer<GC2GS_032_027_ReqOpenRecruit>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_027_ReqOpenRecruit _msg)
    {
        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<Common_ChatContent_GuildRecruit>(_committer) {
                    @Override
                    protected Common_ChatContent_GuildRecruit _createNewTmpObj() {
                        return new Common_ChatContent_GuildRecruit();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, Common_ChatContent_GuildRecruit _msg) {
                        //对全服聊天频道发送消息
                        _commiter.getUserData().getPlayerChatRoomDealer().sendRoomMsg(ENPChatRoomType.US_SERVER.ordinal(), 0,
                                ENPChatMsgType.GUILD_RECRUIT.ordinal(), _commiter.getUserData().toChatPlayerProto().makePackage(), _msg.makePackage(), null);

                        //返回操作结果
                        _commiter.commitSucRes(US2GCWriter_032_GuildOp.make_027_RetOpenRecruit());
                    }
                },
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                null
        );
    }
}
