using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 背包物品使用面板
    /// </summary>
    public class BagItemUseBar : _ABasicBagItemBar
    {
        /// <summary>
        /// 容器对象
        /// </summary>
        private GGUIWndBagItemGrid _m_igItemGrid;
        /// <summary>
        /// 出售物品的窗口对象
        /// </summary>
        private GGUIWndBagPopItemUseSimple _m_siwUseItemWnd;

        public BagItemUseBar(GGUIWndBagItemGrid _grid)
        {
            _m_igItemGrid = _grid;
            _m_siwUseItemWnd = new GGUIWndBagPopItemUseSimple(_grid.wnd.gridAreaUIObj);
        }
        /// <summary>
        /// 是否在本行结束后才插入
        /// </summary>
        public override bool isAfterLineBar { get { return true; } }

        public override int barHeight
        { 
            get 
            {
                return null == _m_siwUseItemWnd.wnd ? 0 : (int)_m_siwUseItemWnd.rectTransform.rect.height;
            } 
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public override void init(Action _doneDelegate)
        {
            if(null == _m_siwUseItemWnd)
                return;

            //开启加载
            _m_siwUseItemWnd.load(
                () =>
                {
                    //刷新显示
                    _refreshItem(_m_biBagItem);
                    if (null != _doneDelegate)
                        _doneDelegate();
                });
        }

        /// <summary>
        /// 设置物品信息
        /// </summary>
        /// <param name="_item"></param>
        protected override void _refreshItem(BagItem _item)
        {
            if(null == _item || null == _m_siwUseItemWnd)
                return;

            _m_siwUseItemWnd.init(_item);
            _m_siwUseItemWnd.showWnd();
        }

        /// <summary>
        /// 隐藏bar处理
        /// </summary>
        public override void hide()
        {
            if(null != _m_siwUseItemWnd)
                _m_siwUseItemWnd.hideWnd();
        }

        public override void setPos(float _x, float _y)
        {
            if(null != _m_siwUseItemWnd)
            {
                ALUGUICommon.setUIPos(_m_siwUseItemWnd.getGameObj(), _x, _y);
            }
        }

        public override void show()
        {
            if(null != _m_siwUseItemWnd)
                _m_siwUseItemWnd.showWnd();
        }

        protected override void _discard()
        {
            if(null != _m_siwUseItemWnd)
                _m_siwUseItemWnd.forceDiscard();
            _m_siwUseItemWnd = null;
        }

        protected override void _reset()
        {
            if(null != _m_siwUseItemWnd)
                _m_siwUseItemWnd.resetWnd();
        }
        public void refreshUseBtnState()
        {
            _m_siwUseItemWnd.refreshUseBtnState();
        }

        public override RectTransform getWndRectTransform()
        {
            if (null == _m_siwUseItemWnd)
                return null;

            return _m_siwUseItemWnd.rectTransform;

        }

        public RectTransform getUseBtnRectTransform()
        {
            return _m_siwUseItemWnd?.getUseBtnRectTransform();
        }
    }
}
