package NPUSServer.GuildMsgDispather.p041_MarsExploreOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p041_MarsExploreOp.GC2GS_041_019_ReqGuildShareMineInfo;
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
public class RequestDealer_NP2US_R_041_019_ReqGuildShareMineInfo extends _ATRequestDealer_GuildOp<GC2GS_041_019_ReqGuildShareMineInfo>
{
    public RequestDealer_NP2US_R_041_019_ReqGuildShareMineInfo(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_041_019_ReqGuildShareMineInfo _msg)
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
        MarsMineSystem.GetMarsMine(getUSServer(), shareInfo.getMineInstanceId(), (err, p) ->
        {
            if(err > 0)
            {
                //火星矿不存在的特殊处理，需要移除该数据
                if(err == MarsErr.MARS_MINE_NOT_FOUND.getCode())
                {
                    //如果是联盟分享矿，需要从联盟分享列表中移除
                    guildInfo.getMarsMineShareMgr().removeByInstanceId(_msg.getGuildShareMineMsgId());
                }

                _committer.commitFailRes(err);

                return;
            }

            _committer.commitSucRes(US2GCWriter_041_MarsExploreOp.make_019_RetGuildShareMineInfo(p));
        });
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
