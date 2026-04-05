package ShareCodeDB;

import ALBasicCommon.ALConfReader;
import NPCommon.Log.CommLog;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class ShareCodeDBConf
{
    private static final ShareCodeDBConf _g_instance = new ShareCodeDBConf();
    public static ShareCodeDBConf getInstance() {return _g_instance;}

    /**
     * 游戏数据库信息部分
     */
    private String _m_sShareCodeDBHost = "localhost";
    private String _m_sShareCodeDBName = "god_share_code";
    private String _m_sShareCodeDBUser = "root";
    private String _m_sShareCodeDBPass = "123456";

    private String _m_sShareCodeDBSafeSavePath = "./_share_code_db";

    public String getShareCodeDBHost()
    {
        return _m_sShareCodeDBHost;
    }

    public String getShareCodeDBName()
    {
        return _m_sShareCodeDBName;
    }

    public String getShareCodeDBUser()
    {
        return _m_sShareCodeDBUser;
    }

    public String getShareCodeDBPass()
    {
        return _m_sShareCodeDBPass;
    }

    public String getShareCodeDBSafeSavePath()
    {
        return _m_sShareCodeDBSafeSavePath;
    }

    /**
     * 初始化属性设置对象
     */
    public boolean init()
    {
        boolean res;

        //初始化默认配置
        res = _init("./conf/ShareCodeDBConf.properties");

        if (_init("./customConf/ShareCodeDBConf.properties") && !res)
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
            System.out.println("[Conf init] Load ShareCodeDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sShareCodeDBHost =
                    ALConfReader.readStr(properties, "ShareCodeDB.DBHost", _m_sShareCodeDBHost);
            _m_sShareCodeDBName =
                    ALConfReader.readStr(properties, "ShareCodeDB.DBName", _m_sShareCodeDBName);
            _m_sShareCodeDBUser =
                    ALConfReader.readStr(properties, "ShareCodeDB.DBUser", _m_sShareCodeDBUser);
            _m_sShareCodeDBPass =
                    ALConfReader.readStr(properties, "ShareCodeDB.DBPass", _m_sShareCodeDBPass);

            _m_sShareCodeDBSafeSavePath =
                    ALConfReader.readStr(properties, "ShareCodeDB.DBSafeSavePath", _m_sShareCodeDBSafeSavePath);

            System.out.println("[Conf init] Finish Load ShareCodeDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read ShareCodeDB property Error!!");
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
