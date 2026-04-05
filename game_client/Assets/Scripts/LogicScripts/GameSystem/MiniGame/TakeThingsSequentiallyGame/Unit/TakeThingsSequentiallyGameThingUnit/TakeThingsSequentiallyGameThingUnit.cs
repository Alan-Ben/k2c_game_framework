using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE.MiniGame
{
    public interface _ITakeThingsSequentiallyGameThingShow
    {
        /// <summary>
        /// 物品唯一id
        /// </summary>
        public long thingId { get; }

        /// <summary>
        /// 设置GameController
        /// </summary>
        /// <param name="_gameController"></param>
        public void setGameController(TakeThingsSequentiallyGameController _gameController);

        /// <summary>
        /// 设置物体状态
        /// </summary>
        /// <param name="_thingState"></param>
        public void setThingState(ETakeThingsSequentiallyGameThingState _thingState);

        /// <summary>
        /// 显示物体
        /// </summary>
        public void showThing();

        /// <summary>
        /// 隐藏物体
        /// </summary>
        public void hideThing();
        
        /// <summary>
        /// 物品的锁列表
        /// </summary>
        public List<TakeThingsSequentiallyGameThingLock> thingLockList { get; }
    }
    
    public class TakeThingsSequentiallyGameThingUnit : _ATakeThingsSequentiallyGameUnit
    {
        [NotNull] private _ITakeThingsSequentiallyGameThingShow _m_thingShow;//物品窗口
        [NotNull] private readonly _TALSimpleStateMachine<ETakeThingsSequentiallyGameThingState> _m_stateMachine;//状态机

        public TakeThingsSequentiallyGameThingUnit([NotNull] _ITakeThingsSequentiallyGameThingShow _thingShow, [NotNull] TakeThingsSequentiallyGameLogic _gameLogic, [NotNull] TakeThingsSequentiallyGameController _gameController) : base(_gameLogic, _gameController)
        {
            _m_thingShow = _thingShow;
            _m_stateMachine = new _TALSimpleStateMachine<ETakeThingsSequentiallyGameThingState>();
            _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingNoneState(this));
        }
        
        internal long thingId { get { return _m_thingShow.thingId; } }
        
        [NotNull] internal _ITakeThingsSequentiallyGameThingShow thingShow { get { return _m_thingShow; } }
        
        /// <summary>
        /// 是否被取走
        /// </summary>
        internal bool isTaken { get { return _m_stateMachine.curState.state == ETakeThingsSequentiallyGameThingState.SUCCESSFULLY_TAKEN; } }

        /// <summary>
        /// 是否可取
        /// </summary>
        internal bool canTake { get { return _m_stateMachine.curState.state == ETakeThingsSequentiallyGameThingState.TAKEABLE; } }

        public override void init()
        {
            _m_thingShow.setGameController(_m_gameController);
            _m_thingShow.showThing();

            bool thingCanTake = true;//物品是否可取
            if (_m_thingShow.thingLockList != null && _m_thingShow.thingLockList.Count > 0)
            {
                foreach (TakeThingsSequentiallyGameThingLock thingLock in _m_thingShow.thingLockList)
                {
                    // 若物品配置了lockList, 则刚初始化完成一定是不可取的
                    if (thingLock != null && thingLock.lockThings != null && thingLock.lockThings.Count > 0)
                    {
                        thingCanTake = false;
                        break;
                    }
                }
            }
            
            if (thingCanTake)
            {
                _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingTakeableState(this));
            }
            else
            {
                _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingNotTakeableState(this));
            }
        }

        public override void discard()
        {
            _m_thingShow.setGameController(null);
            _m_thingShow.hideThing();

            _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingNoneState(this));
        }

        /// <summary>
        /// 刷新物品状态
        /// </summary>
        internal void refreshThingState()
        {
            if (isTaken)//若物品已经被取走 则不再刷新状态
                return;
            
            bool thingCanTake = true;//物品是否可取
            if (_m_thingShow.thingLockList != null && _m_thingShow.thingLockList.Count > 0)
            {
                foreach (TakeThingsSequentiallyGameThingLock thingLocks in _m_thingShow.thingLockList)
                {
                    if (thingLocks != null && thingLocks.lockThings != null && thingLocks.lockThings.Count > 0)
                    {
                        bool allThingLockTaken = true;//是否所有锁住物品都已经取走
                        foreach (_AMonoTakeThingsSequentiallyGameThing item in thingLocks.lockThings)
                        {
                            if (item != null && !_m_gameLogic.thingIsTaken(item.thingId))
                            {
                                allThingLockTaken = false;
                                break;                                
                            }
                        }

                        if (allThingLockTaken)//若所有锁住物品都已经取走, 则该物品可取走
                        {
                            _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingTakeableState(this));
                            return;
                        }
                    }
                }

                // 若遍历完所有锁定物品列表还没有可取, 则为不可取状态
                _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingNotTakeableState(this));
            }
            else//若没有配置, 则可以取走
            {
                _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingTakeableState(this));
            }
        }

        /// <summary>
        /// 取走物品
        /// </summary>
        internal void takeThing()
        {
            _m_stateMachine.changeState(new TakeThingsSequentiallyGameThingSuccessfullyTakenState(this));
        }
    }
}