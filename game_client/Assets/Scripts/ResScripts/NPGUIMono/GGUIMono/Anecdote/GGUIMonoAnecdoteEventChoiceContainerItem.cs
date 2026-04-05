using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoAnecdoteEventChoiceContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("选项名字")]
        public Text txtChoiceName;
        [ALHeader("选项图片")]
        public RawImage imgChoiceTex;
        [ALHeader("选择按钮")]
        public GameObject btnSelect;
        [ALHeader("选中时的显隐列表")]
        public List<GameObject> listSelectShow;
        public List<GameObject> listSelectHide;


        public void setSelect(bool _isSelect)
        {
            ALUGUICommon.setGameObjEnable(listSelectShow, false);
            ALUGUICommon.setGameObjEnable(listSelectHide, false);
            ALUGUICommon.setGameObjEnable(_isSelect ? listSelectShow : listSelectHide, true);
        }
    }
}