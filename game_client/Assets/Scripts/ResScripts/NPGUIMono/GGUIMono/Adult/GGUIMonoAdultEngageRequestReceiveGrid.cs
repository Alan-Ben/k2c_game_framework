using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestReceiveGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoAdultEngageRequestReceiveGridItem>
    {
        [ALHeader("列表为空时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}