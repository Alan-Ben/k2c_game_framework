using System;
using Common.EventEnum;
using Common.EventObj;
using JetBrains.Annotations;

namespace GOE
{
    public class CommonSimpleEventAgentFactory
    {
        public static _ACommonSimpleEventAgent createCommonSimpleEventAgent(long _eventId, byte[] _eventShowInfo, _ICommonSimpleEventDealExtOp _eventDealExtOp)
        {
            CommonEventRefObj eventRefObj = GRefdataCoreMgr.instance.commonEventRefCore.getRef(_eventId);
            if (eventRefObj == null || eventRefObj.eventInstanceSubRefObj == null)
            {
                Debug.LogError($"[CommonSimpleEventAgentFactory createCommonSimpleEventAgent] 创建对象失败, 找不到事件:{_eventId}对应的common_event配表数据或子表数据, 无法确定事件类型");
                return null;
            }

            switch (eventRefObj.eventInstanceSubRefObj.eventType)
            {
                case ECommonEventType.AWARD:
                    return CommonSimpleAwardEventAgent.createEventAgent(_eventId, _eventShowInfo, eventRefObj, _eventDealExtOp);
                
                case ECommonEventType.CHOICE:
                    return CommonSimpleChoiceEventAgent.createEventAgent(_eventId, _eventShowInfo, eventRefObj, _eventDealExtOp);
                
                case ECommonEventType.DIALOG:
                    return CommonSimpleDialogEventAgent.createEventAgent(_eventId, _eventShowInfo, eventRefObj, _eventDealExtOp);
                
                case ECommonEventType.DISPATCH:
                    return CommonSimpleDispatchEventAgent.createEventAgent(_eventId, _eventShowInfo, eventRefObj, _eventDealExtOp);

                case ECommonEventType.PLOT_DIALOG:
                    return CommonSimplePlotDialogEventAgent.createEventAgent(_eventId, _eventShowInfo, eventRefObj, _eventDealExtOp);

                case ECommonEventType.MINI_GAME:
                    return CommonSimpleMiniGameEventAgent.createEventAgent(_eventId, _eventShowInfo, eventRefObj, _eventDealExtOp);
                
                default:
                    Debug.LogError($"[CommonSimpleEventAgentFactory createCommonSimpleEventAgent] 创建对象失败, 没有事件类型:{eventRefObj.eventInstanceSubRefObj.eventType}Agent对象的创建内容");
                    return null;
            }
        }

        #region 获取每种不同事件的_IEventDealAddInfo

        /// <summary>
        /// 获取ECommonEventType.AWARD 事件的_IEventDealAddInfo
        /// </summary>
        /// <returns></returns>
        public static _IEventDealAddInfo getAwardEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult = null)
        {
            return CommonSimpleAwardEventAgent.getEventDealAddInfo(_showResult);
        }
        
        /// <summary>
        /// 获取ECommonEventType.CHOICE 事件的_IEventDealAddInfo
        /// </summary>
        /// <param name="_showResult"></param>
        /// <returns></returns>
        public static _IEventDealAddInfo getChoiceEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult = null)
        {
            return CommonSimpleChoiceEventAgent.getEventDealAddInfo(_showResult);
        }
        
        /// <summary>
        /// 获取ECommonEventType.DIALOG 事件的_IEventDealAddInfo
        /// </summary>
        /// <param name="_showResult"></param>
        /// <returns></returns>
        public static _IEventDealAddInfo getDialogEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult = null)
        {
            return CommonSimpleDialogEventAgent.getEventDealAddInfo(_showResult);
        }
        
        /// <summary>
        /// 获取ECommonEventType.DISPATCH 事件的_IEventDealAddInfo
        /// </summary>
        /// <param name="_showResult"></param>
        /// <returns></returns>
        public static _IEventDealAddInfo getDispatchEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult = null)
        {
            return CommonSimpleDispatchEventAgent.getEventDealAddInfo(_showResult);
        }
        
        /// <summary>
        /// 获取ECommonEventType.PLOT_DIALOG 事件的_IEventDealAddInfo
        /// </summary>
        /// <param name="_showResult"></param>
        /// <returns></returns>
        public static _IEventDealAddInfo getPlotDialogEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult = null)
        {
            return CommonSimplePlotDialogEventAgent.getEventDealAddInfo(_showResult);
        }
        
        /// <summary>
        /// 获取ECommonEventType.MINI_GAME 事件的_IEventDealAddInfo
        /// </summary>
        /// <param name="_showResult"></param>
        /// <returns></returns>
        public static _IEventDealAddInfo getMiniGameEventDealAddInfo(Action<CommonEventShowResultInfo, Action> _showResult = null)
        {
            return CommonSimpleMiniGameEventAgent.getEventDealAddInfo(_showResult);
        }

        #endregion
    }
}