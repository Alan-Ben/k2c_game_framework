package CSDB;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class CommonDBConf
{
    private static CommonDBConf g_instance = new CommonDBConf();

    public static CommonDBConf getInstance()
    {
        return g_instance;
    }

    /**
     * 游戏数据库信息部分
     */
    private String _m_sDBHost = "localhost";
    private String _m_sDBName = "god_common";
    private String _m_sDBUser = "root";
    private String _m_sDBPass = "root";

    private String _m_sCommonDBSafeSavePath = "./_common_db";

    public String getDBHost()
    {
        return _m_sDBHost;
    }

    public String getDBName()
    {
        return _m_sDBName;
    }

    public String getDBUser()
    {
        return _m_sDBUser;
    }

    public String getDBPass()
    {
        return _m_sDBPass;
    }

    public String getCommonDBSafeSavePath()
    {
        return _m_sCommonDBSafeSavePath;
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
        res = _init("./conf/CommonDBConf.properties");

        if (_init("./customConf/CommonDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load CommonDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sDBHost =
                    ALConfReader.readStr(properties, "CommonDB.DBHost", _m_sDBHost);
            _m_sDBName =
                    ALConfReader.readStr(properties, "CommonDB.DBName", _m_sDBName);
            _m_sDBUser =
                    ALConfReader.readStr(properties, "CommonDB.DBUser", _m_sDBUser);
            _m_sDBPass =
                    ALConfReader.readStr(properties, "CommonDB.DBPass", _m_sDBPass);

            _m_sCommonDBSafeSavePath =
                    ALConfReader.readStr(properties, "CommonDB.DBSafeSavePath", _m_sCommonDBSafeSavePath);

            System.out.println("[Conf init] Finish Load CommonDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read CommonDB property Error!!");
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
