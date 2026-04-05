package NPGateServer;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/***************
 * 配置文件
 * @author Administrator
 *
 */
public class GateServerConf
{
    private static GateServerConf g_instance = new GateServerConf();

    public static GateServerConf getInstance()
    {
        return g_instance;
    }

    /**
     * 区域标记
     */
    private String _m_sAreaTag;
    /**
     * 服务器类型Id
     */
    private int _m_iGateTypeId;

    private int _m_iUserMaxCount;
    private int _m_iUserHandleWeight;

    /**
     * 外网客户端连接信息
     */
    private String _m_sConnectIp = "127.0.0.1";
    private int _m_iConnectPort = 5550;
    // 监听端口
    private int _m_iListenPort = 5550;
    private int _m_iClientSocketCacheSize = 102400;

    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public String getAreaTag()
    {
        return _m_sAreaTag;
    }

    public int getGateTypeId()
    {
        return _m_iGateTypeId;
    }

    public int getUserMaxCount()
    {
        return _m_iUserMaxCount;
    }

    public int getUserHandleWeight()
    {
        return _m_iUserHandleWeight;
    }

    public String getConnectIp()
    {
        return _m_sConnectIp;
    }

    public int getConnectPort()
    {
        return _m_iConnectPort;
    }

    public int getClientSocketCacheSize()
    {
        return _m_iClientSocketCacheSize;
    }

    public int getListenPort()
    {
        return _m_iListenPort;
    }

    /*******************
     * 初始化函数
     * @param _properties
     */
    public boolean init()
    {
        if (!_init("./conf/GateServerConf.properties"))
            return false;

        _init("./customConf/GateServerConf.properties");

        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
     * @param _properties
     */
    protected boolean _init(String _filePath)
    {
        Properties properties = new Properties();

        InputStream propertiesInputStream = null;

        try
        {
            propertiesInputStream = new FileInputStream(_filePath);
        } catch (Exception e)
        {
            e.printStackTrace();
        }

        if (null == propertiesInputStream)
        {
            return false;
        }

        //载入配置文件
        try
        {
            properties.load(propertiesInputStream);
        } catch (IOException e)
        {
            e.printStackTrace();
        }

        try
        {
            System.out.println("[Conf init] Start load GateServer Properties ... ...");

            //区域标记
            _m_sAreaTag
                    = ALConfReader.readStr(properties, "GateServer.AreaTag", _m_sAreaTag);
            //服务器类型Id
            _m_iGateTypeId
                    = ALConfReader.readInt(properties, "GateServer.TypeId", _m_iGateTypeId);

            //可以处理的总用户数量
            _m_iUserMaxCount
                    = ALConfReader.readInt(properties, "GateServer.UserMaxCount", _m_iUserMaxCount);
            //单个用户的权重配值
            _m_iUserHandleWeight
                    = ALConfReader.readInt(properties, "GateServer.UserHandleWeight", _m_iUserHandleWeight);

            //连接的平台服务器信息
            _m_sConnectIp
                    = ALConfReader.readStr(properties, "GateServer.ConnectIp", _m_sConnectIp);
            _m_iConnectPort
                    = ALConfReader.readInt(properties, "GateServer.ConnectPort", _m_iConnectPort);
            //监听端口
            _m_iListenPort
                    = ALConfReader.readInt(properties, "GateServer.ListenPort", _m_iListenPort);
            _m_iClientSocketCacheSize
                    = ALConfReader.readInt(properties, "GateServer.ClientSocketCacheSize", _m_iClientSocketCacheSize);

            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "GateServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load GateServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load GateServer Properties Error!!");
            e.printStackTrace();
        } finally
        {
            try
            {
                propertiesInputStream.close();
            } catch (IOException e)
            {
                e.printStackTrace();
            }
        }

        return true;
    }
}
