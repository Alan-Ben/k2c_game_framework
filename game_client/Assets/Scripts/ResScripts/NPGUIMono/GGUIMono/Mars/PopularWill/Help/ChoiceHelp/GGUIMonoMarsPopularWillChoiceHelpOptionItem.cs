using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsPopularWillChoiceHelpOptionItem : _AALBasicUIWndMono
    {
        [ALHeader("选项描述")]
        public TextEx txtDesc;
     
        [ALHeader("选中时 显示的物体")]
        public List<GameObject> selectShowGoList;
        [ALHeader("未选中时 显示的物体")]
        public List<GameObject> unSelectShowGoList;

        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}