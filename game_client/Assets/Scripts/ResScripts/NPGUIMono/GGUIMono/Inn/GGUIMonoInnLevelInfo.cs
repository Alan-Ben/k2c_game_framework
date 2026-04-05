using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnLevelInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("当前等级的名字")]
        public Text txtName;
        [ALHeader("当前等级")]
        public Text txtLevel;
        [ALHeader("升级进度")]
        public Slider sldLevelProgress;
        public Text txtLevelProgress;
        [ALHeader("等级详情按钮")]
        public GameObject btnDetail;
        [ALHeader("当前人气值")]
        public Text txtCurPopularity;
        [ALHeader("迎宾次数上限")]
        public Text txtCurReceiveGuestLimit;
        [ALHeader("当前解锁客人数量")]
        public Text txtCurGuestUnlockNum;
        [ALHeader("满级时展示的内容")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelMaxHide;
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("星级图标容器")]
        public GGUIMonoInnLevelIconContainer monoStarIconContainer;


        public void setLevelMax(bool _levelMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelMaxHide, false);
            ALUGUICommon.setGameObjEnable(_levelMax ? listLevelMaxShow : listLevelMaxHide, true);
        }
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6416); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6416); } }
    }
}