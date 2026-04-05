package NPCommon.DB.BoChecker;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;

public class CheckTask implements _IALSynTask
{
    private long _m_lDealSerialize;

    public CheckTask(long _dealSerialize)
    {
        _m_lDealSerialize = _dealSerialize;
    }

    @Override
    public void run()
    {
        //如果序列号不匹配则不继续处理
        if(!BoChecker.getInstance().tickCheck(_m_lDealSerialize))
            return ;

        ALSynTaskManager.getInstance().regTask(this, 1000);
    }
}
