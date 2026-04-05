package NPUSServer.GuildMsgDispather.p041_MarsExploreOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_041_018_RetOccupyMineInfo;
import GC2GS.p041_MarsExploreOp.GC2GS_041_018_ReqGuildMateForwardCollectMine;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.MarsErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.MarsMineShare.GuildMarsMineShareInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUserServer;

/**
 * 获取联盟副本信息
 */
public class RequestDealer_NP2US_R_041_018_ReqGuildMateForwardCollectMine extends _ATRequestDealer_GuildOp<GC2GS_041_018_ReqGuildMateForwardCollectMine>
{
    public RequestDealer_NP2US_R_041_018_ReqGuildMateForwardCollectMine(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_041_018_ReqGuildMateForwardCollectMine _msg)
    {
        GuildInfo guildInfo = _committer.getGuildInfo();
        if(null == guildInfo)
        {
            _committer.commitFailRes(GuildErr.GUILD_NOT_EXIST.getCode());
            return;
        }

        GuildMarsMineShareInfo shareInfo = guildInfo.getMarsMineShareMgr().lookup(_msg.getShareMineDbId());
        if(null == shareInfo)
        {
            _committer.commitFailRes(MarsErr.GUILD_MARS_MINE_SHARE_NOT_FOUND.getCode());
            return;
        }

        //请求矿的具体信息
        //先获取矿数据，再转变队伍状态
        MarsMineSystem.GetMarsMine(getUSServer(), shareInfo.getMineInstanceId(), (err, _mineObj) ->
        {
            if(err > 0)
            {
                // 火星矿不存在的特殊处理，需要移除该数据
                if(err == MarsErr.MARS_MINE_NOT_FOUND.getCode())
                {
                    //如果是联盟分享矿，需要从联盟分享列表中移除
                    guildInfo.getMarsMineShareMgr().removeByInstanceId(_msg.getShareMineDbId());
                }

                _committer.commitFailRes(err);
                return;
            }

            //判断是否有其他公会玩家占有，如果是非pvp且是其他公会玩家占有，则报错
            if (_mineObj.getGuildId() == guildInfo.getGuildId())
            {
                _committer.commitFailRes(MarsErr.MARS_MINE_ALREADY_OCCUPY_GUILD_MATE.getCode());
                return;
            }

            GuildOp_041_018_RetOccupyMineInfo retInfo = new GuildOp_041_018_RetOccupyMineInfo();
            retInfo.setPosId(shareInfo.getPosId());
            retInfo.setMineInfo(_mineObj);

            _committer.commitSucRes(retInfo);
        });
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}
