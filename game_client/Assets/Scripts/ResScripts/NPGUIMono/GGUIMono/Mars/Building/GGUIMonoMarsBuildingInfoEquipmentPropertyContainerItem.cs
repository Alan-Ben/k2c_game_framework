using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsBuildingInfoEquipmentPropertyContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("属性名")]
        public Text txtTitle;
		[ALHeader("属性 icon ")]
        public RawImage imgIcon;
		[ALHeader("属性当前的值")]
        public Text txtValue;
		[ALHeader("属性下一级的值")]
        public Text txtNextValue;
        [ALHeader("是否满级的相关显示")]
        public List<GameObject> listLevelMaxShow;
        public List<GameObject> listLevelNotMaxShow;
        [ALHeader("描述按钮")]
        public GameObject btnDesc;
        public float toolTipInterval;


        public void setLevelMaxState(bool _isMax)
        {
            ALUGUICommon.setGameObjEnable(listLevelMaxShow, false);
            ALUGUICommon.setGameObjEnable(listLevelNotMaxShow, false);
            ALUGUICommon.setGameObjEnable(_isMax ? listLevelMaxShow : listLevelNotMaxShow, true);
        }
    }
}