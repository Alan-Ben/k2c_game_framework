package CTSDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import CrossTeamServer.CrossTeamServer;
import CrossTeamServer.ECrossTeamServerAsynEnum;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;

public class CrossTeamDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPCrossTeamDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Cross-Team DB ...");
        if (!CrossTeamDBConf.getInstance().init())
        {
            CommLog.fatal("NPCrossTeamDBConf Init Fail!");
            return false;
        }

        WCGDBObj userDb = new WCGDBObj(
                CrossTeamDBConf.getInstance().getDBHost(),
                CrossTeamDBConf.getInstance().getDBName(),
                CrossTeamDBConf.getInstance().getDBUser(),
                CrossTeamDBConf.getInstance().getDBPass(),
                CrossTeamDBConf.getInstance().getDBSafeSavePath()
        );
        userDb.setDbTag(NPCommonEnum.EDBTag.crossteam_main);
        userDb.setTaskIndex(ECrossTeamServerAsynEnum.MAIN_DB.ordinal());

        try
        {
            if (!userDb.initDBCon())
            {
                ALServerLog.Fatal("NPCrossTeamDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to NPCrossTeamDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(userDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.crossteam_main);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.main got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(CrossTeamServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("CTSDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("CTSDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
