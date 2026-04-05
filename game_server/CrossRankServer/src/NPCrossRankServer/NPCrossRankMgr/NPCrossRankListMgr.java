package NPCrossRankServer.NPCrossRankMgr;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import CRSDB.Bo.NpCrossRankBO;
import CRSDB.Bo.NpCrossRankJoinerBO;
import CRSDB.Bo.NpCrossRankObjBO;
import CRSDB.Bo.NpCrossRankSubObjBO;
import NPCommon.CommonRank.RankObj;
import NPCommon.CommonRank.RankSubObj;
import NPCommon.CommonRank._ATRankListMgr;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceInfo;
import NPCrossRankServer.CrossInstanceMgr.NPCrossInstanceMgr;
import NPCrossRankServer.NPCrossRankServer;
import NPGameRes.Refs.Rank.RefRank;

import java.util.ArrayList;
import java.util.List;

/**
 * 本服静态排行榜
 * @author mark
 */
public class NPCrossRankListMgr extends _ATRankListMgr<NPCrossRankRankList, NpCrossRankBO, NpCrossRankObjBO, NpCrossRankSubObjBO> implements _IHandlerHolder
{
    private static NPCrossRankListMgr _g_instance = new NPCrossRankListMgr();
    public static NPCrossRankListMgr getInstance()
    {
        return _g_instance;
    }

    //对应的数据对象
    private _TALMySqlSafeOpDBObj<WCGDBObj> _m_dbObj;

    public NPCrossRankListMgr()
    {
        _m_dbObj = WCGDBFactory.getDbObj(EDBTag.crossrank_main);
    }

    public boolean initFromDB()
    {
        //排行榜数据
        List<NpCrossRankBO> boList = NPCrossRankServer.getInstance().getBM().getBM(NpCrossRankBO.class).s_findAll();
        if (null == boList)
        {
            CommLog.error("rank bo[NpCrossRankBO]  init fail.");
            return false;
        }
        
        //遍历创建排行榜对象，并注册到对应集合中
        //需要后续直接关闭的排行列表数据集
        ArrayList<NPCrossRankRankList> needCloseRankList = new ArrayList<NPCrossRankRankList>();
        for (int i = 0; i < boList.size(); i++)
        {
            NpCrossRankBO bo = boList.get(i);
            if (null == bo)
                continue;

            //创建排行榜数据
            NPCrossRankRankList rankList = _initRankList(bo);
            if (null == rankList)
            {
                CommLog.error("rank:{} instanceId:{} init fail, init fail.", bo.getRankId(), bo.getId());
                continue;
            }

            //注册到对应的跨服集合管理器中
            NPCrossInstanceInfo crossInstanceInfo = NPCrossInstanceMgr.getInstance().ensureCrossInstance(bo.getCrossInstanceId());
            if(null == crossInstanceInfo)
            {
                CommLog.error("rank:{} instanceId:{} init fail, for ensure cross instance id:{}.", bo.getRankId(), bo.getId(), bo.getCrossInstanceId());
                continue;
            }
            
            //将排行榜放入集群中
            crossInstanceInfo.initAddRankList(rankList);
            
            //如果排行无效，则直接加入需要关闭列表
            if(!rankList.isEnable())
            {
                needCloseRankList.add(rankList);
            }
        }

        ////// 初始化排行一级数据对象
        List<NpCrossRankObjBO> objBoList = NPCrossRankServer.getInstance().getBM().getBM(NpCrossRankObjBO.class).s_findAll();
        if (null == objBoList)
        {
            CommLog.error("rank bo[NpCrossRankObjBO]  init fail.");
            return false;
        }
        
        //初始化数据库数据
        _initFromRBOList(objBoList);

        ////// 初始化排行二级数据对象
        List<NpCrossRankSubObjBO> subObjBoList = NPCrossRankServer.getInstance().getBM().getBM(NpCrossRankSubObjBO.class).s_findAll();
        if (null == subObjBoList)
        {
            CommLog.error("rank bo[NpCrossRankSubObjBO]  init fail.");
            return false;
        }
        
        //初始化数据库数据
        _initFromRSBOList(subObjBoList);

        ////// 初始化排行参与者数据
        List<NpCrossRankJoinerBO> joinerBoList = NPCrossRankServer.getInstance().getBM().getBM(NpCrossRankJoinerBO.class).s_findAll();
        if (null == joinerBoList)
        {
            CommLog.error("rank bo[NpCrossRankJoinerBO]  init fail.");
            return false;
        }

        //初始化数据库数据
        _initFromJoinerBOList(joinerBoList);

        //关闭需要关闭的列表，避免部分数据无法放入排行榜而在数据库驻留
        for(int i = 0; i < needCloseRankList.size(); i++)
        {
            closeRankList(needCloseRankList.get(i).getInstanceId());
        }
        needCloseRankList.clear();

        //设置初始化完成
        setRankListMgrInitDone();

        return true;
    }

    //====================数据库接口部分 - 开始

    @Override
    public int getDBTaskThreadIndex()
    {
        if (null == _m_dbObj)
            return 0;

        return _m_dbObj.getThreadIdx();
    }

    @Override
    public _TALMySqlSafeOpDBObj<WCGDBObj> getDBObj() { return _m_dbObj; }
    
    //rank主要信息表数据
    @Override
    public String getRankInfoTableName(){ return "np_cross_rank"; }

    @Override
    public String getRankObjTableName() { return "np_cross_rank_obj"; }
    @Override
    public String getRankObjScoreSourceIdName() { return "score_source_id"; }
    @Override
    public String getRankObjScoreName() { return "score"; }
    @Override
    public String getRankObjUpdatedMsName() { return "updatedMs"; }

    @Override
    public String getRankSubObjTableName() { return "np_cross_rank_sub_obj"; }
    @Override
    public String getRankSubObjScoreSourceIdName() { return "score_source_id"; }
    @Override
    public String getRankSubObjScoreName() { return "score"; }
    @Override
    public String getRankSubObjUpdatedMsName() { return "updatedMs"; }

    //====================数据库接口部分 - 完结


    /**
     * 初始化排行参与者数据
     * @param _joinerBoList 参与者数据
     */
    protected void _initFromJoinerBOList(List<NpCrossRankJoinerBO> _joinerBoList)
    {
        for (NpCrossRankJoinerBO joinerBo : _joinerBoList)
        {
            if (null == joinerBo)
                continue;

            //获取对应的排行榜对象
            NPCrossRankRankList rankList = _m_mapRankObjListMap.get(joinerBo.getCrossRankInstanceId());
            if (null == rankList)
            {
                CommLog.error("Can not find rank list for joiner: " + joinerBo.getId());
                continue;
            }

            //添加参与者
            rankList._initFromJoinerBO(joinerBo);
        }
    }

    /****************
     * 根据排行Id创建对应的排行榜对象
     * @return
     */
    @Override
    protected NPCrossRankRankList _createRankList(NpCrossRankBO _rankBo)
    {
        if(null == _rankBo)
            return null;
        
        //获取静态数据
        RefRank rankRef = RefRank.getMgr().get(_rankBo.getRankId());
        if(null == rankRef)
        {
            ALServerLog.Fatal("Can not find rank: " + _rankBo.getRankId());
            return null;
        }
        
        //创建排行榜对象
        return new NPCrossRankRankList(this, _rankBo, rankRef, new NPCommRankComparer_ScoreGreater());
    }



    /******************
     * 创建一个新数据对象返回，在进行子集排行处理的时候不会通过本函数创建排行数据对象
     * @return
     */
    @Override
    protected long _getRankListIdForRBO(NpCrossRankObjBO _bo)
    {
        if (null == _bo)
            return 0;
        return _bo.getInstanceId();
    }
    @Override
    protected RankObj _createInitRankObj(NpCrossRankObjBO _bo)
    {
        if (null == _bo)
            return null;

        return new RankObj(_bo.getId(), _bo.getObjId(), _bo.getScoreSourceId(), _bo.getScore(), _bo.getUpdatedMs(), 0);
    }

    /******************
     * 创建一个新的子集数据对象返回
     * @return
     */
    @Override
    protected long _getRankListIdForRSBO(NpCrossRankSubObjBO _bo)
    {
        if (null == _bo)
            return 0;
        return _bo.getInstanceId();
    }
    @Override
    protected long _getRankObjIdForRSBO(NpCrossRankSubObjBO _bo)
    {
        if (null == _bo)
            return 0;
        return _bo.getObjId();
    }
    @Override
    protected RankSubObj _createInitRankSubObj(NpCrossRankSubObjBO _bo)
    {
        if (null == _bo)
            return null;

        return new RankSubObj(_bo.getId(), _bo.getSubObjId(), _bo.getScoreSourceId(), _bo.getScore(), _bo.getUpdatedMs());
    }
   
    //==================== 基类业务完毕
    
}
