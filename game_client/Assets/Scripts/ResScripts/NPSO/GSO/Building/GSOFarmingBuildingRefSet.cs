
using System;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class FarmingBuildingRefObj : _IALBasicRefObj
    {
        public long _refId { get { return building_id; } }

        public long building_id;
        public NPCommonItem upgrade_cost_item;
        public long building_level_max;
        public long upgrade_btn_guide_hand_res_id;
        
        // todo: 客户端结构做复杂了，下面的配置由客户端启动时自己修复
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public string name;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public string level_up_desc;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGGoIndex res_index;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGTextureIndex preview_tex_index;
    }
    public class GSOFarmingBuildingRefSet : _TALSOBasicRefSet<FarmingBuildingRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "farming_building"; } }
    }
}