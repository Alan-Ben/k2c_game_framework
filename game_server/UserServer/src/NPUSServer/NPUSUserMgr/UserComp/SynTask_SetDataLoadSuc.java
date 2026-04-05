package NPUSServer.NPUSUserMgr.UserComp;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.NPUSUserData;

public class SynTask_SetDataLoadSuc implements _IALSynTask
{
    private NPUSUserData _m_udUserData;

    public SynTask_SetDataLoadSuc(NPUSUserData _userData)
    {
        _m_udUserData = _userData;
    }

    @Override
    public void run()
    {
        //设置数据加载完成
        _m_udUserData.setDataLoadSuc();
    }
}
