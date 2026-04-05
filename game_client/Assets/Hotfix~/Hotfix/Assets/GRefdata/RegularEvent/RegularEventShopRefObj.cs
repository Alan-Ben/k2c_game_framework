namespace Hotfix
{
    /// <summary>
    /// 万能活动商店表
    /// </summary>
    public class RegularEventShopRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return activity_id; } }

        public long activity_id;
        
        protected override void _parseFromString(string _line)
        {
            activity_id = getLong("activity_id");
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "regular_event_shop"; } }
    }
}