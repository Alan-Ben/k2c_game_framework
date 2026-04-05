package ISDB;


import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import NPInterfaceServer.ENPInterfaceServerAsynEnum;
import NPInterfaceServer.NPInterfaceServer;

public class InterfaceDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPISDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Interface DB ...");
        if (!InterfaceDBConf.getInstance().init())
        {
            CommLog.fatal("NPISDBConf Init Fail!");
            return false;
        }

        WCGDBObj InterfaceDb = new WCGDBObj(
                InterfaceDBConf.getInstance().getInterfaceDBHost(),
                InterfaceDBConf.getInstance().getInterfaceDBName(),
                InterfaceDBConf.getInstance().getInterfaceDBUser(),
                InterfaceDBConf.getInstance().getInterfaceDBPass(),
                InterfaceDBConf.getInstance().getInterfaceDBSafeSavePath()
        );
        InterfaceDb.setDbTag(EDBTag.is_db);
        InterfaceDb.setTaskIndex(ENPInterfaceServerAsynEnum.IS_DB.ordinal());


        try
        {
            if (!InterfaceDb.initDBCon())
            {
                ALServerLog.Fatal("NPInterfaceDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to InterfaceDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(InterfaceDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.is_db);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.is_db got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPInterfaceServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("ISDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("ISDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
