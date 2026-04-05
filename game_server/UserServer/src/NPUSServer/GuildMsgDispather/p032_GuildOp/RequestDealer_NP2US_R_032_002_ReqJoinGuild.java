package NPUSServer.GuildMsgDispather.p032_GuildOp;

import GC2GS.p032_GuildOp.GC2GS_032_002_ReqJoinGuild;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_002_UserJoinInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPUSServer.GuildMsgDispather.GuildMsgCommiter.GuildMsgCommiter;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/**
 * 申请加入联盟
 */
public class RequestDealer_NP2US_R_032_002_ReqJoinGuild extends _ABasicGeneralRequestDealer<GC2GS_032_002_ReqJoinGuild>
{
    public RequestDealer_NP2US_R_032_002_ReqJoinGuild(NPUserServer _server)
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, GC2GS_032_002_ReqJoinGuild _msg)
    {
        //获取联盟对象
        if(!(_committer instanceof GuildMsgCommiter))
        {
            _committer.commitFailRes(CommErr.SYS_ERR.getCode());
            return;
        }

        GuildMsgCommiter guildMsgCommiter = (GuildMsgCommiter)_committer;
        //结构附加信息
        GuildOp_002_UserJoinInfo userJoinInfo = new GuildOp_002_UserJoinInfo();
        userJoinInfo.readPackage(guildMsgCommiter.getAddInfo());

        //加入联盟
        Result result = getUSServer().getGuildMgr().joinGuild(_msg.getGuildId(), guildMsgCommiter.getCid(), userJoinInfo);
        if (!result.isSucc())
        {
            _committer.commitFailRes(result.getCode());
            return;
        }

        _committer.commitSucRes(US2GCWriter_032_GuildOp.make_002_RetJoinGuild());
    }

}