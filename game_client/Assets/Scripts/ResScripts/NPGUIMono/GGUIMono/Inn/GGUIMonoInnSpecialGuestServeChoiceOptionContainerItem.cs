using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnSpecialGuestServeChoiceOptionContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("选项图标")]
        public RawImage imgIcon;
        [ALHeader("选项描述")]
        public Text txtDesc;
        [ALHeader("选项按钮")]
        public GameObject btnSelect;
        [ALHeader("选中时显示的内容")]
        public List<GameObject> listSelectedShow;
        public List<GameObject> listSelectedHide;
        
        
        public void setSelected(bool _selected)
        {
            ALUGUICommon.setGameObjEnable(listSelectedShow, false);
            ALUGUICommon.setGameObjEnable(listSelectedHide, false);
            ALUGUICommon.setGameObjEnable(_selected ? listSelectedShow : listSelectedHide, true);
        }
    }
}