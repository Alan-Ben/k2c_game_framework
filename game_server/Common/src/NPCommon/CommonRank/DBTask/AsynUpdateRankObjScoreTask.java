package NPCommon.CommonRank.DBTask;

import ALBasicServer.ALTask._IALAsynRunnableTask;
import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import NPCommon.CommonRank._IRankDBOper;

/***************
 * 跟新排行单级数据的执行任务
 * @author mj
 *
 */
public class AsynUpdateRankObjScoreTask implements _IALAsynRunnableTask
{
    //管理器
    private _IRankDBOper _m_rlmRankListMgr;

    //数据Id
    private long _m_lDBId;

    //新数据u
    private long _m_lNewScoreSourceId;
    //新数据u
    private long _m_lNewScore;
    //时间戳
    private long _m_lUpdateMs;

    public AsynUpdateRankObjScoreTask(_IRankDBOper _rankListMgr, long _dbId, long _newScoreSourceId, long _newScore, long _updateMs)
    {
        _m_rlmRankListMgr = _rankListMgr;

        _m_lDBId = _dbId;
        _m_lNewScoreSourceId = _newScoreSourceId;
        _m_lNewScore = _newScore;
        _m_lUpdateMs = _updateMs;
    }

    @Override
    public void run()
    {
        if (null == _m_rlmRankListMgr)
            return;

        //条件
        ALMySqlDBConditionObj condObj = new ALMySqlDBConditionObj();
        condObj.setTablesName(_m_rlmRankListMgr.getRankObjTableName());
        condObj.addAndEquals("id", _m_lDBId);

        //操作
        ALMySqlUpdateValue updateV = new ALMySqlUpdateValue();
        updateV.addValueObj(_m_rlmRankListMgr.getRankObjScoreSourceIdName(), _m_lNewScoreSourceId);
        updateV.addValueObj(_m_rlmRankListMgr.getRankObjScoreName(), _m_lNewScore);
        updateV.addValueObj(_m_rlmRankListMgr.getRankObjUpdatedMsName(), _m_lUpdateMs);

        //操作数据库
        _m_rlmRankListMgr.getDBObj().updateByCondition(condObj, updateV);
    }

}
