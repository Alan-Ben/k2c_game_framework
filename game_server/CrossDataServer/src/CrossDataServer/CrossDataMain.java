package CrossDataServer;


import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;

public class CrossDataMain
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
        if (!CrossDataServerConf.getInstance().init())
        {
            ALServerLog.Fatal("WCG Cross-Data ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(CrossDataServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + CrossDataServerConf.getInstance().getTimeZone());

        //开启服务器
        CrossDataServer serverObj = new CrossDataServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , ECrossDataServerAsynEnum.values().length
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "CrossDataServer");
    }
}
