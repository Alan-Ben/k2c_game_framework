package NPUSServer.UserOfflineTmpDataMgr;

import NPUSServer.NPUserServer;

/**
 * 使用到Offline数据查询的统一使用接口
 */
public class UsOfflineDataFunc
{
    private UsOfflineTmpDataCore _m_dataCore;

    protected UsOfflineDataFunc(UsOfflineTmpDataCore _dataCore)
    {
        _m_dataCore = _dataCore;
    }

    public NPUserServer getUSServer() {return _m_dataCore.getUSServer();}
}
