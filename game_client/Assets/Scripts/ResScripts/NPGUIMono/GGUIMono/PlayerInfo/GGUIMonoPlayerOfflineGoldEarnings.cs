
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class GGUIMonoPlayerOfflineGoldEarningsSliderShowInfo
    {
        [ALHeader("进度范围起始值")]
        public float rangeStart;
        [ALHeader("进度范围结束值")]
        public float rangeEnd;
        [ALHeader("该范围内显示的go列表")]
        public List<GameObject> goListShow;
    }
    
    public class GGUIMonoPlayerOfflineGoldEarnings : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("离线时长")]
        public Text offlineTimeTxt;
        [ALHeader("离线奖励数量")]
        public Text offlineCountTxt;
        [ALHeader("离线时长上限")]
        public Text offlineTimeMaxTxt;
        [ALHeader("粒子的起点和资源 id ")]
        public RectTransform particleStart;
        public long specialParticleId;
        [ALHeader("时常进度条")]
        public Slider sldTime;
        [ALHeader("不同进度展示的go列表")]
        public List<GGUIMonoPlayerOfflineGoldEarningsSliderShowInfo> sldShowInfoList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.C_OFFLINE_GOLD_EARNINGS); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.C_OFFLINE_GOLD_EARNINGS); } }
    }
}