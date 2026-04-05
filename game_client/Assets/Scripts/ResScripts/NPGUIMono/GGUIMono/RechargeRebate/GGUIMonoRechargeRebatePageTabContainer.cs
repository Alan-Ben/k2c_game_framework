using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 充值返利页签列表item容器
    /// </summary>
    public class GGUIMonoRechargeRebatePageTabContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoRechargeRebatePageTabContainerItem>
    {
        [ALHeader("列表为空时显示的物体列表")]
        public List<GameObject> goEmptyShowList;
        [ALHeader("列表为空时隐藏的物体列表")]
        public List<GameObject> goEmptyHideList;
        [ALHeader("Container侧边红点提示信息")]
        public ContainerSideRedTipInfo sideRedTipInfo;
    }
}
