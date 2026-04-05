using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 账号切换窗口
    /// </summary>
    public class NPGGUIMonoAccountSwitch : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("切换账号列表")]
        public NPGGUIMonoAccountSwitchContainer monoSwitchContainer;
        [ALHeader("正在加载列表时需要展示的GO列表")]
        public List<GameObject> goLoadingShowList;
        [ALHeader("正在加载列表时需要隐藏的GO列表")]
        public List<GameObject> goLoadingHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1741); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1741); } }
    }
}

