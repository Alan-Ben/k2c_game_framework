using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 游戏逻辑中的表现部分, 只有表现, 不涉及对数据操作
    /// </summary>
    public partial class TileMatchGameLogic
    {
        [NotNull] private GGUIWndTileMatchGamePlay _m_gameShow;

        /// <summary>
        /// 注册方块表现接口
        /// </summary>
        /// <param name="_blockShowRefObj"></param>
        private GGUIWndTileMatchChecker _popBlockShow(TileMatchBlockShowRefObj _blockShowRefObj)
        {
            if (!isRunning || _blockShowRefObj == null)
                return null;

            GGUIWndTileMatchChecker blockShow = _m_gameShow.popTileBlockShow(_blockShowRefObj);
            if (blockShow != null)
            {
                blockShow.onPress += _onCheckerPress;
                blockShow.onTriggerDragMove += _onCheckerDragMove;
            }

            return blockShow;
        }

        private void _pushBackBlockShow(GGUIWndTileMatchChecker _blockShow)
        {
            if(_blockShow == null)
                return;
            
            _blockShow.hideWnd();
            _blockShow.onPress -= _onCheckerPress;
            _blockShow.onTriggerDragMove -= _onCheckerDragMove;

            _m_gameShow.pushBackTileBlockShow(_blockShow);
        }

        /// <summary>
        /// 当棋子被点击时
        /// </summary>
        private void _onCheckerPress(GGUIWndTileMatchChecker _checkerWnd)
        {
            if(_checkerWnd == null || !_checkCurStateCanOp())
                return;
            
            TileMatchBlockInfo blockInfo = _getBlockInfoFormArray(_checkerWnd.checkerLogicPos);
            if(blockInfo == null)
                return;
            
            _m_stateMachine.curState.doPress(blockInfo);
        }

        /// <summary>
        /// 当棋子被拖动时
        /// </summary>
        private void _onCheckerDragMove(GGUIWndTileMatchChecker _checkerWnd, ETileMatchDirection _moveDir)
        {
            if(_checkerWnd == null || !_checkCurStateCanOp())
                return;
            
            TileMatchBlockInfo startBlockInfo = _getBlockInfoFormArray(_checkerWnd.checkerLogicPos);
            if(startBlockInfo == null)
                return;

            TileMatchBlockInfo endBlockInfo = _getSpecifiedAdjoinedBlockInfo(_checkerWnd.checkerLogicPos, _moveDir);
            if(endBlockInfo == null)
                return;
            
            _m_stateMachine.setState((TileMatchGameStateMachineState_Switch _state) =>
            {
                _state?.init(startBlockInfo, endBlockInfo);
            });
        }

        /// <summary>
        /// 播放交换格子的音效
        /// </summary>
        private void _playExchangeBlockAudio()
        {
            if(_m_gameConfig.exchangeBlockAudioId <= 0)
                return;
            
            PlayAudioMgr.instance.playClip(_m_gameConfig.exchangeBlockAudioId);
        }
    }
}