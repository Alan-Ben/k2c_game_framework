package USLOGDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPUSServer.ENPUserServerAsynEnum;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

public class UserLogDBInitializer
{
    private NPUserServer _m_server;

    /** 是否初始化 */
    private boolean _m_bInit=false;
    
    /**当前版本***/
    public String _g_curVersion="1.0.0.1";
    protected NPCommonEnum.EDBTag _g_DBTag;
    protected int _g_iDBThreadIdx;

    public UserLogDBInitializer(NPUserServer _server)
    {
        _m_server = _server;
    }

    public NPUserServer getUSServer(){return _m_server;}

    public NPCommonEnum.EDBTag getDBTag() {return _g_DBTag;}
    public int getThreadIdx() {return _g_iDBThreadIdx;}
    
    
    /**************
     * 初始化数据库连接
     * 
     * @author alzq.z
     * @time   Apr 8, 2013 11:04:37 PM
     */
    public boolean initConnections(UserLogDBConf _conf)
    {
        if(_m_bInit)
        {
            USLog.fatal(_m_server, "Don't try to init NPUSLogDBConnector twice!");
            return false;
        }
        
        //初始化数据库部分
        USLog.sys(_m_server, "Start Init User Log DB ...");
        if(!_conf.init())
        {
            USLog.fatal(_m_server, "NPUserLogDBConf Init Fail!");
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
            _g_DBTag = NPCommonEnum.EDBTag.us_log;
        }
        else
        {
            _g_DBTag = NPCommonEnum.EDBTag.valueOf("us_log_" + _conf.getConfIdx());
        }
        userDb.setDbTag(_g_DBTag);

        //设置线程索引，Log不区分线程处理
        userDb.setTaskIndex(ENPUserServerAsynEnum.USER_lOG_DB.ordinal());
       

        try
        {
            if(!userDb.initDBCon())
            {
                USLog.fatal(_m_server, "NPUserLogDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            USLog.error(_m_server, "Connect to NPUserLogDB failed", e);
            return false;
        }
        

        return WCGDBFactory.regDBObj(userDb);
    }
    
    /**初始化数据库内容***/
    public boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(getDBTag());
        if(null == tmpObj)
        {
            USLog.error(_m_server, "EDBTag.us_log got null");
            return false;
        }
        WCGDBVersionChecker checker =new WCGDBVersionChecker(getUSServer().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("USLOGDB.Bo");
        checker.checkAndUpdateVersion("USLOGDB.OptBo");
        checker.checkAndUpdateVersion("MJLog.Bo");
        checker.checkAndUpdateVersion("MJLog.EventBo");
        checker.setNewestVersion(_g_curVersion);
        if(!checker.runAutoVersionUpdate("USLOGDB.Update"))
        {
            USLog.error(_m_server, "name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
