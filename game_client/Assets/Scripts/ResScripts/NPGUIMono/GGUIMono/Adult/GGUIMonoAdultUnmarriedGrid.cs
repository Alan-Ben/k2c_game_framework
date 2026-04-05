using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultUnmarriedGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoAdultUnmarriedGridItem>
    {
        [ALHeader("列表为空时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}