package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.NpChatObj.NPCommon_ChatContent_Text;
import GC2GS.p032_GuildOp.GC2GS_032_015_ReqGuildBroadcastMessage;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerSendChatInfo;
import NPEnum.ENPChatMsgType;
import NPUSServer.ChatSys.ChatRoomApi;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

/**
 * 发送联盟广播消息
 */
public class RequestDealer_NP2US_R_032_015_ReqGuildBroadcastMessage extends _ATRequestDealer_GuildOp<GC2GS_032_015_ReqGuildBroadcastMessage>
{
    public RequestDealer_NP2US_R_032_015_ReqGuildBroadcastMessage(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_015_ReqGuildBroadcastMessage _msg)
    {
        NPCommon_ChatContent_Text proto = new NPCommon_ChatContent_Text();
        proto.setContent(_msg.getMessage());

        //结构附加信息
        GuildOp_PlayerSendChatInfo addInfo = new GuildOp_PlayerSendChatInfo();
        addInfo.readPackage(_committer.getAddInfo());

        //判断传入的成员列表是否有效
        for (Long cid : _msg.getCidList())
        {
            GuildMemberInfo memberInfo = _committer.getGuildInfo().getMemberMgr().lookup(cid);
            if (memberInfo == null)
                continue;

            //不能给自己发私聊信息
            if(memberInfo.getCid() == _committer.getCid())
                continue;

            //发送私聊
            ChatRoomApi.sendPrivateMsg(getUSServer(), addInfo.getChatUid(), cid, ENPChatMsgType.GUILD_PRIVATE_INFORM.ordinal(),
                    addInfo.getPlayerContent().makePackage(), proto.makePackage(),
                    (_result, Long) ->
                    {
                        if (!_result.isSucc())
                        {
                            USLog.error(getUSServer(),
                                    "GuildMsgDealer_GC2GS_032_015_ReqGuildBroadcastMessage sendPrivateMsg fail. send:{} receive:{}", cid, memberInfo.getCid());
                        }
                    });
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_015_RetGuildBroadcastMessage());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.BROADCAST_MESSAGE;
    }
}