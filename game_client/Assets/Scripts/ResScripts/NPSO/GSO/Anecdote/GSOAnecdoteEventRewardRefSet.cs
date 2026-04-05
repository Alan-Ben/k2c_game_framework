
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    [Serializable]
    public class AnecdoteEventRewardRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; // 主键 id
        public List<NPCommonCostItem> reward_item;
    }
    public class GSOAnecdoteEventRewardRefSet : _TALSOBasicRefSet<AnecdoteEventRewardRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/anecdote_refdata.unity3d"; } }
        public static string objName { get { return "anecdote_event_reward"; } }
    }
}