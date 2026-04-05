package NPUSServer.GuildMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_004_ReqOtherGuildInfo;
import NPCommon.ErrMain.CommErr;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 申请其他联盟信息
 */
public class RequestDealer_NP2US_R_032_004_ReqOtherGuildInfo extends _ABasicGeneralRequestDealer<GC2GS_032_004_ReqOtherGuildInfo>
{
    public RequestDealer_NP2US_R_032_004_ReqOtherGuildInfo(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, GC2GS_032_004_ReqOtherGuildInfo _msg)
    {
        //获取联盟对象
        if(!(_committer instanceof GuildMsgCommiter))
        {
            _committer.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        GuildMsgCommiter guildMsgCommiter = (GuildMsgCommiter)_committer;

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_004_RetOtherGuildInfo(
                guildMsgCommiter.getGuildInfo().makeShowInfo()
                , guildMsgCommiter.getGuildInfo().getMemberMgr().makeProtoMemberList()));
    }

}