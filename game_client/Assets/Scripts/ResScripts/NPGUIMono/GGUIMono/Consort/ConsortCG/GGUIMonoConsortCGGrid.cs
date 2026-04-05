using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoConsortCGGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoConsortCGGridItem>
    {
        [ALHeader("没有CG时的提示")]
        public List<GameObject> noItemShow;
    }
}