using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoLoverCollectBtn : _AALBasicUIWndMono
    {
        [ALHeader("进入按钮")]
        public GameObject btnEnter;
        [ALHeader("赚速进度条")]
        public Slider sliderProgress;
        [ALHeader("赚速百分比文本")]
        public Text txtProgressPercent;
        [ALHeader("可以解救时显示")]
        public List<GameObject> listCanRescueShow;
        [ALHeader("不可以解救时显示")]
        public List<GameObject> listCanRescueHide;


#if NP_GAME
        public void setCanRescueState(bool _canRescue)
        {
            ALUGUICommon.setGameObjEnable(listCanRescueShow, false);
            ALUGUICommon.setGameObjEnable(listCanRescueHide, false);
            ALUGUICommon.setGameObjEnable(_canRescue ? listCanRescueShow : listCanRescueHide, true);
        }
#endif
    }
}
