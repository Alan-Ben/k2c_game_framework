using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 物品获取途径弹窗
    /// </summary>
    public class NPGGUIMonoAccessWays : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("确定按钮")]
        public GameObject btnConfirm;
        [ALHeader("物品icon")]
        public NPGGUIMonoCommonItem monoItem;
        [ALHeader("物品描述")]
        public Text txtItemDesc;
        [ALHeader("item 的父对象")]
        public Transform goItemParent;
        [ALHeader("获取途径item")]
        public NPGGUIMonoAccessWayItem monoAccessWay;
        [ALHeader("使用物品item")]
        public NPGGUIMonoAccessBagItem monoBagItem;
        [ALHeader("合成item")]
        public NPGGUIMonoAccessCombinedItem monoCombinedItem;

        [ALInfo("以下为自适应设置")]
        [ALHeader("需要修改高度的RectTransform")]
        public RectTransform needChgHeightRectTransform;
        [ALHeader("列表layout")]
        public VerticalLayoutGroup listLayoutGroup;
        [ALHeader("没有item时窗口默认高度")]
        public float noItemDefaultHeight;
        [ALHeader("窗口最大高度")]
        public float maxHeight;


        public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_COMMON_ACCESS_WAYS); } }
        public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_COMMON_ACCESS_WAYS); } }
    }
}
