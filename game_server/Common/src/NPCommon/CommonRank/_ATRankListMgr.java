package NPCommon.CommonRank;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALProcess._IALProcessAction;
import ALBasicServer.ALProcess._ITALProcessAction;
import ALServerLog.ALServerLog;
import NPCommon.DB.BaseBO;
import NPCommon.Log.CommLog;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

/**
 * @description: 排行对象管理器
 * 管理 _ARankObjList 的名单，提供基础增删改查功能
 * @author: mark
 * @date: 2022-06-17 11:21:14
 */
public abstract class _ATRankListMgr<RL extends _ARankList, RB extends BaseBO , RBO extends BaseBO, RSBO extends BaseBO> implements _IRankDBOper
{
    //是否初始化完成，只有初始化完成才可调用数据添加等操作
    protected boolean _m_bIsInitDone;

    /**
     * 排行对象列表
     * key：排行榜实例ID
     * val：排行榜对象
     */
    protected final HashMap<Long, RL> _m_mapRankObjListMap;
    protected ArrayList<RL> _m_lRankList;

    /**
     * 锁对象，保护 _m_mapRankObjListMap
     */
    protected final MutexObject _m_mutex;

    public _ATRankListMgr()
    {
        _m_mapRankObjListMap = new HashMap<Long, RL>();
        _m_lRankList = new ArrayList<RL>();
        _m_mutex = new MutexObject();

        _m_bIsInitDone = false;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    //region get&&set

    //endregion

    /**
     * 查询排行数据
     * @param _instanceId 排行实例ID
     * @return _ARankObjList
     */
    public RL lookup(long _instanceId)
    {
        if (!_m_bIsInitDone)
        {
            ALServerLog.Error("Lookup rank list when mgr is not inited!");
            return null;
        }

        _lock();
        try
        {
            return _m_mapRankObjListMap.get(_instanceId);
        } finally
        {
            _unlock();
        }
    }

    /*******************
     * 创建一个排行榜对象
     * 对是否已经存在同样排行榜进行判断
     * 如已经存在需要报错处理
     */
    public RL createRankList(RB _rankBo)
    {
        if (!_m_bIsInitDone)
        {
            ALServerLog.Error("Lookup rank list when mgr is not inited!");
            return null;
        }

        _lock();
        try
        {
            if (_m_mapRankObjListMap.containsKey(_rankBo.getId()))
            {
                ALServerLog.Fatal("Try to Create RankList: " + _rankBo.getId() + " Multi Times!");
                return null;
            }

            //输出日志
            ALServerLog.Sys("Create RankList: " + _rankBo.getId());

            //创建对应排行榜对象
            RL rankList = _createRankList(_rankBo);
            
            //调用排行开启事件
            rankList._onRankCreated_inLock();

            //放入数据
            _m_mapRankObjListMap.put(rankList.getInstanceId(), rankList);
            _m_lRankList.add(rankList);

            return rankList;
        } finally
        {
            _unlock();
        }
    }

    /*********************
     * 关闭指定Id的排行榜对象
     * @param _instanceId 排行榜实例ID
     */
    public void closeRankList(long _instanceId)
    {
        _lock();
        try
        {
            RL rankList = _m_mapRankObjListMap.remove(_instanceId);
            _m_lRankList.remove(rankList);

            if (null == rankList)
                return;

            //输出日志
            ALServerLog.Sys("Close RankList: " + _instanceId);

            //调用排行榜关闭的处理函数
            rankList._onRankClose_inLock();

            //此处设置排行榜为需要删除的状态
            rankList._setRankListNeedDiscard(
                    new _IALProcessAction() {
                            @Override
                            public void dealAction() { rankList._discardAllInfo(); }
                        });
            //调用排行榜内部清理操作 - 上一行处理后，下一个实际删除的需要屏蔽
            //rankList._discard();
        } finally
        {
            _unlock();
        }
    }

    /*********************
     * 重置指定Id的排行榜对象
     * @param _instanceId 排行榜实例ID
     */
    public void resetRankList(long _instanceId)
    {
        if (!_m_bIsInitDone)
        {
            ALServerLog.Error("Close rank list when mgr is not inited!");
            return;
        }

        _lock();
        try
        {
            RL rankList = _m_mapRankObjListMap.get(_instanceId);

            if (null == rankList)
                return;

            //输出日志
            ALServerLog.Sys("reset RankList: " + _instanceId);

            //调用排行榜内部清理操作
            rankList._clearData();
        } finally
        {
            _unlock();
        }
    }

    /*******************
     * 创建一个排行榜对象
     * 对是否已经存在同样排行榜进行判断
     * 如已经存在需要报错处理
     */
    protected RL _initRankList(RB _rankBo)
    {
        if (_m_mapRankObjListMap.containsKey(_rankBo.getId()))
        {
            ALServerLog.Fatal("Try to Create RankList: " + _rankBo.getId() + " Multi Times!");
            return null;
        }

        //创建对应排行榜对象
        RL rankList = _createRankList(_rankBo);
        if (rankList == null)
            return null;

        //调用排行开启事件
        rankList._onRankCreated_inLock();

        //放入数据
        _m_mapRankObjListMap.put(rankList.getInstanceId(), rankList);
        _m_lRankList.add(rankList);

        return rankList;
    }


    /*********************
     * 初始化所有rankObj的数据库对象
     * @param _list
     */
    protected void _initFromRBOList(List<RBO> _list)
    {
        if(null == _list)
            return ;
        
        //遍历数据，逐个进行初始化处理
        for (int i = 0; i < _list.size(); i++)
        {
            RBO bo = _list.get(i);
            if (null == bo)
                continue;

            //查询对应的排行榜对象
            long rankListInstanceID = _getRankListIdForRBO(bo);
            RL rankList = _m_mapRankObjListMap.get(rankListInstanceID);
            if (null == rankList)
            {
                CommLog.error("init rank obj:{} fail, rank list instanceId:{}", bo.getId(), rankListInstanceID);
                continue;
            }
            
            //先构建对象，这样可以通过对象获取排行榜Id
            RankObj rObj = _createInitRankObj(bo);
            if(null == rObj)
                continue;

            //添加数据
            rankList._initAddRankObj(rObj);
        }
    }

    /*********************
     * 初始化所有rankSubObj的数据库对象
     * @param _list
     */
    protected void _initFromRSBOList(List<RSBO> _list)
    {
        if(null == _list)
            return ;
        
        //遍历数据，逐个进行初始化处理
        for (int i = 0; i < _list.size(); i++)
        {
            RSBO bo = _list.get(i);
            if (null == bo)
                continue;

            //查询对应的排行榜对象
            long rankListInstanceID = _getRankListIdForRSBO(bo);
            RL rankList = _m_mapRankObjListMap.get(rankListInstanceID);
            if (null == rankList)
            {
                CommLog.error("init rank sub obj:{} fail, rank list instanceId:{}", bo.getId(), rankListInstanceID);
                continue;
            }
            
            //先构建对象，这样可以通过对象获取排行榜Id
            RankSubObj rObj = _createInitRankSubObj(bo);
            if(null == rObj)
                continue;

            //添加数据
            rankList._initAddRankSubObj(_getRankObjIdForRSBO(bo), rObj);
        }
    }


    /*********************
     * 初始化时添加对应的排行榜数据对象，此函数只可在初始化调用
     * @param _instanceId 排行榜实例ID
     * @param _rankObj
     */
    protected void _initAddRankObj(long _instanceId, RankObj _rankObj)
    {
        //已经初始化要报错
        if (_m_bIsInitDone)
        {
            ALServerLog.Error("Try to Init Add Rank[" + _instanceId + "] Obj after init done!");
            return;
        }

        _lock();
        try
        {
            //获取排行，如不存在则创建
            RL rankList = _m_mapRankObjListMap.get(_instanceId);
            if (null == rankList)
            {
                ALServerLog.Error("Init RankObj Can not find RankList: " + _instanceId);
                return;
            }

            //添加数据
            rankList._initAddRankObj(_rankObj);
        } finally
        {
            _unlock();
        }
    }

    protected void _initAddRankSubObj(long _instanceId, long _objId, RankSubObj _rankSubObj)
    {
        //已经初始化要报错
        if (_m_bIsInitDone)
        {
            ALServerLog.Error("Try to Init Add Rank[" + _instanceId + "] Obj after init done!");
            return;
        }

        _lock();
        try
        {
            //获取排行，如不存在则创建
            RL rankList = _m_mapRankObjListMap.get(_instanceId);
            if (null == rankList)
            {
                ALServerLog.Error("Init RankObj Can not find RankList: " + _instanceId);
                return;
            }

            //添加数据
            rankList._initAddRankSubObj(_objId, _rankSubObj);
        } finally
        {
            _unlock();
        }
    }

    /******************
     * 遍历所有排行榜进行特殊处理的遍历函数
     */
    public void iteratorAllRankList_inLock(_ITALProcessAction<RL> _action)
    {
        if(null == _action)
            return ;
        
        _lock();
        try
        {
            //对所有排行榜进行排序处理
            for (RL rankList : _m_lRankList)
            {
                if (null == rankList)
                    continue;

                //进行处理
                _action.dealAction(rankList);
            }
        } finally
        {
            _unlock();
        }
    }

    /******************
     * 设置排行榜所有数据初始化完成
     */
    public void setRankListMgrInitDone()
    {
        //已经初始化要报错
        if (_m_bIsInitDone)
        {
            ALServerLog.Error("Try to set rankListMgr Init Done Multi Times!");
            return;
        }

        _m_bIsInitDone = true;

        _lock();
        try
        {
            //对所有排行榜进行排序处理
            for (RL rankList : _m_lRankList)
            {
                if (null == rankList)
                    continue;

                //进行排序处理
                rankList._initReRankAll();
            }
        } finally
        {
            _unlock();
        }
    }

    /****************
     * 根据排行Id创建对应的排行榜对象
     * @param _instanceId 排行榜实例ID
     * @param _rankId     排行榜配置ID
     * @return
     */
    protected abstract RL _createRankList(RB _rankBo);


    /******************
     * 创建一个新数据对象返回，在进行子集排行处理的时候不会通过本函数创建排行数据对象
     * @param _objId
     * @param _score
     * @param _rank
     * @return
     */
    protected abstract long _getRankListIdForRBO(RBO _bo);
    protected abstract RankObj _createInitRankObj(RBO _bo);

    /******************
     * 创建一个新的子集数据对象返回
     * @param _objId
     * @param _score
     * @param _rank
     * @return
     */
    protected abstract long _getRankListIdForRSBO(RSBO _bo);
    protected abstract long _getRankObjIdForRSBO(RSBO _bo);
    protected abstract RankSubObj _createInitRankSubObj(RSBO _bo);
}
