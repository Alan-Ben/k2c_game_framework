package NPLoginCheckServer.NPLCSRequestDealer;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import CommonProto.Common_R_255_001_ReqRpc;
import NP2LCS_R.p001_BasicOp.*;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.Log.CommLog;
import NPLoginCheckServer.NPInternalCheckAccMgr;
import NPLoginCheckServer.NPLCSRequestDealer.SynTask.NPSynAccCheckCodeEnterTask;
import NPLoginCheckServer.NPLCSRequestDealer.SynTask.NPSynAccEnterTask;
import NPLoginCheckServer.NPSDKLoginMgr.NPSDKLoginMgr;
import NPLoginCheckServer.RPCDispatcher.LCSRpcDispatcher;
import RPC.RPCDataFactoryMgr;
import RPC._ARPCData;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


/********************
 * 平台服务器发送来请求的处理对象
 * @author Administrator
 *
 */
public class NPLCSRequestDispather extends NPRequestDispatcher
{
    private static NPLCSRequestDispather _g_instance = new NPLCSRequestDispather();

    public static NPLCSRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPLCSRequestDispather()
    {
        regHandler(new NPRequestDealer<NP2LCS_R_001_001_ReqAccInfo>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2LCS_R_001_001_ReqAccInfo _msg)
            {
                //验证帐号信息
                ALSynTaskManager.getInstance().regTask(new NPSynAccEnterTask(_msg.getAccountName(), _msg.getAccountPass(), _committer));
            }
        });

        regHandler(new NPRequestDealer<NP2LCS_R_001_002_ReqAccInfoByChkKey>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2LCS_R_001_002_ReqAccInfoByChkKey _msg)
            {
                //根据帐号和验证串验证结果
                ALSynTaskManager.getInstance().regTask(new NPSynAccCheckCodeEnterTask(_msg.getAccountName(), _msg.getChkKey(), _committer));

            }
        });

        regHandler(new NPRequestDealer<NP2LCS_R_001_003_ReqCheatUidInfo>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2LCS_R_001_003_ReqCheatUidInfo _msg)
            {
                //验证帐号信息
                ALSynTaskManager.getInstance().regTask(new NPSynAccEnterTask(_msg.getUserName(), _msg.getAccountPass(), _committer));
            }
        });
        regHandler(new NPRequestDealer<NP2LCS_R_001_004_ReqSDKCheck>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2LCS_R_001_004_ReqSDKCheck _msg)
            {
                //验证帐号信息
                NPSDKLoginMgr.getInstance().dealSDKEnterRequest(_msg.getAccName(), _msg.getToken(), _msg.getClientIp(), _committer);
            }
        });
        regHandler(new NPRequestDealer<NP2LCS_R_001_098_ReqSetNeedCheckAcc>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2LCS_R_001_098_ReqSetNeedCheckAcc _msg)
            {
                //设置是否验证账号
                NPInternalCheckAccMgr.getInstance().setCheckAcc(_msg.getNeedCheck());
            }
        });

        regHandler(new NPRequestDealer<NP2LCS_R_001_099_ReqUpdateAcc>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2LCS_R_001_099_ReqUpdateAcc _msg)
            {
                //更新帐号信息
                NPInternalCheckAccMgr.getInstance().updateAccInfo(_msg.getUserName(), _msg.getPass());
            }
        });

        regHandler(new NPRequestDealer<Common_R_255_001_ReqRpc>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, Common_R_255_001_ReqRpc _msg)
            {
                _ARPCData rpcData = RPCDataFactoryMgr.getInstance().create(_msg.getClassId());
                if (null == rpcData)
                {
                    CommLog.error("can not read rpc Data for id:", _msg.getClassId());
                    return;
                }
                rpcData.readRequest(_msg.get_buffer_ReqBytes());
                LCSRpcDispatcher.getInstance().dispatchRpc(_committer, rpcData);
            }
        });
    }
}
