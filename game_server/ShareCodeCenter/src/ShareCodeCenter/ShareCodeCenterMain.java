package ShareCodeCenter;

import NPCommon.DB.BoChecker.BoChecker;
import NPCommon.Enum.NPCommonEnum.EDBTag;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;
import ShareCodeCenter.Conf.Server.ShareCodeServerConf;
import ShareCodeCenter.Conf.Server.ShareCodeServerConfMgr;
import ShareCodeCenter.Conf.ShareCodeCenterConf;
import ShareCodeCenter.ShareCodeServer.GMCommand.Cmds.CmdServer;
import ShareCodeCenter.ShareCodeServer.ShareCodeServer;

import java.util.List;

public class ShareCodeCenterMain
{
    /**
     * 主入口函数
     * @param args
     */
    public static void main(String[] args)
    {
        if (!ShareCodeCenter.getInstance().init())
        {
            CommLog.fatal("ShareCodeCenter Init Fail!!!");
            return;
        }

        List<ShareCodeServerConf> confList = ShareCodeServerConfMgr.getInstance().getConfList();
        for (ShareCodeServerConf conf : confList)
        {
            //输出日志
            CommLog.info("Start ShareCodeServer TypeId: " + conf.getId());

            ShareCodeServer server = new ShareCodeServer(conf);
            ShareCodeCenter.getInstance().initAddServer(server);

            //初始化配置
            ShareCodeCenterConf.getInstance().init();
            //设置时区
            CommonFunc.setTimeZone(ShareCodeCenterConf.getInstance().getTimeZone());
            CommLog.info("Set Time Zone:" + ShareCodeCenterConf.getInstance().getTimeZone());

            server.startServer(NPVersion.majorVersion()
                    , NPVersion.minorVersion()
                    , NPVersion.buildVersion()
                    , EShareCodeServerAsynEnum.values().length
                    , null
                    , conf
                    , new CommonServerStartMonitor());

            if (conf.getId() == 1)
            {
                ServerGMUtil.initServer(server, ShareCodeCenter.getInstance().getDDAlert(), "ShareCodeCenter");

                //数据库自动保存，只检测US主库
                BoChecker.getInstance().startCheck(_bo -> _bo.getDBTag() == EDBTag.scc_db);

                //初始化US的GM命令
                GmCommandMgr.getInstance().init(CmdServer.class.getPackage().getName());
            }
        }
    }
}
