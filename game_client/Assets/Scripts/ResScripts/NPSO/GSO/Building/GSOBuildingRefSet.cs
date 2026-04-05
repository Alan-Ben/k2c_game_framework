
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    [Serializable]
    public class BuildingRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;
        public int build_order;
        public string name;
        public NPCommonCostItem build_cost;
        public NPGTextureIndex preview_tex_index;
        public string building_desc; // 建筑本身的描述
        public string build_desc; // 建筑建造成功的描述
        public string level_up_desc; // 建筑升级的描述
        public NPGGoIndex res_index; // todo: 如果要启用客户端复杂的功能，这条字段删掉，改为序列化下面那条不导出的字段
        public NPGGoIndex unbuilt_res_index;
        public string unbuilt_click_tip;
        public _NPPlayerConditionSerializeInfo build_condition;
        public string build_condition_desc;
        public List<string> build_condition_desc_params;
        public List<NPPlayerEffectSerializeInfo> lock_jump_btn_effect;
        public long build_sfx_id; // 建筑建造的特效 id
        public float build_sfx_delay; // 特效播放多久后加载建造完成后的资源
        public long guide_hand_ui_res_id; // 引导手指资源 id
        public long build_success_dialogue_id;// 建筑建造成功对话 id

        // todo: 客户端结构做复杂了，下面的配置由客户端启动时自己修复
        [NonSerialized][ALAutoExportVariableAttr(true, true, true)]
        public NPGGoIndex built_res_index;
    }
    public class GSOBuildingRefSet : _TALSOBasicRefSet<BuildingRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "building"; } }
    }
}