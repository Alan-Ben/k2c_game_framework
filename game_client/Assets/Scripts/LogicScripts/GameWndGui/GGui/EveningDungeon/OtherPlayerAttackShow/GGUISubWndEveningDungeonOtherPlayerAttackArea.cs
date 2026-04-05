using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;

namespace GOE
{
    /// <summary>
    /// 晚间活动其他玩家攻击区域
    /// </summary>
    public class GGUISubWndEveningDungeonOtherPlayerAttackArea : _ANPGGUIBasicSubWnd<GGUISubMonoEveningDungeonOtherPlayerAttackArea>
    {
        private Common.DungeonObj.EveningDungeon_AttackLog _m_attackLog;//攻击日志

        private NPGGUIWndPlayerIcon _m_wPlayerIcon;//玩家头像
        private GGUIWndEveningDungeonGameAirshipShow _m_wAirshipShow;//飞船显示
        private CommonUISfxObj _m_attackFlySfx;//攻击表现飞行特效
        private GGUIWndEveningDungeonLossBloodTip _m_wLossBloodTip;//掉血tip窗口

        private NPCenterTipsRefObj _m_rLossHpCenterTipsRefObj;//掉血tip配表数据
        
        private long _m_lShowAttackSerialize;//攻击表现序列号
        
        public GGUISubWndEveningDungeonOtherPlayerAttackArea(GGUISubMonoEveningDungeonOtherPlayerAttackArea _wnd) : base(_wnd)
        {
            initWnd();
        }

        public Common.DungeonObj.EveningDungeon_AttackLog attackLog { get => _m_attackLog; }
        
        // 一次完整的攻击表现流程显示完成回调
        public event Action<GGUISubWndEveningDungeonOtherPlayerAttackArea> onOnceAttackShowDone;
        // 所有攻击表现显示完成回调
        public event Action<GGUISubWndEveningDungeonOtherPlayerAttackArea> onTotalAttackShowDone;
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);
            
            if(wnd.monoAirshipShow != null)
                _m_wAirshipShow = new GGUIWndEveningDungeonGameAirshipShow(wnd.monoAirshipShow);
            
            if(wnd.monoLossBloodTip != null)
                _m_wLossBloodTip = new GGUIWndEveningDungeonLossBloodTip(wnd.monoLossBloodTip);
        }
        
        protected override void _onDiscard()
        {
            onOnceAttackShowDone = null;
            onTotalAttackShowDone = null;

            _m_rLossHpCenterTipsRefObj = null;
            
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;
            
            _m_wAirshipShow?.discard();
            _m_wAirshipShow = null;
            
            _m_attackFlySfx?.forceDiscard();
            _m_attackFlySfx = null;
            
            _m_wLossBloodTip?.discard();
            _m_wLossBloodTip = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wAirshipShow?.showWnd();
            _m_wLossBloodTip?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wPlayerIcon?.hideWnd();
            _m_wAirshipShow?.hideWnd();
        
            _m_attackFlySfx?.forceDiscard();
            _m_attackFlySfx = null;
            
            _m_wLossBloodTip?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
            _m_wAirshipShow?.resetWnd();
            
            _m_attackFlySfx?.forceDiscard();
            _m_attackFlySfx = null;
         
            _m_wLossBloodTip?.resetWnd();
        }

        public bool showAttack(Common.DungeonObj.EveningDungeon_AttackLog _attackLog)
        {
            if (wnd == null)
                return false;
            
            _m_attackLog = _attackLog;
            long serialize = _m_lShowAttackSerialize = ALSerializeOpMgr.next();
            
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_complete) =>
                {
                    if (_m_lShowAttackSerialize != serialize)
                        return;

                    if (_m_attackLog == null)
                    {
                        _complete?.Invoke();
                        return;
                    }
                    
                    _m_wPlayerIcon?.setPlayer(_m_attackLog.getCid(), () =>
                    {
                        if (_m_lShowAttackSerialize != serialize)
                            return;

                        _m_wPlayerIcon?.showWnd();
                    });

                    if (_m_wAirshipShow != null)
                    {
                        _m_wAirshipShow.airshipEntry(_m_attackLog.getAttackHeroId(), _complete);
                    }
                    else
                    {
                        _complete?.Invoke();
                    }
                })
                .addDelegateProcess((_complete) =>
                {
                    if (_m_lShowAttackSerialize != serialize || wnd == null)
                        return;
                    
                    if (_m_wAirshipShow != null)
                    {
                        _m_wAirshipShow.airshipAttack(_complete);
                    }
                    else
                    {
                        _complete?.Invoke();
                    }
                })
                .addDelegateProcess((_complete) =>
                {
                    if (_m_lShowAttackSerialize != serialize || wnd == null)
                        return;

                    // 显示掉血tip
                    if (_m_attackLog != null)
                        _showLossBloodTip(_m_attackLog.getHarmHp());
                    
                    // 头像隐藏
                    _m_wPlayerIcon?.hideWnd();
                    
                    // 飞船离场
                    if (_m_wAirshipShow != null)
                    {
                        _m_wAirshipShow.airshipDeparture(_complete);
                    }
                    else
                    {
                        _complete?.Invoke();
                    }
                })
                .addDelegateProcess((_complete) =>
                {
                    if (_m_lShowAttackSerialize != serialize)
                        return;

                    onOnceAttackShowDone?.Invoke(this);
                    // 因为在onOnceAttackShowDone回调中有可能之间进行下一次攻击表现, 所以这里延迟到下一帧继续后面一步, 若这时该item需要继续显示时, 序列号会更新, 就不会继续执行隐藏窗口操作
                    ALCommonTaskController.CommonActionAddNextFrameTask(_complete);
                })
                .addProcess(() =>
                {
                    if (_m_lShowAttackSerialize != serialize || wnd == null)
                        return;

                    onTotalAttackShowDone?.Invoke(this);
                });
            
            process.deal();
            return true;
        }
        
        // /// <summary>
        // /// 进行攻击表现
        // /// </summary>
        // public bool showAttack(Common.DungeonObj.EveningDungeon_AttackLog _attackLog)
        // {
        //     if (wnd == null)
        //         return false;
        //     
        //     _m_attackLog = _attackLog;
        //     long serialize = _m_lShowAttackSerialize = ALSerializeOpMgr.next();
        //     
        //     ALProcess process = ALProcess.CreateProcess();
        //     process
        //         .addDelegateProcess((_complete) =>
        //         {
        //             if (_m_lShowAttackSerialize != serialize)
        //                 return;
        //
        //             if (_m_wPlayerIcon == null)
        //                 _complete?.Invoke();
        //             else
        //             {
        //                 if (_m_attackLog != null)
        //                 {
        //                     _m_wPlayerIcon.setPlayer(_m_attackLog.getCid(), () =>
        //                     {
        //                         if (_m_lShowAttackSerialize != serialize)
        //                             return;
        //
        //                         if (_m_wPlayerIcon == null)
        //                             _complete?.Invoke();
        //                         else
        //                             _m_wPlayerIcon.showWnd(_complete);
        //                     });
        //                 }
        //             }
        //         })
        //         .addDelegateProcess((_complete) =>
        //         {
        //             if (_m_lShowAttackSerialize != serialize || wnd == null)
        //                 return;
        //
        //             ALStepCounter stepCounter = new ALStepCounter();
        //             stepCounter.chgTotalStepCount(2);
        //             stepCounter.regAllDoneDelegate(_complete);
        //
        //             _playAnimation(wnd.attackAniName, stepCounter.addDoneStepCount);
        //             _showAttackFlySfx(stepCounter.addDoneStepCount);
        //         })
        //         .addDelegateProcess((_complete) =>
        //         {
        //             if (_m_lShowAttackSerialize != serialize || wnd == null)
        //                 return;
        //
        //             if (_m_attackLog != null)
        //                 _showLossBloodTip(_m_attackLog.getHarmHp());
        //             
        //             _playAnimation(wnd.afterAttackFlyAniName, _complete);
        //         })
        //         .addDelegateProcess((_complete) =>
        //         {
        //             if (_m_lShowAttackSerialize != serialize)
        //                 return;
        //
        //             onAttackShowDone?.Invoke(this);
        //             // 因为在onAttackShowDone回调中有可能之间进行下一次攻击表现, 所以这里延迟到下一帧继续后面一步, 若这时该item需要继续显示时, 序列号会更新, 就不会继续执行隐藏窗口操作
        //             ALCommonTaskController.CommonActionAddNextFrameTask(_complete);
        //         })
        //         .addProcess(() =>
        //         {
        //             if (_m_lShowAttackSerialize != serialize || wnd == null)
        //                 return;
        //
        //             if (_m_wPlayerIcon != null)
        //             {
        //                 _m_wPlayerIcon.hideWnd(()=>
        //                 {
        //                     onHeadHideDone?.Invoke(this);
        //                 });
        //             }
        //             else
        //             {
        //                 onHeadHideDone?.Invoke(this);
        //             }
        //         });
        //     
        //     process.deal();
        //     return true;
        // }

        // /// <summary>
        // /// 显示攻击飞行表现
        // /// </summary>
        // private void _showAttackFlySfx(Action _showDone)
        // {
        //     _m_attackFlySfx?.forceDiscard();
        //     if (wnd == null || wnd.attackTargetTransform == null || wnd.attackFlySfxId <= 0 || wnd.attackFlySfxTime <= 0)
        //     {
        //         _showDone?.Invoke();
        //         return;
        //     }
        //
        //     _m_attackFlySfx = PlaySfxMgr.instance.playUISfx(wnd.attackFlySfxId, wnd.attackFlySfxParent == null ? rectTransform : wnd.attackFlySfxParent);
        //     if (_m_attackFlySfx == null || _m_attackFlySfx.sfxMono == null || _m_attackFlySfx.sfxGo == null)
        //     {
        //         _showDone?.Invoke();
        //     }
        //     else
        //     {
        //         _m_attackFlySfx.regLoadDoneDelegate(() =>
        //         {
        //             if(wnd == null || _m_attackFlySfx.sfxMono == null || wnd.attackTargetTransform == null)
        //                 return;
        //             
        //             if(_m_wPlayerIcon != null && _m_wPlayerIcon.rectTransform != null)
        //                 _m_attackFlySfx.setWorldPos(_m_wPlayerIcon.rectTransform.position);
        //             
        //             _m_attackFlySfx.sfxMono.transform
        //                 .DOMove(wnd.attackTargetTransform.position, wnd.attackFlySfxTime)
        //                 .OnComplete(() =>
        //                 {
        //                     _m_attackFlySfx?.forceDiscard();
        //                     _showDone?.Invoke();
        //                 });
        //         });
        //     }
        // }
        
        /// <summary>
        /// 播放动画
        /// </summary>
        private void _playAnimation(string _aniName, Action _onPlayDone)
        {
            if (wnd == null || wnd.wndAnimation == null || string.IsNullOrEmpty(_aniName))
            {
                _onPlayDone?.Invoke();
                return;
            }
            
            wnd.wndAnimation.ForcePlay(_aniName, 0f, _onPlayDone);
        }
        
        /// <summary>
        /// 显示掉血tip
        /// </summary>
        private void _showLossBloodTip(long _lossHp, Action _onPop = null)
        {
            if (_m_wLossBloodTip == null)
            {
                _onPop?.Invoke();
                return;
            }
            
            _m_wLossBloodTip.showLossBloodTip(_lossHp, _onPop);
        }
    }
}