using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingInfoEquipmentContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("图标")]
        public RawImage imgIcon;
        [ALHeader("等级")]
        public Text txtLevel;
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
        [ALHeader("选中和未选中时显示的内容")]
        public List<GameObject> listSelectShow;
        public List<GameObject> listUnSelectShow;
        [ALHeader("主副部件的显示内容")]
        public List<GameObject> listMainEquipmentShow;
        public List<GameObject> listSubEquipmentShow;
        [ALHeader("解锁和锁定时显示的内容")]
        public List<GameObject> listUnlockShow;
        public List<GameObject> listLockShow;
        public List<MaskableGraphic> listLockGray;
        [ALHeader("升级动画")]
        public Animation upgradeEffectAnim;
        public string upgradeEffectAnimName;


        #if NP_GAME
        public void setSelectState(bool _select)
        {
            ALUGUICommon.setGameObjEnable(listSelectShow, false);
            ALUGUICommon.setGameObjEnable(listUnSelectShow, false);
            ALUGUICommon.setGameObjEnable(_select ? listSelectShow : listUnSelectShow, true);
        }
        public void setEquipmentTypeState(bool _isMain)
        {
            ALUGUICommon.setGameObjEnable(listMainEquipmentShow, false);
            ALUGUICommon.setGameObjEnable(listSubEquipmentShow, false);
            ALUGUICommon.setGameObjEnable(_isMain ? listMainEquipmentShow : listSubEquipmentShow, true);
        }
        public void setLockState(bool _isUnlock)
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