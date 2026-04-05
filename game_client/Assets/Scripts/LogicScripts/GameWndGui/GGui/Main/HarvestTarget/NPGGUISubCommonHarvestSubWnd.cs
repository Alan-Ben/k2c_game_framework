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
    /// 通用对应的一个资源收集目标对象
    /// 只跟征收类型有关，跟资源无关
    /// </summary>
    public class NPGGUISubCommonHarvestSubWnd : _ATALBasicUISubWnd<NPGGUICommonHarvestMono>, _IHarvestTarget
    {
        //当前货币值真实数据
        private long _m_lRealCount;
        //收获数字的管理器，只有在目标类型有效的情况下才创建
        private NPGGUIHarvestNumController _m_hcHarvestController;
        //单次粒子动画回调，用于其他表现
        public event Action<long> particleItemDoneAction;
        //全部粒子完成回调
        public event Action particleDoneAction;
        
        public NPGGUISubCommonHarvestSubWnd(long _initCount, NPGGUICommonHarvestMono _mono): base(_mono)
        {
            initWnd();
            _m_lRealCount = _initCount;
            _m_hcHarvestController = null;
        }

        protected override void _onShowWnd()
        {
            //刷新显示
            _refresh();
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            if(null == wnd)
                return;
            
            //根据类型创建显示数据，并注销目标
            if (wnd.harvestResType != EHarvestType.NONE)
            {
                //注销
                GGUIHarvestCore.instance.unregHarvestTarget(this);
                //清空数据控制类
                _m_hcHarvestController.discard();
                _m_hcHarvestController = null;
            }

            particleItemDoneAction = null;
            particleDoneAction = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            //根据类型创建显示数据，并注册目标
            if (wnd.harvestResType != EHarvestType.NONE)
            {
                _m_hcHarvestController = new NPGGUIHarvestNumController();
                //注册所有进度完成后的处理
                _m_hcHarvestController.regAllTaskDoneDelegate(_refresh);
                //注册
                GGUIHarvestCore.instance.regHarvestTarget(this);
            }
        }

        //设置资源变动
        public void setResChg(long _newCount)
        {
            //刷新真实数据
            _m_lRealCount = _newCount;

            //刷新显示
            _refresh();
        }
        
        /// <summary>
        /// 刷新处理
        /// </summary>
        protected void _refresh()
        {
            if(null == wnd)
                return;
            
            //判断是否有任务在处理
            if (null != _m_hcHarvestController && _m_hcHarvestController.taskEnable)
            {
                //使用任务数据刷新
                long taskCount = _m_hcHarvestController.calTotalCount();
                ALUGUICommon.setLabelTxt(wnd.txtNum, string.IsNullOrEmpty(wnd.transKey) ? taskCount.ToLargeString(wnd.harvestResType.toLargeStringType()) : TextTranslate.instance.getLanguage(wnd.transKey, taskCount.ToLargeString(wnd.harvestResType.toLargeStringType())));
            }
            else
            {
                //刷新显示
                ALUGUICommon.setLabelTxt(wnd.txtNum, string.IsNullOrEmpty(wnd.transKey) ? _m_lRealCount.ToLargeString(wnd.harvestResType.toLargeStringType()) : TextTranslate.instance.getLanguage(wnd.transKey, _m_lRealCount.ToLargeString(wnd.harvestResType.toLargeStringType())));
            }
        }

        #region 收获接口部分
        /// <summary>
        /// 对应的收获资源类型
        /// </summary>
        public EHarvestType harvestType { get { return null == wnd ? EHarvestType.DEFAULT : wnd.harvestResType; } }

        /// <summary>
        /// 获取粒子收集结束对象
        /// </summary>
        public RectTransform targetUIObj { get { return null == wnd ? null : wnd.tarTransform; } }

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

            //刷新显示
            _refresh();
            
            if (particleDoneAction != null) 
                particleDoneAction();
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

            //刷新显示
            _refresh();

            if (particleItemDoneAction != null) 
                particleItemDoneAction(_itemCount);
        }

        #endregion
    }
}