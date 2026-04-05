package NPUSServer.GuildMsgDispather.p041_MarsExploreOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p041_MarsExploreOp.GC2GS_041_024_ReqGuildShareMineHadAttackByOthersTag;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.MarsErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.MarsMineShare.GuildMarsMineShareInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_041_024_ReqGuildShareMineHadAttackByOthersTag extends _ATRequestDealer_GuildOp<GC2GS_041_024_ReqGuildShareMineHadAttackByOthersTag>
{
    public RequestDealer_NP2US_R_041_024_ReqGuildShareMineHadAttackByOthersTag(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_041_024_ReqGuildShareMineHadAttackByOthersTag _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        GuildMarsMineShareInfo shareInfo = guildInfo.getMarsMineShareMgr().lookup(_msg.getGuildShareMineMsgId());
        if(null == shareInfo)
        {
            _committer.commitFailRes(MarsErr.GUILD_MARS_MINE_SHARE_NOT_FOUND.getCode());
            return;
        }

        //先获取矿数据，再转变队伍状态
        MarsMineSystem.GetMarsHadAttackByOtherTag(getUSServer(), shareInfo.getMineInstanceId()
                ,guildInfo.getGuildId(), (err, p) ->
                {
                    if(err > 0)
                    {
                        _committer.commitSucRes(US2GCWriter_041_MarsExploreOp.make_024_RetGuildShareMineHadAttackByOthersTag(p));
                        return;
                    }

                    _committer.commitSucRes(US2GCWriter_041_MarsExploreOp.make_024_RetGuildShareMineHadAttackByOthersTag(p));
                });
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
