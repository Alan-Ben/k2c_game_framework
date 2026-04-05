using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 背包物品类型bar面板
    /// </summary>
    public class BagItemTypeBarController : _AALUGUIGridBarController
    {
        private readonly long _m_uiPathId;
        private readonly Action<BagItemTypeBarController> _m_barStatChg;
        private bool _m_isShowItem = true;
        private GGUIWndBagItemTypeBar _m_barWnd;//bar窗口
        private ENPBagItemType _m_type;

        public BagItemTypeBarController(long _uiPathId, Transform _parent, Action<BagItemTypeBarController> _action)
        {
            _m_uiPathId = _uiPathId;
            _m_barStatChg = _action;
            _m_barWnd = new GGUIWndBagItemTypeBar(_uiPathId,_parent,_onBarStatChg);
            _m_barWnd.load();
            _m_barWnd.regLoadDoneDelegate(() =>
            {
                _m_barWnd.showWnd();
                _m_barWnd.setTabSelected(_m_isShowItem);
            });
        }
    
        public bool isShowItem { get { return _m_isShowItem; } }
        public ENPBagItemType type { get { return _m_type; } }
        public override int barHeight { get { return (null != _m_barWnd && null != _m_barWnd.rectTransform) ? (int)_m_barWnd.rectTransform.rect.height : 0; } }
        
        /// <summary>
        /// 是否在本行结束后才插入
        /// </summary>
        public override bool isAfterLineBar
        {
            get { return false; }
        }

        public override void show()
        {            
            if (null != _m_barWnd)
            {
                _m_barWnd.showWnd();
            }
        }

        public override void hide()
        {
            if (null != _m_barWnd)
            {
                _m_barWnd.hideWnd();
            }
        }

        public override void setPos(float _x, float _y)
        {
            if (null != _m_barWnd)
            {
                ALUGUICommon.setUIPos(_m_barWnd.getGameObj(), _x, _y);
            }
        }

        protected override void _reset()
        {
            _m_isShowItem = true;
            
            if (null != _m_barWnd)
            {
                _m_barWnd.setTabSelected(_m_isShowItem);
            }
        }

        protected override void _discard()
        {
            if (null != _m_barWnd)
            {
                _m_barWnd.discard();
                _m_barWnd = null;
            }
        }
        /// <summary>
        /// 刷新显示数据
        /// </summary>
        /// <param name="_info"></param>
        public void setBarData(ENPBagItemType _type, string _showTypeStr)
        {
            _m_type = _type;
            if (null != _m_barWnd)
            {
                _m_barWnd.setInfo(_showTypeStr);
            }
        }

        /// <summary>
        /// bar状态变化的时候
        /// </summary>
        private void _onBarStatChg()
        {
            _m_isShowItem = !_m_isShowItem;

            if (null != _m_barWnd)
            {
                _m_barWnd.setTabSelected(_m_isShowItem);
            }
            
            if (null != _m_barStatChg)
            {
                _m_barStatChg(this);
            }
        }

        protected override int _compareWhenEqualIdx(_AALUGUIGridBarController _other)
        {
            if(_other is BagItemTypeBarController)
                return base._compareWhenEqualIdx(_other);
            
            return 1;//类型bar会在后面显示
        }
    }
}
