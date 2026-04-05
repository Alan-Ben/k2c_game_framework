using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingInfoPageSettleSlotContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("派遣人员进度")]
        public Text txtSettleProgress;
        public Slider sldSettleProgress;
        [ALHeader("几级解锁")]
        public Text txtUnlockTip;
        [ALHeader("人数变化的动画")]
        public Animation effectAnim;
        public string effectAnimName;
        [ALHeader("解锁和未解锁时显示的内容")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listLockShow;
        public List<MaskableGraphic> listLockGray;


#if NP_GAME
        public void setUnlockState(bool _isUnlock)
        {
            ALUGUICommon.setGameObjEnable(listUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listLockShow, false);
            ALUGUICommon.setGameObjEnable(_isUnlock ? listUnlockShow : listLockShow, true);
            if (_isUnlock)
                GGameCommonInfo.disgrayImage(listLockGray);
            else
                GGameCommonInfo.grayImage(listLockGray);
        }
#endif
    }
}