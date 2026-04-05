package SSDB;


import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import NPScheduleServer.ENPScheduleServerAsynEnum;
import NPScheduleServer.NPScheduleServer;

public class ScheduleDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPSSDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Schedule DB ...");
        if (!ScheduleDBConf.getInstance().init())
        {
            CommLog.fatal("NPSSDBConf Init Fail!");
            return false;
        }

        WCGDBObj ScheduleDb = new WCGDBObj(
                ScheduleDBConf.getInstance().getScheduleDBHost(),
                ScheduleDBConf.getInstance().getScheduleDBName(),
                ScheduleDBConf.getInstance().getScheduleDBUser(),
                ScheduleDBConf.getInstance().getScheduleDBPass(),
                ScheduleDBConf.getInstance().getScheduleDBSafeSavePath()
        );

        ScheduleDb.setDbTag(EDBTag.ss_db);
        ScheduleDb.setTaskIndex(ENPScheduleServerAsynEnum.SS_DB.ordinal());

        try
        {
            if (!ScheduleDb.initDBCon())
            {
                ALServerLog.Fatal("NPInterfaceDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to InterfaceDB failed", e);
            return false;
        }

        return WCGDBFactory.regDBObj(ScheduleDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.ss_db);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.SS_db got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPScheduleServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("SSDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("SSDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
