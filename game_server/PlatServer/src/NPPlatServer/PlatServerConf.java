package NPPlatServer;

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
public class PlatServerConf
{
    private static PlatServerConf g_instance = new PlatServerConf();

    public static PlatServerConf getInstance()
    {
        return g_instance;
    }

    /**
     * 通用的区域标记
     */
    private String _m_sCommonAreaTag;
    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public String getCommonAreaTag()
    {
        return _m_sCommonAreaTag;
    }

    /*******************
     * 初始化函数
     * @param _properties
     */
    public boolean init()
    {
        if (!_init("./conf/PlatServerConf.properties"))
            return false;

        _init("./customConf/PlatServerConf.properties");

        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
     * @param _properties
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
        } catch (IOException e)
        {
            e.printStackTrace();
        }

        try
        {
            System.out.println("[Conf init] Start load PlatServer Properties ... ...");

            //通用区域标记
            _m_sCommonAreaTag
                    = ALConfReader.readStr(properties, "PlatServer.CommonAreaTag", _m_sCommonAreaTag);

            //TIMEZONE
            _m_sTimeZone
                    = ALConfReader.readStr(properties, "PlatServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load PlatServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load PlatServer Properties Error!!");
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
