package MarryMatchServer;

import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;

/***************
 * 登录服务器的入口类对象
 * @author Administrator
 *
 */
public class MarryMatchMain
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
        if (!MarryMatchServerConf.getInstance().init())
        {
            ALServerLog.Fatal("Marry Match ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(MarryMatchServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + MarryMatchServerConf.getInstance().getTimeZone());

        //开启服务器
        MarryMatchServer serverObj = new MarryMatchServer();
        serverObj.startServer(NPVersion.majorVersion(), NPVersion.minorVersion(),
                NPVersion.buildVersion(), EMarryMatchAsynEnum.values().length, new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "MarryMatchServer");
    }
}
