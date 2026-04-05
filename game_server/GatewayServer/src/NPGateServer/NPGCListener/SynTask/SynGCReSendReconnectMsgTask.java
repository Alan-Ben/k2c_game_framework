package NPGateServer.NPGCListener.SynTask;

import ALBasicServer.ALTask._IALSynTask;
import NPGateServer.NPGCListener.NPGSGCListener;
import NPGateServer.NPGCMsgMgr.NPGCMsgItem._ANPGSMsgItem;

import java.util.ArrayList;

/******************
 * 连接对象断开连接处理
 * @author mj
 *
 */
public class SynGCReSendReconnectMsgTask implements _IALSynTask
{
    private NPGSGCListener _m_glGCListener;
    private int _m_iSendedMsgCount;

    public SynGCReSendReconnectMsgTask(NPGSGCListener _listener, int _sendedMsgCount)
    {
        _m_glGCListener = _listener;
        _m_iSendedMsgCount = _sendedMsgCount;
    }

    @Override
    public void run()
    {
        //验证消息,并获取需要返回的消息数量
        ArrayList<_ANPGSMsgItem> needSendBackMsg = _m_glGCListener.getMsgDealer().tryReSendReconnectSendBackMsg(_m_iSendedMsgCount);
        //逐个消息返回
        if (null != needSendBackMsg)
        {
            _ANPGSMsgItem tmp = null;
            for (int i = 0; i < needSendBackMsg.size(); i++)
            {
                tmp = needSendBackMsg.get(i);
                if (null == tmp)
                    continue;

                // 返回消息
                _m_glGCListener.send(tmp.makeBackProtocol());
            }
        }
    }
}
