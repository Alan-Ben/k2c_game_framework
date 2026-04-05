using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 账号绑定窗口
    /// </summary>
    public class NPGGUIMonoAccountBind : _ANPBasicUIWndResBarMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("复制CID按钮")]
        public GameObject btnCopy;
        [ALHeader("玩家头像")]
        public NPGGUIMonoPlayerIcon monoPlayerIcon;
        [ALHeader("当前登录方式图标")]
        public RawImage imgLoginType;
        [ALHeader("所在服务器")]
        public Text txtServer;
        [ALHeader("绑定方式列表")]
        public NPGGUIMonoAccountBindContainer monoBindContainer;
        [ALHeader("切换账号按钮")]
        public GameObject btnSwitchAccount;

        [ALHeader("正在加载列表时需要展示的GO列表")]
        public List<GameObject> goLoadingShowList;
        [ALHeader("正在加载列表时需要隐藏的GO列表")]
        public List<GameObject> goLoadingHideList;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1740); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1740); } }
    }
}

