package NPUSServer.NPUSUserMgr.SynTask;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.UserMsgMgr.NPUSUserMsgMgr;

/*******************
 * 用户消息处理对象
 * @author Administrator
 *
 */
public class NPSynUserDealMsgTask implements _IALSynTask
{
    /**
     * 用户数据对象
     */
    private NPUSUserMsgMgr _m_udUserMsgMgr;

    public NPSynUserDealMsgTask(NPUSUserMsgMgr _userMsgMgr)
    {
        _m_udUserMsgMgr = _userMsgMgr;
    }

    @Override
    public void run()
    {
        if (null == _m_udUserMsgMgr)
            return;

        if (_m_udUserMsgMgr.dealMessage())
            ALSynTaskManager.getInstance().regTask(this);

    }

}
