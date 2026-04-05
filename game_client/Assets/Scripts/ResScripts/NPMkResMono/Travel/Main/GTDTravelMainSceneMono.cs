using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    
    public enum ETravelStatType
    {
        NORMAL,
        RAND_TRAVEL,
        ALL_TRAVEL,
    }
    
    /// <summary>
    /// 游历主场景mono
    /// </summary>
    public class GTDTravelMainSceneMono : MonoBehaviour
    {
        [ALHeader("场景animation")]
        public Animation contentAnimation;
        
        [ALHeader("地图加载点集合")]
        public List<GTDTravelMainPosItemInfo> travelPosList;

        [ALHeader("玩家卧室所处位置")]
        public Transform roomTrans;
        
        [ALHeader("聚焦到地点的时长")]
        public float focusToPosDuration = 0.5f;

        [ALHeader("飞机挂载父节点")]
        public Transform aircraftParent;
        [ALHeader("飞机goindex")]
        public NPGGoIndex aircraftGoIndex;
    }
}