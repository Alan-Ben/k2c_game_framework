package NPUSServer.RankingEvent;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import NPCommon.Log.CommLog;

import java.util.HashMap;
import java.util.Map;

/**
 * 排行榜事件功能类
 * 实现排行榜事件的注册和注销逻辑
 * 如果有新的目标服务器需要实现_IToServerRankingEventDealer接口，并注册到该功能类上
 */
public class USRankingEventRecordCallbackMgr
{
    private static USRankingEventRecordCallbackMgr _g_instance = new USRankingEventRecordCallbackMgr();
    public static USRankingEventRecordCallbackMgr getInstance()
    {
        return _g_instance;
    }

    //序列号和排行榜信息的映射
    private Map<Long, _IRankingEventHolder> _m_serialRankMap;
    //map锁
    private MutexAtom _m_mapLock;

    /**
     * 静态初始化
     * 用于注册实现的针对不同目标服务器的处理器
     */
    protected USRankingEventRecordCallbackMgr()
    {
        //初始化序列号和排行榜信息的映射
        _m_serialRankMap = new HashMap<>();
        //初始化map锁
        _m_mapLock = new MutexAtom();
    }

    /**
     * 注册排行榜事件监听
     */
    public long regRankInfoCallback(_IRankingEventHolder _rankInfo)
    {
        long serialize = ALSerializeMaker.makeNewSerialize();

        _m_mapLock.lock();
        try
        {
            _m_serialRankMap.put(serialize, _rankInfo);
        } finally
        {
            _m_mapLock.unlock();
        }

        return serialize;
    }

    /**
     * 注销排行榜事件监听
     */
    public void unregisterRankingEvent(long _serial)
    {
        _m_mapLock.lock();
        try
        {
            _m_serialRankMap.remove(_serial);
        } finally
        {
            _m_mapLock.unlock();
        }
    }


    /**
     * 排行榜事件触发
     * @param _serial 注册序列号,由发起注册的对象生成
     */
    public void onRankingEvent(long _serial, long _cid, long _scoreSourceId, long _score)
    {
        _IRankingEventHolder rankInfo = null;

        _m_mapLock.lock();
        try
        {
            rankInfo = _m_serialRankMap.get(_serial);
        } finally
        {
            _m_mapLock.unlock();
        }

        if (null == rankInfo)
        {
            CommLog.error("USRankingEventFunc onRankingEvent rankInfo is null, serial:{} cid:{} score:{}", _serial, _cid, _score);
            return;
        }

        //调用排行榜分数变更处理
        rankInfo.onScoreChange(_cid, _scoreSourceId, _score);
    }
}
