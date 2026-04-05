package NPInterfaceServer.NPGeneralListener.RequestDispather.p001_ChatOp;

import ChatSDK.CallbackInterface._ICommonDealFunc;
import ChatSDK.Common.Log.CommLog;
import NP2IS_R.p001_ISOp.NP2IS_R_001_004_ReqJoinRoom;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.ChatErr;
import NPInterfaceServer.ChatMgr.ChatMgr;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomInfo;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;
import NPInterfaceServer.NPGeneralListener.Writer.NP2IS_RB_Writer_001_ISOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NP2IS_R_001_004_ReqJoinRoom_Handler extends NPRequestDealer<NP2IS_R_001_004_ReqJoinRoom>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2IS_R_001_004_ReqJoinRoom _msg)
    {
    	ChatRoomInfo room = ChatRoomMgr.getInstance().lookupBySdkRoomId(_msg.getSdkRoomId());
    	if(null == room)
    	{
    		_receiver.commitFailRes(ChatErr.CHAT_ROOM_NOT_FOUND.getCode());
    		return;
    	}
    	
    	if(!room.isInited())
    	{
    		_receiver.commitFailRes(ChatErr.CHAT_ROOM_NOT_INITED.getCode());
    		return;
    	}

    	ChatMgr.getInstance().getChatSdk().joinRoom(_msg.getChatUid(), room.getSdkRoomId(), 
    			new _ICommonDealFunc() 
    			{
					@Override
					public void dealSuc() 
					{
						_receiver.commitSucRes(NP2IS_RB_Writer_001_ISOp.make_004_RetJoinRoom());
					
						CommLog.info("chatUser:{} join room:{} roomType:{} suc.", _msg.getChatUid(), room.getSdkRoomId(), room.getRoomType());
					}
					
					@Override
					public void dealFail(int _err) 
					{
						_receiver.commitFailRes(ChatErr.CHAT_USER_JOIN_ROOM_FAIL.getCode());
					}
				});
    }
}
