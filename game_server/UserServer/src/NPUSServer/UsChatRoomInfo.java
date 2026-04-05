package NPUSServer;

import ChatSystem._AChatRoomInfo;
import NPEnum.ENPChatRoomType;

public class UsChatRoomInfo extends _AChatRoomInfo
{
    public UsChatRoomInfo(NPUserServer _usServer, ENPChatRoomType _roomType, long _roomTypeId)
    {
        super(_usServer, _roomType, _roomTypeId);
    }

    @Override
    protected void _onDiscard()
    {
    }
}
