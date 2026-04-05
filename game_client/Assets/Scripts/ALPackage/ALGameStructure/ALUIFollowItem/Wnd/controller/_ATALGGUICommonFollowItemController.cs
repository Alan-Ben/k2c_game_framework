using System;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 控制对象的模板化类，方便使用者直接使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ATALGGUICommonFollowItemController<T, W> : _AALGGUICommonFollowItemController
        where T : ALGGUIMonoCommonFollowItem
        where W : _ATALGGUIWndCommonFollowItem<T>
    {
        //创建的窗口对象
        private W _m_wnd;

        public _ATALGGUICommonFollowItemController()
            : base()
        {
            _m_wnd = null;
        }

        public W wnd { get { return _m_wnd; } }

        /// <summary>
        /// 根据框架创建的脚本对象构建对应的实际窗口对象并返回
        /// </summary>
        /// <param name="_mono"></param>
        /// <returns></returns>
        protected override _IALGGUIWndCommonFollowItem _createItem(ALGGUIMonoCommonFollowItem _mono)
        {
            //判断带入mono类型是否匹配
            if(!(_mono is T))
            {
                //报错返回
                ALLog.Error($"mono is not collect class: {_mono._resIndex}");
                return null;
            }

            _m_wnd = _createItemWnd((T)_mono);

            return _m_wnd;
        }

        /// <summary>
        /// 释放创建出来的对象
        /// </summary>
        /// <param name="_item"></param>
        protected override void _discardItem(_IALGGUIWndCommonFollowItem _item)
        {
            if(null != _item)
            {
                ((W)_item).discard();
            }
        }

        /// <summary>
        /// 子类释放资源处理
        /// </summary>
        protected override void _onDiscard()
        {
            //由于本对象窗口控制权交由基类控制，这边不做处理
            _m_wnd = null;
        }

        /// <summary>
        /// 根据带入的窗口脚本，创建并返回窗口子类对象
        /// </summary>
        /// <param name="_wndMono"></param>
        /// <returns></returns>
        protected abstract W _createItemWnd(T _wndMono);
    }
}