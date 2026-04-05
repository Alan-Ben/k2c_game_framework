package USDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPUSServer.NPUSMain;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

public class UserDBInitializer
{
    private NPUserServer _m_server;
    /**
     * 是否初始化
     */
    private boolean _m_bInit = false;

    /**
     * 当前版本
     ***/
    protected String _g_curVersion = "1.0.2.1";
    protected NPCommonEnum.EDBTag _g_DBTag;
    protected int _g_iDBThreadIdx;

    public UserDBInitializer(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    public String getDBVersion() {return _g_curVersion;}
    
    public NPCommonEnum.EDBTag getDBTag() {return _g_DBTag;}
    public int getThreadIdx() {return _g_iDBThreadIdx;}


    /**************
     * 初始化数据库连接
     *
     * @author alzq.z
     * @time Apr 8, 2013 11:04:37 PM
     */
    public boolean initConnections(UserDBConf _conf)
    {
        if (_m_bInit)
        {
            USLog.fatal(_m_server, "Don't try to init NPUSDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        USLog.sys(_m_server, "Start Init User DB ...");
        if (!_conf.init())
        {
            USLog.fatal(_m_server, "NPUserDBConf Init Fail!");
            return false;
        }

        WCGDBObj userDb = new WCGDBObj(
                _conf.getUserDBHost(),
                _conf.getUserDBName(),
                _conf.getUserDBUser(),
                _conf.getUserDBPass(),
                _conf.getUserDBSafeSavePath()
        );

        //0则不改变标记，否则以us_为前缀，改变数据库标记
        if(_conf.getConfIdx() == 0) {
            _g_DBTag = NPCommonEnum.EDBTag.main;
        }
        else
        {
            _g_DBTag = NPCommonEnum.EDBTag.valueOf("us_" + _conf.getConfIdx());
        }

        userDb.setDbTag(_g_DBTag);

        //设置线程索引
        _g_iDBThreadIdx = NPUSMain.GetUserDBAsynThreadIdx(_conf.getConfIdx());
        userDb.setTaskIndex(_g_iDBThreadIdx);


        try
        {
            if (!userDb.initDBCon())
            {
                USLog.fatal(_m_server, "NPUserDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            USLog.error(_m_server, "Connect to UserDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(userDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(getDBTag());
        if (null == tmpObj)
        {
            USLog.error(_m_server, "EDBTag.main got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(getUSServer().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("USDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("USDB.Update"))
        {
            USLog.error(_m_server, "name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
