using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用对应的一个资源收集目标对象 由外部进行表现处理
    /// 只跟征收类型有关，跟资源无关
    /// </summary>
    public class NPGGUISubOutSetHarvestWnd : _IHarvestTarget
    {
        //收获资源对象
        private EHarvestType _m_eHarvestType;
        private RectTransform _m_targetUIObj;
        //当前货币值真实数据
        private long _m_lRealCount;

        //收获数字的管理器，只有在目标类型有效的情况下才创建
        private NPGGUIHarvestNumController _m_hcHarvestController;

        //是否初始化
        private bool _m_bIsInited;

        //资源变动回调
        private Action _m_resChgAction;

        //开始回调
        private Action _m_startAction;

        //单次粒子动画回调，用于其他表现
        private Action<long> _m_perItemDone;

        //第一个粒子到达回调
        private Action<long> _m_firstItemDone;

        //全部粒子完成回调
        private Action _m_doneAction;

        //是否是新的一轮
        private Dictionary<long,bool> _m_dIsNewRound;

        public NPGGUISubOutSetHarvestWnd(RectTransform _targetUIObj, EHarvestType _harvestResType, Action _resChgAction = null, Action _startAction = null, Action<long> _firstItemDone = null, Action<long> _perItemDone = null, Action _doneAction = null)
        {
            _m_eHarvestType = _harvestResType;
            _m_targetUIObj = _targetUIObj;

            _m_lRealCount = 0;
            _m_hcHarvestController = null;

            _m_bIsInited = false;
            _m_dIsNewRound = new Dictionary<long, bool>();

            _m_resChgAction = _resChgAction;
            _m_startAction = _startAction;
            _m_firstItemDone = _firstItemDone;
            _m_perItemDone = _perItemDone;
            _m_doneAction = _doneAction;
        }

        /// <summary>
        /// 初始化相关的监控和管理
        /// </summary>
        public void init()
        {
            if (_m_bIsInited)
                return;

            //设置为已初始化
            _m_bIsInited = true;

            //根据类型创建显示数据，并注册目标
            if (_m_eHarvestType != EHarvestType.NONE)
            {
                _m_hcHarvestController = new NPGGUIHarvestNumController();
                //注册
                GGUIHarvestCore.instance.regHarvestTarget(this);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void discard()
        {
            if (!_m_bIsInited)
                return;

            _m_bIsInited = false;
            _m_dIsNewRound?.Clear();

            //根据类型创建显示数据，并注销目标
            if (_m_eHarvestType != EHarvestType.NONE)
            {
                //注销
                GGUIHarvestCore.instance.unregHarvestTarget(this);
                //清空数据控制类
                _m_hcHarvestController.discard();
                _m_hcHarvestController = null;
            }
        }
        
        //设置资源变动
        public void setResChg(long _newCount)
        {
            //刷新真实数据
            _m_lRealCount = _newCount;
            if (null != _m_resChgAction)
                _m_resChgAction();
        }
        
        /// <summary>
        /// 刷新处理
        /// </summary>
        public long getCurCount()
        {
            long count = 0;
            //判断是否有任务在处理
            if (null != _m_hcHarvestController && _m_hcHarvestController.taskEnable)
            {
                //使用任务数据刷新
                count = _m_hcHarvestController.calTotalCount();
            }
            else
            {
                count = _m_lRealCount;
            }
            return count;
        }

        #region 收获接口部分
        /// <summary>
        /// 对应的收获资源类型
        /// </summary>
        public EHarvestType harvestType { get { return _m_eHarvestType; } }

        /// <summary>
        /// 获取粒子收集结束对象
        /// </summary>
        public RectTransform targetUIObj { get { return null == _m_targetUIObj ? null : _m_targetUIObj; } }

        /// <summary>
        /// 粒子开始表现时的处理
        /// 此时需要设置好相关的状态以及可能触发消息的处理，避免错误的刷新操作
        /// </summary>
        /// <param name="_serialize"></param>
        /// <param name="_canDrawNum"></param>
        /// <param name="_particleNum"></param>
        public void particleStart(long _serialize)
        {
            if (null != _m_hcHarvestController)
                _m_hcHarvestController.startNumFunc(_m_lRealCount, _serialize);

            _m_dIsNewRound[_serialize] = true;

            if (null != _m_startAction)
                _m_startAction();
        }
        /// <summary>
        /// 粒子表现结束的处理
        /// </summary>
        /// <param name="_serialize"></param>
        public void particleComplete(long _serialize)
        {
            //设置任务完成
            if (null != _m_hcHarvestController)
                _m_hcHarvestController.setTaskDone(_serialize);

            if (null != _m_doneAction)
                _m_doneAction();
        }
        /// <summary>
        /// 单个粒子飞行到位的处理
        /// </summary>
        /// <param name="_serialize"></param>
        /// <param name="_itemCount">单个粒子代表数量</param>
        public void particleItemDone(long _serialize, long _itemCount)
        {
            //设置任务数量
            if (null != _m_hcHarvestController)
                _m_hcHarvestController.addTaskNum(_serialize, _itemCount);

            if (_m_dIsNewRound.TryGetValue(_serialize,out bool _isNew) && _isNew)
            {
                _m_dIsNewRound.Remove(_serialize);
                _m_firstItemDone?.Invoke(_itemCount);
            }

            if (null != _m_perItemDone)
            {
                _m_perItemDone(_itemCount);
            }
        }

        #endregion
    }
}