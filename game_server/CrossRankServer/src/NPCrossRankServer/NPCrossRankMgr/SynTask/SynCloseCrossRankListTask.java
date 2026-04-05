package NPCrossRankServer.NPCrossRankMgr.SynTask;

import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankRankList;

/**********************
 * 从跨服实例组中，关闭某个排行榜对象
 * @author mj
 *
 */
public class SynCloseCrossRankListTask implements _IALSynTask
{
    //归属分组Id
    private long _m_lCrossInstanceId;
    //排行数据Id
    private long _m_lRankId;
    //归属实例Id
    private long _m_lCrossRankRankListInstanceId;
    
    public SynCloseCrossRankListTask(NPCrossRankRankList _rankList)
    {
        _m_lCrossInstanceId = _rankList.getCrossInstanceId();
        _m_lRankId = _rankList.getRankId();
        _m_lCrossRankRankListInstanceId = _rankList.getInstanceId();
    }

    @Override
    public void run()
    {
        //查询跨服排行集合信息
        NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().lookupCrossInstance(_m_lCrossInstanceId);
        if(null == crossInstanceInfo)
        {
            CommLog.error("can not find cross instance:[{}] to close rank list:[{}]", _m_lCrossInstanceId, _m_lCrossRankRankListInstanceId);
            return ;
        }
        
        //从实例组中删除排行
        crossInstanceInfo.removeRankList(_m_lRankId, _m_lCrossRankRankListInstanceId);
    }
    
}
