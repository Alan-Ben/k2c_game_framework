using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 关卡派遣事件选择大臣页面
    /// </summary>
    public class GGUIMonoCommonSimpleDispatchEventSelectHero : _AGGUIMonoCommonDispatchEvent
    {
        [ALHeader("大臣卡片列表")]
        public GGUIMonoHeroCommonSimpleDispatchSelectHeroGrid monoHeroCardGrid;

        [ALHeader("开始选择大臣时显示的Go列表")]
        public List<GameObject> startSelectHeroShowList;

        [ALHeader("开始选择大臣时隐藏的Go列表")]
        public List<GameObject> startSelectHeroHideList;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.C_COMMON_EVENT_DISPATCH_EVENT_RES_ID); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.C_COMMON_EVENT_DISPATCH_EVENT_RES_ID); } }
    }
}