
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// showcase通用子窗口
    /// </summary>
    public class NPGGUIWndCommonShowCase : _ANPGGUIWndCommonShowCase<GGUIMonoCommonShowCase>
    {
        //点击item事件
        public event Action<ShowcaseInfo> onClickItem;

        public NPGGUIWndCommonShowCase(GGUIMonoCommonShowCase _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            
        }

        protected override void _onResetEx()
        {
            
        }

        protected override void _onDiscardEx()
        {
        }

        protected override void _onWndInitDoneEx()
        {

        }

        /// <summary>
        /// 点击反馈
        /// </summary>
        /// <param name="_gameObject"></param>
        protected override void _onClickShowcaseEx(GameObject _gameObject)
        {
            if (null == _m_showcaseInfo)
                return;

            onClickItem?.Invoke(_m_showcaseInfo);
        }
    }
}
