using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum EPlayerSkinBtnState
    {
        [InspectorName("LOCK_NEED_UNLOCK_ITEM（未解锁，解锁需要解锁道具）")]
        LOCK_NEED_UNLOCK_ITEM,
        [InspectorName("UNLOCK_NO_WEAR（已解锁未穿戴）")]
        UNLOCK_NO_WEAR,
        [InspectorName("UNLOCK_WEAR（已解锁已穿戴）")]
        UNLOCK_WEAR,
        [InspectorName("LOCK_NO_UNLOCK_ITEM（未解锁，解锁不需要解锁道具）")]
        LOCK_NO_UNLOCK_ITEM,
    }

    [System.Serializable]
    public  class PlayerSkinBtnShowState
    {
        [ALHeader("显示类型")]
        public EPlayerSkinBtnState state;
        [ALHeader("需要显示的GO列表")]
        public List<GameObject> goShowList;
        [ALHeader("需要隐藏的GO列表")]
        public List<GameObject> goHideList;
    }

    /// <summary>
    /// 玩家皮肤界面
    /// </summary>
    public class GGUIMonoPlayerSkin : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("皮肤形象")]
        public GGUIMonoCommonShowCase monoShowCase;
        [ALHeader("皮肤item列表")]
        public GGUIMonoPlayerSkinContainer monoPlayerSkinContainer;
        [ALHeader("名称")]
        public Text txtName;
        [ALHeader("描述")]
        public Text txtDesc;
        [ALHeader("亲密度加成")]
        public Text txtAddIntimacyValue;
        [ALHeader("加护力加成")]
        public Text txtAddCharmValue;
        [ALHeader("家人数量")]
        public Text txtConsortCount;
        [ALHeader("解锁道具")]
        public NPGGUIMonoCommonItem monoUnlockItem;
        [ALHeader("解锁按钮")]
        public GameObject btnUnlock;
        [ALHeader("穿戴按钮")]
        public GameObject btnWear;
        [ALHeader("取消穿戴按钮")]
        public GameObject btnTakeOff;
        [ALHeader("升级道具")]
        public NPGGUIMonoCommonItem monoUpgradeItem;
        [ALHeader("升级按钮")]
        public GameObject btnUpgrade;
        [ALHeader("不需要解锁道具的未解锁按钮")]
        public GameObject btnLockNoUnlockItem;
        [ALHeader("有加成属性时需要显示的GO列表")]
        public List<GameObject> goHavePropertyShowList;
        [ALHeader("有加成属性时需要隐藏的GO列表")]
        public List<GameObject> goHavePropertyHideList;
        [ALHeader("按钮显示状态列表")]
        public List<PlayerSkinBtnShowState> btnShowState;
        [ALHeader("满级时需要显示的GO列表")]
        public List<GameObject> goMaxLevelShowList;
        [ALHeader("满级时需要隐藏的GO列表")]
        public List<GameObject> goMaxLevelHideList;
        [ALHeader("可解锁红点")]
        public GameObject goUnlockRedTip;
        [ALHeader("可升级红点")]
        public GameObject goUpgradeRedTip;

        /// <summary>
        /// 设置按钮状态显隐
        /// </summary>
        /// <param name="state"></param>
        public void setBtnShowState(EPlayerSkinBtnState state)
        {
            foreach (PlayerSkinBtnShowState item in btnShowState)
            {
                if (item != null && item.state == state)
                {
                    ALUGUICommon.setGameObjEnable(item.goShowList,true);
                    ALUGUICommon.setGameObjEnable(item.goHideList,false);
                    break;
                }
            }
        }

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1723); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1723); } }
    }
}

