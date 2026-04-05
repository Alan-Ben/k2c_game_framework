
using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class GGUIWndNumMergeGamePlay
    {
        private enum ResetType
        {
            NONE,
            GAME_OVER,
            ORGANIZE,
        }
        
        private class ResetAllState : _AHotfixSimpleState<GameState, ResetType>
        {
            [NotNull] private static readonly Vector2Int[] _snakeOrderPositions = new Vector2Int[]
            {
                new Vector2Int(0, 3), new Vector2Int(0, 2), new Vector2Int(0, 1), new Vector2Int(0, 0),
                new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2), new Vector2Int(1, 3),
                new Vector2Int(2, 3), new Vector2Int(2, 2), new Vector2Int(2, 1), new Vector2Int(2, 0),
                new Vector2Int(3, 0), new Vector2Int(3, 1), new Vector2Int(3, 2), new Vector2Int(3, 3),
            };

            [NotNull] private readonly GGUIWndNumMergeGamePlay _m_wnd;


            public ResetAllState([NotNull] GGUIWndNumMergeGamePlay _wnd)
            {
                _m_wnd = _wnd;
            }


            public override GameState state { get { return GameState.RESET_ALL; } }


            protected override void _onEnter(ResetType _type)
            {
                int serialize = enterSerialize;
                _playResetTip(_type, () =>
                {
                    if (serialize != enterSerialize)
                        return;
                    
                    _destroyAllItems(_type, () =>
                    {
                        if (serialize != enterSerialize)
                            return;

                        _createAllItems(_type, () =>
                        {
                            if (serialize != enterSerialize)
                                return;

                            _m_wnd._m_machine.changeState(new IdleState(_m_wnd));
                        });
                    });
                });
            }
            protected override void _onExit()
            {
            }
            public override bool canEnterState(GameState _newState)
            {
                return true;
            }


            private void _playResetTip(ResetType _type, Action _complete)
            {
                switch (_type)
                {
                    case ResetType.GAME_OVER:
                        _m_wnd.playGameOverAnim(_complete);
                        break;
                    // case ResetType.ORGANIZE:
                    default:
                        _complete?.Invoke();
                        break;
                }
            }
            private void _destroyAllItems(ResetType _type, Action _complete)
            {
                int serialize = enterSerialize;
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(1);
                stepCounter.regAllDoneDelegate(() =>
                {
                    if (serialize != enterSerialize)
                    {
                        _complete?.Invoke();
                        return;
                    }
                    
                    _m_wnd._destroyAllItems();
                    _complete?.Invoke();
                });

                int count = 0;
                foreach (Vector2Int gridPos in NumMergeComponent.allBoardPositions)
                {
                    GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(gridPos);
                    if (item == null)
                        continue;

                    switch (_type)
                    {
                        case ResetType.GAME_OVER:
                            stepCounter.chgTotalStepCount(1);
                            item.playGameOverDestroyAnim(stepCounter.addDoneStepCount, _m_wnd.itemAnimSpace * count++);
                            break;
                        case ResetType.ORGANIZE:
                            stepCounter.chgTotalStepCount(1);
                            item.playOrganizeDestroyAnim(stepCounter.addDoneStepCount, _m_wnd.itemAnimSpace * count++);
                            break;
                    }
                }
                
                stepCounter.addDoneStepCount();
            }
            private void _createAllItems(ResetType _type, Action _complete)
            {
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(1);
                stepCounter.regAllDoneDelegate(_complete);

                NumMergeTileInfo[][] tileInfos = HotfixNPPlayer.instance.numMergeComponent.tileInfos;
                int count = 0;

                // ORGANIZE 类型使用蛇形遍历（从左上角往下，W字形轨迹）
                Vector2Int[] positions = _type == ResetType.ORGANIZE ? _snakeOrderPositions : NumMergeComponent.allBoardPositions;

                foreach (Vector2Int gridPos in positions)
                {
                    NumMergeTileInfo tileInfo = tileInfos[gridPos.x][gridPos.y];
                    if (tileInfo.isEmpty)
                        continue;

                    _m_wnd._createItem(gridPos, tileInfo.blockRefObj, tileInfo.buffStep);

                    GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(gridPos);
                    if (item == null)
                        continue;

                    switch (_type)
                    {
                        case ResetType.NONE:
                            item.playResetAllAnim();
                            break;
                        case ResetType.ORGANIZE:
                            stepCounter.chgTotalStepCount(1);
                            item.playOrganizeSpawnAnim(stepCounter.addDoneStepCount, _m_wnd.itemAnimSpace * count++);
                            break;
                        case ResetType.GAME_OVER:
                            stepCounter.chgTotalStepCount(1);
                            item.playSpawnAnim(stepCounter.addDoneStepCount, _m_wnd.itemAnimSpace * count++);
                            break;
                    }
                }

                stepCounter.addDoneStepCount();
            }
        }
    }
}
