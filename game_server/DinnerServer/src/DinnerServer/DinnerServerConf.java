package DinnerServer;


import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/***************
 * 配置文件
 *
 * @author Administrator
 */
public class DinnerServerConf
{
    private static DinnerServerConf g_instance = new DinnerServerConf();

    public static DinnerServerConf getInstance()
    {
        return g_instance;
    }

    //时区
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
        if (!_init("./conf/DinnerServerConf.properties"))
            return false;

        _init("./customConf/DinnerServerConf.properties");

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
            e.printStackTrace();
        }
        if (null == propertiesInputStream)
        {
            return false;
        }
        // 载入配置文件
        try
        {
            properties.load(propertiesInputStream);
        } catch (IOException e)
        {
            e.printStackTrace();
        }
        try
        {
            System.out.println("[Conf init] Start load DinnerServer Properties ... ...");

            _m_sTimeZone = ALConfReader.readStr(properties, "DinnerServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load DinnerServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load DinnerServer Properties Error!!");
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
