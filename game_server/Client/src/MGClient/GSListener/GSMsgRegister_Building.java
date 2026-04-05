package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p010_BuildingOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Building
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_001_RetBuildingBuild>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_001_RetBuildingBuild _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_002_RetFarmUpgradeLvl>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_002_RetFarmUpgradeLvl _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_003_RetFarmClickOutput>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_003_RetFarmClickOutput _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_004_RetBusinessUpgradeLvl>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_004_RetBusinessUpgradeLvl _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_005_RetBusinessHireEmployee>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_005_RetBusinessHireEmployee _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_006_RetBusinessHireTenEmployees>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_006_RetBusinessHireTenEmployees _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_050_OnBuidingBuilt>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_050_OnBuidingBuilt _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_051_OnFarmChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_051_OnFarmChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_052_OnBusinessChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_052_OnBusinessChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_053_OnFarmClickChg>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_053_OnFarmClickChg _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_054_OnBusinessBuilt>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_054_OnBusinessBuilt _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_055_OnFarmBuilt>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_055_OnFarmBuilt _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_010_056_OnEffectGainBusinessWorkes>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_010_056_OnEffectGainBusinessWorkes _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}