using ALPackage;

namespace GOE
{
    [System.Serializable]
    public class NPPlayerBubbleRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;

        //气泡框图片- image
        public NPGSpriteIndex spt_icon;

        public _NPPlayerConditionSerializeInfo show_condition;//展示条件
        public bool disable_cannot_see;//无效时是否可以看到此数据
        public string asset_path;
        public string obj_name_r;
        public string obj_name_l;

        public long asset_path_id;//玩家装扮资源路径id
    }

    public class NPSOPlayerBubbleRefSet : _TALSOBasicRefSet<NPPlayerBubbleRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/player_refdata.unity3d"; } }
        public static string objName { get { return "player_bubble"; } }
    }
}

