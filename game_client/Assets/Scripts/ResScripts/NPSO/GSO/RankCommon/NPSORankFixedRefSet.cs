using ALPackage;

namespace GOE
{
    [System.Serializable]
    public class NPRankFixedRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id;//实例id
        public long rank_id; //排行榜id
        public long like_fixed_cd_id;//点赞消耗fixed_cd id
    }

    /// <summary>
    /// 常驻排行榜表
    /// </summary>
    public class NPSORankFixedRefSet : _TALSOBasicRefSet<NPRankFixedRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/rank_refdata.unity3d"; } }
        public static string objName { get { return "rank_fixed"; } }
    }
}

