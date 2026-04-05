using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子解锁详情页面加护page
    /// </summary>
    public class GGUIMonoUnLockConsortDetailBlessPage : _AGGUIMonoUnLockConsortDetailTabPage
    {
        [ALHeader("关联大臣列表")]
        public GGUIMonoConsortBlessHeroSimpleIconContainer monoRelationHeroContainer;

        [ALHeader("加护技能升级特效")]
        public long onBlessSkillLevelUpShowSfxId;
        
        [ALHeader("妃子加护技能列表")]
        public GGUIMonoConsortBlessSkillContainer monoConsortBlessSkillContainer;
    }
}