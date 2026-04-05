package NPUSServer.RankingEvent;

import NPCommon.Util.CallBack._ICallBackResult;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ERankingEventListenerType;
import NPEnum.ERankingEventServerType;

/**
 * 该功能类是用于事件监听的初始化工具类
 */
public interface _IToServerRankingEventDealer
{
    /**
     * 获取该功能类所针对的服务器类型
     * @return 服务器类型
     */
    ERankingEventServerType getServerType();

    /**
     * 开始发起注册
     * @param _listenerType 监听者类型
     * @param _key          监听者主键
     * @param _dealSerial   注册操作序列号
     * @param _callback     注册结果回调 (注册结果,注销序列号)
     */
    void dealRegOp(ERankingEventListenerType _listenerType, long _key, long _dealSerial, _ICallBackResultT<Long> _callback);

    /**
     * 开始发起取消注册
     * @param _listenerType 监听者类型
     * @param _key          监听者主键
     * @param _unRegSerial  注销序列号
     */
    void dealUnRegOp(ERankingEventListenerType _listenerType, long _key, long _unRegSerial, _ICallBackResult _callback);
}
