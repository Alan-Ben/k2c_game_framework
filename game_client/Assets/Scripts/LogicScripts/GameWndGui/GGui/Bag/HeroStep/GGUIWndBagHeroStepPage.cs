using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;
using Common.ChildEnum;

namespace GOE
{
    // 背包骑士阶段页面
    public class GGUIWndBagHeroStepPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoBagHeroStepPage>
    {

        private long _m_resPathId;

        private GGUIWndBagHeroStepContainer _m_stepGridWnd;


        public GGUIWndBagHeroStepPage(long _resPathId, Transform _parent)
        : base(_parent)
        {
            _m_resPathId = _resPathId;
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_resPathId);  } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_resPathId);  } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.stepGridMono)
                _m_stepGridWnd = new GGUIWndBagHeroStepContainer(wnd.stepGridMono);

        }

        protected override void _onShowWnd()
        {
            if (null != _m_stepGridWnd)
                _m_stepGridWnd.showWnd();
        }

        protected override void _onHideWnd()
        {
            if (null != _m_stepGridWnd)
                _m_stepGridWnd.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_stepGridWnd)
                _m_stepGridWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_stepGridWnd)
                _m_stepGridWnd.discard();
            _m_stepGridWnd = null;
        }
    }
}

