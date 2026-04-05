using ALPackage;

namespace Hotfix
{
    /// <summary>
    /// 热更General表
    /// </summary>
    public class HotfixGeneralRefObj : _AHotfixGeneralRefCore
    {
        public long test_long;
        
        public HotfixGeneralRefObj(_AALResourceCore _resCore) 
            : base(_resCore, assetPath, objName)
        {
        }

        protected override void _parseFromString()
        {
            test_long = getLong("test_long");
        }
        
        public static string assetPath
        {
            get { return "refdata/hotfix_refdata.unity3d"; }
        }

        public static string objName
        {
            get { return "general_hotfix"; }
        }
    }
}