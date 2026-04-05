using System;
using ALPackage;
using Common.GuildEnum;

namespace GOE
{
    /// <summary>
    /// 加盟限制表
    /// </summary>
    [Serializable]
    public class GuildJoinLimitRefObj : _IALBasicRefObj
    {
        public long _refId { get; }

        public long id;
        public EGuildJoinLimitType type;//类型
        public string desc;//描述
        public string on_try_join_not_conform_limit_tip;//当尝试加入一个自身不满足该限制的联盟时的提示
        public long no_limit_value;//不做限制的值(服务端给这个值时，代表没有这个限制条件)
    }
    
    public class GSOGuildJoinLimitRefSet : _TALSOBasicRefSet<GuildJoinLimitRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/guild_refdata.unity3d"; } }
        public static string objName { get { return "guild_join_limit"; } }
    }
}