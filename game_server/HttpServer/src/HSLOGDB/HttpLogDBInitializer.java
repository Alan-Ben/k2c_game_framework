package HSLOGDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;
import NPHttpServer.ENPHttpServerAsynEnum;
import NPHttpServer.NPHttpServer;

public class HttpLogDBInitializer
{
    /**
     * 是否初始化
     */
    private static final boolean _m_bInit = false;
    /**
     * 当前版本
     ***/
    public static String _g_curVersion = "1.0.0.1";

    /**************
     * 初始化数据库连接
     *
     * @author alzq.z
     * @time Apr 8, 2013 11:04:37 PM
     */
    public static boolean initConnections()
    {
        if (_m_bInit)
        {
            ALServerLog.Fatal("Don't try to init NPHSLOGDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Http Log DB ...");
        if (!HttpLogDBConf.getInstance().init())
        {
            CommLog.fatal("NPHSLogDBConf Init Fail!");
            return false;
        }

        WCGDBObj HttpDb = new WCGDBObj(
                HttpLogDBConf.getInstance().getHttpDBHost(),
                HttpLogDBConf.getInstance().getHttpDBName(),
                HttpLogDBConf.getInstance().getHttpDBHttp(),
                HttpLogDBConf.getInstance().getHttpDBPass(),
                HttpLogDBConf.getInstance().getHttpDBSafeSavePath()
        );
        HttpDb.setDbTag(NPCommonEnum.EDBTag.hs_log);
        HttpDb.setTaskIndex(ENPHttpServerAsynEnum.HS_LOG_DB.ordinal());


        try
        {
            if (!HttpDb.initDBCon())
            {
                ALServerLog.Fatal("NPHttpLogDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to NPHttpLogDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(HttpDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.hs_log);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.us_log got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPHttpServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("HSLOGDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("HSLOGDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
