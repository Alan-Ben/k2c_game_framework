using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class NPGGUIBtnStateShowParam<T> where T : Enum
    {
        [ALHeader("类型")]
        public T type;

        [ALHeader("显示的物体")]
        public List<GameObject> goList;
    }

    public abstract class _ATNPGGUIMonoStateBtn<T> : _AALBasicUIWndMono where T : Enum
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;

        [ALHeader("不同类型的显示配置")]
        public List<NPGGUIBtnStateShowParam<T>> btnStateShowParamList;
    }
}
