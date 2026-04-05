package NPCommon.CommonRank.DBTask;

import ALBasicServer.ALTask._IALAsynRunnableTask;
import ALMySqlCommon.ALMySqlDBConditionObj;
import NPCommon.CommonRank._IRankDBOper;

/***************
 * 跟新排行单级数据的执行任务
 * @author mj
 *
 */
public class AsynDelRankSubObjScoreTask implements _IALAsynRunnableTask
{
    //管理器
    private _IRankDBOper _m_rlmRankListMgr;

    //数据Id
    private long _m_lDBId;

    public AsynDelRankSubObjScoreTask(_IRankDBOper _rankListMgr, long _dbId)
    {
        _m_rlmRankListMgr = _rankListMgr;

        _m_lDBId = _dbId;
    }

    @Override
    public void run()
    {
        if (null == _m_rlmRankListMgr)
            return;

        //条件
        ALMySqlDBConditionObj condObj = new ALMySqlDBConditionObj();
        condObj.setTablesName(_m_rlmRankListMgr.getRankSubObjTableName());
        condObj.addAndEquals("id", _m_lDBId);

        //操作数据库
        _m_rlmRankListMgr.getDBObj().delByCondition(condObj);
    }

}
