using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;
using Common.TravelObj;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public abstract class _ATravelEventInfo
    {
        private long _m_lInstanceId;
        private long _m_lEventId;
        private long _m_posId;
        
        private bool _m_bEventDealDone;//事件是否处理完成
        private bool _m_bEventDealIng;//事件是否正在处理
        
        private Action _m_aOnEventDealDone;//事件处理完成回调
        private Action _m_onEventBreakDeal;//事件处理中断回调

        private TravelEventRefObj _m_rTravelEventRefObj;
        private TravelPosRefObj _m_rTravelPosRefObj;
        private TravelEventTypeRefObj _m_rTravelEventTypeRefObj;
        
        public _ATravelEventInfo(Travel_Event _event)
        {
            if (null == _event)
                return;
            
            _m_lInstanceId = _event.getInstanceId();
            _m_lEventId = _event.getEventId();
            _m_posId = _event.getPos();
        }

        public _ATravelEventInfo(long _instanceId, long _eventId, long _posId)
        {
            _m_lInstanceId = _instanceId;
            _m_lEventId = _eventId;
            _m_posId = _posId;
        }

        public _ATravelEventInfo(long _instanceId, TravelEventRefObj _eventRefObj, long _posId)
        {
            _m_lInstanceId = _instanceId;
            _m_lEventId = _eventRefObj?.event_id ?? 0;
            _m_rTravelEventRefObj = _eventRefObj;
            _m_posId = _posId;
        }
        
        public long instanceId { get => _m_lInstanceId; }
        public long eventId { get => _m_lEventId; }
        public ETravelEventType eventType { get => travelEventRefObj?.eEventType ?? ETravelEventType.NONE; }
        public long posId { get => _m_posId; }
        
        /// <summary>
        /// 事件是否在数据层面已经完成
        /// </summary>
        public bool inDataLevelEventDone { get { return NPPlayer.instance.travelComp.getEventInfo(instanceId) == null; } }
        
        /// <summary>
        /// 事件是否在表现层面已经完成
        /// </summary>
        public bool eventDealDone { get => _m_bEventDealDone;}
        public bool eventDealIng { get => _m_bEventDealIng; }
        public TravelEventRefObj travelEventRefObj
        {
            get
            {
                if (_m_rTravelEventRefObj == null || _m_rTravelEventRefObj.event_id != _m_lEventId)
                    _m_rTravelEventRefObj = GRefdataCoreMgr.instance.travelEventRefCore.getRef(_m_lEventId);

                return _m_rTravelEventRefObj;
            }
        }

        public TravelPosRefObj travelPosRefObj
        {
            get
            {
                if (_m_rTravelPosRefObj == null || _m_rTravelPosRefObj.id != _m_posId)
                    _m_rTravelPosRefObj = GRefdataCoreMgr.instance.travelPosCore.getRef(_m_posId);

                return _m_rTravelPosRefObj;
            }
        }
        
        public TravelEventTypeRefObj travelEventTypeRefObj
        {
            get
            {
                if (_m_rTravelEventTypeRefObj == null || _m_rTravelEventTypeRefObj.event_type != eventType)
                    _m_rTravelEventTypeRefObj = GRefdataCoreMgr.instance.travelEventTypeRefCore.getRef((long)eventType);

                return _m_rTravelEventTypeRefObj;
            }
        }

        public override string ToString()
        {
            return $"[instanceId:{instanceId} eventId:{eventId} posId:{posId}]";
        }

        /// <summary>
        /// 更新事件id
        /// </summary>
        public void updateEventId(long _eventId)
        {
            _m_lEventId = _eventId;
        }
        
        /// <summary>
        /// 更新位置id
        /// </summary>
        /// <param name="_posId"></param>
        public void updatePosId(long _posId)
        {
            _m_posId = _posId;
        }
        
        /// <summary>
        /// 注册事件处理完成回调
        /// </summary>
        /// <param name="_action"></param>
        public void regEventDealDone(Action _action)
        {
            if(_action == null)
                return;

            if (_m_bEventDealDone)
            {
                _action();
                return;
            }
            
            if (_m_aOnEventDealDone != null)
                _m_aOnEventDealDone += _action;
            else
                _m_aOnEventDealDone = _action;
        }
        
        /// <summary>
        /// 取消注册事件处理完成回调
        /// </summary>
        /// <param name="_action"></param>
        public void unRegEventDealDone(Action _action)
        {
            if(_action == null)
                return;

            if (_m_aOnEventDealDone == null)
                return;

            _m_aOnEventDealDone -= _action;
        }

        /// <summary>
        /// 注册事件处理中断回调
        /// </summary>
        /// <param name="_action"></param>
        public void regEventBreakDeal(Action _action)
        {
            if(_action == null)
                return;

            if(_m_bEventDealDone)//若事件已处理完成, 则不再注册中断回调
                return;
            
            if (_m_onEventBreakDeal != null)
                _m_onEventBreakDeal += _action;
            else
                _m_onEventBreakDeal = _action;
        }
        
        /// <summary>
        /// 注销事件处理中断回调
        /// </summary>
        /// <param name="_action"></param>
        public void unRegEventBreakDeal(Action _action)
        {
            if(_action == null)
                return;

            if (_m_onEventBreakDeal == null)
                return;

            _m_onEventBreakDeal -= _action;
        }
        
        /// <summary>
        /// 处理事件
        /// </summary>
        public void dealEvent(bool _simpleDeal, Action _onEventDealDone = null, Action _onBreakDeal = null)
        {
            if (_m_bEventDealDone)
            {
                _onEventDealDone?.Invoke();
                return;
            }

            regEventDealDone(_onEventDealDone);
            regEventBreakDeal(_onBreakDeal);
            if (_m_bEventDealIng)
            {
                Debug.LogError_EditorOnly($"当前事件:{this} 正在处理中, 但是却再次调用了dealEvent方法, 请检查是否没在事件中断处理时调用breakEventDeal方法, 或没在事件处理完成时调用setEventDealDone方法");
                return;
            }
            
            _m_bEventDealIng = true;

            if(!_simpleDeal)
                _dealEvent();
            else
                _dealEventSimple();
        }
        
        /// <summary>
        /// 中断事件处理
        /// </summary>
        public void breakEventDeal()
        {
            _m_bEventDealIng = false;
            
            _m_aOnEventDealDone = null;
            
            Action breakAction = _m_onEventBreakDeal;
            _m_onEventBreakDeal = null;
            breakAction?.Invoke();
        }
        
        /// <summary>
        /// 设置事件处理完成
        /// </summary>
        public void setEventDealDone()
        {
            _m_bEventDealDone = true;
            _m_bEventDealIng = false;

            _m_onEventBreakDeal = null;
            
            Action action = _m_aOnEventDealDone;
            _m_aOnEventDealDone = null;
            action?.Invoke();
        }

        /// <summary>
        /// 事件主体角色
        /// </summary>
        /// <returns></returns>
        public virtual _ITravelEventRole getEventMainRole()
        {
            return _m_rTravelEventRefObj?.eventTarget?.roleInfo;
        }
        
        protected abstract void _dealEvent();

        /// <summary>
        /// 简化处理事件（去掉所有对话展示）
        /// </summary>
        protected abstract void _dealEventSimple();

        /// <summary>
        /// 事件CenterTip展示文本
        /// </summary>
        /// <returns></returns>
        public abstract string getEventCenterTipText();
        
        #region 一些展示方法

        /// <summary>
        /// 获取玩家经验
        /// </summary>
        /// <returns></returns>
        protected long getExp()
        {
            return GCommon.getItemCount(ENPItemType.CURRENCY, (long) ECurrency.P_EXP);
        }
        
        /// <summary>
        /// 获取玩家收益
        /// </summary>
        /// <returns></returns>
        protected long getEarnings()
        {
            return NPPlayer.instance.specialItemComp.goldData.earnings;
        }

        /// <summary>
        /// 展示事件对话
        /// </summary>
        protected void _showEventDialog(Action _done)
        {
            // 不存在对话需要展示
            if(travelEventRefObj == null || travelEventRefObj.lDialogList == null || travelEventRefObj.lDialogList.Count <= 0)
            {
                _done?.Invoke();
                return;
            }
             
            GCommon.enterDialogueNode(travelEventRefObj.lDialogList.GetRandomItem(), _done, true, false);
        }

        protected void _showEventResultWnd(Travel_EventResult _serverResultInfo, long _oldEarnings, Action<_ITravelResultWnd> onWndShow = null, Action _onClose = null)
        {
            // long addEarnings = getEarnings() - _oldEarnings;
            _ITravelResultWnd wnd = _getEventResultWnd(_serverResultInfo, _oldEarnings, out string nodeUITag);
            if (wnd == null || wnd.getBasicLoadUIWndBasicClass() == null)
            {
                _onClose?.Invoke();
                return;
            }
            
            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, nodeUITag, true
                , false, false, null, wnd.getBasicLoadUIWndBasicClass(), true, false,
                ()=> onWndShow?.Invoke(wnd), null, null, () =>
                {
                    wnd.discard();
                    wnd = null;
                    
                    _onClose?.Invoke();
                }));
        }

        protected virtual _ITravelResultWnd _getEventResultWnd(Travel_EventResult _serverResultInfo, long _oldEarnings, out string _nodeUITag)
        {
            _nodeUITag = UINodeTagConst.C_TRAVEL_EVENT_COMMON_RESULT;
                
            CommonTravelEventResultInfo resultInfo = getCommonEventResultInfo(_serverResultInfo, _oldEarnings);
            GGUIWndTravelCommonResult resultWnd = new GGUIWndTravelCommonResult(resultInfo);
            return resultWnd;
        }
        
        public CommonTravelEventResultInfo getCommonEventResultInfo(Travel_EventResult _serverResultInfo, long _oldEarnings)
        {
            CommonTravelEventResultInfo resultInfo = new CommonTravelEventResultInfo(this, _serverResultInfo?.getItemList(), _serverResultInfo?.getExt(), _oldEarnings);
            return resultInfo;
        }

        #endregion



    }
}