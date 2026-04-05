package RankingEvent;


import ALBasicServer.ALBasicMutex.MutexAtom;
import ALServerLog.ALServerLog;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPair;
import NPGameRes.LogicEvent._ALogicEventBase;
import RankingEvent.Listener._AEventListenerDealer;

import java.util.ArrayList;
import java.util.List;

/***************
 * 单个事件的消息监听对象
 * 一个事件在eventMgr中只会有一个监听对象，不同服务器的监听都会在这个对象中进行管理
 */
public class EventListenerObj implements _IEventListenerTriggerDealer
{
    //监听的事件id
    private int _m_eventId;

    //事件注册触发返回的对象，用于注销的时候使用
    private Object _m_eventTriggerRegObj;

    //监听事件的对象（目前暂定只有US监听）
    private List<_AEventListenerDealer> _m_lEventDealerList;
    //注册队列的锁对象，队列这边使用替换方式，保证每次处理的时候不会因为锁而导致互相互斥
    private MutexAtom _m_mutex;

    public EventListenerObj(int eventId)
    {
        _m_eventId = eventId;
        _m_eventTriggerRegObj = null;

        _m_lEventDealerList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock() { _m_mutex.lock(); }
    protected void _unlock() { _m_mutex.unlock(); }

    public int getEventId()
    {
        return _m_eventId;
    }

    /************
     * 判断本对象是否为空
     * @return
     */
    public boolean isEmpty()
    {
        _lock();

        try {
            return _m_lEventDealerList.isEmpty();
        } finally {
            _unlock();
        }
    }


    /**********
     * 处理事件的逻辑函数，会将事件分发到各个监听器中
     * @param _cid
     * @param _eventBase 事件的数据内容对象
     */
    public void onEventTrigger(long _cid, _IRankingEventEnv _env, _ALogicEventBase _eventBase)
    {
        if(null == _eventBase)
            return ;

        //先拷贝对象，避免在运行过程被更改
        _lock();
        List<_AEventListenerDealer> dealerList = null;
        try {
            dealerList = _m_lEventDealerList;
        } finally {
            _unlock();
        }

        //计算cid所在us
        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);

        //将消息分发到各监听对象中
        _AEventListenerDealer dealer = null;
        for(int i = 0; i < dealerList.size(); i++)
        {
            dealer = dealerList.get(i);
            if(null == dealer)
                continue;

            //跳过不是本us的处理对象
            if (usId != dealer.getUSID())
                continue;

            //调用通用接口计算分数来源
            long scoreSourceId = _env.calcRankCountSourceId(dealer.getEventRef(), _eventBase);

            //调用通用接口计算变化值
            WCGPair<Boolean, Long> chgCount = _env.calcRankCountChg(dealer.getEventRef(), _cid, _eventBase);
            if (!chgCount.first)
                continue;

            //调用不同处理对象的触发处理
            dealer.onEventTrigger(_env, _cid, scoreSourceId, chgCount.second);
        }
    }

    /************
     * 初始化函数
     */
    protected void _init(RankingEventMgr _mgr)
    {
        //注册事件处理对象,在注册的处理对象中需要调用onEventTrigger函数确保事件正确统计
        if(!_mgr.checkInit())
            return ;

        //注册处理对象
        _m_eventTriggerRegObj = _mgr.getEnv().regEventTrigger(getEventId(), this);
    }

    /************
     * 释放相关资源
     */
    protected void _discard(RankingEventMgr _mgr)
    {
        //注册事件处理对象,在注册的处理对象中需要调用onEventTrigger函数确保事件正确统计
        if(_mgr.checkInit())
        {
            //注销处理对象
            _mgr.getEnv().unregEventTrigger(getEventId(), _m_eventTriggerRegObj);
            //重置对象
            _m_eventTriggerRegObj = null;
        }
    }


    /**************
     * 注册事件的处理，统一注册到global event机制内
     */
    protected void _regDealer(_AEventListenerDealer _dealer)
    {
        //注册处理对象
        _addDealer(_dealer);
    }

    /*************
     * 注销事件的处理，统一注册到global event机制内
     */
    protected void _unRegDealer(int _usId, long _unRegSerial)
    {
        _rmvDealer(_usId, _unRegSerial);
    }

    /****************
     * 查询对应usid以及处理序列号的处理对象
     * @param _usId
     * @param _dealSerialize
     * @return
     */
    protected _AEventListenerDealer _getDealer(int _usId, long _dealSerialize)
    {
        _lock();

        try {
            _AEventListenerDealer dealer = null;
            for (int i = 0; i < _m_lEventDealerList.size(); i++) {
                dealer = _m_lEventDealerList.get(i);
                if (null == dealer)
                    continue;

                if (dealer.getUSID() == _usId && dealer.getDealSerialize() == _dealSerialize)
                    return dealer;
            }

            return null;
        } finally {
            _unlock();
        }
    }

    /***************
     * 添加一个监听对象
     * @param _dealer
     */
    protected void _addDealer(_AEventListenerDealer _dealer)
    {
        //检查是否有重复注册
        _AEventListenerDealer preDealer = _getDealer(_dealer.getUSID(), _dealer.getDealSerialize());
        if(null != preDealer)
        {
            ALServerLog.Error("EventListenerObj::_regDealer, event id = " + _m_eventId + ", usid = " + _dealer.getUSID() + ", dealSerialize = " + _dealer.getDealSerialize() + " has been registed!");
            return ;
        }


        _lock();

        try {
            //拷贝队列，再添加
            List<_AEventListenerDealer> newList = new ArrayList<>(_m_lEventDealerList);
            newList.add(_dealer);

            _m_lEventDealerList = newList;
        } finally {
            _unlock();
        }
    }

    /**********
     * 移除一个监听对象
     * @param _usId
     * @param _unRegSerial
     */
    protected void _rmvDealer(int _usId, long _unRegSerial)
    {
        _lock();

        try {
            //拷贝队列，再循环查询删除
            List<_AEventListenerDealer> newList = new ArrayList<>(_m_lEventDealerList);
            _AEventListenerDealer dealer = null;
            for(int i = 0; i < newList.size(); i++)
            {
                dealer = newList.get(i);
                if(null == dealer)
                    continue;

                if(dealer.getUSID() == _usId && dealer.getUnRegSerialize() == _unRegSerial)
                {
                    newList.remove(i);
                    break;
                }
            }

            _m_lEventDealerList = newList;
        } finally {
            _unlock();
        }
    }
}
