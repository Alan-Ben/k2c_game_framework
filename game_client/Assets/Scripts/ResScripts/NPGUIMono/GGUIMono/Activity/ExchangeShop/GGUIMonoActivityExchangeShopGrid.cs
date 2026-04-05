
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用活动兑换商店界面列表
    /// </summary>
    public class GGUIMonoActivityExchangeShopGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoActivityExchangeShopGridItem>
    {
        [ALHeader("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}
