using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 小游戏结算弹窗
    /// </summary>
    public class NPGGUIMonoMiniGameEnd : _AALBasicUIWndMono
    {
        [ALHeader("确认按钮")]
        public GameObject btnConfirm;
        [ALHeader("确认按钮文本")]
        public Text txtBtnConfirm;
        [ALHeader("本局得分文本")]
        public Text txtScore;
        [ALHeader("历史最高分文本")]
        public Text txtHighScore;
        [ALHeader("刷新记录时 显示的物体")]
        public List<GameObject> goListShowOnNewRecord;
        [ALHeader("窗体动画")]
        public Animation anim;
        [ALHeader("刷新记录动画")]
        public string aniNameNewRecord;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(10000); } }
        public static string objName { get { return UIResPathAssistant.getObjName(10000); } }
    }
}
