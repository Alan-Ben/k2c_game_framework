package NPUSServer;

import ALBasicCommon.ALConfReader;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.Properties;

/***************
 * 配置文件
 * @author Administrator
 *
 */
public class UserServerConf
{
    private static UserServerConf g_instance = new UserServerConf();

    public static UserServerConf getInstance()
    {
        return g_instance;
    }

    //开启服务器的数量
    private int _m_iOpenCount = 1;
    //服务器类型Id队列
    private ArrayList<Integer> _m_lTypeIdList = new ArrayList<Integer>();
    //最高可加载的用户数据数量，超出数量则需要排队
    private int _m_iMaxLoadUserData = 3000;
    //最大注册玩家数，目前仅做预警处理，不做限制
    private int _m_iMaxUserCount = 3000;
    //是否打开GM命令
    private boolean _m_openGm = false;
    //是否需要依赖CommonServer开启,同步服务器状态，默认为true
    private boolean _m_needRelyCSOpen;
    //是否非正式服务器，一般在一些日志报警上做判断，默认为false。本地需要屏蔽信息则为true
    private boolean _m_bIsIllegalServer = false;
    //用户数据卸载时长
    private long _m_userDataExpiredTime = 30 * 60 * 1000;
    //时区
    private String _m_sTimeZone = "GMT+8:00";

    public String getTimeZone()
    {
        return _m_sTimeZone;
    }

    public int getOpenCount() {return _m_iOpenCount;}


    public int getTypeId(int _idx)
    {
        return _m_lTypeIdList.get(_idx);
    }

    public int getMaxLoadUserData()
    {
        return _m_iMaxLoadUserData;
    }

    public void setMaxLoadUserData(int _userCount)
    {
        _m_iMaxLoadUserData = _userCount;
    }

    public int getMaxUserCount()
    {
        return _m_iMaxUserCount;
    }

    public long getUserDataExpiredTimeMs()
    {
        return _m_userDataExpiredTime;
    }

    public boolean getGm()
    {
        return _m_openGm;
    }

    public boolean getNeedRelyCSOpen() { return _m_needRelyCSOpen; }

    public boolean getIsIllegalServer() { return _m_bIsIllegalServer; }

    //以下是用于运营日志的平台信息
    //US配置的平台ID
    private int _m_platformId;
    //US配置的大区ID
    private int _m_platAreaId;
    //平台信息 相关GETTER && SETTER
    public int getPlatformId() {return _m_platformId;}
    public int getPlatAreaId() {return _m_platAreaId;}
    
    //来自HS的平台参数
    private int _m_hsPlatformId;
    private int _m_hsPlatAreaId;
    public void setHSPlatformId(int _platId) {_m_hsPlatformId = _platId;}
    public void setHSPlatAreaId(int _areaId) {_m_hsPlatAreaId = _areaId;}
    public int getHSPlatformId() {return _m_hsPlatformId;}
    public int getHSPlatAreaId() {return _m_hsPlatAreaId;}
    
    /**
     * 设置卸载超时的持续时间，单位：秒
     * @param _expiredSecs
     */
    public void setUserDataExpiredTimeSec(int _expiredSecs)
    {
        _m_userDataExpiredTime = _expiredSecs * 1000;
    }

    /*******************
     * 初始化函数
     */
    public boolean init()
    {
        if (!_init("./conf/UserServerConf.properties"))
            return false;

        _init("./customConf/UserServerConf.properties");
        return true;
    }

    /*******************
     * 初始化单个配置文件的处理函数
     * @param _filePath
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
            return false;
        }

        try
        {
            System.out.println("[Conf init] Start load UserServer Properties ... ...");

            //读取开几个服务器
            _m_iOpenCount = ALConfReader.readInt(properties, "UserServer.openCount", _m_iOpenCount);

            //服务器类型Id
            String typeIdListStr = ALConfReader.readStr(properties, "UserServer.typeId", "");
            String[] idArr = typeIdListStr.split(":");
            _m_lTypeIdList.clear();
            //逐个读取服务器的TypeId
            for (int i = 0; i < idArr.length; i++)
            {
                _m_lTypeIdList.add(Integer.parseInt(idArr[i]));
            }

            _m_iMaxLoadUserData = ALConfReader.readInt(properties, "UserServer.maxLoadUserData", _m_iMaxLoadUserData);

            _m_iMaxUserCount = ALConfReader.readInt(properties, "UserServer.userMaxCount", _m_iMaxUserCount);

            _m_userDataExpiredTime = ALConfReader.readLong(properties, "UserServer.userDataExpiredTimeMs", _m_userDataExpiredTime);
            //超时释放最少保证10分钟，避免登录的时候加载完但是用户数据被卸载
            if (_m_userDataExpiredTime < 600000)
                _m_userDataExpiredTime = 600000;

            _m_openGm = ALConfReader.readBool(properties, "UserServer.openGM", _m_openGm);

            _m_needRelyCSOpen = ALConfReader.readBool(properties, "UserServer.needRelyCSOpen", _m_needRelyCSOpen);

            _m_bIsIllegalServer = ALConfReader.readBool(properties, "UserServer.isIllegalServer", _m_bIsIllegalServer);

            //TIMEZONE
            _m_sTimeZone = ALConfReader.readStr(properties, "UserServer.TimeZone", _m_sTimeZone);
            
            _m_platAreaId = ALConfReader.readInt(properties, "UserServer.PlatAreaId", _m_platAreaId);
            _m_platformId = ALConfReader.readInt(properties, "UserServer.PlatformId", _m_platformId);

            System.out.println("[Conf init] Finish load UserServer Properties ... ...");
        } catch (Exception e)
        {
            System.out.println("[Conf Init Error] Load UserServer Properties Error!!");
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

    public boolean getOpenGM()
    {
        return _m_openGm;
    }
}
