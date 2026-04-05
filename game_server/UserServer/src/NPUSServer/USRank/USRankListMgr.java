package NPUSServer.USRank;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import NPCommon.CommonRank.RankObj;
import NPCommon.CommonRank.RankSubObj;
import NPCommon.CommonRank._ATRankListMgr;
import NPCommon.DB.BM.BM;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.Refs.Rank.RefRank;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.UsRankBO;
import USDB.Bo.UsRankObjBO;
import USDB.Bo.UsRankSubObjBO;

import java.util.ArrayList;
import java.util.List;

public class USRankListMgr extends _ATRankListMgr<USRankList, UsRankBO, UsRankObjBO, UsRankSubObjBO> implements _IHandlerHolder
{
    private NPUserServer _m_usUSServer;
    private _TALMySqlSafeOpDBObj<WCGDBObj> _m_dbObj;
    private boolean _m_hasInit = false;
    
    public USRankListMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}


    /**
     * 初始化数据库对象
     */
    public void initDBObj()
    {
        if (_m_hasInit)
        {
            USLog.error(getUSServer(), "NPUSRankListMgr repeat initDBObj!");
            return;
        }

        _m_hasInit = true;
        //初始化数据库对象
        _m_dbObj = WCGDBFactory.getDbObj(_m_usUSServer.getDBInitializer().getDBTag());
    }

    /**
     * 初始化数据
     * @return
     */
    public boolean initFromDB()
    {
        ///初始化排行主体数据
        List<UsRankBO> boList = getUSServer().getBM().getBM(UsRankBO.class).s_findAll();
        if (null == boList)
        {
        	USLog.error(getUSServer(), "rank bo[UsRankBO]  init fail.");
            return false;
        }
        
        //需要后续直接关闭的排行列表数据集
        ArrayList<USRankList> needCloseRankList = new ArrayList<USRankList>();
        for (int i = 0; i < boList.size(); i++)
        {
            UsRankBO bo = boList.get(i);
            if (null == bo)
                continue;

            //创建排行榜数据
            USRankList rankList = _initRankList(bo);
            if (null == rankList)
            {
                USLog.error(getUSServer(), "rank:{} instanceId:{} init fail, init fail.", bo.getRankId(), bo.getId());
                continue;
            }
            
            //如果排行无效，则直接加入需要关闭列表
            if(!rankList.isEnable())
            {
                needCloseRankList.add(rankList);
            }
        }

        ////// 初始化排行一级数据对象
        List<UsRankObjBO> objBoList = getUSServer().getBM().getBM(UsRankObjBO.class).s_findAll();
        if (null == objBoList)
        {
        	USLog.error(getUSServer(), "rank bo[UsRankObjBO]  init fail.");
            return false;
        }
        
        //初始化数据库数据
        _initFromRBOList(objBoList);

        ////// 初始化排行二级数据对象
        List<UsRankSubObjBO> subObjBoList = getUSServer().getBM().getBM(UsRankSubObjBO.class).s_findAll();
        if (null == subObjBoList)
        {
        	USLog.error(getUSServer(), "rank bo[UsRankSubObjBO]  init fail.");
            return false;
        }
        
        //初始化数据库数据
        _initFromRSBOList(subObjBoList);

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
        return _m_usUSServer.getDBInitializer().getThreadIdx();
    }

    @Override
    public _TALMySqlSafeOpDBObj<WCGDBObj> getDBObj() { return _m_dbObj; }
    
    //rank主要信息表数据
    @Override
    public String getRankInfoTableName(){ return "us_rank"; }
    
    @Override
    public String getRankObjTableName() { return "us_rank_obj"; }
    @Override
    public String getRankObjScoreSourceIdName() { return "score_source_id"; }
    @Override
    public String getRankObjScoreName() { return "score"; }
    @Override
    public String getRankObjUpdatedMsName() { return "updatedMs"; }

    @Override
    public String getRankSubObjTableName() { return "us_rank_sub_obj"; }
    @Override
    public String getRankSubObjScoreSourceIdName() { return "score_source_id"; }
    @Override
    public String getRankSubObjScoreName() { return "score"; }
    @Override
    public String getRankSubObjUpdatedMsName() { return "updatedMs"; }

    //====================数据库接口部分 - 完结

    /****************
     * 根据排行Id创建对应的排行榜对象
     * @return
     */
    @Override
    protected USRankList _createRankList(UsRankBO _rankBo)
    {
        if(null == _rankBo)
            return null;
        
        //获取静态数据
        RefRank rankRef = RefRank.getMgr().get(_rankBo.getRankId());
        if(null == rankRef)
        {
            USLog.fatal(getUSServer(), "Can not find rank: " + _rankBo.getRankId());
            return null;
        }
        
        //创建排行榜对象
        return new USRankList(getUSServer(), this, _rankBo, rankRef, new CommRankComparer_ScoreGreater());
    }



    /******************
     * 创建一个新数据对象返回，在进行子集排行处理的时候不会通过本函数创建排行数据对象
     * @return
     */
    @Override
    protected long _getRankListIdForRBO(UsRankObjBO _bo)
    {
        if (null == _bo)
            return 0;
        return _bo.getInstanceId();
    }
    @Override
    protected RankObj _createInitRankObj(UsRankObjBO _bo)
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
    protected long _getRankListIdForRSBO(UsRankSubObjBO _bo)
    {
        if (null == _bo)
            return 0;
        return _bo.getInstanceId();
    }
    @Override
    protected long _getRankObjIdForRSBO(UsRankSubObjBO _bo)
    {
        if (null == _bo)
            return 0;
        return _bo.getObjId();
    }
    @Override
    protected RankSubObj _createInitRankSubObj(UsRankSubObjBO _bo)
    {
        if (null == _bo)
            return null;

        return new RankSubObj(_bo.getId(), _bo.getSubObjId(), _bo.getScoreSourceId(), _bo.getScore(), _bo.getUpdatedMs());
    }
   
    //==================== 基类业务完毕

    
    /***********************
     * 创建US排行榜数据
     * @param _rankId
     * @param _crossInstanceId
     * @return
     */
    public long createUsRank(long _rankId, long _crossInstanceId)
    {
        _lock();

        try
        {
            RefRank ref = RefRank.getMgr().get(_rankId);
            if (null == ref)
            {
                USLog.error(getUSServer(), "rank:{} instanceId:{} init fail, rank ref is null.", _rankId);
                return 0;
            }

            BM bmObj = getUSServer().getBM();

            UsRankBO bo = new UsRankBO();
            bo.setRankId(bmObj, _rankId);
            bo.setCrossInstanceId(bmObj, _crossInstanceId);
            bo.setHasReg(bmObj, false);
            bo.setIsDiscard(bmObj, false);
            bo.insert(bmObj);
            
            //创建排行榜数据
            USRankList rankList = createRankList(bo);
            if (null == rankList)
            {
                //注销排行榜
                bo.del(getUSServer().getBM());
                
                USLog.error(getUSServer(), "rank:{} init fail, create rank fail.", _rankId);
                return 0;
            }

            return rankList.getInstanceId();
        } 
        finally
        {
            _unlock();
        }
    }

    /**
     * 分数变更处理
     * @param _rankInstanceId 排行榜实例ID
     * @param _cid            玩家CID
     * @param _chgValue       变更值
     */
    public void onScoreChg(long _rankInstanceId, long _cid, long _scoreSourceId, long _chgValue)
    {
        USRankList rankList = lookup(_rankInstanceId);
        if (null == rankList)
        {
            USLog.error(getUSServer(), "instanceId:{} cid:{} scoreSourceId:{} chgValue:{} onScoreChg fail, rankList is null.", _rankInstanceId, _cid, _scoreSourceId, _chgValue);
            return;
        }

        rankList.onScoreChg(_cid, _scoreSourceId, _chgValue);
    }
    public void rpcOnGuildScoreChg(long _rankInstanceId, long _cid, long _guildId, long _scoreSourceId, long _chgValue)
    {
        USRankList rankList = lookup(_rankInstanceId);
        if (null == rankList)
        {
            USLog.error(getUSServer(), "instanceId:{} cid:{} guild:{} scoreSourceId:{} chgValue:{} rpcOnGuildScoreChg fail, rankList is null.", _rankInstanceId, _cid, _guildId, _scoreSourceId, _chgValue);
            return;
        }

        rankList.onRPCGuildScoreChg(_cid, _guildId, _scoreSourceId, _chgValue);
    }
}
