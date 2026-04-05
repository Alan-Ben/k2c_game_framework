using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子皮肤窗口
    /// </summary>
    public class GGUIMonoConsortSkinMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("皮肤列表")]
        public GGUIMonoConsortSkinItemContainer monoSkinContainer;

        [ALHeader("妃子名")]
        public TextEx txtConsortName;
        
        [ALHeader("选中的皮肤名")]
        public List<TextEx> txtSelectSkinName;
        [ALHeader("选中的皮肤描述")]
        public TextEx txtSelectSkinDesc;
        
        [ALHeader("选中皮肤形象子窗口")]
        public GGUIMonoCommonShowCase monoSelectSkinShowCaseWnd;
        
        [ALHeader("选中皮肤解锁状态配置")] 
        public List<NPCommonEnumStatInfo<EConsortSkinStateType>> selectSKinStateInfos;

        [ALHeader("解锁消耗道具")]
        public NPGGUIMonoCommonItem unlock_cost_item;
        [ALHeader("解锁按钮")]
        public GameObject btnUnlock;

        [ALHeader("穿戴按钮")]
        public GameObject btnWearing;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1410); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1410);} }
    }
}