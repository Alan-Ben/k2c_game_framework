package NPUSServer.NPUSUserMgr.PlayerChatRoomDealer;

import NPEnum.ENPChatRoomType;

public class PlayerChatRoomDealerMgr
{
    //单例模式
    private static PlayerChatRoomDealerMgr _g_instance = new PlayerChatRoomDealerMgr();
    public static PlayerChatRoomDealerMgr getInstance()
    {
        return _g_instance;
    }

    //聊天房间处理器数组，索引为聊天房间类型枚举值
    private _IPlayerChatRoomDealer[] _m_arrDealerArr;

    public PlayerChatRoomDealerMgr()
    {
        _m_arrDealerArr = new _IPlayerChatRoomDealer[ENPChatRoomType.ENPChatRoomType_Length];

        regDealer(new PlayerChatRoomUsDealer());//注册单服聊天房间处理器
        regDealer(new PlayerChatRoomGuildDealer());//注册公会聊天房间处理器
        regDealer(new PlayerChatRoomActivityTeamDealer());//注册活动队伍聊天房间处理器
    }

    public void regDealer(_IPlayerChatRoomDealer _dealer)
    {
        _m_arrDealerArr[_dealer.getRoomType().ordinal()] = _dealer;
    }

    public _IPlayerChatRoomDealer getDealer(int _roomType)
    {
        return _m_arrDealerArr[_roomType];
    }
}
