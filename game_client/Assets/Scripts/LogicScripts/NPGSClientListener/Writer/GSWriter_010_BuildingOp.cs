using GC2GS.p010_BuildingOp;

namespace GOE
{
    public static class GSWriter_010_BuildingOp
    {
        public static GC2GS_010_001_ReqBuildingBuild make_001_ReqBuildingBuild(long _buildingId)
        {
            GC2GS_010_001_ReqBuildingBuild protocol = new GC2GS_010_001_ReqBuildingBuild();
            protocol.setBuildingId(_buildingId);
            return protocol;
        }
        public static GC2GS_010_002_ReqFarmUpgradeLvl make_002_ReqFarmUpgradeLvl(long _buildingId)
        {
            GC2GS_010_002_ReqFarmUpgradeLvl protocol = new GC2GS_010_002_ReqFarmUpgradeLvl();
            protocol.setBuildingId(_buildingId);
            return protocol;
        }
        public static GC2GS_010_003_ReqFarmClickOutput make_003_ReqFarmClickOutput(long _buildingId)
        {
            GC2GS_010_003_ReqFarmClickOutput protocol = new GC2GS_010_003_ReqFarmClickOutput();
            protocol.setBuildingId(_buildingId);
            return protocol;
        }
        public static GC2GS_010_004_ReqBusinessUpgradeLvl make_004_ReqBusinessUpgradeLvl(long _buildingId)
        {
            GC2GS_010_004_ReqBusinessUpgradeLvl protocol = new GC2GS_010_004_ReqBusinessUpgradeLvl();
            protocol.setBuildingId(_buildingId);
            return protocol;
        }
        public static GC2GS_010_005_ReqBusinessHireEmployee make_005_ReqBusinessHireEmployee(long _buildingId)
        {
            GC2GS_010_005_ReqBusinessHireEmployee protocol = new GC2GS_010_005_ReqBusinessHireEmployee();
            protocol.setBuildingId(_buildingId);
            return protocol;
        }
        public static GC2GS_010_006_ReqBusinessHireTenEmployees make_006_ReqBusinessHireTenEmployees(long _buildingId)
        {
            GC2GS_010_006_ReqBusinessHireTenEmployees protocol = new GC2GS_010_006_ReqBusinessHireTenEmployees();
            protocol.setBuildingId(_buildingId);
            return protocol;
        }
        public static GC2GS_010_007_ReqBusinessUnlockProduct make_007_ReqBusinessUnlockProduct(long _productId)
        {
            GC2GS_010_007_ReqBusinessUnlockProduct protocol = new GC2GS_010_007_ReqBusinessUnlockProduct();
            protocol.setRefId(_productId);
            return protocol;
        }
    }
}