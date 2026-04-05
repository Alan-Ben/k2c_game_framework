package NPUSServer.NPUSUserMgr.ItemDealer.Share;

import NPEnum.ENPChatMsgType;
import NPEnum.ENPChatRoomType;
import NPEnum.ENPShareItemType;
import NPGameRes.Refs.Share.RefBoxComm;
import NPGameRes.Refs.Share.RefShare;
import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class ShareItemDealer_Box extends _AShareItemDealer
{
    @Override
    public ENPShareItemType getShareItem()
    {
        return ENPShareItemType.BOX;
    }

    @Override
    public void gainItem(NPUSUserData _userData, RefShare _shareRef, long _count, NPPlayerContext _context)
    {
        RefBoxComm ref = RefBoxComm.getMgr().get(_shareRef.id);
        if (null == ref)
        {
            USLog.error(_userData.getUSServer(), "ShareItemDealer_Box gainItem, boxRef is null, cid:{} boxId:{}", _userData.getCid(), _shareRef.id);
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            //创建宝箱
            CommBoxInfo boxInfo = _userData.getUSServer().getCommBoxMgr().buildBox(ref, _userData.getCid(), _context);
            //发送聊天消息
            sendChat(_userData, _shareRef.chat_room_list, boxInfo.toChatProto().makePackage());
        }
    }

    @Override
    public void sendChat(NPUSUserData _userData, ArrayList<ENPChatRoomType> _roomTypeList, ByteBuffer _byteContent)
    {
        for (int i = 0; i < _roomTypeList.size(); i++)
        {
            ENPChatRoomType roomType = _roomTypeList.get(i);
            if (null == roomType)
                continue;

            if (roomType == ENPChatRoomType.US_SERVER)
            {
                _userData.getPlayerChatRoomDealer().sendRoomMsg(roomType.ordinal(), 0,
                        ENPChatMsgType.COMM_BOX.ordinal(), _userData.toChatPlayerProto().makePackage(), _byteContent, null);

            } else if (roomType == ENPChatRoomType.GUILD)
            {
                long guildId = _userData.getGuildComponent().getGuildId();
                if (guildId == 0)
                    continue;

                _userData.getPlayerChatRoomDealer().sendRoomMsg(roomType.ordinal(), guildId,
                        ENPChatMsgType.COMM_BOX.ordinal(), _userData.toChatPlayerProto().makePackage(), _byteContent, null);
            }
        }
    }
}
