package NPLoginServer.NPLSQueueMgr.SynTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPLoginServer.NPLSQueueMgr.CallBack.NPPS_RBDealerRequestEnterCode;
import NPLoginServer.NPLSQueueMgr.NPLSQueueInfo;
import NPLoginServer.NPLSQueueMgr.NPLSQueueMgr;
import NPLoginServer.NPLoginServer;
import NPServerProtocolWriter.NP2PS.Request.NP2PS_R_Writer_003_LSOp;

/******************
 * 申请进入游戏服务器的连接串
 * @author Administrator
 *
 */
public class NPSynTaskRequestEnterCode implements _IALSynTask
{
    @Override
    public void run()
    {
        //从队列管理对象中取出第一个节点
        NPLSQueueInfo queueInfo = NPLSQueueMgr.getInstance().getFirstQueue();
        if (null != queueInfo)
        {
            //信息有效此时发送消息申请
            NPLoginServer.getInstance().sendRequestToPlat(
                    NP2PS_R_Writer_003_LSOp.make_001_ReqGateServer(queueInfo.getListener().getUid())
                    , new NPPS_RBDealerRequestEnterCode(queueInfo));
        } else
        {
            //此时延迟500毫秒处理
            ALSynTaskManager.getInstance().regTask(this, 500);
        }
    }

}
