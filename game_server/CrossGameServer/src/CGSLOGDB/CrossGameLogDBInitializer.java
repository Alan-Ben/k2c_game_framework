package CGSLOGDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;
import NPCrossGameServer.EWCGCrossGameServerAsynEnum;
import NPCrossGameServer.NPCrossGameServer;

public class CrossGameLogDBInitializer
{
    /**
     * 是否初始化
     */
    private static boolean _m_bInit = false;

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
            ALServerLog.Fatal("Don't try to init NPCrossGameLogDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Cross-Game LogDB ...");
        if (!CrossGameLogDBConf.getInstance().init())
        {
            CommLog.fatal("NPCrossGameLogDBConf Init Fail!");
            return false;
        }

        WCGDBObj userDb = new WCGDBObj(
                CrossGameLogDBConf.getInstance().getDBHost(),
                CrossGameLogDBConf.getInstance().getDBName(),
                CrossGameLogDBConf.getInstance().getDBUser(),
                CrossGameLogDBConf.getInstance().getDBPass(),
                CrossGameLogDBConf.getInstance().getDBSafeSavePath()
        );
        userDb.setDbTag(NPCommonEnum.EDBTag.crossgame_log);
        userDb.setTaskIndex(EWCGCrossGameServerAsynEnum.LOG_DB.ordinal());


        try
        {
            if (!userDb.initDBCon())
            {
                ALServerLog.Fatal("NPCrossGameLogDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to NPCrossGameLogDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(userDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.crossgame_log);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.main got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPCrossGameServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("CGSLOGDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("CGSLOGDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
