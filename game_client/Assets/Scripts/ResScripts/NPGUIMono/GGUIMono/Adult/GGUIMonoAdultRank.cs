
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultRank : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("子嗣排名列表")]
        public GGUIMonoAdultRankGrid monoAdultGrid;
        [ALHeader("我的排名")]
        public Text txtMyRank;
        [ALHeader("我的子嗣收益")]
        public Text txtMyEarnings;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2502); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2502); } }
    }
}