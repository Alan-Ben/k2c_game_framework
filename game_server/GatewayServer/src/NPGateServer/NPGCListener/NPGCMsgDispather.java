package NPGateServer.NPGCListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo;
import NP2CS_RB.np_p002_serverInfoOp.NP2CS_RB_002_008_RetMostRecommendedUSInfo;
import NP2LCS_RB.p001_BasicOp.NP2RCS_RB_001_002_RetLookupLoginRecord;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Log.CommLog;
import NPCommon.NP_SYS_ServerItem;
import NPCommon.Promise.SerialPromise;
import NPCommon.Util.CallBack._ICallBackT;
import NPCommon.Util.RefWrap;
import NPGC2GS.p001_BasicOp.*;
import NPGS2GC.p001_BasicOp.NPGS2GC_001_006_QuitUSRes;
import NPGS2GC.p001_BasicOp.NPGS2GC_001_011_RetPlayerJoinedUSList;
import NPGateServer.NPGCListener.SynTask.SynGCReSendReconnectMsgTask;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import NPGateServer.NPGCMsgMgr.NPGCMsgItem._ANPGSMsgItem;
import NPGateServer.NPGateServer;
import NPServerProtocolWriter.NP2CS.Request.NP2CS_R_Writer_002_ServerInfoOp;
import NPServerProtocolWriter.NP2RCS.Request.NP2RCS_R_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;

/**************
 * 客户端协议处理对象
 *
 * @author Administrator
 *
 */
public class NPGCMsgDispather extends NPCustomMsgDispatcher
{
    private static NPGCMsgDispather _g_instance = new NPGCMsgDispather();

    public static NPGCMsgDispather getInstance()
    {
        return _g_instance;
    }

    protected NPGCMsgDispather()
    {
        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_001_ReqBasicInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_001_ReqBasicInfo _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener || null == listener.getUSInfo())
                {
                    return;
                }

                // 返回消息
                listener.send(NPGS2GCWriter_001_BasicOp.make_001_RetBasicInfo(listener.getUSInfo().getUSId()));
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_002_HeartPack>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_002_HeartPack _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener)
                {
                    return;
                }
                listener.send(NPGS2GCWriter_001_BasicOp.make_002_HeartPack(_msg.getClientHeartSerialize(), _msg.getClientTimeTag()));
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_005_RequestEnterUS>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_005_RequestEnterUS _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener)
                {
                    return;
                }

                //尝试进入服务器
                listener.requestEnterUS(_msg.getServerLogicId(), _msg.getClientSerialize());
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_006_QuitUS>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_006_QuitUS _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener)
                {
                    return;
                }

                Result res = listener.quitUS(_msg.getUsId());

                //返回信息
                NPGS2GC_001_006_QuitUSRes proto = NPGS2GCWriter_001_BasicOp.make_006_QuitUSRes(res.getCode(), _msg.getClientSerialize());
                listener.send(proto);
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_007_ReqQueueInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_007_ReqQueueInfo _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener || null == listener.getUSInfo())
                {
                    return;
                }
                listener.getUSInfo().requestUSCurIndex(_msg.getClientSerialize());
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_008_ReqQuitQueue>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_008_ReqQuitQueue _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener || null == listener.getUSInfo())
                {
                    return;
                }

                listener.getUSInfo().reqQuitQueue(_msg.getClientSerialize());
                listener.unregUSInfo();
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_009_ResumeUSConnection>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_009_ResumeUSConnection _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener)
                {
                    return;
                }

                //尝试进入服务器
                listener.resumeUSConnection(_msg.getServerLogicId(), _msg.getClientSerialize());
            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_011_ReqPlayerJoinedUSList>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_011_ReqPlayerJoinedUSList _msg)
            {
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener)
                {
                    return;
                }
                //去记录服务器请求登录信息
                NPGateServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.RECORD.ordinal(),
                        NP2RCS_R_Writer_001_BasicOp.make_001_002_ReqLookupLoginRecord(String.valueOf(listener.getUid()))
                        , new _IWCGCallbackDealer()
                        {
                            @Override
                            public _IALProtocolStructure createProtocolObj()
                            {
                                return new NP2RCS_RB_001_002_RetLookupLoginRecord();
                            }

                            @Override
                            public void dealSuc(_IALProtocolStructure _proto)
                            {
                                //获取到登录记录挑选出最后登录的服务器服务器logicId
                                NP2RCS_RB_001_002_RetLookupLoginRecord proto = (NP2RCS_RB_001_002_RetLookupLoginRecord) _proto;
                                NP_SYS_PlayerJoinedUSInfo lastLoginUSInfo = null;
                                for (NP_SYS_PlayerJoinedUSInfo usInfo : proto.getJoinedUSList())
                                {
                                    if (usInfo == null)
                                    {
                                        continue;
                                    }
                                    if (lastLoginUSInfo == null || lastLoginUSInfo.getLastLoginTimeMs() < usInfo.getLastLoginTimeMs())
                                    {
                                        lastLoginUSInfo = usInfo;
                                    }
                                }

                                NPGS2GC_001_011_RetPlayerJoinedUSList retProto = NPGS2GCWriter_001_BasicOp.make_011_RetPlayerJoinedUSList(proto.getJoinedUSList()
                                        , lastLoginUSInfo == null ? 0 : lastLoginUSInfo.getServerItem().getServerLogicId());

                                //返回协议
                                listener.send(retProto);
                            }

                            @Override
                            public void dealFail(int i)
                            {
                                CommLog.error("NPGC2GS_001_011_ReqPlayerJoinedUSList dealFail errCode:{}", i);
                                listener.send(new NPGS2GC_001_011_RetPlayerJoinedUSList());
                            }
                        });

            }
        });

        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_012_ReqMostRecommendedUSInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_012_ReqMostRecommendedUSInfo _msg)
            {
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener)
                {
                    return;
                }

                SerialPromise promise = new SerialPromise(null);
                RefWrap<NP_SYS_ServerItem> targetServerItem = new RefWrap<>(new NP_SYS_ServerItem());
                //1.尝试获取最近登录的服务器
                promise.then(p -> getRecentLoginServer(String.valueOf(listener.getUid()), _serverItem ->
                {
                    if (_serverItem == null)
                    {
                        promise.commit();
                        return;
                    }

                    targetServerItem.set(_serverItem);
                    promise.breakOut();
                }));

                //2.获取推荐服务器
                promise.then(p -> getRecommendServerInfo(_serverItem ->
                {
                    targetServerItem.set(_serverItem);
                    promise.breakOut();
                }));

                //3.返回服务器信息
                promise.over(p-> listener.send(NPGS2GCWriter_001_BasicOp.make_012_RetMostRecommendedUSInfo(targetServerItem.get())));
            }
        });

        //处理断线重连的操作
        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_020_ReconnectInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_020_ReconnectInfo _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener || null == listener.getUSInfo() || !listener.getUSInfo().isInitUSDone())
                {
                    return;
                }

                //获取在处理之前是否已经通过检测，如果此时未通过，在返回之后需要开个任务再次发送消息，保证消息能正常发送到
                boolean preCanSendback = listener.getMsgDealer().getIsCheckReconnect();

//                System.err.println("chk: " + listener.getMsgDealer().getListFirstSendedMsgSerialize() + " - " + _msg.getCurRecMesCount()
//                                    + " - " + listener.getMsgDealer().getSendBackMsgCount());
                //验证消息,并获取需要返回的消息数量
                ArrayList<_ANPGSMsgItem> needSendBackMsg = listener.getMsgDealer().checkReconnectSendBackMsg(_msg.getCurRecMesCount());
                //逐个消息返回
                if (null != needSendBackMsg)
                {
                    _ANPGSMsgItem tmp = null;
                    for (int i = 0; i < needSendBackMsg.size(); i++)
                    {
                        tmp = needSendBackMsg.get(i);
                        if (null == tmp)
                            continue;

                        //System.err.println("resend: " + needSendBackMsg.get(i).getMainOrder() + " - " + needSendBackMsg.get(i).getSubOrder());
                        // 返回消息
                        listener.send(tmp.makeBackProtocol());
                    }
                }

                //如果原先的状态是未检查过，此时需要开启任务重发一遍，确保消息不会在此被卡住
                if (!preCanSendback && null != needSendBackMsg)
                {
                    //开启任务重发消息
                    ALSynTaskManager.getInstance().regTask(new SynGCReSendReconnectMsgTask(listener, _msg.getCurRecMesCount() + needSendBackMsg.size()));
                }
            }
        });

        //核对已收到消息序号的处理
        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_021_ReceivedMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_021_ReceivedMsg _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener || null == listener.getUSInfo() || !listener.getUSInfo().isInitUSDone())
                {
                    return;
                }

                //System.err.println("chk: " + listener.getMsgDealer().getListFirstSendedMsgSerialize() + " - " + _msg.getCurRecMesCount()
                //                    + " - " + listener.getMsgDealer().getSendBackMsgCount());
                listener.getMsgDealer().checkSendedMsg(_msg.getCurRecMesCount());
            }
        });

        //处理客户端发送来的消息
        this.regHandler(new NPCustomMsgDealer<NPGC2GS_001_022_SendSerializeMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGC2GS_001_022_SendSerializeMsg _msg)
            {
                // 强制转化类型对象
                NPGSGCListener listener = (NPGSGCListener) _receiver;
                if (null == listener || null == listener.getUSInfo() || !listener.getUSInfo().isInitUSDone())
                {
                    return;
                }

                //检测用户数据是否可用
                int checkMsg = listener.getMsgDealer().checkClientMsgSerialize(_msg.getMsgSerialize());
                //0表示重复的消息
                if (checkMsg == 0)
                {
                    System.err.println("rec client serialize[" + _msg.getMsgSerialize() + "] msg multi! RecedCount[" + listener.getMsgDealer().getReceivedMsgCount() + "] Account: " + listener.getUid());
                    return;
                }

                if (checkMsg == -1)
                {
                    //验证序号失败
                    System.err.println("rec client serialize[" + _msg.getMsgSerialize() + "] msg fail! RecedCount[" + listener.getMsgDealer().getReceivedMsgCount() + "] Account: " + listener.getUid());
                    return;
                }

                //直接向Us转发数据
                listener.resendToUS(_msg.getClientRequestSerialize(), _msg.getMsg());

                //返回消息数量变更
                listener.send(NPGS2GCWriter_001_BasicOp.make_021_ReceivedMsg(checkMsg));
            }
        });

    }

    /**
     * 获取最近登录的服务器
     * @param _uid
     * @param _callback
     */
    public static void getRecentLoginServer(String _uid, _ICallBackT<NP_SYS_ServerItem> _callback)
    {
        //去记录服务器请求登录信息
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.RECORD.ordinal(),
                NP2RCS_R_Writer_001_BasicOp.make_001_002_ReqLookupLoginRecord(String.valueOf(_uid))
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2RCS_RB_001_002_RetLookupLoginRecord();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        //获取到登录记录挑选出最后登录的服务器服务器logicId
                        NP2RCS_RB_001_002_RetLookupLoginRecord proto = (NP2RCS_RB_001_002_RetLookupLoginRecord) _proto;
                        NP_SYS_PlayerJoinedUSInfo lastLoginUSInfo = null;
                        for (NP_SYS_PlayerJoinedUSInfo usInfo : proto.getJoinedUSList())
                        {
                            if (usInfo == null)
                            {
                                continue;
                            }
                            if (lastLoginUSInfo == null || lastLoginUSInfo.getLastLoginTimeMs() < usInfo.getLastLoginTimeMs())
                            {
                                lastLoginUSInfo = usInfo;
                            }
                        }

                        _callback.onRunOver(lastLoginUSInfo == null ? null : lastLoginUSInfo.getServerItem());
                    }

                    @Override
                    public void dealFail(int i)
                    {
                        _callback.onRunOver(null);
                    }
                });
    }

    /**
     * 获取推荐服务器信息
     * @param _callback
     */
    public static void getRecommendServerInfo(_ICallBackT<NP_SYS_ServerItem> _callback)
    {
        //去CS请求详细的服务器信息
        NPGateServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.COMMON.ordinal()
                , NP2CS_R_Writer_002_ServerInfoOp.make_008_ReqMostRecommendedUSInfo()
                , new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2CS_RB_002_008_RetMostRecommendedUSInfo();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _proto)
                    {
                        NP2CS_RB_002_008_RetMostRecommendedUSInfo proto = (NP2CS_RB_002_008_RetMostRecommendedUSInfo) _proto;
                        _callback.onRunOver(proto.getServerItem());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _callback.onRunOver(new NP_SYS_ServerItem());
                    }
                });
    }
}
