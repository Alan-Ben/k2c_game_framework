package NPCommonServer;

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
public class CommonServerConf
{
    private static CommonServerConf g_instance = new CommonServerConf();

    public static CommonServerConf getInstance()
    {
        return g_instance;
    }

    /**
     * Cross-Game单服权重上限
     */
    private int _m_iCrossGameWeightLimit = 50000;
    //是否加载虚构服务器信息
    private boolean _m_bNeedLoadFooServerList;
    //时区
    private String _m_sTimeZone = "GMT+8:00";

    public int getCrossGameWeightLimit()
    {
        return _m_iCrossGameWeightLimit;
    }

    public boolean getNeedLoadFooServerList()
    {
        return _m_bNeedLoadFooServerList;
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
        if (!_init("./conf/CommonServerConf.properties"))
            return false;

        _init("./customConf/CommonServerConf.properties");

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
            System.out.println("[Conf init] Start load CommonServer Properties ... ...");

            //Game权重上限
            _m_iCrossGameWeightLimit = ALConfReader.readInt(properties, "CommonServer.CrossGameWeightLimit", _m_iCrossGameWeightLimit);
            //是否加载虚构服务器信息
            _m_bNeedLoadFooServerList = ALConfReader.readBool(properties, "CommonServer.NeedLoadFooServerList", _m_bNeedLoadFooServerList);
            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "CommonServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load CommonServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load CommonServer Properties Error!!");
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
