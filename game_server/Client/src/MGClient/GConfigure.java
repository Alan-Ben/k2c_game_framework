package MGClient;


import NPCommon.Util.Delegate.ADelegateNone;

public class GConfigure
{
    private static GConfigure _instance = new GConfigure();

    public static GConfigure getInstance()
    {
        return _instance;
    }

    public ADelegateNone OnConfigChged = new ADelegateNone(this);

    public String getServerIP()
    {
        return _m_sServerIP;
    }

    public int getPort()
    {
        return _m_iPort;
    }

    public void setServerIP(String _m_sServerIP, int _iPort)
    {
        this._m_sServerIP = _m_sServerIP;
        this._m_iPort = _iPort;
        OnConfigChged.onEvent();
    }

    private String _m_sServerIP;
    private int _m_iPort;

    private GConfigure()
    {
        _m_sServerIP = "127.0.0.1";
        _m_iPort = 5101;
    }

}
