package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_034_ReqGuildDispatchHero;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_005_HeroDispatch;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 处理联盟英雄派遣请求
 */
public class RequestDealer_NP2US_R_032_034_ReqGuildDispatchHero extends _ATRequestDealer_GuildOp<GC2GS_032_034_ReqGuildDispatchHero>
{
    public RequestDealer_NP2US_R_032_034_ReqGuildDispatchHero(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_034_ReqGuildDispatchHero _msg)
    {
        //结构附加信息
        GuildOp_005_HeroDispatch userJoinInfo = new GuildOp_005_HeroDispatch();
        userJoinInfo.readPackage(_committer.getAddInfo());

        //加入联盟
        Result result = _committer.getGuildInfo().getHeroDispatchMgr().dispatchHero(_committer.getCid()
            , userJoinInfo.getHeroId(), userJoinInfo.getDispatchValue(), userJoinInfo.getLevel(), userJoinInfo.getPower(), userJoinInfo.getSkinId());

        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());

            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_002_RetJoinGuild());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }

}