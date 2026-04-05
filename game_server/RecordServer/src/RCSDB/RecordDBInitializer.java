package RCSDB;


import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import NPRecordServer.ENPRecordAsynEnum;
import NPRecordServer.NPRecordServer;

public class RecordDBInitializer
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
            ALServerLog.Fatal("Don't try to init NPRCSDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init Record DB ...");
        if (!RecordDBConf.getInstance().init())
        {
            CommLog.fatal("NPRSDBConf Init Fail!");
            return false;
        }

        WCGDBObj RecordDB = new WCGDBObj(
                RecordDBConf.getInstance().getRecordDBHost(),
                RecordDBConf.getInstance().getRecordDBName(),
                RecordDBConf.getInstance().getRecordDBUser(),
                RecordDBConf.getInstance().getRecordDBPass(),
                RecordDBConf.getInstance().getRecordDBSafeSavePath()
        );
        RecordDB.setDbTag(EDBTag.rcs_db);
        RecordDB.setTaskIndex(ENPRecordAsynEnum.RCS_DB.ordinal());


        try
        {
            if (!RecordDB.initDBCon())
            {
                ALServerLog.Fatal("NPRecordDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to RecordDB failed", e);
            return false;
        }


        return WCGDBFactory.regDBObj(RecordDB);
    }

    /**
     * 初始化数据库内容
     ***/
    public static boolean initDB()
    {
        _TALMySqlSafeOpDBObj<WCGDBObj> tmpObj = WCGDBFactory.getDbObj(NPCommonEnum.EDBTag.rcs_db);
        if (null == tmpObj)
        {
            CommLog.error("EDBTag.rcs_db got null");
            return false;
        }
        WCGDBVersionChecker checker = new WCGDBVersionChecker(NPRecordServer.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("RCSDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("RCSDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
