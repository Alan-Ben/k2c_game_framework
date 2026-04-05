package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildAddMarsBattleReport;
import Common.ServerObj.ServerObj_MarsExplorePVPLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/**
 * 新增火星矿联盟战报
 */
public class GuildAddMarsBattleReport_Handler extends _ATBasicUSRpc_Handler<GuildAddMarsBattleReport> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildAddMarsBattleReport _rpc)
    {
        _usServer.getLoaderMgr().safeCall(() ->
        {
            GuildInfo guildInfo = _usServer.getGuildMgr().lookupGuild(_rpc.req().getGuildId());
            if (null == guildInfo)
            {
                _rpc.commitFail(GuildErr.GUILD_NOT_EXIST.getCode());
                return;
            }

            ServerObj_MarsExplorePVPLog logObj = _rpc.req().getLogObj();
            if (null == logObj)
            {
                _rpc.commitFail(CommErr.OBJ_ERR.getCode());
                return;
            }

            guildInfo.getMarsMineBattleReportMgr().addBattleReport(_rpc.req().getCid(), logObj.getLogType(), logObj);
            _rpc.commit();
        });
    }
}
