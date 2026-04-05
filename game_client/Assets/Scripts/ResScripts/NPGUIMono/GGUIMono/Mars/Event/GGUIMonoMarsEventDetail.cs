﻿using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 事件详情窗口
    /// </summary>
    public class GGUIMonoMarsEventDetail : _AALBasicUIWndMono
    {
        [ALHeader("事件名称")]
        public TextEx txtName;
        
        [ALHeader("事件banner")]
        public RawImage eventBanner;
     
        [ALHeader("事件剩余时间")]
        public TextEx txtEventLeftTime;
     
        [ALHeader("是持续事件时显示物体列表")]
        public List<GameObject> isDurationEventShowList;
        [ALHeader("是持续事件时隐藏物体列表")]
        public List<GameObject> isDurationEventHideList;
        
        [ALHeader("事件描述")]
        public TextEx txtEventDesc;
        
        [ALHeader("事件处理方式列表")]
        public GGUIMonoAccessWayContainer accessWayItemContainer;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7209); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7209); } }
    }
}