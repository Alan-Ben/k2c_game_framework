
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMainFunctionTab : _AALBasicUIWndMono
    {
        [ALHeader("按钮类型")]
        public EMainFunctionTabType tabType;
        [ALHeader("选中和未选中时展示的对象")]
        public List<GameObject> selectShow;
        public List<GameObject> unselectShow;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}