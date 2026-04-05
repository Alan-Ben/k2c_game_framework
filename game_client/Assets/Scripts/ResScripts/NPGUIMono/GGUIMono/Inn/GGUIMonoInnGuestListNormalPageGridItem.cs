using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnGuestListNormalPageGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("客人名字")]
        public Text txtName;
        [ALHeader("客人图标")]
        public RawImage imgIcon;
        [ALHeader("解锁相关展示内容")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listLockShow;
        public List<MaskableGraphic> listLockGray;
        [ALHeader("解锁相关颜色内容")] 
        public List<Graphic> listLockStateColor;
        public Color lockColor;
        public Color unlockColor;
        [ALHeader("可领奖相关展示内容")]
        public GameObject btnGetReward;
        public List<GameObject> listCanGetRewardShow;
        public List<GameObject> listCannotGetRewardShow;
        [ALHeader("详情按钮")]
        public GameObject btnDetail;


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
        public void setCanGetReward(bool _canGetReward)
        {
            ALUGUICommon.setGameObjEnable(listCanGetRewardShow, false);
            ALUGUICommon.setGameObjEnable(listCannotGetRewardShow, false);
            ALUGUICommon.setGameObjEnable(_canGetReward ? listCanGetRewardShow : listCannotGetRewardShow, true);
        }
#endif
    }
}