using System;

namespace GOE
{
    public class HotfixDemoExportMenuItem : _AHotfixExportMenuItem
    {
        private string _m_sAssetPath = NPABString.C_HotfixRefdataPath;
        
        public HotfixDemoExportMenuItem(Func<string,string, bool> _judgeCanShowFunc) :
            base( ENPExportSettingEnum.TEST_HOTFIX, _judgeCanShowFunc)
        {
            
        }

        protected override void realExportGeneralRefSet()
        {
            doHotfixExportRefSet("/Resources/Refdata/Excel~/activity_tilematch-消消乐.xlsx", "tilematch_other", _m_sAssetPath);
        }
    }
}