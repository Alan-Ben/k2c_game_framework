package NPLoginServer;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;
import NPLoginServer.VerifyObj.NPClientConnectVerifyObj;
import WCGBasicServer.WCGBasicServerListenPortInfo;

import java.util.ArrayList;

/***************
 * 登录服务器的入口类对象
 * @author Administrator
 *
 */
public class NPLSMain
{
    /**************
     * 主入口函数
     * @param args
     */
    public static void main(String[] args)
    {
        //初始化日志输出
        ALServerLog.initALServerLog();

        //初始化服务器基础配置
        if (!LoginServerConf.getInstance().init())
        {
            ALServerLog.Error("WCGLoginServerConf init fail!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(LoginServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + LoginServerConf.getInstance().getTimeZone());

        //构造监听端口队列
        ArrayList<WCGBasicServerListenPortInfo> portList = new ArrayList<>();
        //加入外网监听端口
        portList.add(new WCGBasicServerListenPortInfo(LoginServerConf.getInstance().getConnectPort()
                , LoginServerConf.getInstance().getClientSocketCacheSize()
                , new NPClientConnectVerifyObj()));

        //开启服务器
        NPLoginServer serverObj = new NPLoginServer();
        serverObj.startServer(NPVersion.majorVersion()
                , NPVersion.minorVersion()
                , NPVersion.buildVersion()
                , NPLoginServerAsynEnum.values().length
                , portList
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "LoginServer");
    }
}
