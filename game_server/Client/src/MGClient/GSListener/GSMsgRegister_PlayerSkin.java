package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p018_PlayerSkinOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_PlayerSkin
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_001_RetSetCommTitle>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_001_RetSetCommTitle _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_002_RetSetComboTitle>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_002_RetSetComboTitle _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_003_RetSetTitleShow>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_003_RetSetTitleShow _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_004_RetUnlockComboTitlePre>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_004_RetUnlockComboTitlePre _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_005_RetUnlockComboTitleSfx>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_005_RetUnlockComboTitleSfx _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_006_RetUnlockComboTitleBg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_006_RetUnlockComboTitleBg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_007_RetviewComboTitlePre>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_007_RetviewComboTitlePre _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_008_RetViewComboTitleSfx>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_008_RetViewComboTitleSfx _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_009_RetViewComboTitleBg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_009_RetViewComboTitleBg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_010_RetUnlockPlayerSkin>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_010_RetUnlockPlayerSkin _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_011_RetSetCurPlayerSkin>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_011_RetSetCurPlayerSkin _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_012_RetUnsetCurPlayerSkin>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_012_RetUnsetCurPlayerSkin _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_013_RetUpgradePlayerSkin>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_013_RetUpgradePlayerSkin _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_015_RetviewTitle>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_015_RetviewTitle _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_050_OnCommTitleChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_050_OnCommTitleChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_051_OnComboTitlePreChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_051_OnComboTitlePreChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_052_OnComboTitleSfxChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_052_OnComboTitleSfxChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_053_OnComboTitleBgChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_053_OnComboTitleBgChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_054_OnCurTitleChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_054_OnCurTitleChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_018_060_OnPlayerSkinChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_018_060_OnPlayerSkinChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}