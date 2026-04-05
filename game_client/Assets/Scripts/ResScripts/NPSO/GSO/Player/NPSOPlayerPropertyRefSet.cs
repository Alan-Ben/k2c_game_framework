using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家属性表结构
    /// </summary>
    /// 
    [System.Serializable]
    public class NPPlayerPropertyRefObj : _IALBasicRefObj
    {
        public long _refId { get { return (long)player_property_type; } }

        public ENPPlayerPropertyType player_property_type;
        public string name;//名称
        public NPGTextureIndex icon;//图标
        public bool is_show;//是否显示
        public int sort_id;// //排序id(小→大)
    }

    public class NPSOPlayerPropertyRefSet : _TALSOBasicRefSet<NPPlayerPropertyRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
        public static string objName { get { return "player_property"; } }
    }
}

