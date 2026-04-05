using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 未解锁妃子详情页面tab类型
    /// </summary>
    public enum ELockConsortDetailWndTabType
    {
        NONE,
        [InspectorName("简介 PROFILE")]
        PROFILE,
        [InspectorName("加护 BLESS")]
        BLESS,
    }

    /// <summary>
    /// 未解锁妃子详情页面tab配置
    /// </summary>
    [Serializable]
    public class GGUIMonoLockConsortDetailWndTabSetting
    {
        [ALHeader("页签类型")]
        public ELockConsortDetailWndTabType tabType;

        [ALHeader("tab脚本")]
        public NPGGUIMonoCommonTab tabMono;
        
        [ALHeader("页面加载路径")]
        public NPCommonAssetPathInfo pagePathInfo;
    }
    
    /// <summary>
    /// 未解锁妃子详细信息
    /// </summary>
    public class GGUIMonoLockConsortDetail : _ANPBasicUIWndResBarMono
    {
        [ALHeader("详细信息子窗口")]
        public GGUISubMonoConsortDetailInfo detailInfoSubWnd;

        [ALHeader("羁绊详情按钮")]
        public GameObject btnFetterDetail;
        [ALHeader("羁绊详情弹窗ui路径id")]
        public long fetterDetailToolTipUIResId;

        [ALHeader("妃子产出来源按钮")]
        public GameObject btnConsortSource;

        [ALHeader("tab配置列表)")]
        public List<GGUIMonoLockConsortDetailWndTabSetting> tabSettingList;
        [ALHeader("默认显示tab类型")]
        public ELockConsortDetailWndTabType defaultTabType;
        
        [ALHeader("tab显示页面的父节点")]
        public Transform tabPageParent;

        [ALHeader("上一妃子按钮")]
        public GameObject btnPre;
        [ALHeader("下一妃子按钮")]
        public GameObject btnNext;
        
        [ALHeader("少于等于一个妃子时隐藏物体")]
        public List<GameObject> lessThanOrEqualOneConsortHideGoList;
        
        [ALHeader("妃子语音气泡")]
        public GGUIMonoConsortVoiceBubble monoVoiceBubble;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1403); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1403);} }
    }
}