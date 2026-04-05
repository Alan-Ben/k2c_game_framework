package NPLoginServer.NPGCListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPGC2LS.p001_BasicOp.*;
import NPLoginServer.NPGCListener.Writer.NPLS2GCWriter_001_BasicOp;
import NPLoginServer.NPLSQueueMgr.NPLSQueueInfo;
import NPLoginServer.NPLSQueueMgr.NPLSQueueMgr;

/**************
 * 客户端协议处理对象
 *
 * @author Administrator
 *
 */
public class NPLSGCDispather extends NPCustomMsgDispatcher
{
    private static NPLSGCDispather _g_instance = new NPLSGCDispather();

    public static NPLSGCDispather getInstance()
    {

        return _g_instance;
    }

    protected NPLSGCDispather()
    {
        this.regHandler(new NPCustomMsgDealer<NPGC2LS_001_001_ReqBasicInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2LS_001_001_ReqBasicInfo _msg)
            {
                // 强制转化类型对象
                NPLSGCListener listener = (NPLSGCListener) _receiver;
                if (null == listener)
                    return;

                // 返回消息
                listener.send(NPLS2GCWriter_001_BasicOp.make_001_RetBasicInfo(listener.getUid(),
                        listener.getCountryType()));

            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2LS_001_002_EnterGame>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2LS_001_002_EnterGame _msg)
            {
                // 强制转化类型对象
                NPLSGCListener listener = (NPLSGCListener) _receiver;
                if (null == listener)
                    return;

                // 将玩家加入队列后返回
                NPLSQueueInfo queueInfo = NPLSQueueMgr.getInstance().enterQueue(listener);
                if (null == queueInfo)
                {
                    // 返回失败
                    listener.send(NPLS2GCWriter_001_BasicOp.make_002_EnterGameFail());
                    return;
                }

                // 设置对象的信息
                listener.setQueue(queueInfo);
                // 返回进入队列消息
                listener.send(NPLS2GCWriter_001_BasicOp.make_004_EnterQueue(queueInfo.getQueueIdx()));
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2LS_001_003_CancelQueue>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2LS_001_003_CancelQueue _msg)
            {
                // 强制转化类型对象
                NPLSGCListener listener = (NPLSGCListener) _receiver;
                if (null == listener)
                    return;

                // 退出队列
                listener.quitQueue();
                // 返回进入队列消息
                listener.send(NPLS2GCWriter_001_BasicOp.make_003_CancelQueueSuc());
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2LS_001_005_ReqQueueHeadIdx>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2LS_001_005_ReqQueueHeadIdx _msg)
            {
                // 强制转化类型对象
                NPLSGCListener listener = (NPLSGCListener) _receiver;
                if (null == listener)
                    return;

                //获取第一个节点
                NPLSQueueInfo firstQueueInfo = NPLSQueueMgr.getInstance().getFirstQueue();
                if (null == firstQueueInfo)
                {
                    // 返回进入队列消息
                    listener.send(NPLS2GCWriter_001_BasicOp.make_005_RetQueueHeadIdx(-1));
                } else
                {
                    // 返回进入队列消息
                    listener.send(NPLS2GCWriter_001_BasicOp.make_005_RetQueueHeadIdx(firstQueueInfo.getQueueIdx()));
                }
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2LS_001_006_ReqEnterSNCode>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2LS_001_006_ReqEnterSNCode _msg)
            {
                // 强制转化类型对象
                final NPLSGCListener listener = (NPLSGCListener) _receiver;
                if (null == listener)
                    return;

                //TODO: 待进行激活码的处理，如果需要做激活码相关功能在此处
            }
        });
    }
}
