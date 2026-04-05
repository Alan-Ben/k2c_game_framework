package NPCrossGameServer;

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
public class CrossGameServerConf
{
    private static CrossGameServerConf g_instance = new CrossGameServerConf();

    public static CrossGameServerConf getInstance()
    {
        return g_instance;
    }

    /**
     * 服务器类型Id
     */
    private int _m_iTypeId;
    /**
     * 区域标记
     */
    private String _m_sAreaTag = "internal";
    //时区信息
    private String _m_sTimeZone = "GMT+8:00";

    public int getTypeId()
    {
        return _m_iTypeId;
    }

    public String getAreaTag()
    {
        return _m_sAreaTag;
    }

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }


    /*******************
     * 初始化函数
     *
     * @param _properties
     */
    public boolean init()
    {
        if (!_init("./conf/CrossGameServerConf.properties"))
            return false;

        _init("./customConf/CrossGameServerConf.properties");

        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
     *
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
            System.out.println("[Conf init] Start load CrossGameServer Properties ... ...");

            // 区域标记
            _m_sAreaTag = ALConfReader.readStr(properties, "CrossGameServer.AreaTag", _m_sAreaTag);
            //服务器类型Id
            _m_iTypeId = ALConfReader.readInt(properties, "CrossGameServer.TypeId", _m_iTypeId);
            //时区信息
            _m_sTimeZone = ALConfReader.readStr(properties, "CrossGameServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load CrossGameServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load CrossGameServer Properties Error!!");
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
