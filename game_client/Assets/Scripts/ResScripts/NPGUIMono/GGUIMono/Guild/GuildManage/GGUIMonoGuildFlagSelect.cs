using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟旗帜选择界面
    /// </summary>
    public class GGUIMonoGuildFlagSelect : _AALBasicUIWndMono
    {
        [ALHeader("点击确认")]
        public GameObject btnConfirm;
        [ALHeader("修改按钮")]
        public GameObject btnChg;
        [ALHeader("选中的旗帜图标")]
        public RawImage imgIcon;
        [ALHeader("旗帜列表")]
        public GGUIMonoGuildFlagSelectGrid monoFlagGrid;
        [ALHeader("修改旗帜消耗")]
        public NPGGUIMonoCommonItem monoChgCost;
        [ALHeader("选中当前使用中的旗帜时需要显示的GO列表")]
        public List<GameObject> goSelectCurShowList;
        [ALHeader("选中当前使用中的旗帜时需要隐藏的GO列表")]
        public List<GameObject> goSelectCurHideList;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(4902); } }
        public static string objName { get { return UIResPathAssistant.getObjName(4902); } }
    }
}
