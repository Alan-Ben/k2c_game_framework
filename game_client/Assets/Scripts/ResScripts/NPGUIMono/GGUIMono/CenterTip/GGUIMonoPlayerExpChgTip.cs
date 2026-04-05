using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 玩家经验变化上浮提示
    /// </summary>
    public class GGUIMonoPlayerExpChgTip : _AALBasicUIWndMono
    {
        [ALHeader("等级图标")]
        public RawImage imgIcon;
        [ALHeader("当前经验进度条")]
        public NPGGUIMonoProgress expProgress;
        [ALHeader("无效的时间间隔")]
        public float disableTime = 2f;
        [ALHeader("数值变化使用时间")]
        public float value_lerp_time = 0.5f;

        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3408); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3408); } }
    }
}
