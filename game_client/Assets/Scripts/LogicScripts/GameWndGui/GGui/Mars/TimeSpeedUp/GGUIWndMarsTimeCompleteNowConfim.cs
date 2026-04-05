using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public interface _IMarsCompleteNowObject
    {
        /// <summary>
        /// 检查是否可以立即完成
        /// </summary>
        /// <param name="_checkCompleteNowCostEnough">是否检查立即完成消耗</param>
        /// <param name="_showUnableTip">是否显示提示</param>
        /// <returns></returns>
        bool checkCanCompleteNow(bool _checkCompleteNowCostEnough, bool _showUnableTip);
        
        /// <summary>
        /// 是否有剩余时间(这个字段作用是用于那种还没有倒计时的情况, 例如建筑还未开始建造、科技还未开始研究, 但是这时也是可以立即完成的)
        /// </summary>
        bool hasRemainTime { get; }
        
        /// <summary>
        /// 剩余时间(毫秒)
        /// </summary>
        long remainTimeMs { get; }
        
        /// <summary>
        /// 立即完成所需物品
        /// </summary>
        NPCommonCostItem completeNowCostItem { get; }
        
        /// <summary>
        /// 立即完成操作
        /// 参数为完成后的回调
        /// </summary>
        public Action<Action<bool>> dealCompleteNowAction { get; }
    }
    
    /// <summary>
    /// 火星时间立即完成确认窗口
    /// </summary>
    public class GGUIWndMarsTimeCompleteNowConfim : _ANPGGUIBasicWnd<GGUIMonoMarsTimeCompleteNowConfim>
    {
        [NotNull] public static GGUIWndMarsTimeCompleteNowConfim instance { get { return _g_instance ??= new GGUIWndMarsTimeCompleteNowConfim(); } }
        private static GGUIWndMarsTimeCompleteNowConfim _g_instance;


        private NPGGUIWndCommonToggleEx _m_dontShowTodayToggle; // 今日不再提示
        private NPGGUIWndCommonItem _m_costItemWnd; // 消耗物品展示
        
        private _IMarsCompleteNowObject _m_targetObject; // 目标对象
        private Action _m_aOnCompleteNowCallback; // 立即完成回调
        private ALCommonEnableTaskController _m_tRemainTimeTickTask; // 剩余时间倒计时任务

        public GGUIWndMarsTimeCompleteNowConfim() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsTimeCompleteNowConfim.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsTimeCompleteNowConfim.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_dontShowTodayToggle?.hideWnd();
            _m_costItemWnd?.hideWnd();

            _discardRemainTimeTickTask();
        }
        protected override void _onReset()
        {
            _m_dontShowTodayToggle?.resetWnd();
            _m_costItemWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (_m_dontShowTodayToggle != null)
            {
                _m_dontShowTodayToggle.clickDelegate -= _dontShowTodayToggleClick;
                _m_dontShowTodayToggle.discard();
                _m_dontShowTodayToggle = null;    
            }
            
            _m_costItemWnd?.discard();
            _m_costItemWnd = null;

            _m_targetObject = null;
            _m_aOnCompleteNowCallback = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化今日不再提示
            if (wnd.monoDontShowToday != null)
            {
                _m_dontShowTodayToggle = new NPGGUIWndCommonToggleEx(wnd.monoDontShowToday);
                _m_dontShowTodayToggle.clickDelegate += _dontShowTodayToggleClick;
            }

            // 初始化消耗物品展示
            if (wnd.monoCostItem != null)
                _m_costItemWnd = new NPGGUIWndCommonItem(wnd.monoCostItem);

            // 绑定按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnCompleteNow, _onBtnCompleteNowClick);
        }


        /// <summary>
        /// 设置信息并刷新窗口
        /// </summary>
        /// <param name="_targetObject">目标对象</param>
        /// <param name="_onCompleteNowCallback">立即完成回调</param>
        public void refreshWnd(_IMarsCompleteNowObject _targetObject, Action _onCompleteNowCallback)
        {
            _m_targetObject = _targetObject;
            _m_aOnCompleteNowCallback = _onCompleteNowCallback;
            refreshWnd();
        }
        /// <summary>
        /// 刷新窗口
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            // 若数据不存在 或者 不允许打开窗口 或者 没有剩余时间了，则关闭窗口
            if (_m_targetObject == null || !_m_targetObject.checkCanCompleteNow(false, false) ||
                (_m_targetObject.hasRemainTime && _m_targetObject.remainTimeMs <= 0))
            {
                // 延迟到later关闭窗口, 防止在有ShowWndAnimation时, 先销毁了资源再播放动画导致报错
                ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
                {
                    _doCloseWnd();
                });
                return;
            }
            
            // 刷新提示文本
            _refreshTipText();

            if (_m_dontShowTodayToggle != null)
            {
                _m_dontShowTodayToggle.showWnd();
                _m_dontShowTodayToggle.setSelected(!AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.MARS_TIME_COMPLETE_NOW_CONFIRM));
            }
            
            // 刷新剩余时间
            _refreshRemainTime();
            
            // 刷新消耗物品
            _refreshCostItem();

            // 初始化倒计时任务
            if(_m_targetObject.hasRemainTime)
                _initRemainTimeTickTask();
        }


        /// <summary>
        /// 刷新提示文本
        /// </summary>
        private void _refreshTipText()
        {
            if (wnd == null || _m_targetObject == null)
                return;

            NPCommonCostItem costItem = _m_targetObject.completeNowCostItem;
            if (costItem == null)
                return;

            string costItemName = GCommon.getItemName(costItem.getItemType(), costItem.subId);
            
            // 消耗:【{0}】来立即完成
            ALUGUICommon.setLabelTxt(wnd.txtTip, TextTranslate.instance.getLanguage(TransKeyConst.mars_timeCompleteNowConfirmTip_str, costItemName));
        }
        /// <summary>
        /// 刷新剩余时间
        /// </summary>
        private void _refreshRemainTime()
        {
            if (wnd == null || _m_targetObject == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.hasRemainTimeShowList, _m_targetObject.hasRemainTime);
            
            long remainMs = _m_targetObject.remainTimeMs;
            string timeStr = TimeUtil.millisecondsToTime_dhms(remainMs);
            
            // 剩余时间: {0}
            ALUGUICommon.setLabelTxt(wnd.txtRemainTime, TextTranslate.instance.getLanguage(TransKeyConst.mars_timeCompleteNowRemainTime_str, timeStr));
        }
        /// <summary>
        /// 刷新消耗物品
        /// </summary>
        private void _refreshCostItem()
        {
            if (wnd == null || _m_targetObject == null || _m_costItemWnd == null)
                return;

            if (_m_costItemWnd != null)
            {
                _m_costItemWnd.showWnd();
                _m_costItemWnd.setItem(_m_targetObject.completeNowCostItem);
            }
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onBtnCloseClick(GameObject _obj)
        {
            // 关闭窗口
            _doCloseWnd();
        }

        private void _dontShowTodayToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if(_m_dontShowTodayToggle == null)
                return;

            bool isOn = !_m_dontShowTodayToggle.isOn;
            _m_dontShowTodayToggle.setSelected(isOn);
        }
        
        /// <summary>
        /// 点击立即完成按钮
        /// </summary>
        private void _onBtnCompleteNowClick(GameObject _obj)
        {
            if (_m_targetObject == null)
                return;

            // 检查消耗是否足够
            NPCommonCostItem costItem = _m_targetObject.completeNowCostItem;
            if (costItem != null && !GCommon.isItemEnough(costItem, true))
                return;

            // 若勾选了今日不再提示，则保存设置
            if (_m_dontShowTodayToggle is { isOn: true })
                AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.MARS_TIME_COMPLETE_NOW_CONFIRM);

            // 执行立即完成操作
            _m_targetObject.dealCompleteNowAction?.Invoke((_isSucc) =>
            {
                if (_isSucc)
                    _dealCompleteNowDone();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void _dealCompleteNowDone()
        {
            Action action = _m_aOnCompleteNowCallback;
            _m_aOnCompleteNowCallback = null;
            action?.Invoke();
            
            _doCloseWnd();
        }
        
        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void _doCloseWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_TIME_COMPLETE_NOW_CONFIRM);
        }

        #region 倒计时任务
        
        /// <summary>
        /// 销毁倒计时任务
        /// </summary>
        private void _discardRemainTimeTickTask()
        {
            _m_tRemainTimeTickTask.setDisable();
        }
        
        /// <summary>
        /// 初始化倒计时任务
        /// </summary>
        private void _initRemainTimeTickTask()
        {
            _discardRemainTimeTickTask();
            
            _m_tRemainTimeTickTask = CommonTaskController.CommonEnableDurationActionAddMonoTask(_remainTimeTickTaskAction, 1f);
        }

        private void _remainTimeTickTaskAction()
        {
            // 若数据不存在 或者 不允许打开窗口 或者 没有剩余时间了，则关闭窗口
            if (_m_targetObject == null || !_m_targetObject.checkCanCompleteNow(false, false) || 
                (_m_targetObject.hasRemainTime && _m_targetObject.remainTimeMs <= 0))
            {
                // 延迟到later关闭窗口, 防止在有ShowWndAnimation时, 先销毁了资源再播放动画导致报错
                ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
                {
                    _doCloseWnd();
                });
                return;
            }
            
            _refreshRemainTime();
            _refreshCostItem();
        }

        #endregion

        /// <summary>
        /// 添加Node
        /// </summary>
        public static void addNode(_IMarsCompleteNowObject _targetObject, Action _onCompleteNowCallback = null)
        {
            instance.refreshWnd(_targetObject, _onCompleteNowCallback);
            QueueMgr.instance.addNode_InGame_SingleWnd(instance, instance.showWnd, UINodeTagConst.C_MARS_TIME_COMPLETE_NOW_CONFIRM);
        }
    }
}
