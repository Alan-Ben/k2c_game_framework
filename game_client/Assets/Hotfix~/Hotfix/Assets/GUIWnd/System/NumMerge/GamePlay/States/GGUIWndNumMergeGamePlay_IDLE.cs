
using ALPackage;
using GOE;
using Hotfix.NumMergeEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class GGUIWndNumMergeGamePlay
    {
        private class IdleState : _AHotfixSimpleState<GameState>
        {
            [NotNull] private readonly GGUIWndNumMergeGamePlay _m_wnd;


            public IdleState([NotNull] GGUIWndNumMergeGamePlay _wnd)
            {
                _m_wnd = _wnd;
            }


            public override GameState state { get { return GameState.IDLE; } }


            protected override void _onEnter()
            {
            }
            protected override void _onExit()
            {
            }
            public override bool canEnterState(GameState _newState)
            {
                return true;
            }


            private bool _checkGameOver()
            {
                NumMergeTileInfo[][] tileInfos = HotfixNPPlayer.instance.numMergeComponent.tileInfos;
                foreach (Vector2Int gridPos in NumMergeComponent.allBoardPositions)
                {
                    NumMergeTileInfo tileInfo = tileInfos[gridPos.x][gridPos.y];
                    if (tileInfo.isEmpty)
                        return false;

                    if (gridPos.x > 0 && tileInfos[gridPos.x - 1][gridPos.y].canMergeWith(tileInfo))
                        return false;
                    if (gridPos.x < 3 && tileInfos[gridPos.x + 1][gridPos.y].canMergeWith(tileInfo))
                        return false;
                    if (gridPos.y > 0 && tileInfos[gridPos.x][gridPos.y - 1].canMergeWith(tileInfo))
                        return false;
                    if (gridPos.y < 3 && tileInfos[gridPos.x][gridPos.y + 1].canMergeWith(tileInfo))
                        return false;
                }

                return true;
            }
            public void moveItem(ENumMerge_MoveDir _dragDir)
            {
                if (_checkGameOver())
                {
                    int serialize = enterSerialize;
                    NPMesMgr.instance.showTwoBtnMes(
                        TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_gameOverConfirm_desc),
                        TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_gameOverConfirm_useItem),
                        () => ALMsgSys.SendMsg(HotfixMsgType.SHOW_NUMMERGE_USE_ITEM_TIP),
                        TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_gameOverConfirm_restart),
                        () =>
                        {
                            if (serialize != enterSerialize)
                                return;

                            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                            HotfixNPPlayer.instance.numMergeComponent.reqNumMergeGameOver((_isSuc, _msg) =>
                            {
                                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                                if (serialize != enterSerialize)
                                    return;

                                if (_isSuc)
                                    _m_wnd._m_machine.changeState(new ResetAllState(_m_wnd), ResetType.GAME_OVER);
                            });
                        },
                        true,
                        HotfixTransKeyConst.numMerge_gameOverConfirm_title);

                    return;
                }
                _m_wnd._m_machine.changeState(new MergeState(_m_wnd), _dragDir);
            }
            public void doOrganize()
            {
                int serialize = enterSerialize;
                int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                HotfixNPPlayer.instance.numMergeComponent.reqNumMergeOrganize((_isSuc, _msg) =>
                {
                    MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                    if (serialize != enterSerialize)
                        return;

                    if (_isSuc)
                    {
                        _m_wnd._m_machine.changeState(new ResetAllState(_m_wnd), ResetType.ORGANIZE);
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_useRefreshItemSucc_tip));                        
                    }
                });
            }
            public void startEliminateMode()
            {
                _m_wnd._m_machine.changeState(new SelectingEliminateState(_m_wnd));
            }
        }
    }
}