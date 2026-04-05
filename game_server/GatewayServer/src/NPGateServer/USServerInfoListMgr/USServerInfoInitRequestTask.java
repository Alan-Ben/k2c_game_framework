package NPGateServer.USServerInfoListMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_001_RetUSInfoListInit;
import NPCommon.Log.CommLog;
import NPGateServer.NPGateServer;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * 重试任务内部类
 */
public class USServerInfoInitRequestTask implements _IALSynTask
{
    /**
     * 重试次数
     */
    public int _m_retryCount;

    public USServerInfoInitRequestTask getThis()
    {
        return this;
    }

    @Override
    public void run()
    {
        //检测是否已经初始化完成
        if (USServerIndexInfoListMgr.getInstance().isInited())
        {
            USServerIndexInfoListMgr.getInstance().setTaskIsOpen(false);
            return;
        }
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(),
                NP2CS_R_Writer_002_ServerInfoOp.make_001_ReqUSInfoListInit(), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_001_RetUSInfoListInit();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_002_001_RetUSInfoListInit proto = (NP2CS_RB_002_001_RetUSInfoListInit) _proto;
                        USServerIndexInfoListMgr.getInstance().initLoadServerList(proto.getServerIndexList());
                    }

                    @Override
                    public void dealFail(int _err)
                    {
                        CommLog.info("USServerIndexInfoListMgr USServerInfoInitRequestTask err:{} retry:{}"
                                , _err, _m_retryCount++);
                        //3秒后重试
                        ALSynTaskManager.getInstance().regTask(getThis(), 3000);
                    }
                });

    }
}