package CGSLOGDB;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class CrossGameLogDBConf
{
    private static CrossGameLogDBConf g_instance = new CrossGameLogDBConf();

    public static CrossGameLogDBConf getInstance()
    {
        return g_instance;
    }

    /**
     * 游戏数据库信息部分
     */
    private String _m_sDBHost = "localhost";
    private String _m_sDBName = "god_cross_game_log";
    private String _m_sDBUser = "root";
    private String _m_sDBPass = "123456";

    private String _m_sDBSafeSavePath = "./_cross_game_log_db";

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

    public String getDBSafeSavePath()
    {
        return _m_sDBSafeSavePath;
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
        res = _init("./conf/CrossGameLogDBConf.properties");

        if (_init("./customConf/CrossGameLogDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load CrossGameLogDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sDBHost =
                    ALConfReader.readStr(properties, "CrossGameLogDB.DBHost", _m_sDBHost);
            _m_sDBName =
                    ALConfReader.readStr(properties, "CrossGameLogDB.DBName", _m_sDBName);
            _m_sDBUser =
                    ALConfReader.readStr(properties, "CrossGameLogDB.DBUser", _m_sDBUser);
            _m_sDBPass =
                    ALConfReader.readStr(properties, "CrossGameLogDB.DBPass", _m_sDBPass);

            _m_sDBSafeSavePath =
                    ALConfReader.readStr(properties, "CrossGameLogDB.DBSafeSavePath", _m_sDBSafeSavePath);

            System.out.println("[Conf init] Finish Load CrossGameLogDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read CrossGameLogDB property Error!!");
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
