package RankingEvent.Listener;


import ALServerLog.ALServerLog;
import NPGameRes.Refs._ARefRankingEvent;
import RankingEvent._IRankingEventEnv;

/*******
 * 事件监听后的处理对象
 * 本类型是本地的处理对象，获取事件的时候直接调用本地的事件函数处理
 */
public class LocalEventListener extends _AEventListenerDealer
{
    private int _m_iUSId;
    private long _m_lDealSerialize;
    private long _m_lUnRegSerialize;

    public LocalEventListener(_ARefRankingEvent _eventRef, int _usId, long _dealSerialize, long _unRegSerial)
    {
        super(_eventRef);

        _m_iUSId = _usId;
        _m_lDealSerialize = _dealSerialize;
        _m_lUnRegSerialize = _unRegSerial;
    }

    /************
     * 获取对应注册的US服务器Id
     * @return
     */
    public int getUSID()
    {
        return _m_iUSId;
    }
    /************
     * 获取对应注册的事件处理序列号
     * @return
     */
    public long getDealSerialize()
    {
        return _m_lDealSerialize;
    }

    /************
     * 获取对应注册的事件反注册序列号
     * @return
     */
    @Override
    public long getUnRegSerialize()
    {
        return _m_lUnRegSerialize;
    }

    /************
     * 事件触发时的触发函数，将会需要将数据分发给对应的接收对象
     * @param _cid
     * @param _chgCount
     */
    public void onEventTrigger(_IRankingEventEnv _env, long _cid, long _scoreSourceId, long _chgCount)
    {
        if(null == _env)
        {
            ALServerLog.Fatal("LocalEventListener.onEventTrigger Local env is null");
            return ;
        }

        //此处直接触发本地排行榜变化
        _env.onLocalEventTrigger(getDealSerialize(), _cid, _scoreSourceId, _chgCount);
    }
}
