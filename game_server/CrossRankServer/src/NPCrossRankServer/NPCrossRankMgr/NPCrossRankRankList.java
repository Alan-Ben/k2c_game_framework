package NPCrossRankServer.NPCrossRankMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALProcess._IALProcessAction;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALServerLog.ALServerLog;
import CRSDB.Bo.NpCrossRankBO;
import CRSDB.Bo.NpCrossRankJoinerBO;
import CRSDB.Bo.NpCrossRankObjBO;
import CRSDB.Bo.NpCrossRankSubObjBO;
import NP2US_RB.p009_CrossRankOp.ToUS_RB_009_001_PushCrossRankScoreChg;
import NPCommon.CommonRank.RankObj;
import NPCommon.CommonRank.RankObjComparer._ARankObjComparer;
import NPCommon.CommonRank.RankSubObj;
import NPCommon.CommonRank._ARankList;
import NPCommon.CommonRank._IRankDBOper;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPCrossRankServer.NPCrossRankMgr.SynTask.SynCloseCrossRankListTask;
import NPCrossRankServer.NPCrossRankServer;
import NPGameRes.Refs.Rank.RefRank;
import NPServerProtocolWriter.NP2US.Request.ToUS_R_Writer_009_CrossRankOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.Hashtable;

/*********************
 * US服务器上的排行榜对象
 * @author mj
 *
 */
public class NPCrossRankRankList extends _ARankList
{
    //排行榜静态数据对象
    private RefRank _m_rankRef;
    
    //存储数据库对象，方便一些数据更改。如释放状态
    private NpCrossRankBO _m_rankBO;
    
    //存储所有加入跨服排行榜的参与对象信息
    private Hashtable<Long, NpCrossRankJoinerBO> _m_htJoinerTable;
    //用于存储删除排行的处理对象，在清空参与对象的时候会调用这个处理函数。
    //初始情况下本处理函数是空的，只有在尝试释放的时候才会设置本值
    private _IALProcessAction _m_clearJoinerDealer;

    public NPCrossRankRankList(_IRankDBOper _rankDbOper, NpCrossRankBO _bo, RefRank _rankRef,
            _ARankObjComparer _comparer)
    {
        super(_rankDbOper, _bo.getId(), _rankRef.rank_id
                , _rankRef.rank_num_max < 100 ? 100 : _rankRef.rank_num_max
                        , _comparer);
        
        _m_rankRef = _rankRef;

        _m_rankBO = _bo;
        _m_htJoinerTable = new Hashtable<Long, NpCrossRankJoinerBO>();
        _m_clearJoinerDealer = null;
    }
    
    public RefRank getRankRefCommon() {return _m_rankRef;}
    public long getCrossInstanceId() {return _m_rankBO.getCrossInstanceId();}
    
    //判断是否有效，无效的情况下外围会调用关闭函数
    public boolean isEnable() {return !_m_rankBO.getIsDiscard();}

    /******************
     * 创建一个新数据对象返回，在进行子集排行处理的时候不会通过本函数创建排行数据对象
     * @param _objId
     * @param _score
     * @param _rank
     * @return
     */
    @Override
    protected RankObj _createRankObj(long _objId, long _scoreSourceId, long _score, int _rank)
    {
        BM bmObj = NPCrossRankServer.getInstance().getBM();
        //创建数据BO
        NpCrossRankObjBO bo = new NpCrossRankObjBO();
        bo.setInstanceId(bmObj, getInstanceId());
        bo.setRankId(bmObj, getRankId());
        bo.setObjId(bmObj, _objId);
        bo.setScoreSourceId(bmObj, _scoreSourceId);
        bo.setScore(bmObj, _score);
        bo.setUpdatedMs(bmObj, CommonFunc.getNowTimeMS());
        bo.insert(bmObj);

        return new RankObj(bo.getId(), bo.getObjId(), bo.getScoreSourceId(), bo.getScore(), bo.getUpdatedMs(), _rank);
    }

    /******************
     * 创建一个新的子集数据对象返回
     * @param _objId
     * @param _score
     * @return
     */
    @Override
    protected RankSubObj _createRankSubObj(long _objId, long _subObjId, long _scoreSourceId, long _score)
    {
        BM bmObj = NPCrossRankServer.getInstance().getBM();
        //创建数据BO
        NpCrossRankSubObjBO bo = new NpCrossRankSubObjBO();
        bo.setInstanceId(bmObj, getInstanceId());
        bo.setRankId(bmObj, getRankId());
        bo.setObjId(bmObj, _objId);
        bo.setSubObjId(bmObj, _subObjId);
        bo.setScoreSourceId(bmObj, _scoreSourceId);
        bo.setScore(bmObj, _score);
        bo.setUpdatedMs(bmObj, CommonFunc.getNowTimeMS());
        bo.insert(bmObj);

        return new RankSubObj(bo.getId(), bo.getSubObjId(), bo.getScoreSourceId(), bo.getScore(), bo.getUpdatedMs());
    }
    
    /***************
     * 在本排行开启的时候处理的函数
     * 
     * 如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）
     */
    @Override
    protected void _onRankCreated_inLock()
    {
        //跨服服务器上的排行榜不需要在创建的时候做处理
    }
    
    /*****************
     * 在本排行关闭的时候触发的事件函数
     * 
     * 如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）
     */
    @Override
    protected void _onRankClose_inLock()
    {
        //跨服服务器上的排行榜不需要在关闭的时候做处理
    }
    
    /**
     * 排行对象被动变更排名时触发的锁内事件
     * 注意：这里的逻辑不是自己主动改变的情况，是被动改变的时候触发的
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     *
     * 功能：推送排名变动通知给所属UserServer
     *
     * @param _rankObj 被动变更排名的对象
     * @param _preRank 原排名
     */
    @Override
    protected void _onRankObjRankBeChanged_inLock(RankObj _rankObj, int _preRank)
    {
        // 只处理玩家排行榜，且前10名内的被动变动
//        if (_preRank <= 10 && _m_rankRef.rank_type == ERankType.PLAYER)
//        {
//            // 通过syn任务异步推送，避免在锁内执行网络操作
//            ALSynTaskManager.getInstance().regTask(() -> {
//                _pushRankScoreChgToUS(_rankObj.getObjId(), _preRank, _rankObj.getRank(), _rankObj.getScore());
//            });
//        }
    }

    /**
     * 排行对象主动变更排名时触发的锁内事件
     * 注意：这里是对象因自身分数变化导致排名变更
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     *
     * 功能：推送排名变动通知给所属UserServer
     *
     * @param _rankObj 主动变更排名的对象
     * @param _preRank 原排名
     */
    @Override
    protected void _onRankObjRankChanged_inLock(RankObj _rankObj, int _preRank)
    {
        // 只处理玩家排行榜
//        if (_m_rankRef.rank_type == ERankType.PLAYER)
//        {
//            // 通过syn任务异步推送，避免在锁内执行网络操作
//            ALSynTaskManager.getInstance().regTask(() -> {
//                _pushRankScoreChgToUS(_rankObj.getObjId(), _preRank, _rankObj.getRank(), _rankObj.getScore());
//            });
//        }
    }

    /**
     * 推送排行榜分数变更通知给UserServer
     *
     * 执行流程：
     * 1. 从玩家cid解析出所属UserServer的ID
     * 2. 构造NP2US_R_009_001推送协议
     * 3. 发送推送给对应的UserServer
     * 4. UserServer会进一步转发给在线玩家
     *
     * @param _cid 玩家CID
     * @param _oriRank 原排名
     * @param _curRank 当前排名
     * @param _score 当前分数
     */
    private void _pushRankScoreChgToUS(long _cid, int _oriRank, int _curRank, long _score)
    {
        // 从cid解析出所属UserServer的ID
        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        // 发送推送给UserServer
        NPCrossRankServer.getInstance().sendRequestToBSServer(
            EServerType.USER.ordinal(),
            usId,
            ToUS_R_Writer_009_CrossRankOp.make_009_001_PushCrossRankScoreChg(
                _cid,                       // 玩家CID
                getCrossInstanceId(),       // 跨服活动实例ID
                getRankId(),                // 排行榜ID
                _oriRank,                   // 原排名
                _curRank,                   // 当前排名
                _score                      // 当前分数
            ),
            new _IWCGCallbackDealer()
            {
                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new ToUS_RB_009_001_PushCrossRankScoreChg();
                }

                @Override
                public void dealSuc(_IALProtocolStructure _proto)
                {

                }

                @Override
                public void dealFail(int _errCode)
                {

                }
            }
        );
    }

    /**
     * 排行榜元素变更通知
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     * 
     * @param _rankObj    变更后的排行榜元素 如果需要新分数或新排名，可以从对象中直接取
     * @param _oldScore   旧分数
     * @param _oldRank    旧排行
     * @param _m_cContext
     */
    @Override
    protected void _onRankObjChg_inLock(RankObj _rankObj, long _oldScoreSourceId, long _oldScore, int _oldRank, _IContext _m_cContext)
    {
        //这里可以根据实际的排行通知需求对相关服务器进行通知
    }
    @Override
    protected void _onRankSubObjChg_inLock(RankObj _rankObj, RankSubObj _rankSubObj, long _oldScoreSourceId, long _oldScore, int _oldRank, _IContext _m_cContext)
    {
        //这里可以根据实际的排行通知需求对相关服务器进行通知
    }

    
    /******************
     * 设置这个排行榜需要被销毁
     * 
     * 后续子类应该根据实际排行榜情况直接删除或者向跨服排行榜服务器发送请求，并在确认跨服排行接收到删除请求后本地才可以删除
     * 如果是总排行，可能需要等待所有子对象清理完毕后才能进行删除处理
     * 
     * @param _realDiscardAction 实际处理删除数据的处理对象
     */
    @Override
    protected void _setRankListNeedDiscard(_IALProcessAction _realDiscardAction)
    {
        //设置本排行榜的状态为释放状态，并根据是否跨服子榜发送请求处理。
        _m_rankBO.saveIsDiscard(NPCrossRankServer.getInstance().getBM(), true);
        
        _lock();
        
        try
        {
            //判断是否有参与者，如果没有参与者则直接删除
            if(_m_htJoinerTable.isEmpty())
            {
                if(null != _realDiscardAction)
                    _realDiscardAction.dealAction();
                
                return ;
            }
            
            //有数据的时候设置清空参与者的处理对象，在清空参与者的时候会调用对应处理
            //设置清空回调
            _m_clearJoinerDealer = _realDiscardAction;
        }
        finally
        {
            _unlock();
        }
    }

    /*********************
     * 初始化参与者的数据库对象
     * @param _joinerBo 参与者数据对象
     */
    protected void _initFromJoinerBO(NpCrossRankJoinerBO _joinerBo)
    {
        //放入数据集
        _m_htJoinerTable.put(_joinerBo.getJoinerId(), _joinerBo);
    }

    /********
     * 添加参与者
     * @param _joiner usId
     */
    public boolean addJoiner(long _joiner)
    {
        //如果本对象已经要销毁则不允许添加参与者
        if(_m_rankBO.getIsDiscard())
        {
            ALServerLog.Error("Add joiner: " + _joiner + " when cross rank: " + _m_rankBO.getId() + " is discard!");
            return false;
        }
        
        _lock();

        try
        {
            //判断是否已经有对象存在，有则直接返回成功
            if(_m_htJoinerTable.containsKey(_joiner))
                return true;

            BM bmObj = NPCrossRankServer.getInstance().getBM();
            //创建一个参与者数据并插入数据库后放入数据集
            NpCrossRankJoinerBO newJoinerBO = new NpCrossRankJoinerBO();
            newJoinerBO.setCrossRankInstanceId(bmObj, getInstanceId());
            newJoinerBO.setJoinerId(bmObj, _joiner);
            
            //插入数据库
            newJoinerBO.insert(bmObj);
            
            //放入数据集
            _m_htJoinerTable.put(_joiner, newJoinerBO);

            return true;
        } 
        finally
        {
            _unlock();
        }
    }

    /********************
     * 移除参与者
     */
    public void removeJoiner(long _joiner)
    {
        _lock();

        try
        {
            //移除数据对象，并删除bo
            NpCrossRankJoinerBO removeBo = _m_htJoinerTable.remove(_joiner);
            
            //判断是否已经有对象存在，有则直接返回成功
            if(null == removeBo)
                return ;

            removeBo.del(NPCrossRankServer.getInstance().getBM());
            
            //此处判断参与者是否为空，是则尝试进行_m_clearJoinerDealer处理
            if(_m_htJoinerTable.isEmpty())
            {
                if(null != _m_clearJoinerDealer)
                    _m_clearJoinerDealer.dealAction();
                _m_clearJoinerDealer = null;
                
                //此处开启任务注销本排行榜（从跨服实例组中注销，跨服实例组会从排行榜管理器中关闭排行）
                ALSynTaskManager.getInstance().regTask(new SynCloseCrossRankListTask(this));
            }
        }
        finally
        {
            _unlock();
        }
    }
}
