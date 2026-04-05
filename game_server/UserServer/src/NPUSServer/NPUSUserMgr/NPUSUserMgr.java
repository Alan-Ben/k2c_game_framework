package NPUSServer.NPUSUserMgr;

import ALBasicCommon.ALBasicCommonFun;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import CommonEnum.EPlatParamType;
import GS2GC.p007_CommOp.GS2GC_007_066_OnClientVersionChg;
import NPCommon.Enum.EUsParam;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPServerEnum.ENPUserDataState;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.PHPParam.BSPHPParamMgr;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.ADelegateNone;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPPlayerParam;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import NPServerProtocolWriter.NP2GS.Msg.NP2GS_Writer_001_BasicOp;
import NPUSServer.NPGeneralListener.NPUSGeneralBasicServerListener;
import NPUSServer.NPUserServer;
import NPUSServer.ServerCallback.NPSynLoadPlayerDataTask;
import NPUSServer.USLog;
import NPUSServer.UserServerConf;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;
import WCGCommon.Enum.NPEnum.EWCGKickOutGateType;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.ConcurrentHashMap;

/*****************
 * 所有用户数据的存储和管理对象
 * @author Administrator
 *
 */
public class NPUSUserMgr implements _IALSynTask, _IHandlerHolder
{
    /****************
     * 判断用户数据是否需要卸载的判断数据对象，通过序列号进行合法性判断
     * @author mj
     *
     */
    public static class NPUSJudgeUserData
    {
        public NPUSUserData data;
        //生成本数据结构的时候的用户数据序列号
        public long serialize;

        public NPUSJudgeUserData(NPUSUserData _userData)
        {
            data = _userData;
            serialize = _userData.getSerialize();
        }
    }

    private NPUserServer _m_usUSServer;

    // 缓存玩家列表
    private ConcurrentHashMap<Long, NPUSUserData> _m_allCachedUserDataMap = new ConcurrentHashMap<>();
    private ArrayList<NPUSUserData> _m_lAllUserList = new ArrayList<NPUSUserData>();

    //安卓和IOS玩家数量统计(运营日志使用)
    private HashSet<Long> _m_lAndroidUserCidSet = new HashSet<>();
    private HashSet<Long> _m_lIOSUserCidSet = new HashSet<>();

    // 在线玩家集合
    private HashSet<NPUSUserData> _m_hsOnlineUserData = new HashSet<NPUSUserData>();
    //临时处理的队列对象，可确保在处理过程不会被变更
    private ArrayList<NPUSUserData> _m_lDealTmpAllUserList = new ArrayList<NPUSUserData>();

    // 预离线玩家列表
    private LinkedList<NPUSJudgeUserData> _m_lJudgeOffLineUserDataList = new LinkedList<NPUSJudgeUserData>();
    // 同步锁变量
    private MutexObject _m_allDataMutex = new MutexObject();
    private MutexAtom _m_onLineMutex = new MutexAtom();
    private MutexAtom _m_offLineMutex = new MutexAtom();

    //秒时间标签
    private long _m_lTickSecTimeTag = 0;
    //分钟时间标签
    private long _m_lTickMinTimeTag = 0;

    //最后一天的天数标记
    private long _m_lLastDayTag;

    public ADelegateNone OnServerCrossDay = new ADelegateNone(this);

    public NPUSUserMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_lLastDayTag = 0;

        BSPHPParamMgr.getInstance().getParamChgDelegate().addHandler(this, new HandlerTwo<EPlatParamType, String>()
        {
            @Override
            public void handle(EPlatParamType _type, String _value)
            {
                if (_type == EPlatParamType.CLIENT_VERSION)
                {
                    broadCastMessage(new GS2GC_007_066_OnClientVersionChg());
                }
            }
        });
    }

    public NPUserServer getUSServer()
    {
        return _m_usUSServer;
    }

    /************************
     * 根据用户Id获取用户的数据
     *
     * @param _cid
     * @return
     */
    public NPUSUserData lookupCacheUserData(long _cid)
    {
        _m_allDataMutex.lock();
        try
        {
            NPUSUserData userData = _m_allCachedUserDataMap.get(_cid);

            return userData;
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /************************
     * 所有玩家列表
     *
     * @return
     */
    public ArrayList<NPUSUserData> getAllCacheUserData()
    {
        _m_allDataMutex.lock();
        try
        {
            return new ArrayList<>(_m_allCachedUserDataMap.values());
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /************************
     * 所有玩家数据的数量
     *
     * @return
     */
    public int getAllCacheUserDataCount()
    {
        _m_allDataMutex.lock();
        try
        {
            return _m_lAllUserList.size();
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /************************
     * 所有在线玩家数据的数量
     *
     * @return
     */
    public int getAllOnlineUserDataCount()
    {
        _m_onLineMutex.lock();
        try
        {
            return _m_hsOnlineUserData.size();
        } finally
        {
            _m_onLineMutex.unlock();
        }
    }

    /************************
     * 所有在线安卓玩家数据的数量
     * @return
     */
    public int getOnlineAndroidUserDataCount()
    {
        _m_onLineMutex.lock();
        try
        {
            return _m_lAndroidUserCidSet.size();
        } finally
        {
            _m_onLineMutex.unlock();
        }
    }

    /************************
     * 所有在线苹果玩家数据的数量
     * @return
     */
    public int getOnlineIOSUserDataCount()
    {
        _m_onLineMutex.lock();
        try
        {
            return _m_lIOSUserCidSet.size();
        } finally
        {
            _m_onLineMutex.unlock();
        }
    }

    /************************
     * 注册用户数据
     *
     * @param _userData
     */
    public void regUserData(NPUSUserData _userData)
    {
        _m_allDataMutex.lock();
        try
        {
            if (!_m_allCachedUserDataMap.containsKey(_userData.getCid()))
                _m_lAllUserList.add(_userData);

            NPUSUserData preData = _m_allCachedUserDataMap.put(_userData.getCid(), _userData);
            if (null != preData)
                _m_lAllUserList.remove(preData);
        } finally
        {
            _m_allDataMutex.unlock();
        }
        //更新CS上的注册人数统计
        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal(),
                NP2CS_R_Writer_002_ServerInfoOp.make_004_ReqUpdateUSHoldInfo(getUSServer().makeHoldInfo()));
    }

    /************************
     * 卸载玩家数据
     *
     * @param _userData
     */
    public void unregUserData(NPUSUserData _userData)
    {
        if (null == _userData)
            return;

        _m_allDataMutex.lock();
        try
        {
            NPUSUserData preData = _m_allCachedUserDataMap.remove(_userData.getCid());
            if (preData != _userData)
            {
                _m_allCachedUserDataMap.put(preData.getCid(), preData);
            } else
            {
                if (null != preData)
                    _m_lAllUserList.remove(preData);
            }
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /************************
     * 将用户调整为在线
     *
     * @param _cid
     * @return
     */
    public NPUSUserData changeUser2Online(long _cid)
    {
        _m_allDataMutex.lock();
        try
        {
            NPUSUserData userData = _m_allCachedUserDataMap.get(_cid);
            if (null == userData)
            {
                return null;
            }

            //刷新在线标记
            userData.online();

            //注册在线操作
            _m_onLineMutex.lock();
            try
            {
                _m_hsOnlineUserData.add(userData);

                //增加对应设备的计数
                if (userData.getSdkInfo().deviceType == NPCommonEnum.EClientType.ANDROID)
                {
                    _m_lAndroidUserCidSet.add(userData.getCid());
                } else if (userData.getSdkInfo().deviceType == NPCommonEnum.EClientType.IOS)
                {
                    _m_lIOSUserCidSet.add(userData.getCid());
                }
            } finally
            {
                _m_onLineMutex.unlock();
            }

            return userData;
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /************************
     * 设置用户初始化离线，这个的特殊处理在于，离线标记需要更长，避免因为加载问题导致数据被卸载
     *
     * @param _cid
     * @param _serialize
     * @return
     */
    public NPUSUserData changeUserInitOffline(long _cid)
    {
        _m_allDataMutex.lock();
        try
        {
            NPUSUserData userData = _m_allCachedUserDataMap.get(_cid);
            if (null == userData)
            {
                return null;
            }

            //刷新在线标记
            userData.offline();

            //注销在线操作
            _m_onLineMutex.lock();
            try
            {
                _m_hsOnlineUserData.remove(userData);

                //把对应设备数量降低
                _m_lAndroidUserCidSet.remove(userData.getCid());
                _m_lIOSUserCidSet.remove(userData.getCid());
            } finally
            {
                _m_onLineMutex.unlock();
            }

            //注册离线操作
            _m_offLineMutex.lock();
            try
            {
                _m_lJudgeOffLineUserDataList.add(new NPUSJudgeUserData(userData));
            } finally
            {
                _m_offLineMutex.unlock();
            }
            return userData;
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /************************
     * 设置用户离线
     *
     * @param _cid
     * @param _serialize
     * @return
     */
    public NPUSUserData changeUserOffline(long _cid, long _serialize)
    {
        _m_allDataMutex.lock();
        try
        {
            NPUSUserData userData = _m_allCachedUserDataMap.get(_cid);
            if (null == userData)
            {
                return null;
            }
            if (_serialize != 0 && userData.getSerialize() != _serialize)
            {
                return null;
            }

            //刷新在线标记
            userData.offline();

            //注销在线操作
            _m_onLineMutex.lock();
            try
            {
                _m_hsOnlineUserData.remove(userData);

                //把对应设备数量降低
                _m_lAndroidUserCidSet.remove(userData.getCid());
                _m_lIOSUserCidSet.remove(userData.getCid());
            } finally
            {
                _m_onLineMutex.unlock();
            }

            //注册离线操作
            _m_offLineMutex.lock();
            try
            {
                _m_lJudgeOffLineUserDataList.add(new NPUSJudgeUserData(userData));
            } finally
            {
                _m_offLineMutex.unlock();
            }
            return userData;
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /************************
     * 加载用户数据
     *
     * @param _cid
     * @param _uid
     * @param _customData
     * @return
     */
    public NPUSUserData initUserData(long _cid, String _uid, String _customData, _IWCGBasicRequestCommiter _commiter)
    {
        //拦截已经被冻结的用户
        if (getUSServer().getPlayerFreezeMgr().checkPlayerIsFreeze(_cid))
        {
            //如果存在数据则直接返回成功
            _commiter.commitFailRes(PlayerErr.PLAYER_ACCOUNT_IS_FREEZE.getCode());
            return null;
        }

        _m_allDataMutex.lock();
        try
        {
            NPUSUserData userData = _m_allCachedUserDataMap.get(_cid);
            if (null == userData)
            {
                userData = new NPUSUserData(getUSServer(), _cid, _uid, _customData);
                //注册用户
                regUserData(userData);
                //设置用户数据状态
                userData.setDataState(ENPUserDataState.LOADING);

                //开始加载过程
                NPSynLoadPlayerDataTask loadTask = new NPSynLoadPlayerDataTask(userData);

                //更新玩家标志
                getUSServer().getUsUserMgr().changeUserInitOffline(_cid);

                //设置加载处理对象
                userData.setDataLoadRequestCommiter(_commiter);
                //先注册处理再开启任务
                ALSynTaskManager.getInstance().regTask(loadTask);

                return userData;
            }

            //更新玩家标志
            getUSServer().getUsUserMgr().changeUserInitOffline(_cid);

            //设置加载处理对象
            userData.setDataLoadRequestCommiter(_commiter);

            return userData;
        } finally
        {
            _m_allDataMutex.unlock();
        }
    }

    /***************
     * 开始时间统计
     */
    public void startTimeCheck()
    {
        long nowTime = ALBasicCommonFun.getNowTimeMS();
        _m_lTickSecTimeTag = nowTime - (nowTime % 1000) + 1000;
        _m_lTickMinTimeTag = nowTime - (nowTime % 60000) + 60000;

        //获取当前的天数标记
        _m_lLastDayTag = getUSServer().getUSParams().getParam(EUsParam.LAST_RUN_DAY);

        //开启任务
        ALSynTaskManager.getInstance().regTask(this, 1 * 1000);
    }

    /**************
     * 设置最后一天的标记
     */
    public void setLastDayTag(long _tag)
    {
        _m_lLastDayTag = _tag;
        //设置服务器数据库值
        getUSServer().getUSParams().setParam(EUsParam.LAST_RUN_DAY, _m_lLastDayTag);
    }

    @Override
    public void run()
    {
        long nowTime = ALBasicCommonFun.getNowTimeMS();

        //获取当前的用户队列
        _m_lDealTmpAllUserList.clear();
        _m_allDataMutex.lock();
        try
        {
            _m_lDealTmpAllUserList.addAll(_m_lAllUserList);
        } finally
        {
            _m_allDataMutex.unlock();
        }

        //判断每秒处理的时间标签
        if (nowTime > _m_lTickSecTimeTag)
        {
            _tick1Sec(_m_lDealTmpAllUserList);
            _m_lTickSecTimeTag += 1000;
        }

        //判断每秒处理的时间标签
        if (nowTime > _m_lTickMinTimeTag)
        {
            _tick1Min(_m_lDealTmpAllUserList);
            _m_lTickMinTimeTag += 60000;
        }

        //重置数据
        _m_lDealTmpAllUserList.clear();

        ALSynTaskManager.getInstance().regTask(this, _m_lTickSecTimeTag - nowTime);
    }

    //对带入的用户队列每个用户进行操作
    private void _tick1Sec(ArrayList<NPUSUserData> _userList)
    {
        int nowSec = CommonFunc.getNowTimeSec();
        int nowTag = CommonFunc.getTimeTagYYYYMMDD(nowSec);

        //临时对象
        NPUSUserData tmpData = null;

        //判断是否跨天，跨天则进行其他处理
        if (nowTag != _m_lLastDayTag)//跨天了
        {
            //设置天数标记
            setLastDayTag(nowTag);

            //触发玩家跨天登录
            for (int i = 0; i < _userList.size(); i++)
            {
                tmpData = _userList.get(i);
                //判断数据状态
                if (null == tmpData || tmpData.getUserDataState() != ENPUserDataState.LOADED)
                    continue;

                tmpData.lockUser();
                try
                {
                    tmpData.checkLoginCrossDay(nowTag);
                } finally
                {
                    tmpData.unlockUser();
                }
            }

            OnServerCrossDay.onAsyncEvent();
        }

        //进行例行的每秒检测
        for (int i = 0; i < _userList.size(); i++)
        {
            tmpData = _userList.get(i);
            //判断数据状态
            if (null == tmpData || tmpData.getUserDataState() != ENPUserDataState.LOADED)
                continue;

            tmpData.lockUser();
            try
            {
                tmpData.tick1Sec();
            } finally
            {
                tmpData.unlockUser();
            }
        }
    }

    private void _tick1Min(ArrayList<NPUSUserData> _userList)
    {
        //处理离线数据
        unloadExpiredUserData(false);

        //打印在线人数
        int allNum = _m_allCachedUserDataMap.size();
        int onlineNum = _m_hsOnlineUserData.size();
        if (allNum > 0)
        {
            long mb = 1024 * 1024;
            long totalMem = Runtime.getRuntime().totalMemory() / mb;
            long freeMem = Runtime.getRuntime().freeMemory() / mb;
            long maxMem = Runtime.getRuntime().maxMemory() / mb;
            USLog.info(getUSServer(), "[SYSINFO] all[{}],online[{}],offline[{}], [mem]:total[{}MB],free[{}MB],maxavail[{}MB]"
                    , allNum
                    , onlineNum
                    , allNum - onlineNum
                    , totalMem
                    , freeMem
                    , maxMem);
        }
    }

    /****************
     * 取出第一个需要判断的数据对象
     * @return
     */
    private NPUSJudgeUserData _popJudgeOffLineUserData()
    {
        //注册离线操作
        _m_offLineMutex.lock();
        try
        {
            if (_m_lJudgeOffLineUserDataList.isEmpty())
                return null;

            return _m_lJudgeOffLineUserDataList.getFirst();
        } finally
        {
            _m_offLineMutex.unlock();
        }
    }

    /**
     * 卸载超时玩家的定时处理函数
     * @param _isForce 是否强制清理数据
     */
    public void unloadExpiredUserData(boolean _isForce)
    {
        NPUSJudgeUserData judgeData = _popJudgeOffLineUserData();
        if (null == judgeData)
            return;

        long now = CommonFunc.getNowTimeMS();
        //逐个判断
        while (null != judgeData)
        {
            NPUSUserData rmvUserData = judgeData.data;
            NPUSUserData discardUserData = null;

            //表示用户可能超时的时候，需要锁定所有用户管理数据。之后才进行用户数据的时间戳判断
            _m_allDataMutex.lock();
            try
            {
                //判断序列号合法性，并判断时间戳。当序列号不一致或时间戳等于0时表示数据在线，不做处理
                //时间戳改动只在UserMgr控制下，所以此处判断可以保证正确性
                if (null != rmvUserData && judgeData.serialize == rmvUserData.getSerialize() && rmvUserData.getOnlineTag() != 0)
                {
                    //已离线时间
                    long offloadTimeMs = now - rmvUserData.getOnlineTag();
                    //根据时间戳判断是否已经离线30分钟，是则从数据集中删除对应数据对象
                    if (rmvUserData.getOnlineTag() != 0 &&
                            (offloadTimeMs > UserServerConf.getInstance().getUserDataExpiredTimeMs() || _isForce))//离线30分钟,卸载改成配置方便调整
                    {
                        //注销用户
                        unregUserData(rmvUserData);

                        //设置释放玩家
                        discardUserData = rmvUserData;
                    } else
                    {
                        //进入此位置表示判断的数据合法，且未到达需要释放的时间点，这个地方直接跳出循环
                        break;
                    }
                }
            } finally
            {
                _m_allDataMutex.unlock();
            }

            if (null != discardUserData)
            {
                //处理事件函数
                _onUnloadExpiredUserData(discardUserData);

                //锁定用户数据进行后续处理
                discardUserData.lockUser();

                try
                {
                    //注销资源
                    discardUserData.dispose();
                } finally
                {
                    discardUserData.unlockUser();
                }
            }

            //进入这个位置的情况下都需要移除当前节点，并获取下一节点
            _m_offLineMutex.lock();
            try
            {
                _m_lJudgeOffLineUserDataList.removeFirst();

                //取出下一个需要判断的数据
                judgeData = _popJudgeOffLineUserData();
            } finally
            {
                _m_offLineMutex.unlock();
            }
        }
    }

    /**
     * 卸载用户数据的时候触发的函数
     */
    private void _onUnloadExpiredUserData(NPUSUserData _userData)
    {
    }


    /**
     * 强制把玩家踢下线,并通知客户端用户断开连接
     * @param _cid 角色id
     */
    public void forceKickUser(long _cid)
    {
        //如果玩家在线将玩家强制下线
        NPUSUserData userData = changeUserOffline(_cid, 0);
        if (userData != null)
        {
            //数据有效则处理离线操作
            //标记用户登出游戏
            userData.setLogicOnline(false);
            //删除GS连接对象
            NPUSGeneralBasicServerListener preGsListener = userData.chgGSListener(null);
            //发送消息通知用户连接被踢
            if (null != preGsListener)
            {
                preGsListener.sendCustomMsg(NP2GS_Writer_001_BasicOp.make_002_UserGateKicked(_cid, userData.getClinetSessionId()
                        , EWCGKickOutGateType.OPERATE));
            }


        }
    }


    /**
     * 发送协议消息，如果玩家存在的话
     */
    public void sendProtoIfUserDataExist(long _cid, _IALProtocolStructure _proto)
    {
        ALSynTaskManager.getInstance().regTask(() ->
        {
            NPUSUserData userData = _m_allCachedUserDataMap.get(_cid);
            if (userData == null)
            {
                return;
            }
            userData.sendMsgToGC(_proto);
        });
    }

    /**
     * 广播消息
     * @param _proto 协议
     */
    public void broadCastMessage(_IALProtocolStructure _proto)
    {
        ArrayList<NPUSUserData> userList = new ArrayList<>();
        _m_allDataMutex.lock();
        try
        {
            userList.addAll(_m_lAllUserList);
        } finally
        {
            _m_allDataMutex.unlock();
        }

        for (NPUSUserData tmpData : userList)
        {
            //判断数据状态
            if (null == tmpData || tmpData.getUserDataState() != ENPUserDataState.LOADED)
                continue;

            tmpData.lockUser();
            try
            {
                tmpData.sendMsgToGC(_proto);
            } finally
            {
                tmpData.unlockUser();
            }
        }
    }

    /**
     * 踢出所有在线玩家
     */
    public void forceKickAllUser()
    {
        List<NPUSUserData> needKickUserList;
        //注销在线操作
        _m_onLineMutex.lock();
        try
        {
            needKickUserList = new ArrayList<>(_m_hsOnlineUserData);
        } finally
        {
            _m_onLineMutex.unlock();
        }

        for (NPUSUserData onlineData : needKickUserList)
        {
            forceKickUser(onlineData.getCid());
        }
    }

    /**
     * 服务器开始时间变更时触发的函数
     */
    public void onServerStartDateChg()
    {
        for (NPUSUserData userData : getAllCacheUserData())
        {
            userData.safeCall(() ->
            {
                //设置服务器开始时间
                userData.setParam(ENPPlayerParam.SERVER_START_DAYS, -1);
            });
        }

    }
}
