package NPMonitorServer.TestCallback;

import ALBasicProtocolPack._IALProtocolStructure;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGMonitor2BS.p001_R_MonitorOp.WCGMonitor2BS_RB_001_001_GetServerInfo;

/*********************
 * 获取所有服务器信息列表的回调处理
 * @author mj
 *
 */
public class TestGetServerInfoCallback implements _IWCGCallbackDealer
{
    //对应请求的信息
    private int _m_iServerType;
    private int _m_iServerTypeId;

    public TestGetServerInfoCallback(int _serverType, int _serverTypeId)
    {
        _m_iServerType = _serverType;
        _m_iServerTypeId = _serverTypeId;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new WCGMonitor2BS_RB_001_001_GetServerInfo();
    }

    @Override
    public void dealSuc(_IALProtocolStructure _retProtocol)
    {
        WCGMonitor2BS_RB_001_001_GetServerInfo serverInfo = (WCGMonitor2BS_RB_001_001_GetServerInfo) _retProtocol;
        if (null == serverInfo)
        {
            return;
        }

        System.out.println("server - type:" + _m_iServerType + " - typeId:" + _m_iServerTypeId + " info - " + serverInfo.getServerInfo());
    }

    @Override
    public void dealFail(int _errCode)
    {
        //返回错误信息给请求对象
        System.out.println("get server info fail - type:" + _m_iServerType + " - typeId:" + _m_iServerTypeId + " error: " + _errCode);
    }

}
