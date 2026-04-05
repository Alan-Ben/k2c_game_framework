package NPUSServer.GuildMsgDispather.p041_MarsExploreOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_RetMarsMineShareId;
import GC2GS.p041_MarsExploreOp.GC2GS_041_025_ReqShareMarsMineToGuildChat;
import NPCommon.ErrMain.MarsErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.MarsMineShare.GuildMarsMineShareInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 火星探险-分享矿到联盟聊天（公会侧处理器）
 * 根据矿实例ID查询联盟分享矿记录，返回其数据库ID
 */
public class RequestDealer_NP2US_R_041_025_ReqShareMarsMineToGuildChat extends _ATRequestDealer_GuildOp<GC2GS_041_025_ReqShareMarsMineToGuildChat>
{
    public RequestDealer_NP2US_R_041_025_ReqShareMarsMineToGuildChat(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_041_025_ReqShareMarsMineToGuildChat _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();

        // 通过矿实例ID查询联盟分享矿记录
        GuildMarsMineShareInfo shareInfo = guildInfo.getMarsMineShareMgr().lookupByInstanceId(_msg.getMineInstanceId());
        if (null == shareInfo)
        {
            _committer.commitFailRes(MarsErr.GUILD_MARS_MINE_SHARE_NOT_FOUND.getCode());
            return;
        }

        // 返回联盟分享矿ID，玩家服收到后发送聊天消息
        GuildOp_RetMarsMineShareId ret = new GuildOp_RetMarsMineShareId();
        ret.setGuildShareMineMsgId(shareInfo.getDbId());
        _committer.commitSucRes(ret);
    }

    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}
