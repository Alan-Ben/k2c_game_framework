
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 一个会自动修正位置的容器
    /// </summary>
    public abstract class _AGGUISubWndPosFixContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _AGGUISubWndCommonContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _AALBasicUIWndMono
        where _T_CONTAINER_MONO : _AGGUIMonoPosFixContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALBasicUISubWnd<_T_ITEM_MONO>
    {
        // 一个内部的简单状态机
        [NotNull] private readonly _TALSimpleStateMachine<EPosFixType> _m_stateMachine;
        // 滚动区域组件
        private ALUGUIMonoCommonScrollRect _m_scrollRect;
        private int _m_currentItemIndex;


        protected _AGGUISubWndPosFixContainer(_T_CONTAINER_MONO _containerMono) 
            : base(_containerMono)
        {
            _m_stateMachine = new _TALSimpleStateMachine<EPosFixType>();
            _m_stateMachine.changeState(new NoneState());
        }
        
        
        public event Action onItemIndexChanged;

        public int currentItemIndex
        {
            get { return _m_currentItemIndex; }
            private set
            {
                if (value == _m_currentItemIndex) 
                    return;
                
                _m_currentItemIndex = value; 
                onItemIndexChanged?.Invoke();
            }
        }
        public float fixPosDuration { get { if (wnd == null) return 0; return wnd.fixPosDuration; } }
        public EaseType easeType { get { if (wnd == null) return GGameCommonInfo.instance.obj.defaultValueEaseType; return wnd.easeType; } }
        public float dragEndVelocityThreshold { get { if (wnd == null) return 0; return wnd.dragEndVelocityThreshold; } }
        public int moveSerialize { get { return _m_stateMachine.curState.enterSerialize; } }
        

        protected override void _onShowWnd()
        {
            base._onShowWnd();

            // 在 show 的时候, 监听滚动事件
            if (_m_scrollRect != null)
                _m_scrollRect.onStartDragDelegate += _onStartDrag;
            
            // 同时先自动修正位置一次
            fixToItemIndex(-1, false);
        }
        protected override void _onHideWnd()
        {
            base._onHideWnd();

            // hide 的时候移除事件监听
            if (_m_scrollRect != null)
                _m_scrollRect.onStartDragDelegate -= _onStartDrag;
            
            // 状态机恢复的空状态
            _m_stateMachine.changeState(new NoneState());
        }
        protected override void _onWndInitDone()
        {
            if (wnd != null)
                _m_scrollRect = wnd.scrollRect as ALUGUIMonoCommonScrollRect;

            base._onWndInitDone();
        }
        protected override void _onRefreshWnd()
        {
            base._onRefreshWnd();
            
            // 刷新的时候，也自动修正一次位置
            currentItemIndex = 0;
            fixToItemIndex(-1, false);
        }


        public void fixToItemIndex(int _itemIndex, bool _needSmooth)
        {
            // 如果外部需要主动修正到某个 item 上，就切换到修正状态
            _m_stateMachine.changeState(new FixPosState(this), _itemIndex, _needSmooth);
        }
        
        
        private void _onStartDrag()
        {
            // 如果玩家开始拖动列表，就进入拖动处理状态
            _m_stateMachine.changeState(new DragState(this));
        }

        #region 状态机
        public enum EPosFixType
        {
            None,
            Drag,
            FixPos,
        }
        // 空状态，消除操作
        private class NoneState : _ASimpleState<EPosFixType>
        {
            public override EPosFixType state { get { return EPosFixType.None; } }
            protected override void _onEnter() { }
            protected override void _onExit() { }
            public override bool canEnterState(EPosFixType _newState) { return true; }
        }
        // 修正位置到指定 item 的状态
        private class FixPosState : _ASimpleState<EPosFixType, int, bool>, _IScrollRectMoveTask
        {
            [NotNull] private readonly _AGGUISubWndPosFixContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> _m_container;
            

            public FixPosState([NotNull] _AGGUISubWndPosFixContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> _container)
            {
                _m_container = _container;
            }

            public override EPosFixType state { get { return EPosFixType.FixPos; } }

            protected override void _onEnter(int _index, bool _needSmooth)
            {
                if (_index == -1)
                    _index = _m_container.currentItemIndex;
                else
                    _m_container.currentItemIndex = _index;

                float normalizedPos = _m_container.itemNum <= 1 ? 0 : _index / ((float)_m_container.itemNum - 1);
                new ScrollRectMoveTask(this, _m_container._m_scrollRect.vertical, normalizedPos, _needSmooth ? _m_container.fixPosDuration : 0f, _m_container.easeType).deal();
            }

            protected override void _onExit()
            {
            }
            public override bool canEnterState(EPosFixType _newState)
            {
                return true;
            }

            ScrollRect _IScrollRectMoveTask.scrollRect { get { return _m_container._m_scrollRect; } }
            int _IScrollRectMoveTask.serialize { get { return enterSerialize; } }
        }

        // 当玩家在拖动列表的处理
        private class DragState : _ASimpleState<EPosFixType>
        {
            [NotNull] private readonly _AGGUISubWndPosFixContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> _m_container;
            private bool _m_isDragEnd;
            
            public DragState([NotNull] _AGGUISubWndPosFixContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> _container)
            {
                _m_container = _container;
            }

            
            public override EPosFixType state { get { return EPosFixType.Drag; } }

            
            protected override void _onEnter()
            {
                // 注册拖动相关的事件
                _m_container._m_scrollRect.onEndDragDelegate += _onEndDragDelegate;
                _m_container._m_scrollRect.onValueChanged.AddListener(_onValueChanged);
                _onValueChanged(_m_container._m_scrollRect.normalizedPosition);
            }
            protected override void _onExit()
            {
                // 事件结束后取消掉
                _m_container._m_scrollRect.onEndDragDelegate -= _onEndDragDelegate;
                _m_container._m_scrollRect.onValueChanged.RemoveListener(_onValueChanged);
            }
            public override bool canEnterState(EPosFixType _newState)
            {
                return true;
            }
            
            
            private void _onEndDragDelegate()
            {
                _m_isDragEnd = true;
            }
            private void _onValueChanged(Vector2 _value)
            {
                // 在拖动的过程中，实时计算当前的 item 索引
                float normalizedPos = _m_container._m_scrollRect.vertical ? _value.y : _value.x;
                _m_container.currentItemIndex = Mathf.Clamp(Mathf.RoundToInt(normalizedPos * (_m_container.itemNum - 1)), 0, _m_container.itemNum - 1);

                // 如果拖动还没结束，后面的处理就不做了
                if (!_m_isDragEnd)
                    return;

                // 如果此时的拖动已经结束，并且速度小于一定值，就自动修正到最近的 item
                float velocity = _m_container._m_scrollRect.vertical ? _m_container._m_scrollRect.velocity.y : _m_container._m_scrollRect.velocity.x;
                velocity = Mathf.Abs(velocity);
                if (velocity < _m_container.dragEndVelocityThreshold + 0.01f)
                    _m_container.fixToItemIndex(-1, true);
            }
        }

        #endregion
    }
}