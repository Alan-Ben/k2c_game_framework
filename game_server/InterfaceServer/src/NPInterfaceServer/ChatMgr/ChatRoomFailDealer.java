package NPInterfaceServer.ChatMgr;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ChatSDK.Common.Log.CommLog;
import ChatSDK.Common.Task._IDealerTask_2T;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;

public class ChatRoomFailDealer implements _IDealerTask_2T<Long, Long>
{
	@Override
	public void run(Long _roomId, Long _roomSerial) 
	{
		CommLog.info("=========== Chat Sdk Room Err, Id:{} Serial:{} ===========", _roomId, _roomSerial);
		
		//房间错误时触发处理
		ALSynTaskManager.getInstance().regTask(()->
		{
			ChatRoomMgr.getInstance().onRoomErr(_roomId, _roomSerial);
		});
	}
}
