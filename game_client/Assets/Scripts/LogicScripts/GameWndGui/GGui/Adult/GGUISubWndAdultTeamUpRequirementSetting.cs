using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 子嗣组队要求设置窗口
    /// </summary>
    public class GGUISubWndAdultTeamUpRequirementSetting : _ATALBasicUIWnd<GGUIMonoAdultTeamUpRequirementSetting>
    {
        private static GGUISubWndAdultTeamUpRequirementSetting _g_instance;
        public static GGUISubWndAdultTeamUpRequirementSetting instance { get{return _g_instance ??= new GGUISubWndAdultTeamUpRequirementSetting(); } }

        private long _m_iChildEarnings;//子嗣收益
        private int _m_iCurLimitPercent;//当前收益限制百分比
        
        // 确认回调
        private System.Action<long> _m_confirmCallback;

        public GGUISubWndAdultTeamUpRequirementSetting() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoAdultTeamUpRequirementSetting.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultTeamUpRequirementSetting.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public long curLimitEarnings
        {
            get
            {
                return _m_iChildEarnings * _m_iCurLimitPercent / 100;
            }
        }
        
        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_confirmCallback = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnAddLimit, _onAddLimitBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnReduceLimit, _onReduceLimitBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onConfirmBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnAddLimit, _onAddLimitBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReduceLimit, _onReduceLimitBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onConfirmBtnClick);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        /// <param name="_confirmCallback">确认回调，参数为选择的百分比</param>
        public void refreshWnd(long _childEarnings, System.Action<long> _confirmCallback)
        {
            _m_iChildEarnings = _childEarnings;
            // 初始化限制值设置为默认最大值
            _m_iCurLimitPercent = GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.max;
            _m_confirmCallback = _confirmCallback;

            refreshWnd();
        }

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _refreshLimitRangeText();
            _refreshCurLimitText();
            _refreshAdjustBtnText();
        }


        /// <summary>
        /// 刷新最低收益限制范围文本
        /// </summary>
        private void _refreshLimitRangeText()
        {
            if (wnd == null)
                return;

            long minLimit = _m_iChildEarnings * GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.min / 100;
            long maxLimit = _m_iChildEarnings * GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.max / 100;
            
            string key = string.IsNullOrEmpty(wnd.lowerEarningsLimitRangeKey) ? TransKeyConst.common_interval_num_num : wnd.lowerEarningsLimitRangeKey;
            ALUGUICommon.setLabelTxt(wnd.txtLowerEarningsLimitRange
                , TextTranslate.instance.getLanguage(key, minLimit.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD), maxLimit.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
        }

        /// <summary>
        /// 刷新当前最低收益限制文本
        /// </summary>
        private void _refreshCurLimitText()
        {
            if (wnd == null)
                return;

            // 显示当前限制百分比
            if (!string.IsNullOrEmpty(wnd.nowLimitEarningsKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtNowLimitEarnings, TextTranslate.instance.getLanguage(wnd.nowLimitEarningsKey, curLimitEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtNowLimitEarnings, curLimitEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
            }
        }

        /// <summary>
        /// 刷新增减按钮的百分比文本
        /// </summary>
        private void _refreshAdjustBtnText()
        {
            if (wnd == null)
                return;

            int adjustmentPercentage = GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_adjustment_percentage;
            
            // 增加按钮文本：+步长%
            ALUGUICommon.setLabelTxt(wnd.txtAddLimitAdjustmentPercentage, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, adjustmentPercentage));
            // 减少按钮文本：-步长%
            ALUGUICommon.setLabelTxt(wnd.txtReduceLimitAdjustmentPercentage, TextTranslate.instance.getLanguage(TransKeyConst.common_reducedPropPer_num, adjustmentPercentage));
        }

        private void _doCloseNode()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADULT_TEAM_UP_REQUIREMENT_SETTING);
        }

        #region 点击事件

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        private void _onCloseBtnClick(GameObject _obj)
        {
            _doCloseNode();
        }

        /// <summary>
        /// 增加限制按钮点击
        /// </summary>
        private void _onAddLimitBtnClick(GameObject _obj)
        {
            if (_m_iCurLimitPercent >= GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.max)
            {
                if(wnd != null)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(wnd.reachMaxLimitTip);
                return;
            }

            _m_iCurLimitPercent += GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_adjustment_percentage;
            // 确保不超过最大值
            if (_m_iCurLimitPercent > GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.max)
                _m_iCurLimitPercent = GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.max;

            _refreshCurLimitText();
        }

        /// <summary>
        /// 减少限制按钮点击
        /// </summary>
        private void _onReduceLimitBtnClick(GameObject _obj)
        {
            if (_m_iCurLimitPercent <= GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.min)
            {
                if(wnd != null)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(wnd.reachMinLimitTip);
                return;
            }
            
            _m_iCurLimitPercent -= GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_adjustment_percentage;
            // 确保不低于最小值
            if (_m_iCurLimitPercent < GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.min)
                _m_iCurLimitPercent = GRefdataCoreMgr.instance.npGeneral.child_marry_earnings_lower_limit_percentage_range.min;

            _refreshCurLimitText();
        }

        /// <summary>
        /// 确认按钮点击
        /// </summary>
        private void _onConfirmBtnClick(GameObject _obj)
        {
            _m_confirmCallback?.Invoke(curLimitEarnings);
            _doCloseNode();
        }

        #endregion
    }
}
