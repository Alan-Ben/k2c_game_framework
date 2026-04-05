using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GTDMonoMarsScene : MonoBehaviour
    {
        [ALHeader("对象都在这个节点下")]
        public Transform unitParent;
        [ALHeader("建筑位置列表")]
        public List<GTDMonoBuildingPos> buildingPosList;

        [ALHeader("居民补充脚本")]
        public GTDMonoMarsResidentReplenish monoMarsResidentReplenish;
        
        [ALHeader("民意中心脚本")]
        public GTDMonoMarsPopularWillCenter monoMarsPopularWillCenter;
        
        [ALHeader("建筑升级建造完成的特效 id ")]
        public long upgradeSfxId;
        [ALHeader("火星实力出现的延迟时间")]
        public float marsPowerDelay = 0.5f;
        [ALHeader("侧边提示出现的延迟时间")]
        public float sideTipDelay = 0.5f;
        public long sideTipId = 1;
    }
}