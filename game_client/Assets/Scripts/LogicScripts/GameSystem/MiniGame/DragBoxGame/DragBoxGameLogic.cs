using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class DragBoxGameLogic : _AMiniGameLogic
    {
        private static DragBoxGameLogic _g_instance;
        public static DragBoxGameLogic instance { get { return _g_instance ??= new DragBoxGameLogic(); } }
        
        [NotNull] private DragBoxGameController _m_controller;
        [NotNull] private DragBoxGameUnit _m_dragBoxGameUnit;
        [NotNull] private List<DragBoxGameBoxUnit> _m_lBoxUnitList = new List<DragBoxGameBoxUnit>();//物品单元列表
        private DragBoxGamePlayerUnit _m_playerCuteActorUnit;//玩家单元
        
        private DragBoxGameRefObj _m_rDragBoxRefObj;//配表数据
        private int _m_iBoxMatchedCount;//匹配成功的box数量

        public DragBoxGameLogic()
        {
            _m_controller = new DragBoxGameController(this);
            _m_dragBoxGameUnit = new DragBoxGameUnit(this, _m_controller);
        }
        
        public override EMiniGameType eMiniGameType { get { return EMiniGameType.DRAG_BOX; } }
        public override _AMiniGameController controller { get { return _m_controller; } }
        [NotNull] public override _ANPBasicAddContainerUIScene uiScene { get { return GMainGUIAddSceneMiniGame.instance; } }
        [NotNull] public override _AMainAdditionMiniGameTDScene tdScene { get { return MainAdditionDragBoxGameTDScene.instance; } }
        
        [NotNull] internal DragBoxGameUnit dragBoxGameUnit { get { return _m_dragBoxGameUnit; } }


        public DragBoxGameRefObj dragBoxGameRefObj { get { return _m_rDragBoxRefObj; } }

        protected override void _startGameOpSub(Action _complete, Action _failed)
        {
            // 获取游戏配置
            _m_rDragBoxRefObj = GRefdataCoreMgr.instance.dragBoxGameRefCore.getRef(subGameId);
            if (_m_rDragBoxRefObj == null)
            {
                Debug.LogError($"[DragBoxGameLogic _startGameOp] GameLogic 启动失败，因为找不到{eMiniGameType}游戏, id为:{subGameId}的DragBoxGameRefObj配表数据");
                _failed?.Invoke();
                return;
            }
            
            _m_dragBoxGameUnit.preLoadAsset(() =>
            {
                _complete?.Invoke();
            }, () =>
            {
                Debug.LogError("[DragBoxGameLogic _startGameOp] GameLogic 启动失败，因为 _m_dragBoxGameUnit 预加载失败");
                _failed?.Invoke();
            });
        }

        protected override void _onStartSub()
        {
            _m_dragBoxGameUnit.init();
            _m_dragBoxGameUnit.dealAllBoxShow((_boxShow) =>
            {
                if (_boxShow == null)
                    return;
                
                DragBoxGameBoxUnit boxUnit = new DragBoxGameBoxUnit(_boxShow, _onBoxMatched, this, _m_controller);
                _m_lBoxUnitList.Add(boxUnit);
                addGameUnit(boxUnit);
            });

            _m_iBoxMatchedCount = 0;
            
            _m_playerCuteActorUnit = new DragBoxGamePlayerUnit(this, _m_controller, _m_dragBoxGameUnit.playerCuteActorMono);
            addGameUnit(_m_playerCuteActorUnit);
        }

        protected override void _onStopSub()
        {
            foreach (DragBoxGameBoxUnit boxUnit in _m_lBoxUnitList)
            {
                removeGameUnit(boxUnit);
            }
            _m_lBoxUnitList.Clear();
            
            removeGameUnit(_m_playerCuteActorUnit);
            _m_playerCuteActorUnit = null;
            
            _m_dragBoxGameUnit.discard();   
            _m_dragBoxGameUnit.discradPreLoadAsset();
            
            _m_iBoxMatchedCount = 0;
        }

        protected override void _onTickSub(float _deltaTime)
        {
        }
        
        private void _onBoxMatched(DragBoxGameBoxUnit _boxUnit)
        {
            
        }
    }
}