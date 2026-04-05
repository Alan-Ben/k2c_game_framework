using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家限时称号界面
    /// </summary>
    public class GGUIMonoPlayerTitleLimitedPage : _AALBasicUIWndMono
    {
        [ALHeader("预览的称号")]
        public GGUIMonoSubPlayerTitle monoTitleShow;
        [ALHeader("称号来源")]
        public Text txtSource;
        [ALHeader("称号有效期")]
        public Text txtCd;
        [ALHeader("称号列表")]
        public GGUIMonoPlayerTitleLimitedGrid monoTitleLimitedGrid;
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

