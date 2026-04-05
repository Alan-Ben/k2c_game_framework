
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class GGUIWndNumMergeGamePlay
    {
        private class EliminateState : _AHotfixSimpleState<GameState>
        {
            [NotNull] private readonly GGUIWndNumMergeGamePlay _m_wnd;


            public EliminateState([NotNull] GGUIWndNumMergeGamePlay _wnd)
            {
                _m_wnd = _wnd;
            }


            public override GameState state { get { return GameState.ELIMINATE; } }


            protected override void _onEnter()
            {
                // 把服务端数据中不存在的 item 都播放 destroy 动画，然后动画结束后切回 IDLE 状态
                int serialize = enterSerialize;

                // 获取服务器数据
                NumMergeTileInfo[][] tileInfos = HotfixNPPlayer.instance.numMergeComponent.tileInfos;

                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(1);
                stepCounter.regAllDoneDelegate(() =>
                {
                    if (serialize != enterSerialize)
                        return;

                    // 消除完成，转到 IDLE 状态
                    _m_wnd._m_machine.changeState(new IdleState(_m_wnd));
                });

                // 遍历所有位置，销毁 UI 有但服务器没有的棋子
                foreach (Vector2Int gridPos in NumMergeComponent.allBoardPositions)
                {
                    NumMergeTileInfo tileInfo = tileInfos[gridPos.x][gridPos.y];
                    GGUIWndNumMergeGamePlayBlockItem item = _m_wnd._getItem(gridPos);

                    // UI 有这个格子，但服务器没有 - 需要销毁
                    if (item != null && tileInfo.isEmpty)
                    {
                        stepCounter.chgTotalStepCount(1);
                        item.playDestroyAnim(() =>
                        {
                            if (serialize != enterSerialize)
                            {
                                stepCounter.addDoneStepCount();
                                return;
                            }

                            _m_wnd._destroyItem(gridPos);
                            stepCounter.addDoneStepCount();
                        });
                    }
                }

                stepCounter.addDoneStepCount();
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
