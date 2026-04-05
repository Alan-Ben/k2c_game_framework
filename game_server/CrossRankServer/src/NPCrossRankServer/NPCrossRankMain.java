package NPCrossRankServer;


import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;

public class NPCrossRankMain
{
    /**************
     * 主入口函数
     * @param args
     */
    public static void main(String[] args)
    {
        //初始化日志输出
        ALServerLog.initALServerLog();

        //初始化服务器配置
        if (!CrossRankServerConf.getInstance().init())
        {
            ALServerLog.Fatal("WCG Cross-Rank ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(CrossRankServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + CrossRankServerConf.getInstance().getTimeZone());

        //开启服务器
        NPCrossRankServer serverObj = new NPCrossRankServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , EWCGCrossRankServerAsynEnum.values().length
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "CrossRankServer");
    }
}
