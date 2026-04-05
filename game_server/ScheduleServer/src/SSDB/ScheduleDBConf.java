package SSDB;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class ScheduleDBConf
{
    private static final ScheduleDBConf g_instance = new ScheduleDBConf();
    /**
     * 游戏数据库信息部分
     */
    private String _m_sScheduleDBHost = "localhost";
    private String _m_sScheduleDBName = "god_schedule";
    private String _m_sScheduleDBUser = "root";
    private String _m_sScheduleDBPass = "123456";
    private String _m_sScheduleDBSafeSavePath = "./_schedule_db";

    public static ScheduleDBConf getInstance()
    {
        return g_instance;
    }

    public String getScheduleDBHost()
    {
        return _m_sScheduleDBHost;
    }

    public String getScheduleDBName()
    {
        return _m_sScheduleDBName;
    }

    public String getScheduleDBUser()
    {
        return _m_sScheduleDBUser;
    }

    public String getScheduleDBPass()
    {
        return _m_sScheduleDBPass;
    }

    public String getScheduleDBSafeSavePath()
    {
        return _m_sScheduleDBSafeSavePath;
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
        res = _init("./conf/ScheduleDBConf.properties");

        if (_init("./customConf/ScheduleDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load ScheduleDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sScheduleDBHost =
                    ALConfReader.readStr(properties, "ScheduleDB.DBHost", _m_sScheduleDBHost);
            _m_sScheduleDBName =
                    ALConfReader.readStr(properties, "ScheduleDB.DBName", _m_sScheduleDBName);
            _m_sScheduleDBUser =
                    ALConfReader.readStr(properties, "ScheduleDB.DBUser", _m_sScheduleDBUser);
            _m_sScheduleDBPass =
                    ALConfReader.readStr(properties, "ScheduleDB.DBPass", _m_sScheduleDBPass);

            _m_sScheduleDBSafeSavePath =
                    ALConfReader.readStr(properties, "ScheduleDB.DBSafeSavePath", _m_sScheduleDBSafeSavePath);

            System.out.println("[Conf init] Finish Load ScheduleDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read ScheduleDB property Error!!");
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
