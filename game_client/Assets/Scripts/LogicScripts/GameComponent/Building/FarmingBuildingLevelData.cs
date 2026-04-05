using CommonEnum;

namespace GOE
{
    public class FarmingBuildingLevelData
    {
        public int level;
        public NPCommonCostItem upgrade_cost_item;
        public int earning_rate;
        public long tap_to_collect_num;
        public int auto_tap_num_per_sec;
        public NPGGoIndex res_index;
        public NPGTextureIndex preview_tex_index;
        
        public PlayerBonusPropertyModifier bonus_modify;

        public FarmingBuildingLevelData(FarmingBuildingRefObj _baseRef, FarmingBuildingLevelRefObj _levelRef, int _level)
        {
            level = _level;
            if (_levelRef == null)
            {
                upgrade_cost_item = null;
                earning_rate = 0;
                tap_to_collect_num = 0;
                auto_tap_num_per_sec = 0;
                res_index = null;
                preview_tex_index = null;
                bonus_modify = null;
                return;
            }

            long upgrade_cost_num = _levelRef.upgrade_cost_num + _levelRef.upgrade_cost_num_per_level * (_level - _levelRef.level);
            earning_rate = _levelRef.earning_rate + _levelRef.earning_rate_per_level * (_level - _levelRef.level);
            tap_to_collect_num = _levelRef.tap_to_collect_num + _levelRef.tap_to_collect_num_per_level * (_level - _levelRef.level);
            auto_tap_num_per_sec = _levelRef.auto_tap_num_per_sec + _levelRef.auto_tap_num_per_sec_per_level * (_level - _levelRef.level);
            res_index = _levelRef.res_index;
            preview_tex_index = _levelRef.preview_tex_index;

            bonus_modify = new PlayerBonusPropertyModifier();
            bonus_modify.setProperty(EBonusPropertyType.BUILDING_PROFIT_ADD_PER, earning_rate);
            
            if (_baseRef == null)
            {
                upgrade_cost_item = null;
                return;
            }
            
            upgrade_cost_item = new NPCommonCostItem(_baseRef.upgrade_cost_item, upgrade_cost_num);
        }
    }
}