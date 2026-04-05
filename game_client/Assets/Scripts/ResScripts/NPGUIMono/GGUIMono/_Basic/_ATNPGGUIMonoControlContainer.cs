using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 自己控制子窗口的Container
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    public class _ATNPGGUIMonoControlContainer<_T_ITEM_MONO> : _TALUGUIMonoContainerWnd<_T_ITEM_MONO> where _T_ITEM_MONO : _AALBasicUIWndMono
    {
        [ALHeader("没有数据要显示时显示列表")]
        public List<GameObject> noItemShow;
    }
}