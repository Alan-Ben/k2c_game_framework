using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using NPEnum;

namespace GOE
{
    [System.Serializable]
    public class PlayerLvlRefObj : _IALBasicRefObj
    {
        public long _refId { get { return lvl; } }

        public int lvl;
        public string name;//玩家等级名称
        public List<string> name_args;//玩家等级名称参数
        public long exp;//升到当前等级需要的经验
        public long earnings;//升级到当前等级所需要的赚速（不扣除）
        public NPGTextureIndex icon;//等级图片
        // public NPGTextureIndex card_image;//主角半身像
        // public NPGGoIndex td_res_index;//玩家的 3d 形象
        public bool ignore_pop_wnd;//当前等级是否忽略升级弹窗展示
        public List<NPCommonCostItem> daily_reward_item;//每日奖励
        public List<NPCommonCostItem> reward_item_list;//升到当前等级奖励列表
        public List<NPCommonCostItem> show_reward_item_list;//客户端展示的奖励列表
        public bool need_show_unchange;//玩家升级是否需要展示未变化的属性
        // public List<string> key_list;//玩家等级展示属性
        // public List<string> value_list; //属性参数
        // public List<string> add_key_list;//额外增加属性
        // public List<string> add_value_list; //额外属性参数
        public List<string> special_key_list; //额外特殊条目参数
        public List<string> special_value_list; //额外特殊条目属性
        public long child_educate_cost;//子嗣上课的金币消耗
        public long child_educate_get_hero_exp;//子嗣上课获得的英雄经验


        public NPPlayerPropertyModifier player_property;//玩家该等级的属性
        public long child_educate_get_base_earnings;//子嗣毕业的基础收益
        public long child_graduate_get_earnings_add;//子嗣毕业的评级加成
        public long trigger_push_gift_group_id;//触发推送礼包组id

        //玩家等级名称
        public string nameStr { get { return TextTranslate.instance.getLanguage(name, name_args); } }

    }

    public class NPSOPlayerLvlRefSet : _TALSOBasicRefSet<PlayerLvlRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
        public static string objName { get { return "player_lvl"; } }
    }
}

