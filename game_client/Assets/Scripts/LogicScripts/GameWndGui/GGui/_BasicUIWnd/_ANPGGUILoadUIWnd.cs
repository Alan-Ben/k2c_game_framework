using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;


namespace GOE
{
    //活动窗口
    public abstract class _ANPGGUILoadUIWnd<T> : _ATALBasicLoadUIWnd<T> where T : _AALBasicUIWndMono
    {
        protected _ANPGGUILoadUIWnd()
            : base()
        {
        }
#if AL_PUERTS
        /// <summary>
        /// 增加对应puerts的处理
        /// </summary>
        protected override ALPuertsManager _puertsMgr
        {
            get { return NPPuertsMgr.instance; }
        }
#endif
        
        #region 模糊背景的窗口控制处理
        /// <summary>
        /// 隐藏UI，一般使用scale缩放，不影响本身窗口的show，hide逻辑
        /// </summary>
        public void hideUI()
        {
            if (null == wnd)
                return;

            ALUGUICommon.setUIObjScale(wnd, 0f);
        }

        /// <summary>
        /// 显示UI，一般使用scale缩放，不影响本身窗口的show，hide逻辑
        /// </summary>
        public void showUI()
        {
            if (null == wnd)
                return;

            ALUGUICommon.setUIObjScale(wnd, 1f);
        }
        #endregion
    }
}
