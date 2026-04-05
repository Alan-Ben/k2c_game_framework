using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡 节点item地图页面
    /// </summary>
    public class GGUIMonoChapterMapPage : _AALBasicUIWndMono
    {
        [ALHeader("node列表")]
        public List<GGUIMonoChapterMapNodeItem> nodeItemList;
    }
}