using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 现金礼包页签列表item容器
    /// </summary>
    public class GGUIMonoCashGiftPackMainPageTabContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoCashGiftPackMainPageTabContainerItem>
    {
        [ALHeader("列表为空时显示的物体列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("Container侧边红点提示信息")]
        public ContainerSideRedTipInfo sideRedTipInfo;
    }
}
