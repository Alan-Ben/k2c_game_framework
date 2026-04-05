package NPInterfaceServer.NPGeneralListener.RequestDispather.p001_ChatOp;

import NP2IS_R.p001_ISOp.NP2IS_R_001_001_ReqRegRoom;
import NPCommon.Dispather.NPRequestDispatcher.NPRequestDealer;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomInfo;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;
import NPInterfaceServer.NPGeneralListener.NPISBasicServerListener;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NP2IS_R_001_001_ReqRegRoom_Handler extends NPRequestDealer<NP2IS_R_001_001_ReqRegRoom>
{
    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _receiver, NP2IS_R_001_001_ReqRegRoom _msg)
    {
    	NPISBasicServerListener listener = (NPISBasicServerListener) _receiver.getRequestDealer();
    	
    	ChatRoomInfo room = ChatRoomMgr.getInstance().regRoom(listener.getServerType(), listener.getServerTypeId(), _msg.getRoomType(), _msg.getRoomTypeId());
    	
    	room.setLastRegCommiter(_receiver);
    }
}
