package NPCrossRankServer;


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
public class CrossRankServerConf
{
    private static CrossRankServerConf g_instance = new CrossRankServerConf();

    public static CrossRankServerConf getInstance()
    {
        return g_instance;
    }

    /**
     * 服务器类型Id
     */
    private int _m_iTypeId;
    //时区信息
    private String _m_sTimeZone = "GMT+8:00";

    public int getTypeId()
    {
        return _m_iTypeId;
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
        if (!_init("./conf/CrossRankServerConf.properties"))
            return false;

        _init("./customConf/CrossRankServerConf.properties");

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
            System.out.println("[Conf init] Start load CrossRankServer Properties ... ...");

            //服务器类型Id
            _m_iTypeId = ALConfReader.readInt(properties, "CrossRankServer.TypeId", _m_iTypeId);
            //时区信息
            _m_sTimeZone = ALConfReader.readStr(properties, "CrossRankServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load CrossRankServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load CrossRankServer Properties Error!!");
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
