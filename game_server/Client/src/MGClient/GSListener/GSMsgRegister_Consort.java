package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p015_ConsortOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Consort
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_001_RetUpgradeFettersLvl>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_001_RetUpgradeFettersLvl _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_002_RetUnderstandBusinessSkill>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_002_RetUnderstandBusinessSkill _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_003_RetUpgradeBlessSkill>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_003_RetUpgradeBlessSkill _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_004_RetCallRand>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_004_RetCallRand _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_005_RetCallAkey>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_005_RetCallAkey _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_006_RetCallAppoint>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_006_RetCallAppoint _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_007_RetSetCurSkin>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_007_RetSetCurSkin _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_008_RetGetRoundTravelCount>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_008_RetGetRoundTravelCount _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_009_RetGetAllBusinessSkill>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_009_RetGetAllBusinessSkill _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_010_RetUpgradeHaloLvl>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_010_RetUpgradeHaloLvl _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_011_RetGetCgUnlockReward>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_011_RetGetCgUnlockReward _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_012_RetUnlockSkin>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_012_RetUnlockSkin _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_013_RetUnlockHalo>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_013_RetUnlockHalo _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_050_OnConsortAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_050_OnConsortAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_051_OnConsortIntimacyChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_051_OnConsortIntimacyChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_052_OnConsortCharmChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_052_OnConsortCharmChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_053_OnConsortCharmPointChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_053_OnConsortCharmPointChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_054_OnTriggeredCallDlgIdAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_054_OnTriggeredCallDlgIdAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_055_OnSkinAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_055_OnSkinAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_056_OnCurSkinChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_056_OnCurSkinChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_057_OnFettersChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_057_OnFettersChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_058_OnBusinessSkillChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_058_OnBusinessSkillChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_059_OnBlessSkillChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_059_OnBlessSkillChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_060_OnHaloChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_060_OnHaloChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_061_OnCgChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_061_OnCgChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_015_062_OnRandCallConsortChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_015_062_OnRandCallConsortChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}