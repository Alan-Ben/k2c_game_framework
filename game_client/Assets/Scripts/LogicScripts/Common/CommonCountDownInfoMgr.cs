using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public interface _ICommonCountDownInfo
    {
        /// <summary>
        /// 剩余时间(毫秒)
        /// </summary>
        long remainTimeMs { get; }
    }
    
    /// <summary>
    /// 通用倒计时数据管理器
    /// </summary>
    public class CommonCountDownInfoMgr<T> where T : _ICommonCountDownInfo
    {
        private float _m_fTickInterval = 1f;//倒计时任务间隔时间(秒)
        
        private List<T> _m_lTotalInfoList;
        private T _m_iEarliestFinishCountDownInfo;//最早结束的倒计时信息
        
        private ALCommonEnableTaskController _m_CountDownTaskController;//倒计时任务

        public CommonCountDownInfoMgr(float _tickInterval)
        {
            _m_lTotalInfoList = null;
            _m_iEarliestFinishCountDownInfo = default;
            _m_fTickInterval = _tickInterval;
            
            onCountDownTick = null;
            onCountDownFinish = null;
            onEarliestFinishCountDownTargetChg = null;
        }
        
        ~CommonCountDownInfoMgr()
        {
            onCountDownTick = null;
            onCountDownFinish = null;
            onEarliestFinishCountDownTargetChg = null;
            
            clear();
        }

        public IReadOnlyList<T> totalInfoList { get { return _m_lTotalInfoList; } }
        public T earliestFinishCountDownInfo { get { return _m_iEarliestFinishCountDownInfo; } }
        
        public event Action onCountDownTick;
        public event Action<T> onCountDownFinish;//倒计时完成
        public event Action<T, T> onEarliestFinishCountDownTargetChg;//最早结束倒计时目标变化
        
        public void clear()
        {
            _m_iEarliestFinishCountDownInfo = default;
            _m_lTotalInfoList?.Clear();
            _m_lTotalInfoList = null;
            _discardUpgradeTechCountDownTask();
        }

        public void addCountDown(List<T> _totalInfoList)
        {
            if (_m_lTotalInfoList == null)
                _m_lTotalInfoList = new List<T>();
            if (_totalInfoList != null)
            {
                foreach (T info in _totalInfoList)
                {
                    if(info != null && !_m_lTotalInfoList.Contains(info))
                        _m_lTotalInfoList.Add(info);
                }
            }
            
            refreshEarliestFinishCountDownInfo();
        }

        public void addCountDown(T countDownInfo)
        {
            if (_m_lTotalInfoList == null)
                _m_lTotalInfoList = new List<T>();
            if(countDownInfo != null && !_m_lTotalInfoList.Contains(countDownInfo))
                _m_lTotalInfoList.Add(countDownInfo);
            
            refreshEarliestFinishCountDownInfo();
        }

        public bool removeCountDown(T countDownInfo)
        {
            if(_m_lTotalInfoList == null)
                return false;

            bool removeRes = _m_lTotalInfoList.Remove(countDownInfo);
            if(removeRes)
                refreshEarliestFinishCountDownInfo();
            
            return removeRes;
        }
        
        /// <summary>
        /// 刷新最早结束的倒计时信息
        /// </summary>
        public void refreshEarliestFinishCountDownInfo()
        {
            if(_m_lTotalInfoList == null)
                return;
            
            T preInfo = _m_iEarliestFinishCountDownInfo;
            _m_iEarliestFinishCountDownInfo = default;
            
            for(int i = _m_lTotalInfoList.Count - 1; i >= 0; i--)
            {
                var info = _m_lTotalInfoList[i];
                if(info == null || info.remainTimeMs <= 0)
                {
                    _m_lTotalInfoList.RemoveAt(i);
                    onCountDownFinish?.Invoke(info);
                    continue;
                }
                
                if(_m_iEarliestFinishCountDownInfo == null || _m_iEarliestFinishCountDownInfo.remainTimeMs > info.remainTimeMs)
                {
                    _m_iEarliestFinishCountDownInfo = info;
                }
            }
            
            if(!EqualityComparer<T>.Default.Equals(preInfo, _m_iEarliestFinishCountDownInfo))
                onEarliestFinishCountDownTargetChg?.Invoke(_m_iEarliestFinishCountDownInfo, preInfo);

            if (!EqualityComparer<T>.Default.Equals(_m_iEarliestFinishCountDownInfo, default))
            {
                _initUpgradeTechCountDownTask();
            }
            else
            {
                _discardUpgradeTechCountDownTask();
            }
        }
        
        #region 任务

        private void _discardUpgradeTechCountDownTask()
        {
            _m_CountDownTaskController.setDisable();
        }

        private void _initUpgradeTechCountDownTask()
        {
            _discardUpgradeTechCountDownTask();
            
            _m_CountDownTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(
                () =>
                {
                    onCountDownTick?.Invoke();
                    
                    // 刷新最早结束的倒计时信息
                    if(_m_iEarliestFinishCountDownInfo == null || _m_iEarliestFinishCountDownInfo.remainTimeMs <= 0)
                        refreshEarliestFinishCountDownInfo();
                    
                }, _m_fTickInterval);
        }
        
        #endregion
    }
}