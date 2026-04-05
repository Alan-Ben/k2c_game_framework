using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 周卡结算信息
    /// </summary>
    public class GGUIMonoWeekCardSettle : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("执政官形象")]
        public GGUIMonoCommonShowCase assignShowcase;
        [ALHeader("奖励item列表")]
        public NPGGUIMonoCommonItemContainer itemContainer;
        [ALHeader("信息排序")]
        public List<EWeekCardSettleType> settleTypeSort;
        [ALHeader("信息文本")]
        public Text txtSettleInfo;
        [ALHeader("离线时长")]
        public Text txtTime;
        [ALHeader("周卡过期显示，没过期隐藏")]
        public List<GameObject> weekCardExpiredShow;
        [ALHeader("没有处理信息的时候显示，否则隐藏")]
        public List<GameObject> noSettleInfoShow;
        [ALHeader("没有处理信息的时候隐藏，否则显示")]
        public List<GameObject> noSettleInfoHide;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4501); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4501); } }
    }
}