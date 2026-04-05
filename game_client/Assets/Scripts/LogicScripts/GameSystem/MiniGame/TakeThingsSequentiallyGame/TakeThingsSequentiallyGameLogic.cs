using System;
using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame.TakeThingsSequentiallyGame;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class TakeThingsSequentiallyGameLogic : _AMiniGameLogic
    {
        private static TakeThingsSequentiallyGameLogic _g_instance;
        public static TakeThingsSequentiallyGameLogic instance { get { return _g_instance ??= new TakeThingsSequentiallyGameLogic(); } }
        
        [NotNull] private TakeThingsSequentiallyGameController _m_controller;
        [NotNull] private TakeThingsSequentiallyGameUnit _m_SeqGameUnit;
        // 状态机
        [NotNull] private readonly _TALSimpleStateMachine<ETakeThingsSequentiallyGameState> _m_stateMachine;
        
        private TakeThingsSequentiallyGameRefObj _m_rTakeThingsSequentiallyGameRefObj;//配表数据
        
        [NotNull] private Dictionary<long, TakeThingsSequentiallyGameThingUnit> _m_ThingUnitDic = new Dictionary<long, TakeThingsSequentiallyGameThingUnit>();//物品单元字典
        
        public TakeThingsSequentiallyGameLogic()
        {
            _m_controller = new TakeThingsSequentiallyGameController(this);
            _m_SeqGameUnit = new TakeThingsSequentiallyGameUnit(this, _m_controller);

            _m_stateMachine = new _TALSimpleStateMachine<ETakeThingsSequentiallyGameState>();
            _m_stateMachine.changeState(new TakeThingsSequentiallyNoneGame(this));
        }
        
        public override EMiniGameType eMiniGameType { get { return EMiniGameType.TAKE_THINGS_SEQUENTIALLY; } }
        [NotNull] public override _AMiniGameController controller { get { return _m_controller; } }
        [NotNull] public override _ANPBasicAddContainerUIScene uiScene { get { return GMainGUIAddSceneMiniGame.instance; } }
        public override _AMainAdditionMiniGameTDScene tdScene { get { return MainAdditionTakeThingsSequentiallyGameTDScene.instance; } }

        [NotNull] internal TakeThingsSequentiallyGameUnit sequentiallyGameUnit { get { return _m_SeqGameUnit; } }
        [NotNull] internal _TALSimpleStateMachine<ETakeThingsSequentiallyGameState> stateMachine { get { return _m_stateMachine; } }
        

        internal TakeThingsSequentiallyGameRefObj takeThingsSequentiallyGameRefObj { get { return _m_rTakeThingsSequentiallyGameRefObj; } }

        protected override void _startGameOpSub(Action _complete, Action _failed)
        {
            // 获取游戏配置
            _m_rTakeThingsSequentiallyGameRefObj = GRefdataCoreMgr.instance.takeThingsSequentiallyGameRefCore.getRef(subGameId);
            if (_m_rTakeThingsSequentiallyGameRefObj == null)
            {
                Debug.LogError($"[TakeThingsSequentiallyGameLogic _startGameOp] GameLogic 启动失败，因为找不到{eMiniGameType}游戏, id为:{subGameId}的TakeThingsSequentiallyGameRefObj配表数据");
                _failed?.Invoke();
                return;
            }
            
            _m_SeqGameUnit.preLoadAsset(() =>
            {
                _complete?.Invoke();
            }, () =>
            {
                Debug.LogError("[TakeThingsSequentiallyGameLogic _startGameOp] GameLogic 启动失败，因为 _m_SeqGameWndUnit 预加载失败");
                _failed?.Invoke();
            });
        }

        protected override void _onStartSub()
        {
            _m_SeqGameUnit.init();
            _m_SeqGameUnit.dealAllThingShow((_thingShow) =>
            {
                if (_thingShow == null)
                    return;

                if (_m_ThingUnitDic.ContainsKey(_thingShow.thingId))
                {
                    Debug.LogError($"[TakeThingsSequentiallyGameLogic _onStartSub] 重复添加物品:{_thingShow.thingId}, 请检查是否UI或场景配置重复物品");
                    return;
                }
                
                TakeThingsSequentiallyGameThingUnit thingUnit = new TakeThingsSequentiallyGameThingUnit(_thingShow, this, _m_controller);
                _m_ThingUnitDic[_thingShow.thingId] = thingUnit;
                addGameUnit(thingUnit);
            });
            
            _m_stateMachine.changeState(new TakeThingsSequentiallyIdleGame(this));//状态机切换为idle状态
        }

        protected override void _onStopSub()
        {
            foreach (TakeThingsSequentiallyGameThingUnit thingUnit in _m_ThingUnitDic.Values)
            {
                removeGameUnit(thingUnit);
            }
            _m_ThingUnitDic.Clear();
            
            _m_SeqGameUnit.discard();   
            _m_SeqGameUnit.discradPreLoadAsset();
            
            _m_stateMachine.changeState(new TakeThingsSequentiallyNoneGame(this));
        }

        protected override void _onTickSub(float _deltaTime)
        {
        }

        /// <summary>
        /// 取走物品
        /// </summary>
        /// <param name="_thingId"></param>
        internal void takeThing(long _thingId)
        {
            if (!_m_ThingUnitDic.TryGetValue(_thingId, out TakeThingsSequentiallyGameThingUnit thingUnit) || thingUnit == null)
            {
                Debug.LogError($"[TakeThingsSequentiallyGameLogic takeThing] 未找物品:{_thingId}的TakeThingsSequentiallyGameThingUnit, 不应该出现这种情况, 请检查是否哪里有逻辑问题");
                return;
            }

            if (!thingUnit.canTake)
            {
                Debug.LogError("[TakeThingsSequentiallyGameLogic takeThing] 物品的TakeThingsSequentiallyGameThingUnit状态为不可获取状态, 但是UI却请求取走该物品, 请检查是否哪里有逻辑问题");
                return;
            }
            
            // 取走物品
            thingUnit.takeThing();

            bool allThingTaken = true;//是否所有物品都被取走
            _dealAllThingUnit((_thingUnit) =>
            {
                if (_thingUnit == null)
                    return;

                _thingUnit.refreshThingState(); //刷新物品状态
                if (!_thingUnit.isTaken)
                    allThingTaken = false;
            });

            if (allThingTaken)//若所有物品都取走了
            {
                _m_stateMachine.changeState(new TakeThingsSequentiallySuccessGame(this));
            }
        }
        
        /// <summary>
        /// 物品是否已经被取走
        /// </summary>
        /// <param name="_thingId"></param>
        /// <returns></returns>
        internal bool thingIsTaken(long _thingId)
        {
            if (!_m_ThingUnitDic.TryGetValue(_thingId, out TakeThingsSequentiallyGameThingUnit thingUnit) || thingUnit == null)
            {
                Debug.LogError($"[TakeThingsSequentiallyGameLogic thingIsTaken] 未找物品:{_thingId}的TakeThingsSequentiallyGameThingUnit, 请检查是否配置错误");
                return false;
            }
            
            return thingUnit.isTaken;
        }

        private void _dealAllThingUnit(Action<TakeThingsSequentiallyGameThingUnit> _action)
        {
            if(_action == null)
                return;

            _m_ThingUnitDic.Values.ForEach(_action);
        }
    }
}