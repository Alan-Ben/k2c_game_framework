package NPGateServer.NPGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2GS.p001_BasicOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPCommon.Log.CommLog;
import NPGateServer.NPGCListener.NPGSGCListener;
import NPGateServer.NPGCListener.Writer.NPGS2GCWriter_001_BasicOp;
import NPGateServer.NPGCMsgMgr.NPGCMsgDealer;
import NPGateServer.NPGCMsgMgr.NPGCMsgItem._ANPGSMsgItem;
import NPGateServer.NPGCMsgMgr.NPGCMsgMgr;
import NPGateServer.NPGSGCMgr.NPGSGCMgr;
import NPGateServer.NPGateServer;
import NPGateServer.NPGeneralListener.NPGSBasicServerListener;
import NPGateServer.USRefVersionMgr.USVersionInfo;
import NPGateServer.USRefVersionMgr.USVersionMgr;
import NPServerProtocolWriter.NP2RCS.Request.NP2RCS_R_Writer_001_BasicOp;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;
import WCGCommon.Enum.NPEnum.EWCGKickOutGateType;

/**************
 * 客户端协议处理对象
 * @author Administrator
 *
 */
public class NPGSGeneralMsgDispather extends NPCustomMsgDispatcher
{
    private static NPGSGeneralMsgDispather _g_instance = new NPGSGeneralMsgDispather();

    public static NPGSGeneralMsgDispather getInstance()
    {

        return _g_instance;
    }

    NPGSGeneralMsgDispather()
    {
        this.regHandler(new NPCustomMsgDealer<NP2GS_001_001_SendbackUserMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_001_001_SendbackUserMsg _msg)
            {
                //查询用户连接对象
                NPGSGCListener gcListener = NPGSGCMgr.getInstance().getGCListener(_msg.getSessionId());
                if (null == gcListener)
                {
                    //此时根据消息的处理序列号寻找对应对象并缓存
                    NPGCMsgDealer msgDealer = NPGCMsgMgr.getInstance().tryGetMsgDealerBySerialize(_msg.getDealerSerialize());
                    if (null != msgDealer)
                        msgDealer.addSendBackMsg(_msg, false);

                    return;
                }

                //检测是否允许发送
                if (!gcListener.getMsgDealer().checkSendAble(_msg))
                    return;

//                System.err.println("send: " + _msg.getMsg()[0] + " - " + _msg.getMsg()[1] + " - "
//                    + (gcListener.getMsgDealer().getListFirstSendedMsgSerialize() + gcListener.getMsgDealer().getSendBackMsgCount()));

                //判断消息处理对象是否一致，不一致则直接不需要处理
                if (gcListener.getMsgDealer().getSerialize() != _msg.getDealerSerialize())
                    return;

                //添加发送的消息
                _ANPGSMsgItem msgItem = gcListener.getMsgDealer().addSendBackMsg(_msg, true);
                if (null == msgItem)
                    return;

                gcListener.send(msgItem.makeBackProtocol());
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2GS_001_002_UserGateKicked>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_001_002_UserGateKicked _msg)
            {
                //查询用户连接对象
                NPGSGCListener gcListener = NPGSGCMgr.getInstance().getGCListener(_msg.getSessionId());
                if (null == gcListener)
                {
                    return;
                }

                EWCGKickOutGateType kickType = EWCGKickOutGateType.values()[_msg.getKickType()];

                if (kickType == EWCGKickOutGateType.DEVICE)
                {
                    //被设备踢出走特殊处理
                    gcListener.onDeviceKickout();
                } else
                {
                    gcListener.onGCUSKickout(_msg.getKickType());
                }
            }
        });
        this.regHandler(new NPCustomMsgDealer<NP2GS_001_004_OnEnterUSButFreeze>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_001_004_OnEnterUSButFreeze _msg)
            {

                //查询用户连接对象
                NPGSGCListener gcListener = NPGSGCMgr.getInstance().getGCListener(_msg.getGsSessionId());
                if (null == gcListener)
                    return;

                if (null == gcListener.getUSInfo())
                    return;

                gcListener.send(NPGS2GCWriter_001_BasicOp.make_013_EnterUSButFreeze(gcListener.getUSInfo().getClientInitSerialize()
                        , _msg.getFreezeTimeMs()));
                //断开连接
                gcListener.unregUSInfo(_msg.getInfoSerialize());
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2GS_001_003_OnUserDataLoaded>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_001_003_OnUserDataLoaded _msg)
            {
                //判断错误并打印
                if (_msg.getErrCode() != 0)
                {
                    CommLog.error("Load Cid:{} fail! errCode:{}!", _msg.getCid(), _msg.getErrCode());
                    return;
                }

                //查询用户连接对象
                NPGSGCListener gcListener = NPGSGCMgr.getInstance().getGCListener(_msg.getGsSessionId());
                if (null == gcListener)
                    return;

                if (null == gcListener.getUSInfo())
                    return;

                //调用初始化完成处理
                gcListener.getUSInfo().onGCUSInited(_msg.getInfoSerialize(), _msg.getCid());
                //更新登录记录到RecordServer
                NPGateServer.getInstance().sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.RECORD.ordinal()
                        , NP2RCS_R_Writer_001_BasicOp.make_001_001_ReqUpdateLoginRecord(_msg.getCid(), gcListener.getUid(),
                                gcListener.getUSInfo().getUSLogicId(), gcListener.getUSInfo().getUSId()));

            }
        });


        this.regHandler(new NPCustomMsgDealer<NP2GS_001_010_SendbackUserClientRequest>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_001_010_SendbackUserClientRequest _msg)
            {
                //查询用户连接对象
                NPGSGCListener gcListener = NPGSGCMgr.getInstance().getGCListener(_msg.getSessionId());
                if (null == gcListener)
                {
                    //此时根据消息的处理序列号寻找对应对象并缓存
                    NPGCMsgDealer msgDealer = NPGCMsgMgr.getInstance().tryGetMsgDealerBySerialize(_msg.getDealerSerialize());
                    if (null != msgDealer)
                        msgDealer.addSendBackMsg(_msg, false);

                    return;
                }

                //检测是否允许发送
                if (!gcListener.getMsgDealer().checkSendAble(_msg))
                    return;

//                System.err.println("send: " + _msg.getMsg()[0] + " - " + _msg.getMsg()[1] + " - "
//                    + (gcListener.getMsgDealer().getListFirstSendedMsgSerialize() + gcListener.getMsgDealer().getSendBackMsgCount()));

                //判断消息处理对象是否一致，不一致则直接不需要处理
                if (gcListener.getMsgDealer().getSerialize() != _msg.getDealerSerialize())
                    return;

                //添加发送的消息
                _ANPGSMsgItem msgItem = gcListener.getMsgDealer().addSendBackMsg(_msg, true);
                if (null == msgItem)
                    return;

                gcListener.send(msgItem.makeBackProtocol());
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2GS_001_005_UpdateUsServerInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2GS_001_005_UpdateUsServerInfo _msg)
            {
                if (_receiver == null)
                    return;

                NPGSBasicServerListener serverListener = (NPGSBasicServerListener) _receiver;
                if (serverListener.getServerType() != EServerType.USER.ordinal())
                    return;

                //是US数据则检索数据对象
                USVersionInfo info = USVersionMgr.getInstance().getOrCreateUSRefVersionInfo(_msg.getUsId());

                info.updateServerInfo(_msg.getServerVersion(), _msg.getResVersion());
            }
        });
    }
}
