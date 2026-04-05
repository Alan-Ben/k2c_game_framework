using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 图鉴页签
    /// </summary>
    public class GGUIMonoTreasureHuntCatalogTab : _AALBasicUIWndMono
    {
        [ALHeader("tab")]
        public NPGGUIMonoCommonTab monoTab;
        
        [ALHeader("页签名列表")]
        public List<TextEx> txtNameList;
    }
}