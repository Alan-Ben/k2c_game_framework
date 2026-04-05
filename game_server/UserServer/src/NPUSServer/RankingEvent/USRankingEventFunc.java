package NPUSServer.RankingEvent;

import NPCommon.ErrMain.CommErr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ERankingEventServerType;
import NPGameRes.Refs._ARefRankingEvent;
import NPUSServer.NPUserServer;
import NPUSServer.RankingEvent.Dealer.ToUsRankingEventDealer;

/**
 * 排行榜事件功能类
 * 实现排行榜事件的注册和注销逻辑
 * 如果有新的目标服务器需要实现_IToServerRankingEventDealer接口，并注册到该功能类上
 */
public class USRankingEventFunc
{
    private _IToServerRankingEventDealer[] _m_dealerList;

    public USRankingEventFunc(NPUserServer _server)
    {
        //初始化处理器列表
        _m_dealerList = new _IToServerRankingEventDealer[ERankingEventServerType.ERankingEventServerType_Length];

        //注册相关处理器
        _regDealer(new ToUsRankingEventDealer(_server));
    }

    /**
     * 注册处理器对象
     * @param _dealer 处理器对象
     */
    private void _regDealer(_IToServerRankingEventDealer _dealer)
    {
        if (null == _dealer)
            return;

        _m_dealerList[_dealer.getServerType().ordinal()] = _dealer;
    }

    /**
     * 获取目标服务器的处理器对象
     * @param _serverType 服务器类型
     * @return 处理器对象
     */
    public _IToServerRankingEventDealer getTargetServerDealer(ERankingEventServerType _serverType)
    {
        return _m_dealerList[_serverType.ordinal()];
    }

    /**
     * 注册排行榜事件监听
     * @param _rankingEventRef 排行榜事件配置
     * @param _serial          注册序列号,由发起注册的对象生成
     * @param _callback        注册结果回调 (注册结果,注销序列号)
     */
    public void registerRankingEvent(_ARefRankingEvent _rankingEventRef, long _serial, _ICallBackResultT<Long> _callback)
    {
        if (_rankingEventRef == null)
        {
            _callback.onRunOver(CommErr.REF_NOT_FOUND, 0L);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingEventDealer dealer = getTargetServerDealer(_rankingEventRef.getTriggerServerType());
        if (null == dealer)
        {
            CommLog.error("USRankingEventFunc registerRankingEvent getTargetServerDealer is null, triggerServerType:{}", _rankingEventRef.getTriggerServerType());
            _callback.onRunOver(CommErr.SYS_ERR, 0L);
            return;
        }

        //发起注册
        dealer.dealRegOp(_rankingEventRef.getListenerType(), _rankingEventRef.Id(), _serial, _callback);
    }

    /**
     * 注销排行榜事件监听
     * @param _rankingEventRef 排行榜事件配置
     * @param _callback        注册结果回调
     */
    public void unregisterRankingEvent(_ARefRankingEvent _rankingEventRef, long _unRegSerial, _ICallBackResult _callback)
    {
        if (_rankingEventRef == null)
        {
            _callback.onRunOver(CommErr.REF_NOT_FOUND);
            return;
        }

        //获取目标服务器的处理器对象
        _IToServerRankingEventDealer dealer = getTargetServerDealer(_rankingEventRef.getTriggerServerType());
        if (null == dealer)
        {
            CommLog.error("USRankingEventFunc unregisterRankingEvent getTargetServerDealer is null, triggerServerType:{}", _rankingEventRef.getTriggerServerType());
            _callback.onRunOver(CommErr.SYS_ERR);
            return;
        }

        //发起注销
        dealer.dealUnRegOp(_rankingEventRef.getListenerType(), _rankingEventRef.Id(), _unRegSerial, _callback);
    }
}
