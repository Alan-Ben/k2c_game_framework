package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import GC2GS.p032_GuildOp.GC2GS_032_017_ReqGuildConstruct;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPEnum.ENpRewardShowType;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 每日联盟建设
 */
public class RequestDealer_NP2US_R_032_017_ReqGuildConstruct extends _ATRequestDealer_GuildOp<GC2GS_032_017_ReqGuildConstruct>
{
    public RequestDealer_NP2US_R_032_017_ReqGuildConstruct(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_017_ReqGuildConstruct _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_CONSTRUCT);

        //结构附加信息
        GuildOp_PlayerCname addInfo = new GuildOp_PlayerCname();
        addInfo.readPackage(_committer.getAddInfo());

        //获取成员信息
        GuildMemberInfo memberInfo = _committer.getGuildInfo().getMemberMgr().lookup(_committer.getCid());
        if(null == memberInfo)
        {
            _committer.commitFailRes(GuildErr.MEMBER_NOT_FOUND.getCode());
            return;
        }

        Result result = _committer.getGuildInfo().construct(addInfo, memberInfo, _msg.getRefId(), context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        //建造奖励展示弹窗推送
        getUSServer().sendMsgToGC(_committer.getCid(), context.getCollector().toProto(ENpRewardShowType.TIP));

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_017_RetGuildConstruct());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return null;
    }
}