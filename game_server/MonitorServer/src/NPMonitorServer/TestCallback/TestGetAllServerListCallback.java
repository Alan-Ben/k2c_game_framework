package NPMonitorServer.TestCallback;

import ALBasicProtocolPack._IALProtocolStructure;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGMonitor2Plat.p000_R_BasicOp.WCGMonitor2Plat_RB_000_001_RetAllServerList;
import WCGMonitor2Plat.p000_R_BasicOp.WCGMonitor2Plat_RB_000_001_ServerInfo;

/*********************
 * 获取所有服务器信息列表的回调处理
 * @author mj
 *
 */
public class TestGetAllServerListCallback implements _IWCGCallbackDealer
{
    public TestGetAllServerListCallback()
    {
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new WCGMonitor2Plat_RB_000_001_RetAllServerList();
    }

    @Override
    public void dealSuc(_IALProtocolStructure _retProtocol)
    {
        WCGMonitor2Plat_RB_000_001_RetAllServerList serverList = (WCGMonitor2Plat_RB_000_001_RetAllServerList) _retProtocol;
        if (null == serverList)
        {
            return;
        }

        WCGMonitor2Plat_RB_000_001_ServerInfo tmpInfo;

        System.out.println("get [" + serverList.getServerList().size() + "] servers");

        for (int i = 0; i < serverList.getServerList().size(); i++)
        {
            tmpInfo = serverList.getServerList().get(i);
            if (null == tmpInfo)
                continue;

            System.out.println("server - type:" + tmpInfo.getServerType() + " - typeId:" + tmpInfo.getServerTypeId());
        }
    }

    @Override
    public void dealFail(int _errCode)
    {
        System.out.print("get server list fail - " + _errCode);
    }

}
