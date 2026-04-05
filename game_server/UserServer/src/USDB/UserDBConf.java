package USDB;

import ALBasicCommon.ALConfReader;
import NPCommon.Log.CommLog;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class UserDBConf
{
    private final int _m_iIdx;
    private final int _m_iServerTypeId;

    /**
     * 游戏数据库信息部分
     */
    private String _m_sUserDBHost = "localhost";
    private String _m_sUserDBName = "god_user";
    private String _m_sUserDBUser = "root";
    private String _m_sUserDBPass = "123456";

    private String _m_sUserDBSafeSavePath = "./_user_db";


    public UserDBConf(int _idx, int _iServerTypeId)
    {
        _m_iIdx = _idx;
        _m_iServerTypeId = _iServerTypeId;
    }

    public int getConfIdx() {return _m_iIdx;}

    public String getUserDBHost()
    {
        return _m_sUserDBHost;
    }

    public String getUserDBName()
    {
        return _m_sUserDBName;
    }

    public String getUserDBUser()
    {
        return _m_sUserDBUser;
    }

    public String getUserDBPass()
    {
        return _m_sUserDBPass;
    }

    public String getUserDBSafeSavePath()
    {
        return _m_sUserDBSafeSavePath;
    }

    /*******************
     * 初始化属性设置对象
     *
     * @author alzq.z
     * @time Feb 21, 2013 2:01:17 PM
     */
    public boolean init()
    {
        if (_m_iServerTypeId == 0)
            return false;

        boolean res = false;

        //0则不加后缀，其他增加后缀
        String name = "UserDBConf" + _m_iServerTypeId + ".properties";

        //初始化默认配置
        res = _init("./conf/" + name);

        if (_init("./customConf/" + name))
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
            CommLog.error("UserDBConf._init Load properties file Error!! idx:{} typeId:{}",
                    _m_iIdx, _m_iServerTypeId, e);
            return false;
        }

        try
        {
            System.out.println("[Conf init] Load UserDB Properties start ... ...");

            //读取游戏数据库部分信息
            _m_sUserDBHost =
                    ALConfReader.readStr(properties, "UserDB.DBHost", _m_sUserDBHost);
            _m_sUserDBName =
                    ALConfReader.readStr(properties, "UserDB.DBName", _m_sUserDBName);
            _m_sUserDBUser =
                    ALConfReader.readStr(properties, "UserDB.DBUser", _m_sUserDBUser);
            _m_sUserDBPass =
                    ALConfReader.readStr(properties, "UserDB.DBPass", _m_sUserDBPass);

            _m_sUserDBSafeSavePath =
                    ALConfReader.readStr(properties, "UserDB.DBSafeSavePath", _m_sUserDBSafeSavePath);

            System.out.println("[Conf init] Finish Load UserDB Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf init Error] Read UserDB property Error!!");
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
