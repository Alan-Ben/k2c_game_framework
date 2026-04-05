using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 功能入口解锁提示附加窗口
    /// </summary>
    public class GGUIMonoSubEntryFuncUnlockTip : _AALBasicUIWndMono
    {
        [ALHeader("提示内容")]
        public Text txtContent;
        [ALHeader("倒计时")]
        public Text txtCountdown;
        [ALHeader("无CD时需要隐藏的列表")]
        public List<GameObject> goNoCDHideList;
    }
}