using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnStationListContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("等级和名字一起")]
        public Text txtLevelAndName;
        [ALHeader("只有名字")]
        public Text txtName;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("图标2")]
        public RawImage imgIcon2;
        [ALHeader("提示文本")]
        public Text txtTip;
        [ALHeader("点击按钮")]
        public GameObject btnEnter;
        [ALHeader("解锁相关展示内容")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listLockShow;
        public List<GameObject> listCanUnlockShow;
        public List<GameObject> listPendingUnlockShow;
        [ALHeader("未解锁时的置灰列表")] 
        public List<MaskableGraphic> listLockGrayShow;
        [ALHeader("满级时显隐的列表")]
        public List<GameObject> listMaxLevelShow;
        public List<GameObject> listMaxLevelHide;
        [ALHeader("可否升级时的显示内容")]
        public List<GameObject> listCanLevelUpShow;
        public List<GameObject> listCannotLevelUpShow;
        [ALHeader("选中时显隐的列表")]
        public List<GameObject> goSelectShowList;
        public List<GameObject> goSelectHideList;


#if NP_GAME
        public void setUnlock(bool _isUnlock, bool _canUnlock, bool _canLevelUp, bool _isPendingUnlock)
        {
            ALUGUICommon.setGameObjEnable(listUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listLockShow, false);
            ALUGUICommon.setGameObjEnable(listCanUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listCanLevelUpShow, false);
            ALUGUICommon.setGameObjEnable(listCannotLevelUpShow, false);
            ALUGUICommon.setGameObjEnable(listPendingUnlockShow, false);
            
            if (_isUnlock)
            {
                ALUGUICommon.setGameObjEnable(listUnlockShow, true);
                GGameCommonInfo.disgrayImage(listLockGrayShow);
                ALUGUICommon.setGameObjEnable(_canLevelUp ? listCanLevelUpShow : listCannotLevelUpShow, true);
            }
            else
            {
                GGameCommonInfo.grayImage(listLockGrayShow);
                if(_isPendingUnlock)
                    ALUGUICommon.setGameObjEnable(listPendingUnlockShow, true);
                else
                    ALUGUICommon.setGameObjEnable(_canUnlock ? listCanUnlockShow : listLockShow, true);
            }
        }
        public void setLevelMax(bool _levelMax) 
        {
            ALUGUICommon.setGameObjEnable(listMaxLevelShow, false);
            ALUGUICommon.setGameObjEnable(listMaxLevelHide, false);
            ALUGUICommon.setGameObjEnable(_levelMax ? listMaxLevelShow : listMaxLevelHide, true);
        }
#endif
    }
}