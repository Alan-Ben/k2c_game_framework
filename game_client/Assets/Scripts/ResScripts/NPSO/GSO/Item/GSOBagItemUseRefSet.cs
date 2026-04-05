using ALPackage;
using NPEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [System.Serializable]
    public class BagItemUseRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; //物品ID

        public List<EImproveTargetType> improve_target_type_list; // 使用道具可提升的目标类型列表
        public bool can_batch_use;  // 能否批量使用
        public bool is_auto_use; // 该物品是否自动使用
        public List<NPCommonCostItem> cost_item_list; // 使用消耗
        public string cost_not_fix_tip; // 消耗不足的提示
        public List<string> cost_not_fix_tip_args; // 消耗不足的提示参数


        public List<NPCommonCostItem> option_item_list; // 可选奖励列表
        public int option_count; // 可选数量，可从奖励列表中选择的数量

        public int get_reward_id;//奖励列表

        public bool is_pre_high_pri;//是否前置执行效果具有更高优先级，如无则两者一定都会执行，true则优先判断前置是否执行
        public _NPPlayerConditionSerializeInfo pre_use_cond; // 前置使用效果的使用条件
        public string pre_effects; // 前置使用效果

        public _NPPlayerEffectSerializeInfo c_effects_in_bag;//客户端在背包中使用的效果（仅在背包界面有效）
        public _NPPlayerConditionSerializeInfo use_cond; // 使用条件
        public _NPPlayerEffectSerializeInfo c_effects;//客户端使用效果
        public string effects; // 使用效果
        public string cond_desc; //使用条件说明
        public List<string> cond_desc_args; //使用条件说明参数

        public string cond_not_fix_tip; // 使用条件不足时的提示
        public List<string> cond_not_fix_tip_args; // 使用条件不足时的提示

        public ENPBagItemUseType use_type; //物品使用类型
        public bool isNx { get { return option_item_list != null && option_item_list.Count > 1; } }

        public NPGTextureIndex real_gain_Item_Img_Idx; //实际获得的物品图标 - 客户端用

        public _NPPlayerVariableSerializeInfo real_gain_item_count;//计数器的高级公式(ENPPlayerVariableType)
        public ENpRewardShowType tip_reward;//是否使用tip样式展示奖励
        public ENpRewardShowType tip_reward_multiple;//多个使用表现tip样式展示奖励 即大于等于general表bag_auto_use_max_count的使用次数时
    }

    /**************
     * 物品表
     **/
    public class GSOBagItemUseRefSet : _TALSOBasicRefSet<BagItemUseRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/bag_refdata.unity3d"; } }
        public static string objName { get { return "bag_item_use"; } }
    }
}

