package NPUSServer.NPGeneralListener.RequestDispather;

import NP2US_R.p002_GSOp.*;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.LoginErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.EServerOnlineState;
import NPServerProtocolWriter.NP2GS.Msg.NP2GS_Writer_001_BasicOp;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPGeneralListener.NPUSGeneralBasicServerListener;
import NPUSServer.NPGeneralListener.Writer.NP2US_RB_Writer_002_GSOp;
import NPUSServer.NPGeneralListener._ANPUSBasicServerListener;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCommon.Enum.NPEnum.EServerType;
import WCGCommon.Enum.NPEnum.EWCGKickOutGateType;

public class NPUSGeneral_002_RequestDispatcher_GSOp extends NPRequestDispatcher
{
    public static void init(NPUSGeneralRequestDispather _dispather)
    {
        NPUserServer usServer = _dispather.getUSServer();

        _dispather.regHandler(new NPRequestDealer<NP2US_R_002_001_RegUserGate>()
        {
            @Override
            protected void _dealMessage(final _IWCGBasicRequestCommiter _committer, final NP2US_R_002_001_RegUserGate _msg)
            {
                if (!usServer.isServerReady())
                {
                    USLog.error(usServer, "player cid:{} login while server not ready!", _msg.getCid());
                    _committer.commitFailRes(LoginErr.LOGIN_WHEN_US_NOT_READY.getCode());
                    return;
                }

                USLog.error(usServer, "cid:{} reg gs listener:{}", _msg.getCid(), _msg.getSessionId());
                NPUSUserData userData = usServer.getUsUserMgr().lookupCacheUserData(_msg.getCid());
                if (null == userData)
                {
                    _committer.commitFailRes(CommErr.PLAYER_NOT_FOUND.getCode());
                    return;
                }

                // 设置对应的连接服务器
                NPUSGeneralBasicServerListener gsListener = (NPUSGeneralBasicServerListener) _committer.getRequestDealer();
                // 设置服务器Id
                NPUSGeneralBasicServerListener preGsListener = userData.chgGSListener(gsListener);

                //如果前后GS一致则直接返回成功
                if (preGsListener == gsListener
                        && userData.getClinetSessionId() == _msg.getSessionId()
                        && userData.getMsgDealerSerialize() == _msg.getDealerSerialize())
                {
                    //回包
                    _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_001_RegUserGateSuc(userData.getSerialize()));
                    return;
                }

                //发送消息通知用户连接被踢
                if (null != preGsListener)
                {
                    preGsListener.sendCustomMsg(
                            NP2GS_Writer_001_BasicOp.make_002_UserGateKicked(userData.getCid(),
                                    userData.getClinetSessionId(), EWCGKickOutGateType.DEVICE));
                }

                //设置用户对应的gate服务器序列号
                userData.setClientSessionId(_msg.getSessionId());
                userData.setMsgDealerSerialize(_msg.getDealerSerialize());

                //客户端自定义数据
                userData.getSdkInfo().setExternalInfo(false, _msg.getClientIp(), _msg.getCustomData());
                
                //更新用户AreaTag
                userData.setAreaTag(_msg.getAreaTag());

                //标记用户进入游戏
                userData.setLogicOnline(true);

                //更新玩家标志
                usServer.getUsUserMgr().changeUser2Online(_msg.getCid());

                //回包
                _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_001_RegUserGateSuc(userData.getSerialize()));
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2US_R_002_002_UnregUserGate>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_002_002_UnregUserGate _msg)
            {
                NPUSUserData userData = usServer.getUsUserMgr().changeUserOffline(_msg.getCid(), _msg.getSerialize());

                //数据有效则处理离线操作
                if (null != userData)
                {
                    //标记用户登出游戏
                    userData.setLogicOnline(false);
                    //这里刻意不删除gs连接对象，这样即使用户断线，消息还是能到gs得到缓存
                    //userData.chgGSListener(null);
                }

                _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_002_UnregUserGateSuc());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2US_R_002_004_ReqResumeUSInfo>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_002_004_ReqResumeUSInfo _msg)
            {
                _ANPUSBasicServerListener listener = (_ANPUSBasicServerListener) _committer.getRequestDealer();
                //判断操作服务器是否gate
                if (listener.getServerType() != EServerType.GATE.ordinal())
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    USLog.error(usServer, "Req Enter US for uid: " + _msg.getUid() + "  not from gs!");
                    return;
                }

                //获取gsId
                int gsId = listener.getServerTypeId();

                long cid = _dispather.getUSServer().getUserIdxMgr().lookupUserCid(_msg.getUid());

                //服务器状态是否允许登录
                if (usServer.getUsOnlineState() == EServerOnlineState.CLOSED.ordinal()
                        || usServer.getUsOnlineState() == EServerOnlineState.TEMP_CLOSED.ordinal())
                {
                    UsFunc.checkInWhiteAccList(usServer, _msg.getUid(), _isInWhiteList ->
                    {
                        if (!_isInWhiteList)
                        {
                            //服务器状态拦截直接返回false
                            _committer.commitFailRes(LoginErr.LOGIN_WHEN_US_NOT_READY.getCode());

                            //返回消息通知数据未加载
                            usServer.sendMessageToBSServer(EServerType.GATE.ordinal(), gsId
                                    , NP2GS_Writer_001_BasicOp.make_003_OnUserDataLoaded(LoginErr.LOGIN_WHEN_US_NOT_READY.getCode(),
                                            _msg.getGsSessionId(), _msg.getGsInfoSerialize(), cid));

                            return;
                        }

                        //处理恢复用户信息
                        _deal_002_004_resumeUsInfo(_committer, _msg, cid, gsId, usServer, _dispather);
                    });
                } else
                {
                    //处理恢复用户信息
                    _deal_002_004_resumeUsInfo(_committer, _msg, cid, gsId, usServer, _dispather);
                }
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2US_R_002_005_ReqEnterUS>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_002_005_ReqEnterUS _msg)
            {
                _ANPUSBasicServerListener listener = (_ANPUSBasicServerListener) _committer.getRequestDealer();
                //判断操作服务器是否gate
                if (listener.getServerType() != EServerType.GATE.ordinal())
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    USLog.error(usServer, "Req Enter US for uid: " + _msg.getUid() + "  not from gs!");
                    return;
                }

                //获取gsId
                int gsId = listener.getServerTypeId();

                //检索uid对应的cid
                long cid = _dispather.getUSServer().getUserIdxMgr().lookupUserCid(_msg.getUid());

                //服务器状态是否允许登录
                if (usServer.getUsOnlineState() == EServerOnlineState.CLOSED.ordinal()
                        || usServer.getUsOnlineState() == EServerOnlineState.TEMP_CLOSED.ordinal())
                {
                    UsFunc.checkInWhiteAccList(usServer, _msg.getUid(), _isInWhiteList ->
                    {
                        if (!_isInWhiteList)
                        {
                            //服务器状态拦截直接返回false
                            _committer.commitFailRes(LoginErr.LOGIN_WHEN_US_NOT_READY.getCode());

                            //返回消息通知数据未加载
                            usServer.sendMessageToBSServer(EServerType.GATE.ordinal(), gsId
                                    , NP2GS_Writer_001_BasicOp.make_003_OnUserDataLoaded(LoginErr.LOGIN_WHEN_US_NOT_READY.getCode(),
                                            _msg.getGsSessionId(), _msg.getGsInfoSerialize(), cid));

                            return;
                        }

                        //处理进入游戏
                        _deal_002_005_enterUs(usServer, cid, gsId, _msg, _dispather, _committer, _msg.getCustomData());
                    });
                } else
                {
                    //处理进入游戏
                    _deal_002_005_enterUs(usServer, cid, gsId, _msg, _dispather, _committer, _msg.getCustomData());
                }
            }
        });

        //申请队列当前位置信息
        _dispather.regHandler(new NPRequestDealer<NP2US_R_002_006_ReqQueueInfo>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_002_006_ReqQueueInfo _msg)
            {
                //返回消息
                _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_006_RetQueueInfo(_dispather.getUSServer()));
            }
        });

        //申请退出当前队列
        _dispather.regHandler(new NPRequestDealer<NP2US_R_002_007_ReqQuitQueue>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_002_007_ReqQuitQueue _msg)
            {
                //尝试取消当前队列节点操作
                Result res = _dispather.getUSServer().getQueueMgr().quitQueue(_msg.getCurQueueIndex(), _msg.getInfoSerialize());

                //返回结果
                _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_007_RetQuitQueueRes(res.getCode()));
            }
        });
    }

    /**
     * 处理002_004恢复用户信息
     */
    private static void _deal_002_004_resumeUsInfo(_IWCGBasicRequestCommiter _committer, NP2US_R_002_004_ReqResumeUSInfo _msg,
                                                   long cid, int gsId, NPUserServer usServer, NPUSGeneralRequestDispather _dispather)
    {
        //拦截已经被冻结的角色
        if (usServer.getPlayerFreezeMgr().checkPlayerIsFreeze(cid))
        {
            //玩家被冻结
            usServer.sendMessageToBSServer(EServerType.GATE.ordinal(), gsId
                    , NP2GS_Writer_001_BasicOp.make_004_OnEnterUSButFreeze(
                            usServer.getPlayerFreezeMgr().lookupFreezeTime(cid), cid, _msg.getGsSessionId(),
                            _msg.getGsInfoSerialize()));
            return;
        }

        //尝试获取用户数据，如用户数据不存在则返回错误
        long preCid = _dispather.getUSServer().getUserIdxMgr().lookupUserCid(_msg.getUid());
        if (preCid == 0)
        {
            //返回信息
            _committer.commitFailRes(LoginErr.LOGIN_NO_USER_INFO.getCode());
            return;
        }

        //已经存在cid，查询是否存在UserData，使用尝试设置离线处理，可以保证数据不会被销毁
        NPUSUserData preUserData = usServer.getUsUserMgr().changeUserOffline(preCid, 0);
        if (null == preUserData)
        {
            //返回信息
            _committer.commitFailRes(LoginErr.LOGIN_NO_USER_INFO.getCode());
            return;
        }

        //返回信息
        _committer.commitSucRes(NP2US_RB_Writer_002_GSOp.make_004_RetResumeUSInfo(preUserData.getCid()));
    }

    /**
     * 处理002_005进入游戏
     */
    private static void _deal_002_005_enterUs(NPUserServer _usServer, long _cid, int _gsId, NP2US_R_002_005_ReqEnterUS _msg,
                                              NPUSGeneralRequestDispather _dispather, _IWCGBasicRequestCommiter _committer, String _customData)
    {
        //拦截已经被冻结的角色
        if (_usServer.getPlayerFreezeMgr().checkPlayerIsFreeze(_cid))
        {
            //玩家被冻结
            _usServer.sendMessageToBSServer(EServerType.GATE.ordinal(), _gsId
                    , NP2GS_Writer_001_BasicOp.make_004_OnEnterUSButFreeze(
                            _usServer.getPlayerFreezeMgr().lookupFreezeTime(_cid), _cid, _msg.getGsSessionId(),
                            _msg.getGsInfoSerialize()));
            return;
        }

        //调用队列处理
        _dispather.getUSServer().getQueueMgr().tryLoadUserData(_msg.getUid(), _msg.getGsSessionId(),
                _gsId, _msg.getGsInfoSerialize(), _customData, _committer);
    }
}