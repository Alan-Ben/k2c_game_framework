using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 科技树
    /// </summary>
    public class GGUIMonoMarsTechnologyTree : _AALBasicUIWndMono
    {
        [ALHeader("科技类型页签列表")]
        public List<GGUIMonoMarsTechnologyTypeTab> tabMonoList;
        [ALHeader("默认选中的科技类型")]
        public EMarsTechnologyType defaultSelectType = EMarsTechnologyType.NONE;
        
        [ALHeader("加成总览按钮")]
        public GameObject addOverviewBtn;
        
        [ALHeader("科技树层级Grid")]
        public GGUIMonoMarsTechnologyTreeLayerGrid monoTechnologyTreeLayerGrid;
        
        [ALHeader("升级中科技展示")]
        public GGUIMonoMarsUpgradingTechnologyInfo monoUpgradingTechnology;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(7305); } }
        public static string objName { get { return UIResPathAssistant.getObjName(7305);} }
    }
}