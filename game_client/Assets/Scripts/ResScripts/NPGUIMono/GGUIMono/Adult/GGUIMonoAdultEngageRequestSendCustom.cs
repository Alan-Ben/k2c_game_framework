using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestSendCustom : _AALBasicUIWndMono
    {
        [ALHeader("推荐列表")]
        public GGUIMonoAdultEngageRequestSendCustomContainer monoRecommendContainer;
        [ALHeader("玩家 id 输入框和搜索按钮")]
        public InputField iptPlayerId;
        public GameObject btnSearch;
        [ALHeader("搜索结果的玩家信息和联姻按钮")]
        public NPGGUIMonoPlayerIcon monoSearchResult;
        public GameObject btnEngageSearchResult;
        [ALHeader("有搜索结果时展示的内容")]
        public List<GameObject> listSearchResultShow;
        [ALHeader("加载中显示和隐藏的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        [ALHeader("切换推荐和好友按钮")]
        public NPGGUIMonoCommonToggleEx monoRecommendToggle;
        public NPGGUIMonoCommonToggleEx monoFriendToggle;
        
        
        public void setLoadingShow(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isShow ? listLoadingShow : listLoadingHide, true);
        }
        public void setSearchingResultState(bool _show)
        {
            ALUGUICommon.setGameObjEnable(listSearchResultShow, _show);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2508); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2508); } }
    }
}