using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 背包物品详情面板
    /// </summary>
    public class BagItemDetailBar : _ABasicBagItemBar
    {
        /// <summary>
        /// 容器对象
        /// </summary>
        private GGUIWndBagItemGrid _m_igItemGrid;
        /// <summary>
        /// 出售物品的窗口对象
        /// </summary>
        private GGUIWndBagPopItemDetailSimple _m_siwItemDetailWnd;

        public BagItemDetailBar(GGUIWndBagItemGrid _grid)
        {
            _m_igItemGrid = _grid;
            _m_siwItemDetailWnd = new GGUIWndBagPopItemDetailSimple(_grid.wnd.gridAreaUIObj);
        }
        /// <summary>
        /// 是否在本行结束后才插入
        /// </summary>
        public override bool isAfterLineBar { get { return true; } }

        public override int barHeight 
        {
            get {
                return null == _m_siwItemDetailWnd.wnd ? 0 : (int)_m_siwItemDetailWnd.rectTransform.rect.height;
            }
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public override void init(Action _doneDelegate)
        {
            if(null == _m_siwItemDetailWnd)
                return;

            //开启加载
            _m_siwItemDetailWnd.load(
                () =>
                {
                    if(null != _doneDelegate)
                        _doneDelegate();

                //刷新显示
                _refreshItem(_m_biBagItem);
                });
        }

        /// <summary>
        /// 设置物品信息
        /// </summary>
        /// <param name="_item"></param>
        protected override void _refreshItem(BagItem _item)
        {
            if(null == _item || null == _m_siwItemDetailWnd)
                return;

            _m_siwItemDetailWnd.init(_item);
        }

        /// <summary>
        /// 隐藏bar处理
        /// </summary>
        public override void hide()
        {
            if(null != _m_siwItemDetailWnd)
                _m_siwItemDetailWnd.hideWnd();
        }

        public override void setPos(float _x, float _y)
        {
            if(null != _m_siwItemDetailWnd)
            {
                ALUGUICommon.setUIPos(_m_siwItemDetailWnd.getGameObj(), _x, _y);
            }
        }

        public override void show()
        {
            if(null != _m_siwItemDetailWnd)
                _m_siwItemDetailWnd.showWnd();
        }

        protected override void _discard()
        {
            if(null != _m_siwItemDetailWnd)
                _m_siwItemDetailWnd.forceDiscard();
            _m_siwItemDetailWnd = null;
        }

        protected override void _reset()
        {
            if(null != _m_siwItemDetailWnd)
                _m_siwItemDetailWnd.resetWnd();
        }

        public override RectTransform getWndRectTransform()
        {
            if (null == _m_siwItemDetailWnd)
                return null;

            return _m_siwItemDetailWnd.rectTransform;

        }
    }
}
