using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 分享家人CG的grid
    /// </summary>
    public class GGUIMonoShareConsortCGGrid : _TALUGUIMonoGridWnd<GGUIMonoShareConsortCGGridItem>
    {
        [ALHeader("列表为空时的提示")]
        public List<GameObject> noneItemsTips;
    }
}
