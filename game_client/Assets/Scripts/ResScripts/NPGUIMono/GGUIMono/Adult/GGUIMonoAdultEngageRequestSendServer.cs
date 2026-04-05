
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestSendServer : _AALBasicUIWndMono
    {
        [ALHeader("刷新按钮")]
        public GameObject btnRefresh;
        [ALHeader("推荐列表")]
        public GGUIMonoAdultEngageRequestSendServerContainer monoRecommendContainer;
        [ALHeader("我的子嗣信息")]
        public GGUIMonoChildInfo monoAdultInfo;
        [ALHeader("发送全服联谊")]
        public GameObject btnRandom;
        [ALHeader("联谊限制勾选Toggle")]
        public NPGGUIMonoCommonToggleEx toggleEngageLimit;
        [ALHeader("加载中显示和隐藏的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        
        
        public void setLoadingShow(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isShow ? listLoadingShow : listLoadingHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2509); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2509); } }
    }
}