using ALPackage;
using Common.ChildEnum;
using JetBrains.Annotations;
using System;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndAdultUnmarriedGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoAdultUnmarriedGridItem>
    {
        private GGUISubWndChildInfo _m_adultInfoWnd;
        
        private UnmarriedInfo _m_unmarriedInfo;
        private EAdultStatus _m_curStatus;
        //item点击申请组队按钮
        private Action<UnmarriedInfo> _m_aOnClickItemApply;

        /// <summary>
        /// item点击申请组队按钮
        /// </summary>
        public Action<UnmarriedInfo> onClickItemApply { get { return _m_aOnClickItemApply; } set { _m_aOnClickItemApply = value; } }


        public GGUISubWndAdultUnmarriedGridItem(GGUIMonoAdultUnmarriedGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_adultInfoWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_adultInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_adultInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_adultInfoWnd?.discard();
            _m_adultInfoWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onCancelBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSearching, _onSearchingBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAdultInfo != null)
                _m_adultInfoWnd = new GGUISubWndChildInfo(wnd.monoAdultInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onCancelBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnSearching, _onSearchingBtnClick);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(UnmarriedInfo _unmarriedInfo)
        {
            _m_unmarriedInfo = _unmarriedInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_unmarriedInfo == null)
                return;
            
            _m_adultInfoWnd?.refreshWnd(_m_unmarriedInfo.adultInfo);
            _m_curStatus = _m_unmarriedInfo.status;
            wnd.setSearchingType(_m_curStatus);
            _refreshLowerBonusLimit();
            ALUGUICommon.setLabelTxt(wnd.txtSearchingTime, TimeUtil.millisecondsToTime_hms((_m_unmarriedInfo.expiredTs - FpsAndPingMgr.instance.serverTimeTagS) * 1000));
        }
        public void refreshState()
        {
            if (wnd == null || _m_unmarriedInfo == null)
                return;
            
            _m_curStatus = _m_unmarriedInfo.status;
            wnd.setSearchingType(_m_curStatus);

            _refreshLowerBonusLimit();
        }
        public void refreshTime()
        {
            if (wnd == null || _m_unmarriedInfo == null)
                return;

            if (_m_curStatus is EAdultStatus.NONE or EAdultStatus.MARRIED)
                return;
            
            long remainTimeS = _m_unmarriedInfo.expiredTs - FpsAndPingMgr.instance.serverTimeTagS;
            if (remainTimeS < 0)
                refreshState();
            else
                ALUGUICommon.setLabelTxt(wnd.txtSearchingTime, TimeUtil.millisecondsToTime_hms(remainTimeS * 1000));
        }

        private void _refreshLowerBonusLimit()
        {
            if (wnd == null || !isShow || _m_unmarriedInfo == null)
                return;
            
            bool hasLowerBonusLimit = _m_unmarriedInfo.minAllowBonus > 0 && _m_curStatus == EAdultStatus.APPLY_SERVER;
            ALUGUICommon.setGameObjEnable(wnd.hasLowerEarningsLimitShow, hasLowerBonusLimit);
            ALUGUICommon.setGameObjEnable(wnd.hasLowerEarningsLimitHide, !hasLowerBonusLimit);
            if (hasLowerBonusLimit)
            {
                string limitLowerEarningsLargeStr = _m_unmarriedInfo.minAllowBonus.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD);
                ALUGUICommon.setLabelTxt(wnd.txtLimitLowerEarnings, string.IsNullOrEmpty(wnd.limitLowerEarningsKey) ? limitLowerEarningsLargeStr : 
                    TextTranslate.instance.getLanguage(wnd.limitLowerEarningsKey, limitLowerEarningsLargeStr));       
            }
        }

        private void _onCancelBtnClick(GameObject _obj)
        {
            if (_m_unmarriedInfo == null)
                return;

            EAdultStatus status = _m_unmarriedInfo.status;
            if (status is EAdultStatus.APPLY_PLAYER)
                NPGSClientListener.sendMsgByLog(GSWriter_014_ChildOp.make_016_ReqCancelApplyToPlayer(_m_unmarriedInfo.adultId));
            else if (status is EAdultStatus.APPLY_SERVER)
                NPGSClientListener.sendMsgByLog(GSWriter_014_ChildOp.make_017_ReqCancelApplyToGroup(_m_unmarriedInfo.adultId));
        }
        private void _onSearchingBtnClick(GameObject _obj)
        {
            _m_aOnClickItemApply?.Invoke(_m_unmarriedInfo);
        }
    }
}