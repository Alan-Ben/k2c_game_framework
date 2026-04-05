using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 组合称号页签类型
    /// </summary>
    public enum EPlayerTitleComboTabType
    {
        [InspectorName("PREFIX（前缀）")]
        PREFIX,
        [InspectorName("SUFFIX（后缀）")]
        SUFFIX,
        [InspectorName("BG（底框）")]
        BG,
    }

    /// <summary>
    /// 玩家组合称号页签
    /// </summary>
    [System.Serializable]
    public class GGUIPlayerTitleComboTabMono
    {
        [ALHeader("页签类型")]
        public EPlayerTitleComboTabType tabType;
        [ALHeader("通用页签脚本")]
        public NPGGUIMonoCommonTab monoTab;
    }

    public enum EPlayerTitleComboBtnState
    {
        [InspectorName("NO_SELECT（未选中）")]
        NO_SELECT,
        [InspectorName("CUR_WEAR（当前穿戴）")]
        CUR_WEAR,
        [InspectorName("CAN_WEAR（可穿戴）")]
        CAN_WEAR,
    }

    /// <summary>
    /// 组合称号界面按钮状态
    /// </summary>
    [System.Serializable]
    public class GGUIPlayerTitleComboBtnStateMono
    {
        [ALHeader("按钮状态")]
        public EPlayerTitleComboBtnState btnState;
        [ALHeader("需要显示的GO列表")]
        public List<GameObject> goShowList;
        [ALHeader("需要隐藏的GO列表")]
        public List<GameObject> goHideList;
    }

    /// <summary>
    /// 玩家组合称号界面
    /// </summary>
    public class GGUIMonoPlayerTitleComboPage : _AALBasicUIWndMono
    {
        [ALHeader("预览的组合称号")]
        public GGUIMonoSubPlayerTitle monoTitleShow;
        [ALHeader("选中的前缀解锁描述")]
        public Text txtPreSource;
        [ALHeader("选中的后缀解锁描述")]
        public Text txtSfxSource;
        [ALHeader("选中的底图解锁描述")]
        public Text txtBgSource;
        [ALHeader("页签列表")]
        public List<GGUIPlayerTitleComboTabMono> monoTabList;
        [ALHeader("前缀列表")]
        public GGUIMonoPlayerTitleComboTextGrid monoPrefixGrid;
        [ALHeader("后缀列表")]
        public GGUIMonoPlayerTitleComboTextGrid monoSuffixGrid;
        [ALHeader("底框列表")]
        public GGUIMonoPlayerTitleComboBgGrid monoBgGrid;
        [ALHeader("穿戴按钮")]
        public GameObject btnWear;
        [ALHeader("向其他人展示称号开关")]
        public NPGGUIMonoCommonToggleEx toggleShowOthers;
        [ALHeader("按钮状态配置")]
        public List<GGUIPlayerTitleComboBtnStateMono> monoBtnStateList;
        [ALHeader("未解锁详情tooltip偏移量")]
        public Vector2 unlockDetailToolTipInterval;

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="_state"></param>
        public void setBtnState(EPlayerTitleComboBtnState _state)
        {
            if (monoBtnStateList == null)
                return;

            for (int i = 0; i < monoBtnStateList.Count; i++)
            {
                if (monoBtnStateList[i] != null && monoBtnStateList[i].btnState == _state)
                {
                    ALUGUICommon.setGameObjEnable(monoBtnStateList[i].goShowList,true);
                    ALUGUICommon.setGameObjEnable(monoBtnStateList[i].goHideList,false);
                    break;
                }
            }

        }
    }
}

