using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 解锁妃子详情页面tab选中模式
    /// </summary>
    public enum EUnLockConsortDetailWndTabSelectMode
    {
        [InspectorName("取消选中之前的所有Tab, 这些tab对应的页签窗口会被隐藏")]
        UNCHECK_PRE_ALL_TAB,
        [InspectorName("保持选中之前的Tab, 但是这些tab对应的页签窗口会被隐藏")]
        KEEP_TAB_CHECK_TAB_PAGE_HIDE,
        [InspectorName("保持选中之前的Tab, 并且这些tab对应的页签窗口也保持显示")]
        KEEP_TAB_CHECK_TAB_PAGE_SHOW,
    }
    
    /// <summary>
    /// 解锁妃子详情页面tab类型
    /// </summary>
    public enum EUnLockConsortDetailWndTabType
    {
        NONE,
        [InspectorName("简介")]
        PROFILE,
        [InspectorName("星辉")]
        HALO,
        [InspectorName("羁绊")]
        FETTER,
        [InspectorName("经营")]
        BUSINESS,
        [InspectorName("加护")]
        BLESS,
        [InspectorName("互动")]
        INTERACTION,
    }
    
    /// <summary>
    /// 已解锁妃子详情页面tab配置
    /// </summary>
    [Serializable]
    public class GGUIMonoUnLockConsortDetailWndTabSetting
    {
        [ALHeader("页签类型")]
        public EUnLockConsortDetailWndTabType tabType;

        [ALHeader("tab脚本")]
        public NPGGUIMonoCommonTab tabMono;
        
        [ALHeader("加载ui_path_id")]
        public long resPathId;

        [ALHeader("加载的父节点")]
        public Transform goParent;
        
        [ALHeader("页签选中模式")]
        public EUnLockConsortDetailWndTabSelectMode selectMode;
        
        [ALHeader("选中动画")]
        public Animation selectAnimation;
        [ALHeader("选中动画名称")]
        public string selectAniName;
        [ALHeader("隐藏取消选中动画名称")]
        public string disSelectAniName;
    }
    
    /// <summary>
    /// 已有妃子的下部选择Tab列表
    /// </summary>
    public class GGUIMonoUnLockConsortMainDetailWnd : _AALBasicUIWndMono
    {
        [ALHeader("是否只展示形象toggle")]
        public NPGGUIMonoCommonToggleEx onlyShowActorToggle;

        [ALHeader("妃子配音气泡附加窗口")]
        public GGUIMonoConsortVoiceBubble monoConsortVoiceBubble;
        
        [ALHeader("tab配置列表)")]
        public List<GGUIMonoUnLockConsortDetailWndTabSetting> tabSettingList;

        [ALHeader("妃子简介按钮")]
        public GameObject btnConsortProfile;
        
        //上下切换按钮放在这里的原因是，子页签切换的时候，上下按钮需要跟随上下移动。因此上下按钮需要在这里作为容器一部分统一管理
        [ALHeader("上一妃子按钮")]
        public GameObject btnPre;
        [ALHeader("下一妃子按钮")]
        public GameObject btnNext;

        [ALHeader("少于等于一个妃子时隐藏物体")]
        public List<GameObject> lessThanOrEqualOneConsortHideGoList;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1407); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1407);} }
    }
}