using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 使用物品-情人列表
    /// </summary>
    public class GGUIMonoBagItemUseConsortGrid : _TALUGUIMonoGridWnd<GGUIMonoBagItemUseConsortGridItem>
    {
        [ALHeader("列表为空显示的GoList")]
        public List<GameObject> zeroShowGoList;
    }
}
