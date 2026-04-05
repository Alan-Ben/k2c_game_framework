package NPUSServer.NPUserMsgDispather.Write;

import GS2GC.p010_BuildingOp.*;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingFarmFunc;

public class US2GCWriter_010_BuildingOp
{
    public static GS2GC_010_001_RetBuildingBuild make_001_RetBuildingBuild()
    {
    	GS2GC_010_001_RetBuildingBuild proto = new GS2GC_010_001_RetBuildingBuild();
    	
    	return proto;
    }

    public static GS2GC_010_002_RetFarmUpgradeLvl make_002_RetFarmUpgradeLvl()
    {
    	GS2GC_010_002_RetFarmUpgradeLvl proto = new GS2GC_010_002_RetFarmUpgradeLvl();
    	
    	return proto;
    }

    public static GS2GC_010_003_RetFarmClickOutput make_003_RetFarmClickOutput()
    {
    	GS2GC_010_003_RetFarmClickOutput proto = new GS2GC_010_003_RetFarmClickOutput();
    	
    	return proto;
    }
    
    public static GS2GC_010_004_RetBusinessUpgradeLvl make_004_RetBusinessUpgradeLvl()
    {
    	GS2GC_010_004_RetBusinessUpgradeLvl proto = new GS2GC_010_004_RetBusinessUpgradeLvl();
    	
    	return proto;
    }
    
    public static GS2GC_010_005_RetBusinessHireEmployee make_005_RetBusinessHireEmployee()
    {
    	GS2GC_010_005_RetBusinessHireEmployee proto = new GS2GC_010_005_RetBusinessHireEmployee();
    	
    	return proto;
    }
    
    public static GS2GC_010_006_RetBusinessHireTenEmployees make_006_RetBusinessHireTenEmployees()
    {
    	GS2GC_010_006_RetBusinessHireTenEmployees proto = new GS2GC_010_006_RetBusinessHireTenEmployees();
    	
    	return proto;
    }

    public static GS2GC_010_007_RetBusinessUnlockProduct make_007_RetBusinessUnlockProduct()
    {
        return new GS2GC_010_007_RetBusinessUnlockProduct();
    }
    
    public static GS2GC_010_050_OnBuidingBuilt make_050_OnBuidingBuilt(BuildingInfo _info)
    {
    	GS2GC_010_050_OnBuidingBuilt proto = new GS2GC_010_050_OnBuidingBuilt();
    	proto.setBuildingId(_info.getBuildingId());
    	
    	return proto;
    }
    
    public static GS2GC_010_051_OnFarmChg make_051_OnFarmChg(BuildingFarmFunc _info)
    {
    	GS2GC_010_051_OnFarmChg proto = new GS2GC_010_051_OnFarmChg();
    	proto.setFarm(_info.toProto());
    	
    	return proto;
    }

    public static GS2GC_010_052_OnBusinessChg make_052_OnBusinessChg(BuildingBusinessFunc _info)
    {
    	GS2GC_010_052_OnBusinessChg proto = new GS2GC_010_052_OnBusinessChg();
    	proto.setBusiness(_info.toProto());
    	
    	return proto;
    }
}
