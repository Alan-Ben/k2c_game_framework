package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import Common.NpChatObj.NPCommon_ChatContent_MarsExploreMineShare;
import Common.NpChatObj.NPCommon_ChatPlayerContent;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetMarsMineShareId;
import GC2GS.p041_MarsExploreOp.GC2GS_041_025_ReqShareMarsMineToGuildChat;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.MarsErr;
import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter._ATGuildUserMsgRedirectCommiter;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsMineComp.MarsMineInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.USLog;

/**
 * 火星探险-分享矿到联盟聊天
 * 流程：本地校验 -> 转发到联盟服查询 guildShareMineMsgId -> 回到玩家服发送玩家聊天消息
 */
public class MsgDealer_GC2GS_041_025_ReqShareMarsMineToGuildChat extends NPUserMsgDealer<GC2GS_041_025_ReqShareMarsMineToGuildChat>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_025_ReqShareMarsMineToGuildChat _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 检查玩家是否在联盟
        long guildId = userData.getGuildComponent().getGuildId();
        if (guildId <= 0)
        {
            _commiter.commitFailRes(GuildErr.NOT_MEMBER_OF_GUILD.getCode());
            return;
        }

        // 校验本地矿数据存在
        MarsMineInfo mineInfo = userData.getMarsMineComponent().lookup(_msg.getMineInstanceId());
        if (null == mineInfo)
        {
            _commiter.commitFailRes(MarsErr.MARS_MINE_NOT_FOUND.getCode());
            return;
        }

        // 转发到联盟服查询 guildShareMineMsgId，回调在玩家服处理
        getUSServer().dealGuildMsgByRedirectCommiter(
                new _ATGuildUserMsgRedirectCommiter<GuildOp_RetMarsMineShareId>(_commiter)
                {
                    @Override
                    protected GuildOp_RetMarsMineShareId _createNewTmpObj()
                    {
                        return new GuildOp_RetMarsMineShareId();
                    }

                    @Override
                    protected void _dealTmpCommitMsg(_ANPUSUserBasicMsgItem _commiter, GuildOp_RetMarsMineShareId _retMsg)
                    {
                        // 构造聊天消息内容
                        NPCommon_ChatPlayerContent userProto = getUserData().toChatPlayerProto();

                        NPCommon_ChatContent_MarsExploreMineShare content = new NPCommon_ChatContent_MarsExploreMineShare();
                        content.setGuildShareMineMsgId(_retMsg.getGuildShareMineMsgId());
                        content.setRefId(mineInfo.getRefId());

                        // 以玩家身份发送到联盟聊天频道
                        _commiter.getUserData().getPlayerChatRoomDealer().sendRoomMsg(ENPChatRoomType.GUILD.ordinal(), 0,
                                ENPChatMsgType.SHARE_MARS_EXPLORE_MINE.ordinal(), userProto.makePackage(), content.makePackage(),
                                (_result) ->
                                {
                                    if (!_result.isSucc())
                                    {
                                        USLog.error(getUSServer(), "MsgDealer_GC2GS_041_025 - send guild chat fail: cid={}, mineInstanceId={}, guildId={}",
                                                userData.getCid(), _msg.getMineInstanceId(), guildId);
                                        _commiter.commitFailRes(_result.getCode());
                                        return;
                                    }

                                    _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_025_RetShareMarsMineToGuildChat());
                                });
                    }
                },
                userData.getCid(),
                guildId,
                _msg,
                null
        );
    }
}
