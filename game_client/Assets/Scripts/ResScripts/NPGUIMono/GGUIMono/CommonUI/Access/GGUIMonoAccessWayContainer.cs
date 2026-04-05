﻿using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 获取途径列表容器
    /// </summary>
    public class GGUIMonoAccessWayContainer : _AALBasicUIWndMono
    {
        [ALHeader("没有获取途径时需要显示的GO列表")] 
        public List<GameObject> noAccessWayShowList;
        
        [ALHeader("item 的父对象")]
        public Transform goItemParent;
        
        [ALHeader("获取途径item模板")]
        public NPGGUIMonoAccessWayItem monoAccessWay;
        
        [ALHeader("使用物品item模板")]
        public NPGGUIMonoAccessBagItem monoBagItem;
        
        [ALHeader("合成item模板")]
        public NPGGUIMonoAccessCombinedItem monoCombinedItem;
        
        [ALInfo("以下为自适应设置")]
        [ALHeader("需要修改高度的RectTransform")]
        public RectTransform needChgHeightRectTransform;
        
        [ALHeader("列表layout")]
        public VerticalLayoutGroup listLayoutGroup;
        
        [ALHeader("没有item时容器默认高度")]
        public float noItemDefaultHeight;
        
        [ALHeader("容器最大高度")]
        public float maxHeight;
    }
}