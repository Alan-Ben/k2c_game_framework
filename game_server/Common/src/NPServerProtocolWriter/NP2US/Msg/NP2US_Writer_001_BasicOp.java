package NPServerProtocolWriter.NP2US.Msg;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.NpServerObj.NpServerObj_SYS_ServerItem;
import NP2US.p001_BasicOp.NP2US_001_001_ToUSUserMsg;
import NP2US.p001_BasicOp.NP2US_001_002_SendbackUserMsg;
import NP2US.p001_BasicOp.NP2US_001_003_USInfoChg;
import NP2US.p001_BasicOp.NP2US_001_004_BroadUserMsg;
import NP2US.p001_BasicOp.NP2US_001_005_TriggerPlayerEvent;
import NP2US.p001_BasicOp.NP2US_001_006_BroadTriggerPlayerEvent;

import java.util.ArrayList;
import java.nio.ByteBuffer;

public class NP2US_Writer_001_BasicOp
{
    public static NP2US_001_001_ToUSUserMsg make_001_ToUSUserMsg(long _cid, long _serialize, long _clientRequestSerialize, byte[] _msg)
    {
        NP2US_001_001_ToUSUserMsg protocol = new NP2US_001_001_ToUSUserMsg();

        protocol.setCid(_cid);
        protocol.setSerialize(_serialize);
        protocol.setClientRequestSerialize(_clientRequestSerialize);
        protocol.setMsg(_msg);

        return protocol;
    }

    public static NP2US_001_001_ToUSUserMsg make_001_ToUSUserMsg(long _cid, long _serialize, java.nio.ByteBuffer _msg)
    {
        NP2US_001_001_ToUSUserMsg protocol = new NP2US_001_001_ToUSUserMsg();

        protocol.setCid(_cid);
        protocol.setSerialize(_serialize);
        protocol.setMsg(_msg);

        return protocol;
    }

    public static NP2US_001_002_SendbackUserMsg make_002_SendbackUserMsg(long _cid, _IALProtocolStructure _msg)
    {
        NP2US_001_002_SendbackUserMsg protocol = new NP2US_001_002_SendbackUserMsg();

        protocol.setCid(_cid);
        protocol.setMsg(_msg.makeFullPackage());

        return protocol;
    }

    public static NP2US_001_002_SendbackUserMsg make_002_SendbackUserMsg(long _cid, ByteBuffer _msg)
    {
        NP2US_001_002_SendbackUserMsg protocol = new NP2US_001_002_SendbackUserMsg();

        protocol.setCid(_cid);
        protocol.setMsg(_msg);

        return protocol;
    }

    public static NP2US_001_003_USInfoChg make_003_USInfoChg(NpServerObj_SYS_ServerItem _serverItem)
    {
        return new NP2US_001_003_USInfoChg(_serverItem);
    }

    public static NP2US_001_004_BroadUserMsg make_004_BroadUserMsg(ArrayList<Long> _cidList, ByteBuffer _msgBuffer)
    {
        NP2US_001_004_BroadUserMsg protocol = new NP2US_001_004_BroadUserMsg();
        protocol.getCidList().addAll(_cidList);
        protocol.setMsgBuffer(_msgBuffer);
        return protocol;
    }

    /** 远程触发指定玩家的逻辑事件（单人） */
    public static NP2US_001_005_TriggerPlayerEvent make_005_TriggerPlayerEvent(long _cid, String _eventName, ArrayList<Long> _paramList)
    {
        NP2US_001_005_TriggerPlayerEvent protocol = new NP2US_001_005_TriggerPlayerEvent();
        protocol.setCid(_cid);
        protocol.setEventName(_eventName);
        if (null != _paramList)
        {
            protocol.getParamList().addAll(_paramList);
        }
        return protocol;
    }

    /** 远程广播触发多个玩家的逻辑事件 */
    public static NP2US_001_006_BroadTriggerPlayerEvent make_006_BroadTriggerPlayerEvent(ArrayList<Long> _cidList, String _eventName, ArrayList<Long> _paramList)
    {
        NP2US_001_006_BroadTriggerPlayerEvent protocol = new NP2US_001_006_BroadTriggerPlayerEvent();
        protocol.getCidList().addAll(_cidList);
        protocol.setEventName(_eventName);
        if (null != _paramList)
        {
            protocol.getParamList().addAll(_paramList);
        }
        return protocol;
    }
}
