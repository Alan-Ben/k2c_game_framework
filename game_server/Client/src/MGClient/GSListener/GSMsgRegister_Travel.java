package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p008_TravelOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Travel
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_001_RetStartTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_001_RetStartTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_002_RetAkeyTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_002_RetAkeyTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_003_RetDealRewardTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_003_RetDealRewardTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_004_RetDealConsortBarTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_004_RetDealConsortBarTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_005_RetDealChangeTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_005_RetDealChangeTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_006_RetDealInvitationTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_006_RetDealInvitationTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_007_RetDealAddPowerTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_007_RetDealAddPowerTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_008_RetDealConsortLikeTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_008_RetDealConsortLikeTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_009_RetDealConsortIntimacyTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_009_RetDealConsortIntimacyTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_010_RetDealGiftedTravel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_010_RetDealGiftedTravel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_050_OnEventAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_050_OnEventAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_051_OnEventDel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_051_OnEventDel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_008_052_OnConsortChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_008_052_OnConsortChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}