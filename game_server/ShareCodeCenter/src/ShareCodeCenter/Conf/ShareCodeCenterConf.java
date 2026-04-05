package ShareCodeCenter.Conf;

import ALBasicCommon.ALConfReader;
import NPCommon.Log.CommLog;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class ShareCodeCenterConf
{
    private static final ShareCodeCenterConf g_instance = new ShareCodeCenterConf();

    public static ShareCodeCenterConf getInstance()
    {
        return g_instance;
    }

    //当前服务器时区
    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    /*******************
     * 初始化函数
     */
    public boolean init()
    {
        if (!_init("./conf/ShareCodeCenterConf.properties"))
            return false;

        _init("./customConf/ShareCodeCenterConf.properties");

        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
     */
    protected boolean _init(String _filePath)
    {
        Properties properties = new Properties();

        InputStream propertiesInputStream = null;

        try
        {
            propertiesInputStream = new FileInputStream(_filePath);
        } catch (Exception e)
        {
            CommLog.error("", e);
        }

        if (null == propertiesInputStream)
        {
            return false;
        }

        //载入配置文件
        try
        {
            properties.load(propertiesInputStream);
        } catch (IOException e)
        {
            CommLog.error("", e);
        }

        try
        {
            System.out.println("[Conf init] Start load ShareCodeCenter Properties ... ...");

            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "ShareCodeCenter.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load ShareCodeCenter Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load ShareCodeCenter Properties Error!!");
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
