
using System.Collections.Generic;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动消耗商店仓库container
    /// </summary>
    public class GGUIMonoRegularEventWarehouseContainer : _AHotfixShowAnimContainerBaseMono
    {
        [HotfixMonoAttribute("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}