using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestSendGuild : _AALBasicUIWndMono
    {
        [ALHeader("推荐列表")]
        public GGUIMonoAdultEngageRequestSendGuildContainer monoRecommendContainer;
        [ALHeader("我的子嗣信息")]
        public GGUIMonoChildInfo monoAdultInfo;
        [ALHeader("发送公会联谊")]
        public GameObject btnRandom;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1208); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1208); } }
    }
}