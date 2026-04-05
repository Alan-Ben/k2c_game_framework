using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 已解锁妃子上部分的主要信息窗口
    /// </summary>
    public class GGUIMonoUnLockConsortSubMainInfoWnd : _ANPBasicUIWndResBarMono
    {
        [ALHeader("妃子详细信息子窗口")]
        public GGUISubMonoUnlockConsortDetailInfo monoUnlockConsortDetailInfo;
        
        [ALHeader("仅显示形象动画")]
        public Animation onlyShowActorAnim;
        [ALHeader("仅显示形象功能开启时展示动画名")]
        public string onlyShowActorFuncOnAnimName;
        [ALHeader("仅显示形象功能关闭时展示动画名")]
        public string onlyShowActorFuncOffAnimName;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1405); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1405);} }
    }
}