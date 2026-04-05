using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnSpecialGuestInfo : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("客人头像")]
        public RawImage imgIcon;
        [ALHeader("客人名字")]
        public Text txtName;
        [ALHeader("客人描述")]
        public Text txtDesc;
        [ALHeader("解锁提示列表")]
        public GGUIMonoInnGuestUnlockTipContainer monoUnlockTipContainer;
        [ALHeader("解锁相关显示内容")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listLockShow;
        public List<MaskableGraphic> listLockGray;
        [ALHeader("解锁相关颜色内容")] 
        public List<Graphic> listLockStateColor;
        public Color lockColor;
        public Color unlockColor;
        [ALHeader("故事回看按钮")]
        public GameObject btnStory;
#if NP_GAME
      
        public void setUnlock(bool _isUnlock)
        {
            ALUGUICommon.setGameObjEnable(listUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listLockShow, false);
            ALUGUICommon.setGameObjEnable(_isUnlock ? listUnlockShow : listLockShow, true);
            if (_isUnlock)
                GGameCommonInfo.disgrayImage(listLockGray);
            else
                GGameCommonInfo.grayImage(listLockGray);
            ALUGUICommon.setUIObjColor(listLockStateColor, _isUnlock ? unlockColor : lockColor);
        }
        
#endif
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6429); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6429); } }
    }
}