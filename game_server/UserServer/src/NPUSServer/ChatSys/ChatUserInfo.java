package NPUSServer.ChatSys;

import Common.ServerObj.ServerObj_ChatUser;

public class ChatUserInfo
{
    //玩家CID
	private final long _m_lCid;
    //玩家Serial，用于在注销聊天用户时与游戏玩家数据进行校验
    private final long _m_lSerial;
    //玩家在聊天系统的用户凭证
	private final String _m_sChatUid;
	
	public ChatUserInfo(long _cid, long _serial, String _chatUid)
	{
		_m_lCid = _cid;
        _m_lSerial = _serial;
		_m_sChatUid = _chatUid;
	}
	
	public long getCid() {return _m_lCid;}
    public long getSerial() {return _m_lSerial;}
	public String getChatUid() {return _m_sChatUid;}

    public ServerObj_ChatUser toChatUser()
    {
        ServerObj_ChatUser chatUser = new ServerObj_ChatUser();
        chatUser.setCid(_m_lCid);
        chatUser.setChatUid(_m_sChatUid);
        chatUser.setUserSerial(_m_lSerial);

        return chatUser;
    }
}
