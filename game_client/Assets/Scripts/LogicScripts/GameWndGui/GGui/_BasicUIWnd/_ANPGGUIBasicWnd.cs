using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;
using System;


namespace GOE
{
    //活动窗口
    public abstract class _ANPGGUIBasicWnd<T> : _ATALBasicUIWnd<T>, _IGGUIScreenBlurCurUI where T : _AALBasicUIWndMono
    {
        protected _ANPGGUIBasicWnd(EALUIWndLayer _layer)
            : base(_layer)
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
        /// alzq 后续使用canvasGroup进行隐藏处理，这样可以避免ScrollRect因为缩放导致惯性残留，引发窗口内容位移的问题
        /// </summary>
        private CanvasGroup _m_cgCGMono;
        //表示判断是否是自己创建的comp，是的话需要setEnabled
        private bool _m_bNeedRemoveCG = false;

        public virtual void hideUI()
        {
            if (null == wnd)
                return;

            _m_cgCGMono = wnd.GetComponent<CanvasGroup>();
            if(null == _m_cgCGMono)
            {
                _m_cgCGMono = wnd.AddMissingComponent<CanvasGroup>();
                _m_bNeedRemoveCG = true;
            }
            
            //还找不到不处理，应该不存在
            if(null == _m_cgCGMono)
                return;

            //表示判断是否是自己创建的comp，是的话需要setEnabled
            if(_m_bNeedRemoveCG)
                _m_cgCGMono.enabled = true;
            
            //控制不渲染
            _m_cgCGMono.alpha = 0f;
        }

        /// <summary>
        /// 显示UI，一般使用scale缩放，不影响本身窗口的show，hide逻辑
        /// alzq 后续使用canvasGroup进行隐藏处理，这样可以避免ScrollRect因为缩放导致惯性残留，引发窗口内容位移的问题
        /// </summary>
        public virtual void showUI()
        {
            if (null == wnd)
                return;

            if(null != _m_cgCGMono)
            {
                //表示判断是否是自己创建的comp，是的话需要setEnabled
                if(_m_bNeedRemoveCG)
                    _m_cgCGMono.enabled = false;
                
                //控制渲染
                _m_cgCGMono.alpha = 1f;
            }
        }
        #endregion
    }
}
