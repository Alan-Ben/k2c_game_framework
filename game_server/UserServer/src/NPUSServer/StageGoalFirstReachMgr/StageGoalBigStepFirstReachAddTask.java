package NPUSServer.StageGoalFirstReachMgr;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.NPUserServer;

public class StageGoalBigStepFirstReachAddTask implements _IALSynTask
{
    private NPUserServer _m_server;
    private long _m_bigStageId;

    public StageGoalBigStepFirstReachAddTask(NPUserServer _server, long _bigStageId)
    {
        _m_server = _server;
        _m_bigStageId = _bigStageId;
    }

    @Override
    public void run()
    {
        _m_server.getUsUserMgr().broadCastMessage(US2GCWriter_007_CommOp.make_077_OnStageGoalBigStepFirstReachAdd(_m_bigStageId));
    }
}
