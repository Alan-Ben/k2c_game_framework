using System;
using System.Collections.Generic;
using ALPackage;
using Common.TreasureHuntEnum;
using Common.TreasureHuntObj;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class TreasureHuntGameLogic
    {
        public class TreasureHuntGameEndState : _ATreasureHuntGameState<bool, bool, float>
        {
            private bool _m_isAdvance;
            private float _m_gameStartPos;
            private GNodeCommonWndWithCloseFunc _m_endEffectNode;
            
            
            public TreasureHuntGameEndState([NotNull] TreasureHuntGameLogic _gameLogic)
                : base(_gameLogic)
            {
            }


            public override TreasureHuntGameStateType state { get { return TreasureHuntGameStateType.END; } }


            protected override void _onEnter(bool _isWin, bool _isAdvance, float _gameStartPos)
            {
                _m_isAdvance = _isAdvance;
                _m_gameStartPos = _gameStartPos;
                
                gameLogic._m_playerUnit.animSetBoostRemainTime(-1);
                
                // 停止玩家移动
                if (!_isWin)
                {
                    gameLogic._m_playerUnit.setToZeroSpeed(true);
                    gameLogic._m_playerUnit.setAdditionalSpeed(0, true);
                    gameLogic._m_playerUnit.animTriggerGameLose();
                }
                else
                    gameLogic._m_playerUnit.animTriggerGameWin();

                List<TreasureHunt_CaptureResult> resultList = null;
                int serialize = enterSerialize;
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(2);
                stepCounter.regAllDoneDelegate(() =>
                {
                    if (serialize != enterSerialize)
                        return;
                    
                    GGUIWndTreasureHuntGamePlayEndEffect.instance.refreshWnd(_isWin);
                    _m_endEffectNode = new GNodeCommonWndWithCloseFunc(GGUIWndTreasureHuntGamePlayEndEffect.instance, () =>
                    {
                        if (serialize != enterSerialize)
                            return;
                        
                        // 显示捕获结果
                        TreasureHuntUtil.showCaptureResult(resultList, () =>
                        {
                            if (serialize != enterSerialize)
                                return;
                        
                            // 所有结果显示完成后，返回到空闲状态
                            gameLogic._m_stateMachine.changeState(new TreasureHuntGameIdleState(gameLogic));
                        });
                    }, UINodeTagConst.C_TREASURE_HUNT_GAME_PLAY_END_EFFECT);
                    QueueMgr.instance.AddNode(_m_endEffectNode);
                });
                
                
                // 向服务器发送游戏结果并处理奖励
                _sendGameResultToServer(_isWin, _list =>
                {
                    resultList = _list;
                    stepCounter.addDoneStepCount();
                });
                MainAdditionTreasureHuntGameTDScene.instance.doEndGameDelay(_isWin, stepCounter.addDoneStepCount);
            }
            protected override void _onExit()
            {
                QueueMgr.instance.forceCloseNode(_m_endEffectNode);
                _m_endEffectNode = null;
            }
            public override bool canEnterState(TreasureHuntGameStateType _newState)
            {
                return _newState is TreasureHuntGameStateType.NONE or TreasureHuntGameStateType.IDLE;
            }
            
            
            private void _sendGameResultToServer(bool _isWin, Action<List<TreasureHunt_CaptureResult>> _complete)
            {
                long serialize = enterSerialize;
                
                float distance = gameLogic._m_playerUnit.forwardOffset - _m_gameStartPos;
                // 转换回原始尺度发送给服务器
                float worldScale = MainAdditionTreasureHuntGameTDScene.instance.getWorldScale();
                float originalDistance = distance / worldScale;
                // 请求服务器处理游戏结果
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntOreCapture(ETreasureHuntCaptureType.NORMAL, _m_isAdvance, gameLogic._m_areaRefObj.area_id, Mathf.RoundToInt(originalDistance), (_isSuc, _msg) =>
                {
                    if (serialize != enterSerialize)
                    {
                        _complete?.Invoke(null);
                        return;
                    }
                    
                    if (!_isSuc || _msg == null)
                    {
                        ALLog.Error("Failed to get treasure hunt result from server");
                        _complete?.Invoke(null);
                        return;
                    }
                    
                    _complete?.Invoke(_msg.getResultList());
                });
            }
            private void _showCaptureResult(List<TreasureHunt_CaptureResult> _resultInfoList, Action _showDone = null)
            {
                if (_resultInfoList == null)
                {
                    _showDone?.Invoke();
                    return;
                }

                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.regAllDoneDelegate(_showDone);
                stepCounter.chgTotalStepCount(1);

                foreach (TreasureHunt_CaptureResult item in _resultInfoList)
                {
                    List<TreasureHunt_CaptureReward> rewardList = item?.getCaptureRewardList();
                    if (rewardList == null)
                        continue;

                    foreach (TreasureHunt_CaptureReward reward in rewardList)
                    {
                        stepCounter.chgTotalStepCount(1);
                        _showCaptureResult(reward, stepCounter.addDoneStepCount);
                    }
                }
                
                stepCounter.addDoneStepCount();
            }
            private void _showCaptureResult(TreasureHunt_CaptureReward _resultInfo, Action _showDone = null)
            {
                if (_resultInfo == null)
                {
                    _showDone?.Invoke();
                    return;
                }
                
                TreasureHuntCaptureResultBase resultBaseInfo = TreasureHuntCaptureResultBase.CreateCaptureResult(_resultInfo);
                if (resultBaseInfo == null)
                {
                    _showDone?.Invoke();
                    return;
                }
                
                resultBaseInfo.showCaptureResult(_showDone);
            }
        }
    }
}