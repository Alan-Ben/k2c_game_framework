using System;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;
using UnityEngine.Playables;

namespace GOE
{
    [System.Serializable]
    public class GAvatarGachaQualityInfo
    {
        [ALHeader("品质")]
        public EQuality quality;
        [ALHeader("品质timeline")]
        public PlayableDirector playableDirector;
    }
    
    /// <summary>
    /// 抽卡表现场景mono
    /// </summary>
    public class GTDAvatarGachaDrawSceneMono : MonoBehaviour
    {
        [ALHeader("单抽不同品质对应的配置")]
        public List<GAvatarGachaQualityInfo> qualityInfoList_Single;
        [ALHeader("10连抽不同品质对应的配置")]
        public List<GAvatarGachaQualityInfo> qualityInfoList_Ten;
        
#if NP_GAME

        public event Action onTimeLinePlayEnd;
        
        public void onTimeLineEnd()
        {
            onTimeLinePlayEnd?.Invoke();
        }
        
#endif
    }
}