package NPUSServer.NPGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2US.p001_BasicOp.NP2US_001_001_ToUSUserMsg;
import NP2US.p001_BasicOp.NP2US_001_002_SendbackUserMsg;
import NP2US.p001_BasicOp.NP2US_001_003_USInfoChg;
import NP2US.p001_BasicOp.NP2US_001_004_BroadUserMsg;
import NP2US.p001_BasicOp.NP2US_001_005_TriggerPlayerEvent;
import NP2US.p001_BasicOp.NP2US_001_006_BroadTriggerPlayerEvent;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

public class NPUSGeneral_001_MsgDispather
{
    public static void init(NPUSGeneralMsgDispather _dispather)
    {
        final NPUserServer usServer = _dispather.getUSServer();

        _dispather.regHandler(new NPCustomMsgDealer<NP2US_001_001_ToUSUserMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_001_001_ToUSUserMsg _msg)
            {
                //查询对应的用户对象，并处理对应的用户消息
                NPUSUserData userData = usServer.getUsUserMgr().lookupCacheUserData(_msg.getCid());
                if (null == userData || userData.getSerialize() != _msg.getSerialize())
                    return;

                //加入处理数据并进行处理
                userData.getMsgMgr().addMessage(_msg.getClientRequestSerialize(), _msg.get_buffer_Msg());
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2US_001_002_SendbackUserMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_001_002_SendbackUserMsg _msg)
            {
                // 查询对应的用户对象，并处理对应的用户消息
                NPUSUserData userData = usServer.getUsUserMgr().lookupCacheUserData(_msg.getCid());
                if (null == userData)
                {
                    return;
                }
                // 加入处理数据并进行处理
                userData.sendMsgToGC(_msg.get_buffer_Msg());
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2US_001_003_USInfoChg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_001_003_USInfoChg _msg)
            {
                usServer.setUsState(_msg.getServerItem().getOnlineStateTypeId(),
                        _msg.getServerItem().getShowStateTypeId(), _msg.getServerItem().getStartDate());
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2US_001_004_BroadUserMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_001_004_BroadUserMsg _msg)
            {
                // 遍历目标cid列表，只处理本服玩家
                for (long cid : _msg.getCidList())
                {
                    if (CommonFunc.parseServerTypeIdFromCid(cid) != usServer.getServerTypeId())
                    {
                        continue;
                    }

                    NPUSUserData userData = usServer.getUsUserMgr().lookupCacheUserData(cid);
                    if (null == userData)
                    {
                        continue;
                    }

                    userData.safeCall(()->
                    {
                        userData.sendMsgToGC(_msg.get_buffer_MsgBuffer());
                    });
                }
            }
        });

        // 远程触发指定玩家的逻辑事件（单人）
        _dispather.regHandler(new NPCustomMsgDealer<NP2US_001_005_TriggerPlayerEvent>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_001_005_TriggerPlayerEvent _msg)
            {
                NPUSUserData userData = usServer.getUsUserMgr().lookupCacheUserData(_msg.getCid());
                if (null == userData)
                {
                    return;
                }

                userData.safeCall(() ->
                {
                    // 独立生成 context，gameEvent 使用 REMOTE_EVENT
                    NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.REMOTE_EVENT);

                    _ALogicEventBase event = _ALogicEventBase.createFromNameAndParams(_msg.getEventName(), _msg.getParamList(), context);
                    if (null == event)
                    {
                        USLog.error(usServer, "TriggerPlayerEvent failed: eventName={}, cid={}", _msg.getEventName(), _msg.getCid());
                        return;
                    }

                    userData.onLogicEvent(event);
                });
            }
        });

        // 远程广播触发多个玩家的逻辑事件
        _dispather.regHandler(new NPCustomMsgDealer<NP2US_001_006_BroadTriggerPlayerEvent>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_001_006_BroadTriggerPlayerEvent _msg)
            {
                String eventName = _msg.getEventName();

                // 遍历目标cid列表，只处理本服玩家
                for (long cid : _msg.getCidList())
                {
                    if (CommonFunc.parseServerTypeIdFromCid(cid) != usServer.getServerTypeId())
                    {
                        continue;
                    }

                    NPUSUserData userData = usServer.getUsUserMgr().lookupCacheUserData(cid);
                    if (null == userData)
                    {
                        continue;
                    }

                    userData.safeCall(() ->
                    {
                        // 每个cid独立生成 context，gameEvent 使用 REMOTE_EVENT
                        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.REMOTE_EVENT);

                        _ALogicEventBase event = _ALogicEventBase.createFromNameAndParams(eventName, _msg.getParamList(), context);
                        if (null == event)
                        {
                            USLog.error(usServer, "BroadTriggerPlayerEvent failed: eventName={}, cid={}", eventName, cid);
                            return;
                        }

                        userData.onLogicEvent(event);
                    });
                }
            }
        });
    }
}
