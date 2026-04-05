using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingRnDPageProductGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("业务图片和名字")]
        public RawImage imgProductIcon;
        public Text txtProductName;
        [ALHeader("业务的加成")]
        public GGUIMonoCommonBonusShower monoProductBonus;
        [ALHeader("解锁所需的员工数量")]
        public Text txtUnlockRequire;
        [ALHeader("解锁按钮")]
        public GameObject btnUnlock;
        [ALHeader("解锁进度条")]
        public NPGGUIMonoProgress monoUnlockProgress;
        [ALHeader("解锁，可以解锁，未达成的显示列表，下一个即将可解锁")]
        public List<GameObject> listUnlockStateShow;
        public List<GameObject> listCanUnlockStateShow;
        public List<GameObject> listCannotUnlockStateShow;
        public List<GameObject> listNextUnlockStateShow;
        [ALHeader("未解锁时置灰列表")]
        public List<MaskableGraphic> listLockedGray;
        [ALHeader("名字文本颜色")]
        public List<Graphic> listNameColorTargets;
        public Color colorNameUnlocked = Color.white;
        public Color colorNameLocked = Color.white;
        [ALHeader("加成文本颜色")]
        public List<Graphic> listBonusColorTargets;
        public Color colorBonusUnlocked = Color.white;
        public Color colorBonusLocked = Color.white;


        public void setStateShow(bool _canUnlock, bool _unlocked, bool _isNextUnlock)
        {
            ALUGUICommon.setGameObjEnable(listUnlockStateShow, false);
            ALUGUICommon.setGameObjEnable(listCanUnlockStateShow, false);
            ALUGUICommon.setGameObjEnable(listCannotUnlockStateShow, false);
            ALUGUICommon.setGameObjEnable(listNextUnlockStateShow, false);

            if (_unlocked)
                ALUGUICommon.setGameObjEnable(listUnlockStateShow, true);
            else if (_canUnlock)
                ALUGUICommon.setGameObjEnable(listCanUnlockStateShow, true);
            else if(_isNextUnlock)
                ALUGUICommon.setGameObjEnable(listNextUnlockStateShow, true);
            else
                ALUGUICommon.setGameObjEnable(listCannotUnlockStateShow, true);

            _setLockedGray(!_unlocked);
            ALUGUICommon.setUIObjColor(listNameColorTargets, _unlocked ? colorNameUnlocked : colorNameLocked);
            ALUGUICommon.setUIObjColor(listBonusColorTargets, _unlocked ? colorBonusUnlocked : colorBonusLocked);
        }

        private void _setLockedGray(bool _isLocked)
        {
#if NP_GAME
            if (_isLocked)
                GGameCommonInfo.grayImage(listLockedGray);
            else
                GGameCommonInfo.disgrayImage(listLockedGray);
#endif
        }
    }
}