using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴套系技能列表item容器
    /// </summary>
    public class GGUIMonoHeroSuitSkillContainer : _ATNPGGUIMonoShowAnimContainer<GGUIMonoHeroSuitSkillContainerItem>
    {
        [ALHeader("列表为空时需要显示的GO列表")]
        public List<GameObject> goEmptyShowList;
    }
}
