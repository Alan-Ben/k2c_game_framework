package NPHttpServer.Http.HttpService.MsgDispather;

import NPCommon.Log.CommLog;
import NPHttpServer.Http.HttpService.MsgDispather.Http_001_Op.NP2HS_001_PlatFromMainDealer;
import NPHttpServer.Http.HttpService.MsgDispather.Http_002_Op.NP2HS_002_PlatFromMainDealer;
import NPHttpServer.Http.HttpService.MsgDispather.Http_003_Op.NP2HS_003_PlatFromMainDealer;
import NPHttpServer.Http.HttpService.MsgDispather.Http_005_Op.NP2HS_005_PlatFromMainDealer;
import NPHttpServer.Http.HttpService.MsgDispather.Http_007_Op.NP2HS_007_PlatFromMainDealer;
import NPHttpServer.Http.HttpService.MsgDispather.Http_009_Op.NP2HS_009_PlatFromMainDealer;
import NPHttpServer.Http.HttpService.MsgDispather.Http_013_Op.NP2HS_013_PlatFromMainDealer;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * @description: http 请求处理分发器
 * @author: ricci
 * @date: 2023-03-24 16:42:43
 */
public class NPHttpDealerDispatcher
{
    /**
     * 消息处理分发器
     */
    private Map<Integer, _ANPPlatFormHttpMainDealer> _m_PlatFormHttpMainDealerMap;
    //////单例的//////
    private static final NPHttpDealerDispatcher _s_instance = new NPHttpDealerDispatcher();


    public static NPHttpDealerDispatcher getInstance()
    {
        return _s_instance;
    }

    private NPHttpDealerDispatcher()
    {
        _m_PlatFormHttpMainDealerMap = new ConcurrentHashMap<>();
        regist(new NP2HS_001_PlatFromMainDealer());
        regist(new NP2HS_002_PlatFromMainDealer());
        regist(new NP2HS_003_PlatFromMainDealer());
        regist(new NP2HS_005_PlatFromMainDealer());
        regist(new NP2HS_007_PlatFromMainDealer());
        regist(new NP2HS_009_PlatFromMainDealer());
        regist(new NP2HS_013_PlatFromMainDealer());
    }

    /**
     * 注册
     * @param _mainDealer _ANPPlatFormHttpMainDealer
     */
    public void regist(_ANPPlatFormHttpMainDealer _mainDealer)
    {
        _m_PlatFormHttpMainDealerMap.put(_mainDealer.mainOrder(), _mainDealer);
    }

    /**
     * 通过主次协议号找到消息处理对象
     * @param _mainOrder 主协议号
     * @param _subOrder  次协议号
     * @return _ANPPlatFormHttpSubDealer<?>
     */
    public _ANPPlatFormHttpSubDealer<?> lookupDealer(int _mainOrder, int _subOrder)
    {
        //查找主处理器
        _ANPPlatFormHttpMainDealer mainDealer = _m_PlatFormHttpMainDealerMap.get(_mainOrder);
        if (mainDealer == null)
        {
            CommLog.error("NPHttpDealerDispatcher cant find mainDealer {}-{}", _mainOrder, _subOrder);
            return null;
        }
        //查找实际处理器
        _ANPPlatFormHttpSubDealer<?> subDealer = mainDealer.lookup(_subOrder);
        if (subDealer == null)
        {
            CommLog.error("NPHttpDealerDispatcher cant find subDealer {}-{}", _mainOrder, _subOrder);
            return null;
        }
        return subDealer;
    }
}
