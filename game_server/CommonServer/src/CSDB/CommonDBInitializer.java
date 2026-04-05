package CSDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;
import NPCommonServer.EWCGCommonServerAsynEnum;
import NPCommonServer.NPCommonServer;

public class CommonDBInitializer
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
            ALServerLog.Fatal("Don't try to init CommonDBInitializer twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init CommonServer DB ...");
        if (!CommonDBConf.getInstance().init())
        {
            CommLog.fatal("NPCommonDBConf Init Fail!");
            return false;
        }

        WCGDBObj db = new WCGDBObj(
                CommonDBConf.getInstance().getDBHost(),
                CommonDBConf.getInstance().getDBName(),
                CommonDBConf.getInstance().getDBUser(),
                CommonDBConf.getInstance().getDBPass(),
                CommonDBConf.getInstance().getCommonDBSafeSavePath()
        );
        db.setDbTag(NPCommonEnum.EDBTag.comm_main);
        db.setTaskIndex(EWCGCommonServerAsynEnum.COMMON_DB.ordinal());


        try
        {
            if (!db.initDBCon())
            {
                ALServerLog.Fatal("NPCommDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to DB Failed!", e);
            return false;
        }


        return WCGDBFactory.regDBObj(db);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = null;
        for (int i = 0; i < WCGDBFactory.getDBObjects().size(); i++)
        {
            tmpObj = WCGDBFactory.getDBObjects().get(i);
            if (null == tmpObj)
                continue;

            WCGDBVersionChecker checker = new WCGDBVersionChecker(NPCommonServer.getInstance().getBM(), tmpObj.getDB());
            checker.checkAndUpdateVersion("CSDB.Bo");
            checker.setNewestVersion(_g_curVersion);
            if (!checker.runAutoVersionUpdate("CSDB.Update"))
            {
                CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
                return false;
            }
        }
        return true;
    }

}
