package PayCenter.Conf.Server;

import WCGBasicServer.WCGBasicServerConf;

/**
 * PayServerConf - PayServer配置类
 * 
 * 主要功能：
 * 1. 继承WCG基础服务器配置
 * 2. 存储PayServer实例的连接配置信息
 * 3. 提供配置参数的访问接口
 * 
 * 设计特点：
 * - 基于WCG基础配置框架
 * - 封装服务器连接参数
 * - 支持多实例配置管理
 */
public class PayServerConf extends WCGBasicServerConf
{
    // 对应的Id
    private int _m_iId;
    // 平台Id
    private int _m_iPlatformId = -1;
    // 大区Id
    private int _m_iPlatAreaId = -1;

    public PayServerConf()
    {
        _m_iId = 0;
    }

    public PayServerConf(int _id, int _clientRecBufferLen, int _port, String _platServerIp, int _platServerPort, String _platServerPassword, String _internalIp, int _internalPort)
    {
        super(_clientRecBufferLen, _port, _platServerIp, _platServerPort, _platServerPassword, _internalIp, _internalPort);

        _m_iId = _id;
    }

    public int getId()
    {
        return _m_iId;
    }

    public void setPlatformId(int _platformId)
    {
        _m_iPlatformId = _platformId;
    }

    public void setPlatAreaId(int _platAreaId)
    {
        _m_iPlatAreaId = _platAreaId;
    }

    public int getPlatformId()
    {
        return _m_iPlatformId;
    }

    public int getPlatAreaId()
    {
        return _m_iPlatAreaId;
    }
}