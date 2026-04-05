package NPHttpServer.Http.HttpService.MsgDispather;

import NPCommon.Log.CommLog;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * @description: 主协议处理器
 * @author: ricci
 * @date: 2023-03-24 16:47:11
 */
public abstract class _ANPPlatFormHttpMainDealer
{
    private final Map<Integer, _ANPPlatFormHttpSubDealer<?>> _m_subDealerMap;

    public _ANPPlatFormHttpMainDealer()
    {
        _m_subDealerMap = new ConcurrentHashMap<>();
    }

    /**
     * 注册处理器
     * @param _dealer 处理器
     */
    public void regist(_ANPPlatFormHttpSubDealer<?> _dealer)
    {
        if (_dealer == null)
        {
            return;
        }
        if (_m_subDealerMap.containsKey(_dealer.subOrder()))
        {
            //报告重复注册，但是仍然使用新注册的覆盖旧注册的
            CommLog.error("_ANPPlatFormHttpMainDealer {}-{} duplicate regist", mainOrder(), _dealer.subOrder());
        }
        _m_subDealerMap.put(_dealer.subOrder(), _dealer);
    }

    /**
     * 查找消息处理器
     * @param _subOrder 子协议号
     * @return _ANPPlatFormHttpSubDealer
     */
    public _ANPPlatFormHttpSubDealer<?> lookup(int _subOrder)
    {
        return _m_subDealerMap.get(_subOrder);
    }

    /**
     * 主协议号
     * @return int
     */
    public abstract int mainOrder();


}
