package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p017_ActivityOp.GS2GC_017_050_OnActivityAdd;
import GS2GC.p017_ActivityOp.GS2GC_017_051_OnActivityChg;
import GS2GC.p017_ActivityOp.GS2GC_017_052_OnActivityRemove;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Activity
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_017_050_OnActivityAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_017_050_OnActivityAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_017_051_OnActivityChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_017_051_OnActivityChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_017_052_OnActivityRemove>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_017_052_OnActivityRemove _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}