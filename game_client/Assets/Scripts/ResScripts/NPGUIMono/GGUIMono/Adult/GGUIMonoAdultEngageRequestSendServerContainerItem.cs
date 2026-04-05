
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAdultEngageRequestSendServerContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoAdultInfo;
        [ALHeader("联姻按钮")]
        public GameObject btnEngage;
        [ALHeader("加载中显示和隐藏的内容")]
        public List<GameObject> listLoadingShow;
        public List<GameObject> listLoadingHide;
        
        [ALHeader("有最低村庄收益时限制显示对象")]
        public List<GameObject> hasLowerEarningsLimitShow;
        [ALHeader("有最低村庄收益时限制隐藏对象")]
        public List<GameObject> hasLowerEarningsLimitHide;
        [ALHeader("限制最低村庄收益")]
        public TextEx txtLimitLowerEarnings;
        [ALHeader("限制最低村庄收益key(一个参数, 限制值, 程序会附上大数值单位, 如K,M等)")]
        public string limitLowerEarningsKey;
        
        [ALHeader("玩家子嗣收益低于要求的收益限制提示文本")]
        public string onSelfChildEarningsLessThanLimitTip;

        [ALHeader("收益达标颜色变化数据")]
        public List<Graphic> listEarningsColorChangeList;
        public Color colorEarningsEnough;
        public Color colorEarningsNotEnough;

        public void setEarningsColor(bool _isEnough)
        {
            ALUGUICommon.setUIObjColor(listEarningsColorChangeList, _isEnough ? colorEarningsEnough : colorEarningsNotEnough);
        }

        public void setLoadingShow(bool _isShow)
        {
            ALUGUICommon.setGameObjEnable(listLoadingShow, false);
            ALUGUICommon.setGameObjEnable(listLoadingHide, false);
            ALUGUICommon.setGameObjEnable(_isShow ? listLoadingShow : listLoadingHide, true);
        }
    }
}