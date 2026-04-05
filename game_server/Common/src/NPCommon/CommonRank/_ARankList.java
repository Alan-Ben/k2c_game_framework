package NPCommon.CommonRank;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALProcess._IALProcessAction;
import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALServerLog.ALServerLog;
import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_ItemDump;
import Common.ServerObj.ServerObj_RankObjInfo;
import NPCommon.CommonRank.DBTask.AsynClearRankDataTask;
import NPCommon.CommonRank.DBTask.AsynDelRankInfoTask;
import NPCommon.CommonRank.DBTask.AsynDelRankObjScoreTask;
import NPCommon.CommonRank.RankObjComparer._ARankObjComparer;
import NPCommon.Context._IContext;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate._IHandlerHolder;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.function.Consumer;
import java.util.function.Predicate;

/**
 * @description: 排行的基类，
 * @author: mark
 * @date: 2022-06-17 11:20:21
 */
public abstract class _ARankList implements _IHandlerHolder
{
    //归属的排行榜管理对象，相关的接口需要通过管理对象进行处理
    private final _IRankDBOper _m_rlmRankDBOper;

    //排行榜是否还有效
    private boolean _m_bEnable;

    //排行榜是否暂时关闭
    private boolean _m_bTempClose;

    /**
     * 排行榜实例id
     */
    private final long _m_lInstanceId;
    /**
     * 排行榜配置ID
     */
    private final long _m_lRankId;

    /**
     * 排行榜具体元素数据，帮助快速查询的 map
     */
    protected final HashMap<Long, RankObj> _m_mapRankItemMap;
    /**
     * 排行榜具体元素，提供遍历和排名功能的 list,如果数据对象被ban，在这个列表中是找不到的
     */
    protected final List<RankObj> _m_lRankItemList;

    /**
     * 排行最大数据
     */
    private final int _m_iRankMaxSize;

    //比较排序的比较对象
    private _ARankObjComparer _m_rcRankComparer;

    /**
     * 锁对象，保护_m_mapRankItemMap 和 _m_lRankItemList
     */
    private final MutexAtom _m_mutex;

    public _ARankList(_IRankDBOper _rankDbOper, long _instanceId, long _rankId, int _iRankMaxSize, _ARankObjComparer _comparer)
    {
        _m_rlmRankDBOper = _rankDbOper;

        _m_lInstanceId = _instanceId;
        _m_lRankId = _rankId;
        _m_iRankMaxSize = _iRankMaxSize;
        _m_rcRankComparer = _comparer;

        _m_mapRankItemMap = new HashMap<>();
        _m_lRankItemList = new ArrayList<>();

        //默认都为有效状态
        _m_bEnable = true;

        _m_mutex = new MutexAtom();
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

    public final long getInstanceId()
    {
        return _m_lInstanceId;
    }

    public final long getRankId()
    {
        return _m_lRankId;
    }

    public final int getRankMaxSize()
    {
        return _m_iRankMaxSize;
    }

    protected _IRankDBOper _getRankDBOper()
    {
        return _m_rlmRankDBOper;
    }

    //开启和关闭排行榜处理
    public void enableRank()
    {
        _m_bEnable = true;
        ALServerLog.Sys("Enable Rank: " + _m_lInstanceId);
    }

    public void disableRank()
    {
        _m_bEnable = false;
        ALServerLog.Sys("Diable Rank: " + _m_lInstanceId);
    }

    public int getRankSize()
    {
        return _m_lRankItemList.size();
    }

    /*************
     * 拷贝排行数据
     * @return
     */
    public ArrayList<RankObj> cloneRankItemList()
    {
        _lock();
        try
        {
            return new ArrayList<>(_m_lRankItemList);
        } finally
        {
            _unlock();
        }
    }

    //endregion

    /**
     * 查询方法
     * @param _itemKey 查询key值
     * @return _ARankObj
     */
    public RankObj lookup(long _itemKey)
    {
        _lock();
        try
        {
            return _m_mapRankItemMap.get(_itemKey);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过排名查询用户
     * @param _rank 排名
     * @return _ARankObj
     */
    public RankObj lookupByRank(int _rank)
    {
        _lock();
        try
        {
            return _m_lRankItemList.get(_rank - 1);
        } catch (IndexOutOfBoundsException _exception)
        {
            //给定排名不合法
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**********************
     * 遍历所有排行对象进行的操作，在锁内
     * @param _iterator
     */
    public void iteratorAllRankObj_inLockAtoM(_IRankObjIterator _iterator)
    {
        if (null == _iterator)
            return;

        _lock();
        try
        {
            //进行遍历处理
            for (int i = 0; i < _m_lRankItemList.size(); i++)
            {
                _iterator.doIterator(_m_lRankItemList.get(i));
            }
        } finally
        {
            _unlock();
        }
    }


    /**
     * 通过给定过滤方法查询一批数据
     * @param _testFunc     测试方法，返回true时，rankObj会被加入结果列表中
     * @param _supplierFunc 消费方法，不为空时每一个被过滤到的元素，都会调用该方法处理
     * @return ArrayList<_ARankObj>
     */
    public ArrayList<RankObj> lookupByCheckFunc(Predicate<RankObj> _testFunc, Consumer<RankObj> _supplierFunc)
    {
        _lock();
        try
        {
            ArrayList<RankObj> rankObjs = new ArrayList<>();
            for (RankObj rankObj : _m_lRankItemList)
            {
                if (rankObj == null)
                {
                    continue;
                }
                //测试过滤条件
                if (_testFunc != null && _testFunc.test(rankObj))
                {
                    _supplierFunc.accept(rankObj);
                    rankObjs.add(rankObj);
                }
            }
            return rankObjs;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新排行榜分数
     * @param _objId         主体id
     * @param _scoreSourceId 分数来源id
     * @param _score         分数
     * @param _setGreater    是否设置更大值
     * @param _context       上下文
     * @return 是否设置成功
     */
    public boolean setScore(long _objId, long _scoreSourceId, long _score, boolean _setGreater, _IContext _context)
    {
        if (_m_bTempClose)
            return false;

        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not set rank[" + _m_lInstanceId + "] obj when list is disable! objId: " + _objId + " | score: " + _score);

            return false;
        }

        _lock();
        try
        {
            RankObj rankObj = _ensure(_objId);
            //旧分数
            long oldScore = rankObj.getScore();
            //旧分数来源
            long oldScoreSourceId = rankObj.getScoreSourceId();
            //分数没有变化，不更新排行
            if (oldScore == _score)
            {
                return true;
            }
            //需要记录更大值，小于则返回
            if (_setGreater && _score < oldScore)
                return true;

            //设置分数
            rankObj._setScore(_scoreSourceId, _score, ALBasicCommonFun.getNowTimeMS());
            //更新排行榜
            _onRankObjScoreChg(rankObj, oldScoreSourceId, oldScore, _context);

            return true;
        } finally
        {
            _unlock();
        }
    }

    public boolean chgScore(long _objId, long _scoreSourceId, long _chgScore, _IContext _context)
    {
        if (_m_bTempClose)
            return false;

        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not change rank[" + _m_lInstanceId + "] obj when list is disable! objId: " + _objId + " | chgScore: " + _chgScore);
            return false;
        }

        _lock();
        try
        {
            RankObj rankObj = _ensure(_objId);
            //旧分数
            long oldScore = rankObj.getScore();
            //旧分数来源
            long oldScoreSourceId = rankObj.getScoreSourceId();
            //分数没有变化，不更新排行
            if (0 == _chgScore)
            {
                return true;
            }

            //修改分数
            rankObj._chgScore(_scoreSourceId, _chgScore, ALBasicCommonFun.getNowTimeMS());
            //更新排行榜
            _onRankObjScoreChg(rankObj, oldScoreSourceId, oldScore, _context);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新排行榜分数(比较更新时间)
     * @param _objId         主体id
     * @param _scoreSourceId 分数来源id
     * @param _score         分数
     * @param _context       上下文
     * @return 是否设置成功
     */
    public boolean setScoreCompareUpdateTime(long _objId, long _scoreSourceId, long _score, long _updateTimeMs, _IContext _context)
    {
        if (_m_bTempClose)
            return false;

        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not set rank[" + _m_lInstanceId + "] obj when list is disable! objId: " + _objId + " | score: " + _score);
            return false;
        }

        _lock();
        try
        {
            //旧分数
            long oldScore = 0;
            //旧分数来源
            long oldScoreSourceId = 0;

            RankObj rankObj = lookup(_objId);
            if (null == rankObj)
            {
                rankObj = _ensure(_objId);
            }else
            {
                //旧分数
                oldScore = rankObj.getScore();
                //旧分数来源
                oldScoreSourceId = rankObj.getScoreSourceId();

                //分数没有变化，不更新排行
                if (oldScore == _score)
                    return true;

                //如果更新时间小于对象的更新时间，则不更新
                if (_updateTimeMs < rankObj.getUpdatedMs())
                    return true;
            }

            //设置分数
            rankObj._setScore(_scoreSourceId, _score, _updateTimeMs);
            //更新排行榜
            _onRankObjScoreChg(rankObj, oldScoreSourceId, oldScore, _context);

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 销毁排行榜元素，谨慎使用，会导致排行榜重排
     * @param _itemKey 排行榜元素查询key
     * @return _ARankObj 被移除的元素
     */
    public RankObj remove(long _objId)
    {
        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not remove rank[" + _m_lInstanceId + "] obj when list is disable! objId: " + _objId);
            return null;
        }

        _lock();
        try
        {
            //移除map中的元素
            RankObj removeObj = _m_mapRankItemMap.remove(_objId);
            if (null == removeObj)
                return null;

            //移除排行中的相同元素
            _m_lRankItemList.remove(removeObj);

            //开启数据库操作删除数据
            ALAsynTaskManager.getInstance().regTask(_m_rlmRankDBOper.getDBTaskThreadIndex()
                    , new AsynDelRankObjScoreTask(_m_rlmRankDBOper, removeObj.getDBId()));

            //排行榜重排
            _refreshRankIndex();

            return removeObj;
        } finally
        {
            _unlock();
        }
    }

// region 子集数据操作处理函数

    /**
     * 更新排行榜分数
     * @param _objId
     * @param _subObjId
     * @param _scoreSourceId
     * @param _score
     * @param _setGreater
     * @param _context
     * @return
     */
    public boolean setSubScore(long _objId, long _subObjId, long _scoreSourceId, long _score, boolean _setGreater, _IContext _context)
    {
        if (_m_bTempClose)
            return false;

        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not set rank[" + _m_lInstanceId + "] obj when list is disable! objId: " + _objId + " | score: " + _score);

            return false;
        }

        _lock();
        try
        {
            RankObj rankObj = _ensureSubRankObj(_objId);

            //获取原有数据
            RankSubObj subRankObj = rankObj._ensure(_subObjId);
            //旧分数
            long oldScore = subRankObj.getScore();
            //旧分数来源
            long oldScoreSourceId = subRankObj.getScoreSourceId();
            //设置分数
            RankSubObj rankSubObj = rankObj._setSubScore(_subObjId, _scoreSourceId, _score, _setGreater);
            if (rankSubObj != subRankObj)
            {
                ALServerLog.Fatal("rank sub obj is not enure obj!");
            }

            //更新排行榜
            if (rankObj.getScore() != oldScore || rankSubObj.getScoreSourceId() != oldScoreSourceId)
            {
                _onRankSubObjScoreChg(rankObj, rankSubObj, oldScoreSourceId, oldScore, _context);
            }

            return true;
        } finally
        {
            _unlock();
        }
    }

    public boolean chgSubScore(long _objId, long _subObjId, long _scoreSourceId, long _chgScore, _IContext _context)
    {
        if (_m_bTempClose)
            return false;

        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not change rank[" + _m_lInstanceId + "] obj when list is disable! objId: " + _objId + " | chgScore: " + _chgScore);
            return false;
        }

        _lock();
        try
        {
            RankObj rankObj = _ensureSubRankObj(_objId);

            //获取原有数据
            RankSubObj subRankObj = rankObj._ensure(_subObjId);
            //旧分数
            long oldScore = subRankObj.getScore();
            //旧分数来源
            long oldScoreSourceId = subRankObj.getScoreSourceId();
            //修改分数
            RankSubObj rankSubObj = rankObj._chgSubScore(_subObjId, _scoreSourceId, _chgScore);
            if (rankSubObj != subRankObj)
            {
                ALServerLog.Fatal("rank sub obj is not enure obj!");
            }

            //更新排行榜
            if (rankObj.getScore() != oldScore || rankSubObj.getScoreSourceId() != oldScoreSourceId)
            {
                _onRankSubObjScoreChg(rankObj, rankSubObj, oldScoreSourceId, oldScore, _context);
            }

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新排行榜分数
     * @param _objId
     * @param _subObjId
     * @param _scoreSourceId
     * @param _score
     * @param _updateTimeMs
     * @param _context
     * @return
     */
    public boolean setSubScoreCompareUpdateTime(long _objId, long _subObjId, long _scoreSourceId, long _score, long _updateTimeMs, _IContext _context)
    {
        if (_m_bTempClose)
            return false;

        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not set rank[" + _m_lInstanceId + "] obj when list is disable! objId: " + _objId + " | score: " + _score);
            return false;
        }

        _lock();
        try
        {
            //旧分数
            long oldScore = 0;
            //旧分数来源
            long oldScoreSourceId = 0;

            RankObj rankObj = _ensureSubRankObj(_objId);

            //获取原有数据
            RankSubObj subRankObj = rankObj.lookup(_subObjId);
            if (subRankObj != null)
            {
                //旧分数
                oldScore = subRankObj.getScore();
                //旧分数来源
                oldScoreSourceId = subRankObj.getScoreSourceId();
            }

            //设置分数
            RankSubObj rankSubObj = rankObj._setSubScoreCompareUpdateTime(_subObjId, _scoreSourceId, _score, _updateTimeMs);
            if (rankSubObj != subRankObj)
            {
                ALServerLog.Fatal("rank sub obj is not ensure obj!");
            }

            //更新排行榜
            if (rankObj.getScore() != oldScore || rankSubObj.getScoreSourceId() != oldScoreSourceId)
            {
                _onRankSubObjScoreChg(rankObj, rankSubObj, oldScoreSourceId, oldScore, _context);
            }

            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 销毁排行榜元素，谨慎使用，会导致排行榜重排
     * @param _subObjId 排行榜子集元素主体id
     */
    public void removeSubObj(long _subObjId, _IContext _context)
    {
        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not remove rank[" + _m_lInstanceId + "] obj when list is disable! subObjId: " + _subObjId);
            return;
        }

        _lock();
        try
        {
            //子集数据需要遍历所有队列删除
            RankObj tmpRank = null;
            for (int i = 0; i < _m_lRankItemList.size(); i++)
            {
                tmpRank = _m_lRankItemList.get(i);
                if (null == tmpRank)
                    continue;

                //旧分数
                long oldScore = tmpRank.getScore();
                //旧分数来源
                long oldScoreSourceId = tmpRank.getScoreSourceId();

                //移除数据
                RankSubObj removeSubObj = tmpRank._removeSubScore(_subObjId);
                if (removeSubObj != null)
                {
                    //如果子集排行为空，则删除该排行
                    if (tmpRank.isSubRankEmpty())
                    {
                        //移除map中的元素
                        _m_mapRankItemMap.remove(tmpRank.getObjId());
                        //移除排行中的相同元素
                        _m_lRankItemList.remove(tmpRank);

                        //排行榜重排
                        _refreshRankIndex();
                    }else
                    {
                        //对排行进行重新排序
                        _onRankObjScoreChg(tmpRank, oldScoreSourceId, oldScore, _context);
                    }
                }
            }
        } finally
        {
            _unlock();
        }
    }

    public void removeSubObj(long _objId, long _subObjId, _IContext _context)
    {
        //无效状态无法进行设置值操作
        if (!_m_bEnable)
        {
            ALServerLog.Fatal("Can not remove rank[" + _m_lInstanceId + "] obj when list is disable! subObjId: " + _subObjId);
            return;
        }

        _lock();
        try
        {
            //子集数据需要遍历所有队列删除
            RankObj rankObj = _m_mapRankItemMap.get(_objId);
            if (null == rankObj)
                return;

            //旧分数
            long oldScore = rankObj.getScore();
            //旧分数来源
            long oldScoreSourceId = rankObj.getScoreSourceId();

            //移除数据
            RankSubObj removeSubObj = rankObj._removeSubScore(_subObjId);
            if (removeSubObj != null)
            {
                //如果子集排行为空，则删除该排行
                if (rankObj.isSubRankEmpty())
                {
                    //移除map中的元素
                    _m_mapRankItemMap.remove(_objId);
                    //移除排行中的相同元素
                    _m_lRankItemList.remove(rankObj);

                    //排行榜重排
                    _refreshRankIndex();
                }else
                {
                    //对排行进行重新排序
                    _onRankObjScoreChg(rankObj, oldScoreSourceId, oldScore, _context);
                }
            }
        } finally
        {
            _unlock();
        }
    }

    // endregion 子集数据操作处理函数结束

    /**
     * 从数据库初始化
     * @param _rankObj 排行榜元素对象
     * @return 初始化是否成功
     */
    protected void _initAddRankObj(RankObj _rankObj)
    {
        if (null == _rankObj)
            throw new NullPointerException();

        _lock();
        try
        {
            _rankObj._initRankList(this);

            _m_mapRankItemMap.put(_rankObj.getObjId(), _rankObj);
            _m_lRankItemList.add(_rankObj);
        } finally
        {
            _unlock();
        }
    }

    /*************
     * 初始化子集数据对象，会自动创建归属对象，并在对应一级对象下增加子集数据对象
     * @param _objId
     * @param _rankSubObj
     */
    protected void _initAddRankSubObj(long _objId, RankSubObj _rankSubObj)
    {
        if (null == _rankSubObj)
            throw new NullPointerException();

        _lock();
        try
        {
            //获取对应数据
            RankObj rankObj = _ensureSubRankObj(_objId);
            //初始化子集数据
            rankObj._initAddRankSubObj(_rankSubObj);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过排序方法重新排序整个排行
     */
    protected void _initReRankAll()
    {
        _lock();
        try
        {
            //使用子类提供的排序方法排序
            _m_rcRankComparer.reRank(_m_lRankItemList);

            //重设排行
            for (int i = 0; i < _m_lRankItemList.size(); i++)
            {
                RankObj rankObj = _m_lRankItemList.get(i);
                if (rankObj == null)
                {
                    continue;
                }
                rankObj._setRank(i + 1);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 销毁数据
     */
    protected void _clearData()
    {
        _lock();
        try
        {
            _m_mapRankItemMap.clear();
            _m_lRankItemList.clear();

            //开启数据库任务删除数据
            ALAsynTaskManager.getInstance().regTask(_getRankDBOper().getDBTaskThreadIndex()
                    , new AsynClearRankDataTask(_getRankDBOper(), getInstanceId()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 删除排行
     */
    protected void _discardAllInfo()
    {
        _lock();
        try
        {
            //清除内部数据
            _clearData();
            
            //删除排行的基本信息
            ALAsynTaskManager.getInstance().regTask(_getRankDBOper().getDBTaskThreadIndex()
                    , new AsynDelRankInfoTask(_getRankDBOper(), getInstanceId()));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保创建排行榜元素数据
     * @return _objId 主体id
     */
    private RankObj _ensure(long _objId)
    {
        _lock();
        try
        {
            RankObj rankObj = _m_mapRankItemMap.get(_objId);
            if (rankObj == null)
            {
                //创建
                rankObj = _createRankObj(_objId, 0, 0, _m_lRankItemList.size() + 1);

                //初始化归属队列
                rankObj._initRankList(this);

                //放入容器中
                _m_mapRankItemMap.put(_objId, rankObj);
                _m_lRankItemList.add(rankObj);

                _relocateRankItem(rankObj, true, true);
            }

            return rankObj;
        } finally
        {
            _unlock();
        }
    }

    /*************
     * 创建子集排行对象，由于子集对象是特殊管理的，这里不走mgr的创建操作
     * @param _objId
     * @return
     */
    private RankObj _ensureSubRankObj(long _objId)
    {
        _lock();
        try
        {
            RankObj rankObj = _m_mapRankItemMap.get(_objId);
            if (rankObj == null)
            {
                //创建
                rankObj = new RankObj(_objId, _m_lRankItemList.size() + 1);

                //初始化归属队列
                rankObj._initRankList(this);

                //放入容器中
                _m_mapRankItemMap.put(_objId, rankObj);
                _m_lRankItemList.add(rankObj);

                _relocateRankItem(rankObj, true, true);
            }

            return rankObj;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 元素数据的排行分数变更
     * @param _aRankObj         排行元素
     * @param _oldScoreSourceId
     * @param _oldScore         旧分数
     * @param _context
     */
    private void _onRankObjScoreChg(RankObj _aRankObj, long _oldScoreSourceId, long _oldScore, _IContext _context)
    {
        _lock();
        try
        {
            long newScore = _aRankObj.getScore();
            int oldRank = _aRankObj.getRank();

            //冒泡排序重新排队排行数据
            _relocateRankItem(_aRankObj, newScore - _oldScore > 0, false);

            //调用触发事件函数
            _onRankObjChg_inLock(_aRankObj, _oldScoreSourceId, _oldScore, oldRank, _context);
        } finally
        {
            _unlock();
        }
    }

    private void _onRankSubObjScoreChg(RankObj _aRankObj, RankSubObj _rankSubObj, long _oldScoreSourceId, long _oldScore, _IContext _context)
    {
        _lock();
        try
        {
            long newScore = _aRankObj.getScore();
            int oldRank = _aRankObj.getRank();

            //冒泡排序重新排队排行数据
            _relocateRankItem(_aRankObj, newScore - _oldScore > 0, false);

            //调用触发事件函数
            _onRankSubObjChg_inLock(_aRankObj, _rankSubObj, _oldScoreSourceId, _oldScore, oldRank, _context);
        } finally
        {
            _unlock();
        }
    }


    /**
     * 冒泡法重新定位位置
     * @param _item  待排序对象
     * @param _bUp   是否向上查找
     * @param _isNew
     */
    private void _relocateRankItem(RankObj _item, boolean _bUp, boolean _isNew)
    {
        _lock();
        try
        {
            if (_bUp) //向上查找
            {
                //记录主动变化玩家的原排名
                int initiatorOldRank = _item.getRank();

                //超越目标
                int index = _item.getRank() - 1; //当前item的位置下标
                int preIndex = index - 1;
                //是否有排名变更
                boolean chg = false;
                while (preIndex >= 0)
                {
                    RankObj preItem = _m_lRankItemList.get(preIndex);
                    //对比数据，>0表示大，需要往前移
                    if (_m_rcRankComparer.compareItemFunc(preItem, _item) > 0)
                    {
                        _m_lRankItemList.set(index, preItem);
                        //比较对象的原排名
                        int preItemOriRank = preItem.getRank();
                        preItem._setRank(index + 1);

                        index--;
                        //设置有排名变更
                        chg = true;

                        preIndex = index - 1;

                        //调用触发对象排行变更的事件函数
                        _onRankObjRankBeChanged_inLock(preItem, preItemOriRank);
                    } else
                    {
                        break;
                    }
                }

                //数据有变更则最后设置一次数据
                if (chg)
                {
                    _m_lRankItemList.set(index, _item);
                    _item._setRank(index + 1);

                    //调用主动变更排名的事件函数
                    _onRankObjRankChanged_inLock(_item, initiatorOldRank);
                }
            } else //向下查找
            {
                //记录主动变化玩家的原排名
                int initiatorOldRank = _item.getRank();

                int index = _item.getRank() - 1;
                int nextIndex = index + 1;
                //是否有排名变更
                boolean chg = false;
                while (nextIndex < _m_lRankItemList.size())
                {
                    RankObj nextItem = _m_lRankItemList.get(nextIndex);
                    //对比数据，<0表示小，需要往后移
                    if (_m_rcRankComparer.compareItemFunc(nextItem, _item) < 0)
                    {
                        _m_lRankItemList.set(index, nextItem);
                        int nextItemOldRank = nextItem.getRank();
                        nextItem._setRank(index + 1);

                        index++;
                        //设置有排名变更
                        chg = true;

                        nextIndex = index + 1;

                        //调用触发对象排行变更的事件函数
                        _onRankObjRankBeChanged_inLock(nextItem, nextItemOldRank);
                    } else
                    {
                        break;
                    }
                }

                //数据有变更则最后设置一次数据
                if (chg)
                {
                    _m_lRankItemList.set(index, _item);
                    _item._setRank(index + 1);

                    //调用主动变更排名的事件函数
                    _onRankObjRankChanged_inLock(_item, initiatorOldRank);
                }
            }

            //新加入的元素，触发排名变更事件
            if (_isNew)
            {
                //调用主动变更排名的事件函数
                _onRankObjRankChanged_inLock(_item, 0);
            }

            //检查排行榜长度
            _checkRankListSize();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 保留前最大数量上限的数据
     */
    private void _checkRankListSize()
    {
        //数据保留
//        while (_m_lRankItemList.size() > _m_iRankMaxSize) 
//        {
//            int idx = _m_lRankItemList.size() - 1;
//            RankObj item = _m_lRankItemList.remove(idx);
//            if (null == item) 
//                continue;
//
//            //移除映射表数据
//            _m_mapRankItemMap.remove(item.getObjId());
//            //开启数据库操作删除数据
//            ALAsynTaskManager.getInstance().regTask(_m_rlmRankListMgr.getDBTaskThreadIndex()
//                    , new AsynDelRankObjScoreTask(_m_rlmRankListMgr, item.getDBId()));
//        }
    }

    /**
     * 刷新所有排行数据当前下标
     * 此操作不进行排序
     */
    private void _refreshRankIndex()
    {
        _lock();
        try
        {
            //重设排行
            for (int i = 0; i < _m_lRankItemList.size(); i++)
            {
                RankObj rankObj = _m_lRankItemList.get(i);
                if (rankObj == null)
                {
                    continue;
                }

                rankObj._setRank(i + 1);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造排行榜基础数据列表
     */
    public List<Rank_BaseItem> makeRankObjBaseList()
    {
        _lock();
        try
        {
            List<Rank_BaseItem> list = new ArrayList<>();
            for (int i = 0; i < _m_lRankItemList.size(); i++)
            {
                RankObj obj = _m_lRankItemList.get(i);
                if (null == obj)
                    continue;

                list.add(obj.toBaseProto());
            }
            return list;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造排行榜基础数据列表
     * @param _limit 限制返回数量
     */
    public List<Rank_BaseItem> makeRankObjBaseList(int _limit)
    {
        _lock();
        try
        {
            List<Rank_BaseItem> list = new ArrayList<>();
            for (int i = 0; i < _m_lRankItemList.size(); i++)
            {
                if (list.size() >= _limit)
                    break;

                RankObj obj = _m_lRankItemList.get(i);
                if (null == obj)
                    continue;

                list.add(obj.toBaseProto());
            }

            return list;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造排行榜基础数据通过排名
     */
    public Rank_BaseItem makeRankObjBaseByRank(int _rank)
    {
        _lock();
        try
        {
            RankObj rankObj = lookupByRank(_rank);
            return rankObj == null ? null : rankObj.toBaseProto();
        } finally
        {
            _unlock();
        }
    }


    /**
     * 构造排行榜基础数据通过主体id
     */
    public Rank_BaseItem makeRankObjBaseByKey(long _key)
    {
        _lock();
        try
        {
            RankObj rankObj = lookup(_key);
            return rankObj == null ? null : rankObj.toBaseProto();
        } finally
        {
            _unlock();
        }
    }
    /**
     * 构造排行榜基础数据通过（主体id+子id）
     * @param _key
     * @param _subKey
     * @return
     */
    public Rank_BaseItem makeRankObjBaseByKey2(long _key, long _subKey)
    {
        _lock();
        try
        {
            RankObj rankObj = lookup(_key);
            if(null == rankObj)
            	return null;
            
            RankSubObj rankSubObj = rankObj.lookup(_subKey);
            return rankSubObj == null ? null : rankObj.toBaseProto();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 拉取指定排名范围的排行榜数据 指定us
     * @param _usFilterFunc us过滤函数
     * @param _limitRank    限制排名
     * @return 排行榜数据
     */
    public List<Rank_ItemDump> dumpRankObjListWithLimitRankByUs(Predicate<Integer> _usFilterFunc, int _limitRank)
    {
        _lock();
        try
        {
            List<Rank_ItemDump> rankItemList = new ArrayList<>();

            for (RankObj rankObj : _m_lRankItemList)
            {
                if (_limitRank > 0 && rankObj.getRank() >= _limitRank)
                    break;

                rankObj.toDumpProto(_usFilterFunc, rankItemList);
            }

            return rankItemList;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 构造所有的排行榜数据列表
     * @return 排行榜数据
     */
    public List<ServerObj_RankObjInfo> makeAllRankObjInfoList()
    {
        _lock();
        try
        {
            List<ServerObj_RankObjInfo> rankItemList = new ArrayList<>();

            for (RankObj rankObj : _m_lRankItemList)
            {
                rankItemList.add(rankObj.makeRankObjInfo());
            }

            return rankItemList;
        }
        finally
        {
            _unlock();
        }
    }


    /**
     * 设置临时开启
     */
    public void setTempStartRank()
    {
        if (!_m_bTempClose)
            return;

        _m_bTempClose = false;
        CommLog.info("temp start Rank rankId:{}", _m_lInstanceId);
    }

    /**
     * 设置排行临时关闭
     */
    public void setTempCloseRank()
    {
        if (_m_bTempClose)
            return;

        _m_bTempClose = true;
        CommLog.info("temp close Rank rankId:{}", _m_lInstanceId);
    }


    /******************
     * 创建一个新数据对象返回，在进行子集排行处理的时候不会通过本函数创建排行数据对象
     * @param _objId
     * @param _score
     * @param _rank
     * @return
     */
    protected abstract RankObj _createRankObj(long _objId, long _scoreSourceId, long _score, int _rank);

    /******************
     * 创建一个新的子集数据对象返回
     * @param _objId
     * @param _subObjId
     * @param _score
     * @return
     */
    protected abstract RankSubObj _createRankSubObj(long _objId, long _subObjId, long _scoreSourceId, long _score);
    
    /***************
     * 在本排行开启的时候处理的函数
     * 
     * 如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）
     */
    protected abstract void _onRankCreated_inLock();
    
    /*****************
     * 在本排行关闭的时候触发的事件函数
     * 
     * 如果因为锁而无法处理，则需要开启Syn任务处理（注意Syn任务的时序是不确定的，需要根据实际的状态做处理）
     */
    protected abstract void _onRankClose_inLock();
    
    /*******************
     * 排行对象在逻辑导致排行变更的时候触发的锁内事件（被动波及）
     * 注意：这里的逻辑不是自己主动改变的情况，是被动改变的时候触发的
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     *
     * @param _rankObj 被动波及的排行对象
     * @param _preRank 原排名
     */
    protected abstract void _onRankObjRankBeChanged_inLock(RankObj _rankObj, int _preRank);

    /*******************
     * 排行对象主动变更排名时触发的锁内事件
     * 注意：这里是对象因自身分数变化导致排名变更
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     *
     * @param _rankObj 主动变更排名的对象
     * @param _preRank 原排名
     */
    protected abstract void _onRankObjRankChanged_inLock(RankObj _rankObj, int _preRank);

    /**
     * 排行榜元素变更通知
     * （如需要执行外部逻辑，需要自己调用syn任务处理）
     * @param _rankObj    变更后的排行榜元素 如果需要新分数或新排名，可以从对象中直接取
     * @param _oldScoreSourceId   旧分数来源
     * @param _oldScore   旧分数
     * @param _oldRank    旧排行
     * @param _m_cContext
     */
    protected abstract void _onRankObjChg_inLock(RankObj _rankObj, long _oldScoreSourceId, long _oldScore, int _oldRank, _IContext _m_cContext);
    protected abstract void _onRankSubObjChg_inLock(RankObj _rankObj, RankSubObj _rankSubObj, long _oldScoreSourceId, long _oldScore, int _oldRank, _IContext _m_cContext);

    
    /******************
     * 设置这个排行榜需要被销毁
     * 
     * 后续子类应该根据实际排行榜情况直接删除或者向跨服排行榜服务器发送请求，并在确认跨服排行接收到删除请求后本地才可以删除
     * 如果是总排行，可能需要等待所有子对象清理完毕后才能进行删除处理
     * 
     * @param _realDiscardAction 实际处理删除数据的处理对象
     */
    protected abstract void _setRankListNeedDiscard(_IALProcessAction _realDiscardAction);
}