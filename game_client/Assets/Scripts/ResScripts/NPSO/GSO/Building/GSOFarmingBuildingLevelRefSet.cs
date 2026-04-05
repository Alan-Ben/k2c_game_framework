
using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class FarmingBuildingLevelRefObj : _IALBasicRefObj, IComparable<FarmingBuildingLevelRefObj>
    {
        public long _refId { get { return id; } }

        public long id;
        public long building_id;
        public int level;
        public long upgrade_cost_num;
        public long upgrade_cost_num_per_level;
        public int earning_rate;
        public int earning_rate_per_level;
        public long tap_to_collect_num;
        public long tap_to_collect_num_per_level;
        public int auto_tap_num_per_sec;
        public int auto_tap_num_per_sec_per_level;
        public NPGGoIndex override_res_index;
        public NPGTextureIndex override_preview_tex_index;
        public CSWeightRandomList multiple_weight_list;//暴击倍数权重列表

        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGGoIndex res_index;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGTextureIndex preview_tex_index;
        
        public int CompareTo(FarmingBuildingLevelRefObj _other)
        {
            if (_other == null)
                return -1;
            
            return level.CompareTo(_other.level);
        }
    }
    public class GSOFarmingBuildingLevelRefSet : _TALSOBasicRefSet<FarmingBuildingLevelRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "farming_building_level"; } }
    }
}