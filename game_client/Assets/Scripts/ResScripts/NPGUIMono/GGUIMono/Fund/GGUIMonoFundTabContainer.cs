using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoFundTabContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoFundTabContainerItem>
    {
        [ALHeader("列表为空时显示的物体列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("Container侧边红点提示信息")]
        public ContainerSideRedTipInfo sideRedTipInfo;
    }
}