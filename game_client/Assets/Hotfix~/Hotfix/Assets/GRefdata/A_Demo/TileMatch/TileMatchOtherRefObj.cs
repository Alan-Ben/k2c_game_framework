using ALPackage;
using UnityEngine;

namespace Hotfix
{
    public class TileMatchOtherRefObj : _AHotfixGeneralRefCore
    {
        public long tilematch_lazy_cd_id;//三消体力道具id
        public Vector2Int tilematch_map_size;//棋盘大小(行:列)
        public NPCommonItem tilematch_currency_item;//【三消】三消兑换币道具
        public int tilematch_lazy_cd_red_tip_show_per;//【三消】三消体力红点显示百分比
        public long tilematch_activity_id;//【三消】三消活动id
        
        public TileMatchOtherRefObj(_AALResourceCore _resCore) : base(_resCore, assetPath, objName)
        {
        }

        protected override void _parseFromString()
        {
            tilematch_lazy_cd_id = getLong("tilematch_lazy_cd_id");
            tilematch_map_size = getVector2Int("tilematch_map_size");
            tilematch_currency_item = NPCommonItem.readFromStr(getString("tilematch_currency_item"));
            tilematch_lazy_cd_red_tip_show_per = getInt("tilematch_lazy_cd_red_tip_show_per");
            tilematch_activity_id = getLong("tilematch_activity_id");
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "tilematch_other"; } }
    }
}