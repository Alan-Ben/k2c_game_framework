package NPInterfaceServer.ChatMgr;

import ChatSDK.ChatSDK;
import Common.CommEnum.EISChatClientType;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPInterfaceServer.ChatSDKConf;

public class ChatMgr implements _IHandlerHolder
{
    private static final ChatMgr _g_instance = new ChatMgr();
    public static ChatMgr getInstance()
    {
        return _g_instance;
    }

    
    //聊天服务器接口
    private final ChatSDK _m_csChatSdk;

    //SDK序列号，用于重启时的检查
    private long _m_lChatSdkSerial;
    
    private ChatMgr()
    {
    	_m_csChatSdk = new ChatSDK(EISChatClientType.SDK, 
    			ChatSDKConf.getInstance().getSystemId(), 
    			ChatSDKConf.getInstance().getSystemTag(), 
    			ChatSDKConf.getInstance().getConnectIp(), 
    			ChatSDKConf.getInstance().getConnectPort(), 
    			ChatSDKConf.getInstance().getConnectUser(), 
    			ChatSDKConf.getInstance().getConnectPassword(), 
    			new ChatClientLoginSucDealer());
    	
    	_m_csChatSdk.setRoomErrTask(new ChatRoomFailDealer());
    }
    
    public ChatSDK getChatSdk() {return _m_csChatSdk;} 
    
    public long getChatSdkSerial() {return _m_lChatSdkSerial;}
    public void setChatSdkSerial(long _chatSdkSerial) 
    {
    	_m_lChatSdkSerial = _chatSdkSerial;
	}
}
