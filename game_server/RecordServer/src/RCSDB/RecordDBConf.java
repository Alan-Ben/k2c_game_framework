package RCSDB;


import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class RecordDBConf
{
    private static final RecordDBConf g_instance = new RecordDBConf();
    /**
     * 游戏数据库信息部分
     */
    private String _m_sRecordDBHost = "localhost";
    private String _m_sRecordDBName = "god_record";
    private String _m_sRecordDBUser = "root";
    private String _m_sRecordDBPass = "123456";
    private String _m_sRecordDBSafeSavePath = "./_record_db";

    public static RecordDBConf getInstance()
    {
        return g_instance;
    }

    public String getRecordDBHost()
    {
        return _m_sRecordDBHost;
    }

    public String getRecordDBName()
    {
        return _m_sRecordDBName;
    }

    public String getRecordDBUser()
    {
        return _m_sRecordDBUser;
    }

    public String getRecordDBPass()
    {
        return _m_sRecordDBPass;
    }

    public String getRecordDBSafeSavePath()
    {
        return _m_sRecordDBSafeSavePath;
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
        res = _init("./conf/RecordDBConf.properties");

        if (_init("./customConf/RecordDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load RecordDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sRecordDBHost =
                    ALConfReader.readStr(properties, "RecordDB.DBHost", _m_sRecordDBHost);
            _m_sRecordDBName =
                    ALConfReader.readStr(properties, "RecordDB.DBName", _m_sRecordDBName);
            _m_sRecordDBUser =
                    ALConfReader.readStr(properties, "RecordDB.DBUser", _m_sRecordDBUser);
            _m_sRecordDBPass =
                    ALConfReader.readStr(properties, "RecordDB.DBPass", _m_sRecordDBPass);
            _m_sRecordDBSafeSavePath =
                    ALConfReader.readStr(properties, "RecordDB.DBSafeSavePath", _m_sRecordDBSafeSavePath);

            System.out.println("[Conf init] Finish Load RecordDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read RecordDB property Error!!");
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
