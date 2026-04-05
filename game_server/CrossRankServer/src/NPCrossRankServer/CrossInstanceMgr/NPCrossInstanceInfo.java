package NPCrossRankServer.CrossInstanceMgr;

import ALBasicServer.ALBasicMutex.MutexManager;
import ALBasicServer.ALProcess._IALProcessAction;
import ALServerLog.ALServerLog;
import CRSDB.Bo.NpCrossInstanceBO;
import CRSDB.Bo.NpCrossInstanceJoinerBO;
import CRSDB.Bo.NpCrossRankBO;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankListMgr;
import NPCrossRankServer.NPCrossRankMgr.NPCrossRankRankList;
import NPCrossRankServer.NPCrossRankServer;

import java.util.Hashtable;

/*********************
 * 跨服排行榜的分组信息对象
 * @author mj
 *
 */
public class NPCrossInstanceInfo
{
    //跨服排行榜分组的主数据对象
    private NpCrossInstanceBO _m_crossInstanceBO;
    
    //参与这个排行榜分组的所有参与对象
    private Hashtable<Long, NpCrossInstanceJoinerBO> _m_htJoinerTable;
    
    //在分组内开启的排行Id和对应实例Id的映射关系表
    private Hashtable<Long, Long> _m_htCrossInstanceRankTable;
    
    //用于存储删除排行的处理对象，在清空参与对象的时候会调用这个处理函数。
    //初始情况下本处理函数是空的，只有在尝试释放的时候才会设置本值
    private _IALProcessAction _m_clearJoinerDealer;
    
    //锁对象，保护UsList
    private MutexManager _m_mutex;

    public NPCrossInstanceInfo(NpCrossInstanceBO _bo)
    {
        _m_crossInstanceBO = _bo;

        _m_htJoinerTable = new Hashtable<Long, NpCrossInstanceJoinerBO>();
        
        _m_htCrossInstanceRankTable = new Hashtable<Long, Long>();
        _m_clearJoinerDealer = null;

        _m_mutex = new MutexManager();
    }

    public long getInstanceId() { return _m_crossInstanceBO.getCrossInstanceId(); }
    public boolean isEnable() {return !_m_crossInstanceBO.getNeedDiscard();}

    private void _lock() { _m_mutex.lock(); }
    private void _unlock() { _m_mutex.unlock(); }

    /********
     * 添加参与者
     * @param _joiner
     */
    public boolean addJoiner(long _joiner)
    {
        //如果本对象已经要销毁则不允许添加参与者
        if(_m_crossInstanceBO.getNeedDiscard())
        {
            ALServerLog.Error("Add joiner: " + _joiner + " when cross instance: " + _m_crossInstanceBO.getCrossInstanceId() + " is discard!");
            return false;
        }
        
        _lock();

        try
        {
            //判断是否已经有对象存在，有则直接返回成功
            if(_m_htJoinerTable.containsKey(_joiner))
                return true;

            BM bmOBj = NPCrossRankServer.getInstance().getBM();

            //创建一个参与者数据并插入数据库后放入数据集
            NpCrossInstanceJoinerBO newJoinerBO = new NpCrossInstanceJoinerBO();
            newJoinerBO.setCrossInstanceId(bmOBj, getInstanceId());
            newJoinerBO.setJoinerId(bmOBj, _joiner);
            
            //插入数据库
            newJoinerBO.insert(bmOBj);
            
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
            NpCrossInstanceJoinerBO removeBo = _m_htJoinerTable.remove(_joiner);
            
            //判断是否已经有对象存在，有则直接返回成功
            if(null == removeBo)
                return ;

            removeBo.del(NPCrossRankServer.getInstance().getBM());
            
            //此处尝试进行_m_clearJoinerDealer处理
            _checkAndDealDiscardOP();
        }
        finally
        {
            _unlock();
        }
    }
    
    /***************
     * 初始化的时候，将排行榜放入本跨服集合中
     * @return
     */
    public void initAddRankList(NPCrossRankRankList _rankList)
    {
        if(null == _rankList)
            return ;
        
        _lock();

        try
        {
            //判断是否已经存在，是则报错
            if(_m_htCrossInstanceRankTable.containsKey(_rankList.getRankId()))
            {
                ALServerLog.Error("Init Add CrossRank multi rank id: " + _rankList.getRankId() + " for cross instance: " + getInstanceId() + " !!");
                return ;
            }
            
            //将排行放入数据集中
            _m_htCrossInstanceRankTable.put(_rankList.getRankId(), _rankList.getInstanceId());
        }
        finally
        {
            _unlock();
        }
    }
    
    /***************
     * 确认一个对应排行Id的排行返回对应的实例Id
     * @param _rankId
     * @return
     */
    public long ensureRankListId(long _rankId)
    {
        _lock();

        try
        {
            //查询对应的排行榜是否有数据对象
            Long rankInstanceId = _m_htCrossInstanceRankTable.get(_rankId);
            if(null == rankInstanceId)
            {
                BM bmObj = NPCrossRankServer.getInstance().getBM();
                //创建新排行，并将其设置为本集合内数据
                NpCrossRankBO bo = new NpCrossRankBO();
                bo.setRankId(bmObj, _rankId);
                bo.setCrossInstanceId(bmObj, getInstanceId());
                bo.setIsDiscard(bmObj, false);
                bo.insert(bmObj);
                
                //创建实例对象
                NPCrossRankRankList rankList = NPCrossRankListMgr.getInstance().createRankList(bo);
                if(null == rankList)
                {
                    ALServerLog.Error("Create CrossRank for rank id: " + _rankId + " error!!");
                    bo.del(bmObj);
                    return 0;
                }
                
                //将排行放入数据集中
                _m_htCrossInstanceRankTable.put(_rankId, rankList.getInstanceId());
                
                return rankList.getInstanceId();
            }
            
            return rankInstanceId;
        }
        finally
        {
            _unlock();
        }
    }
    
    /***************
     * 查询是否存在对应的排行榜，只有当排行榜存在才会正常返回值
     * @param _rankId
     * @return
     */
    public long lookupRankListId(long _rankId)
    {
        _lock();

        try
        {
            //查询对应的排行榜是否有数据对象
            Long rankInstanceId = _m_htCrossInstanceRankTable.get(_rankId);
            if(null == rankInstanceId)
            {
                return 0;
            }
            
            return rankInstanceId;
        }
        finally
        {
            _unlock();
        }
    }
    
    /********************
     * 从分组中关闭一个排行榜，带入排行Id，以及对应实例Id，确保不会关闭错误的实例
     * @param _rankId
     * @param _checkRankListInstanceId
     */
    public void removeRankList(long _rankId, long _checkRankListInstanceId)
    {
        _lock();

        try
        {
            //查询对应的排行榜是否有数据对象
            Long rankInstanceId = _m_htCrossInstanceRankTable.get(_rankId);
            //未查询到，则暂时不处理，避免错误。但是需要报错
            if(null == rankInstanceId || rankInstanceId != _checkRankListInstanceId)
            {
                CommLog.fatal("Can not remove rankList from cross instance:[{}] rank list instance id:[{}] for rank id:[{}],  current rank instance id:[{}]!!"
                        , getInstanceId(), _checkRankListInstanceId, _rankId, rankInstanceId);
                return ;
            }
            
            //从数据集中删除
            _m_htCrossInstanceRankTable.remove(_rankId);
            //调用移除处理
            NPCrossRankListMgr.getInstance().closeRankList(_checkRankListInstanceId);
            
            //此处尝试进行_m_clearJoinerDealer处理
            _checkAndDealDiscardOP();
        }
        finally
        {
            _unlock();
        }
    }

    
    /******************
     * 设置这个排行榜需要被销毁
     * 
     * 后续子类应该根据实际排行榜情况直接删除或者向跨服排行榜服务器发送请求，并在确认跨服排行接收到删除请求后本地才可以删除
     * 如果是总排行，可能需要等待所有子对象清理完毕后才能进行删除处理
     * 
     * @param _realDiscardAction 实际处理删除数据的处理对象
     */
    public void tryDiscardCrossInstance(_IALProcessAction _realDiscardAction)
    {
        //设置本排行榜的状态为释放状态，并根据是否跨服子榜发送请求处理。
        _m_crossInstanceBO.saveNeedDiscard(NPCrossRankServer.getInstance().getBM(), true);
        
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

    /********************
     * 尝试调用移除处理，只有在参与者与子排行都为空的时候才会进行删除处理
     */
    protected void _checkAndDealDiscardOP()
    {
        _lock();

        try
        {
            //此处判断参与者是否为空，是则尝试进行_m_clearJoinerDealer处理
            if(_m_htJoinerTable.isEmpty() && _m_htCrossInstanceRankTable.isEmpty())
            {
                if(null != _m_clearJoinerDealer)
                    _m_clearJoinerDealer.dealAction();
                _m_clearJoinerDealer = null;
            }
        }
        finally
        {
            _unlock();
        }
    }

    /*********
     * 销毁数据
     */
    public void _discardAllInfo()
    {
        _m_crossInstanceBO.del(NPCrossRankServer.getInstance().getBM());
    }
}
