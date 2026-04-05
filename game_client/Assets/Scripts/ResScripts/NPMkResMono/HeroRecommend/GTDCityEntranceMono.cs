using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主城场景相关入口的mono
    /// </summary>
    public class GTDCityEntranceMono : MonoBehaviour
    {
        [ALHeader("骑士推荐入口列表")]
        public List<GTDHeroRecommendPointMono> heroRecommendEntranceList;
    }
}