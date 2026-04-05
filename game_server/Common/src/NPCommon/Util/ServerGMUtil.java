package NPCommon.Util;

import ALBasicServer.ALServerCmd.ALCmdDealerManager;
import ALBasicServer.ALServerCmd._IALBasicServerCmdDealer;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALServerLog.ALServerLog;
import AllRpcData.All_Service.AllExecGmCommand;
import NPCommon.DDAlert.DDAlert;
import NPCommon.DDAlert.SynTask_MonitorAlert;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.GMCommand.DefualtCmds.CmdSystem;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.GitNode.NPGitNode;
import NPCommon.HotLoad.HotLoadHelper;
import NPCommon.Log.CommLog;
import NPCommon.Util.Delegate.HandlerTwo;
import NPEnum.ENPGameEvent;
import RPC.RPCDataFactoryMgr;
import RPC.RpcSender;
import RPC._ARpcCallBack;
import WCGBasicServer._AWCGBasicServer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class ServerGMUtil
{
    //声明一个默认执行的服务器对象，用于执行一些无法指派Cid的GM命令
    protected static _AWCGBasicServer _g_defaultExeServer = null;
    public static void initDefaultExeServer(_AWCGBasicServer _server)
    {
        _g_defaultExeServer = _server;
    }
    public static _AWCGBasicServer getDefaltExeServer()
    {
        return _g_defaultExeServer;
    }

    public static void initServer(_ABasicServerObj _defaultExeServer, String _serverName)
    {
        initServer(_defaultExeServer, _defaultExeServer.getDDAlert(), _serverName);
    }
    public static void initServer(_AWCGBasicServer _defaultExeServer, DDAlert _ddAlert, String _serverName)
    {
        System.out.printf("[%s] started, pid = %d\n", _serverName, CommonFunc.getPid());
        System.out.println("working dir:"+System.getProperty("user.dir"));

        _g_defaultExeServer = _defaultExeServer;

        //初始化错误码
        ResultMgr.getInstance().registByPackage(CommErr.class.getPackage().getName());

        ALServerLog.initALServerLog();
        CommonFunc.printVersion(); //打印版本信息
        NPGitNode.PrintGitNodeInfo(); //打印Git信息
        if (!RPCDataFactoryMgr.getInstance().Init())
        {
            System.exit(0);
        }
        if(!HotLoadHelper.checkEvn())
        {
            System.exit(0);
        }

        //确保服务器单例执行
        SingleApplication.makeSingle(_serverName);

        //初始化GM命令
        //注册默认的GM命令
        GmCommandMgr.getInstance().init(CmdSystem.class.getPackage().getName());
        //注册GameRes的GM命令
        GmCommandMgr.getInstance().initByClassName("NPGameRes.GmCmds.ResGmCommandHolder");

        //开启服务器内存检测，如对象为空则报错，不开启任务
        if(null != _ddAlert)
        {
            ALSynTaskManager.getInstance().regTask(new SynTask_MonitorAlert(_ddAlert));
        }
        else
        {
            ALServerLog.Sys("DDAlert is null, skip memory monitor task");
        }

        //注册命令行输入
        ALCmdDealerManager.getInstance().regDealer(new _IALBasicServerCmdDealer()
        {
            @Override
            public void dealCmd(String paramString)
            {
                if (routeGmCommand(paramString, new HandlerTwo<Boolean, String>()
                {
                    @Override
                    public void handle(Boolean _bSucc, String _result)
                    {
                        CommLog.info("[result]:{},[msg]:{}", _bSucc, _result);
                    }
                }))
                {
                    //已经路由到其它服务器执行了
                } else
                {
                    NPCmdLineContext context = NPCmdLineContext.createNew(ENPGameEvent.GM_CMD);
                    GmCommandMgr.getInstance().run(null, paramString, context, (_bSucc, _result) -> CommLog.info("[result]:{},[msg]:{}", _bSucc, _result));
                }
            }
        });
    }

    /******
     * 把GM命令路由到其它服务器去执行
     * @param _command
     * @param _handler
     * @return
     */
    public static boolean routeGmCommand(String _command, HandlerTwo<Boolean, String> _handler)
    {
        return routeGmCommand(_g_defaultExeServer, _command, _handler);
    }
    public static boolean routeGmCommand(_AWCGBasicServer _server, String _command, HandlerTwo<Boolean, String> _handler)
    {
        String[] strs = CommonFunc.charSplit(_command, ' ', 3);
        if (strs.length < 2)
        {
            return false;
        }
        EServerType eServerType = null;
        int typeId = 0;
        String command = "";
        if (strs[0].compareToIgnoreCase("ps") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.NONE.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        }
        else if (strs[0].compareToIgnoreCase("lcs") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.LOGIN_CHECK.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        }
        else if (strs[0].compareToIgnoreCase("cs") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.COMMON.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];

        } 
        else if (strs[0].compareToIgnoreCase("hs") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.HTTP.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        }
        else if (strs[0].compareToIgnoreCase("is") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.INTERFACE.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        }
        else if (strs[0].compareToIgnoreCase("rcs") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.RECORD.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        } 
        else if (strs[0].compareToIgnoreCase("ss") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.SCHEDULE.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        } 
        else if (strs[0].compareToIgnoreCase("mms") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.MARRY_MATCH.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        } 
        else if (strs[0].compareToIgnoreCase("dns") == 0)
        {
            eServerType = EServerType.SINGLE;
            typeId = ENPSingleServerType.DINNER.ordinal();
            command = strs[1];
            if (strs.length > 2) command += " " + strs[2];
        }
        //////////////////////////////////////////////////////////////// 多服分界线 ////////////////////////////////////////////////////////////////
        else if (strs[0].compareToIgnoreCase("ls") == 0)
        {
            eServerType = EServerType.LOGIN;
            typeId = Integer.valueOf(strs[1]);
            if (strs.length > 2) command = strs[2];
        } 
        else if (strs[0].compareToIgnoreCase("us") == 0)
        {
            eServerType = EServerType.USER;
            typeId = Integer.valueOf(strs[1]);
            if (strs.length > 2) command = strs[2];
        }
        else if (strs[0].compareToIgnoreCase("gs") == 0)
        {
            eServerType = EServerType.GATE;
            typeId = Integer.valueOf(strs[1]);
            if (strs.length > 2) command = strs[2];
        }  
        else if (strs[0].compareToIgnoreCase("cgs") == 0)
        {
            eServerType = EServerType.CROSS_GAME;
            typeId = Integer.valueOf(strs[1]);
            if (strs.length > 2) command = strs[2];
        }
        else if (strs[0].compareToIgnoreCase("crs") == 0)
        {
            eServerType = EServerType.CROSS_RANK;
            typeId = Integer.valueOf(strs[1]);
            if (strs.length > 2) command = strs[2];
        }
        
        if (null == eServerType)
        {
            return false;
        }

        //路由GM命令到指定服务器
        routeGMCommandDistinct(_server, eServerType, typeId, command, _handler);

        return true;
    }

    /**
     * 路由GM命令到指定服务器
     * @param _serverType
     * @param _typeId
     * @param _command
     * @param _handler
     */
    public static void routeGMCommandDistinct(_AWCGBasicServer _server, EServerType _serverType, int _typeId, String _command, HandlerTwo<Boolean, String> _handler)
    {
        AllExecGmCommand rpc = new AllExecGmCommand();
        if (_command.length() < 10240)
        {
            rpc.req().setCommand(_command);
        } else
        {
            rpc.req().getExCommands().addAll(CommonFunc.splitStringByLength(_command, 10240));
        }

        RpcSender sender = new RpcSender(_server, _serverType, _typeId);
        sender.requestTo(_typeId, rpc, new _ARpcCallBack<AllExecGmCommand>()
        {
            @Override
            public void call_back(int _errCode, AllExecGmCommand _rpc)
            {
                if (_errCode != 0)
                {
                    if (null != _handler)
                    {
                        _handler.handle(false, "rpc call failed,err:" + _errCode);
                    }
                    return;
                }

                String result = StringFunc.joinString(rpc.retObj().getResultList());
                if (null != _handler)
                {
                    _handler.handle(_rpc.retObj().getIsSucc(), result);
                }
            }
        });
    }


}
