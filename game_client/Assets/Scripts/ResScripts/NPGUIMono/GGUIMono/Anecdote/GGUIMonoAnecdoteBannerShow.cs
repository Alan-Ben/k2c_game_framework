using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoAnecdoteBannerShow : _AALBasicUIWndMono
    {
        [ALHeader("点击关闭按钮")]
        public GameObject btnClose;
        [ALHeader("自动关闭的时间")]
        public float autoCloseTime;
        
        
        // 特殊事件的资源路径和对象名
        public static string specialEventAssetPath { get { return UIResPathAssistant.getAssetPath(3407); } }
        public static string specialEventObjName { get { return UIResPathAssistant.getObjName(3407); } }
        // 事件未完待续的资源路径和对象名
        public static string toBeContinuedAssetPath { get { return UIResPathAssistant.getAssetPath(3408); } }
        public static string toBeContinuedObjName { get { return UIResPathAssistant.getObjName(3408); } }
        // 事件已结束的资源路径和对象名
        public static string eventEndedAssetPath { get { return UIResPathAssistant.getAssetPath(3409); } }
        public static string eventEndedObjName { get { return UIResPathAssistant.getObjName(3409); } }
    }
}