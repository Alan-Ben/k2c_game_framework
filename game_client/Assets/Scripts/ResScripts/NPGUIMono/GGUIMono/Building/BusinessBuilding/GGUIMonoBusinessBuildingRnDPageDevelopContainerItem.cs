using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoBusinessBuildingRnDPageDevelopContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("业务图片，名字，描述")]
        public RawImage imgDevelopIcon;
        public Text txtDevelopName;
        public Text txtDevelopDesc;
        [ALHeader("业务的加成")]
        public GGUIMonoCommonBonusShower monoDevelopBonus;
        [ALHeader("解锁所需的等级")]
        public Text txtUnlockRequire;
        [ALHeader("解锁，未解锁的显示列表")]
        public List<GameObject> listUnlockStateShow;
        public List<GameObject> listLockStateShow;
        [ALHeader("在列表第一个需要特殊展示的GO列表")]
        public List<GameObject> goFirstSpcShowList;
        [ALHeader("在列表第一个需要特殊隐藏的GO列表")]
        public List<GameObject> goFirstSpcHideList;


        public void setStateShow(bool _unlocked, bool _isFirst)
        {
            ALUGUICommon.setGameObjEnable(listUnlockStateShow, false);
            ALUGUICommon.setGameObjEnable(listLockStateShow, false);
            ALUGUICommon.setGameObjEnable(_unlocked ? listUnlockStateShow : listLockStateShow, true);
            ALUGUICommon.setGameObjEnable(goFirstSpcShowList, _isFirst);
            ALUGUICommon.setGameObjEnable(goFirstSpcHideList, !_isFirst);
        }
    }
}