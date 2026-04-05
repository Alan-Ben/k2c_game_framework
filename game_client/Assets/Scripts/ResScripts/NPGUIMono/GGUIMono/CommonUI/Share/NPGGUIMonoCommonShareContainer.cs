using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 分享item列表
    /// </summary>
    public class NPGGUIMonoCommonShareContainer : _ATNPGGUIMonoShowAnimContainer<NPGGUIMonoCommonShareItem>
    {
        [ALHeader("无可分享对象时 显示的物体")]
        public List<GameObject> goListShowOnEmpty;
    }
}
