package NPHttpServer.NPHSAllServerMail;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

public class SynTask_HSCheckPHPAllServerMailExpired implements _IALSynTask
{
    @Override
    public void run()
    {
        try
        {
            //全服邮件检查检查过期
            NPHSAllServerMailMgr.getInstance().checkExpired();
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        //检查邮件过期时间（5分钟1刷）
        ALSynTaskManager.getInstance().regTask(this, 600);
    }
}
