using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 性别表结构
    /// </summary>
    /// 
    [System.Serializable]
    public class NPPlayerGenderRefObj : _IALBasicRefObj
    {
        public long _refId { get { return (long)gender_type; } }

        public ENPGenderType gender_type;

        public NPGTextureIndex icon;//图标
    }

    public class NPSOPlayerGenderRefSet : _TALSOBasicRefSet<NPPlayerGenderRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
        public static string objName { get { return "player_gender"; } }
    }
}

