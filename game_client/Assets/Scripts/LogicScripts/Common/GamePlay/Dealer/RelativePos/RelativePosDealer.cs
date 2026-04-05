using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 相对位置处理器，管理单位相对于原点的位置
    /// </summary>
    public class RelativePosDealer : _AGameDealer
    {
        private Vector3 _m_originPos;
        [ItemNotNull][NotNull] private readonly List<_IRelativePosUnit> _m_relativePosUnits;
        
        
        public RelativePosDealer([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_relativePosUnits = new List<_IRelativePosUnit>();
            _m_originPos = Vector3.zero;
        }
        public RelativePosDealer([NotNull] _AGameLogic _gameLogic, Vector3 _originPos) : base(_gameLogic)
        {
            _m_relativePosUnits = new List<_IRelativePosUnit>();
            _m_originPos = _originPos;
        }
        

        /// <summary>
        /// 获取当前原点位置
        /// </summary>
        public Vector3 originPos { get { return _m_originPos; } }


        protected override void _onStart()
        {
            _updateAllRelativePositions();
        }
        protected override void _onStop()
        {
        }
        protected override void _onTick(float _deltaTime)
        {
            _updateAllRelativePositions();
        }
        protected override void _onAddGameUnit(_AGameUnit _unit)
        {
            if (_unit is _IRelativePosUnit relativePosUnit)
            {
                _m_relativePosUnits.Add(relativePosUnit);
                if (isRunning)
                    _updateRelativePosition(relativePosUnit);
            }
        }
        protected override void _onRemoveGameUnit(_AGameUnit _unit)
        {
            if (_unit is _IRelativePosUnit relativePosUnit)
                _m_relativePosUnits.Remove(relativePosUnit);
        }
        

        /// <summary>
        /// 设置原点位置
        /// </summary>
        public void setOriginPos(Vector3 _position)
        {
            _m_originPos = _position;
            _updateAllRelativePositions();
        }
        

        /// <summary>
        /// 更新所有单位的相对位置
        /// </summary>
        private void _updateAllRelativePositions()
        {
            foreach (_IRelativePosUnit unit in _m_relativePosUnits)
            {
                _updateRelativePosition(unit);
            }
        }
        /// <summary>
        /// 更新单个单位的相对位置
        /// </summary>
        private void _updateRelativePosition([NotNull] _IRelativePosUnit _unit)
        {
            _unit.relativePosition = _unit.position - _m_originPos;
        }
    }
}