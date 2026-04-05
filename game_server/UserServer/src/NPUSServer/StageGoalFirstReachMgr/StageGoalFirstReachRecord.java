package NPUSServer.StageGoalFirstReachMgr;

import Common.StageGoalObj.StageGoal_BigStepFirstReachInfo;
import USDB.Bo.BigStageGoalFirstReachBO;

public class StageGoalFirstReachRecord
{
    private StageGoalFirstReachInfo _m_StageGoalFirstReachInfo;

    private long _m_dbdId;
    private long _m_cid;
    private long _m_reachTimeMs;

    public StageGoalFirstReachRecord(StageGoalFirstReachInfo _StageGoalFirstReachInfo, BigStageGoalFirstReachBO _bo)
    {
        _m_StageGoalFirstReachInfo = _StageGoalFirstReachInfo;
        _m_dbdId = _bo.getId();
        _m_cid = _bo.getCid();
        _m_reachTimeMs = _bo.getReachTimeMs();
    }

    public long getDbdId()
    {
        return _m_dbdId;
    }

    public StageGoal_BigStepFirstReachInfo makeProto()
    {
        StageGoal_BigStepFirstReachInfo proto = new StageGoal_BigStepFirstReachInfo();
        proto.setBigStepId(_m_StageGoalFirstReachInfo.getBigStageId());
        proto.setCid(_m_cid);
        proto.setReachTimeMs(_m_reachTimeMs);
        return proto;
    }
}
