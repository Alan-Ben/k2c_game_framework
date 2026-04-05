package NPUSServer.GuildMsgDispather.p032_GuildOp;

import Common.GuildEnum.EGuildPermissionType;
import Common.GuildEnum.EGuildPositionType;
import GC2GS.p032_GuildOp.GC2GS_032_007_ReqGuildPositionAppoint;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.GuildMsgDispather._ATRequestDealer_GuildOp;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;

/**
 * 修改联盟职位
 */
public class RequestDealer_NP2US_R_032_007_ReqGuildPositionAppoint extends _ATRequestDealer_GuildOp<GC2GS_032_007_ReqGuildPositionAppoint>
{
    public RequestDealer_NP2US_R_032_007_ReqGuildPositionAppoint(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealGuildMessage(GuildMsgCommiter _committer, GC2GS_032_007_ReqGuildPositionAppoint _msg)
    {
        EGuildPositionType positionType = EGuildPositionType.EGuildPositionType_FromInt((int) _msg.getPositionId());
        if (positionType == null)
        {
            _committer.commitFailRes(GuildErr.GUILD_POSITION_NOT_EXIST.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_POSITION_APPOINT);

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

        //任命职位
        Result result = _committer.getGuildInfo().getMemberMgr().appointPosition(addInfo,
                memberInfo, _msg.getMemberId(), positionType, context);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_007_RetGuildPositionAppoint());
    }


    @Override
    public EGuildPermissionType getNeedPermissionType()
    {
        return EGuildPermissionType.POSITION_APPOINT;
    }
}