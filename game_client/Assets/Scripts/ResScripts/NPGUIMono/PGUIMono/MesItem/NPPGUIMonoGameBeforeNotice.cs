using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 游戏前公告弹窗
    /// </summary>
    public class NPPGUIMonoGameBeforeNotice : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮列表")]
        public List<GameObject> btnCloseList;
        [ALHeader("标题")]
        public Text txtTitle;
        [ALHeader("内容")]
        public Text txtContent;
        [ALHeader("按钮")]
        public GameObject btn;
        [ALHeader("按钮文本")]
        public Text txtBtn;

        [ALHeader("没有公告时需要展示的GO列表")]
        public List<GameObject> goNoneShowList;
        [ALHeader("没有公告时需要隐藏的GO列表")]
        public List<GameObject> goNoneHideList;

        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "gui/plat_gui.unity3d"; } }
        public static string objName { get { return "win_game_before_notice"; } }
    }
}
