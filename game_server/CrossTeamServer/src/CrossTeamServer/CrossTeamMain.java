package CrossTeamServer;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;

public class CrossTeamMain
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
        if (!CrossTeamServerConf.getInstance().init())
        {
            ALServerLog.Fatal("WCG Cross-Team ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(CrossTeamServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + CrossTeamServerConf.getInstance().getTimeZone());

        //开启服务器
        CrossTeamServer serverObj = new CrossTeamServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , ECrossTeamServerAsynEnum.values().length
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "CrossTeamServer");
    }
}
