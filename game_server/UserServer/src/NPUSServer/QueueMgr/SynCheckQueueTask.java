package NPUSServer.QueueMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserServer;

/*************************
 * 定时检测排队队列的检测任务
 * @author mj
 *
 */
public class SynCheckQueueTask implements _IALSynTask
{
    private NPUserServer _m_server;
    //检测操作序列号，避免检测任务多开
    private long _m_lCheckSerialize;

    public SynCheckQueueTask(NPUserServer _server, long _checkSerialize)
    {
        _m_server = _server;

        _m_lCheckSerialize = _checkSerialize;
    }

    /*********
     * 处理函数
     */
    public void run()
    {
        //检测，并根据结果处理
        if (!_m_server.getQueueMgr()._checkQueue(_m_lCheckSerialize))
            return;

        //间隔1秒处理
        ALSynTaskManager.getInstance().regTask(this, 1000);
    }
}
