package PayDB;

import ALBasicCommon.ALConfReader;
import NPCommon.Log.CommLog;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class PayDBConf
{
    private static final PayDBConf _g_instance = new PayDBConf();
    public static PayDBConf getInstance() {return _g_instance;}

    /**
     * 游戏数据库信息部分
     */
    private String _m_sPayDBHost = "localhost";
    private String _m_sPayDBName = "god_share_code";
    private String _m_sPayDBUser = "root";
    private String _m_sPayDBPass = "123456";

    private String _m_sPayDBSafeSavePath = "./_share_code_db";

    public String getPayDBHost()
    {
        return _m_sPayDBHost;
    }

    public String getPayDBName()
    {
        return _m_sPayDBName;
    }

    public String getPayDBUser()
    {
        return _m_sPayDBUser;
    }

    public String getPayDBPass()
    {
        return _m_sPayDBPass;
    }

    public String getPayDBSafeSavePath()
    {
        return _m_sPayDBSafeSavePath;
    }

    /**
     * 初始化属性设置对象
     */
    public boolean init()
    {
        boolean res;

        //初始化默认配置
        res = _init("./conf/PayDBConf.properties");

        if (_init("./customConf/PayDBConf.properties") && !res)
            res = true;

        return res;
    }

    /**
     * 服务器配置初始化函数
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
            CommLog.error("", e);
        }

        try
        {
            System.out.println("[Conf init] Load PayDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sPayDBHost =
                    ALConfReader.readStr(properties, "PayDB.DBHost", _m_sPayDBHost);
            _m_sPayDBName =
                    ALConfReader.readStr(properties, "PayDB.DBName", _m_sPayDBName);
            _m_sPayDBUser =
                    ALConfReader.readStr(properties, "PayDB.DBUser", _m_sPayDBUser);
            _m_sPayDBPass =
                    ALConfReader.readStr(properties, "PayDB.DBPass", _m_sPayDBPass);

            _m_sPayDBSafeSavePath =
                    ALConfReader.readStr(properties, "PayDB.DBSafeSavePath", _m_sPayDBSafeSavePath);

            System.out.println("[Conf init] Finish Load PayDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read PayDB property Error!!");
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
