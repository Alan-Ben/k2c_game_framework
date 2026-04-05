package NPLoginServer.NPLSQueueMgr.CallBack;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NP2PS_RB.p003_LSOp.NP2PS_RB_003_001_RetGateKey;
import NPLoginServer.NPGCListener.Writer.NPLS2GCWriter_001_BasicOp;
import NPLoginServer.NPLSQueueMgr.NPLSQueueInfo;
import NPLoginServer.NPLSQueueMgr.NPLSQueueMgr;
import NPLoginServer.NPLSQueueMgr.SynTask.NPSynTaskRequestEnterCode;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

/****************
 * 请求进入key的返回回调处理对象
 * @author Administrator
 *
 */
public class NPPS_RBDealerRequestEnterCode implements _IWCGCallbackDealer
{
    private NPLSQueueInfo _m_qiQueueInfo;

    public NPPS_RBDealerRequestEnterCode(NPLSQueueInfo _queueInfo)
    {
        _m_qiQueueInfo = _queueInfo;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2PS_RB_003_001_RetGateKey();
    }

    @Override
    public void dealFail(int _errCode)
    {
        //将节点从队列中删除
        NPLSQueueInfo firstQueue = NPLSQueueMgr.getInstance().removeFirstQueue();
        if (firstQueue != _m_qiQueueInfo)
        {
            //重新放回队列
            NPLSQueueMgr.getInstance().pushBackQueue(firstQueue);
        }

        //返回失败
        _m_qiQueueInfo.getListener().send(NPLS2GCWriter_001_BasicOp.make_002_EnterGameFail());

        //延迟500毫秒开启处理任务
        ALSynTaskManager.getInstance().regTask(new NPSynTaskRequestEnterCode(), 500);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _protocol)
    {
        try
        {
            //判断对象是否有效，如对象无效则不做处理
            if (null == _m_qiQueueInfo || !_m_qiQueueInfo.enable())
                return;

            //将节点从队列中删除
            NPLSQueueInfo firstQueue = NPLSQueueMgr.getInstance().removeFirstQueue();
            if (firstQueue != _m_qiQueueInfo)
            {
                //重新放回队列
                NPLSQueueMgr.getInstance().pushBackQueue(firstQueue);
                //返回失败
                _m_qiQueueInfo.getListener().send(NPLS2GCWriter_001_BasicOp.make_002_EnterGameFail());
                return;
            }

            NP2PS_RB_003_001_RetGateKey protocolObj = (NP2PS_RB_003_001_RetGateKey) _protocol;
            //返回消息
            _m_qiQueueInfo.getListener().send(
                    NPLS2GCWriter_001_BasicOp.make_002_EnterGameSuc(protocolObj.getUid()
                            , protocolObj.getCheckCode(), protocolObj.getGateServerIp(),
                            protocolObj.getGateServerPort(),_m_qiQueueInfo.getListener().inWhitelist()));


        } finally
        {
            //获取信息成功则直接开启下一个处理
            ALSynTaskManager.getInstance().regTask(new NPSynTaskRequestEnterCode());
        }
    }

}
