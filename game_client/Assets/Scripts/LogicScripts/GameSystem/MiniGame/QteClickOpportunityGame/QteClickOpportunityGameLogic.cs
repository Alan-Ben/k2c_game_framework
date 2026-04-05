using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public class QteClickOpportunityGameLogic : _AMiniGameLogic
    {
        private static QteClickOpportunityGameLogic _g_instance;
        public static QteClickOpportunityGameLogic instance { get { return _g_instance ??= new QteClickOpportunityGameLogic(); } }
        
        [NotNull] private QteClickOpportunityGameController _m_controller;
        [NotNull] private QteClickOpportunityGameUnit _m_QteClickOpportunityGameUnit;
        // 状态机
        [NotNull] private readonly _TALSimpleStateMachine<EQteClickOpportunityGameState> _m_stateMachine;
        
        private QteClickOpportunityGameRefObj _m_rQteClickOpportunityGameRefObj;//配表数据

        [NotNull]private List<QteClickOpportunityGameItemUnit> _m_lQteClickOpportunityGameItemUnitList = new List<QteClickOpportunityGameItemUnit>();
        private int _m_iHasTriggeredItemCount = 0;//已经触发的item数量
        private int _m_iSuccessItemCount = 0;//成功的item数量

        private const int _m_iGameSuccessNeedItemSuccessCount = 0;//游戏成功需要item成功的数量

        public QteClickOpportunityGameLogic()
        {
            _m_controller = new QteClickOpportunityGameController(this);
            _m_QteClickOpportunityGameUnit = new QteClickOpportunityGameUnit(this, _m_controller);

            _m_stateMachine = new _TALSimpleStateMachine<EQteClickOpportunityGameState>();
            _m_stateMachine.changeState(new QteClickOpportunityGameNoneState(this));
        }
        
        public override EMiniGameType eMiniGameType { get { return EMiniGameType.QTE_CLICK_OPPORTUNITY_GAME; } }
        [NotNull] public override _AMiniGameController controller { get { return _m_controller; } }
        public override _ANPBasicAddContainerUIScene uiScene { get { return null; } }
        public override _AMainAdditionMiniGameTDScene tdScene { get { return null; } }
        
        [NotNull] internal QteClickOpportunityGameUnit qteClickOpportunityGameUnit { get { return _m_QteClickOpportunityGameUnit; } }
        [NotNull] internal _TALSimpleStateMachine<EQteClickOpportunityGameState> stateMachine { get { return _m_stateMachine; } }
        
        internal QteClickOpportunityGameRefObj qteClickOpportunityGameRefObj { get { return _m_rQteClickOpportunityGameRefObj; } }
        
        protected override void _startGameOpSub(Action _complete, Action _failed)
        {
            // 获取游戏配置
            _m_rQteClickOpportunityGameRefObj = GRefdataCoreMgr.instance.qteClickOpportunityGameRefCore.getRef(subGameId);
            if (_m_rQteClickOpportunityGameRefObj == null)
            {
                Debug.LogError($"[QteClickOpportunityGameLogic _startGameOp] GameLogic 启动失败，因为找不到{eMiniGameType}游戏, id为:{subGameId}的QteClickOpportunityGameRefObj配表数据");
                _failed?.Invoke();
                return;
            }
            
            _m_QteClickOpportunityGameUnit.preLoadAsset(() =>
            {
                _complete?.Invoke();
            }, () =>
            {
                Debug.LogError("[QteClickOpportunityGameLogic _startGameOp] GameLogic 启动失败，因为 _m_QteClickOpportunityGameUnit 预加载失败");
                _failed?.Invoke();
            });
        }

        protected override void _onStartSub()
        {
            _m_iHasTriggeredItemCount = 0;
            _m_iSuccessItemCount = 0;
            _m_QteClickOpportunityGameUnit.init();
            _m_QteClickOpportunityGameUnit.dealAllItemShow((_itemShow) =>
            {
                if (_itemShow == null)
                    return;

                QteClickOpportunityGameItemUnit itemUnit = new QteClickOpportunityGameItemUnit(_itemShow, _onItemTrigger, this, _m_controller);
                _m_lQteClickOpportunityGameItemUnitList.Add(itemUnit);
                addGameUnit(itemUnit);
            });
            
            _m_stateMachine.changeState(new QteClickOpportunityGamePlayingState(this));//状态机切换为idle状态
        }

        protected override void _onStopSub()
        {
            foreach (QteClickOpportunityGameItemUnit itemUnit in _m_lQteClickOpportunityGameItemUnitList)
            {
                removeGameUnit(itemUnit);
            }
            _m_lQteClickOpportunityGameItemUnitList.Clear();
            
            _m_QteClickOpportunityGameUnit.discard();   
            _m_QteClickOpportunityGameUnit.discradPreLoadAsset();
            
            _m_stateMachine.changeState(new QteClickOpportunityGameNoneState(this));
        }

        protected override void _onTickSub(float _deltaTime)
        {
        }

        private void _onItemTrigger(QteClickOpportunityGameItemUnit _itemUnit)
        {
            if(_itemUnit == null || !_itemUnit.isTrigger)
                return;
            
            _m_iHasTriggeredItemCount++;
            if (_itemUnit.isSuccess)
                _m_iSuccessItemCount++;

            if (_m_iHasTriggeredItemCount >= _m_lQteClickOpportunityGameItemUnitList.Count)
            {
                if (_m_iSuccessItemCount >= _m_iGameSuccessNeedItemSuccessCount)
                {
                    _m_stateMachine.changeState(new QteClickOpportunityGameSuccessState(this));
                }
                else
                    _m_stateMachine.changeState(new QteClickOpportunityGameFailState(this));
            }
        }
    }
}