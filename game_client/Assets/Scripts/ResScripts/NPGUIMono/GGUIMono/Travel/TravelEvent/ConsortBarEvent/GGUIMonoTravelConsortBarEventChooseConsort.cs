using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 酒馆事件
    /// </summary>
    public class GGUIMonoTravelConsortBarEventChooseConsort : _AALBasicUIWndMono
    {
        [ALHeader("显示动画名")]
        public string showAnimationName;
        [ALHeader("跳过显示动画按钮")]
        public GameObject skipShowAniBtn;
        [ALHeader("显示动画后, 屏蔽操作时间(秒)")]
        public float afterShowAniShieldOpTime;
        
        [ALHeader("妃子item列表")]
        public List<GGUIMonoTarvelConsortItem> travelConsortItemList;

        [ALHeader("选择消耗道具子窗口")]
        public GGUIMonoTravelConsortBarEventChooseCost monoChooseCost;
        
        [ALHeader("在显示选择消耗窗口时显示的物体")]
        public List<GameObject> onShowChooseCostWndShowGoList;
        [ALHeader("在显示选择消耗窗口时隐藏的物体")]
        public List<GameObject> onShowChooseCostWndHideGoList;
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(3610); } }
        public static string objName { get { return UIResPathAssistant.getObjName(3610); } }
    }
}