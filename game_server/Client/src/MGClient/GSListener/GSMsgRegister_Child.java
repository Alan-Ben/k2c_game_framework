package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p014_ChildOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Child
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_001_RetSetChildName>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_001_RetSetChildName _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_002_RetTrainChild>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_002_RetTrainChild _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_003_RetAddSeatEnergy>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_003_RetAddSeatEnergy _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_004_RetSetChildGraduate>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_004_RetSetChildGraduate _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_006_RetGetUnMarriedAdult>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_006_RetGetUnMarriedAdult _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_007_RetGetMarriedAdult>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_007_RetGetMarriedAdult _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_008_RetGetToMeApply>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_008_RetGetToMeApply _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_009_RetRefuseToMeApply>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_009_RetRefuseToMeApply _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_010_RetAkeyRefuseToMeApply>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_010_RetAkeyRefuseToMeApply _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_011_RetAgreeToMeApply>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_011_RetAgreeToMeApply _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_012_RetGetRecommendPlayerList>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_012_RetGetRecommendPlayerList _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_013_RetApplyToPlayer>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_013_RetApplyToPlayer _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_014_RetApplyToGroup>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_014_RetApplyToGroup _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_015_RetAgreeApplyGroup>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_015_RetAgreeApplyGroup _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_016_RetCancelApplyToPlayer>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_016_RetCancelApplyToPlayer _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_017_RetCancelApplyToGroup>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_017_RetCancelApplyToGroup _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_018_RetGetPoolAdult>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_018_RetGetPoolAdult _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_050_OnChildAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_050_OnChildAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_051_OnChildNameChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_051_OnChildNameChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_052_OnChildLvlChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_052_OnChildLvlChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_053_OnSeatChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_053_OnSeatChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_054_OnUnmarriedAdultAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_054_OnUnmarriedAdultAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_055_OnMarriedAdultAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_055_OnMarriedAdultAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_056_OnToMeApplyAdd>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_056_OnToMeApplyAdd _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_057_OnChildRemove>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_057_OnChildRemove _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_058_OnToMeApplyDel>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_058_OnToMeApplyDel _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_059_OnUnmarriedAdultStatusChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_059_OnUnmarriedAdultStatusChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_060_OnToMeApplyClear>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_060_OnToMeApplyClear _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_061_OnChildBonusSumChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_061_OnChildBonusSumChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_062_OnChildBonusChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_062_OnChildBonusChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_014_063_OnAdultBonusSumChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_014_063_OnAdultBonusSumChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}