package NPCommonServer.CrossRankServerHandleMgr;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2CRS_RB.p001_CrossRankOp.NP2CRS_RB_001_007_RetCrossRankServerInfo;
import NPCommon.Log.CommLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;

public class CrossRankServerInitCallbackDealer implements _IWCGCallbackDealer
{
    //CRS服务器类型ID
    private CrossRankServerInfo _m_serverInfo;

    public CrossRankServerInitCallbackDealer(CrossRankServerInfo _serverInfo)
    {
        _m_serverInfo = _serverInfo;
    }

    @Override
    public _IALProtocolStructure createProtocolObj()
    {
        return new NP2CRS_RB_001_007_RetCrossRankServerInfo();
    }

    @Override
    public void dealFail(int _errCode)
    {
        CommLog.error("Request Cross Rank Server info fail! - " + _errCode);
    }

    @Override
    public void dealSuc(_IALProtocolStructure _msg)
    {
        NP2CRS_RB_001_007_RetCrossRankServerInfo protocol = (NP2CRS_RB_001_007_RetCrossRankServerInfo) _msg;
        if (null == protocol)
            return;

        _m_serverInfo.initServerInfo(protocol.getHadHandleInstanceNum(), protocol.getHandleInstanceLimit());
    }
}
