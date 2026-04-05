using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndChildContainerItem : _ATALBasicUISubWnd<GGUIMonoChildContainerItem>
    {
        private readonly Action<SeatInfo> _m_onSeatClick;

        private GGUISubWndChildInfo _m_childInfoWnd;

        private SeatInfo _m_seatInfo;
        private bool _m_isSelect;
        private bool _m_bNeedShowUnlockDesc;
        [NotNull] private readonly List<_ISfxObj> _m_sfxList;


        public GGUISubWndChildContainerItem(GGUIMonoChildContainerItem _wnd, Action<SeatInfo> _onSeatClick)
            : base(_wnd)
        {
            _m_sfxList = new List<_ISfxObj>();
            _m_onSeatClick = _onSeatClick;

            initWnd();
        }
        

        public ChildInfo childInfo { get { return _m_seatInfo?.childInfo; } }
        public SeatInfo seatInfo { get { return _m_seatInfo; } }


        protected override void _onShowWnd()
        {
            _m_childInfoWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_childInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _clearAllSfx();

            _m_childInfoWnd?.discard();
            _m_childInfoWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onBtnClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.haveChildShow.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.haveChildShow.monoChildInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClicked);
        }


        public void refreshWnd(SeatInfo _seatInfo, bool _isSelect, bool _needShowUnlockDesc)
        {
            _m_seatInfo = _seatInfo;
            _m_isSelect = _isSelect;
            _m_bNeedShowUnlockDesc = _needShowUnlockDesc;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_seatInfo == null)
                return;
            
            _m_childInfoWnd?.refreshWnd(_m_seatInfo.childInfo);
            wnd.setSelect(_m_isSelect);
            wnd.setType(!_m_seatInfo.isUnlock() ? GGUIMonoChildContainerItemShowType.Lock :
                _m_seatInfo.childInfo == null ? GGUIMonoChildContainerItemShowType.Empty : GGUIMonoChildContainerItemShowType.HaveChild);
            wnd.setCanGraduate(_m_seatInfo.childInfo != null && _m_seatInfo.childInfo.canGraduate());
            wnd.setRedTipShow(_m_seatInfo.needShowRedTip());

            //解锁条件描述
            if (!_m_seatInfo.isUnlock() && _m_bNeedShowUnlockDesc)
                ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, _m_seatInfo.baseRef.getTranslatedUnlockDesc());
            else
                ALUGUICommon.setLabelTxt(wnd.txtUnlockDesc, "");
        }
        public void showExpCollect(long _num)
        {
            if (wnd == null || _m_seatInfo == null)
                return;

            if (wnd.haveChildShow.transOneKeyEffectPoint == null)
                return;

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.uiCamera, wnd.haveChildShow.transOneKeyEffectPoint.position);
            NPGTextureIndex expIcon = GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long) ECurrency.HERO_EXP);
            string text = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _num.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            NPGUIAddSceneCenterTip.instance.showIconTextTip(expIcon, text, wnd.haveChildShow.oneKeyEducateCollectTipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); });
            GCommon.showItemParticle(new NPCommon_ItemInfo((int) ENPItemType.CURRENCY, (long) ECurrency.HERO_EXP, wnd.haveChildShow.oneKeyParticleNum, null), screenPos, wnd.particleId, null, true);
        }
        public void showEnergyRecover(long _count)
        {
            if (wnd == null)
                return;

            if (wnd.energyRecoverEffectPoint == null)
                return;

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.uiCamera, wnd.energyRecoverEffectPoint.position);
            string text = TextTranslate.instance.getLanguage(TransKeyConst.child_energyRecoverNum_num, _count);
            NPGUIAddSceneCenterTip.instance.showTextTip(text, wnd.energyRecoverTipId, (_tipWnd) => { _tipWnd?.setTipPos(screenPos); });
        }
        public void playEducateSfx()
        {
            if (wnd == null)
                return;

            _playSfx(wnd.haveChildShow.educateSfxId, wnd.haveChildShow.educateSfxParent);
        }


        private void _onBtnClicked(GameObject _)
        {
            _m_onSeatClick?.Invoke(_m_seatInfo);
        }
        private void _playSfx(long _sfxId, Transform _sfxParent)
        {
            if (_sfxParent == null)
                return;

            _ISfxObj sfx = PlaySfxMgr.instance.playUISfx(_sfxId, _sfxParent);
            if (sfx != null)
            {
                _m_sfxList.Add(sfx);
                _limitSfxCount();
            }
        }
        private void _limitSfxCount()
        {
            if (wnd == null || wnd.haveChildShow.maxSfxCount < 0)
                return;

            while (_m_sfxList.Count > wnd.haveChildShow.maxSfxCount)
            {
                _ISfxObj oldestSfx = _m_sfxList[0];
                _m_sfxList.RemoveAt(0);
                oldestSfx?.forceDiscard();
            }
        }
        private void _clearAllSfx()
        {
            foreach (_ISfxObj sfx in _m_sfxList)
                sfx?.forceDiscard();
            _m_sfxList.Clear();
        }
    }
}