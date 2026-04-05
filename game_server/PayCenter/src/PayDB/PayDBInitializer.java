package PayDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import PayCenter.EPayServerAsynEnum;
import PayCenter.PayCenter;

public class PayDBInitializer
{
    /**
     * 是否初始化
     */
    private static final boolean _m_bInit = false;
    /**
     * 当前版本
     ***/
    public static String _g_curVersion = "1.0.0.1";
    /**
     * 获取数据库标签
     */
    public static EDBTag getDBTag()
    {
        return EDBTag.pc_db;
    }

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
            ALServerLog.Fatal("Don't try to init PayDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Pay DB ...");
        if (!PayDBConf.getInstance().init())
        {
            CommLog.fatal("PayDBConf Init Fail!");
            return false;
        }

        WCGDBObj PayDb = new WCGDBObj(
                PayDBConf.getInstance().getPayDBHost(),
                PayDBConf.getInstance().getPayDBName(),
                PayDBConf.getInstance().getPayDBUser(),
                PayDBConf.getInstance().getPayDBPass(),
                PayDBConf.getInstance().getPayDBSafeSavePath()
        );

        PayDb.setDbTag(getDBTag());
        PayDb.setTaskIndex(EPayServerAsynEnum.PAY_DB.ordinal());

        try
        {
            if (!PayDb.initDBCon())
            {
                ALServerLog.Fatal("PayDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to PayDB failed", e);
            return false;
        }

        return WCGDBFactory.regDBObj(PayDb);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(getDBTag());
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.share_code_db got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(PayCenter.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("PayDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("PayDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
