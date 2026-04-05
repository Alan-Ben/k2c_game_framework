using UnityEngine;

namespace GOE
{
    public class GTDMonoMarsBuildingEnergyNormal : GTDMonoMarsBuildingNormal
    {
        [ALHeader("收集的上浮提示")]
        public long collectTipId = 3;
        [ALHeader("收集的粒子效果")]
        public long collectParticleId;
    }
}