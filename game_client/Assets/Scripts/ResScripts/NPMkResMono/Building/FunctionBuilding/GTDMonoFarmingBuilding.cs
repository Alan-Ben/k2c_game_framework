using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GTDMonoFarmingBuilding : MonoBehaviour
    {
        [ALHeader("名字的跟随目标")]
        public Transform nameHudTarget;
        [ALHeader("收益的跟随目标")]
        public Transform earningsHudTarget;
        [ALHeader("暴击倍数的跟随目标")]
        public Transform multipleHudTarget;
        [ALHeader("点击收益的跟随目标")]
        public Transform clickEarningsHudTarget;
        [ALHeader("收获按钮")]
        public GTDCommonPosClickMono earningClickMono;
        [ALHeader("升级按钮，以及可以展示时显示的内容")]
        public GTDCommonPosClickMono levelUpClickMono;
        public List<GameObject> listUpgradableShow;

        [ALHeader("收获特效的挂点和 id ")]
        public Transform collectEffectTarget;
        public long collectSfxId;
        [ALHeader("倍数暴击特效挂点和id")]
        public Transform multipleEffectTarget;
        public long multipleSfxId;
        [ALHeader("粒子的起始点和特殊的收获粒子 id ")]
        public Transform particleTarget;
        public long specialCollectParticleId = 0;
        [ALHeader("收获动画和动画名")]
        public Animation collectAnimation;
        public string collectAnimationName;

        [ALHeader("有建筑正在建造时需要显示的GO列表")]
        public List<GameObject> goBuildBuildingShowList;
        [ALHeader("有建筑正在建造时需要隐藏的GO列表")]
        public List<GameObject> goBuildBuildingHideList;
    }
}