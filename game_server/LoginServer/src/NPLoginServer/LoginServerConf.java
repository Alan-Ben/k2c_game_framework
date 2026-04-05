package NPLoginServer;

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
public class LoginServerConf
{
    private static LoginServerConf g_instance = new LoginServerConf();

    public static LoginServerConf getInstance()
    {

        return g_instance;
    }

    /**
     * 是否允许作弊登录
     */
    private boolean _m_bCheatEnterEnable;

    /**
     * 区域标记
     */
    private String _m_sAreaTag;
    /**
     * 服务器类型Id
     */
    private int _m_iLoginTypeId;

    /**
     * 外网客户端连接信息
     */
    private int _m_iConnectPort = 5550;
    private int _m_iClientSocketCacheSize = 102400;

    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public boolean getCheatEnterEnable()
    {
        return _m_bCheatEnterEnable;
    }

    public String getAreaTag()
    {
        return _m_sAreaTag;
    }

    public int getLoginTypeId()
    {
        return _m_iLoginTypeId;
    }

    public int getConnectPort()
    {
        return _m_iConnectPort;
    }

    public int getClientSocketCacheSize()
    {
        return _m_iClientSocketCacheSize;
    }

    /*******************
     * 初始化函数
     * @param _properties
     */
    public boolean init()
    {
        if (!_init("./conf/LoginServerConf.properties"))
            return false;

        _init("./customConf/LoginServerConf.properties");

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
            System.out.println("[Conf init] Start load LoginServer Properties ... ...");

            //是否允许作弊登录
            _m_bCheatEnterEnable
                    = ALConfReader.readBool(properties, "LoginServer.CheatEnterEnable", _m_bCheatEnterEnable);
            //区域标记
            _m_sAreaTag
                    = ALConfReader.readStr(properties, "LoginServer.AreaTag", _m_sAreaTag);
            //服务器类型Id
            _m_iLoginTypeId
                    = ALConfReader.readInt(properties, "LoginServer.LoginTypeId", _m_iLoginTypeId);

            //连接的平台服务器信息
            _m_iConnectPort
                    = ALConfReader.readInt(properties, "LoginServer.ConnectPort", _m_iConnectPort);
            _m_iClientSocketCacheSize
                    = ALConfReader.readInt(properties, "LoginServer.ClientSocketCacheSize", _m_iClientSocketCacheSize);

            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "LoginServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load LoginServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load LoginServer Properties Error!!");
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
