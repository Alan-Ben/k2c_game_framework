using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class InnViewMgr
    {
        private class NormalState // : _ASimpleState<InnViewMgrType> 曾经是一个状态机的一部分，需求变更，其它状态都不要了，留下这个注释当作纪念，其它状态都删掉了
        {
            [NotNull] private readonly InnViewMgr _m_viewMgr;
            [ItemNotNull, NotNull] private readonly List<InnGuestView> _m_guestViewList;
            [ItemNotNull, NotNull] private readonly List<InnGuestView> _m_leavingGuestViewList;
            [NotNull] private readonly InnServeCounterView _m_serveCounterView;
            private ALCommonEnableTaskController _m_tickTask;
            // 服务一个客人所需的时间
            private readonly float _m_serveTime;
            // 当前服务中的客人剩余时间
            private float _m_currentServeTime;

            private InnGuestView _m_servingGuest;
            
            
            public NormalState([NotNull] InnViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;
                _m_guestViewList = new List<InnGuestView>();
                _m_leavingGuestViewList = new List<InnGuestView>();
                _m_serveCounterView = new InnServeCounterView(_m_viewMgr);
                _m_serveTime = GRefdataCoreMgr.instance.npGeneral.inn_receive_cost_sec;
            }

            
            public void enter(Action _complete)
            {
                List<InnGuestInfo> guestList = _m_viewMgr._m_guestDataMgr.getGuestList();
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(guestList.Count + 1 + 1);
                stepCounter.regAllDoneDelegate(_complete);
                {
                    for (int i = 0; i < guestList.Count; i++)
                    {
                        InnGuestInfo guestInfo = guestList[i];
                        InnGuestView guestView = new InnGuestView(guestInfo, MainAdditionInnTDScene.instance.getGuestMoveSpeed());
                        guestView.queueOffset = MainAdditionInnTDScene.instance.getQueueOffset();
                        guestView.position = _getGuestPosition(guestView, i);
                        guestView.load(stepCounter.addDoneStepCount);
                        _m_guestViewList.Add(guestView);
                    }
                    
                    _m_serveCounterView.load(stepCounter.addDoneStepCount);
                }
                stepCounter.addDoneStepCount();

                _m_viewMgr._m_guestDataMgr.onGuestAdd += _addGuest;
                _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick);
                _m_currentServeTime = _m_serveTime;
            }
            // Pre-exit called before exit
            public void preExit()
            {
                _m_tickTask.setDisable();
            }
            public void exit()
            {
                _m_servingGuest = null;
                _m_tickTask.setDisable();
                
                _m_viewMgr._m_guestDataMgr.onGuestAdd -= _addGuest;
                foreach (InnGuestView view in _m_guestViewList)
                {
                    view.discard();
                }
                _m_guestViewList.Clear();
                
                foreach (InnGuestView view in _m_leavingGuestViewList)
                {
                    view.discard();
                }
                _m_leavingGuestViewList.Clear();
                _m_serveCounterView.discard();
            }


            private void _tick()
            {
                // Update movement for all guests in queue
                foreach (InnGuestView guestView in _m_guestViewList)
                    guestView.updateMovement();
                
                // Update movement for leaving guests
                _updateLeavingGuests();
                
                // Handle serving logic for the first guest in queue
                if (_m_guestViewList.Count > 0)
                {
                    InnGuestView firstGuest = _m_guestViewList[0];
                    if (!firstGuest.isMoving)
                    {
                        if (_m_currentServeTime > 0)
                        {
                            // Continue serving
                            _m_currentServeTime -= Time.deltaTime;
                            if (_m_servingGuest != firstGuest)
                            {
                                _m_servingGuest = firstGuest;
                                firstGuest.startServing();
                                _m_serveCounterView.startServing(firstGuest.guestInfo);
                            }
                        }
                        else
                        {
                            // Service complete - make guest leave
                            _makeGuestLeave(firstGuest);
                            // Update queue positions for remaining guests
                            _updateGuestQueuePositions();
                            // Reset serve time for next guest
                            _m_currentServeTime = _m_serveTime;
                        }
                    }
                }
            }
            private void _addGuest(InnGuestInfo _guestInfo)
            {
                InnGuestView guestView = new InnGuestView(_guestInfo, MainAdditionInnTDScene.instance.getGuestMoveSpeed());
                guestView.queueOffset = MainAdditionInnTDScene.instance.getQueueOffset();
                
                // Set spawn position
                Vector3 spawnPos = MainAdditionInnTDScene.instance.calculateSpawnPosition(_m_guestViewList.Count);
                guestView.position = spawnPos + guestView.queueOffset;
                
                // Calculate queue position
                int queueIndex = _m_guestViewList.Count;
                Vector3 queuePos = _getGuestPosition(guestView, queueIndex);
                
                // Load the guest view and move to queue
                guestView.load();
                guestView.moveToPosition(queuePos);
                
                _m_guestViewList.Add(guestView);
            }
            private void _updateGuestQueuePositions()
            {
                for (int i = 0; i < _m_guestViewList.Count; i++)
                {
                    InnGuestView guestView = _m_guestViewList[i];
                    Vector3 newQueuePos = _getGuestPosition(guestView, i);
                    guestView.moveToPosition(newQueuePos);
                }
            }
            private void _makeGuestLeave(InnGuestView _guestView)
            {
                if (_guestView == null)
                    return;
                
                // Remove from queue list
                _m_guestViewList.Remove(_guestView);
                // Move to leave position
                Vector3 leavePos = MainAdditionInnTDScene.instance.getQueuePosition(-1); // -1 for leave position
                _guestView.moveToPosition(leavePos);
                // Add to leaving list
                _m_leavingGuestViewList.Add(_guestView);
            }
            private void _updateLeavingGuests()
            {
                for (int i = _m_leavingGuestViewList.Count - 1; i >= 0; i--)
                {
                    InnGuestView leavingGuest = _m_leavingGuestViewList[i];
                    leavingGuest.updateMovement();
                    
                    // Check if guest has reached the leave position (not moving and at leave index)
                    if (!leavingGuest.isMoving)
                    {
                        // Guest has left - remove and discard
                        leavingGuest.discard();
                        _m_leavingGuestViewList.RemoveAt(i);
                    }
                }
            }
            private Vector3 _getGuestPosition(InnGuestView _guestView, int _index)
            {
                Vector3 result = MainAdditionInnTDScene.instance.getQueuePosition(_index);
                if (_index > 0 && _guestView != null)
                    result += _guestView.queueOffset;

                return result;
            }
        }
    }
}