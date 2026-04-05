using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 已解锁妃子上部分的主要信息窗口
    /// </summary>
    public class GGUIMonoConsortMainShowCaseWnd : _AALBasicUIWndMono
    {
        [ALHeader("妃子形象子窗口")]
        public GGUIMonoConsortShowCaseSubWnd monoShowCaseSubWnd;
        
        [ALHeader("妃子展示特效父节点")]
        public Transform consortSfxParent;
        [ALHeader("送亲密度礼物特效ID")]
        public long sendIntimacyGiftSfxId;
        [ALHeader("送好感度礼物特效ID")]
        public long sendCharmGiftSfxId;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1406); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1406);} }
    }
}