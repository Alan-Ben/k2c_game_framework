package NPLoginCheckServer;

import ALBasicCommon.ALConfReader;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonParser;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

/***************
 * 配置文件
 * @author Administrator
 *
 */
public class LoginCheckServerConf
{
    //////单例的//////
    private static final LoginCheckServerConf _s_instance = new LoginCheckServerConf();

    public static LoginCheckServerConf getInstance()
    {
        return _s_instance;
    }

    private LoginCheckServerConf()
    {
    }

    private String _m_SDKKey = "";
    private String _m_sSDKAppId = "";
    private String _m_sSDKUrl = "http://127.0.0.1";
    private String _m_sSDKLoginUrl = "";

    private List<String> _m_lStandBySDKUrlList = new ArrayList<>();

    /**
     * 区域标记
     */
    private boolean _m_bCheckAcc;

    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public boolean isCheckAcc()
    {
        return _m_bCheckAcc;
    }

    public String getSDKUrl()
    {
        return _m_sSDKUrl;
    }

    public String getSDKLoginUrl()
    {
        return _m_sSDKLoginUrl;
    }

    public void setSDKLoginUrl(String sdkLoginUrl)
    {
        this._m_sSDKLoginUrl = sdkLoginUrl;
    }

    public List<String> getStandBySDKUrlList()
    {
        return new ArrayList<>(_m_lStandBySDKUrlList);
    }

    /*******************
     * 初始化函数
     * @param _properties
     */
    public boolean init()
    {
        if (!_init("./conf/LoginCheckServerConf.properties"))
            return false;

        _init("./customConf/LoginCheckServerConf.properties");

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
            System.out.println("[Conf init] Start load LoginCheckServer Properties ... ...");

            //区域标记
            _m_bCheckAcc = ALConfReader.readBool(properties, "LoginCheckServer.NeedCheckAcc", _m_bCheckAcc);
            _m_sSDKUrl = ALConfReader.readStr(properties, "LoginCheckServer.SdkUrl", _m_sSDKUrl);
            _m_sSDKLoginUrl = ALConfReader.readStr(properties, "LoginCheckServer.SdkLoginUrl", _m_sSDKLoginUrl);
            _m_sSDKAppId = ALConfReader.readStr(properties, "LoginCheckServer.SdkAppId", _m_sSDKAppId);
            _m_SDKKey = ALConfReader.readStr(properties, "LoginCheckServer.SdkKey", _m_SDKKey);

            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "LoginCheckServer.TimeZone", _m_sTimeZone);

            //解析备用SDK登录校验地址
            String standByLoginUrlList = ALConfReader.readStr(properties, "LoginCheckServer.StandByLoginUrlList", "[]");
            //通过Json进行解析
            JsonArray standByUrlJsonArray = new JsonParser().parse(standByLoginUrlList).getAsJsonArray();
            for (JsonElement standByUrl : standByUrlJsonArray)
            {
                _m_lStandBySDKUrlList.add(standByUrl.getAsString());
            }

            System.out.println("[Conf init] Finish load LoginCheckServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load LoginCheckServer Properties Error!!");
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

    public String getStrAppId()
    {
        return _m_sSDKAppId;
    }

    public void setStrAppId(String _appId)
    {
        this._m_sSDKAppId = _appId;
    }

    public String getSDKKey()
    {
        return _m_SDKKey;
    }

    public void setSDKKey(String _sdkKey)
    {
        this._m_SDKKey = _sdkKey;
    }
}
