using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游戏效果设置窗口
    /// </summary>
    public class NPGGUIMonoGameSettingEffect : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("最低画质按钮")]
        public NPGGUIMonoCommonToggleEx monoToggleQualityVeryLow;
        [ALHeader("低画质按钮")]
        public NPGGUIMonoCommonToggleEx monoToggleQualityLow;
        [ALHeader("中画质按钮")]
        public NPGGUIMonoCommonToggleEx monoToggleQualityNormal;
        [ALHeader("高画质按钮")]
        public NPGGUIMonoCommonToggleEx monoToggleQualityHigh;
        [ALHeader("最高画质按钮")]
        public NPGGUIMonoCommonToggleEx monoToggleQualityUltra;
        
        [ALHeader("高帧率开关")]
        public NPGGUIMonoCommonTab monoFPSTab;
        [ALHeader("屏幕点击效果开关")]
        public NPGGUIMonoCommonTab monoScreenClickEffectTab;
        [ALHeader("战斗大招子弹效果开关")]
        public NPGGUIMonoCommonTab monoBattleSkillTab;
        [ALHeader("降分辨率开关")]
        public NPGGUIMonoCommonTab monoLowResolutionTab;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1749); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1749); } }
    }
}
