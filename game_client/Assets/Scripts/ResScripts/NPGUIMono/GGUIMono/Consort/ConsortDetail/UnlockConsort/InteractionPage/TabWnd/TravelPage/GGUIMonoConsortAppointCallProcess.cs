using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一键邀约表现过程
    /// </summary>
    public class GGUIMonoConsortAppointCallProcess : _AALBasicUIWndMono
    {
        [ALHeader("背景 showcase")]
        public GGUIMonoCommonShowCase monoBgShowcase;
        [ALHeader("背景的下标")]
        public int bgShowcaseIndex = 0;
        [ALHeader("加载的背景的默认材质")] 
        public Material defaultMaterial;
        [ALHeader("加载的背景的默认动画")] 
        public string defaultAniName;
        
        [ALHeader("对话子窗口")]
        public NPGGUIMonoSubDialogue monoSubDialogue;
        
        [ALHeader("显示背景动画名")]
        public string showBgAniName;
        [ALHeader("显示对话动画名")]
        public string showDialogAniName;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1427); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1427);} }
    }
}