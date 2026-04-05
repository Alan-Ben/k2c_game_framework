using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoPlayerHeroGainGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("大臣的头像")]
        public RawImage imgHero;
        [ALHeader("大臣的名字")]
        public Text txtHeroName;
        [ALHeader("大臣的属性图标")]
        public RawImage imgAttr;
        [ALHeader("大臣的属性名")]
        public Text txtAttrName;
        
        [ALHeader("跳转按钮")]
        public GameObject btnJump;
        [ALHeader("未获得伙伴详情按钮")]
        public GameObject btnLockDetail;
        [ALHeader("已获得伙伴详情按钮")]
        public GameObject btnUnLockDetail;
        [ALHeader("解锁提示")]
        public Text txtUnlockTip;

        [ALInfo("四种状态的显示列表")]
        [ALHeader("已获得显示列表")]
        public List<GameObject> listGainedShow;
        [ALHeader("已解锁未获得显示列表")]
        public List<GameObject> listUnlockShow;
        [ALHeader("第一个未解锁显示列表")]
        public List<GameObject> listFirstLockShow;
        [ALHeader("非第一个未解锁显示列表")]
        public List<GameObject> listLockShow;

        [ALHeader("未解锁和未获得时的置灰列表")]
        public List<MaskableGraphic> listLockGray;
        public List<MaskableGraphic> listNotGainedGray;

        [ALHeader("红点GO")]
        public GameObject goRedTip;

#if NP_GAME

        /// <summary>
        /// 设置解锁与否
        /// </summary>
        public void setUnlockState(bool _unlock, bool _gained, bool _isFirstLock)
        {
            ALUGUICommon.setGameObjEnable(listGainedShow, false);
            ALUGUICommon.setGameObjEnable(listUnlockShow, false);
            ALUGUICommon.setGameObjEnable(listFirstLockShow, false);
            ALUGUICommon.setGameObjEnable(listLockShow, false);
            if (_gained)
                ALUGUICommon.setGameObjEnable(listGainedShow, true);
            else
                ALUGUICommon.setGameObjEnable(_unlock ? listUnlockShow : (_isFirstLock ? listFirstLockShow : listLockShow), true);
            
            GGameCommonInfo.disgrayImage(listLockGray);
            GGameCommonInfo.disgrayImage(listNotGainedGray);
            if (!_unlock)
                GGameCommonInfo.grayImage(listLockGray);
            if (!_gained)
                GGameCommonInfo.grayImage(listNotGainedGray);
        }
        
#endif

    }
}