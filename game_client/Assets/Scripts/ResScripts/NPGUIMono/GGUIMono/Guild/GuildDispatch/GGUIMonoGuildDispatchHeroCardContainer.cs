using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟派遣大臣卡片itemContainer
    /// </summary>
    public class GGUIMonoGuildDispatchHeroCardContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoGuildDispatchHeroCard>
    {
        [ALHeader("没有item时显示的物体列表")]
        public List<GameObject> noItemShow;
    }
}