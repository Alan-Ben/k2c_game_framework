using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 简易漫画窗口
    /// </summary>
    public class GGUIWndSimpleComicSubWnd : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoSimpleComicSubWnd>
    {
        //当前漫画配置
        private SimpleComicRefObj _m_simpleComicRefObj;
        //当前播放到第几篇
        private int _m_curPlayIndex = -1;
        //当前的字页签
        private GGUIWndSimpleComicPage _m_curPageWnd;
        
        //点击跳过回调
        private event Action _m_skipAction;
        //点击完成回调
        private event Action _m_doneAction;
        
        public GGUIWndSimpleComicSubWnd(Transform _parent) : base(_parent)
        {
            
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return GGUIMonoSimpleComicSubWnd.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSimpleComicSubWnd.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _showCurPage();
        }

        protected override void _onHideWnd()
        {
            if(null != _m_curPageWnd)
                _m_curPageWnd.hideWnd();

            _m_curPlayIndex = -1;
        }

        protected override void _onReset()
        {
            if(null != _m_curPageWnd)
                _m_curPageWnd.resetWnd();
            
            _m_curPlayIndex = -1;
            _m_doneAction = null;
            _m_skipAction = null;
        }

        protected override void _onDiscard()
        {
            if(null != _m_curPageWnd)
                _m_curPageWnd.discard();
            _m_curPageWnd = null;
            
            _m_curPlayIndex = -1;
            _m_doneAction = null;
            _m_skipAction = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNextBtn);
            ALUGUICommon.combineBtnClick(wnd.btnLast, _onClickLastBtn);
            ALUGUICommon.combineBtnClick(wnd.btnDone, _onClickDoneBtn);
        }

        //初始化信息
        public void initShowInfo(long _comicId)
        {
            _m_simpleComicRefObj = GRefdataCoreMgr.instance.simpleCommicRefCore.getRef(_comicId);
            initShowInfo(_m_simpleComicRefObj);
        }

        public void initShowInfo(SimpleComicRefObj _comicRefObj)
        {
            _m_simpleComicRefObj = _comicRefObj;
            _m_curPlayIndex = 0;

            _showCurPage();
        }
        
        public void regSkipAndDoneAction(Action _skipAction, Action _doneAction)
        {
            _m_skipAction = _skipAction;
            _m_doneAction = _doneAction;
        }

        //显示当前index页面
        private void _showCurPage()
        {
            if(null == wnd)
                return;
            
            if(null == _m_simpleComicRefObj || _m_curPlayIndex < 0)
                return;
            
            if(null != _m_curPageWnd)
                _m_curPageWnd.discard();
            
            _m_curPageWnd = new GGUIWndSimpleComicPage(_m_simpleComicRefObj.getResIdByIndex(_m_curPlayIndex), wnd.transformParent);
            _m_curPageWnd.onPlayEnd += _setToNextPage;
            _m_curPageWnd.load(() =>
            {
                _m_curPageWnd.showWnd();
                _m_curPageWnd.startPlay();
            });
            
            //还在播放
            ALUGUICommon.setGameObjEnable(wnd.doneShowGoList, false);
            ALUGUICommon.setGameObjEnable(wnd.doneHideGoList, true);
        }

        //设置到下一页
        private void _setToNextPage()
        {
            if(null == wnd || null == _m_simpleComicRefObj || null == _m_simpleComicRefObj.page_res_id_list)
                return;
            
            _m_curPlayIndex++;
            
            if (_m_curPlayIndex < _m_simpleComicRefObj.page_res_id_list.Count)
            {
                _showCurPage();
            }
            else
            {
                //到最后一页了展示相关ui
                ALUGUICommon.setGameObjEnable(wnd.doneShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.doneHideGoList, false);
            }
        }
        
        //点击下一页按钮
        private void _onClickNextBtn(GameObject _gameObject)
        {
            if(null == wnd)
                return;
            
            _setToNextPage();
        }

        //点击跳过按钮
        private void _onClickLastBtn(GameObject _gameObject)
        {
            if (_m_skipAction != null) 
                _m_skipAction();

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SIMPLE_COMIC);
        }
        
        //点击完成按钮
        private void _onClickDoneBtn(GameObject _gameObject)
        {
            if (_m_doneAction != null) 
                _m_doneAction();
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SIMPLE_COMIC);
        }
    }
}
