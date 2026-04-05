package NPInterfaceServer.NPGeneralListener.RequestDispather.p001_ChatOp;

import ChatSDK.CallbackInterface._IRegUserDealFunc;
import ChatSDK.Common.Log.CommLog;
import Common.ServerObj.GSLoginCheckObj;
import NP2IS_R.p001_ISOp.NP2IS_R_001_003_ReqRegChatUser;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPInterfaceServer.ChatMgr.ChatMgr;
import NPInterfaceServer.NPGeneralListener.Writer.NP2IS_RB_Writer_001_ISOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;


public class NP2IS_R_001_003_ReqRegChatUser_Handler extends NPRequestDealer<NP2IS_R_001_003_ReqRegChatUser>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2IS_R_001_003_ReqRegChatUser _msg)
    {
    	String chatUid = ChatMgr.getInstance().getChatSdk().getChatUid(String.valueOf(_msg.getCid()));
    	
    	ChatMgr.getInstance().getChatSdk().regUser(chatUid, _msg.getAreaTag(), 
    			new _IRegUserDealFunc() 
    			{
					@Override
					public void dealSuc(GSLoginCheckObj _ret) 
					{
						_receiver.commitSucRes(NP2IS_RB_Writer_001_ISOp.make_003_RetRegChatUser(chatUid, _ret));
						
						CommLog.info("player:{} reg chat user:{} suc.", _msg.getCid(), chatUid);
					}
					
					@Override
					public void dealFail(int _err) 
					{
						_receiver.commitFailRes(_err);
					}
				});
    }
}
