using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsIntelligentControlContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("智能控制决策信息")]
        public GGUIMonoMarsIntelligentControlInfo intelligentControlInfo;

        [ALHeader("点击按钮")]
        public GameObject btnClick;
    }
}