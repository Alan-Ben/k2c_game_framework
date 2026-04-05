using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子cg页签配置
    /// </summary>
    [Serializable]
    public class ConsortCgTabSetting
    {
        [ALHeader("cg类型")]
        public EConsortCGType cgType;

        [ALHeader("tabMono")]
        public NPGGUIMonoCommonTab tabMono;
        
        [ALHeader("展示CG使用的Grid")]
        public GGUIMonoConsortCGGrid cgGrid;
        
        [ALHeader("页签名")]
        public string tabNameKey;
    }
    
    public class GGUIMonoConsortCGMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("选中的页签名")]
        public TextEx txtSelectTabName;

        [ALHeader("收集进度")]
        public TextEx txtCollectionProgress;
        [ALHeader("收集进度使用key")]
        public string collectionProgressKey;

        [ALHeader("不同的cg类型设置")]
        public List<ConsortCgTabSetting> cgTypeSetting;
        [ALHeader("默认选中的cg类型")]
        public EConsortCGType defaultCgType = EConsortCGType.INVITE;
        
        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1408); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1408);} }
    }
}