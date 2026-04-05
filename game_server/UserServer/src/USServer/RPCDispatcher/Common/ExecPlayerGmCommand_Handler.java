package USServer.RPCDispatcher.Common;

import AllRpcData.US_Service.Common.ExecPlayerGmCommand;
import Common.OfflineRewardEnum.EOfflineRewardEnum;
import Common.OfflineRewardObj.Offline_PlayerGmCommand;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.GMCommand.UsCmdPlayerExecutor;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USDB.Bo.PlayerOfflineRewardBO;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

public class ExecPlayerGmCommand_Handler extends _ATBasicUSRpc_Handler<ExecPlayerGmCommand> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, ExecPlayerGmCommand _rpc)
    {
        _usServer.getLoaderMgr().safeCall(() ->
        {
            NPUSUserData userData = _usServer.getUsUserMgr().lookupCacheUserData(_rpc.req().getCid());
            if (null == userData)
            {
                //保存为离线后上线需要处理的数据
                Offline_PlayerGmCommand offlinePlayerGmCommand = new Offline_PlayerGmCommand();
                offlinePlayerGmCommand.setCommand(_rpc.req().getCommand());

                PlayerOfflineRewardBO bo = new PlayerOfflineRewardBO();
                bo.setCid(_usServer.getBM(), _rpc.req().getCid());
                bo.setRewardType(_usServer.getBM(), EOfflineRewardEnum.GM.ordinal());
                bo.setOfflineData(_usServer.getBM(), CommonFunc.ByteBfferToBytes(offlinePlayerGmCommand.makePackage()));
                bo.insert(_usServer.getBM());

                _rpc.retObj().setIsSucc(true);
                _rpc.retObj().setResult("Add Offline GM Suc!");

                _rpc.commit();
            } else
            {
                userData.safeCall(() ->
                {
                    //执行GM命令
                    final NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GM_CMD);
                    UsCmdPlayerExecutor execContext = new UsCmdPlayerExecutor(userData);

                    GmCommandMgr.getInstance().run(execContext, _rpc.req().getCommand(), context, (_bSucc, _result) ->
                    {
                        _rpc.retObj().setIsSucc(_bSucc);
                        _rpc.retObj().setResult(_result);
                        _rpc.commit();
                    });
                });
            }
        });
    }
}
