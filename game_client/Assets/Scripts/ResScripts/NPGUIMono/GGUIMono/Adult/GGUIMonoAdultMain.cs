
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum GGUIMonoAdultMainTabType
    {
        Unmarried,
        Married,
    }
    public class GGUIMonoAdultMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnBack;
        [ALHeader("毕业总人数")]
        public Text txtAdultTotalNum;
        [ALHeader("毕业生总村庄收益")]
        public Text txtAdultTotalEarnings;
        [ALHeader("已婚未婚的切换按钮")]
        public GGUIMonoAdultMainPageTabList pageTabList;
        [ALHeader("排行榜按钮")]
        public GameObject btnRank;
        [ALHeader("联谊请求按钮")]
        public GameObject btnEngageRequest;
        
                
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2501); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2501); } }
    }
}