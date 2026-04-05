package NPCrossGameServer;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;

public class NPCrossGameMain
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
        if (!CrossGameServerConf.getInstance().init())
        {
            ALServerLog.Fatal("WCG Cross-Game ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(CrossGameServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + CrossGameServerConf.getInstance().getTimeZone());

        //开启服务器
        NPCrossGameServer serverObj = new NPCrossGameServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , EWCGCrossGameServerAsynEnum.values().length
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "CrossGameServer");
    }
}
