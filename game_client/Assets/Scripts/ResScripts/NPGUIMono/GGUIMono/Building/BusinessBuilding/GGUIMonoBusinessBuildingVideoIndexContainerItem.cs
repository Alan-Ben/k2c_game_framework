using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoBusinessBuildingVideoIndexContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("生效时的显隐列表")]
        public List<GameObject> listEnableShow;
        public List<GameObject> listEnableHide;


        public void setEnable(bool _enable)
        {
            ALUGUICommon.setGameObjEnable(listEnableShow, false);
            ALUGUICommon.setGameObjEnable(listEnableHide, false);
            ALUGUICommon.setGameObjEnable(_enable ? listEnableShow : listEnableHide, true);
        }
    }
}