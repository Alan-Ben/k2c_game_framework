package NPUSServer.NPGeneralListener.MsgDispather;


import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NP2US_B.p001_BasicOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPCommon.NPVersion;
import NPGameRes.ServerVersionInfo;
import NPServerProtocolWriter.NP2GS.Msg.NP2GS_Writer_001_BasicOp;
import NPUSServer.NPUserServer;
import NPUSServer.SynTask.USGetPayCallbackListTask;
import NPUSServer.USLog;
import WCGCommon.Enum.NPEnum;


/**************
 * 客户端协议处理对象
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time 2020年12月9日 下午10:52:56
 */
public class USBroadMsgDispather extends NPCustomMsgDispatcher
{
    private NPUserServer _m_usUSServer;

    public USBroadMsgDispather(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_001_CSOnline>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_001_CSOnline _msg)
            {
                //同步负载信息给CS
                _m_usUSServer.usServerUpdateHoldInfoToCS();
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_002_PlayerFreeze>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_002_PlayerFreeze _msg)
            {
                long cid = getUSServer().getUserIdxMgr().lookupUserCid(_msg.getUid());
                if (cid == 0)
                {
                    return;
                }
                //如果玩家在线将玩家强制下线
                _m_usUSServer.getUsUserMgr().forceKickUser(cid);
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_004_GSOnline>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_004_GSOnline _msg)
            {
                //同步资源版本信息给CS
                _m_usUSServer.sendMessageToBSServer(NPEnum.EServerType.GATE.ordinal(), _msg.getGsId(),
                        NP2GS_Writer_001_BasicOp.make_005_UpdateUsServerVersion(_m_usUSServer.getServerTypeId(), NPVersion.getString(), ServerVersionInfo.getInstance().getVersion()));
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_005_PayOnline>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_005_PayOnline _msg)
            {
                ALSynTaskManager.getInstance().regTask(new USGetPayCallbackListTask(getUSServer()));
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_006_USInfoListInit>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_006_USInfoListInit _msg)
            {
                _m_usUSServer.synUSOnlineState();
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_003_AllServerMailUpdate>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_003_AllServerMailUpdate _msg)
            {
                _m_usUSServer.getAllServerMailTemplateMgr().refreshMailTemplate(_isSucc ->
                {
                    USLog.info(_usServer, "NP2US_B_001_003_AllServerMailUpdate deal :{}", _isSucc);
                });
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_008_DinnerServerOnline>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_008_DinnerServerOnline _msg)
            {
            	
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_009_MatchAdultServerOnline>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_009_MatchAdultServerOnline _msg)
            {
            	//检查是否再次重新上传所有子嗣联姻数据
               // _m_usUSServer.getMatchAdultPool().checkUploadAllAdultItem();
            }
        });

        this.regHandler(new NPCustomMsgDealer<NP2US_B_001_010_CrossDataServerOnline>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_B_001_010_CrossDataServerOnline _msg)
            {
                //将所有CrossData管理器的数据都全部重新同步
                _m_usUSServer.getCrossDataCore().onCrossDataServerOnline();
            }
        });
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}
}
