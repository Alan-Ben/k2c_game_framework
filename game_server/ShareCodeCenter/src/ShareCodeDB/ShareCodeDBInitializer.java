package ShareCodeDB;

import ALMySqlCommon.ALMySqlSafeOp._TALMySqlSafeOpDBObj;
import ALServerLog.ALServerLog;
import NPCommon.DB.Version.WCGDBVersionChecker;
import NPCommon.DB.WCGDBFactory;
import NPCommon.DB.WCGDBObj;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.Log.CommLog;
import ShareCodeCenter.EShareCodeServerAsynEnum;
import ShareCodeCenter.ShareCodeCenter;

public class ShareCodeDBInitializer
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
        return EDBTag.scc_db;
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
            ALServerLog.Fatal("Don't try to init ShareCodeDBConnector twice!");
            return false;
        }

        //初始化数据库部分
        CommLog.sys("Start Init ShareCode DB ...");
        if (!ShareCodeDBConf.getInstance().init())
        {
            CommLog.fatal("ShareCodeDBConf Init Fail!");
            return false;
        }

        WCGDBObj ShareCodeDb = new WCGDBObj(
                ShareCodeDBConf.getInstance().getShareCodeDBHost(),
                ShareCodeDBConf.getInstance().getShareCodeDBName(),
                ShareCodeDBConf.getInstance().getShareCodeDBUser(),
                ShareCodeDBConf.getInstance().getShareCodeDBPass(),
                ShareCodeDBConf.getInstance().getShareCodeDBSafeSavePath()
        );

        ShareCodeDb.setDbTag(getDBTag());
        ShareCodeDb.setTaskIndex(EShareCodeServerAsynEnum.SHARE_CODE_DB.ordinal());

        try
        {
            if (!ShareCodeDb.initDBCon())
            {
                ALServerLog.Fatal("ShareCodeDB Init Fail!");
                return false;
            }
        } catch (Exception e)
        {
            CommLog.error("Connect to ShareCodeDB failed", e);
            return false;
        }

        return WCGDBFactory.regDBObj(ShareCodeDb);
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
        WCGDBVersionChecker checker = new WCGDBVersionChecker(ShareCodeCenter.getInstance().getBM(), tmpObj.getDB());
        checker.checkAndUpdateVersion("ShareCodeDB.Bo");
        checker.setNewestVersion(_g_curVersion);
        if (!checker.runAutoVersionUpdate("ShareCodeDB.Update"))
        {
            CommLog.error("name:[{}],tag:[{}]!!!!!!!!! 数据库版本升级失败 !!!!!!!!!!", tmpObj.getDB().getDBName(), tmpObj.getDB().getDbTag());
            return false;
        }
        return true;
    }
}
