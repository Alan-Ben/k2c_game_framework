package GameLogicServer;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;

public class GameLogicMain
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
        if (!GameLogicServerConf.getInstance().init())
        {
            ALServerLog.Fatal("WCG Game-Logic ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(GameLogicServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + GameLogicServerConf.getInstance().getTimeZone());

        //开启服务器
        GameLogicServer serverObj = new GameLogicServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , EGameLogicServerAsynEnum.values().length
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "GameLogicServer");
    }
}
