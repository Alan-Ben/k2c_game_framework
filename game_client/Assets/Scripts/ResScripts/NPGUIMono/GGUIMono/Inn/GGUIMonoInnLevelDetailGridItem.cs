using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnLevelDetailGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("等级名称")]
        public Text txtLevelName;
        [ALHeader("奖牌图标")]
        public RawImage imgMedalIcon;
        [ALHeader("需要人气值")]
        public Text txtPopularityRequire;
        [ALHeader("迎宾次数上限")]
        public Text txtReceiveGuestLimit;
        [ALHeader("解锁客人数量")]
        public Text txtGuestUnlockNum;
        [ALHeader("当前是否是这个等级的展示")]
        public List<GameObject> listCurrentShow;
        public List<GameObject> listCurrentHide;
        [ALHeader("当前状态时需要变色的UI")]
        public List<Graphic> listCurrentColorChange;
        public Color currentColor;
        public Color normalColor;
        [ALHeader("星级图标容器")]
        public GGUIMonoInnLevelIconContainer monoStarIconContainer;
        
        
        public void setCurrentShow(bool _isCurrent)
        {
            ALUGUICommon.setGameObjEnable(listCurrentShow, false);
            ALUGUICommon.setGameObjEnable(listCurrentHide, false);
            ALUGUICommon.setGameObjEnable(_isCurrent ? listCurrentShow : listCurrentHide, true);
            Color targetColor = _isCurrent ? currentColor : normalColor;
            ALUGUICommon.setUIObjColor(listCurrentColorChange, targetColor);
        }
    }
}