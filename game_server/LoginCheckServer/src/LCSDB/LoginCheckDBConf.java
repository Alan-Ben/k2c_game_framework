package LCSDB;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class LoginCheckDBConf
{
    private static LoginCheckDBConf g_instance = new LoginCheckDBConf();

    public static LoginCheckDBConf getInstance()
    {
        return g_instance;
    }

    /**
     * 游戏数据库信息部分
     */
    private String _m_sAccDBHost = "localhost";
    private String _m_sAccDBName = "god_login_check";
    private String _m_sAccDBUser = "root";
    private String _m_sAccDBPass = "123456";

    private String _m_sAccDBSafeSavePath = "./_login_check_db";

    public String getAccDBHost()
    {
        return _m_sAccDBHost;
    }

    public String getAccDBName()
    {
        return _m_sAccDBName;
    }

    public String getAccDBUser()
    {
        return _m_sAccDBUser;
    }

    public String getAccDBPass()
    {
        return _m_sAccDBPass;
    }

    public String getAccDBSafeSavePath()
    {
        return _m_sAccDBSafeSavePath;
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
        res = _init("./conf/LoginCheckDBConf.properties");

        if (_init("./customConf/LoginCheckDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load LoginCheckDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sAccDBHost =
                    ALConfReader.readStr(properties, "LoginCheckDB.DBHost", _m_sAccDBHost);
            _m_sAccDBName =
                    ALConfReader.readStr(properties, "LoginCheckDB.DBName", _m_sAccDBName);
            _m_sAccDBUser =
                    ALConfReader.readStr(properties, "LoginCheckDB.DBUser", _m_sAccDBUser);
            _m_sAccDBPass =
                    ALConfReader.readStr(properties, "LoginCheckDB.DBPass", _m_sAccDBPass);

            _m_sAccDBSafeSavePath =
                    ALConfReader.readStr(properties, "LoginCheckDB.DBSafeSavePath", _m_sAccDBSafeSavePath);

            System.out.println("[Conf init] Finish Load LoginCheckDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read LoginCheckDB property Error!!");
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
