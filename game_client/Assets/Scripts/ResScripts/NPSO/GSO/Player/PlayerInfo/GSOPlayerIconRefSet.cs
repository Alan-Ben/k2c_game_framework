using ALPackage;

namespace GOE
{
    /// <summary>
    /// 头像表结构
    /// </summary>
    /// 
    [System.Serializable]
    public class PlayerIconRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;

        public _NPPlayerConditionSerializeInfo show_condition;//展示条件
        public EPlayerInfoIconType show_type;//页签类型
        public long sort_id;//排序id
        public long asset_path_id; //加载路径
        public bool disable_cannot_see;
        public bool gain_is_show; //活动时候是否需要表现
    }

    public class GSOPlayerIconRefSet : _TALSOBasicRefSet<PlayerIconRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
        public static string objName { get { return "player_icon"; } }
    }
}

