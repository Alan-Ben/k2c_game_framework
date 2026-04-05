using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

namespace GOE
{
    public class GGUIMonoIntroStoryNew : _AALBasicUIWndMono
    {
        [ALHeader("跳过按钮")]
        public GameObject btnSkip;
    
        // [ALHeader("双击跳过按钮")]
        // public GameObject doubleClickSkip;
        // [ALHeader("双击跳过需要的点击间隔时间ms(两次点击间隔小于这个时间(毫秒)才会触发双击跳过)")]
        // public long doubleClickSkipIntervalTimeMs;
        // [ALHeader("双击跳过提示")]
        // public string doubleClickSkipTip;

        [ALHeader("视频播放时间(秒)")]
        public float videoPlayTimeS;
        
        [ALHeader("视频播放器")]
        public VideoAniRawImageMono videoAniMono;
    
        [ALHeader("时间线")]
        public PlayableDirector playableDirector;

        [ALHeader("需要预热的音效id列表")]
        public List<long> warmUpAudioRefId;
    
        [ALHeader("不同语言对应不同的TimeLine, 可能用于配置不同语言对应的同一句话的长度不同的情况")]
        public List<GGUIMonoLanguagePlayableAsset> LanguagePlayableAssetList;

        public PlayableAsset getAssetByLanguage(ENPLanguage _language)
        {
            if(LanguagePlayableAssetList != null)
            {
                for (int i = 0; i < LanguagePlayableAssetList.Count; i++)
                {
                    GGUIMonoLanguagePlayableAsset languagePlayableAsset = LanguagePlayableAssetList[i];
                    if(languagePlayableAsset != null && languagePlayableAsset.language == _language)
                        return languagePlayableAsset.playableAsset;
                }
            }
            return null;
        }
    }
}