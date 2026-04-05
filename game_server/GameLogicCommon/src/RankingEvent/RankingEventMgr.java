package RankingEvent;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexObject;
import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPEnum.ERankingEventListenerType;
import NPGameRes.Refs._ARefRankingEvent;
import RankingEvent.Listener.LocalEventListener;
import RankingEvent.Listener.RemoteEventListener;
import RankingEvent.Listener._AEventListenerDealer;

import java.util.Hashtable;

/**
 * 排行榜相关事件的总监听事件管理器
 *
 * 本对象只存在于注册事件的目标服务器
 *
 * 使用本系统需要通过本类调用init函数进行初始化
 */
public class RankingEventMgr
{
    //环境对象
    private _IRankingEventEnv _m_iRankingEnv;
    //是否初始化
    private boolean _m_isInit;
    private boolean _m_isInitDone;

    //消息监听对象的管理数据集
    private Hashtable<Integer, EventListenerObj> _m_htEventListenerTable;
    //数据集的锁管理对象
    private MutexObject _m_mutex;

    public RankingEventMgr()
    {
        _m_iRankingEnv = null;
        _m_isInit = false;
        _m_isInitDone = false;

        _m_htEventListenerTable = new Hashtable<Integer, EventListenerObj>();
        _m_mutex = new MutexObject();
    }

    protected void _lock() { _m_mutex.lock(); }
    protected void _unlock() { _m_mutex.unlock(); }

    public _IRankingEventEnv getEnv() {return _m_iRankingEnv;}

    /****************
     * 检查初始化状态
     * @return
     */
    public boolean checkInit()
    {
        //判断是否有开始初始化
        if(!_m_isInit)
        {
            ALServerLog.Error("RankingEventMgr has not start initialized!");
            return false;
        }

        //判断是否结束初始化
        if(!_m_isInitDone)
        {
            ALServerLog.Error("RankingEventMgr has not finish initialized!");
            return false;
        }

        return true;
    }


    /**
     * 设置排行榜处理对象的环境接口对象
     */
    public synchronized boolean init(_IRankingEventEnv _env)
    {
        if (_m_isInit)
        {
            ALServerLog.Error("RankingEventMgr has been initialized!");
            return false;
        }
        //直接设置变量，避免多次调用初始化
        _m_isInit = true;

        _m_iRankingEnv = _env;

        //此处调用环境中的初始化处理
        if (null != _m_iRankingEnv)
        {
            _m_iRankingEnv.onEventMgrInit(this);
        }

        //调用处理后再设置变量，确保不会过程中判断状态错误
        _m_isInitDone = true;

        return true;
    }

    /****************
     * 注册一个本地排行监听处理对象
     * @param _listenerType
     * @param _key
     * @param _usId
     * @param _dealSerialize
     */
    public long regLocalRankingEvent(ERankingEventListenerType _listenerType, long _key, int _usId, long _dealSerialize)
    {
        _ARefRankingEvent rankingEventRef = _ARefRankingEvent.getRefRankingEvent(_listenerType, _key);
        if(null == rankingEventRef)
        {
            CommLog.error("RankingEventMgr regLocalRankingEvent can't find rankingEventRef, _listenerType:{} _key:{}", _listenerType, _key);
            return -1;
        }

        //申请一个反注册序列号
        long unRegSerial = ALSerializeMaker.makeNewSerialize();

        //注册本地处理对象
        _regRanking(rankingEventRef, new LocalEventListener(rankingEventRef, _usId, _dealSerialize, unRegSerial));

        return unRegSerial;
    }

    /**
     * 注册一个远程排行监听处理对象
     * @param _listenerType
     * @param _key
     * @param _usId
     * @param _dealSerialize
     * @return
     */
    public long regRemoteRankingEvent(ERankingEventListenerType _listenerType, long _key, int _usId, long _dealSerialize)
    {
        _ARefRankingEvent rankingEventRef = _ARefRankingEvent.getRefRankingEvent(_listenerType, _key);
        if(null == rankingEventRef)
        {
            CommLog.error("RankingEventMgr regLocalRankingEvent can't find rankingEventRef, _listenerType:{} _key:{}", _listenerType, _key);
            return -1;
        }

        //申请一个反注册序列号
        long unRegSerial = ALSerializeMaker.makeNewSerialize();

        //注册本地处理对象
        _regRanking(rankingEventRef, new RemoteEventListener(rankingEventRef, _usId, _dealSerialize, unRegSerial));

        return unRegSerial;
    }

    /**********************
     * 注销一个本地排行监听处理对象
     * @param _usId
     * @param _unRegSerial
     */
    public void unRegRankingEvent(ERankingEventListenerType _listenerType, long _key, int _usId, long _unRegSerial)
    {
        _ARefRankingEvent rankingEventRef = _ARefRankingEvent.getRefRankingEvent(_listenerType, _key);
        if(null == rankingEventRef)
        {
            CommLog.error("RankingEventMgr regLocalRankingEvent can't find rankingEventRef, _listenerType:{} _key:{}", _listenerType, _key);
            return;
        }

        _unRegRankingDealer(rankingEventRef, _usId, _unRegSerial);
    }

    /******************
     * 注册一个排行榜的消息处理事件
     * @param _eventRef
     * @param _eventListenerDealer
     */
    protected void _regRanking(_ARefRankingEvent _eventRef, _AEventListenerDealer _eventListenerDealer)
    {
        if(null == _eventRef)
            return ;

        //加锁，避免注册和注销同时进行，导致事件被错误删除
        _lock();

        try {
            //根据事件注册消息处理，并在响应事件中统一进行处理
            EventListenerObj listenerObj = _ensureEventListenerObj(_eventRef.getLogicEventId());
            if (null == listenerObj) {
                ALServerLog.Error("RankingEventMgr.regRanking: can't find listenerObj by id: " + _eventRef.getLogicEventId());
                return;
            }

            //这里要注意，各消息的处理应该根据不同服务器和不同消息各自处理
            listenerObj._regDealer(_eventListenerDealer);
        }
        finally {
            _unlock();
        }
    }

    /************
     * 注销一个排行榜的消息处理事件
     * @param _eventRef
     * @param _usId
     * @param _unRegSerial
     */
    protected void _unRegRankingDealer(_ARefRankingEvent _eventRef, int _usId, long _unRegSerial)
    {
        _lock();

        try {
            //根据事件注册消息处理，并在响应事件中统一进行处理
            EventListenerObj listenerObj = _lookupEventListenerObj(_eventRef.getLogicEventId());
            if (null == listenerObj) {
                ALServerLog.Error("RankingEventMgr.unRegRankingDealer: can't find listenerObj by id: " + _eventRef.getLogicEventId());
                return;
            }

            //这里要注意，各消息的处理应该根据不同服务器和不同消息各自处理
            listenerObj._unRegDealer(_usId, _unRegSerial);

            //判断对象是否为空，是则从数据集中删除
            if(listenerObj.isEmpty())
            {
                //删除对象
                EventListenerObj rmvObj = _m_htEventListenerTable.remove(_eventRef.getLogicEventId());
                //重复校验删除对象是否正确，不正确需要放回去
                if(rmvObj != listenerObj)
                {
                    _m_htEventListenerTable.put(_eventRef.getLogicEventId(), rmvObj);
                }
                else
                {
                    rmvObj._discard(this);
                }
            }
        }
        finally {
            _unlock();
        }
    }

    /***************
     * 注册一个远程事件监听对象
     * @param _eventId
     * @return
     */
    protected EventListenerObj _ensureEventListenerObj(int _eventId)
    {
        _lock();

        try {
            EventListenerObj obj = _m_htEventListenerTable.get(_eventId);
            if (null == obj) {
                obj = new EventListenerObj(_eventId);
                //调用初始化函数
                obj._init(this);

                _m_htEventListenerTable.put(_eventId, obj);
            }

            return obj;
        }
        finally {
            _unlock();
        }
    }

    /***************
     * 查询一个远程事件监听对象，不存在不会创建
     * @param _eventId
     * @return
     */
    protected EventListenerObj _lookupEventListenerObj(int _eventId)
    {
        _lock();

        try {
            return _m_htEventListenerTable.get(_eventId);
        }
        finally {
            _unlock();
        }
    }
}
