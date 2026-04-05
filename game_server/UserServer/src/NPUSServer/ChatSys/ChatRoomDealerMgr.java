package NPUSServer.ChatSys;

import NPEnum.ENPChatRoomType;

public class ChatRoomDealerMgr
{
    //单例模式
    private static ChatRoomDealerMgr _g_instance = new ChatRoomDealerMgr();
    public static ChatRoomDealerMgr getInstance()
    {
        return _g_instance;
    }

    //聊天房间处理器数组，索引为聊天房间类型枚举值
    private _IChatRoomDealer[] _m_arrDealerArr;

    public ChatRoomDealerMgr()
    {
        _m_arrDealerArr = new _IChatRoomDealer[ENPChatRoomType.ENPChatRoomType_Length];

        regDealer(new ChatRoomDealerUs());//注册单服聊天房间处理器
        regDealer(new ChatRoomDealerGuild());//注册公会聊天房间处理器
        regDealer(new ChatRoomDealerActivityTeam());//注册活动队伍聊天房间处理器
    }

    public void regDealer(_IChatRoomDealer _dealer)
    {
        _m_arrDealerArr[_dealer.getRoomType().ordinal()] = _dealer;
    }

    public _IChatRoomDealer getDealer(int _roomType)
    {
        return _m_arrDealerArr[_roomType];
    }
}
