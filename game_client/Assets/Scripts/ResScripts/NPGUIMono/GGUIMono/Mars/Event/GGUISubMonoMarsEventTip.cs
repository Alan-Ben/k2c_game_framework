using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 事件提示子窗口
    /// </summary>
    public class GGUISubMonoMarsEventTip : _AALBasicUIWndMono
    {
        [ALHeader("tip父节点")]
        public Transform parentGo;

        [ALHeader("显示的tip的NPCenterTipsRefObj表id(资源应该挂载的脚本为NPGGUIMonoIconTextTip)")]
        public long showCenterTipRefObjId;
    }
}