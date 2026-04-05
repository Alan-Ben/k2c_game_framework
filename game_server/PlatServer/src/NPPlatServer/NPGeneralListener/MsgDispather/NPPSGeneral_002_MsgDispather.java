package NPPlatServer.NPGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2PS.p002_GSOp.NP2PS_002_001_ReduceHandleUser;
import NP2PS.p002_GSOp.NP2PS_002_002_AddHandleUser;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import NPPlatServer.NPPS_Listener.PS_GSListener;

public class NPPSGeneral_002_MsgDispather
{
    public static void init(NPPSGeneralMsgDispather _dispather)
    {
        _dispather.regHandler(new NPCustomMsgDealer<NP2PS_002_001_ReduceHandleUser>()
        {

            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2PS_002_001_ReduceHandleUser _msg)
            {
                // 获取服务器对象
                PS_GSListener gsListener = (PS_GSListener) _receiver;
                if (null == gsListener)
                    return;

                //减少处理的用户数量
                gsListener.reduceHandleUser();
            }
        });
        _dispather.regHandler(new NPCustomMsgDealer<NP2PS_002_002_AddHandleUser>()
        {

            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2PS_002_002_AddHandleUser _msg)
            {
                // 获取服务器对象
                PS_GSListener gsListener = (PS_GSListener) _receiver;
                if (null == gsListener)
                    return;

                //减少处理的用户数量
                gsListener.directAddHandleUser();
            }
        });
    }
}
