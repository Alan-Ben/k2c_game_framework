
using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using GOE;
using Hotfix.NumMergeEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class GGUIWndNumMergeGamePlay
    {
        private class MergeState : _AHotfixSimpleState<GameState, ENumMerge_MoveDir>
        {
            [ItemNotNull, NotNull] private readonly List<Tween> _m_activeTweens;
            [ItemNotNull, NotNull] private readonly List<GGUIWndNumMergeGamePlayBlockItem> _m_movingItems;
            [NotNull] private readonly GGUIWndNumMergeGamePlay _m_wnd;


            public MergeState([NotNull] GGUIWndNumMergeGamePlay _wnd)
            {
                _m_activeTweens = new List<Tween>(16);
                _m_movingItems = new List<GGUIWndNumMergeGamePlayBlockItem>(16);
                
                _m_wnd = _wnd;
            }


            public override GameState state { get { return GameState.MERGE; } }


            protected override void _onEnter(ENumMerge_MoveDir _moveDir)
            {
                int serialize = enterSerialize;
                bool serverSuccess = false;
                bool hasMoved = false;

                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(2); // 1 for animations, 1 for server response
                // When both complete, validate and transition
                stepCounter.regAllDoneDelegate(() =>
                {
                    if (serialize != enterSerialize)
                        return;

                    if (!hasMoved)
                    {
                        _m_wnd._m_machine.changeState(new IdleState(_m_wnd));
                        return;
                    }
                    
                    if (!serverSuccess)
                    {
                        _m_wnd._m_machine.changeState(new ResetAllState(_m_wnd), ResetType.NONE);
                        return;
                    }

                    // Validate: check if local state matches server state
                    if (_validateMoveResult())
                        _m_wnd._m_machine.changeState(new SpawnState(_m_wnd));
                    else
                    {
                        Debug.LogError("[MERGE] Local moves don't match server, resetting board");
                        _m_wnd._m_machine.changeState(new ResetAllState(_m_wnd), ResetType.NONE);
                    }
                });

                // Execute local move/merge animations
                hasMoved = _executeLocalMoves(_moveDir, stepCounter.addDoneStepCount);
                if (!hasMoved)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(HotfixTransKeyConst.numMerge_notMove_tip);
                    // No local moves, skip server request
                    serverSuccess = true;
                    stepCounter.addDoneStepCount();
                    return;
                }

                // Play move sound
                PlayAudioMgr.instance.playClip(_m_wnd.hotfixWnd?.moveAudioId ?? 0);
                // Send request to server
                int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                HotfixNPPlayer.instance.numMergeComponent.reqNumMergeMove(
                    HotfixAccountSettingMgr.instance.hotfixAccountSetting.getNumMergeModeType(),
                    _moveDir,
                    (_isSuc, _msg) =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                        serverSuccess = _isSuc;
                        stepCounter.addDoneStepCount();
                    });
            }
            protected override void _onExit()
            {
                // 回收所有还在移动的 item
                foreach (GGUIWndNumMergeGamePlayBlockItem item in _m_movingItems)
                    _m_wnd._m_itemCache?.pushBackCacheItem(item);
                _m_movingItems.Clear();
                
                // 清理所有活动的 DOTween 动画
                foreach (Tween tween in _m_activeTweens)
                    tween.Kill();
                _m_activeTweens.Clear();
            }
            public override bool canEnterState(GameState _newState)
            {
                return true;
            }


            private bool _executeLocalMoves(ENumMerge_MoveDir _moveDir, Action _complete)
            {
                bool hasMoved = false;

                switch (_moveDir)
                {
                    case ENumMerge_MoveDir.LEFT:
                        hasMoved = _processDirection(0, 1, 0, 1, -1, 0, _complete);
                        break;
                    case ENumMerge_MoveDir.RIGHT:
                        hasMoved = _processDirection(3, -1, 0, 1, 1, 0, _complete);
                        break;
                    case ENumMerge_MoveDir.UP:
                        hasMoved = _processDirection(0, 1, 3, -1, 0, 1, _complete);
                        break;
                    case ENumMerge_MoveDir.DOWN:
                        hasMoved = _processDirection(0, 1, 0, 1, 0, -1, _complete);
                        break;
                }
                
                return hasMoved;
            }
            /// <summary>
            /// 处理指定方向的所有棋子移动和合并
            /// </summary>
            /// <param name="_startX">X方向扫描起点（左移从0开始，右移从3开始）</param>
            /// <param name="_stepX">X方向扫描步长（左移+1，右移-1）</param>
            /// <param name="_startY">Y方向扫描起点（下移从0开始，上移从3开始）</param>
            /// <param name="_stepY">Y方向扫描步长（下移+1，上移-1）</param>
            /// <param name="_dirX">棋子移动的X方向（-1左/+1右/0不动）</param>
            /// <param name="_dirY">棋子移动的Y方向（-1下/+1上/0不动）</param>
            /// <returns>是否有棋子发生了移动</returns>
            private bool _processDirection(int _startX, int _stepX, int _startY, int _stepY, int _dirX, int _dirY, Action _complete)
            {
                bool hasMoved = false;
                bool[][] merged = new bool[4][]; // 防止同一回合多次合并
                for (int i = 0; i < 4; i++)
                    merged[i] = new bool[4];

                // 第一阶段：根据 2048 的规则计算需要移动的棋子，和它们的目标位置，把目标位置存在 item 的 gridPos 里
                // 按照移动方向的相反方向扫描棋盘（例如右移时从右往左扫描）
                // 这样可以确保先处理距离目标边缘最近的棋子
                for (int x = _startX; _stepX > 0 ? x < 4 : x >= 0; x += _stepX)
                {
                    for (int y = _startY; _stepY > 0 ? y < 4 : y >= 0; y += _stepY)
                    {
                        Vector2Int pos = new Vector2Int(x, y);
                        GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(pos);

                        // 空格子跳过
                        if (item == null)
                            continue;

                        // 找到该棋子在该方向上的最远位置
                        Vector2Int targetPos = _findFarthestPosition(pos, _dirX, _dirY, merged, item.blockRefObj);

                        // 如果目标位置和当前位置不同，表示该棋子需要移动
                        if (targetPos != pos)
                        {
                            // 从棋盘上释放该棋子
                            GGUIWndNumMergeGamePlayBlockItem freedItem = _m_wnd._freeItem(pos);
                            if (freedItem != null)
                            {
                                ALUnityCommon.moveTransformToLast(freedItem.rectTransform);
                                
                                hasMoved = true;
                                // 检查目标位置是否会发生合并（需要检查静态棋子和移动中的棋子）
                                bool willMerge = false;
                                
                                // 先检查是否有移动中的棋子目标是这个位置
                                GGUIWndNumMergeGamePlayBlockItem targetMovingItem = _getMovingItemByTargetPos(targetPos);
                                if (targetMovingItem != null)
                                {
                                    if (targetMovingItem.blockRefObj != null && targetMovingItem.blockRefObj.canMergeWith(freedItem.blockRefObj))
                                        willMerge = true;
                                    else
                                        Debug.LogError("[MERGE] Logic error: two moving items targeting same position but different levels");
                                }

                                // 如果没有移动中的棋子，检查静态棋子
                                if (!willMerge)
                                {
                                    GGUIWndNumMergeGamePlayBlockItem staticItem = _m_wnd._getItem(targetPos);
                                    if (staticItem != null)
                                    {
                                        if (staticItem.blockRefObj != null && staticItem.blockRefObj.canMergeWith(freedItem.blockRefObj))
                                            willMerge = true;
                                        else
                                            Debug.LogError("[MERGE] Logic error: static item at target position has different level");
                                    }
                                }

                                if (willMerge)
                                    merged[targetPos.x][targetPos.y] = true;
                                
                                // 将目标位置存储在 gridPos 中
                                freedItem.gridPos = targetPos;
                                _m_movingItems.Add(freedItem);
                            }
                        }
                    }
                }

                if (!hasMoved)
                {
                    _complete?.Invoke();
                    return false;
                }
                
                float duration = _m_wnd.hotfixWnd?.itemMoveTime ?? 0.1f;
                // 第二阶段：用 dotween 执行所有移动动画，gridPos 是它们的终点
                foreach (GGUIWndNumMergeGamePlayBlockItem movingItem in _m_movingItems)
                {
                    _animateMovingItem(movingItem, movingItem.gridPos, duration);
                }
                
                int serialize = enterSerialize;
                // 第三阶段，遍历所有移动的 item，并和 _m_wnd 的棋盘一起比对，把同位置的 item 进行融合（回收或删除旧 item ，生成融合后的 item ）
                // 如果移动后没有产生融合，则直接把 item 放回棋盘
                CommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialize != enterSerialize)
                        return;
                    
                    HashSet<GGUIWndNumMergeGamePlayBlockItem> processedItems = new HashSet<GGUIWndNumMergeGamePlayBlockItem>();
                    foreach (GGUIWndNumMergeGamePlayBlockItem movingItem in _m_movingItems)
                    {
                        // 如果已经被处理过（作为合并的一部分），跳过
                        if (processedItems.Contains(movingItem))
                            continue;

                        Vector2Int targetPos = movingItem.gridPos;

                        // 先检查是否有另一个移动中的棋子也目标是这个位置
                        GGUIWndNumMergeGamePlayBlockItem otherMovingItem = null;
                        foreach (GGUIWndNumMergeGamePlayBlockItem other in _m_movingItems)
                        {
                            if (other != movingItem && !processedItems.Contains(other) && other.gridPos == targetPos)
                            {
                                otherMovingItem = other;
                                break;
                            }
                        }
                        // 检查静态棋子
                        GGUIWndNumMergeGamePlayBlockItem staticItem = _m_wnd._getItem(targetPos);
                        // 标记当前移动的棋子为已处理
                        processedItems.Add(movingItem);
                        
                        if (otherMovingItem != null)
                        {
                            if (otherMovingItem.blockRefObj != null && otherMovingItem.blockRefObj.canMergeWith(movingItem.blockRefObj))
                            {
                                // 两个移动中的棋子合并
                                int newLevel = movingItem.blockRefObj.level + 1;
                                // 判断要合成的两个棋子是否有 buff
                                bool hasBuff = movingItem.hasBuff || otherMovingItem.hasBuff;
                                
                                // 回收两个移动的棋子
                                _m_wnd._m_itemCache?.pushBackCacheItem(movingItem);
                                _m_wnd._m_itemCache?.pushBackCacheItem(otherMovingItem);

                                // 标记为已处理
                                processedItems.Add(otherMovingItem);

                                // 获取新等级的配置
                                NumMergeBlockRefObj newBlockRefObj = HotfixRefdataCoreMgr.instance.numMergeBlockRefCore.getRef(newLevel);
                                // 创建合并后的新棋子
                                _m_wnd._createItem(targetPos, newBlockRefObj, 0);
                                GGUIWndNumMergeGamePlayBlockItem newItem = _m_wnd._getItem(targetPos);
                                if (hasBuff)
                                    newItem?.playBuffMergeSpawnAnim(null);
                                else
                                    newItem?.playMergeSpawnAnim(null);
                            }
                            else
                            {
                                Debug.LogError("[MERGE] Logic error: two moving items targeting same position but different levels during merge processing");
                                _m_wnd._m_itemCache?.pushBackCacheItem(movingItem);
                            }
                        }
                        else if (staticItem != null)
                        {
                            if (staticItem.blockRefObj != null && staticItem.blockRefObj.canMergeWith(movingItem.blockRefObj))
                            {
                                // 与静态棋子合并
                                int newLevel = movingItem.blockRefObj.level + 1;
                                // 判断要合成的两个棋子是否有 buff
                                bool hasBuff = movingItem.hasBuff || staticItem.hasBuff;
                                
                                // 回收移动的棋子
                                _m_wnd._m_itemCache?.pushBackCacheItem(movingItem);
                                // 销毁静态棋子
                                _m_wnd._destroyItem(targetPos);
                                // 获取新等级的配置
                                NumMergeBlockRefObj newBlockRefObj = HotfixRefdataCoreMgr.instance.numMergeBlockRefCore.getRef(newLevel);
                                // 创建合并后的新棋子
                                _m_wnd._createItem(targetPos, newBlockRefObj, 0);
                                GGUIWndNumMergeGamePlayBlockItem newItem = _m_wnd._getItem(targetPos);
                                if (hasBuff)
                                    newItem?.playBuffMergeSpawnAnim(null);
                                else
                                    newItem?.playMergeSpawnAnim(null);
                            }
                            else
                            {
                                Debug.LogError("[MERGE] Logic error: static item at target position has different level during merge processing");
                                _m_wnd._m_itemCache?.pushBackCacheItem(movingItem);
                            }
                        }
                        else
                        {
                            // 没有合并：直接把棋子放回棋盘
                            _m_wnd._setItem(targetPos, movingItem);
                        }
                    }

                    _m_movingItems.Clear();
                    _complete?.Invoke();
                }, duration);

                return true;
            }

            /// <summary>
            /// 根据目标位置获取移动中的棋子
            /// </summary>
            private GGUIWndNumMergeGamePlayBlockItem _getMovingItemByTargetPos(Vector2Int _targetPos)
            {
                foreach (GGUIWndNumMergeGamePlayBlockItem item in _m_movingItems)
                {
                    if (item.gridPos == _targetPos)
                        return item;
                }
                return null;
            }
            /// <summary>
            /// 找到棋子在指定方向上能移动到的最远位置
            /// </summary>
            private Vector2Int _findFarthestPosition(Vector2Int _from, int _dirX, int _dirY, bool[][] _merged, NumMergeBlockRefObj _blockRef)
            {
                Vector2Int current = _from;
                Vector2Int next = new Vector2Int(current.x + _dirX, current.y + _dirY);

                while (_isValidPosition(next))
                {
                    // 检查静态棋子（还在棋盘上的）
                    GGUIWndNumMergeGamePlayBlockItem staticItem = _m_wnd._getItem(next);
                    // 检查移动中的棋子（已经 free 了，目标位置是 next）
                    GGUIWndNumMergeGamePlayBlockItem movingItem = _getMovingItemByTargetPos(next);

                    // 优先检查移动中的棋子
                    if (movingItem != null)
                    {
                        if (movingItem.blockRefObj.canMergeWith(_blockRef) && !_merged[next.x][next.y])
                            // 可以与移动中的棋子合并
                            return next;

                        // 等级不同或已经合并过，停止
                        break;
                    }

                    if (staticItem == null)
                    {
                        // 空格子，继续移动
                        current = next;
                        next = new Vector2Int(current.x + _dirX, current.y + _dirY);
                    }
                    else if (staticItem.blockRefObj.canMergeWith(_blockRef) && !_merged[next.x][next.y])
                        // 可以与静态棋子合并
                        return next;
                    else
                        // 等级不同或已经合并过，停止
                        break;
                }

                return current;
            }
            /// <summary>
            /// 检查位置是否在棋盘内
            /// </summary>
            private bool _isValidPosition(Vector2Int _pos)
            {
                return _pos.x is >= 0 and < 4 && _pos.y is >= 0 and < 4;
            }
            /// <summary>
            /// 执行单个棋子的移动动画
            /// </summary>
            private void _animateMovingItem(GGUIWndNumMergeGamePlayBlockItem _item, Vector2Int _targetPos, float _duration)
            {
                if (_item == null || _m_wnd.hotfixWnd == null)
                    return;

                Vector3 targetLocalPos = _m_wnd._calculateGridPosition(_targetPos);
                Tween moveTween = _item.rectTransform.DOLocalMove(targetLocalPos, _duration).SetEase(Ease.OutQuad);
                _m_activeTweens.Add(moveTween);
                moveTween.OnComplete(() => _m_activeTweens.Remove(moveTween));
            }

            private bool _validateMoveResult()
            {
                NumMergeTileInfo[][] tileInfos = HotfixNPPlayer.instance.numMergeComponent.tileInfos;

                // _debugLogTilemap("CLIENT", false);
                // _debugLogTilemap("SERVER", true);

                foreach (Vector2Int gridPos in NumMergeComponent.allBoardPositions)
                {
                    NumMergeTileInfo tileInfo = tileInfos[gridPos.x][gridPos.y];
                    GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(gridPos);

                    if (tileInfo.isEmpty)
                    {
                        if (item != null)
                        {
                            Debug.LogError($"[MERGE] Validation failed at ({gridPos.x},{gridPos.y}): server empty but UI has item");
                            return false;
                        }
                    }
                    else
                    {
                        if (item == null)
                        {
                            // 这里不同没事，交给下一个 spaw state 去创建
                            continue;
                        }

                        if (item.blockRefObj?.level != tileInfo.level)
                        {
                            Debug.LogError($"[MERGE] Validation failed at ({gridPos.x},{gridPos.y}): level mismatch");
                            return false;
                        }
                    }
                }

                return true;
            }

            // /// <summary>
            // /// 输出当前棋盘状态用于调试
            // /// </summary>
            // /// <param name="_label">标签（CLIENT 或 SERVER）</param>
            // /// <param name="_useServerData">是否使用服务器数据</param>
            // private void _debugLogTilemap(string _label, bool _useServerData)
            // {
            //     System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //     sb.AppendLine($"[MERGE] {_label} Tilemap:");
            //     sb.AppendLine("     X=0  X=1  X=2  X=3");
            //
            //     NumMergeTileInfo[][] serverTileInfos = null;
            //     if (_useServerData)
            //         serverTileInfos = HotfixNPPlayer.instance.numMergeComponent.tileInfos;
            //
            //     // Y从3到0，这样输出的棋盘是上下正确的方向
            //     for (int y = 3; y >= 0; y--)
            //     {
            //         sb.Append($"Y={y}  ");
            //         for (int x = 0; x < 4; x++)
            //         {
            //             if (_useServerData && serverTileInfos != null)
            //             {
            //                 NumMergeTileInfo tileInfo = serverTileInfos[x][y];
            //                 if (tileInfo.isEmpty)
            //                     sb.Append("[  ] ");
            //                 else
            //                 {
            //                     string buffMark = tileInfo.hasBuff ? "*" : " ";
            //                     sb.Append($"[{tileInfo.level,2}{buffMark}]");
            //                 }
            //             }
            //             else
            //             {
            //                 GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(new Vector2Int(x, y));
            //                 if (item == null)
            //                     sb.Append("[  ] ");
            //                 else
            //                 {
            //                     string buffMark = " ";
            //                     sb.Append($"[{item.blockRefObj?.level ?? 0,2}{buffMark}]");
            //                 }
            //             }
            //         }
            //         sb.AppendLine();
            //     }
            //
            //     Debug.Log(sb.ToString());
            // }
        }
    }
}
