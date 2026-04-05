
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    [Serializable]
    public class AnecdoteEventEarningsRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; // 主键 id
        public long earnings; // 目标赚速
        public List<NPCommonCostItem> reward_item; // 首次点开的奖励
        public string event_desc; // 事件描述
        public List<string> event_desc_params; // 事件描述参数
        public List<NPCommonCostItem> result_reward_item; // 完成目标后的奖励
    }
    public class GSOAnecdoteEventEarningsRefSet : _TALSOBasicRefSet<AnecdoteEventEarningsRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/anecdote_refdata.unity3d"; } }
        public static string objName { get { return "anecdote_event_earnings"; } }
    }
}