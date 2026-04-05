package NPCommon.CommonRank.DBTask;

import ALBasicServer.ALTask._IALAsynRunnableTask;
import ALMySqlCommon.ALMySqlDBConditionObj;
import NPCommon.CommonRank._IRankDBOper;

/***************
 * 跟新排行单级数据的执行任务
 * @author mj
 *
 */
public class AsynDelRankInfoTask implements _IALAsynRunnableTask
{
    //管理器
    private _IRankDBOper _m_rlmRankListMgr;
    //排行榜实例ID
    private long _m_lInstanceId;

    public AsynDelRankInfoTask(_IRankDBOper _rankListMgr, long _instanceId)
    {
        _m_rlmRankListMgr = _rankListMgr;
        _m_lInstanceId = _instanceId;
    }

    @Override
    public void run()
    {
        if (null == _m_rlmRankListMgr)
            return;

        //删除分数主对象
        ALMySqlDBConditionObj objCond = new ALMySqlDBConditionObj();
        objCond.setTablesName(_m_rlmRankListMgr.getRankInfoTableName());
        objCond.addAndEquals("id", _m_lInstanceId);
        _m_rlmRankListMgr.getDBObj().delByCondition(objCond);
    }

}
