package RankingEvent.Listener;

import NPGameRes.Refs._ARefRankingEvent;
import RankingEvent._IRankingEventEnv;

/*******
 * 事件监听后的处理对象
 * 一般本地和远程的会区分为不同的处理逻辑
 *
 * 每个对象都需要返回指定的标记用于注册和删除相关的处理
 */
public abstract class _AEventListenerDealer
{
    //排行榜数据对象，这里为了子类可以自行处理相关的计算，需要将数据存储在这里
    private _ARefRankingEvent _m_ref;
    public _AEventListenerDealer(_ARefRankingEvent _ref)
    {
        _m_ref = _ref;
    }

    public _ARefRankingEvent getEventRef() { return _m_ref; }

    /************
     * 获取对应注册的US服务器Id
     * @return
     */
    public abstract int getUSID();

    /************
     * 获取对应注册的事件处理序列号
     * @return
     */
    public abstract long getDealSerialize();

    /************
     * 获取对应注册的事件反注册序列号
     * @return
     */
    public abstract long getUnRegSerialize();

    /************
     * 事件触发时的触发函数，将会需要将数据分发给对应的接收对象
     * @param _cid
     * @param _chgCount
     */
    public abstract void onEventTrigger(_IRankingEventEnv _env, long _cid, long _scoreSourceId, long _chgCount);
}
