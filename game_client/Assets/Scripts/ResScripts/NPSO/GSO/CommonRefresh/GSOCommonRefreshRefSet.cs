
using ALPackage;
using System;

namespace GOE
{
    [Serializable]
    public class CommonRefreshRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; // 主键 id
    }
    public class GSOCommonRefreshRefSet : _TALSOBasicRefSet<CommonRefreshRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "common_refresh"; } }
    }
}