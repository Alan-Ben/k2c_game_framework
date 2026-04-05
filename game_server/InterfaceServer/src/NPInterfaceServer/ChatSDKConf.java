package NPInterfaceServer;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class ChatSDKConf
{
    private static final ChatSDKConf g_instance = new ChatSDKConf();
    //连接用户名
    private String _m_sSystemTag;
    //连接用户名
    private long _m_sSystemId;
    //连接用户名
    private String _m_sUser;
    //连接密码
    private String _m_sPassword;
    //连接端口
    private int _m_connectPort;
    //连接 IP
    private String _m_connectIP;

    public static ChatSDKConf getInstance()
    {
        return g_instance;
    }

    public String getSystemTag()
    {
        return _m_sSystemTag;
    }

    public long getSystemId()
    {
        return _m_sSystemId;
    }

    public String getConnectUser()
    {
        return _m_sUser;
    }

    public String getConnectPassword()
    {
        return _m_sPassword;
    }

    public String getConnectIp()
    {
        return _m_connectIP;
    }

    public int getConnectPort()
    {
        return _m_connectPort;
    }

    /**
     * 初始化入口
     * @return 是否成功初始化
     */
    public boolean init()
    {
        if (!_init("./conf/ChatSDKConf.properties"))
            return false;

        _init("./customConf/ChatSDKConf.properties");

        return true;
    }

    /**
     * 初始化单个文件
     * @param _filePath 文件路径
     * @return boolean
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
            System.out.println("[Conf init] Start load ChatSDk Properties ... ...");

            //连接用户名
            _m_sSystemTag = ALConfReader.readStr(properties, "ChatSDK.SystemTag", _m_sSystemTag);
            //连接用户名
            _m_sSystemId = ALConfReader.readLong(properties, "ChatSDK.SystemId", _m_sSystemId);
            //连接用户名
            _m_sUser = ALConfReader.readStr(properties, "ChatSDK.ConnectUser", _m_sUser);
            //连接密码
            _m_sPassword = ALConfReader.readStr(properties, "ChatSDK.ConnectPassword", _m_sPassword);
            //连接端口
            _m_connectPort = ALConfReader.readInt(properties, "ChatSDK.ConnectPort", _m_connectPort);
            //连接 Ip
            _m_connectIP = ALConfReader.readStr(properties, "ChatSDK.ConnectIp", _m_connectIP);

            System.out.println("[Conf init] Finish load ChatSDk Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load ChatSDk Properties Error!!");
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
