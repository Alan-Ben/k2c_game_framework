using System;
using System.Collections.Generic;
using ALPackage;
using Common.BagItemUseEnum;
using Common.BagItemUseObj;
using Common.ConsortEnum;
using Common.TravelEnum;
using Common.TravelObj;
using GS2GC.p015_ConsortOp;
using NPCommon;
using NPEnum;

namespace GOE
{
    public class TravelEventUtil
    {
        public static _ATravelEventInfo makeTravelEventInfo(Travel_Event _event)
        {
            if (_event == null)
                return null;

            return makeTravelEventInfo(_event.getInstanceId(), _event.getEventId(), _event.getPos());
        }

        public static _ATravelEventInfo makeTravelEventInfo(long _instanceId, long _eventId, long _posId)
        {
            TravelEventRefObj eventRefObj = GRefdataCoreMgr.instance.travelEventRefCore.getRef(_eventId);
            if (eventRefObj == null)
                return null;
            switch (eventRefObj.eEventType)
            {
                case ETravelEventType.REWARD:
                    return new TravelRewardEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.CONSORT_LIKE:
                    return new TravelConsortLikeEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.CONSORT_INTIMACY:
                    return new TravelConsortIntimacyEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.CONSORT_BAR:
                    return new TravelConsortBarEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.CHANGE:
                    return new TravelChangeEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.INVITATION:
                    return new TravelInvitationEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.GIFTDE:
                    return new TravelGiftdeEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.ADD_POWER:
                    return new TravelAddPowerEventInfo(_instanceId, eventRefObj, _posId);
                
                case ETravelEventType.GAMBLING:
                    return new TravelGambleEventInfo(_instanceId, eventRefObj, _posId);
                
                default:
                    Debug.LogError($"[TravelEventUtil getTravelEventInfo] 没写类型:{eventRefObj.eEventType} 的创建事件数据方法");
                    return null;
            }
        }

        /// <summary>
        /// 构建游历事件结果数据
        /// </summary>
        /// <returns></returns>
        public static _ITravelEventResultInfo makeTravelResultEventInfo(Travel_EventResult _serverTravelResult, long _oldEarnings)
        {
            if (_serverTravelResult == null)
                return null;
            TravelEventRefObj eventRefObj = GRefdataCoreMgr.instance.travelEventRefCore.getRef(_serverTravelResult.getEventId());
            if (eventRefObj == null)
                return null;
            
            switch (eventRefObj.eEventType)
            {
                case ETravelEventType.REWARD:
                    return new TravelRewardEventInfo(0, eventRefObj, 0).getCommonEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.CONSORT_LIKE:
                    return new TravelConsortLikeEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.CONSORT_INTIMACY:
                    return new TravelConsortIntimacyEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.CONSORT_BAR:
                    return new TravelConsortBarEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.CHANGE:
                    return new TravelChangeEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.INVITATION:
                    return new TravelInvitationEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.GIFTDE:
                    return new TravelGiftdeEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.ADD_POWER:
                    return new TravelAddPowerEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                case ETravelEventType.GAMBLING:
                    return new TravelGambleEventInfo(0, eventRefObj, 0).getEventResultInfo(_serverTravelResult, _oldEarnings);
                
                default:
                    Debug.LogError($"[TravelEventUtil makeTravelResultEventInfo] 没写类型:{eventRefObj.eEventType} 的创建事件结果数据方法");
                    return null;
            }
        }

        /// <summary>
        /// 尝试进行一件事件处理
        /// </summary>
        private static void _tryDealAEvent(bool _simpleDeal, Action _onDealDone, Action _onBreak = null)
        {
            // 若当前事件未处理完成
            if (NPPlayer.instance.travelComp.curDealEvent != null && !NPPlayer.instance.travelComp.curDealEvent.eventDealDone)
            {
                if (NPPlayer.instance.travelComp.curDealEvent.inDataLevelEventDone)//若当前事件在数据层面已处理完成
                {
                    // 设置事件已完成, 不返回, 继续处理下一个事件
                    NPPlayer.instance.travelComp.curDealEvent.setEventDealDone();
                }
                else//若当前事件在数据层面还未处理完成
                {
                    // 进行事件处理, 并返回
                    GGUIAddSceneTravelMain.instance.refreshSceneShowWnd(null);
                    NPPlayer.instance.travelComp.curDealEvent.dealEvent(_simpleDeal, ()=>
                    {
                        NPPlayer.instance.travelComp.clearCurDealEvent();//清除当前处理事件
                        GGUIAddSceneTravelMain.instance.refreshSceneShowWnd(null);
                        _onDealDone?.Invoke();
                    }, ()=>
                    {
                        NPPlayer.instance.travelComp.clearCurDealEvent();//清除当前处理事件
                        GGUIAddSceneTravelMain.instance.refreshSceneShowWnd(null);
                        _onBreak?.Invoke();
                    });
                    return;
                }
            }

            // 获取第一个可处理事件
            _ATravelEventInfo travelEventInfo = NPPlayer.instance.travelComp.eventList?.GetFirst();
            if (travelEventInfo == null)
            {
                _onDealDone?.Invoke();
                return;
            }

            NPPlayer.instance.travelComp.setCurDealEvent(travelEventInfo);//设置当前处理事件
            GGUIAddSceneTravelMain.instance.refreshSceneShowWnd(null);
            
            // 场景中聚焦到事件位置
            WinMsg.SendMsg(WinMsgType.TRAVEL_FOCUS_TO_POS, travelEventInfo.posId, 0);
            // 设置当前事件位置为停靠位置
            WinMsg.SendMsg(WinMsgType.TRAVEL_SET_PARKING_POS, travelEventInfo.posId);
            
            // 开始进行事件处理
            travelEventInfo.dealEvent(_simpleDeal, () =>
            {
                //自己手动移除下事件, 防止服务端没有推送导致的死循环
                NPPlayer.instance.travelComp.removeEvent(travelEventInfo);
                NPPlayer.instance.travelComp.clearCurDealEvent();//清除当前处理事件
                GGUIAddSceneTravelMain.instance.refreshSceneShowWnd(null);
                _onDealDone?.Invoke();
            }, ()=>
            {
                NPPlayer.instance.travelComp.clearCurDealEvent();//清除当前处理事件
                GGUIAddSceneTravelMain.instance.refreshSceneShowWnd(null);
                _onBreak?.Invoke();
            });
        }

        private static void _tryDealAllEvent(bool _simpleDeal, Action _onAllDealDone, Action _onBreak = null)
        {
            // 所有事件都处理完成
            if (NPPlayer.instance.travelComp.eventList.Count <= 0)
            {
                _onAllDealDone?.Invoke();
                return;
            }

            _tryDealAEvent(_simpleDeal, () =>
            {
                // 继续处理下一个事件
                _tryDealAllEvent(_simpleDeal, _onAllDealDone, _onBreak);
            }, _onBreak);
        }
        
        /// <summary>
        /// 尝试处理所有事件
        /// </summary>
        public static void tryDealAllEvent(bool _simpleDeal, Action _onAllDealDone, Action _onBreak = null)
        {
            _onAllDealDone += () =>
            {
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.ALL_TRAVEL_EVENT_DEAL_DONE);
            };
            
            _tryDealAllEvent(_simpleDeal, _onAllDealDone, _onBreak);
        }
    }
}