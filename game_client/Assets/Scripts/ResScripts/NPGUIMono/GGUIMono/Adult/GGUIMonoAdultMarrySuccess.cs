using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultMarrySuccess : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("双方子嗣信息")]
        public GGUIMonoChildInfo monoMyChildInfo;
        public GGUIMonoChildInfo monoOtherChildInfo;
        [ALHeader("双方子嗣随机气泡列表")]
        public List<GameObject> listMyChildRandomBubble;
        public List<GameObject> listOtherChildRandomBubble;
        [ALHeader("联姻谢礼")]
        public NPGGUIMonoCommonItem monoMarryItem;
        [ALHeader("联姻时间")]
        public Text txtDate;
        [ALHeader("加载中显示和隐藏的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        
        
        public void setLoadingShow(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isShow ? listLoadingShow : listLoadingHide, true);
        }
        public void refreshBubble()
        {
            ALUGUICommon.setGameObjEnable(listMyChildRandomBubble, false);
            ALUGUICommon.setGameObjEnable(listOtherChildRandomBubble, false);
            ALUGUICommon.setGameObjEnable(listMyChildRandomBubble.GetRandomItem(), true);
            ALUGUICommon.setGameObjEnable(listOtherChildRandomBubble.GetRandomItem(), true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(2505); } }
        public static string objName { get { return UIResPathAssistant.getObjName(2505); } }
    }
}