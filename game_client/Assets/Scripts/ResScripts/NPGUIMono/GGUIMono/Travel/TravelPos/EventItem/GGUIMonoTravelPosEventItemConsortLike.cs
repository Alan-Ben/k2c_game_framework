using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 游历地点事件item - 妃子好感度类型Mono
    /// </summary>
    public class GGUIMonoTravelPosEventItemConsortLike : _AGGUIMonoTravelPosEventItem
    {
        [ALHeader("妃子详情按钮")]
        public GameObject btnConsortInfo;
        
        [ALHeader("妃子好感度进度条")]
        public NPGGUIMonoProgress monoLikeProgress;
    }
}
