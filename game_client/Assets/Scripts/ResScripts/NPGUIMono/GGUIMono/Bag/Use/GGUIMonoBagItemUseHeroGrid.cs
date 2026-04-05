using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public enum EGGUIMonoBagItemUseHeroGridTargetHeroType
    {
        NONE,
        INDEX,//下标
    }
    
    /// <summary>
    /// 使用物品-伙伴列表
    /// </summary>
    public class GGUIMonoBagItemUseHeroGrid : _TALUGUIMonoGridWnd<GGUIMonoBagItemUseHeroGridItem>
    {
        [ALHeader("列表为空显示的GoList")]
        public List<GameObject> zeroShowGoList;
    }
}
