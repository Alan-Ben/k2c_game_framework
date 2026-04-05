package SSLOGDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;
import NPScheduleServer.ENPScheduleServerAsynEnum;
import NPScheduleServer.NPScheduleServer;

public class ScheduleLogDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPSSLOGDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Schedule Log DB ...");
        if (!ScheduleLogDBConf.getInstance().init())
        {
            CommLog.fatal("NPSSLogDBConf Init Fail!");
            return false;
        }

        WCGDBObj scheduleDb = new WCGDBObj(
                ScheduleLogDBConf.getInstance().getScheduleDBHost(),
                ScheduleLogDBConf.getInstance().getScheduleDBName(),
                ScheduleLogDBConf.getInstance().getScheduleDBUser(),
                ScheduleLogDBConf.getInstance().getScheduleDBPass(),
                ScheduleLogDBConf.getInstance().getScheduleDBSafeSavePath()
        );
        scheduleDb.setDbTag(NPCommonEnum.EDBTag.ss_log);
        scheduleDb.setTaskIndex(ENPScheduleServerAsynEnum.SS_LOG_DB.ordinal());


        try
        {
            if (!scheduleDb.initDBCon())
            {
                ALServerLog.Fatal("NPScheduleLogDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to NPScheduleLogDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(scheduleDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.ss_log);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.us_log got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPScheduleServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("SSLOGDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("SSLOGDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
