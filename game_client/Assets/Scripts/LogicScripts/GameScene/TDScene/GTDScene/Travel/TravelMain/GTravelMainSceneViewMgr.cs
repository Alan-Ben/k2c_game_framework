using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    
    /// <summary>
    /// 游历主场景管理类
    /// </summary>
    public class GTravelMainSceneViewMgr : _AALBasicLoadObj
    {
        [NotNull] private Dictionary<long, GTravelMainPosView> _m_dTravelPosDic = new Dictionary<long, GTravelMainPosView>();//地点列表
        private GTravelMainCityView _m_mainCityView;//主城显示体
        private GTravelAircraftView _m_aircraftView;//飞机显示体

        private _ITravelParkable _m_iCurParking;//当前停靠的地点
        
        public _ITravelParkable curParking { get { return _m_iCurParking; } }
        
        protected override void _loadOp()
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_setLoadDone);
            stepCounter.chgTotalStepCount(1);
            
            // 加载游历地点
            GRefdataCoreMgr.instance.travelPosCore.dealAllRef((_travelPosRefObj) =>
            {
                if(_travelPosRefObj == null)
                    return;

                GTDTravelMainPosItemInfo posItemInfo = MainAddtionTravelMainTDScene.instance.getTravelPosInfo(_travelPosRefObj.id);
                GTravelMainPosView posView = new GTravelMainPosView(_travelPosRefObj, posItemInfo?.loadParent);
                _m_dTravelPosDic[_travelPosRefObj.id] = posView;
                stepCounter.chgTotalStepCount(1);
                posView.load(() =>
                {
                    stepCounter.addDoneStepCount();
                });
            });
            
            // 加载主城地点
            stepCounter.chgTotalStepCount(1);
            _m_mainCityView = new GTravelMainCityView(NPPlayer.instance.roomSkinComp.currentRoomSkinRefObj, MainAddtionTravelMainTDScene.instance.getRoomPosTrans());
            _m_mainCityView.load(() =>
            {
                stepCounter.addDoneStepCount();
            });
            
            // 加载飞机
            _m_aircraftView = MainAddtionTravelMainTDScene.instance.createTravelAircraftView();
            if (_m_aircraftView != null)
            {
                stepCounter.chgTotalStepCount(1);
                _m_aircraftView.load(() =>
                {
                    stepCounter.addDoneStepCount();
                });
            }
            
            stepCounter.addDoneStepCount();
        }

        protected override void _discard()
        {
            foreach (var posView in _m_dTravelPosDic.Values)
            {
                if(posView != null)
                    posView.discard();
            }
            _m_dTravelPosDic.Clear();
            
            _m_mainCityView?.discard();
            _m_mainCityView = null;

            _m_aircraftView?.discard();
            _m_aircraftView = null;
            
            _m_iCurParking = null;
        }

        public void refreshAllView()
        {
            foreach (var posView in _m_dTravelPosDic.Values)
            {
                posView?.refreshView();
            }
            
            _m_mainCityView?.refreshView();
        }

        public void showAll()
        {
            foreach (var posView in _m_dTravelPosDic.Values)
            {
                posView?.show();
            }
            
            _m_mainCityView?.show();
            
            WinMsg.RegisterMsg(WinMsgType.TRAVEL_SET_PARKING_POS, _msgSetParkingPos);
        }
        
        /// <summary>
        /// 隐藏所有
        /// </summary>
        public void hideAll()
        {
            WinMsg.UnregisterMsg(WinMsgType.TRAVEL_SET_PARKING_POS, _msgSetParkingPos);
            
            foreach (var posView in _m_dTravelPosDic.Values)
            {
                posView?.hide();
            }
            
            _m_mainCityView?.hide();
            
            _m_aircraftView?.hide();
        }

        /// <summary>
        /// 设置在主城停靠
        /// </summary>
        public void setParkingMainCity(bool _needPlayAnimation, Action _onSetDone = null)
        {
            _m_iCurParking?.setIsParking(false, false, null);
            _m_iCurParking = _m_mainCityView;
            if (_m_iCurParking != null)
            {
                _m_iCurParking.setIsParking(true, _needPlayAnimation, _onSetDone);
            }
            else
            {
                _onSetDone?.Invoke();
            }
        }

        /// <summary>
        /// 设置在某个地点停靠
        /// </summary>
        /// <param name="_posId"></param>
        public void setParkingPos(long _posId, bool _needPlayAnimation, Action _onSetDone = null)
        {
            _m_iCurParking?.setIsParking(false, false, null);
            if (!_m_dTravelPosDic.TryGetValue(_posId, out var posView))
            {
                _onSetDone?.Invoke();
                return;
            }

            _m_iCurParking = posView;
            if (_m_iCurParking != null)
            {
                _m_iCurParking.setIsParking(true, _needPlayAnimation, _onSetDone);
            }
            else
            {
                _onSetDone?.Invoke();
            }
        }
        
        /// <summary>
        /// 飞机飞到某个地点
        /// </summary>
        /// <param name="_posId"></param>
        /// <param name="_complete"></param>
        public void aircraftFlyToPos(long _posId, Action _complete = null)
        {
            // 若找不到飞行目标，直接回调飞行完成
            if (!_m_dTravelPosDic.TryGetValue(_posId, out GTravelMainPosView targetPosView) || targetPosView == null)
            {
                _complete?.Invoke();
                return;
            }

            if (targetPosView == _m_iCurParking)
            {
                // 已经停靠在目标地点，播放重复游历动画，动画完成后设置停靠状态
                targetPosView.showRepeatTravel(() =>
                {
                    _m_iCurParking?.setIsParking(true, true, _complete);
                });
                return;
            }

            if (_m_aircraftView == null)//若不存在飞机View，直接在目标地点设置停靠状态
            {
                _m_iCurParking?.setIsParking(false, false, null);
                _m_iCurParking = targetPosView;
                _m_iCurParking.setIsParking(true, true, _complete);
            }
            else if (_m_iCurParking == null)// 若当前停靠的地点不存在, 直接在目标地点设置停靠状态
            {
                _m_iCurParking = targetPosView;
                _m_iCurParking.setIsParking(true, true, _complete);
            }
            else // 当前停靠的地点存在
            {
                // 先取消当前停靠地点的停靠状态，飞行到目标地点，飞行完成后设置目标地点的停靠状态
                _m_iCurParking.setIsParking(false, false, null);
                _m_aircraftView.fly(_m_iCurParking.getAircraftParkingPos(), targetPosView.getAircraftParkingPos(), () =>
                {
                    _m_iCurParking = targetPosView;
                    _m_iCurParking.setIsParking(true, true, _complete);
                });
            }
        }
        
        
        /// <summary>
        /// 首次进入表现
        /// </summary>
        /// <param name="_onShowDone"></param>
        public void showFirstEnter(Action _onShowDone = null)
        {
            if (_m_mainCityView == null)
            {
                _onShowDone?.Invoke();
            }
            else
            {
                _m_mainCityView.showFirstEnter(_onShowDone);
            }
        }

        /// <summary>
        /// 进行游历地点解锁表现
        /// </summary>
        public void showPosUnlock(long _posId, Action _onShowDone)
        {
            if (!_m_dTravelPosDic.TryGetValue(_posId, out GTravelMainPosView posView) || posView == null)
            {
                _onShowDone?.Invoke();
                return;
            }
            
            posView.showUnlock(_onShowDone);
        }
        
        #region 消息监听

        /// <summary>
        /// 设置停靠在某个地点
        /// </summary>
        private void _msgSetParkingPos(params object[] _params)
        {
            if(_params == null || _params.Length < 1 || !(_params[0] is long _posId))
                return;
            
            bool needPlayAnimation = false;
            if(_params.Length > 1 && _params[1] is bool _needPlay)
                needPlayAnimation = _needPlay;
            
            setParkingPos(_posId, needPlayAnimation);
        }

        #endregion
    }
}