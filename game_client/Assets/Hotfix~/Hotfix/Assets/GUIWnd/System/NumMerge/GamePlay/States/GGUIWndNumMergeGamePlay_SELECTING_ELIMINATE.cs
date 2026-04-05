
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class GGUIWndNumMergeGamePlay
    {
        private class SelectingEliminateState : _AHotfixSimpleState<GameState>
        {
            [NotNull] private readonly GGUIWndNumMergeGamePlay _m_wnd;


            public SelectingEliminateState([NotNull] GGUIWndNumMergeGamePlay _wnd)
            {
                _m_wnd = _wnd;
            }


            public override GameState state { get { return GameState.SELECTING_ELIMINATE; } }


            protected override void _onEnter()
            {
                _m_wnd.hotfixWnd?.setEliminateModeShow(true);
            }
            protected override void _onExit()
            {
                _m_wnd.hotfixWnd?.setEliminateModeShow(false);
            }
            public override bool canEnterState(GameState _newState)
            {
                return true;
            }


            public void selectTile(Vector2Int _gridPos)
            {
                int serialize = enterSerialize;
                int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                HotfixNPPlayer.instance.numMergeComponent.reqNumMergeEliminate(_gridPos, (_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                    if (serialize != enterSerialize)
                        return;

                    if (!_isSuc)
                        return;
                    
                    // 成功，切到消除表现状态
                    _m_wnd._m_machine.changeState(new EliminateState(_m_wnd));
                });
            }
            public void cancelEliminateMode()
            {
                _m_wnd._m_machine.changeState(new IdleState(_m_wnd));
            }
        }
    }
}
