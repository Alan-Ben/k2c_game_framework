package CGSDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;
import NPCrossGameServer.EWCGCrossGameServerAsynEnum;
import NPCrossGameServer.NPCrossGameServer;

public class CrossGameDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPCrossGameDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Cross-Game DB ...");
        if (!CrossGameDBConf.getInstance().init())
        {
            CommLog.fatal("NPCrossGameDBConf Init Fail!");
            return false;
        }

        WCGDBObj userDb = new WCGDBObj(
                CrossGameDBConf.getInstance().getDBHost(),
                CrossGameDBConf.getInstance().getDBName(),
                CrossGameDBConf.getInstance().getDBUser(),
                CrossGameDBConf.getInstance().getDBPass(),
                CrossGameDBConf.getInstance().getDBSafeSavePath()
        );
        userDb.setDbTag(NPCommonEnum.EDBTag.crossgame_main);
        userDb.setTaskIndex(EWCGCrossGameServerAsynEnum.MAIN_DB.ordinal());


        try
        {
            if (!userDb.initDBCon())
            {
                ALServerLog.Fatal("NPCrossGameDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to NPCrossGameDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(userDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.crossgame_main);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.main got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPCrossGameServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("CGSDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("CGSDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
