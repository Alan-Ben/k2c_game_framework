package MarryMatchServer;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/***************
 * 配置文件
 * @author Administrator
 *
 */
public class MarryMatchServerConf
{
    private static final MarryMatchServerConf g_instance = new MarryMatchServerConf();


    public static MarryMatchServerConf getInstance()
    {
        return g_instance;
    }

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
        if (!_init("./conf/MarryMatchServerConf.properties"))
            return false;

        _init("./customConf/MarryMatchServerConf.properties");

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

        //载入配置文件
        try
        {
            properties.load(propertiesInputStream);

            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "MarryMatchServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load MarryMatchServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load MarryMatchServer Properties Error!!");
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
