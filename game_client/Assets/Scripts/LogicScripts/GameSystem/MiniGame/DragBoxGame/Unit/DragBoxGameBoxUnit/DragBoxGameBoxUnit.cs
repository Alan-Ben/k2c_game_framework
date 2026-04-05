using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public interface _IDragBoxGameBoxShow
    {
        /// <summary>
        /// 设置GameController
        /// </summary>
        /// <param name="_gameController"></param>
        public void setGameController(DragBoxGameController _gameController, Action<EDragBoxGameBoxDragDirection> _onBoxDrag);
        
        /// <summary>
        /// 显示物体
        /// </summary>
        public void show();

        /// <summary>
        /// 隐藏物体
        /// </summary>
        public void hide();

        /// <summary>
        /// 可拖动的方向列表
        /// </summary>
        public List<EDragBoxGameBoxDragDirection> canDragDirectionList { get; }

        /// <summary>
        /// 拖动box
        /// </summary>
        /// <param name="_direction"></param>
        public void dragBox(EDragBoxGameBoxDragDirection _direction, bool _isMatched);
    }
    
    public class DragBoxGameBoxUnit : _ADragBoxGameUnit
    {
        [NotNull] private _IDragBoxGameBoxShow _m_iBoxShow;//物品窗口
        private Action<DragBoxGameBoxUnit> _m_aOnBoxMatched;//box匹配成功时回调

        private bool _m_bIsBoxMatched;//是否匹配成功

        public DragBoxGameBoxUnit([NotNull] _IDragBoxGameBoxShow _boxShow, Action<DragBoxGameBoxUnit> _onBoxMatched, [NotNull] DragBoxGameLogic _gameLogic, [NotNull] DragBoxGameController _gameController) : base(_gameLogic, _gameController)
        {
            _m_iBoxShow = _boxShow;
            _m_aOnBoxMatched = _onBoxMatched;
        }
        
        [NotNull] internal _IDragBoxGameBoxShow boxShow { get { return _m_iBoxShow; } }

        public override void init()
        {
            _m_iBoxShow.setGameController(_m_gameController, _onBoxDrag);
            _m_iBoxShow.show();

            _m_bIsBoxMatched = false;
        }

        public override void discard()
        {
            _m_iBoxShow.setGameController(null, null);
            _m_iBoxShow.hide();
        }

        private void _onBoxDrag(EDragBoxGameBoxDragDirection _direction)
        {
            if (_m_bIsBoxMatched)
                return;

            if (_m_iBoxShow.canDragDirectionList != null && _m_iBoxShow.canDragDirectionList.Contains(_direction))
            {
                _m_bIsBoxMatched = true;
                _m_aOnBoxMatched?.Invoke(this);
            }
            
            _m_iBoxShow.dragBox(_direction, _m_bIsBoxMatched);
        }
    }
}