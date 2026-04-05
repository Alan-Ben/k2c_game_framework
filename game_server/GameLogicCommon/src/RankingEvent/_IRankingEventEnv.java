package RankingEvent;

import NPCommon.Util.Pair.WCGPair;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs._ARefRankingEvent;
import WCGBasicServer._AWCGBasicServer;

/************
 * 排行榜相关环境的接口对象
 * 如果要开启排行榜相关监听机制，需要实现本环境接口
 */
public interface _IRankingEventEnv
{
    /************
     * 处理本排行事件的服务器对象
     * @return
     */
    _AWCGBasicServer getBasicServerObj();

    /*****************
     * 在管理器初始化的时候会调用的接口函数
     * @param _mgr
     * @return
     */
    boolean onEventMgrInit(RankingEventMgr _mgr);

    /****************
     * 根据排行榜的数据，计算某次事件在排行榜上会引发的变化值
     * @param _rankEventRef
     * @param _cid
     * @param _eventBase
     * @return
     */
    WCGPair<Boolean, Long> calcRankCountChg(_ARefRankingEvent _rankEventRef, long _cid, _ALogicEventBase _eventBase);

    /*************
     * 在具体的服务器中注册事件触发处理对象
     * 注意注册的处理对象需要调用_triggerDealer对象的onEventTrigger函数
     * 注册后返回注销的对象，用于注销时使用
     * @param _triggerDealer
     */
    Object regEventTrigger(int _eventId, _IEventListenerTriggerDealer _triggerDealer);

    /*************
     * 在具体的服务器中注销事件触发处理对象
     * @param _regObj
     */
    void unregEventTrigger(int _eventId, Object _regObj);

    /**
     * 本地事件触发处理
     * @param _dealSerialize
     * @param _cid
     * @param _scoreSourceId
     * @param _chgCount
     */
    void onLocalEventTrigger(long _dealSerialize, long _cid, long _scoreSourceId, long _chgCount);

    /**
     * 计算排行榜的分数来源
     * @param _rankEventRef
     * @param _eventBase
     * @return
     */
    long calcRankCountSourceId(_ARefRankingEvent _rankEventRef, _ALogicEventBase _eventBase);
}
