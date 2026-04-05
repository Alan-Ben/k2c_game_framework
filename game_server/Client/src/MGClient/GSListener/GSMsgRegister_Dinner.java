package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p019_DinnerOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Dinner
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_001_RetStartDinner>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_001_RetStartDinner _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_002_RetStartDinnerByPermit>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_002_RetStartDinnerByPermit _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_003_RetJoinedPlayerList>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_003_RetJoinedPlayerList _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_004_RetGetStartLogIdxList>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_004_RetGetStartLogIdxList _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_005_RetGetStartLogInfo>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_005_RetGetStartLogInfo _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_006_RetGetDinnerIdxList>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_006_RetGetDinnerIdxList _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_007_RetGetDinnerInfo>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_007_RetGetDinnerInfo _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_008_RetJoinDinner>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_008_RetJoinDinner _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_009_RetTakeOpenReward>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_009_RetTakeOpenReward _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_011_RetGetPreDinnerInfo>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_011_RetGetPreDinnerInfo _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_012_RetGetNextDinnerInfo>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_012_RetGetNextDinnerInfo _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_013_RetSendInviteToPlayer>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_013_RetSendInviteToPlayer _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_014_RetCheckDinnerInvite>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_014_RetCheckDinnerInvite _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_050_OnDinnerAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_050_OnDinnerAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_051_OnDinnerEnd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_051_OnDinnerEnd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_052_OnPermitAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_052_OnPermitAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_019_053_OnJoinerAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_019_053_OnJoinerAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}