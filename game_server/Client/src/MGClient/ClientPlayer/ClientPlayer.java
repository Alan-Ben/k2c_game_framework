package MGClient.ClientPlayer;

import ALBasicProtocolPack._IALProtocolStructure;
import GC2GS.p004_PlayerOp.GC2GS_004_002_ReqGmCommand;
import GS2GC.p004_PlayerOp.GS2GC_004_002_RetGmCommand;
import MGClient.ClientPlayer.PlayerComp.Comps.ClientBagComp;
import MGClient.ClientPlayer.PlayerComp.Comps.ClientCurrencyComp;
import MGClient.ClientPlayer.PlayerComp.Comps.ClientPlayerComp;
import MGClient.ClientPlayer.PlayerComp.WCGClientCompMgr;
import MGClient.ClientPlayer.PlayerComp._AClientComponentBase;
import MGClient.ClientRequestMgr.ClientRequestMgr;
import MGClient.ClientRequestMgr._AClientRequestHandler;
import MGClient.ClientRequestMgr._IClientRequestHandler;
import MGClient.Cmd.CommandMgr;
import MGClient.GSListener.GSListener;
import MGClient.LSListener.ENPLSGameClientType;
import MGClient.LSListener.LSListener;
import MGClient.Logic.LogicBase;
import MGClient.Logic.LogicObjs.LogicFactory;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.ADelegateNone;
import NPCommon.Util.Delegate.ADelegateOne;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerParam;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_020_ReconnectInfo;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_022_SendSerializeMsg;
import WCGCommon.Enum.NPEnum;

import java.net.InetAddress;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

public class ClientPlayer
{

    public ADelegateOne<ENPPlayerParam> OnParamUpdateOne = new ADelegateOne<>(this);
    public ADelegateNone OnParamsInited = new ADelegateNone(this);

    public Long getParam(ENPPlayerParam _eParam)
    {
        return 0L;
    }

    public enum EClientPlayerType
    {
        none,
        normal,
        login_test,
        enter_game_test,
    }

    private long _m_cid;
    private String _m_name;
    private GSListener _m_gateListener;
    private CommandMgr _m_CommandMgr = new CommandMgr();
    private LogicBase _m_logic = null;
    private EClientPlayerType _m_ePlayerType = EClientPlayerType.normal;

    public ADelegateOne<String> onProtoArrived = new ADelegateOne<>(this);
    public ADelegateNone OnLoginGame = new ADelegateNone(this);
    public ADelegateNone OnLsDisconnected = new ADelegateNone(this);//LS 断开连接触发
    public ADelegateNone OnLsConnectFailed = new ADelegateNone(this);//LS 连接失败触发
    public ADelegateNone OnLsLoginSucc = new ADelegateNone(this);//LS 断开连接触发

    public ADelegateOne<Boolean> OnReceiveGateInfo = new ADelegateOne<>(this);//从 LS收到Gate服务器信息

    public ADelegateOne<ClientPlayer> OnPlayerOffLine = new ADelegateOne<>(this);

    private WCGClientCompMgr _m_compMgr = new WCGClientCompMgr();


    private ClientPlayerComp _m_comPlayer = null;
    private ClientBagComp _m_compBag = null;
    private ClientCurrencyComp _m_compCurrency = null;

    private boolean _m_bOnline = false;
    private String _m_accountId;

    public ClientPlayerComp getCompPlayer()
    {
        return _m_comPlayer;
    }

    public ClientBagComp getCompBag()
    {
        return _m_compBag;
    }

    public ClientCurrencyComp getCompCurrency()
    {
        return _m_compCurrency;
    }

    public GSListener getGateListener()
    {
        return _m_gateListener;
    }

    public ClientPlayer(String _name, String _password)
    {
        _m_name = _name;
        _m_comPlayer = new ClientPlayerComp(this);
        _m_compBag = new ClientBagComp(this);
        _m_compCurrency = new ClientCurrencyComp(this);
    }

    public void setPlayerType(EClientPlayerType _eType)
    {
        _m_ePlayerType = _eType;
    }

    public EClientPlayerType getPlayerType()
    {
        return _m_ePlayerType;
    }

    public void login(String serverIp, int _port)
    {
        if (!CommonFunc.checkIp(serverIp))
        {
            String hostName = serverIp;
            try
            {
                InetAddress addr = InetAddress.getByName(hostName);//根据域名创建主机地址对象
                serverIp = addr.getHostAddress();//获取主机IP地址
                CommLog.info("域名为：" + hostName + "的主机IP地址：" + serverIp);
            } catch (Exception e)
            {
                CommLog.info("不能根据域名获得主机IP地址：" + e.getMessage());
                return;
            }
        }
        CommLog.info("player:{} start login to server:{} port:{}", getName(), serverIp, _port);
        LSListener listener = new LSListener(serverIp, _port);
        listener.setOwner(this);
        listener.login(ENPLSGameClientType.USER.ordinal(), _m_name, "1", "EnterGame");
    }

    public void setCid(long cid)
    {
        _m_cid = cid;
    }

    public long getCid()
    {
        return _m_cid;
    }

    public void onReceiveGateServerInfo(String _uid, String gateServerIp, int gateServerPort, String checkCode)
    {
        CommLog.info("connecting to gate server:" + gateServerIp + " port:" + gateServerPort);

        if (!CommonFunc.checkIp(gateServerIp))
        {
            String hostName = gateServerIp;
            try
            {
                InetAddress addr = InetAddress.getByName(hostName);//根据域名创建主机地址对象
                gateServerIp = addr.getHostAddress();//获取主机IP地址
                CommLog.info("域名为：" + hostName + "的主机IP地址：" + gateServerIp);
            } catch (Exception e)
            {
                CommLog.info("不能根据域名获得主机IP地址：" + e.getMessage());
                return;
            }
        }

        _m_gateListener = new GSListener(gateServerIp, gateServerPort);
        _m_gateListener.login(NPEnum.EClientGSLoginType.NORMAL.ordinal(), String.valueOf(_uid), checkCode);
        _m_gateListener.setOwner(this);
    }

    public void setAccountId(String _accountId)
    {
        _m_accountId = _accountId;
    }

    public void sendGameMsg(_IALProtocolStructure _protocolObj)
    {
        NPGC2GS_001_022_SendSerializeMsg proto = new NPGC2GS_001_022_SendSerializeMsg();
        proto.setMsg(_protocolObj.makeFullPackage());
        proto.setMsgSerialize(this.incMsgCount());
        proto.setClientRequestSerialize(0);
        this._m_gateListener.send(proto);
    }

    public void request(_IALProtocolStructure _request, _IClientRequestHandler _handler)
    {
        _handler.setCaller(this);
        long serial = ClientRequestMgr.getInstance().regRequest(_request, _handler);
        NPGC2GS_001_022_SendSerializeMsg proto = new NPGC2GS_001_022_SendSerializeMsg();
        proto.setMsg(_request.makeFullPackage());
        proto.setMsgSerialize(this.incMsgCount());
        proto.setClientRequestSerialize(serial);
        this._m_gateListener.send(proto);
    }


    public String getAccountId()
    {
        return _m_accountId;
    }

    public void onLoginGame()
    {

        _m_CommandMgr.init(this);

        NPGC2GS_001_020_ReconnectInfo protocol = new NPGC2GS_001_020_ReconnectInfo();
        protocol.setCurRecMesCount(0);
        _m_gateListener.send(protocol);

        setIsOnline(true);
        for (String comand : _m_postLoginCommands)
        {
            CommLog.info(runCmd(comand));
        }
        OnLoginGame.onEvent();

    }

    public String getName()
    {
        return this._m_name;
    }

    public String runCmd(String cmd)
    {
        CommLog.info("run command:" + cmd);
        return _m_CommandMgr.run(cmd);
    }

    public void runGm(String gmCommand)
    {
        CommLog.info("run server gm command:" + gmCommand);
        GC2GS_004_002_ReqGmCommand msg = new GC2GS_004_002_ReqGmCommand();
        msg.setCommand(gmCommand);
        request(msg, new _AClientRequestHandler<GS2GC_004_002_RetGmCommand>(GS2GC_004_002_RetGmCommand.class)
        {
            @Override
            public void handle(GS2GC_004_002_RetGmCommand _response)
            {
                //logProto(_response.getResult());
            }
        });
    }

    public void getCmdList(String _cmd,_ICallBackT<String[]> _callback)
    {
        GC2GS_004_002_ReqGmCommand msg = new GC2GS_004_002_ReqGmCommand();
        msg.setCommand(_cmd);
        request(msg, new _AClientRequestHandler<GS2GC_004_002_RetGmCommand>(GS2GC_004_002_RetGmCommand.class)
        {
            @Override
            public void handle(GS2GC_004_002_RetGmCommand _response)
            {
                String[] split = _response.getResult().trim().split("\n");
                _callback.onRunOver(split);
            }
        });
    }



    public void logProto(String msg)
    {
        onProtoArrived.onAsyncEvent(msg);

    }

    public void changeLogic(String logicName)
    {
        if (null != _m_logic)
        {
            _m_logic.stop();
        }
        _m_logic = LogicFactory.getInstance().createLogic(logicName);
        _m_logic.Init(this);
        _m_logic.startRun();
    }

    public void registComp(_AClientComponentBase _clientComponentBase)
    {
        _m_compMgr.registComp(_clientComponentBase);
    }

    public void logout()
    {
        _m_gateListener.logout();
    }

    public boolean getIsOnline()
    {
        return _m_bOnline;
    }

    public void setIsOnline(boolean b)
    {
        _m_bOnline = b;
    }

    private ArrayList<String> _m_postLoginCommands = new ArrayList<>();

    public void addLoginCommands(String... commands)
    {
        _m_postLoginCommands.addAll(Arrays.asList(commands));
    }

    public ArrayList<String> getPostLoginCommands()
    {
        return _m_postLoginCommands;
    }

    public void stopCurLogic()
    {
        if (null != _m_logic)
        {
            _m_logic.stop();
            _m_logic = null;
        }
    }


    private int _m_msgCount = 0;

    private int incMsgCount()
    {
        _m_msgCount++;
        return _m_msgCount;
    }


    public void onGetRecentServer(LSListener dealer, String _serverTag)
    {

    }

    public void onKicked()
    {
        setIsOnline(false);
        OnPlayerOffLine.onEvent(this);
        if (null != _m_logic)
        {
            _m_logic.stop();
            _m_logic = null;
        }
    }

    private Map<Class<?>, Object> _m_mDelegateMap = new ConcurrentHashMap<>();

    public <T extends _IALProtocolStructure> void onReceiveMsg(T _msg)
    {
        @SuppressWarnings("unchecked")
        ADelegateOne<T> delegateOne = (ADelegateOne<T>) _m_mDelegateMap.get(_msg.getClass());
        if (null == delegateOne)
        {
            delegateOne = new ADelegateOne<T>(this);
            _m_mDelegateMap.put(_msg.getClass(), delegateOne);
        }
        delegateOne.onEvent(_msg);
    }

    public <T extends _IALProtocolStructure> void addMsgHandler(_IHandlerHolder _holder, Class<T> _clazz, HandlerOne<T> _handler)
    {
        if (_clazz == null)
            return;
        @SuppressWarnings("unchecked")
        ADelegateOne<T> delegateOne = (ADelegateOne<T>) _m_mDelegateMap.get(_clazz);
        if (null == delegateOne)
        {
            delegateOne = new ADelegateOne<T>(this);
            _m_mDelegateMap.put(_clazz, delegateOne);
        }
        delegateOne.addHandler(_holder, _handler);


    }

    public <T extends _IALProtocolStructure> void removeMsgHandler(Class<T> _clazz, _IHandlerHolder _holder)
    {
        if (_clazz == null)
            return;
        @SuppressWarnings("unchecked")
        ADelegateOne<T> delegateOne = (ADelegateOne<T>) _m_mDelegateMap.get(_clazz);
        if (null == delegateOne)
        {
            return;
        }
        delegateOne.clear(_holder);

    }
}
