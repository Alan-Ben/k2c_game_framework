using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    [Serializable]
    public class _ATNPGGUIStateShowParam<T> where T : Enum
    {
        [ALHeader("类型")]
        public T type;

        [ALHeader("显示的物体")]
        public List<GameObject> goList;
    }

    public abstract class _ATNPGGUIMonoStateShow<_T_STATE_ENUM, _T_STATE_SHOW_PARAM_MONO> : _TALUGUIMonoGridItem 
        where _T_STATE_ENUM : Enum
        where _T_STATE_SHOW_PARAM_MONO : _ATNPGGUIStateShowParam<_T_STATE_ENUM>
    {
        [ALHeader("不同类型的显示配置")]
        public List<_T_STATE_SHOW_PARAM_MONO> diffStateShowParamList;
    }
}
