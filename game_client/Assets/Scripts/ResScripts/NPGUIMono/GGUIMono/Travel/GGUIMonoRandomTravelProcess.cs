using System;
using System.Collections.Generic;
using ALPackage;
using TMPro;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class RandomTravelVideoConfig
    {
        [ALHeader("加载的视频AssetPath")]
        public NPCommonAssetPathInfo videoAssetPathInfo;
        
        [ALHeader("视频播放时间")]
        public float videoPlayTime;
    }
    
    /// <summary>
    /// 随机游历表现流程窗口
    /// </summary>
    public class GGUIMonoRandomTravelProcess : _AALBasicUIWndMono
    {
        // [ALHeader("变化游历背景延迟时间(从开始表现计时, 单位秒)")]
        // public float chgBgDelayTimeS;
     
        [ALHeader("游历背景图片")]
        public RawImage bgImg;
        
        [ALHeader("在视频出现前的动画名(在这个动画完后才会对视频赋值)")]
        public string beforeVideoShowAniName;

        [ALHeader("播放视频子窗口")] 
        public GGUIMonoCommonVideo showVideoMono;
        
        [ALHeader("视频配置列表")]
        public List<RandomTravelVideoConfig> videoConfigList;

        [ALHeader("视频播放完后显示游历地点信息动画")]
        public string afterVideoShowPosInfoAniName;

        [ALHeader("游历地点Banner图片")]
        public RawImage travelPosBannerImg;
        [ALHeader("游历地点名称列表")]
        public List<TMP_Text> txtTravelPosNameList;
        [ALHeader("游历地点名称Key")]
        public string txtTravelPosNameKey;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3618); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3618); } }
    }
}