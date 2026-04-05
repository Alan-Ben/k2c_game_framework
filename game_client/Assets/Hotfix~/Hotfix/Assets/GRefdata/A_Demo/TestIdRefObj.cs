namespace Hotfix
{
    /// <summary>
    /// 测试验证用配表，后期删
    /// </summary>
    public class TestIdRefObj : _AHotfixBaseRefObj
    {
        public override long _refId { get { return id; } }

        public long id;
        public string desc;
        
        protected override void _parseFromString(string _line)
        {
            id = getLong("id");
            desc = getString("desc");
        }
        
        public static string assetPath { get { return "refdata/hotfix_refdata.unity3d"; } }
        public static string objName { get { return "test_id"; } }
    }
}