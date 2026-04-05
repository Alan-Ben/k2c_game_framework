
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    [Serializable]
    public class AnecdoteEventChoiceOptionRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; // 主键 id
        public string option_name; // 选项名称
        public NPGTextureIndex option_tex; // 选项贴图
        public List<NPCommonCostItem> reward_item_list; // 选项奖励
        public string option_result_desc; // 选项结果描述
        public bool is_right_choice; // 是否是正确的选项
    }
    public class GSOAnecdoteEventChoiceOptionRefSet : _TALSOBasicRefSet<AnecdoteEventChoiceOptionRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/anecdote_refdata.unity3d"; } }
        public static string objName { get { return "anecdote_event_choice_option"; } }
    }
}