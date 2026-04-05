using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    public class EveningDungeonGameCurAirshipShowInfo
    {
        public long loadSerialize;//飞船加载的序列号
        
        public long curShowHeroId;//当前展示飞船的大臣id
        public GGUIWndEveningDungeonGameAirshipActor curUsingAirshipActor;//当前使用的飞船Actor

        /// <summary>
        /// 飞船当前状态
        /// </summary>
        public EEveningDungeonGameAirshipState curAirshipState;
        
        /// <summary>
        /// 飞船状态的序列号
        /// </summary>
        public long stateSerialize;
        
        /// <summary>
        /// 飞船当前状态变化时回调
        /// </summary>
        public Action<EEveningDungeonGameAirshipState, EEveningDungeonGameAirshipState, long> onAirshipStateChg;
    }
    
    public class GGUIWndEveningDungeonGameAirshipShow : _ANPGGUIBasicSubWnd<GGUIMonoEveningDungeonGameAirshipShow>
    {
        private GGUIWndEveningDungeonGameAirshipActorMgr<EQuality> _m_airshipActorMgr;//飞船Actor管理器
        
        private long _m_lAirShipLoadSerialize;//飞船加载的序列号
        public EveningDungeonGameCurAirshipShowInfo _m_curAirshipShowInfo;
        
        public GGUIWndEveningDungeonGameAirshipShow(GGUIMonoEveningDungeonGameAirshipShow _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long curShowHeroId { get { return _m_curAirshipShowInfo?.curShowHeroId ?? 0; } }
        public EEveningDungeonGameAirshipState curAirshipState { get { return _m_curAirshipShowInfo?.curAirshipState ?? EEveningDungeonGameAirshipState.NONE; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.airshipActorMgr != null)
                _m_airshipActorMgr = new GGUIWndEveningDungeonGameAirshipActorMgr<EQuality>(1, 2, wnd.airshipActorMgr);
        }
        
        protected override void _onDiscard()
        {
            if (_m_airshipActorMgr != null)
            {
                _m_airshipActorMgr.pushbackAirshipActor(_m_curAirshipShowInfo?.curUsingAirshipActor);
                _m_airshipActorMgr.discard();
            }
            _m_airshipActorMgr = null;

            _m_curAirshipShowInfo = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lAirShipLoadSerialize = ALSerializeOpMgr.next();
            if (_m_curAirshipShowInfo != null)
            {
                _m_curAirshipShowInfo.stateSerialize = ALSerializeOpMgr.next();

                _m_airshipActorMgr?.pushbackAirshipActor(_m_curAirshipShowInfo.curUsingAirshipActor);
            }
            _m_curAirshipShowInfo = null;
        }

        protected override void _onReset()
        {
            _m_airshipActorMgr?.pushbackAirshipActor(_m_curAirshipShowInfo?.curUsingAirshipActor);
            
            _m_curAirshipShowInfo = null;
        }

        public void showNoShip()
        {
            if(wnd == null)
                return;
            
            if(wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.noAirShipShowAnimName))
                wnd.wndAnimation.ForcePlay(wnd.noAirShipShowAnimName);
        }
        
        /// <summary>
        /// 飞船入场
        /// </summary>
        public void airshipEntry(long _heroId, Action _onEntryDone = null)
        {
            // 若当前有正在使用中的飞船
            if (_m_curAirshipShowInfo != null)
            {
                if (_heroId == _m_curAirshipShowInfo.curShowHeroId)
                {
                    if (_m_curAirshipShowInfo.curAirshipState == EEveningDungeonGameAirshipState.ENTRY)
                    {
                        long serialize = _m_curAirshipShowInfo.stateSerialize;
                        _m_curAirshipShowInfo.onAirshipStateChg += (preState, newState, beforeChgSerializeId) =>
                        {
                            if(serialize == beforeChgSerializeId)
                                _onEntryDone?.Invoke();
                        };
                    }
                    else
                    {
                        _onEntryDone?.Invoke();
                    }
                    
                    return;
                }
                
                // 先让当前飞船离场
                airshipDeparture();
                _m_curAirshipShowInfo = null;
            }

            if (_m_airshipActorMgr == null)
            {
                _onEntryDone?.Invoke();   
                return;
            }

            _m_curAirshipShowInfo = new EveningDungeonGameCurAirshipShowInfo();
            _m_curAirshipShowInfo.loadSerialize = _m_lAirShipLoadSerialize = ALSerializeOpMgr.next();
            _m_curAirshipShowInfo.curShowHeroId = _heroId;
            
            EQuality quality = GCommon.getItemQuality(ENPItemType.HERO, _heroId);
            _m_airshipActorMgr.popAirshipActor(quality, (airshipActor) =>
            {
                // 若飞船加载序列号变化,表示中途有新的飞船入场请求，直接回收该飞船
                if (_m_curAirshipShowInfo == null || _m_curAirshipShowInfo.loadSerialize != _m_lAirShipLoadSerialize)
                {
                    _m_airshipActorMgr?.pushbackAirshipActor(airshipActor);
                    return;
                }

                if (airshipActor == null)
                {
                    Debug.LogError($"[airshipEntry] 获取不到大臣:{_heroId} 品质:{quality} 的飞船Actor");
                    _onEntryDone?.Invoke();
                    return;
                }
                
                _m_curAirshipShowInfo.curUsingAirshipActor = airshipActor;
                _m_curAirshipShowInfo.curUsingAirshipActor.showWnd();
                long airShipStateSerialize = _setAirshipState(EEveningDungeonGameAirshipState.ENTRY);
                _m_curAirshipShowInfo.onAirshipStateChg += (preState, newState, beforeChgSerializeId) =>
                {
                    if(airShipStateSerialize == beforeChgSerializeId)
                        _onEntryDone?.Invoke();
                };
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(_m_curAirshipShowInfo == null || airShipStateSerialize != _m_curAirshipShowInfo.stateSerialize)
                        return;
                    
                    // 进入idle状态
                    _setAirshipState(EEveningDungeonGameAirshipState.IDLE);
                }, wnd == null ? 0 : wnd.airshipEntryStateTime);
            });
        }

        /// <summary>
        /// 飞船离场
        /// </summary>
        public void airshipDeparture(Action _onDepartureDone = null)
        {
            if (_m_curAirshipShowInfo == null)
            {
                _onDepartureDone?.Invoke();
                return;
            }
            
            _m_lAirShipLoadSerialize = ALSerializeOpMgr.next();//为了防止飞船入场还未加载完成时, 就调用了飞机离场的情况, 在离场时, 直接变更加载序列号, 让未加载完成的飞船加载回调失效
            EveningDungeonGameCurAirshipShowInfo airshipActorShowInfo = _m_curAirshipShowInfo;
            long stateSerialize = _setAirshipState(EEveningDungeonGameAirshipState.DEPARTURE);//进入离场状态
            // 开始离场后, 就直接将当前使用的飞船置空, 防止重复调用离场
            _m_curAirshipShowInfo = null;
            
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(stateSerialize != airshipActorShowInfo.stateSerialize)
                    return;
                
                _setAirshipState(airshipActorShowInfo, EEveningDungeonGameAirshipState.NONE);
                // 离场完成后回收
                _m_airshipActorMgr?.pushbackAirshipActor(airshipActorShowInfo.curUsingAirshipActor);
                
                _onDepartureDone?.Invoke();
            }, wnd == null ? 0 : wnd.airshipDepartureStateTime);
        }

        /// <summary>
        /// 飞船进行攻击
        /// </summary>
        /// <param name="_onAttackDone"></param>
        public void airshipAttack(Action _onAttackDone = null)
        {
            if (_m_curAirshipShowInfo == null)
            {
                _onAttackDone?.Invoke();
                return;
            }

            long stateSerialize = _setAirshipState(EEveningDungeonGameAirshipState.ATTACK);//进入攻击状态
            ALCommonTaskController.CommonActionAddMonoTask(()=>
            {
                // 若序列号变化,表示中途有新的状态变化请求,则不进行后续的状态设置
                if(_m_curAirshipShowInfo == null || stateSerialize != _m_curAirshipShowInfo.stateSerialize)
                    return;
                
                // 攻击状态持续一段时间后,回到idle状态
                _setAirshipState(EEveningDungeonGameAirshipState.IDLE);
                
                _onAttackDone?.Invoke();
            }, wnd != null ? wnd.airshipAttackStateTime : 0);
        }
        
        private long _setAirshipState(EEveningDungeonGameAirshipState _state)
        {
            if (_m_curAirshipShowInfo == null || _m_curAirshipShowInfo.curAirshipState == _state)
                return 0;
            
            if (wnd != null && wnd.airshipStateAniStatInfoList != null)
            {
                NPCommonEnumAniStatInfo<EEveningDungeonGameAirshipState>.setStat(wnd.airshipStateAniStatInfoList, _state);
            }
            long stateSerialize = _setAirshipState(_m_curAirshipShowInfo, _state);

            return stateSerialize;
        }

        private long _setAirshipState(EveningDungeonGameCurAirshipShowInfo _airshipShowInfo, EEveningDungeonGameAirshipState _state)
        {
            if (_airshipShowInfo == null || _airshipShowInfo.curAirshipState == _state)
                return 0;
            
            EEveningDungeonGameAirshipState preState = _airshipShowInfo.curAirshipState;
            _airshipShowInfo.curAirshipState = _state;
            long beforeChgSerialize = _airshipShowInfo.stateSerialize;
            long stateSerialize = _airshipShowInfo.stateSerialize = ALSerializeOpMgr.next();
            
            if (_airshipShowInfo.curUsingAirshipActor != null)
            {
                _airshipShowInfo.curUsingAirshipActor.setAirShipState(_state);
            }
            
            Action<EEveningDungeonGameAirshipState, EEveningDungeonGameAirshipState, long> onAirshipStateChg = _airshipShowInfo.onAirshipStateChg;
            _airshipShowInfo.onAirshipStateChg = null;
            onAirshipStateChg?.Invoke(preState, _state, beforeChgSerialize);

            return stateSerialize;
        }
    }
}