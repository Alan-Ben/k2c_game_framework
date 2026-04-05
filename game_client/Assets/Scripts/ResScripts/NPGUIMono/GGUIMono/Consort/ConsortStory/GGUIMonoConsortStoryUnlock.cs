using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子故事解锁弹窗
    /// </summary>
    public class GGUIMonoConsortStoryUnlock : _AALBasicUIWndMono
    {
        [ALHeader("解锁提示")]
        public TextEx txtUnlockTip;

        [ALHeader("故事需要触发时解锁提示使用key(两个参数, 参数1:解锁条件描述, 参数2:触发方式描述)")]
        public string storyTriggerNeedfulUnlockTipKey;
        
        [ALHeader("故事不需要触发时解锁提示使用key(一个参数, 解锁条件描述)")]
        public string storyTriggerNeedlessUnlockTipKey;

        [ALHeader("故事名")]
        public TextEx txtStoryName;
        
        [ALHeader("确认按钮")]
        public GameObject btnSure;
        [ALHeader("前往按钮")]
        public GameObject btnGoto;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1418); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1418);} }
    }
}