package LCSDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Log.CommLog;
import NPLoginCheckServer.ENPLoginCheckServerAsynEnum;
import NPLoginCheckServer.NPLoginCheckServer;

public class LoginCheckDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPUSDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init LoginCheckServer DB ...");
        if (!LoginCheckDBConf.getInstance().init())
        {
            CommLog.fatal("NPAccDBConf Init Fail!");
            return false;
        }

        WCGDBObj db = new WCGDBObj(
                LoginCheckDBConf.getInstance().getAccDBHost(),
                LoginCheckDBConf.getInstance().getAccDBName(),
                LoginCheckDBConf.getInstance().getAccDBUser(),
                LoginCheckDBConf.getInstance().getAccDBPass(),
                LoginCheckDBConf.getInstance().getAccDBSafeSavePath()
        );
        db.setDbTag(NPCommonEnum.EDBTag.account_db);
        db.setTaskIndex(ENPLoginCheckServerAsynEnum.ACC_CHECK.ordinal());


        try
        {
            if (!db.initDBCon())
            {
                ALServerLog.Fatal("NPAccountDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to NPAccountDB Failed!", e);
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

            WCGDBVersionChecker checker = new WCGDBVersionChecker(NPLoginCheckServer.getInstance().getBM(), tmpObj.getDB());
            checker.checkAndUpdateVersion("LCSDB.Bo");
            checker.setNewestVersion(_g_curVersion);
            if (!checker.runAutoVersionUpdate("LCSDB.Update"))
            {
                CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
                return false;
            }
        }
        return true;
    }

}
