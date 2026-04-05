
using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class BusinessBuildingLevelRefObj : _IALBasicRefObj, IComparable<BusinessBuildingLevelRefObj>
    {
        public long _refId { get { return id; } }

        public long id;
        public long building_id;
        public int level;
        public long earning_rate;
        public NPCommonCostItem upgrade_cost_item; // 升到下一级的消耗
        public NPGGoIndex override_res_index;
        public NPGTextureIndex override_preview_tex_index;

        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGGoIndex res_index;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGTextureIndex preview_tex_index;
        
        public int CompareTo(BusinessBuildingLevelRefObj _other)
        {
            if (_other == null)
                return -1;
            
            return level.CompareTo(_other.level);    
        }
    }
    public class GSOBusinessBuildingLevelRefSet : _TALSOBasicRefSet<BusinessBuildingLevelRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "business_building_level"; } }
    }
}