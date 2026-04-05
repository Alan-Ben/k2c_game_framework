package NPInterfaceServer.NPGeneralListener.RequestDispather.p001_ChatOp;

import ChatSDK.CallbackInterface._ICommonDealFunc;
import Common.CommObj.Common_Msg;
import NP2IS_R.p001_ISOp.NP2IS_R_001_006_ReqSendRoomMsg;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPCommon.ErrMain.ChatErr;
import NPInterfaceServer.ChatMgr.ChatMgr;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomInfo;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;
import NPInterfaceServer.NPGeneralListener.Writer.NP2IS_RB_Writer_001_ISOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NP2IS_R_001_006_ReqSendRoomMsg_Handler extends NPRequestDealer<NP2IS_R_001_006_ReqSendRoomMsg>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2IS_R_001_006_ReqSendRoomMsg _msg)
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
    	
    	Common_Msg msg = new Common_Msg();
    	msg.setMsgType(_msg.getMsgType());
    	if(null != _msg.get_buffer_GameUser())
    		msg.setGameUser(_msg.get_buffer_GameUser());
    	msg.setGameContent(_msg.get_buffer_GameContent());
    	
    	ChatMgr.getInstance().getChatSdk().sendRoomMsg(_msg.getChatUid(), _msg.getSdkRoomId(), msg, 
    			new _ICommonDealFunc() 
    			{
					@Override
					public void dealSuc() 
					{
						_receiver.commitSucRes(NP2IS_RB_Writer_001_ISOp.make_006_RetSendRoomMsg());
					}
					
					@Override
					public void dealFail(int _err) 
					{
						_receiver.commitFailRes(_err);
					}
				});
    }
}
