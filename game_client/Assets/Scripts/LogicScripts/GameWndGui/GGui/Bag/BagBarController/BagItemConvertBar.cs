using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 背包物品兑换面板
    /// </summary>
    public class BagItemConvertBar : _ABasicBagItemBar
    {
        /// <summary>
        /// 容器对象
        /// </summary>
        private GGUIWndBagItemGrid _m_igItemGrid;
        /// <summary>
        /// 出售物品的窗口对象
        /// </summary>
        private GGUIWndBagPopItemConvertSimple _m_siwConvertItemWnd;

        public BagItemConvertBar(GGUIWndBagItemGrid _grid)
        {
            _m_igItemGrid = _grid;
            _m_siwConvertItemWnd = new GGUIWndBagPopItemConvertSimple(_grid.wnd.gridAreaUIObj);
        }
        /// <summary>
        /// 是否在本行结束后才插入
        /// </summary>
        public override bool isAfterLineBar { get { return true; } }

        public override int barHeight
        { 
            get 
            {
                return null == _m_siwConvertItemWnd.wnd ? 0 : (int)_m_siwConvertItemWnd.rectTransform.rect.height;
            } 
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public override void init(Action _doneDelegate)
        {
            if(null == _m_siwConvertItemWnd)
                return;

            //开启加载
            _m_siwConvertItemWnd.load(
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
            if(null == _item || null == _m_siwConvertItemWnd)
                return;

            _m_siwConvertItemWnd.init(_item);
            _m_siwConvertItemWnd.showWnd();
        }

        /// <summary>
        /// 隐藏bar处理
        /// </summary>
        public override void hide()
        {
            if(null != _m_siwConvertItemWnd)
                _m_siwConvertItemWnd.hideWnd();
        }

        public override void setPos(float _x, float _y)
        {
            if(null != _m_siwConvertItemWnd)
            {
                ALUGUICommon.setUIPos(_m_siwConvertItemWnd.getGameObj(), _x, _y);
            }
        }

        public override void show()
        {
            if(null != _m_siwConvertItemWnd)
                _m_siwConvertItemWnd.showWnd();
        }

        protected override void _discard()
        {
            if(null != _m_siwConvertItemWnd)
                _m_siwConvertItemWnd.forceDiscard();
            _m_siwConvertItemWnd = null;
        }

        protected override void _reset()
        {
            if(null != _m_siwConvertItemWnd)
                _m_siwConvertItemWnd.resetWnd();
        }

        public override RectTransform getWndRectTransform()
        {
            if (null == _m_siwConvertItemWnd)
                return null;

            return _m_siwConvertItemWnd.rectTransform;

        }
    }
}
