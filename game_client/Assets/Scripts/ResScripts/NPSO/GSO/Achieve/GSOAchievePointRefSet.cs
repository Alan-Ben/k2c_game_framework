using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 成就点数数据
    /// </summary>
    [System.Serializable]
    public class AchievePointRefObj:_IALBasicRefObj
    {
        public long _refId { get { return (long)type; } }
        public EAchieveType type;
    }

    public class GSOAchievePointRefSet : _TALSOBasicRefSet<AchievePointRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/achieve_refdata.unity3d"; } }
        public static string objName { get { return "achieve_point"; } }
    }
}

