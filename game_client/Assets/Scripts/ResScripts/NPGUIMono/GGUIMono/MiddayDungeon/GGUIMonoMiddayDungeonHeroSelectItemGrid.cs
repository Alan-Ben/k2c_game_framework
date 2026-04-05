using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIMonoMiddayDungeonHeroSelectItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoMiddayDungeonHeroSelectItem>
    {
        [ALHeader("列表为空时显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}
