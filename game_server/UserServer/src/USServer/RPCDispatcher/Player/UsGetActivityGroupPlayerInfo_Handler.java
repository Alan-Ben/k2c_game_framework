package USServer.RPCDispatcher.Player;

import ALBasicProtocolPack._IALProtocolStructure;
import AllRpcData.US_Service.Player.UsGetActivityGroupPlayerInfo;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.ErrMain.CommErr;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/**
 * UsGetActivityGroupPlayerInfo RPC处理器
 * 根据 groupId + cid 查找对应的数据提供者，在 userdata 加载完成后构建并返回玩家群组序列化数据
 */
public class UsGetActivityGroupPlayerInfo_Handler extends _ATBasicUSRpc_Handler<UsGetActivityGroupPlayerInfo> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsGetActivityGroupPlayerInfo _rpc)
    {
        long groupId = _rpc.req().getGroupId();
        long cid = _rpc.req().getCid();

        //查找对应活动
        _AActivityBase activity = _usServer.getCommActivityMgr().lookupActivityByGroupId(groupId);
        if (null == activity)
        {
            _rpc.commitFail(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 查找玩家UserData
        NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(cid);
        if (null == userData)
        {
            _rpc.commitFail(CommErr.PLAYER_NOT_FOUND.getCode());
            return;
        }

        // 等待 userdata 加载完成后再构建数据
        userData.safeCall(() ->
        {
            _IALProtocolStructure obj = activity.makePlayerObj(userData);
            if(null != obj)
            {
                _rpc.retObj().setObj(obj.makePackage());
            }

            _rpc.commit();
        });
    }
}
