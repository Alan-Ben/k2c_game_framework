using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 居民补充结果窗口
    /// </summary>
    public class GGUIMonoMarsResidentReplenishResult : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;

        [ALHeader("当居民数量达到上限时显示的对象列表")]
        public List<GameObject> onResidentNumReachLimitShowList;
        [ALHeader("当居民数量达到上限时隐藏的对象列表")]
        public List<GameObject> onResidentNumReachLimitHideList;
        
        [ALHeader("补充居民数量文本")]
        public TextEx txtReplenishNum;
        
        [ALHeader("奖励列表")]
        public GGUIMonoCommonRewardContainer monoRewardContainer;
        
        [ALHeader("确认按钮")]
        public GameObject btnSure;
        
        // 资源加载路径
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7202); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7202); } }
    }
}