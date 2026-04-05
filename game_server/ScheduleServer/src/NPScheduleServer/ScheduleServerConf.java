package NPScheduleServer;

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
public class ScheduleServerConf
{
    private static final ScheduleServerConf g_instance = new ScheduleServerConf();

    public static ScheduleServerConf getInstance()
    {
        return g_instance;
    }

    //排期活动的文件路径
    private String _m_sZoneSchedulePath = "./data/zone_schedule_";
    //当前服务器时区
    private String _m_sTimeZone = "GMT+8:00";

    public String getZoneSchedulePath()
    {
    	return _m_sZoneSchedulePath;
    }
    
    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    /*******************
     * 初始化函数
     */
    public boolean init()
    {
        if (!_init("./conf/ScheduleServerConf.properties"))
            return false;

        _init("./customConf/ScheduleServerConf.properties");

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
        } catch (IOException e)
        {
            e.printStackTrace();
        }

        try
        {
            System.out.println("[Conf init] Start load ScheduleServer Properties ... ...");

            //下一个排期的文件路径
            _m_sZoneSchedulePath = ALConfReader.readStr(properties, "ScheduleServer.ZoneSchedulePath", _m_sZoneSchedulePath);
            
            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "ScheduleServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load ScheduleServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load ScheduleServer Properties Error!!");
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
