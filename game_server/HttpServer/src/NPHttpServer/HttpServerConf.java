package NPHttpServer;

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
public class HttpServerConf
{
    private static final HttpServerConf g_instance = new HttpServerConf();
    //平台提供的大区id，通用请求的一部分
    private int _m_platAreaId;
    //平台提供的平台id，通用请求的一部分
    private int _m_platformId;
    //平台提供的请求地址
    private String _m_platUrl;
    //AI平台提供的请求地址
    private String _m_aiPlatUrl;
    //AI平台的回调请求地址
    private String _m_aiPlatCallbackUrl;
    //对平台开放服务端http端口
    private int _m_toPlatHttpPort;
    //平台提供的请求key用来生成签名
    private String _m_platKey;
    //平台推送的验证
    private String _m_platFromClientUser;
    private String _m_platFromClientPassword;
    private String _m_platFromClientKey;

    private String _m_sTimeZone = "GMT+8:00";

    /**
     * 外网客户端连接信息
     */
    private int _m_iClientConnectPort = 9202;
    private int _m_iClientSocketCacheSize = 10240;

    private boolean _m_bOpenDDAlert = false;

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public static HttpServerConf getInstance()
    {
        return g_instance;
    }

    public int getPlatAreaId()
    {
        return _m_platAreaId;
    }

    public int getPlatformId()
    {
        return _m_platformId;
    }

    /******
     * 返回大区标识
     * @return
     */
    public int getRegionId()
    {
        return _m_platformId * 100 + _m_platAreaId;
    }

    public String getPlatUrl()
    {
        return _m_platUrl;
    }

    public String getAiPlatUrl()
    {
        return _m_aiPlatUrl;
    }

    public String getAiPlatCallbackUrl()
    {
        return _m_aiPlatCallbackUrl;
    }

    public int getToPlatHttpPort()
    {
        return _m_toPlatHttpPort;
    }

    public String getPlatKey()
    {
        return _m_platKey;
    }

    public String getPlatFromClientUser()
    {
        return _m_platFromClientUser;
    }

    public String getPlatFromClientPassword()
    {
        return _m_platFromClientPassword;
    }

    public String getPlatFromClientKey()
    {
        return _m_platFromClientKey;
    }

    public int getClientConnectPort()
    {
        return _m_iClientConnectPort;
    }

    public int getClientSocketCacheSize()
    {
        return _m_iClientSocketCacheSize;
    }

    public boolean getOpenDDAlert()
    {
        return _m_bOpenDDAlert;
    }

    /*******************
     * 初始化函数
     */
    public boolean init()
    {
        if (!_init("./conf/HttpServerConf.properties"))
            return false;

        _init("./customConf/HttpServerConf.properties");

        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
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
            System.out.println("[Conf init] Start load HttpServer Properties ... ...");

            //区域标记

            _m_platAreaId = ALConfReader.readInt(properties, "HttpServer.PlatAreaId", _m_platAreaId);
            _m_platformId = ALConfReader.readInt(properties, "HttpServer.PlatformId", _m_platformId);
            _m_platUrl = ALConfReader.readStr(properties, "HttpServer.PlatUrl", _m_platUrl);
            _m_aiPlatUrl = ALConfReader.readStr(properties, "HttpServer.AiPlatUrl", _m_aiPlatUrl);
            _m_aiPlatCallbackUrl = ALConfReader.readStr(properties, "HttpServer.AiPlatCallbackUrl", _m_aiPlatCallbackUrl);
            _m_platKey = ALConfReader.readStr(properties, "HttpServer.PlatKey", _m_platKey);
            _m_toPlatHttpPort = ALConfReader.readInt(properties, "HttpServer.ToPlatHttpPort", _m_toPlatHttpPort);
            _m_platFromClientUser = ALConfReader.readStr(properties, "HttpServer.PlatFromClientUser", _m_platFromClientUser);
            _m_platFromClientPassword = ALConfReader.readStr(properties, "HttpServer.PlatFromClientPassword", _m_platFromClientPassword);
            _m_platFromClientKey = ALConfReader.readStr(properties, "HttpServer.PlatFromClientKey", _m_platFromClientKey);
            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "HttpServer.TimeZone", _m_sTimeZone);

            //外网客户端连接信息
            _m_iClientConnectPort = ALConfReader.readInt(properties, "HttpServer.ClientConnectPort", _m_iClientConnectPort);
            _m_iClientSocketCacheSize = ALConfReader.readInt(properties, "HttpServer.ClientSocketCacheSize", _m_iClientSocketCacheSize);

            _m_bOpenDDAlert = ALConfReader.readBool(properties, "HttpServer.OpenDDAlert", _m_bOpenDDAlert);

            System.out.println("[Conf init] Finish load HttpServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load HttpServer Properties Error!!");
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
