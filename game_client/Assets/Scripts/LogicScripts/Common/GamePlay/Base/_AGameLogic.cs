
using System;
using System.Collections.Generic;

using ALPackage;
using UnityEngine;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 一个设计Gameplay的逻辑层基类
    /// </summary>
    /// <remarks>
    /// 设计初衷在于将逻辑层和表现层分离，如果你需要这种实现方式，可以使用这个类作为模板
    /// </remarks>
    public abstract class _AGameLogic
    {
        // 是否开始成功并且正在运行
        private bool _m_isRunning;
        // 是否开始了
        private bool _m_isStart;
        // 是否暂停了
        private bool _m_isPause;
        // 游戏主Tick
        private ALCommonEnableTaskController _m_tickTask;
        // 当前游戏的序列号，用来同步一些异步的操作
        private int _m_startSerialize;

        // 游戏处理器
        [ItemNotNull][NotNull] private readonly List<_AGameDealer> _m_dealerList;
        // 游戏单位的列表
        [ItemNotNull][NotNull] private readonly List<_IGameTickUnit> _m_tickUnitList;
        [ItemNotNull][NotNull] private readonly List<_AGameUnit> _m_unitList;

        protected _AGameLogic()
        {
            // 赋值初始值
            _m_isRunning = false;
            _m_isStart = false;
            _m_isPause = true;
            
            _m_startSerialize = 0;

            _m_dealerList = new List<_AGameDealer>();
            _m_tickUnitList = new List<_IGameTickUnit>();
            _m_unitList = new List<_AGameUnit>();
        }

        /// <summary>
        /// 逻辑层是否运行起来了
        /// </summary>
        public bool isRunning { get { return _m_isRunning; } }
        /// <summary>
        /// 逻辑层是否启动了（可能还在启动中，也就是没有 running）
        /// </summary>
        public bool isStart { get { return _m_isStart; } }
        /// <summary>
        /// 逻辑层是否暂停了
        /// </summary>
        public bool isPause { get { return _m_isPause; } }
        /// <summary>
        /// 当前游戏的序列号，用来同步一些异步的操作
        /// </summary>
        public long startSerialize { get { return _m_startSerialize; } }
        /// <summary>
        /// 逻辑层全部采用 deltaTime 来 tick
        /// </summary>
        public virtual float deltaTime { get { return Time.deltaTime; } }

        /// <summary>
        /// 开始游戏，有成功回调和失败回调
        /// </summary>
        public void start(Action _onComplete, Action _onFailed)
        {
            // 如果已经启动返回错误
            if (_m_isStart)
            {
                _onFailed?.Invoke();
                return;
            }

            // 标识游戏开始运行
            _m_isStart = true;

            // 自增游戏同步序列号
            _m_startSerialize = ALSerializeOpMgr.next();
            // 缓存当前的序列号
            int serialize = _m_startSerialize;

            // 如果成功了，并且还是相同的操作序列号，就标识游戏正在运行
            void complete()
            {
                // 如果序列号还是相等，就证明启动成功了
                if (serialize == _m_startSerialize)
                {
                    // 开始游戏的tick
                    _m_tickTask.setDisable();
                    _m_tickTask = ALCommonEnableFixedTickActionMonoTask.addFixedMonoTask(_tick, 0);
                    // 调用自类的启动方法
                    _onStart();

                    foreach (_AGameUnit unit in _m_unitList)
                    {
                        unit.init();
                    }
                    // 启动所有的处理器
                    foreach (_AGameDealer dealer in _m_dealerList)
                    {
                        dealer.start();
                    }
                    
                    // 改变标记位
                    _m_isRunning = true;
                    _m_isPause = false;
                    
                    _onComplete?.Invoke();
                }
                else
                    _onFailed?.Invoke();
            }

            // 如果失败了，并且还是相同的操作序列号，就停止游戏
            void failed()
            {
                if (serialize == _m_startSerialize)
                    stop();

                _onFailed?.Invoke();
            }

            // 请求游戏初始化数据，并开始游戏
            _startGameOp(complete, failed);
        }
        /// <summary>
        /// 停止游戏
        /// </summary>
        public void stop()
        {
            if (!_m_isStart)
            {
                return;
            }

            // 停止所有的处理器
            foreach (_AGameDealer dealer in _m_dealerList)
            {
                dealer.stop();
            }
            foreach (_AGameUnit unit in _m_unitList)
            {
                unit.discard();
            }

            // 做停止游戏额外的操作
            _onStop();
            
            // 标识游戏没有在运行
            _m_isStart = false;
            _m_isRunning = false;
            _m_isPause = true;

            // 停止游戏的tick
            _m_tickTask.setDisable();
            
            // 自增游戏同步序列号
            _m_startSerialize++;
        }
        /// <summary>
        /// 暂停游戏运行
        /// </summary>
        public void pauseGame()
        {
            if (_m_isPause)
                return;

            _m_isPause = true;

            // 停止游戏的tick
            _m_tickTask.setDisable();
        }
        /// <summary>
        /// 恢复游戏运行
        /// </summary>
        public void resumeGame()
        {
            if (!_m_isPause)
                return;

            _m_isPause = false;

            // 开始游戏的tick
            _m_tickTask.setDisable();
            _m_tickTask = ALCommonEnableFixedTickActionMonoTask.addFixedMonoTask(_tick, 0);
        }
        /// <summary>
        /// 添加游戏单位
        /// </summary>
        public void addGameUnit(_AGameUnit _unit)
        {
            if (_unit == null)
                return;
            
            _m_unitList.Add(_unit);
            // 初始化游戏单位
            if (isRunning)
                _unit.init();

            if (_unit is _IGameTickUnit tickUnit)
            {
                _m_tickUnitList.Add(tickUnit);
            }

            foreach (_AGameDealer dealer in _m_dealerList)
            {
                dealer.addGameUnit(_unit);
            }
            
        }
        /// <summary>
        /// 移除游戏单位
        /// </summary>
        public void removeGameUnit(_AGameUnit _unit)
        {
            if (_unit == null)
                return;
            
            _m_unitList.Remove(_unit);

            if (_unit is _IGameTickUnit tickUnit)
            {
                _m_tickUnitList.Remove(tickUnit);
            }
            
            foreach (_AGameDealer dealer in _m_dealerList)
            {
                dealer.removeGameUnit(_unit);
            }

            // 释放游戏单位
            if (isRunning)
                _unit.discard();
        }
        /// <summary>
        /// 添加游戏逻辑处理器
        /// </summary>
        public void addGameDealer(_AGameDealer _dealer)
        {
            if (_dealer == null)
                return;
            
            _m_dealerList.Add(_dealer);
            
            // 执行所有 unit 的 tick
            foreach (_AGameUnit unit in _m_unitList)
            {
                _dealer.addGameUnit(unit);
            }
            
            if (isRunning)
                _dealer.start();
        }
        /// <summary>
        /// 移除一个游戏逻辑处理器
        /// </summary>
        public void removeGameDealer(_AGameDealer _dealer)
        {
            if (_dealer == null)
                return;

            if (!_m_dealerList.Remove(_dealer))
                return;
            
            // 执行所有 unit 的 tick
            foreach (_AGameUnit unit in _m_unitList)
            {
                _dealer.removeGameUnit(unit);
            }
            
            if (isRunning)
                _dealer.stop();
        }

        /// <summary>
        /// 实现具体游戏启动所属的准备操作
        /// </summary>
        /// <remarks>
        /// 在启动之后根据启动情况调用对应的 <paramref name="_complete"/> 和 <paramref name="_failed"/> 方法
        /// </remarks>
        protected abstract void _startGameOp(Action _complete, Action _failed);
        /// <summary>
        /// 做对应的开始游戏的操作
        /// </summary>
        protected abstract void _onStart();
        /// <summary>
        /// 做对应的停止游戏的操作
        /// </summary>
        protected abstract void _onStop();
        /// <summary>
        /// 当游戏每帧 tick 的时候调用
        /// </summary>
        protected abstract void _onTick(float _deltaTime);

        [ItemNotNull][NotNull] private readonly List<_AGameDealer> _m_tickDealerList = new List<_AGameDealer>();
        [ItemNotNull][NotNull] private readonly List<_IGameTickUnit> _m_tickTickUnitList = new List<_IGameTickUnit>();
        // 每帧一次的 tick
        private void _tick()
        {
            // 如果游戏没有开始运行就返回
            if (!_m_isRunning)
                return;

            // 执行所有 dealer 的 tick
            _m_tickDealerList.Clear();
            _m_tickDealerList.AddRange(_m_dealerList);
            foreach (_AGameDealer dealer in _m_tickDealerList)
            {
                dealer.tick(deltaTime);
            }
            // 执行所有 unit 的 tick
            _m_tickTickUnitList.Clear();
            _m_tickTickUnitList.AddRange(_m_tickUnitList);
            foreach (_IGameTickUnit tickUnit in _m_tickTickUnitList)
            {
                tickUnit.tick(deltaTime);
            }
            // 调用游戏逻辑本身的 tick
            _onTick(deltaTime);
        }
    }
}