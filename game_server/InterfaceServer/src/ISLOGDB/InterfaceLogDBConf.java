package ISLOGDB;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class InterfaceLogDBConf
{
    private static final InterfaceLogDBConf g_instance = new InterfaceLogDBConf();
    /**
     * 游戏数据库信息部分
     */
    private String _m_sInterfaceDBHost = "localhost";
    private String _m_sInterfaceDBName = "god_interface_log";
    private String _m_sInterfaceDBInterface = "root";
    private String _m_sInterfaceDBPass = "123456";

    private String _m_sInterfaceDBSafeSavePath = "./_interface_log_db";

    public static InterfaceLogDBConf getInstance()
    {
        return g_instance;
    }

    public String getInterfaceDBHost()
    {
        return _m_sInterfaceDBHost;
    }

    public String getInterfaceDBName()
    {
        return _m_sInterfaceDBName;
    }

    public String getInterfaceDBInterface()
    {
        return _m_sInterfaceDBInterface;
    }

    public String getInterfaceDBPass()
    {
        return _m_sInterfaceDBPass;
    }

    public String getInterfaceDBSafeSavePath()
    {
        return _m_sInterfaceDBSafeSavePath;
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
        res = _init("./conf/InterfaceLogDBConf.properties");

        if (_init("./customConf/InterfaceLogDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load InterfaceLogDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sInterfaceDBHost =
                    ALConfReader.readStr(properties, "InterfaceLogDB.DBHost", _m_sInterfaceDBHost);
            _m_sInterfaceDBName =
                    ALConfReader.readStr(properties, "InterfaceLogDB.DBName", _m_sInterfaceDBName);
            _m_sInterfaceDBInterface =
                    ALConfReader.readStr(properties, "InterfaceLogDB.DBUser", _m_sInterfaceDBInterface);
            _m_sInterfaceDBPass =
                    ALConfReader.readStr(properties, "InterfaceLogDB.DBPass", _m_sInterfaceDBPass);
            _m_sInterfaceDBSafeSavePath =
                    ALConfReader.readStr(properties, "InterfaceLogDB.DBSafeSavePath", _m_sInterfaceDBSafeSavePath);

            System.out.println("[Conf init] Finish Load InterfaceLogDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read InterfaceLogDB property Error!!");
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
