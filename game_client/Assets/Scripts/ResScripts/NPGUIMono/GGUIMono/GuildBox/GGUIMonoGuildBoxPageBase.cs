using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱-免费宝箱页面
    /// </summary>
    public class GGUIMonoGuildBoxPageBase : _AALBasicUIWndMono
    {
        [ALHeader("宝箱列表")]
        public GGUIMonoGuildBoxGrid itemGrid;
        [ALHeader("一键领取")]
        public GameObject btnCollectAll;
        [ALHeader("不可一键领取时置灰的列表")]
        public List<MaskableGraphic> canNotCollectAllGrayList;
    }
}