
using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using Hotfix.NumMergeEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hotfix
{
    /// <summary>
    /// 2048 游戏玩法子窗口
    /// </summary>
    public partial class GGUIWndNumMergeGamePlay : _AHotfixBaseSubWnd<GGUIMonoNumMergeGamePlay>
    {
        private enum GameState
        {
            IDLE, // 进入时检查是否 gameover，是就发送消息给服务端，然后 reset all，不是就等待用户输入
            SPAWN, // 移动完棋子之后生成新棋子（由服务端生成）
            MERGE, // 先移动棋子，然后融合，并发送消息给服务端，让服务端生成棋子，检查客户端是否融合正确，正确的话切到 spawn 状态，错误的话 reset_all
            SELECTING_ELIMINATE, // 选择要被消除的棋子
            ELIMINATE, // 棋子被消除中
            RESET_ALL, // 刷新整个棋盘（根据不同刷新类型播放动画）
        }


        [NotNull] private readonly _THotfixSimpleStateMachine<GameState> _m_machine;
        [NotNull] private readonly GGUIWndNumMergeGamePlayBlockItem[][] _m_blockItems; // 4x4棋子UI
        
        private NPGGUICommonTipDealerMgr _m_tipMgr; // 加分提示管理器
        private ItemCache _m_itemCache;

        private long _m_lastScore; // 上次的分数，用于计算加分
        private Vector2 _m_dragStartLocalPos; // 拖拽起始本地位置
        private bool _m_bDragHandled; // 拖拽是否已处理

#if UNITY_EDITOR
        private ALCommonEnableTaskController _m_tickTask;
#endif


        public GGUIWndNumMergeGamePlay(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            _m_machine = new _THotfixSimpleStateMachine<GameState>();
            _m_blockItems = new GGUIWndNumMergeGamePlayBlockItem[4][];
            for (int x = 0; x < 4; x++)
                _m_blockItems[x] = new GGUIWndNumMergeGamePlayBlockItem[4];

            initWnd();
        }
        public float itemAnimSpace { get { return hotfixWnd?.itemAnimSpace ?? 0; } }


        protected override void _onShowWnd()
        {
            _m_tipMgr?.start();

            refreshWnd();

            HotfixNPPlayer.instance.numMergeComponent.onScoreChg += refreshScore;

#if UNITY_EDITOR
            CommonTaskController.CommonEnableTickActionAddMonoTask(_tick);      
#endif
        }
        protected override void _onHideWnd()
        {
#if UNITY_EDITOR
            _m_tickTask.setDisable();
#endif
            HotfixNPPlayer.instance.numMergeComponent.onScoreChg -= refreshScore;

            _m_tipMgr?.clear();

            _m_machine.changeState(new HotfixNoneSimpleState<GameState>());
            _destroyAllItems();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            _m_tipMgr?.clear();
            _m_tipMgr = null;
            _m_itemCache?.discard();
            _m_itemCache = null;
            
            if (hotfixWnd == null)
                return;
            
            ALUGUICommon.uncombineBeginDrag(hotfixWnd.btnDragArea, _onBeginDrag);
            ALUGUICommon.uncombineDrag(hotfixWnd.btnDragArea, _onDrag);
            ALUGUICommon.uncombineEndDrag(hotfixWnd.btnDragArea, _onEndDrag);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnCancelEliminateMode, _onCancelEliminateModeClick);
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            // 初始化提示管理器
            if (hotfixWnd.transGameScoreAddTipParent != null)
                _m_tipMgr = new NPGGUICommonTipDealerMgr(hotfixWnd.transGameScoreAddTipParent);
            if (hotfixWnd.monoItemTemplate != null)
            {
                _m_itemCache = new ItemCache(hotfixWnd.transGameBoardParent, _onItemClick);
                _m_itemCache.init(hotfixWnd.monoItemTemplate);
            }
            
            ALUGUICommon.combineBeginDrag(hotfixWnd.btnDragArea, _onBeginDrag);
            ALUGUICommon.combineDrag(hotfixWnd.btnDragArea, _onDrag);
            ALUGUICommon.combineEndDrag(hotfixWnd.btnDragArea, _onEndDrag);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnCancelEliminateMode, _onCancelEliminateModeClick);
        }


        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            sampleGameOverAnim(1f);
            sampleBuffScoreAnim(0f);
            _m_machine.changeState(new ResetAllState(this), ResetType.NONE);
            _m_lastScore = -1;
            refreshScore(false);
        }
        /// <summary>
        /// 刷新得分显示
        /// </summary>
        public void refreshScore(bool _byBuff)
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            // 获取当前分数
            long currentScore = HotfixNPPlayer.instance.numMergeComponent.currentScore;
            long lastScore = _m_lastScore;

            // 记录当前分数
            _m_lastScore = currentScore;

            if (_byBuff)
                CommonTaskController.CommonActionAddMonoTask(refreshScoreDisplay, hotfixWnd.buffScoreAnimDelay);
            else
                refreshScoreDisplay();
            return;

            void refreshScoreDisplay()
            {
                if (_byBuff)
                    playBuffScoreAnim(null);
                
                // 更新分数显示
                ALUGUICommon.setLabelTxt(hotfixWnd.txtGameScore, currentScore);

                // 显示加分提示
                if (_m_tipMgr != null && lastScore >= 0 && currentScore > lastScore)
                {
                    NPCenterTipsRefObj tipRefObj = GRefdataCoreMgr.instance.tipMap.getRef(hotfixWnd.gameScoreAddTipId);
                    long addScore = currentScore - lastScore;
                    _m_tipMgr.addTip(new NPTextTipDealer(
                        new List<string> { TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, addScore) },
                        tipRefObj, null));
                }
            }
        }
        public void startEliminateMode()
        {
            if (_m_machine.curState is IdleState idleState)
                idleState.startEliminateMode();
        }
        public void doOrganize()
        {
            if (_m_machine.curState is IdleState idleState)
                idleState.doOrganize();
        }
        public void playGameOverAnim(Action _complete)
        {
            if (hotfixWnd?.behaviorAnim == null || string.IsNullOrEmpty(hotfixWnd.gameOverAnimName))
            {
                _complete?.Invoke();
                return;
            }

            hotfixWnd.behaviorAnim.ForcePlay(hotfixWnd.gameOverAnimName, 0, _complete);
        }
        public void sampleGameOverAnim(float _normalizedTime)
        {
            if (hotfixWnd?.behaviorAnim == null || string.IsNullOrEmpty(hotfixWnd.gameOverAnimName))
                return;
            
            hotfixWnd.behaviorAnim.Sample(hotfixWnd.gameOverAnimName, _normalizedTime);
        }
        public void playBuffScoreAnim(Action _complete)
        {
            if (hotfixWnd?.buffScoreAnim == null || string.IsNullOrEmpty(hotfixWnd.buffScoreAnimName))
            {
                _complete?.Invoke();
                return;
            }

            hotfixWnd.buffScoreAnim.ForcePlay(hotfixWnd.buffScoreAnimName, 0, _complete);
        }
        public void sampleBuffScoreAnim(float _normalizedTime)
        {
            if (hotfixWnd?.buffScoreAnim == null || string.IsNullOrEmpty(hotfixWnd.buffScoreAnimName))
                return;
            
            hotfixWnd.buffScoreAnim.Sample(hotfixWnd.buffScoreAnimName, _normalizedTime);
        }


        private GGUIWndNumMergeGamePlayBlockItem _getItem(Vector2Int _gridPos)
        {
            return _m_blockItems[_gridPos.x][_gridPos.y];
        }
        private void _createItem(Vector2Int _gridPos, NumMergeBlockRefObj _blockRefObj, int _buffStep)
        {
            if (_m_blockItems[_gridPos.x][_gridPos.y] != null)
            {
                UnityEngine.Debug.LogError("Try to create an item at an occupied grid position!");
                return;
            }
            
            GGUIWndNumMergeGamePlayBlockItem blockItem = _m_itemCache?.popItem();
            if (blockItem == null)
                return;
            
            blockItem.refreshRef(_blockRefObj);
            blockItem.refreshBuff(_buffStep);
            blockItem.gridPos = _gridPos;
            blockItem.localPosition = _calculateGridPosition(_gridPos);
            _m_blockItems[_gridPos.x][_gridPos.y] = blockItem;
        }
        private void _destroyItem(GGUIWndNumMergeGamePlayBlockItem _item)
        {
            if (_item == null)
                return;

            if (_m_blockItems[_item.gridPos.x][_item.gridPos.y] != _item)
            {
                UnityEngine.Debug.LogError("Try to destroy an item that is not in the grid!");
                return;
            }
            
            _m_blockItems[_item.gridPos.x][_item.gridPos.y] = null;
            _m_itemCache?.pushBackCacheItem(_item);
        }
        private void _destroyItem(Vector2Int _gridPos)
        {
            GGUIWndNumMergeGamePlayBlockItem item = _m_blockItems[_gridPos.x][_gridPos.y];
            if (item == null)
                return;
            
            _m_blockItems[_gridPos.x][_gridPos.y] = null;
            _m_itemCache?.pushBackCacheItem(item);
        }
        /// <summary>
        /// 释放 Item 出 grid 管理
        /// </summary>
        private GGUIWndNumMergeGamePlayBlockItem _freeItem(Vector2Int _gridPos)
        {
            GGUIWndNumMergeGamePlayBlockItem item = _m_blockItems[_gridPos.x][_gridPos.y];
            if (item == null)
                return null;
            
            _m_blockItems[_gridPos.x][_gridPos.y] = null;
            return item;
        }
        /// <summary>
        /// 设置 Item 到 grid 管理
        /// </summary>
        private bool _setItem(Vector2Int _gridPos, GGUIWndNumMergeGamePlayBlockItem _item)
        {
            if (_item == null)
                return false;
            
            if (_m_blockItems[_gridPos.x][_gridPos.y] != null)
            {
                UnityEngine.Debug.LogError("Try to set an item at an occupied grid position!");
                return false;
            }

            _m_blockItems[_gridPos.x][_gridPos.y] = _item;
            _item.gridPos = _gridPos;
            return true;
        }
        private void _destroyAllItems()
        {
            foreach (Vector2Int gridPos in NumMergeComponent.allBoardPositions)
                _destroyItem(gridPos);
        }
        private Vector3 _calculateGridPosition(Vector2Int _gridPos)
        {
            if (hotfixWnd?.transGameBoardParent == null)
                return Vector3.zero;

            float cellSize = hotfixWnd.gameBoardCellSize;
            return new Vector3(_gridPos.x * cellSize, _gridPos.y * cellSize, 0f);
        }

        private void _onItemClick(Vector2Int _gridPos)
        {
            if (_m_machine.curState is SelectingEliminateState selectingEliminateState)
                selectingEliminateState.selectTile(_gridPos);
        }
        private void _onBeginDrag(PointerEventData _obj)
        {
            if (hotfixWnd?.btnDragArea == null)
                return;

            RectTransform rectTransform = hotfixWnd.btnDragArea.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, _obj.position, _obj.pressEventCamera, out _m_dragStartLocalPos);
            }
            _m_bDragHandled = false;
        }
        private void _onDrag(PointerEventData _obj)
        {
            if (_m_bDragHandled)
                return;

            if (hotfixWnd?.btnDragArea == null)
                return;

            RectTransform rectTransform = hotfixWnd.btnDragArea.GetComponent<RectTransform>();
            if (rectTransform == null)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, _obj.position, _obj.pressEventCamera, out Vector2 currentLocalPos);

            // 计算拖拽距离
            Vector2 delta = currentLocalPos - _m_dragStartLocalPos;

            // 距离太小，不处理
            if (delta.sqrMagnitude < hotfixWnd.dragThreshold * hotfixWnd.dragThreshold)
                return;

            _m_bDragHandled = true;
            
            // 确定拖拽方向
            ENumMerge_MoveDir dragDir;
            if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                // 横向拖拽
                dragDir = delta.x > 0 ? ENumMerge_MoveDir.RIGHT : ENumMerge_MoveDir.LEFT;
            else
                // 纵向拖拽
                dragDir = delta.y > 0 ? ENumMerge_MoveDir.UP : ENumMerge_MoveDir.DOWN;
            
            // 调用状态机处理移动
            if (_m_machine.curState is IdleState idleState)
            {
                // 检查 CD 是否足够
                ENumMerge_ModeType modeType = HotfixAccountSettingMgr.instance.hotfixAccountSetting.getNumMergeModeType();
                NumMergeModeRefObj modeRef = HotfixRefdataCoreMgr.instance.numMergeModeRefCore.getRef((int)modeType);
                if (modeRef == null || !GCommon.isItemEnough(ENPItemType.LAZY_CD, HotfixRefdataCoreMgr.instance.numMergeOtherRefObj.num_merge_lazy_cd_id, modeRef.consume_cd, true))
                    return;
                
                idleState.moveItem(dragDir);
            }
        }
        private void _onEndDrag(PointerEventData _obj)
        {
        }
        private void _onCancelEliminateModeClick(GameObject _obj)
        {
            if (_m_machine.curState is SelectingEliminateState selectingEliminateState)
                selectingEliminateState.cancelEliminateMode();
        }
#if UNITY_EDITOR
        private void _tick()
        {
            // 判断是否在 game 界面，不在就不响应了
            if (QueueMgr.instance._lastNode.nodeTag != HotfixUINodeTagConst.NUMMERGE_GAME)
                return;
            
            ENumMerge_MoveDir dir = ENumMerge_MoveDir.NONE;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                dir = ENumMerge_MoveDir.UP;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                dir = ENumMerge_MoveDir.DOWN;
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                dir = ENumMerge_MoveDir.LEFT;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                dir = ENumMerge_MoveDir.RIGHT;

            if (dir != ENumMerge_MoveDir.NONE)
            {
                if (_m_machine.curState is IdleState idleState)
                    idleState.moveItem(dir);
            }
        }
#endif
    }
}
