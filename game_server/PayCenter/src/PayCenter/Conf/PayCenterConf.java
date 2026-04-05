package PayCenter.Conf;

import ALBasicCommon.ALConfReader;
import NPCommon.Log.CommLog;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 * PayCenterConf - PayCenter主配置类
 * 
 * 主要功能：
 * 1. 管理PayCenter系统的全局配置参数
 * 2. 从配置文件加载系统设置
 * 3. 提供配置参数的统一访问接口
 * 4. 支持配置热更新机制
 * 
 * 设计特点：
 * - 单例模式管理全局配置
 * - Properties文件格式支持
 * - 配置参数类型安全访问
 * - 支持默认配置和自定义配置覆盖
 */
public class PayCenterConf
{
    private static final PayCenterConf g_instance = new PayCenterConf();

    public static PayCenterConf getInstance()
    {
        return g_instance;
    }

    // 当前服务器时区
    private String _m_sTimeZone = "GMT+8:00";
    
    // SDK支付回调签名密钥
    private String _m_sPaySecret = "";

    // 对平台开放服务端HTTP端口
    private int _m_toPlatHttpPort = 6004;

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }
    
    public String getPaySecret()
    {
        return _m_sPaySecret;
    }

    public int getToPlatHttpPort()
    {
        return _m_toPlatHttpPort;
    }

    /**
     * 初始化配置
     * 
     * @return true=初始化成功, false=初始化失败
     */
    public boolean init()
    {
        if (!_init("./conf/PayCenterConf.properties"))
            return false;

        _init("./customConf/PayCenterConf.properties");

        return true;
    }

    /**
     * 初始化单个配置文件的处理函数
     * 
     * @param _filePath 配置文件路径
     * @return true=加载成功, false=加载失败
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
            CommLog.error("", e);
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
            CommLog.error("", e);
        }

        try
        {
            System.out.println("[Conf init] Start load PayCenter Properties ... ...");

            // TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "PayCenter.TimeZone", _m_sTimeZone);
            
            // 支付相关配置
            _m_sPaySecret = ALConfReader.readStr(properties, "PayCenter.PaySecret", _m_sPaySecret);

            // HTTP端口配置
            _m_toPlatHttpPort = ALConfReader.readInt(properties, "PayCenter.ToPlatHttpPort", _m_toPlatHttpPort);

            System.out.println("[Conf init] Finish load PayCenter Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load PayCenter Properties Error!!");
            CommLog.error("", e);
        } finally
        {
            try
            {
                propertiesInputStream.close();
            } catch (IOException e)
            {
                CommLog.error("", e);
            }
        }

        return true;
    }
}