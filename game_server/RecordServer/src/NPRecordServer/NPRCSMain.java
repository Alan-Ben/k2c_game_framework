package NPRecordServer;

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
public class NPRCSMain
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
        if (!RecordServerConf.getInstance().init())
        {
            ALServerLog.Fatal("NP Record ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(RecordServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + RecordServerConf.getInstance().getTimeZone());

        //开启服务器
        NPRecordServer serverObj = new NPRecordServer();
        serverObj.startServer(NPVersion.majorVersion(), NPVersion.minorVersion(),
                NPVersion.buildVersion(), ENPRecordAsynEnum.values().length, new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, "RecordServer");
    }
}
