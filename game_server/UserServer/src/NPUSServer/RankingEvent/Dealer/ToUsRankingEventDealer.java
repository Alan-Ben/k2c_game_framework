package NPUSServer.RankingEvent.Dealer;

import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ERankingEventListenerType;
import NPEnum.ERankingEventServerType;
import NPUSServer.NPUserServer;
import NPUSServer.RankingEvent._IToServerRankingEventDealer;

public class ToUsRankingEventDealer implements _IToServerRankingEventDealer
{
    private NPUserServer _m_server;

    public ToUsRankingEventDealer(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    @Override
    public ERankingEventServerType getServerType()
    {
        return ERankingEventServerType.USER;
    }

    @Override
    public void dealRegOp(ERankingEventListenerType _listenerType, long _key,  long _dealSerial, _ICallBackResultT<Long> _callback)
    {
        long unRegSerial = getUSServer().getRankingEventMgr().regLocalRankingEvent(_listenerType, _key, getUSServer().getServerTypeId(), _dealSerial);
        _callback.onRunOver(Result.SUCC, unRegSerial);
    }

    @Override
    public void dealUnRegOp(ERankingEventListenerType _listenerType, long _key,  long _unRegSerial, _ICallBackResult _callback)
    {
        getUSServer().getRankingEventMgr().unRegRankingEvent(_listenerType, _key, getUSServer().getServerTypeId(), _unRegSerial);
        _callback.onRunOver(Result.SUCC);
    }
}
