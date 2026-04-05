package NPGateServer.USRefVersionMgr;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NPCommon.Log.CommLog;

/**************
 * US服务器版本信息对象
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2020年12月9日 下午10:32:28
 */
public class USVersionInfo implements _IALProtocolReceiver
{
    //usId
    private final int _m_iUSTypeId;
    //服务器版本
    private String _m_sServerVersion;
    //服务器资源版本
    private String _m_sResVersion;

    public USVersionInfo(int _usTypeId)
    {
        _m_iUSTypeId = _usTypeId;
    }

    public int getUSTypeId()
    {
        return _m_iUSTypeId;
    }

    public String getServerVersion()
    {
        return _m_sServerVersion;
    }

    public String getResVersion()
    {
        return _m_sResVersion;
    }

    /************
     * 设置版本
     * @param _serverVersion 服务器版本
     * @param _resVersion 服务器资源版本
     */
    public void updateServerInfo(String _serverVersion, String _resVersion)
    {
        if (_serverVersion.equals(_m_sServerVersion) && _resVersion.equals(_m_sResVersion))
            return;

        _m_sServerVersion = _serverVersion;
        _m_sResVersion = _resVersion;

        CommLog.info("USVersionInfo serverInfo update usId:{} serverVersion:{} resVersion:{}", _m_iUSTypeId, _m_sServerVersion, _m_sResVersion);
    }
}
