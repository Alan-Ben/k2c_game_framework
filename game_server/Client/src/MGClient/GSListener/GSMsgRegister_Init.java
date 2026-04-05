package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p002_InitOp.GS2GC_002_009_RetAnecdoteInit;
import GS2GC.p002_InitOp.GS2GC_002_060_RetDinnerInit;
import GS2GC.p002_InitOp.GS2GC_002_082_RetGuildBoxInit;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Init
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_002_060_RetDinnerInit>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_002_060_RetDinnerInit _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_002_009_RetAnecdoteInit>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_002_009_RetAnecdoteInit _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_002_082_RetGuildBoxInit>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_002_082_RetGuildBoxInit _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}