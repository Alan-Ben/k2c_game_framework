using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class NPGGUIMonoCommonPageTagItem : _AALBasicUIWndMono
    {
        [ALHeader("选中时显示的物体")]
        public List<GameObject> goListShowOnSelect;

        [ALHeader("选中时隐藏的物体")]
        public List<GameObject> goListHideOnSelect;
    }
}
