package HSDB;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class HttpDBConf
{
    private static final HttpDBConf g_instance = new HttpDBConf();
    /**
     * 游戏数据库信息部分
     */
    private String _m_sNPHttpDBHost = "localhost";
    private String _m_sNPHttpDBName = "god_http";
    private String _m_sNPHttpDBUser = "root";
    private String _m_sNPHttpDBPass = "123456";
    private String _m_sNPHttpDBSafeSavePath = "./_http_db";

    public static HttpDBConf getInstance()
    {
        return g_instance;
    }

    public String getNPHttpDBHost()
    {
        return _m_sNPHttpDBHost;
    }

    public String getNPHttpDBName()
    {
        return _m_sNPHttpDBName;
    }

    public String getNPHttpUser()
    {
        return _m_sNPHttpDBUser;
    }

    public String getNPHttpDBPass()
    {
        return _m_sNPHttpDBPass;
    }

    public String getNPHttpDBSafeSavePath()
    {
        return _m_sNPHttpDBSafeSavePath;
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
        res = _init("./conf/HttpDBConf.properties");

        if (_init("./customConf/HttpDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load HttpDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sNPHttpDBHost =
                    ALConfReader.readStr(properties, "HttpDB.DBHost", _m_sNPHttpDBHost);
            _m_sNPHttpDBName =
                    ALConfReader.readStr(properties, "HttpDB.DBName", _m_sNPHttpDBName);
            _m_sNPHttpDBUser =
                    ALConfReader.readStr(properties, "HttpDB.DBUser", _m_sNPHttpDBUser);
            _m_sNPHttpDBPass =
                    ALConfReader.readStr(properties, "HttpDB.DBPass", _m_sNPHttpDBPass);

            _m_sNPHttpDBSafeSavePath =
                    ALConfReader.readStr(properties, "HttpDB.DBSafeSavePath", _m_sNPHttpDBSafeSavePath);

            System.out.println("[Conf init] Finish Load HttpDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read HttpDB property Error!!");
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
