using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 到达火星着陆选择界面
    /// </summary>
    public class GGUIMonoMarsLandingSelect : _ANPBasicUIWndResBarMono
    {
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("着陆地点列表")]
        public List<GGUIMonoMarsLandingAreaItem> landingAreaList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7004); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7004); } }
    }
}
