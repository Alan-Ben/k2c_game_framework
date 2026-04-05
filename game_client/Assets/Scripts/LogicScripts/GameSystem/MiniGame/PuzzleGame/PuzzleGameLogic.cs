using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class PuzzleGameLogic : _AMiniGameLogic
    {
        [NotNull] public static PuzzleGameLogic instance { get { return _g_instance ??= new PuzzleGameLogic(); } }
        private static PuzzleGameLogic _g_instance;
        
        private PuzzleGameRefObj _m_rPuzzleGameRefObj;//配表数据
        
        [NotNull] private PuzzleGameController _m_controller;//控制器
        [NotNull] private PuzzleGameMainWndUnit _m_PuzzleGameMainWndUnit;//游戏主窗口单位
        
        // 状态机
        [NotNull] private readonly _TALSimpleStateMachine<EPuzzleGameState> _m_stateMachine;

        [NotNull] private List<PuzzleGameMatchItemUnit> _m_lMatchItemUnitList = new List<PuzzleGameMatchItemUnit>();
        [NotNull] private List<PuzzleGameMatchItemBgUnit> _m_lMatchItemBgUnitList = new List<PuzzleGameMatchItemBgUnit>();
        
        public override EMiniGameType eMiniGameType { get { return EMiniGameType.PUZZLE; } }
        public PuzzleGameRefObj puzzleGameRefObj { get { return _m_rPuzzleGameRefObj; } }
        [NotNull] public override _AMiniGameController controller { get { return _m_controller; } }
        [NotNull] public override _ANPBasicAddContainerUIScene uiScene { get { return GMainGUIAddSceneMiniGame.instance; } }
        public override _AMainAdditionMiniGameTDScene tdScene { get { return null; } }

        [NotNull] internal PuzzleGameMainWndUnit puzzleGameMainWndUnit { get { return _m_PuzzleGameMainWndUnit; } }

        [NotNull] internal _TALSimpleStateMachine<EPuzzleGameState> stateMachine { get { return _m_stateMachine; } }

        protected PuzzleGameLogic()
        {
            _m_controller = new PuzzleGameController(this);
            _m_PuzzleGameMainWndUnit = new PuzzleGameMainWndUnit(this, _m_controller);
            
            // 初始化状态机
            _m_stateMachine = new _TALSimpleStateMachine<EPuzzleGameState>();
            _m_stateMachine.changeState(new PuzzleGameNoneState(this));
        }
        
        protected override void _startGameOpSub(Action _complete, Action _failed)
        {
            // 获取游戏配置
            _m_rPuzzleGameRefObj = GRefdataCoreMgr.instance.puzzleGameRefCore.getRef(subGameId);
            if (_m_rPuzzleGameRefObj == null)
            {
                Debug.LogError($"[PuzzleGameLogic _startGameOp] GameLogic 启动失败，因为找不到{eMiniGameType}游戏, id为:{subGameId}的PuzzleGameRefObj配表数据");
                _failed?.Invoke();
                return;
            }

            _m_PuzzleGameMainWndUnit.preLoadWnd(() =>
            {
                _complete?.Invoke();
            }, () =>
            {
                Debug.LogError("[PuzzleGameLogic _startGameOp] GameLogic 启动失败，因为 _m_PuzzleGameMainWndUnit 预加载失败");
                _failed?.Invoke();
            });
        }

        protected override void _onStartSub()
        {
            _m_PuzzleGameMainWndUnit.init();
            
            _m_PuzzleGameMainWndUnit.dealAllMatchItem((_itemWnd) =>
            {
                if (_itemWnd == null)
                    return false;
                
                PuzzleGameMatchItemUnit matchItemUnit = new PuzzleGameMatchItemUnit(this, _m_controller, _itemWnd);
                _m_lMatchItemUnitList.Add(matchItemUnit);
                addGameUnit(matchItemUnit);

                return false;
            });
            
            _m_PuzzleGameMainWndUnit.dealAllMatchItemBg((_itemBgWnd) =>
            {
                if (_itemBgWnd == null)
                    return false;
                
                PuzzleGameMatchItemBgUnit matchItemBgUnit = new PuzzleGameMatchItemBgUnit(this, _m_controller, _itemBgWnd);
                _m_lMatchItemBgUnitList.Add(matchItemBgUnit);
                addGameUnit(matchItemBgUnit);

                return false;
            });
            
            _m_stateMachine.onStateChg += _onGameStateChg;
            _m_stateMachine.changeState(new PuzzleGameIdleState(this));
        }

        protected override void _onStopSub()
        {
            _m_stateMachine.onStateChg -= _onGameStateChg;
            _m_stateMachine.changeState(new PuzzleGameNoneState(this));
            
            foreach (var matchItemUnit in _m_lMatchItemUnitList)
            {
                if(matchItemUnit != null)
                    removeGameUnit(matchItemUnit);
            }
            _m_lMatchItemUnitList.Clear();
            
            foreach (var matchItemBgUnit in _m_lMatchItemBgUnitList)
            {
                if(matchItemBgUnit != null)
                    removeGameUnit(matchItemBgUnit);
            }
            _m_lMatchItemBgUnitList.Clear();

            _m_PuzzleGameMainWndUnit.discard();
            _m_PuzzleGameMainWndUnit.discardPreLoadWnd();
        }

        protected override void _onTickSub(float _deltaTime)
        {
        }
        
        private void _onGameStateChg(EPuzzleGameState _lastState, EPuzzleGameState _nextState)
        {
        }

        internal bool findMatchItemUnitList(long _matchItemId, out PuzzleGameMatchItemUnit _matchItemUnit)
        {
            _matchItemUnit = null;
            foreach (var itemUnit in _m_lMatchItemUnitList)
            {
                if (itemUnit == null)
                    continue;

                if (itemUnit.matchItemId == _matchItemId)
                {
                    _matchItemUnit = itemUnit;
                    return true;
                }
            }

            return false;
        }
        
        internal bool findMatchItemBgUnitList(long _matchItemBgId, out PuzzleGameMatchItemBgUnit _matchItemBgUnit)
        {
            _matchItemBgUnit = null;
            foreach (var itemBgUnit in _m_lMatchItemBgUnitList)
            {
                if (itemBgUnit == null)
                    continue;

                if (itemBgUnit.matchItemId == _matchItemBgId)
                {
                    _matchItemBgUnit = itemBgUnit;
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// 检查游戏是否结束
        /// </summary>
        internal void checkGameSuccess()
        {
            foreach (var matchItemUnit in _m_lMatchItemUnitList)
            {
                if (matchItemUnit == null)
                    continue;

                if (matchItemUnit.stateMachine.curState.state != EPuzzleGameMatchItemState.MATCH_SUCCESS)
                    return;
            }
            
            _m_stateMachine.changeState(new PuzzleGameSuccessState(this));
        }
        
        /// <summary>
        /// 当匹配item鼠标被点下时
        /// </summary>
        internal void onMatchItemPointDown(GGUIWndPuzzleGameMatchItem _itemWnd)
        {
            if(_itemWnd == null || !findMatchItemUnitList(_itemWnd.matchItemId, out PuzzleGameMatchItemUnit matchItemUnit) || matchItemUnit == null)
                return;
            
            matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemIdleState(matchItemUnit));
            matchItemUnit.moveTransformToLast();
        }
        
        /// <summary>
        /// 当匹配Item被拖动时
        /// </summary>
        /// <param name="_itemWnd"></param>
        internal void onMatchItemDrag(GGUIWndPuzzleGameMatchItem _itemWnd)
        {
            if(_itemWnd == null || !findMatchItemUnitList(_itemWnd.matchItemId, out PuzzleGameMatchItemUnit matchItemUnit) || matchItemUnit == null)
                return;
            
            _m_stateMachine.changeState(new PuzzleGameWaitingMatchState(this));//切换到等待匹配状态
            
            PuzzleGameMatchItemBgUnit matchItemBgUnit = null;//当前匹配的matchItemBgUnit
            foreach (var itemBgUnit in _m_lMatchItemBgUnitList)
            {
                if(itemBgUnit == null || itemBgUnit.itemState == EPuzzleGameMatchItemBgState.MATCH_SUCCESS)
                    continue;

                if (matchItemBgUnit != null)//若已经找到匹配的itemBgUnit, 剩余的itemBgUnit都不算相交
                {
                    itemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgNoItemWaitingMatchState(itemBgUnit));//其余背景item进入等待匹配状态, 已经匹配成功的不会变化状态，因为已经成功的状态只能进入None状态
                    continue;
                }

                // 若相交
                if (matchItemUnit.matchRectTransform.Overlaps(itemBgUnit.matchRectTransform))
                {
                    matchItemBgUnit = itemBgUnit;
                }
                else//若不相交, 其余背景item进入等待匹配状态, 已经匹配成功的不会变化状态，因为已经成功的状态只能进入None状态
                {
                    itemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgNoItemWaitingMatchState(itemBgUnit));
                }
            }

            if (matchItemBgUnit != null)//若当前有匹配项
            {
                if (matchItemBgUnit.matchItemId == matchItemUnit.matchItemId)//若匹配项相同
                {
                    matchItemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgRightItemWaitingMatchState(matchItemBgUnit));
                    matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemRightItemWaitingMatchState(matchItemUnit));
                }
                else//若匹配项不相同
                {
                    matchItemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgWrongItemWaitingMatchState(matchItemBgUnit));
                    matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemWrongItemWaitingMatchState(matchItemUnit));
                }
            }
            else//若当前没有匹配项
            {
                matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemNoItemWaitingMatchState(matchItemUnit));
            }
        }

        /// <summary>
        /// 当匹配Item被放下时
        /// </summary>
        /// <param name="_itemWnd"></param>
        internal void onMatchItemPointUp(GGUIWndPuzzleGameMatchItem _itemWnd)
        {
            if(_itemWnd == null || !findMatchItemUnitList(_itemWnd.matchItemId, out PuzzleGameMatchItemUnit matchItemUnit) || matchItemUnit == null)
                return;
            
            PuzzleGameMatchItemBgUnit matchItemBgUnit = null;//当前匹配的matchItemBgUnit
            foreach (var itemBgUnit in _m_lMatchItemBgUnitList)
            {
                if(itemBgUnit == null || itemBgUnit.itemState == EPuzzleGameMatchItemBgState.MATCH_SUCCESS)
                    continue;

                if (matchItemBgUnit != null)//若已经找到匹配的itemBgUnit, 剩余的itemBgUnit都不算相交
                {
                    itemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgNoMatchResumeState(itemBgUnit));//其余背景item进入没匹配的恢复状态, 已经匹配成功的不会变化状态，因为已经成功的状态只能进入None状态
                    continue;
                }

                // 若相交
                if (matchItemUnit.matchRectTransform.Overlaps(itemBgUnit.matchRectTransform))
                {
                    matchItemBgUnit = itemBgUnit;
                }
                else//若不相交
                {
                    itemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgNoMatchResumeState(itemBgUnit));//其余背景item进入没匹配的恢复状态, 已经匹配成功的不会变化状态，因为已经成功的状态只能进入None状态
                }
            }

            if (matchItemBgUnit != null)//若当前有匹配项
            {
                if (matchItemBgUnit.matchItemId == matchItemUnit.matchItemId)//若匹配项相同
                {
                    matchItemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgMatchSuccessState(matchItemBgUnit));
                    matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemMatchSuccessState(matchItemUnit), matchItemBgUnit.itemUIWorldPos);
                    
                    _m_stateMachine.changeState(new PuzzleGameMatchSuccessState(this));//游戏进入有item匹配成功状态
                }
                else//若匹配项不相同
                {
                    matchItemBgUnit.stateMachine.changeState(new PuzzleGameMatchItemBgMatchFailResumeState(matchItemBgUnit));
                    matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemMatchFailResumeState(matchItemUnit), matchItemBgUnit.itemUIWorldPos);
                    
                    _m_stateMachine.changeState(new PuzzleGameMatchFailResumeState(this));//游戏进入有item匹配失败状态
                }
            }
            else//若当前没有匹配项
            {
                matchItemUnit.stateMachine.changeState(new PuzzleGameMatchItemNoMatchResumeState(matchItemUnit));//没匹配项时重置
                
                _m_stateMachine.changeState(new PuzzleGameMatchFailResumeState(this));//游戏进入有item匹配失败状态
            }
        }
    }
}