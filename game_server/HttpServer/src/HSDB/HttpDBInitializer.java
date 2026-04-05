package HSDB;


import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import NPHttpServer.ENPHttpServerAsynEnum;
import NPHttpServer.NPHttpServer;

public class HttpDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPHSDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Http DB ...");
        if (!HttpDBConf.getInstance().init())
        {
            CommLog.fatal("NPHSDBConf Init Fail!");
            return false;
        }

        WCGDBObj NPHttpDB = new WCGDBObj(
                HttpDBConf.getInstance().getNPHttpDBHost(),
                HttpDBConf.getInstance().getNPHttpDBName(),
                HttpDBConf.getInstance().getNPHttpUser(),
                HttpDBConf.getInstance().getNPHttpDBPass(),
                HttpDBConf.getInstance().getNPHttpDBSafeSavePath()
        );
        NPHttpDB.setDbTag(EDBTag.hs_db);
        NPHttpDB.setTaskIndex(ENPHttpServerAsynEnum.HS_DB.ordinal());


        try
        {
            if (!NPHttpDB.initDBCon())
            {
                ALServerLog.Fatal("NPHttpDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to NPHttpDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(NPHttpDB);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.hs_db);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.hs_db got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPHttpServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("HSDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("HSDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
