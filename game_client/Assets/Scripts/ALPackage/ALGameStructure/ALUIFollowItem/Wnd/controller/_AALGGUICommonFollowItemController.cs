using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 跟随UI的子对象的控制器
    /// 因为创建的资源脚本是使用基类
    /// 需要在这个控制器中根据实际的情况创建不同的子类对象，并加以控制
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AALGGUICommonFollowItemController
    {
        //是否已经初始化，本控制对象无法进行重复初始化处理
        private bool _m_bIsInited;

        //本控制对象所依托的实例对象Id
        private long _m_lFollowInstanceSerialize;
        //本控制对象应用于的窗口对象
        private _IALGGUICommonFollowItemControllerContainer _m_wControllerContainer;
        //窗口返回的窗口脚本资源对象，用于缓存和释放处理
        private ALGGUIMonoCommonFollowItem _m_iItemMono;

        //控制的实际显示对象
        private _IALGGUIWndCommonFollowItem _m_iControlItem;
        
        //资源是否加载完成
        private bool _m_bItemWndIsLoadDone;
        //资源是否加载完成回调
        private Action _m_dItemWndIsLoadDoneDelegate;
        
        public _AALGGUICommonFollowItemController()
        {
            _m_bIsInited = false;
            _m_bItemWndIsLoadDone = false;

            _m_dItemWndIsLoadDoneDelegate = null;
            
            _m_lFollowInstanceSerialize = 0;
            _m_wControllerContainer = null;
            _m_iItemMono = null;

            _m_iControlItem = null;
        }

        public bool isInited { get { return _m_bIsInited; } }
        public bool ItemWndIsLoadDone { get { return _m_bItemWndIsLoadDone; } }

        public ALGGUIMonoCommonFollowItem itemMono { get { return _m_iItemMono; } }
        public _IALGGUIWndCommonFollowItem controlItem { get { return _m_iControlItem; } }

        protected internal long _followInstanceSerialize { get { return _m_lFollowInstanceSerialize; } }

        /// <summary>
        /// 资源释放操作
        /// </summary>
        public void discard()
        {
            //子类释放资源
            _onDiscard();

            //窗口对象会在_release的时候调用pushback释放
            //将资源从控制窗口释放
            if (null != _m_wControllerContainer)
                _m_wControllerContainer._releaseItem(this);
            _m_wControllerContainer = null;

            //设置未初始化完成
            _m_bIsInited = false;
            _m_bItemWndIsLoadDone = false;
            _m_dItemWndIsLoadDoneDelegate = null;
        }

        /// <summary>
        /// 初始化本对象的控制信息
        /// 记录容器对象为了释放的时候能注销相关处理
        /// </summary>
        /// <param name="_rootWnd"></param>
        protected internal bool _initItem(_IALGGUICommonFollowItemControllerContainer _rootWnd, long _followInstanceSerialize)
        {
            //数据无效直接不做处理
            if (null == _rootWnd)
                return false;

            if (_m_bIsInited)
            {
                ALLog.Error($"Init a follow item controller multi:{followItemIndex}");
                return false;
            }

            //设置初始化完成
            _m_bIsInited = true;

            //设置容器对象
            _m_wControllerContainer = _rootWnd;
            _m_lFollowInstanceSerialize = _followInstanceSerialize;

            //调用资源创建处理
            _m_wControllerContainer._popItem(this);

            return true;
        }

        /// <summary>
        /// 刷新所有Item的位置信息
        /// </summary>
        protected internal void _refreshItemPos(Vector2 _uiCenterPos)
        {
            if (null == _m_iControlItem)
                return;

            //判断是否初始化完成，如果未初始化需要报错
            if (!_m_bIsInited)
            {
                ALLog.Error($"refresh pos follow item {followItemIndex} for instance: {_m_lFollowInstanceSerialize} is not inited!");
                return;
            }

            _m_iControlItem.setUIPos(_uiCenterPos);
        }

        /// <summary>
        /// 初始化Controller中的Item
        /// 在RootWnd中在添加的时候调用，资源对象由父窗口创建
        /// 
        /// 返回是否创建成功，如果失败外围需要释放资源
        /// </summary>
        /// <param name="_mono"></param>
        protected internal bool _onMonoCreate(ALGGUIMonoCommonFollowItem _mono)
        {
            //数据无效直接不做处理
            if(null == _mono)
                return false;

            if(!_m_bIsInited)
            {
                ALLog.Error($"Init a follow item when it is no inited :{followItemIndex}");
                return false;
            }

            //如果脚本已经存在同样返回失败
            if(null != _m_iItemMono)
            {
                //返回失败，保证资源不会泄露或者重复设置
                return false;
            }

            //设置资源对象
            _m_iItemMono = _mono;
            //尝试创建窗口对象
            _m_iControlItem = _createItem(_mono);

            //控制窗口创建完成
            _m_bItemWndIsLoadDone = true;
            
            //执行创建完回调
            if (null != _m_dItemWndIsLoadDoneDelegate)
                _m_dItemWndIsLoadDoneDelegate();
            _m_dItemWndIsLoadDoneDelegate = null;
            
            return true;
        }

        /// <summary>
        /// 在Mono对象被放回cache的时候触发的处理
        /// </summary>
        protected internal void _onMonoPushback(ALGGUIMonoCommonFollowItem _mono)
        {
            //先比对数据对象是否一致，只有一致才会重置
            if (_mono != _m_iItemMono)
                return;

            //尝试释放窗口对象
            if (null != _m_iControlItem)
                _discardItem(_m_iControlItem);
            _m_iControlItem = null;
            _m_iItemMono = null;
        }
        
        /// <summary>
        /// 注册Item窗口创建完成的回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regItemWndLoadDoneDelegate(Action _delegate)
        {
            if(null == _delegate)
                return;
            
            //已经完成直接处理
            if(_m_bItemWndIsLoadDone)
            {
                _delegate();
                return;
            }

            //注册回调
            if (null == _m_dItemWndIsLoadDoneDelegate)
                _m_dItemWndIsLoadDoneDelegate = _delegate;
            else
                _m_dItemWndIsLoadDoneDelegate += _delegate;
        }

        /// <summary>
        /// 在本控制对象的容器窗口显示的时候
        /// 将会调用的事件函数，默认不处理任何内容
        /// 如果子类有需要，可以在本控制对象重载本函数处理
        /// </summary>
        public virtual void onFollowRootShow() { }
        /// <summary>
        /// 在本控制对象的容器窗口隐藏的时候
        /// 将会调用的事件函数，默认不处理任何内容
        /// 如果子类有需要，可以在本控制对象重载本函数处理
        /// </summary>
        public virtual void onFollowRootHide() { }

        /// <summary>
        /// 资源数据对象
        /// </summary>
        public abstract _AALBasicLoadResIndexInfo followItemIndex { get; }

        /// <summary>
        /// 根据框架创建的脚本对象构建对应的实际窗口对象并返回
        /// </summary>
        /// <param name="_mono"></param>
        /// <returns></returns>
        protected abstract _IALGGUIWndCommonFollowItem _createItem(ALGGUIMonoCommonFollowItem _mono);
        /// <summary>
        /// 释放创建出来的对象
        /// </summary>
        /// <param name="_item"></param>
        protected abstract void _discardItem(_IALGGUIWndCommonFollowItem _item);

        /// <summary>
        /// 子类释放资源处理
        /// </summary>
        protected abstract void _onDiscard();
    }
}