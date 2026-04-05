package NPCrossGameServer;

import ALBasicCommon.ALSerializeMaker;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NP2CS_R.np_p002_serverInfoOp.NP2CS_R_002_010_ReqSetCrossGameWeight;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_010_RetSetCrossGameWeight;
import NPCommon.Log.CommLog;
import NPCrossGameServer.NPCrossGameCore.CrossGameCategoryMgr;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class SynTask_CalCrossGameWeight implements _IALSynTask
{
    private static SynTask_CalCrossGameWeight _g_instance = new SynTask_CalCrossGameWeight();

    public static SynTask_CalCrossGameWeight getInstance()
    {
        return _g_instance;
    }

    //当前处理序列号
    private long _m_lSerial;
    //正在处理中标志位
    private boolean _m_bProcessing;

    /**
     * 计算服务器的权重
     * @return
     */
    public int calSeverWeight()
    {
        return CrossGameCategoryMgr.getInstance().calWeight() //跨服游戏实例的权重和
                ;
    }

    @Override
    public void run()
    {
        //有外部新请求则重新生成序列号
        _m_lSerial = ALSerializeMaker.makeNewSerialize();

        //正在处理，不再重复发送
        if (_m_bProcessing)
            return;
        _m_bProcessing = true;

        //记录当前处理的序列号
        final long curSerial = _m_lSerial;

        //向CS发起请求
        NP2CS_R_002_010_ReqSetCrossGameWeight proto = new NP2CS_R_002_010_ReqSetCrossGameWeight();
        proto.setServerTypeId(NPCrossGameServer.getInstance().getServerTypeId());
        proto.setServerHandleWeight(calSeverWeight());

        NPCrossGameServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal()
                , ENPSingleServerType.COMMON.ordinal()
                , proto
                , new _IWCGCallbackDealer()
                {

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        onRunOver(true, curSerial);

                        CommLog.info("Send To CS cur Weigth Suc:{}", proto.getServerHandleWeight());
                    }

                    @Override
                    public void dealFail(int paramInt)
                    {
                        onRunOver(false, curSerial);

                        CommLog.error("Send To CS for Cross-Game Weight Fail, {}", paramInt);
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_010_RetSetCrossGameWeight();
                    }
                });
    }

    /**
     * 处理回调
     * @param _isSuc
     * @param _curSerial
     */
    private void onRunOver(boolean _isSuc, long _curSerial)
    {
        //设置标志位
        _m_bProcessing = false;

        if (!_isSuc //本次回调失败
                || _curSerial < _m_lSerial //权重更新，需要再次更新
        )
            //5秒重试
            ALSynTaskManager.getInstance().regTask(this, 5000);
        {
            //CommLog.warn("Send To CS Cross-Game Weight Repeat!, serial:{}->{}", _curSerial, _m_lSerial);
        }
    }
}
