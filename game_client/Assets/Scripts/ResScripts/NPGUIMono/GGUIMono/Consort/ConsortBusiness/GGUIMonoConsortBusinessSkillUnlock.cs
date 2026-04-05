using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 经营技能解锁弹窗
    /// </summary>
    public class GGUIMonoConsortBusinessSkillUnlock : _AALBasicUIWndMono
    {
        [ALHeader("解锁提示")]
        public TextEx txtUnlockTip;
        
        [ALHeader("经营技能图标")]
        public RawImage imgSkillIcon;
        [ALHeader("技能效果描述")]
        public TextEx txtSkillEffectDesc;
        
        [ALHeader("今日不再提示toggle")]
        public NPGGUIMonoCommonToggleEx monoDontShowTodayToggle;

        [ALHeader("确认按钮")]
        public GameObject btnSure;
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1419); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1419);} }
    }
}