package NPUSServer.NPUSUserMgr.GameSystem.MailSystem.AllServerMail;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserServer;

public class SynTask_USCheckPHPAllServerMailExpired implements _IALSynTask
{
    private NPUserServer _m_server;

    public SynTask_USCheckPHPAllServerMailExpired(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public void run()
    {
        try
        {
            //全服邮件检查检查过期
            getUSServer().getAllServerMailTemplateMgr().checkExpired();
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        //检查邮件过期时间（1天刷1次）
        ALSynTaskManager.getInstance().regTask(this, 86400000);
    }
}
