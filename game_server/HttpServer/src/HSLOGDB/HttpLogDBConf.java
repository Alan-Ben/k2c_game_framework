package HSLOGDB;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class HttpLogDBConf
{
    private static final HttpLogDBConf g_instance = new HttpLogDBConf();
    /**
     * 游戏数据库信息部分
     */
    private String _m_sHttpDBHost = "localhost";
    private String _m_sHttpDBName = "god_http_log";
    private String _m_sHttpDBHttp = "root";
    private String _m_sHttpDBPass = "123456";
    private String _m_sHttpDBSafeSavePath = "./_http_log_db";

    public static HttpLogDBConf getInstance()
    {
        return g_instance;
    }

    public String getHttpDBHost()
    {
        return _m_sHttpDBHost;
    }

    public String getHttpDBName()
    {
        return _m_sHttpDBName;
    }

    public String getHttpDBHttp()
    {
        return _m_sHttpDBHttp;
    }

    public String getHttpDBPass()
    {
        return _m_sHttpDBPass;
    }

    public String getHttpDBSafeSavePath()
    {
        return _m_sHttpDBSafeSavePath;
    }

    /*******************
     * 初始化属性设置对象
     *
     * @author alzq.z
     * @time Feb 21, 2013 2:01:17 PM
     */
    public boolean init()
    {
        boolean res = false;

        //初始化默认配置
        res = _init("./conf/HttpLogDBConf.properties");

        if (_init("./customConf/HttpLogDBConf.properties") && !res)
            res = true;

        return res;
    }

    /******************
     * 服务器配置初始化函数
     *
     * @author alzq.z
     * @time Feb 21, 2013 2:01:24 PM
     */
    protected boolean _init(String _filePath)
    {
        Properties properties = new Properties();

        InputStream propertiesInputStream = null;

        try
        {
            propertiesInputStream = new FileInputStream(_filePath);
        } catch (IOException e)
        {
            System.out.println("[Conf init Error] Can not find properties file: " + _filePath + "!!");
        }

        if (null == propertiesInputStream)
            return false;

        //输入有效则开始读取对应配置
        try
        {
            properties.load(propertiesInputStream);
        } catch (IOException e)
        {
            e.printStackTrace();
        }

        try
        {
            System.out.println("[Conf init] Load HttpLogDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sHttpDBHost =
                    ALConfReader.readStr(properties, "HttpLogDB.DBHost", _m_sHttpDBHost);
            _m_sHttpDBName =
                    ALConfReader.readStr(properties, "HttpLogDB.DBName", _m_sHttpDBName);
            _m_sHttpDBHttp =
                    ALConfReader.readStr(properties, "HttpLogDB.DBHttp", _m_sHttpDBHttp);
            _m_sHttpDBPass =
                    ALConfReader.readStr(properties, "HttpLogDB.DBPass", _m_sHttpDBPass);
            _m_sHttpDBSafeSavePath =
                    ALConfReader.readStr(properties, "HttpLogDB.DBSafeSavePath", _m_sHttpDBSafeSavePath);

            System.out.println("[Conf init] Finish Load HttpLogDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read HttpLogDB property Error!!");
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
