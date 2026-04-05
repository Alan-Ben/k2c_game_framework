package NPCommon.CommonRank;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicServer.ALServerAsynTask.ALAsynTaskManager;
import ALServerLog.ALServerLog;
import Common.RankObj.Rank_BaseItem;
import Common.RankObj.Rank_BaseSubItem;
import Common.RankObj.Rank_ItemDump;
import Common.ServerObj.ServerObj_RankObjInfo;
import NPCommon.CommonRank.DBTask.AsynDelRankSubObjScoreTask;
import NPCommon.CommonRank.DBTask.AsynUpdateRankObjScoreTask;
import NPCommon.CommonRank.DBTask.AsynUpdateRankSubObjScoreTask;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;
import java.util.List;
import java.util.function.Consumer;
import java.util.function.Predicate;

/**
 * @description: 排行榜数据存储对象
 * @author: mark
 * @date: 2022-06-17 14:15:17
 */
public class RankObj
{
    //记录归属排行，单纯用于子集处理的时候加锁处理
    private _ARankList _m_rlRankList;

    //数据Id，当作为二级数据模式的时候，数据Id无效
    private long _m_lDbId;

    /**
     * 当前排名,初始化时设置成所属排行最后一名
     */
    protected int _m_iCurRank;

    //数据对应Id，根据不同排行使用不同Id，具体什么Id取决于外部调用方式
    protected long _m_lObjId;
    //对应分数来源
    protected long _m_lScoreSourceId;
    //对应分数
    protected long _m_lScore;
    //对应变更时间戳
    protected long _m_lUpdatedMs;

    //数据子集管理队列，一般一个Obj要么是单级，要么是子集方式，这边需要对是否子集进行判断处理
    private boolean _m_bIsSubRank;
    private ArrayList<RankSubObj> _m_lSubObjList;

    //单级模式的排行创建处理
    public RankObj(long _dbId, long _objId, long _scoreSourceId, long _score, long _updatedMs, int _rank)
    {
        _m_rlRankList = null;

        _m_lDbId = _dbId;

        _m_iCurRank = _rank;

        _m_lObjId = _objId;
        _m_lScoreSourceId = _scoreSourceId;
        _m_lScore = _score;
        _m_lUpdatedMs = _updatedMs;

        _m_bIsSubRank = false;
        _m_lSubObjList = null;
    }

    //子集模式的排行创建处理
    public RankObj(long _objId, int _rank)
    {
        _m_rlRankList = null;

        _m_lDbId = 0;

        _m_iCurRank = _rank;

        _m_lObjId = _objId;
        _m_lScore = 0;
        _m_lUpdatedMs = 0;

        _m_bIsSubRank = true;
        _m_lSubObjList = new ArrayList<RankSubObj>();
    }

    //region get&&set
    public long getDBId()
    {
        return _m_lDbId;
    }

    public int getRank()
    {
        return _m_iCurRank;
    }

    public long getObjId()
    {
        return _m_lObjId;
    }

    public long getScore()
    {
        return _m_lScore;
    }

    public long getScoreSourceId()
    {
        return _m_lScoreSourceId;
    }

    public long getUpdatedMs()
    {
        return _m_lUpdatedMs;
    }

    //框架内设置rankList的操作
    protected void _initRankList(_ARankList _rankList)
    {
        _m_rlRankList = _rankList;
    }


    /*************
     * 拷贝排行数据
     * @return
     */
    public ArrayList<RankSubObj> cloneRankSubObjList()
    {
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to cloneRankSubObjList when rank is not subObjRank! rankId: " + _m_rlRankList.getInstanceId());
            return null;
        }

        _m_rlRankList._lock();
        try
        {
            return new ArrayList<>(_m_lSubObjList);
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    //endregion

    /**
     * 查询方法
     * @param _itemKey 查询key值
     * @return _ARankObj
     */
    public RankSubObj lookup(long _subObjId)
    {
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to cloneRankSubObjList when rank is not subObjRank! rankId: " + _m_rlRankList.getInstanceId());
            return null;
        }

        _m_rlRankList._lock();
        try
        {
            return _lookupSubObj(_subObjId);
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    /**********************
     * 遍历所有排行对象进行的操作，在锁内
     * @param _iterator
     */
    public void iteratorAllRankSubObj_inLockAtoM(_IRankSubObjIterator _iterator)
    {
        if (null == _iterator)
            return;

        _m_rlRankList._lock();
        try
        {
            //进行遍历处理
            for (int i = 0; i < _m_lSubObjList.size(); i++)
            {
                _iterator.doIterator(_m_lSubObjList.get(i));
            }
        } finally
        {
            _m_rlRankList._unlock();
        }
    }


    /**
     * 通过给定过滤方法查询一批数据
     * @param _testFunc     测试方法，返回true时，rankObj会被加入结果列表中
     * @param _supplierFunc 消费方法，不为空时每一个被过滤到的元素，都会调用该方法处理
     * @return ArrayList<_ARankObj>
     */
    public ArrayList<RankSubObj> lookupByCheckFunc(Predicate<RankSubObj> _testFunc, Consumer<RankSubObj> _supplierFunc)
    {
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to cloneRankSubObjList when rank is not subObjRank! rankId: " + _m_rlRankList.getInstanceId());
            return null;
        }

        _m_rlRankList._lock();
        try
        {
            ArrayList<RankSubObj> rankObjs = new ArrayList<>();
            for (RankSubObj rankObj : _m_lSubObjList)
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
            _m_rlRankList._unlock();
        }
    }

    /**
     * 设置排名，只允许本元素所属排行榜使用该方法
     * @param _rank 排名
     */
    protected void _setRank(int _rank)
    {
        _m_iCurRank = _rank;
    }

    /**********
     * 设置和更改分数操作
     * @param _score
     * @param _updateTimeMs
     */
    protected void _setScore(long _scoreSourceId, long _score, long _updateTimeMs)
    {
        //判断是否单级处理，如果非单级则不允许操作，需要报错
        if (_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to set RankObj Score for a SubObjRank Obj! rankId: "
                    + _m_rlRankList.getInstanceId() + " | objId: " + _m_lObjId  + " | scoreSourceId: " + _scoreSourceId + " | score: " + _score);
            return;
        }

        _m_lScoreSourceId = _scoreSourceId;
        _m_lScore = _score;
        //更新本对象的时间戳
        _m_lUpdatedMs = _updateTimeMs;

        //开启数据库任务处理
        ALAsynTaskManager.getInstance().regTask(_m_rlRankList._getRankDBOper().getDBTaskThreadIndex()
                , new AsynUpdateRankObjScoreTask(_m_rlRankList._getRankDBOper(), _m_lDbId, _m_lScoreSourceId, _m_lScore, _m_lUpdatedMs));
    }

    protected void _chgScore(long _scoreSourceId, long _chgValue, long _updateTimeMs)
    {
        //判断是否单级处理，如果非单级则不允许操作，需要报错
        if (_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to chg RankObj Score for a SubObjRank Obj! rankId: "
                    + _m_rlRankList.getInstanceId() + " | objId: " + _m_lObjId  + " | scoreSourceId: " + _scoreSourceId + " | chgScore: " + _chgValue);
            return;
        }

        _m_lScoreSourceId = _scoreSourceId;
        _m_lScore += _chgValue;
        //更新本对象的时间戳
        _m_lUpdatedMs = _updateTimeMs;

        //开启数据库任务处理
        ALAsynTaskManager.getInstance().regTask(_m_rlRankList._getRankDBOper().getDBTaskThreadIndex()
                , new AsynUpdateRankObjScoreTask(_m_rlRankList._getRankDBOper(), _m_lDbId, _m_lScoreSourceId, _m_lScore, _m_lUpdatedMs));
    }

    /****************
     * 初始化添加排行数据的子集
     * @param _rankSubObj
     */
    protected void _initAddRankSubObj(RankSubObj _rankSubObj)
    {
        if (null == _rankSubObj)
            throw new NullPointerException();

        _m_rlRankList._lock();
        try
        {
            //添加新值
            _m_lScore += _rankSubObj.getScore();
            //尝试更新本对象的时间戳
            if (_rankSubObj.getUpdatedMs() > _m_lUpdatedMs)
                _m_lUpdatedMs = _rankSubObj.getUpdatedMs();

            //加入队列
            _m_lSubObjList.add(_rankSubObj);
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    /***************
     * 子集数据的操作接口
     * @param _score
     */
    protected RankSubObj _setSubScoreCompareUpdateTime(long _subObjId, long _scoreSourceId, long _score, long _updateTimeMs)
    {
        //判断是否单级处理，如果非单级则不允许操作，需要报错
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to set SubRankObj Score for a single Level Rank Obj! rankId: "
                    + _m_rlRankList.getInstanceId() + " | objId: " + _m_lObjId + " | subObjId: " + _subObjId + " | scoreSourceId: " + _scoreSourceId + " | score: " + _score);
            return null;
        }

        _m_rlRankList._lock();
        try
        {
            //查询数据
            RankSubObj subObj = _lookupSubObj(_subObjId);

            if (subObj == null)
            {
                subObj = _ensure(_subObjId);
                //如果原来没有数据, 则直接设置
                subObj._setScore(_scoreSourceId, _score, _updateTimeMs);
                //添加新值
                _m_lScore += subObj.getScore();
            }else
            {
                //清理原值
                _m_lScore -= subObj.getScore();

                //判断更新时间是否大于子对象的时间, 如果小于则不处理
                if (_updateTimeMs < subObj.getUpdatedMs())
                    return subObj;

                //设置值和来源
                subObj._setScore(_scoreSourceId, _score, _updateTimeMs);
                //添加新值
                _m_lScore += subObj.getScore();
            }

            //尝试更新本对象的时间戳
            if (subObj.getUpdatedMs() > _m_lUpdatedMs)
                _m_lUpdatedMs = subObj.getUpdatedMs();

            //开启数据库任务处理
            ALAsynTaskManager.getInstance().regTask(_m_rlRankList._getRankDBOper().getDBTaskThreadIndex()
                    , new AsynUpdateRankSubObjScoreTask(_m_rlRankList._getRankDBOper(), subObj.getDBId(), subObj.getScoreSourceId(), subObj.getScore(), subObj.getUpdatedMs()));

            return subObj;
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    /***************
     * 子集数据的操作接口
     * @param _score
     */
    protected RankSubObj _setSubScore(long _subObjId, long _scoreSourceId, long _score, boolean _setGreater)
    {
        //判断是否单级处理，如果非单级则不允许操作，需要报错
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to set SubRankObj Score for a single Level Rank Obj! rankId: "
                    + _m_rlRankList.getInstanceId() + " | objId: " + _m_lObjId + " | subObjId: " + _subObjId + " | scoreSourceId: " + _scoreSourceId + " | score: " + _score);
            return null;
        }

        _m_rlRankList._lock();

        try
        {
            //查询数据
            RankSubObj subObj = _ensure(_subObjId);

            //需要记录更大值，小于则返回
            long oldScore = subObj.getScore();
            if (_setGreater && _score < oldScore)
                return subObj;

            //清理原值
            _m_lScore -= subObj.getScore();
            //设置值和来源
            subObj._setScore(_scoreSourceId, _score, ALBasicCommonFun.getNowTimeMS());
            //添加新值
            _m_lScore += subObj.getScore();

            //尝试更新本对象的时间戳
            if (subObj.getUpdatedMs() > _m_lUpdatedMs)
                _m_lUpdatedMs = subObj.getUpdatedMs();

            //开启数据库任务处理
            ALAsynTaskManager.getInstance().regTask(_m_rlRankList._getRankDBOper().getDBTaskThreadIndex()
                    , new AsynUpdateRankSubObjScoreTask(_m_rlRankList._getRankDBOper(), subObj.getDBId(), subObj.getScoreSourceId(), subObj.getScore(), subObj.getUpdatedMs()));

            return subObj;
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    protected RankSubObj _chgSubScore(long _subObjId, long _scoreSourceId, long _chgScore)
    {
        //判断是否单级处理，如果非单级则不允许操作，需要报错
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to chg SubRankObj Score for a single Level Rank Obj! rankId: "
                    + _m_rlRankList.getInstanceId() + " | objId: " + _m_lObjId + " | subObjId: " + _subObjId + " | scoreSourceId: " + _scoreSourceId + " | chgScore: " + _chgScore);
            return null;
        }

        _m_rlRankList._lock();

        try
        {
            //查询数据
            RankSubObj subObj = _ensure(_subObjId);

            //清理原值
            _m_lScore -= subObj.getScore();
            //设置值
            subObj._chgScore(_scoreSourceId, _chgScore, ALBasicCommonFun.getNowTimeMS());
            //添加新值
            _m_lScore += subObj.getScore();

            //尝试更新本对象的时间戳
            if (subObj.getUpdatedMs() > _m_lUpdatedMs)
                _m_lUpdatedMs = subObj.getUpdatedMs();

            //开启数据库任务处理
            ALAsynTaskManager.getInstance().regTask(_m_rlRankList._getRankDBOper().getDBTaskThreadIndex()
                    , new AsynUpdateRankSubObjScoreTask(_m_rlRankList._getRankDBOper(), subObj.getDBId(), subObj.getScoreSourceId(), subObj.getScore(), subObj.getUpdatedMs()));

            return subObj;
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    /**
     * 销毁排行榜元素，谨慎使用，会导致排行榜重排
     * @param _itemKey 排行榜元素查询key
     * @return _ARankObj 被移除的元素
     */
    protected RankSubObj _removeSubScore(long _subObjId)
    {
        //判断是否单级处理，如果非单级则不允许操作，需要报错
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to remove SubRankObj for a single Level Rank Obj! rankId: " + _m_rlRankList.getInstanceId() + " | objId: " + _m_lObjId + " | subObjId: " + _subObjId);
            return null;
        }

        _m_rlRankList._lock();

        try
        {
            //查询数据
            RankSubObj subObj = _removeSubObj(_subObjId);
            if (null == subObj)
                return null;

            ALServerLog.Sys("Remove SubRankObj Rank: " + _m_rlRankList.getInstanceId() + " | rankObjId: " + _m_lObjId + " | subObjId: " + _subObjId);

            //清理原值
            _m_lScore -= subObj.getScore();

            //开启数据库操作删除数据
            ALAsynTaskManager.getInstance().regTask(_m_rlRankList._getRankDBOper().getDBTaskThreadIndex()
                    , new AsynDelRankSubObjScoreTask(_m_rlRankList._getRankDBOper(), subObj.getDBId()));

            return subObj;
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    //endregion

    /*****************
     * 检索子集数据的数据，一般子集数据不大，所以这里直接遍历检索
     * 此函数要保证在锁内调用
     * @param _subObjId
     * @return
     */
    private RankSubObj _lookupSubObj(long _subObjId)
    {
        if (null == _m_lSubObjList)
            return null;

        RankSubObj tmpObj = null;
        for (int i = 0; i < _m_lSubObjList.size(); i++)
        {
            tmpObj = _m_lSubObjList.get(i);
            if (null == tmpObj)
                continue;

            if (tmpObj.getSubObjId() == _subObjId)
                return tmpObj;
        }

        return null;
    }

    /*************
     * 从子集数据中删除对应数据
     * 此函数要保证在锁内调用
     * @param _subObjId
     * @return
     */
    private RankSubObj _removeSubObj(long _subObjId)
    {
        if (null == _m_lSubObjList)
            return null;

        RankSubObj tmpObj = null;
        for (int i = 0; i < _m_lSubObjList.size(); i++)
        {
            tmpObj = _m_lSubObjList.get(i);
            if (null == tmpObj)
                continue;

            if (tmpObj.getSubObjId() == _subObjId)
            {
                _m_lSubObjList.remove(i);
                return tmpObj;
            }
        }

        return null;
    }

    /***********
     * 查询子集数据，如果不存在则创建
     * 此函数要保证在锁内调用
     * @param _subObjId
     * @return
     */
    protected RankSubObj _ensure(long _subObjId)
    {
        if (null == _m_lSubObjList)
            return null;

        RankSubObj subObj = _lookupSubObj(_subObjId);
        if (null == subObj)
        {
            //调用总管理对象的创建操作
            subObj = _m_rlRankList._createRankSubObj(_m_lObjId, _subObjId, 0, 0);
            _m_lSubObjList.add(subObj);
        }

        return subObj;
    }

    /**
     * 获取排行榜基础数据
     * @return
     */
    public Rank_BaseItem toBaseProto()
    {
        Rank_BaseItem proto = new Rank_BaseItem();
        proto.setKey(getObjId());
        proto.setSourceId(getScoreSourceId());
        proto.setScore(getScore());
        proto.setRank(getRank());
        return proto;
    }

    /**
     * 获取排行榜基础数据
     * @return
     */
    public void toDumpProto(Predicate<Integer> _usFilterFunc, List<Rank_ItemDump> _dumpList)
    {
        //判断是否是子集排行，如果是子集排行则需要遍历子集数据
        if (!_m_bIsSubRank)
        {
            int usId = CommonFunc.parseServerTypeIdFromCid(getObjId());
            if (!_usFilterFunc.test(usId))
                return;

            Rank_ItemDump proto = new Rank_ItemDump();
            proto.setKey(getObjId());
            proto.setRank(getRank());
            proto.setScore(getScore());
            _dumpList.add(proto);
        }else
        {
            _m_rlRankList._lock();
            try{
                for (RankSubObj rankSubObj : _m_lSubObjList)
                {
                    int usId = CommonFunc.parseServerTypeIdFromCid(rankSubObj.getSubObjId());
                    if (!_usFilterFunc.test(usId))
                        continue;

                    Rank_ItemDump proto = new Rank_ItemDump();
                    proto.setKey(rankSubObj.getSubObjId());
                    proto.setGroupId(getObjId());
                    proto.setRank(getRank());
                    proto.setScore(getScore());
                    _dumpList.add(proto);
                }
            }finally
            {
                _m_rlRankList._unlock();
            }
        }
    }

    /**
     * 判断是否是子集排行，且为空
     * @return
     */
    public boolean isSubRankEmpty()
    {
        if (!_m_bIsSubRank)
        {
            ALServerLog.Fatal("Try to check SubRankObj empty for a single Level Rank Obj! rankId: " + _m_rlRankList.getInstanceId() + " | objId: " + _m_lObjId);
            return false;
        }

        _m_rlRankList._lock();
        try
        {
            return _m_lSubObjList.isEmpty();
        } finally
        {
            _m_rlRankList._unlock();
        }
    }

    /**
     * 构造排行榜对象数据协议
     * @return
     */
    public ServerObj_RankObjInfo makeRankObjInfo()
    {
        ServerObj_RankObjInfo info = new ServerObj_RankObjInfo();
        info.setObjId(getObjId());

        _m_rlRankList._lock();
        try{
            //判断是否是子集排行，如果是子集排行则需要遍历子集数据
            if (_m_bIsSubRank)
            {
                for (RankSubObj rankSubObj : _m_lSubObjList)
                {
                    info.addSubObjList(rankSubObj.toRankSubObjInfo());
                }
            }else
            {
                info.setScoreSourceId(getScoreSourceId());
                info.setScore(getScore());
                info.setUpdatedMs(getUpdatedMs());
            }
        }finally
        {
            _m_rlRankList._unlock();
        }
        return info;
    }
    
    /**
     * 构造子数据列表
     * @param _list
     */
    public List<Rank_BaseSubItem> makeRankObjBaseSubList()
    {
    	_m_rlRankList._lock();
    	
        try
        {
        	ArrayList<Rank_BaseSubItem> list = new ArrayList<>();
            for (RankSubObj rankSubObj : _m_lSubObjList)
            {
                if(null == rankSubObj)
                	continue;
                
                list.add(rankSubObj.toRankBaseSubItem());
            }
            
            return list;
        }
        finally
        {
            _m_rlRankList._unlock();
        }
    }
}
