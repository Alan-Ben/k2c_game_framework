
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class GGUIWndNumMergeGamePlay
    {
        private class SpawnState : _AHotfixSimpleState<GameState>
        {
            [NotNull] private readonly GGUIWndNumMergeGamePlay _m_wnd;


            public SpawnState([NotNull] GGUIWndNumMergeGamePlay _wnd)
            {
                _m_wnd = _wnd;
            }


            public override GameState state { get { return GameState.SPAWN; } }


            protected override void _onEnter()
            {
                // 获取服务器数据
                NumMergeTileInfo[][] tileInfos = HotfixNPPlayer.instance.numMergeComponent.tileInfos;

                int spawnCount = 0;

                // 遍历所有位置，创建服务器有但 UI 没有的棋子
                foreach (Vector2Int gridPos in NumMergeComponent.allBoardPositions)
                {
                    NumMergeTileInfo tileInfo = tileInfos[gridPos.x][gridPos.y];
                    GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(gridPos);

                    // 服务器有这个格子，但 UI 没有 - 需要创建新棋子
                    if (!tileInfo.isEmpty && item == null)
                    {
                        NumMergeBlockRefObj blockRefObj = HotfixRefdataCoreMgr.instance.numMergeBlockRefCore.getRef(tileInfo.level);
                        _m_wnd._createItem(gridPos, blockRefObj, 0);
                        GGUIWndNumMergeGamePlayBlockItem newItem = _m_wnd._getItem(gridPos);

                        if (newItem != null)
                        {
                            newItem.playSpawnAnim(null, _m_wnd.itemAnimSpace * spawnCount++);
                        }
                    }
                }

                // 刷新全部棋子的 buff
                foreach (Vector2Int gridPos in NumMergeComponent.allBoardPositions)
                {
                    NumMergeTileInfo tileInfo = tileInfos[gridPos.x][gridPos.y];
                    GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(gridPos);
                    item?.refreshBuff(tileInfo.buffStep);
                }

                // 不等待动画结束，直接转到 IDLE 状态
                _m_wnd._m_machine.changeState(new IdleState(_m_wnd));
            }
            protected override void _onExit()
            {
            }
            public override bool canEnterState(GameState _newState)
            {
                return true;
            }
        }
    }
}
