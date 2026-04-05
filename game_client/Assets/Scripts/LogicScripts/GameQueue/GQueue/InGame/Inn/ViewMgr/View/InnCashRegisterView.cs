using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class InnCashRegisterView : _AALBasicLoadObj
    {
        [NotNull] private InnViewMgr _m_viewMgr;
        private GTDMonoInnCashRegister _m_mono;
        private long _m_rewardNum;
        private GGUICommonFollowTarget _m_followTarget;
        private GGUIWndInnCashRegisterHudFollowerController _m_hudWnd;
        private int _m_loadSerialize;
        [NotNull] private readonly List<_ISfxObj> _m_sfxList;
        private const int MAX_SFX_COUNT = 3;
        private NPGGoIndex _m_goIndex;
        
        
        public InnCashRegisterView([NotNull] InnViewMgr _viewMgr)
        {
            _m_sfxList = new List<_ISfxObj>();
            _m_viewMgr = _viewMgr;
        }
        

        public GTDMonoInnCashRegister mono { get { return _m_mono; } }


        public void setRewardNum(long _rewardNum)
        {
            _m_rewardNum = _rewardNum;
            _m_hudWnd?.refreshWnd(_m_rewardNum);
            if (_m_mono == null)
                return;
            
            _m_mono.setGuestNum(_rewardNum);
        }
        public void triggerClick()
        {
            _onClick();
        }
        

        protected override void _loadOp()
        {
            _m_goIndex = NPPlayer.instance.innComp.levelRef?.inn_cash_register_index;
            Vector3 position = MainAdditionInnTDScene.instance.getCashRegisterPosition();
            MainAdditionInnTDScene.instance.createUnit<GTDMonoInnCashRegister>(_m_goIndex, position, _mono =>
            {
                if (_mono == null)
                {
                    _setLoadDone();
                    return;
                }
                
                _m_mono = _mono;
                _onInitDone();
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            _m_loadSerialize = ALSerializeOpMgr.next();
            _clearAllSfx();
            _onDiscard();
            _m_mono = null;
        }
        

        private void _onInitDone()
        {
            if (_m_mono == null)
                return;
            
            if (_m_mono.followTarget != null)
            {
                _m_followTarget = new GGUICommonFollowTarget(mono.followTarget, Vector3.zero);
                GGUIWndInnHud.instance.regInstance(_m_followTarget);
                _m_hudWnd = new GGUIWndInnCashRegisterHudFollowerController(this);
                _m_hudWnd?.refreshWnd(_m_rewardNum);
                _m_followTarget.addController(_m_hudWnd);
            }

            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick += _onClick;
            
            _m_mono.setGuestNum(_m_rewardNum);
            
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_CASH_REGISTER, _onClick);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_TRIGGER_INN_CASH_REGISTER_COLLECT_ANIM, _playRewardCollectAnim);
            playIdleAnim();
        }
        private void _onDiscard()
        {
            if (_m_mono == null)
                return;
            
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_TRIGGER_INN_CASH_REGISTER_COLLECT_ANIM, _playRewardCollectAnim);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_CASH_REGISTER, _onClick);
            
            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick -= _onClick;

            GGUIWndInnHud.instance.removeInstance(_m_followTarget);
            _m_followTarget?.discard();
            _m_followTarget = null;
            MainAdditionInnTDScene.instance.discardUnit(_m_goIndex, _m_mono);
        }


        public void popRewardGainTip(long _popularityGain, long _finesseGain, long _affectionGain)
        {
            if (_m_followTarget == null || _m_mono == null)
                return;
            
            if (_popularityGain > 0)
            {
                GGUIWndInnCashRegisterRewardTipFollowItemController popularityTip = new GGUIWndInnCashRegisterRewardTipFollowItemController();
                popularityTip.setReward(_popularityGain, GRefdataCoreMgr.instance.npGeneral.inn_popularity_icon);
                _m_followTarget.addController(popularityTip);
                _popularityGain = 0;
            }
            else if (_finesseGain > 0)
            {
                GGUIWndInnCashRegisterRewardTipFollowItemController finesseTip = new GGUIWndInnCashRegisterRewardTipFollowItemController();
                finesseTip.setReward(_finesseGain, GRefdataCoreMgr.instance.npGeneral.inn_dish_finesse_icon);
                _m_followTarget.addController(finesseTip);
                _finesseGain = 0;
            }
            else if (_affectionGain > 0)
            {
                GGUIWndInnCashRegisterRewardTipFollowItemController affectionTip = new GGUIWndInnCashRegisterRewardTipFollowItemController();
                affectionTip.setReward(_affectionGain, GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long) ECurrency.INN_AFFECTION));
                _m_followTarget.addController(affectionTip);
                _affectionGain = 0;
            }
            
            float timeSpace = _m_mono.tipTimeSpace;
            ALCommonActionMonoTask.addMonoTask(() => popRewardGainTip(_popularityGain, _finesseGain, _affectionGain), timeSpace);
        }
        public void playRewardAddEffect()
        {
            if (_m_hudWnd == null)
                return;
            
            _m_hudWnd.playRewardAddEffect();
        }
        
        
        private void _onClick()
        {
            _m_viewMgr.dealCashRegisterClick();
        }

        internal void setRewardCollecting(bool _collecting)
        {
            _m_hudWnd?.setRewardCollecting(_collecting);
        }

        internal void playCollectAnim()
        {
            _playRewardCollectAnim();
        }
        internal void playIdleAnim()
        {
            if (_m_mono == null)
                return;

            if (_m_mono.animController != null)
                _m_mono.animController.playAniTag(_m_mono.idleAnimName);
            if (_m_mono.recoverExtraAnim != null)
                _m_mono.recoverExtraAnim.Sample(_m_mono.recoverExtraAnimName, 0);
            if (_m_mono.collectExtraAnim != null)
                _m_mono.collectExtraAnim.Sample(_m_mono.collectExtraAnimName, 0);
        }
        internal void playRecoverAnim([NotNull] Action _complete)
        {
            if (_m_mono == null)
            {
                _complete.Invoke();
                return;
            }

            if (_m_mono.animController != null)
                _m_mono.animController.playAniTag(_m_mono.recoverAnimName);
            if (_m_mono.recoverExtraAnim != null)
                _m_mono.recoverExtraAnim.Play(_m_mono.recoverExtraAnimName);
            ALCommonActionMonoTask.addMonoTask(_complete, _m_mono.recoverAnimTime);
        }
        private void _playRewardCollectAnim()
        {
            if (_m_mono == null)
                return;
            
            if (_m_mono.animController != null)
                _m_mono.animController.setAniTag(_m_mono.collectAnimName);
            if (_m_mono.collectExtraAnim != null)
                _m_mono.collectExtraAnim.Play(_m_mono.collectExtraAnimName);
        }


        private void _playSfx(long _sfxId, Transform _sfxParent)
        {
            if (_sfxParent == null)
                return;

            _ISfxObj sfx = PlaySfxMgr.instance.playTDSfx(_sfxId, _sfxParent);
            if (sfx != null)
            {
                _m_sfxList.Add(sfx);
                _limitSfxCount();
            }
        }
        private void _playSfx(long _sfxId, Transform _sfxParent, float _delayTime)
        {
            if (_sfxParent == null)
                return;

            int serialize = _m_loadSerialize;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                if (serialize != _m_loadSerialize)
                    return;
                
                _playSfx(_sfxId, _sfxParent);
            }, _delayTime);
        }
        private void _limitSfxCount()
        {
            while (_m_sfxList.Count > MAX_SFX_COUNT)
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