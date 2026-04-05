package NPUSServer.NPUserMsgDispather.Write;

import Common.MarsEnum.EMarsBagItemUseTimeType;
import Common.MarsObj.Mars_BuildingGainEnergyResult;
import GS2GC.p039_MarsBuildingOp.*;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.*;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep4_TechComp.MarsTechInfo;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_MarsEnergy;

import java.util.ArrayList;

/**
 * 39 - 火星建筑系统
 * @author mj
 *
 */
public class US2GCWriter_039_MarsBuildingOp
{
    public static GS2GC_039_001_RetCreateBuilding make_001_RetCreateBuilding()
    {
    	GS2GC_039_001_RetCreateBuilding proto = new GS2GC_039_001_RetCreateBuilding();
    	
        return proto;
    }
    
    public static GS2GC_039_002_RetUpgradeBuildingLvl make_002_RetUpgradeBuildingLvl()
    {
    	GS2GC_039_002_RetUpgradeBuildingLvl proto = new GS2GC_039_002_RetUpgradeBuildingLvl();
    	
        return proto;
    }
    
    public static GS2GC_039_003_RetSetHonePowerOn make_003_RetSetHonePowerOn()
    {
    	GS2GC_039_003_RetSetHonePowerOn proto = new GS2GC_039_003_RetSetHonePowerOn();
    	
        return proto;
    }
    
    public static GS2GC_039_004_RetDispatchPeople make_004_RetDispatchPeople()
    {
    	GS2GC_039_004_RetDispatchPeople proto = new GS2GC_039_004_RetDispatchPeople();
    	
        return proto;
    }
    
    public static GS2GC_039_005_RetUpgradeEquipment make_005_RetUpgradeEquipment()
    {
    	GS2GC_039_005_RetUpgradeEquipment proto = new GS2GC_039_005_RetUpgradeEquipment();
    	
        return proto;
    }
    
    public static GS2GC_039_006_RetGetBuildingEnergy make_006_RetGetBuildingEnergy(ArrayList<Mars_BuildingGainEnergyResult> _list)
    {
    	GS2GC_039_006_RetGetBuildingEnergy proto = new GS2GC_039_006_RetGetBuildingEnergy();
    	for(int i = 0; i < _list.size(); i++)
    	{
    		Mars_BuildingGainEnergyResult result = _list.get(i);
    		if(null == result)
    			continue;
    		
    		proto.addList(result);
    	}
    	
        return proto;
    }
    
    public static GS2GC_039_007_RetConfirmCreateBuilding make_007_RetConfirmCreateBuilding()
    {
    	GS2GC_039_007_RetConfirmCreateBuilding proto = new GS2GC_039_007_RetConfirmCreateBuilding();
    	
        return proto;
    }

    public static GS2GC_039_008_RetConfirmUpgradeBuildingLvl make_008_RetConfirmUpgradeBuildingLvl()
    {
        GS2GC_039_008_RetConfirmUpgradeBuildingLvl proto = new GS2GC_039_008_RetConfirmUpgradeBuildingLvl();

        return proto;
    }

    public static GS2GC_039_009_RetHomeCollectTimeChg make_009_RetHomeCollectTimeChg(int _lastCollectTimeS)
    {
        GS2GC_039_009_RetHomeCollectTimeChg proto = new GS2GC_039_009_RetHomeCollectTimeChg();

        proto.setLastHomeOutputCollectTimeS(_lastCollectTimeS);

        return proto;
    }
    
    public static GS2GC_039_010_RetSetBuildingDone make_010_RetSetBuildingDone()
    {
    	GS2GC_039_010_RetSetBuildingDone proto = new GS2GC_039_010_RetSetBuildingDone();
    	
        return proto;
    }
    
    public static GS2GC_039_011_RetCancelBuildingUpgrade make_011_RetCancelBuildingUpgrade()
    {
    	GS2GC_039_011_RetCancelBuildingUpgrade proto = new GS2GC_039_011_RetCancelBuildingUpgrade();
    	
        return proto;
    }
    
    public static GS2GC_039_014_RetBagUseItemForTimeReduce make_014_RetBagUseItemForTimeReduce()
    {
    	GS2GC_039_014_RetBagUseItemForTimeReduce proto = new GS2GC_039_014_RetBagUseItemForTimeReduce();
    	
        return proto;
    }
    
    public static GS2GC_039_020_RetUpgradeTechnologyLvl make_020_RetUpgradeTechnologyLvl()
    {
    	GS2GC_039_020_RetUpgradeTechnologyLvl proto = new GS2GC_039_020_RetUpgradeTechnologyLvl();
    	
        return proto;
    }
    
    public static GS2GC_039_021_RetConfirmUpgradeTechnologyLvl make_021_RetConfirmUpgradeTechnologyLvl()
    {
    	GS2GC_039_021_RetConfirmUpgradeTechnologyLvl proto = new GS2GC_039_021_RetConfirmUpgradeTechnologyLvl();
    	
        return proto;
    }
    
    public static GS2GC_039_022_RetCancelTechnologyUpgrade make_022_RetCancelTechnologyUpgrade()
    {
    	GS2GC_039_022_RetCancelTechnologyUpgrade proto = new GS2GC_039_022_RetCancelTechnologyUpgrade();
    	
        return proto;
    }
    
    public static GS2GC_039_023_RetSetTechnologyDone make_023_RetSetTechnologyDone()
    {
    	GS2GC_039_023_RetSetTechnologyDone proto = new GS2GC_039_023_RetSetTechnologyDone();
    	
        return proto;
    }
    
    public static GS2GC_039_050_OnBuildingChg make_050_OnBuildingChg(MarsBuildingInfo _info)
    {
    	GS2GC_039_050_OnBuildingChg proto = new GS2GC_039_050_OnBuildingChg();
    	proto.setInfo(_info.toProto());
    	
        return proto;
    }
    
    public static GS2GC_039_051_OnHomeChg make_051_OnHomeChg(MarsHomeBuildingFunc _info)
    {
    	GS2GC_039_051_OnHomeChg proto = new GS2GC_039_051_OnHomeChg();
    	proto.setInfo(_info.toHomeBuildingProto());
    	
        return proto;
    }
    
    public static GS2GC_039_052_OnPeopleChg make_052_OnPeopleChg(MarsPeopleBuildingFunc _info)
    {
    	GS2GC_039_052_OnPeopleChg proto = new GS2GC_039_052_OnPeopleChg();
    	proto.setInfo(_info.toPeopleBuildingProto());
    	
        return proto;
    }
    
    public static GS2GC_039_053_OnEquipmentChg make_053_OnEquipmentChg(MarsBuildingEquipmentInfo _info)
    {
    	GS2GC_039_053_OnEquipmentChg proto = new GS2GC_039_053_OnEquipmentChg();
    	proto.setInfo(_info.toProto());
    	
        return proto;
    }
    
    public static GS2GC_039_054_OnEnergyOutputChg make_054_OnEnergyOutputChg(MarsPeopleBuildingFunc _func)
    {
    	GS2GC_039_054_OnEnergyOutputChg proto = new GS2GC_039_054_OnEnergyOutputChg();
    	proto.setInfo(_func.toOutputProto());
    	
        return proto;
    }
    
    public static GS2GC_039_055_OnMarsEnergyChg make_055_OnMarsEnergyChg(SpecialItemDealer_MarsEnergy _dealer)
    {
    	GS2GC_039_055_OnMarsEnergyChg proto = new GS2GC_039_055_OnMarsEnergyChg();
    	proto.setInfo(_dealer.toProto());
    	
        return proto;
    }
    
    public static GS2GC_039_056_OnBuildingUpQueueAdd make_056_OnBuildingUpQueueAdd(MarsBuildingUpQueueInfo _info)
    {
    	GS2GC_039_056_OnBuildingUpQueueAdd proto = new GS2GC_039_056_OnBuildingUpQueueAdd();
    	proto.setInfo(_info.toProto());
    	
        return proto;
    }
    
    public static GS2GC_039_057_OnBuildingUpQueueDel make_057_OnBuildingUpQueueDel(long _id)
    {
    	GS2GC_039_057_OnBuildingUpQueueDel proto = new GS2GC_039_057_OnBuildingUpQueueDel();
    	proto.setId(_id);
    	
        return proto;
    }
    
    public static GS2GC_039_060_OnMarsTechnologyChg make_060_OnMarsTechnologyChg(MarsTechInfo _info)
    {
    	GS2GC_039_060_OnMarsTechnologyChg proto = new GS2GC_039_060_OnMarsTechnologyChg();
    	proto.setInfo(_info.toProto());
    	
        return proto;
    }
    
    public static GS2GC_039_061_OnExtMoodIndexChg make_061_OnExtMoodIndexChg(long _value)
    {
    	GS2GC_039_061_OnExtMoodIndexChg proto = new GS2GC_039_061_OnExtMoodIndexChg();
    	proto.setValue(_value);
    	
        return proto;
    }
    
    public static GS2GC_039_062_OnItemHelpSecsChg make_062_OnItemHelpSecsChg(EMarsBagItemUseTimeType _objType, long _objId, int _helpSecs)
    {
    	GS2GC_039_062_OnItemHelpSecsChg proto = new GS2GC_039_062_OnItemHelpSecsChg();
    	proto.setObjType(_objType);
    	proto.setObjId(_objId);
    	proto.setSecs(_helpSecs);
    	
        return proto;
    }
}