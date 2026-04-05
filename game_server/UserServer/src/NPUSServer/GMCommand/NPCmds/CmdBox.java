package NPUSServer.GMCommand.NPCmds;


import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPGameRes.Refs.Share.RefBoxComm;
import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * @author mark
 * @date 2022年4月14日
 */
@ACommander(comment = "宝箱相关命令", name = "box")
public class CmdBox extends UsCmdBase
{
    @ACommand(comment = "开启宝箱（宝箱配置ID）")
    public String openBox(long _boxId)
    {
        RefBoxComm ref = RefBoxComm.getMgr().get(_boxId);
        if (null == ref)
            return "fail, box:" + _boxId;

        CommBoxInfo boxInfo = getUserServer().getCommBoxMgr().buildBox(ref, getOwner().getCid(), getContext());
        //TODO 发送聊天消息
//        getUserServer().sendUSServerChatRoomSystemMsg(ENPChatMsgType.COMM_BOX
//                , getOwner().toChatPlayerProto().makePackage()
//                , boxInfo.toChatProto().makePackage());

        return "ok";
    }

    @ACommand(comment = "清空宝箱领取玩家数（宝箱实例ID 0-全部宝箱，玩家CID 0-全部玩家）")
    public String clearGainedCid(long _instanceId, long _cid)
    {
        getUserServer().getCommBoxMgr().cmdClearGainedCid(_instanceId, _cid);

        return "ok";
    }
}
