package ShareCodeCenter.Conf.Server;

import WCGBasicServer.WCGBasicServerConf;

public class ShareCodeServerConf extends WCGBasicServerConf
{
    //对应的Id
    private int _m_iId;

    public ShareCodeServerConf()
    {
        _m_iId = 0;
    }

    public ShareCodeServerConf(int _id, int _clientRecBufferLen, int _port, String _platServerIp, int _platServerPort
            , String _platServerPassword, String _internalIp, int _internalPort)
    {
        super(_clientRecBufferLen, _port, _platServerIp, _platServerPort
                , _platServerPassword, _internalIp, _internalPort);

        _m_iId = _id;
    }

    public int getId()
    {
        return _m_iId;
    }
}
