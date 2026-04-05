package NPInterfaceServer;

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
public class InterfaceServerConf
{
    private static final InterfaceServerConf g_instance = new InterfaceServerConf();

    public static InterfaceServerConf getInstance()
    {
        return g_instance;
    }

    /**
     * 区域标记
     */
    private String _m_sAreaTag;

    /**
     * 登录聊天服使用的系统标识
     */
    private String _m_chatSystemTag = "np_interface_chat_system";
    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public String getAreaTag()
    {
        return _m_sAreaTag;
    }

    public String getChatSystemTag()
    {
        return _m_chatSystemTag;
    }

    /*******************
     * 初始化函数
     */
    public boolean init()
    {
        if (!_init("./conf/InterfaceServerConf.properties"))
            return false;

        _init("./customConf/InterfaceServerConf.properties");

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
            System.out.println("[Conf init] Start load InterfaceServer Properties ... ...");

            //区域标记
            _m_sAreaTag
                    = ALConfReader.readStr(properties, "InterfaceServer.AreaTag", _m_sAreaTag);
            _m_chatSystemTag =
                    ALConfReader.readStr(properties, "InterfaceServer.ChatSystemTag", _m_chatSystemTag);

            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "InterfaceServer.TimeZone", _m_sTimeZone);

            System.out.println("[Conf init] Finish load InterfaceServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load InterfaceServer Properties Error!!");
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
