using System;
using System.Collections.Generic;
using ALPackage;
using Common.ChapterEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class ChapterForwardLogicMgr
    {
        //是否初始化
        private bool _m_isInit;
        // 刷新任务
        private ALCommonEnableTaskController _m_tickTask;

        //是否在自动前进
        private bool _m_isInAutoForward;
        //是否在快速前进
        private bool _m_isInQuickForward;
        //上次连续点击的事件
        private double _m_lastContinueClickTime;
        //是否在连续点击前进
        private bool _m_isInContinueClickForward;
        
        //执行自动前进最大鼓舞次数
        [NotNull]private Dictionary<EChapterInspireType, int> _m_autoForwardInspireMaxDic = new Dictionary<EChapterInspireType, int>();
        
        // 一个内部的简单状态机
        private ChapterStateMachine _m_stateMachine;
        //表现接口
        private _IChapterEffectInterface _m_effectInterface;
        //地图表现接口
        private _IChapterMapEffectInterface _m_mapEffectInterface;
        
        
        public bool isInAutoForward { get { return _m_isInAutoForward; } }
        public bool isInQuickForward { get { return _m_isInQuickForward; }  }
        public bool isInContinueClickForward { get { return _m_isInContinueClickForward; } }
        
        public EChapterForwardType curState { get { return null == _m_stateMachine ? EChapterForwardType.NONE : _m_stateMachine.curState.state; } }
        
        public void init()
        {
            if(_m_isInit)
                return;
            _m_isInit = true;

            _m_isInAutoForward = false;
            _m_isInQuickForward = false;
            _m_isInContinueClickForward = false;
            _m_lastContinueClickTime = 0;
            
            _m_stateMachine = new ChapterStateMachine(new ChapterStateFactory(this));
            _m_stateMachine.changeState<IdleState>();
            
            // 开始刷新任务
            _m_tickTask.setDisable();
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick, 0);
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAPTER_POS_CHG_CHEAT, _onChapterPosChg);
        }

        public void discard()
        {
            _m_isInAutoForward = false;
            _m_isInQuickForward = false;
            _m_isInContinueClickForward = false;
            _m_lastContinueClickTime = 0;
            
            _m_isInit = false;
            if (_m_stateMachine != null) 
                _m_stateMachine.changeState<NoneState>();
            _m_stateMachine = null;
            
            _m_tickTask.setDisable();
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAPTER_POS_CHG_CHEAT, _onChapterPosChg);
        }
        
        private void _onChapterPosChg()
        {
            //为了作弊命令处理，作弊命令修改位置，直接强制到idle状态
            if(_m_stateMachine.curState.state != EChapterForwardType.ONCE_FORWARD)
            {
                _m_stateMachine.setState<IdleState>();
            }
        }
        
        //注册表现接口
        public void regEffectInterface(_IChapterEffectInterface _interface)
        {
            if(null == _interface)
                return;
            
            _m_effectInterface = _interface;
        }
        
        //注册表现接口
        public void unregEffectInterface(_IChapterEffectInterface _interface)
        {
            if(null == _interface)
                return;

            if (_m_effectInterface == _interface)
                _m_effectInterface = null;
        }
        
        //注册表现接口
        public void regMapEffectInterface(_IChapterMapEffectInterface _interface)
        {
            if(null == _interface)
                return;
            
            _m_mapEffectInterface = _interface;
        }
        
        //注册表现接口
        public void unregMapEffectInterface(_IChapterMapEffectInterface _interface)
        {
            if(null == _interface)
                return;

            if (_m_mapEffectInterface == _interface)
                _m_mapEffectInterface = null;
        }
        
        /// <summary>
        /// 前进关卡
        /// </summary>
        public void forwardToChapter(bool _isQuick)
        {
            //当前是是boss战不允许前进
            if(NPPlayer.instance.chapterComp.curIsBossPoint())
                return;
            
            //已经在自动不允许点击前进
            if (NPPlayer.instance.chapterComp.forwardLogicMgr.isInQuickForward)
                return;
            
            //通关不允许前进
            if (NPPlayer.instance.chapterComp.chapterRefObj == null)
                return; 
        
            _m_isInQuickForward = _isQuick;
            if (_m_stateMachine != null) 
                _m_stateMachine.changeState<OnceForwardState>();
        }
        
        /// <summary>
        /// 开始连续点击前进
        /// </summary>
        public void startContinueClickForward(double _time)
        {
            //1秒内连续点击算连续点击前进
            if(_time - _m_lastContinueClickTime < 1)
                _m_isInContinueClickForward = true;
            
            _m_lastContinueClickTime = _time;
        }
        
        /// <summary>
        /// 关闭连续点击前进
        /// </summary>
        public void stopContinueClickForward()
        {
            _m_lastContinueClickTime = 0;
            _m_isInContinueClickForward = false;
        }
        
        /// <summary>
        /// 关卡打boss
        /// </summary>
        public void forwardToBoss()
        {
            //当前不是boss战不允许调动
            if(!NPPlayer.instance.chapterComp.curIsBossPoint())
                return;

            if (_m_stateMachine != null) 
                _m_stateMachine.changeState<FightBossState>();
        }


        public void showDialogSliderTip(long _pointId, Action _doneAction)
        {
            if(null == _m_effectInterface)
            {
                if (_doneAction != null) 
                    _doneAction();
                return;
            }
            
            _m_effectInterface.showDialogSliderTip(_pointId, _doneAction);
        }
        
        /// <summary>
        /// 展示对话表现
        /// </summary>
        public void showDialogTipEffect()
        {
            if(null == _m_effectInterface)
                return;
            
            _m_effectInterface.showDialogTipEffect();
        }
        
        /// <summary>
        /// 展示对话表现
        /// </summary>
        public void showEventTipEffect()
        {
            if(null == _m_effectInterface)
                return;
            
            _m_effectInterface.showEventTipEffect();
        }
        
        /// <summary>
        /// 尝试进入事件状态
        /// </summary>
        public void tryEnterEventState()
        {
            //当前不是boss战不允许调动
            if(NPPlayer.instance.chapterComp.curChapterEventId == 0)
                return;

            if (_m_stateMachine != null) 
                _m_stateMachine.changeState<DealEventState>();
        }
        
        /// <summary>
        /// 尝试进入章节进入对话
        /// </summary>
        public void tryEnterDialogState(long _dialogId)
        {
            //当前不是boss战不允许调动
            if(_dialogId == 0)
                return;

            //不是idle状态不处理
            if(_m_stateMachine?.curState.state != EChapterForwardType.IDLE)
                return;
            
            _m_stateMachine?.changeState<DialogState>((_state) =>
            {
                if (_state != null)
                {
                    _state.setDialogId(_dialogId,  NPPlayer.instance.chapterComp.curPointId, false);
                }
            });
        }
        
        /// <summary>
        /// 停止快速前进
        /// </summary>
        public void stopQuickForwardToChapter()
        {
            _m_isInQuickForward = false;
            
            //如果当前是idle状态再刷一次
            if (null != _m_stateMachine && _m_stateMachine.curState.state == EChapterForwardType.IDLE)
            {
                _dealChgIdleEffect();
            }
        }

        /// <summary>
        /// 执行自动前进
        /// </summary>
        public void startAutoForwardToChapter(Dictionary<EChapterInspireType, int> _inspireInfoDic)
        {
            //不是idle状态不处理
            if(_m_stateMachine?.curState.state != EChapterForwardType.IDLE && _m_stateMachine?.curState.state != EChapterForwardType.FIGHT_BOSS_WAIT)
                return;
            
            //最大鼓舞次数统计
            _m_autoForwardInspireMaxDic.AddRange(_inspireInfoDic);
            
            _m_isInAutoForward = true;
            _m_stateMachine?.changeState<AutoBattleState>();
        }
        
        /// <summary>
        /// 停止自动前进
        /// </summary>
        public void stopAutoForwardToChapter()
        {
            //最大鼓舞次数统计
            _m_autoForwardInspireMaxDic.Clear();
            _m_isInAutoForward = false;

            _m_stateMachine.changeState<IdleState>();
        }

        /// <summary>
        /// 获取最大鼓舞的限制
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public int getMaxInspireTimes(EChapterInspireType _type)
        {
            if (_m_autoForwardInspireMaxDic.TryGetValue(_type, out int times))
                return times;
            return 0;
        }
        
        //处理切换到默认状态
        private void _dealChgIdleEffect()
        {
            if(null == _m_effectInterface)
                return;
            
            _m_effectInterface.chgIdleEffect();
        }
        
        //处理前进表现
        private void _dealChapterForwardEffect(float _fade, long _sfxId, long _tdSfxId, int _coefficient, long _rewardExp, long _rewardPlayerExp, long _eventId, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            if(null == _m_effectInterface)
            {
                return;
            }
            
            _m_effectInterface.forwardToChapter(_fade, _sfxId, _tdSfxId, _coefficient, _rewardExp, _rewardPlayerExp, _eventId, _itemList);
        }
        
        //处理快速前进表现
        private void _dealQuickForwardToChapter(float _fade, long _sfxId, long _tdSfxId, int _coefficient, long _rewardExp, long _rewardPlayerExp, long _eventId, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            if(null == _m_effectInterface)
            {
                return;
            }
            
            _m_effectInterface.quickForwardToChapter(_fade, _sfxId, _tdSfxId, _coefficient, _rewardExp, _rewardPlayerExp, _eventId, _itemList);
        }
        
        //处理前进视频状态动画表现
        private void _dealPlayForwardVideoAniState(EChapterVideoForwardState _forwardState)
        {
            if(null == _m_effectInterface)
            {
                return;
            }
            
            _m_effectInterface.playForwardVideoAniState(_forwardState);
        }
        
        //处理boss战表现
        private void _dealChapterBossEffect(ChapterRefObj _chapterRef, Action _doneAction = null)
        {
            if(null == _m_effectInterface)
            {
                if(null != _doneAction)
                    _doneAction();
                return;
            }
            
            _m_effectInterface.forwardToBoss(_chapterRef, _doneAction);
        }
        
        //处理boss战前摇表现
        private void _dealChapterBossPerEffect(Action _doneAction)
        {
            if(null == _m_effectInterface)
            {
                if(null != _doneAction)
                    _doneAction();
                return;
            }
            
            _m_effectInterface.forwardToBossPer(_doneAction);
        }
        
        //处理boss战前连线表现
        private void _dealChapterBossWaitEffect()
        {
            if(null == _m_effectInterface)
            {
                return;
            }
            
            _m_effectInterface.forwardToBossWait();
        }
        
        //处理节点移动表现
        private void _dealChapterNodeMoveEffect()
        {
            if(null == _m_effectInterface)
            {
                return;
            }
            _m_effectInterface._dealChapterNodeMoveEffect();
        }
        
        //node移动Boss表现
        private void _dealChapterNodeMoveBossEffect()
        {
            if(null == _m_effectInterface)
            {
                return;
            }
            _m_effectInterface._dealChapterNodeMoveBossEffect();
        }
        
        //处理自动前进表现
        private void _dealAutoForwardToChapter(int _targetNodeIndex, float _fade,  int _coefficient, long _rewardExp, long _rewardPlayerExp, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            if(null == _m_mapEffectInterface)
            {
                return;
            }
            
            _m_mapEffectInterface.dealAutoForwardToChapter(_targetNodeIndex, _fade, _coefficient, _rewardExp, _rewardPlayerExp, _itemList);
        }
        
        //处理自动前进表现
        private void dealAutoBossToChapter(Action _playDone)
        {
            if(null == _m_mapEffectInterface)
            {
                return;
            }
            
            _m_mapEffectInterface.dealAutoBossToChapter(_playDone);
        }

        //处理自动前进表现
        private void playChapterCompleteEffect(ChapterRefObj _chapterRef, int _targetNodeIndex)
        {
            if(null == _m_mapEffectInterface)
            {
                return;
            }
            
            _m_mapEffectInterface.playChapterCompleteEffect(_chapterRef, _targetNodeIndex);
        }
        

        //进入自动状态
        private void _enterAutoState()
        {
            if(null == _m_mapEffectInterface)
            {
                return;
            }
            
            _m_mapEffectInterface.enterAutoState();
        }

        //quitAutoState
        private void _quitAutoState()
        {
            if(null == _m_mapEffectInterface)
            {
                return;
            }
            
            _m_mapEffectInterface.quitAutoState();
        }
        
        private void _tick()
        {
            //1秒没重新点击连续点击前进就算结束
            if (Time.realtimeSinceStartupAsDouble - _m_lastContinueClickTime > 1)
            {
                stopContinueClickForward();
            }
            
            if (_m_stateMachine != null) 
                _m_stateMachine.tick(Time.deltaTime);
        }
    }
}