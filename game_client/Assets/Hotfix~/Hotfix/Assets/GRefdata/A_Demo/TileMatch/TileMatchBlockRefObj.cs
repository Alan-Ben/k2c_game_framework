namespace Hotfix
{
    /// <summary>
    /// 三消格子配表
    /// </summary>
    public class TileMatchBlockRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return id; } }
        
        public long id;
        public TileMatchEnum.ETileMatch_BlockType type;//格子类型
        
        protected override void _parseFromString(string _line)
        {
            id = getLong("id");
            type = getEnum<TileMatchEnum.ETileMatch_BlockType>("type");
        }

        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "tilematch_block"; } }
    }
}