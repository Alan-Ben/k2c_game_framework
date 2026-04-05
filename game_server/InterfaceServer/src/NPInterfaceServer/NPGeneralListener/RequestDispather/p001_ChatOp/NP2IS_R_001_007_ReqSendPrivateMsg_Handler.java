package NPInterfaceServer.NPGeneralListener.RequestDispather.p001_ChatOp;

import ChatSDK.CallbackInterface._IPrivateChatMsgDealFunc;
import Common.CommObj.Common_Msg;
import NP2IS_R.p001_ISOp.NP2IS_R_001_007_ReqSendPrivateMsg;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPInterfaceServer.ChatMgr.ChatMgr;
import NPInterfaceServer.NPGeneralListener.Writer.NP2IS_RB_Writer_001_ISOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NP2IS_R_001_007_ReqSendPrivateMsg_Handler extends NPRequestDealer<NP2IS_R_001_007_ReqSendPrivateMsg>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2IS_R_001_007_ReqSendPrivateMsg _msg)
    {
    	String receiverChatUid = ChatMgr.getInstance().getChatSdk().getChatUid(String.valueOf(_msg.getReceiverCid()));
    	
    	Common_Msg msg = new Common_Msg();
    	msg.setMsgType(_msg.getMsgType());
    	msg.setGameContent(_msg.get_buffer_GameContent());
    	msg.setGameUser(_msg.get_buffer_GameUser());
    	
    	ChatMgr.getInstance().getChatSdk().sendPrivateMsg(_msg.getSenderChatUid(), receiverChatUid, msg, 
    			new _IPrivateChatMsgDealFunc() 
    			{
					@Override
					public void dealSuc(long _msgId) 
					{
						_receiver.commitSucRes(NP2IS_RB_Writer_001_ISOp.make_007_RetSendPrivateMsg(_msgId));
					}
					
					@Override
					public void dealFail(int _err) 
					{
						_receiver.commitFailRes(_err);
					}
				});
    }
}
