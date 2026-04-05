package USServer.RPCDispatcher.Mars;

import AllRpcData.US_Service.Mars.MarsRallySInitedCheck;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.GuildRallyErr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Rally.GuildRallyInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/**
 * 启动阶段集结状态校验RPC处理器
 *
 * 校验规则：
 * 1. rally不存在返回RALLY_NOT_FOUND
 * 2. rally存在但成员cid不存在返回MEMBER_NOT_FOUND
 */
public class MarsRallySInitedCheck_Handler extends _ATBasicUSRpc_Handler<MarsRallySInitedCheck> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, MarsRallySInitedCheck _rpc)
    {
        long cid = _rpc.req().getCid();
        long guildId = _rpc.req().getGuildId();
        long rallyId = _rpc.req().getRallyId();

        if (cid <= 0 || guildId <= 0 || rallyId <= 0)
        {
            _rpc.commitFail(CommErr.PARAM_ERROR.getCode());
            return;
        }

        GuildInfo guildInfo = _usServer.getGuildMgr().lookupGuild(guildId);
        if (guildInfo == null)
        {
            _rpc.commitFail(GuildRallyErr.RALLY_NOT_FOUND.getCode());
            return;
        }

        GuildRallyInfo<?> targetRally = guildInfo.getRallyMgr().lookupRally(rallyId);

        if (targetRally == null)
        {
            _rpc.commitFail(GuildRallyErr.RALLY_NOT_FOUND.getCode());
            return;
        }

        if (targetRally.lookupMemberInfo(cid) == null)
        {
            _rpc.commitFail(GuildErr.MEMBER_NOT_FOUND.getCode());
            return;
        }

        _rpc.commit();
    }
}


