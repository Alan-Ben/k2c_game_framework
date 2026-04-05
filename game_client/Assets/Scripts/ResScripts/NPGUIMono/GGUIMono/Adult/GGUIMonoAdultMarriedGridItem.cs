using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultMarriedGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("双方的子嗣信息")]
        public GGUIMonoChildInfo monoMyChildInfo;
        public GGUIMonoChildInfo monoOtherChildInfo;
        [ALHeader("双方子嗣的详情按钮")]
        public GameObject btnMyChildDetail;
        public GameObject btnOtherChildDetail;
        public Vector2 detailInterval;
        [ALHeader("结婚时间")]
        public Text txtMarriedTime;
        [ALHeader("两人的总收益")]
        public Text txtTotalEarnings;
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