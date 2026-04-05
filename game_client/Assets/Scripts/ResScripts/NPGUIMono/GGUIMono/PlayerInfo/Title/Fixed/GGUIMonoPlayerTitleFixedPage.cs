using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public enum EPlayerTitleFixedBtnState
    {
        [InspectorName("CUR_WEAR（当前穿戴）")]
        CUR_WEAR,
        [InspectorName("CAN_WEAR（可穿戴）")]
        CAN_WEAR,
        [InspectorName("LOCK（未解锁）")]
        LOCK,
    }

    [System.Serializable]
    public class GGUIPlayerTitleFixedBtnStateMono
    {
        [ALHeader("按钮状态")]
        public EPlayerTitleFixedBtnState btnState;
        [ALHeader("需要显示的GO列表")]
        public List<GameObject> goShowList;
        [ALHeader("需要隐藏的GO列表")]
        public List<GameObject> goHideList;
    }

    /// <summary>
    /// 玩家固定称号界面
    /// </summary>
    public class GGUIMonoPlayerTitleFixedPage : _AALBasicUIWndMono
    {
        [ALHeader("预览的称号")]
        public GGUIMonoSubPlayerTitle monoTitleShow;
        [ALHeader("称号列表")]
        public GGUIMonoPlayerTitleFixedGrid monoGrid;
        [ALHeader("称号描述")]
        public Text txtDesc;
        [ALHeader("获得时间")]
        public Text txtGainTime;
        [ALHeader("穿戴按钮")]
        public GameObject btnWear;
        [ALHeader("向其他人展示称号开关")]
        public NPGGUIMonoCommonToggleEx toggleShowOthers;
        [ALHeader("按钮状态列表")]
        public List<GGUIPlayerTitleFixedBtnStateMono> monoBtnSateList;

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="_state"></param>
        public void setBtnState(EPlayerTitleFixedBtnState _state)
        {
            if (monoBtnSateList == null)
                return;

            for (int i = 0; i < monoBtnSateList.Count; i++)
            {
                if (monoBtnSateList[i] != null && monoBtnSateList[i].btnState == _state)
                {
                    ALUGUICommon.setGameObjEnable(monoBtnSateList[i].goShowList, true);
                    ALUGUICommon.setGameObjEnable(monoBtnSateList[i].goHideList, false);
                    break;
                }
            }
        }
    }
}
