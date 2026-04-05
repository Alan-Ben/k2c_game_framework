using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    /// <summary>
    /// 
    /// </summary>
    public interface _IQteClickOpportunityGameItemShow
    {
        public QteClickOpportunityItemConfig itemConfig { get; }
        
        public void setGameController([NotNull] QteClickOpportunityGameController _gameController, Action _onItemTrigger);
        
        /// <summary>
        /// 显示item
        /// </summary>
        public void showItem();

        /// <summary>
        /// 隐藏item
        /// </summary>
        public void hideItem();

        /// <summary>
        /// 设置触发时间
        /// </summary>
        /// <param name="_triggerTime"></param>
        public void setTriggerTime(float _triggerTime, OpportunityTimeRange _inTimeRange);
    }
    
    public class QteClickOpportunityGameItemUnit : _AQteClickOpportunityGameUnit
    {
        [NotNull] private _IQteClickOpportunityGameItemShow _m_itemShow;
        private Action<QteClickOpportunityGameItemUnit> _m_aOnItemTrigger;

        private bool _m_bIsTrigger;//是否已经触发/倒计时结束
        private bool _m_bIsSuccess;//是否成功
        
        private float _m_fShowTime;//item显示出来的时间
        
        public QteClickOpportunityGameItemUnit([NotNull] _IQteClickOpportunityGameItemShow _itemShow, Action<QteClickOpportunityGameItemUnit> _onItemTrigger, [NotNull] QteClickOpportunityGameLogic _gameLogic, [NotNull] QteClickOpportunityGameController _gameController) : base(_gameLogic, _gameController)
        {
            _m_itemShow = _itemShow;
            _m_aOnItemTrigger = _onItemTrigger;
        }

        public bool isTrigger { get { return _m_bIsTrigger; } }
        public bool isSuccess { get { return _m_bIsSuccess; } }

        public override void init()
        {
            _m_bIsTrigger = false;
            _m_fShowTime = 0;

            _m_itemShow.setGameController(_m_gameController, _onTrigger);
            _dealDelayShow();
        }

        public override void discard()
        {
            _m_itemShow.hideItem();

            _m_bIsTrigger = true;
            _m_fShowTime = 0;
        }

        /// <summary>
        /// 进行延迟显示
        /// </summary>
        private void _dealDelayShow()
        {
            // 若不需要延迟, 直接显示
            if (_m_itemShow.itemConfig == null || _m_itemShow.itemConfig.showDelay <= 0)
            {
                _showItem();
            }
            else//需要延迟显示
            {
                long serializedId = _m_gameLogic.gameSerializeId;
                CommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(_m_gameLogic.gameSerializeId != serializedId)
                        return;

                    _showItem();
                });
            }
        }

        private void _showItem()
        {
            _m_itemShow.showItem();
            _m_fShowTime = Time.realtimeSinceStartup;

            if (_m_itemShow.itemConfig != null && !_m_itemShow.itemConfig.loop)
            {
                if (_m_itemShow.itemConfig.loopTime > 0)
                {
                    long serializedId = _m_gameLogic.gameSerializeId;
                    CommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        if(_m_gameLogic.gameSerializeId != serializedId)
                            return;

                        _onOutOfLimitTime();
                    }, _m_itemShow.itemConfig.loopTime);
                }
                else
                {
                    _onOutOfLimitTime();
                }
            }
        }

        /// <summary>
        /// 超出限制时间时
        /// </summary>
        private void _onOutOfLimitTime()
        {
            if(_m_bIsTrigger)//若当前已经结束, 直接返回
                return;

            _m_bIsTrigger = true;
            if(_m_itemShow.itemConfig == null)
                return;

            float triggerTime = float.MaxValue;
            OpportunityTimeRange opportunityTimeRange = null;
            _m_bIsSuccess = false;
            if (_m_itemShow.itemConfig.opportunityTimeRangeList == null || _m_itemShow.itemConfig.opportunityTimeRangeList.Count <= 0)
                _m_bIsSuccess = true;
            else
            {
                foreach (OpportunityTimeRange timeRange in _m_itemShow.itemConfig.opportunityTimeRangeList)
                {
                    if(timeRange == null || timeRange.timeRange == null)
                        continue;
                    
                    if(timeRange.timeRange != null && timeRange.timeRange.inRange(triggerTime))
                    {
                        opportunityTimeRange = timeRange;
                        _m_bIsSuccess = timeRange.isSuccess;
                        break;                        
                    }
                }
            }
            
            _m_itemShow.setTriggerTime(triggerTime, opportunityTimeRange);
            _m_aOnItemTrigger?.Invoke(this);
        }

        /// <summary>
        /// item触发时调用方法
        /// </summary>
        private void _onTrigger()
        {
            if(_m_bIsTrigger)//若当前已经结束, 直接返回
                return;
            
            _m_bIsTrigger = true;
            if(_m_itemShow.itemConfig == null)
                return;
            
            float triggerTime = Time.realtimeSinceStartup;
            float triggerInterval = triggerTime - _m_fShowTime;//从开始显示到点击的时间间隔
            if (_m_itemShow.itemConfig.loop && _m_itemShow.itemConfig.loopTime != 0) //若是循环的
                triggerInterval = triggerInterval % _m_itemShow.itemConfig.loopTime;//点击间隔转化为循环中的时间间隔

            OpportunityTimeRange opportunityTimeRange = null;
            _m_bIsSuccess = false;
            if (_m_itemShow.itemConfig.opportunityTimeRangeList == null || _m_itemShow.itemConfig.opportunityTimeRangeList.Count <= 0)
                _m_bIsSuccess = true;
            else
            {
                foreach (OpportunityTimeRange timeRange in _m_itemShow.itemConfig.opportunityTimeRangeList)
                {
                    if(timeRange == null || timeRange.timeRange == null)
                        continue;
                    
                    if(timeRange.timeRange != null && timeRange.timeRange.inRange(triggerInterval))
                    {
                        opportunityTimeRange = timeRange;
                        _m_bIsSuccess = timeRange.isSuccess;
                        break;                        
                    }
                }
            }
            
            _m_itemShow.setTriggerTime(triggerInterval, opportunityTimeRange);
            _m_aOnItemTrigger?.Invoke(this);
        }
    }
}