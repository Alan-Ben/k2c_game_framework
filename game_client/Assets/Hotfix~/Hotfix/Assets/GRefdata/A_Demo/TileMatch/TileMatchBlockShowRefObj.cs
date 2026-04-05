namespace Hotfix
{
    public class TileMatchBlockShowRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return id; } }

        public long id;//唯一id
        public long block_id;//棋子id
        public TileMatchEnum.ETileMatch_ModeType mode_type;//模式类型
        public NPGSpriteIndex icon;//图标
        public NPCommonAssetPathInfo prefab_asset_path;//预制加载路径

        protected override void _parseFromString(string _line)
        {
            id = getLong("id");
            block_id = getLong("block_id");
            mode_type = getEnum<TileMatchEnum.ETileMatch_ModeType>("mode_type");
            icon = NPGSpriteIndex.readIndexInfo(getString("icon"));
            prefab_asset_path = NPCommonAssetPathInfo.readFromStr(getString("prefab_asset_path"));
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "tilematch_block_show"; } }
    }
}