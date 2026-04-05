package NPInterfaceServer.NPGeneralListener.RequestDispather.p001_ChatOp;

import NP2IS_R.p001_ISOp.NP2IS_R_001_002_ReqUnRegRoom;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;
import NPInterfaceServer.NPGeneralListener.NPISBasicServerListener;
import NPInterfaceServer.NPGeneralListener.Writer.NP2IS_RB_Writer_001_ISOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NP2IS_R_001_002_ReqUnRegRoom_Handler extends NPRequestDealer<NP2IS_R_001_002_ReqUnRegRoom>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2IS_R_001_002_ReqUnRegRoom _msg)
    {
    	NPISBasicServerListener listener = (NPISBasicServerListener) _receiver.getRequestDealer();
    	
    	ChatRoomMgr.getInstance().unRegRoom(listener.getServerType(), listener.getServerTypeId(), _msg.getRoomType(), _msg.getRoomTypeId());
    	
    	_receiver.commitSucRes(NP2IS_RB_Writer_001_ISOp.make_002_RetUnRegRoom());
    }
}
