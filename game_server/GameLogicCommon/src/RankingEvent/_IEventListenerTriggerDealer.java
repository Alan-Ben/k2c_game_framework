package RankingEvent;

import NPGameRes.LogicEvent._ALogicEventBase;

/*******
 * 触发事件的处理函数接口对象
 */
public interface _IEventListenerTriggerDealer
{
    /************
     * 事件触发时的触发函数，将会需要将数据分发给对应的接收对象
     * @param _cid
     * @param _eventBase 事件的数据内容对象
     */
    void onEventTrigger(long _cid, _IRankingEventEnv _env, _ALogicEventBase _eventBase);
}
