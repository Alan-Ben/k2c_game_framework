package RankingEvent.Listener;


import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPGameRes.Refs._ARefRankingEvent;
import RankingEvent._IRankingEventEnv;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_007_ReqRankingEventTrigger;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_007_RetRankingEventTrigger;
import WCGCommon.Enum.NPEnum;

/***********
 * 远程事件的处理监听对象，这里将会把触发的事件内容通过消息发送给对应的服务器
 */
public class RemoteEventListener extends _AEventListenerDealer
{
    private int _m_iUSId;
    private long _m_lDealSerialize;
    private long _m_lUnRegSerialize;

    public RemoteEventListener(_ARefRankingEvent _eventRef, int _usId, long _dealSerialize, long _unRegSerial)
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
            ALServerLog.Fatal("LocalEventListener.onEventTrigger Remote env is null");
            return ;
        }

        //此处需要发送消息给对应US
        _env.getBasicServerObj().sendRequestToBSServer(NPEnum.EServerType.USER.ordinal(), getUSID(),
                new NP2US_R_003_007_ReqRankingEventTrigger(getDealSerialize(), _cid, _scoreSourceId, _chgCount),
                new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_003_007_RetRankingEventTrigger();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retMsg)
                    {

                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        CommLog.error("RemoteEventListener onEventTrigger dealFail errCode:{}, listenerType:{} key:{} usId:{} cid:{} chgCount:{} ",
                                _errCode, getEventRef().getListenerType(), getEventRef().Id(), getUSID(), _cid, _chgCount);
                    }
                });
    }
}