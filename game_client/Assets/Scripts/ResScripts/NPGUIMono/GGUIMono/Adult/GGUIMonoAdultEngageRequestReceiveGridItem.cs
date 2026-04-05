
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestReceiveGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("子嗣信息")]
        public GGUIMonoChildInfo monoAdultInfo;
        [ALHeader("剩余时间")]
        public Text txtEnableTime;
        [ALHeader("拒绝按钮")]
        public GameObject btnRefuse;
        [ALHeader("同意按钮")]
        public GameObject btnAgree;
        [ALHeader("加载中显示和隐藏的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        
        
        public void setLoadingShow(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isShow ? listLoadingShow : listLoadingHide, true);
        }
    }
}