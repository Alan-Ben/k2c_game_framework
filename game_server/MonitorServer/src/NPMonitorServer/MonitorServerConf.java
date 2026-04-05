package NPMonitorServer;

import ALBasicCommon.ALConfReader;
import NPCommon.Log.CommLog;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/***************
 * 配置文件
 *
 * @author Administrator
 *
 */
public class MonitorServerConf
{
    private static MonitorServerConf g_instance = new MonitorServerConf();

    public static MonitorServerConf getInstance()
    {
        return g_instance;
    }

    /**
     * PHP2HS
     */
    private String _m_sPhpClientUser = "phpClientUser";
    private String _m_sPhpClientPassword = "phpClientPassword";
    private String _m_sPhpClientKey = "phpClientKey";
    //发送警报
    private boolean _m_bSendAlert = true;
    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public String getPhpClientUser()
    {
        return _m_sPhpClientUser;
    }

    public String getPhpClientPassword()
    {
        return _m_sPhpClientPassword;
    }

    public String getPhpClientKey()
    {
        return _m_sPhpClientKey;
    }

    public boolean isSendAlert()
    {
        return _m_bSendAlert;
    }

    /*******************
     * 初始化函数
     *
     * @param _properties
     */
    public boolean init()
    {
        if (!_init("./conf/MonitorServerConf.properties", false))
            return false;

        _init("./customConf/MonitorServerConf.properties", true);

        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
     * @param _bOptional
     *
     * @param _properties
     */
    protected boolean _init(String _filePath, boolean _bOptional)
    {
        Properties properties = new Properties();

        InputStream propertiesInputStream = null;

        try
        {
            propertiesInputStream = new FileInputStream(_filePath);
        } catch (Exception e)
        {
            if (_bOptional)
            {
                CommLog.error("ignore bad config file:{}", _filePath);
            } else
            {
                e.printStackTrace();
            }
        }

        if (null == propertiesInputStream)
        {
            return false;
        }

        // 载入配置文件
        try
        {
            properties.load(propertiesInputStream);
        } catch (IOException e)
        {
            e.printStackTrace();
        }

        try
        {
            System.out.println("[Conf init] Start load MonitorServer Properties ... ...");


            //PHP2HS
            _m_sPhpClientUser
                    = ALConfReader.readStr(properties, "MonitorServer.PhpClientUser", _m_sPhpClientUser);
            _m_sPhpClientPassword
                    = ALConfReader.readStr(properties, "MonitorServer.PhpClientPassword", _m_sPhpClientPassword);
            _m_sPhpClientKey
                    = ALConfReader.readStr(properties, "MonitorServer.PhpClientKey", _m_sPhpClientKey);
            //TIMEZONE
            _m_sTimeZone
                    = ALConfReader.readStr(properties, "MonitorServer.TimeZone", _m_sTimeZone);
            //LIFETIME
            _m_bSendAlert
                    = ALConfReader.readBool(properties, "MonitorServer.SendAlert", _m_bSendAlert);

            System.out.println("[Conf init] Finish load MonitorServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load MonitorServer Properties Error!!");
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
