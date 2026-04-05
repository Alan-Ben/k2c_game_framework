package NPHttpServer;


import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;
import WCGBasicServer.WCGBasicServerListenPortInfo;

import java.util.ArrayList;

/***************
 * 登录服务器的入口类对象
 * @author Administrator
 *
 */
public class NPHSMain
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
        if (!HttpServerConf.getInstance().init())
        {
            ALServerLog.Fatal("NP Http ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(HttpServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + HttpServerConf.getInstance().getTimeZone());

        //构造监听端口队列
        ArrayList<WCGBasicServerListenPortInfo> portList = new ArrayList<>();
        //加入外网监听端口
        portList.add(new WCGBasicServerListenPortInfo(HttpServerConf.getInstance().getClientConnectPort()
                , HttpServerConf.getInstance().getClientSocketCacheSize()
                , new NPClientConnectVerifyObj()));

        //开启服务器
        NPHttpServer serverObj = new NPHttpServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , ENPHttpServerAsynEnum.values().length
                , portList
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "HttpServer");
    }
}
