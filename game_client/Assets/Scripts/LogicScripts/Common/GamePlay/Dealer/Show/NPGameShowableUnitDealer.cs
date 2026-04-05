using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 表现接口处理器
    /// </summary>
    /// <remarks>
    /// 如果你的游戏要接入表现层，需要加入这个处理器
    /// </remarks>
    public class NPGameShowableUnitDealer<T> : _AGameDealer where T : _INPGameShow
    {
        [NotNull] private readonly List<_INPGameShowableUnit<T>> _m_unitList;
        private T _m_gameShow;
        
        public NPGameShowableUnitDealer([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_unitList = new List<_INPGameShowableUnit<T>>();
        }

        protected override void _onStart()
        {
        }

        protected override void _onStop()
        {
        }
        
        /// <summary>
        /// 当前注册进来的表现接口
        /// </summary>
        public T showInterface { get { return _m_gameShow; } }
        
        /// <summary>
        /// 注册一个表现接口
        /// </summary>
        public void setGameShow([NotNull] T _gameShow)
        {
            if (_m_gameShow != null)
                resetGameShow();

            _m_gameShow = _gameShow;
            foreach (_INPGameShowableUnit<T> showUnit in _m_unitList)
            {
                showUnit?.setGameShow(_m_gameShow);
            }
        }
        /// <summary>
        /// 清除表现接口
        /// </summary>
        public void resetGameShow()
        {
            foreach (_INPGameShowableUnit<T> showUnit in _m_unitList)
            {
                showUnit?.resetGameShow();
            }
            _m_gameShow = default;
        }
        
        /// <summary>
        /// 重新设置单位表现层接口
        /// </summary>
        public void refreshSetUnitShow(_AGameUnit _unit)
        {
            if (_unit is _INPGameShowableUnit<T> showUnit)
            {
                showUnit.refreshSetUnitShow();
            }
        }

        protected override void _onTick(float _deltaTime)
        {
        }

        protected override void _onAddGameUnit(_AGameUnit _unit)
        {
            if (_unit is _INPGameShowableUnit<T> showUnit)
            {
                _m_unitList.Add(showUnit);
                if (_m_gameShow != null)
                    showUnit.setGameShow(_m_gameShow);
            }
        }

        protected override void _onRemoveGameUnit(_AGameUnit _unit)
        {
            if (_unit is _INPGameShowableUnit<T> showUnit)
            {
                _m_unitList.Remove(showUnit);
                if (_m_gameShow != null)
                    showUnit.resetGameShow();
            }
        }
    }
}