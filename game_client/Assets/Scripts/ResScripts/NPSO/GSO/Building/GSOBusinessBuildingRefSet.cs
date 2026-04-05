
using System;
using System.Collections.Generic;
using ALPackage;
using Common.ConditionEnum;
using CommonEnum;
using GOE.Condition;

namespace GOE
{
    [Serializable]
    public class BusinessBuildingRefObj : _IALBasicRefObj, _ITNPConditionDealerData<EBuildingConditionType>
    {
        public long _refId { get { return building_id; } }

        public long building_id;
        public long employee_earnings;
        public long employee_base_max_count;
        public ESpecAttrType attr_type;
        public List<long> hero_slot_employee_num_list;
        public long addition_employee_count_per_level;
        public int hire_cost_multiple;
        public long video_group_id;
        public long open_building_audio_id;
        
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public BusinessBuildingVideoGroupRefObj video_group_ref;
        // todo: 客户端结构做复杂了，下面的配置由客户端启动时自己修复
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public string name;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public string desc;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public string level_up_desc;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGGoIndex res_index;
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGTextureIndex preview_tex_index;
    }
    public class GSOBusinessBuildingRefSet : _TALSOBasicRefSet<BusinessBuildingRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "business_building"; } }
    }
}