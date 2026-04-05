using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 属性子窗体
    /// </summary>
    public class GGUIMonoConsortSkillAttrItem : _AALBasicUIWndMono
    {
        [ALHeader("属性")]
        public GGUIMonoCommonAttrItem attrItem;
        [ALHeader("为解锁置灰列表")]
        public List<MaskableGraphic> goLockGray;
    }
}
