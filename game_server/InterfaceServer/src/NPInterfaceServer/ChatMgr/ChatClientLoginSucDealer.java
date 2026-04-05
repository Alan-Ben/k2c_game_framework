package NPInterfaceServer.ChatMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ChatSDK.Common.Log.CommLog;
import ChatSDK.Common.Task._IDealerTask_1T;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;

public class ChatClientLoginSucDealer implements _IDealerTask_1T<Long>
{
	@Override
	public void run(final Long _sdkSerial) 
	{
		CommLog.info("=========== Chat Sdk Login Suc ===========");
		
		ChatMgr.getInstance().setChatSdkSerial(_sdkSerial);
		
		//重置房间状态并重新发起注册
		ALSynTaskManager.getInstance().regTask(()->
		{
			//重置房间数据
			ChatRoomMgr.getInstance().resetAll(_sdkSerial);
		});
	}
}
