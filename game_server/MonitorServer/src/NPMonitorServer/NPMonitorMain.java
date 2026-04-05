package NPMonitorServer;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import ALServerLog.ALServerLog;
import NPCommon.Log.CommLog;
import NPCommon.NPVersion;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.CommonServerStartMonitor;
import NPCommon.Util.ServerGMUtil;
import NPMonitorServer.TestCallback.TestGetAllServerListCallback;
import NPMonitorServer.TestCallback.TestGetServerInfoCallback;
import WCGBasicMonitorServer.BasicServerListener.Writer.WCGMonitor2BS_R_Writer_001_MonitorOp;
import WCGBasicMonitorServer._AWCGBasicMonitorServer;
import WCGMonitor2Plat.p000_R_BasicOp.WCGMonitor2Plat_R_000_001_ReqAllServerList;

public class NPMonitorMain
{
    /**************
     * 主入口函数
     *
     * @param args
     */
    public static void main(String[] args)
    {
        //初始化日志输出
        ALServerLog.initALServerLog();

        //初始化服务器配置
        if (!MonitorServerConf.getInstance().init())
        {
            ALServerLog.Fatal("NP Monitor ServerConf Init Fail!!!");
            return;
        }

        //设置时区
        CommonFunc.setTimeZone(MonitorServerConf.getInstance().getTimeZone());
        CommLog.info("Set Time Zone:" + MonitorServerConf.getInstance().getTimeZone());

        // 开启服务器
        NPMonitorServer serverObj = new NPMonitorServer();

        //开启服务器
        _AWCGBasicMonitorServer.startMonitorServer(
                NPVersion.majorVersion(), NPVersion.minorVersion(), NPVersion.buildVersion()
                , ENPMonitorServerAsynEnum.values().length
                , serverObj
                , new NPMonitorVerifyObj()
                , new CommonServerStartMonitor());

        ServerGMUtil.initServer(serverObj, serverObj.getDDAlert(), "MonitorServer");

        ALSynTaskManager.getInstance().regTask(new _IALSynTask()
        {

            @Override
            public void run()
            {
                ALServerLog.Sys("start deal - ");

                //测试发送消息到总服务器
                _AWCGBasicMonitorServer.getInstance().sendRequestToPlat(
                        new WCGMonitor2Plat_R_000_001_ReqAllServerList()        //此协议不需要参数所以直接构造发送
                        , new TestGetAllServerListCallback());

                //测试请求服务器信息
                int type = 1;
                int typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 1;
                typeId = 2;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 1;
                typeId = 3;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 1;
                typeId = 4;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 1;
                typeId = 5;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 2;
                typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 3;
                typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 4;
                typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 5;
                typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 6;
                typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 9999;
                typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                type = 10000;
                typeId = 1;
                _AWCGBasicMonitorServer.getInstance().sendRequestToBSServer(
                        type
                        , typeId
                        , WCGMonitor2BS_R_Writer_001_MonitorOp.make_001_GetServerInfo("aa")
                        , new TestGetServerInfoCallback(type, typeId));

                ALServerLog.Sys("end deal - ");
            }
        }, 3000);
    }
}
