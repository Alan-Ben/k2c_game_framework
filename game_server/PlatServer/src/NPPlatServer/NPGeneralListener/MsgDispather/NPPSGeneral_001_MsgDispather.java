package NPPlatServer.NPGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2PS.p001_BasicOp.NP2PS_001_002_BroadcastLS;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import WCGBasicPlatServer.BasicServerListener._AWCGBasicServerListener;
import WCGBasicPlatServer.BasicServerMgr.WCGBasicServerMgr;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;

public class NPPSGeneral_001_MsgDispather
{
    public static void init(NPPSGeneralMsgDispather _dispather)
    {
        _dispather.regHandler(new NPCustomMsgDealer<NP2PS_001_002_BroadcastLS>()
        {

            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2PS_001_002_BroadcastLS _msg)
            {
                //对所有LS进行广播
                ArrayList<_AWCGBasicServerListener> listeners = new ArrayList<_AWCGBasicServerListener>();
                WCGBasicServerMgr.getInstance().getBasicServerListener(EServerType.LOGIN.ordinal(), listeners);
                //遍历发送
                for (_AWCGBasicServerListener listener : listeners)
                {
                    listener.send(_msg.get_buffer_Msg());
                }
                ;
            }
        });
    }
}
