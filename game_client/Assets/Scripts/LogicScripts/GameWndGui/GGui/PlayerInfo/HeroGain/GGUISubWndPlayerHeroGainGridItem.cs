using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndPlayerHeroGainGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoPlayerHeroGainGridItem>
    {
        private NPGGuiWndTexture _m_heroIconWnd;
        private NPGGuiWndTexture _m_attrIconWnd;
        
        private PlayerHeroUnlockShowRefObj _m_heroShowRef;
        //是否是第一个未解锁的伙伴
        private bool _m_bIsFirstLock;
        
        
        public GGUISubWndPlayerHeroGainGridItem(GGUIMonoPlayerHeroGainGridItem _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_heroIconWnd?.showWnd();
            _m_attrIconWnd?.showWnd();

            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_heroIconWnd?.hideWnd();
            _m_attrIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_heroIconWnd?.discardTexture();
            _m_attrIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_heroIconWnd?.discard();
            _m_attrIconWnd?.discard();
            _m_heroIconWnd = null;
            _m_attrIconWnd = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnJump, _onJumpBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnLockDetail, _onClickLockDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnUnLockDetail, _onClickUnlockDetail);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgHero != null)
                _m_heroIconWnd = new NPGGuiWndTexture(wnd.imgHero);
            if (wnd.imgAttr != null)
                _m_attrIconWnd = new NPGGuiWndTexture(wnd.imgAttr);
            
            ALUGUICommon.combineBtnClick(wnd.btnJump, _onJumpBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnLockDetail, _onClickLockDetail);
            ALUGUICommon.combineBtnClick(wnd.btnUnLockDetail, _onClickUnlockDetail);
        }
        protected override void _resetGridItem()
        {
        }
        

        public void refreshWnd(PlayerHeroUnlockShowRefObj _heroRef, bool _isFirstLock)
        {
            _m_heroShowRef = _heroRef;
            _m_bIsFirstLock = _isFirstLock;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_heroShowRef == null)
                return;
            
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroShowRef.hero_id);
            if (heroRef == null)
            {
                ALLog.Error($"[GGUISubWndPlayerHeroGainGridItem] refreshWnd heroRef is null, hero_id: {_m_heroShowRef.hero_id}");
                return;                
            }

            BasicAttrRefObj attrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)heroRef.spec_attr_type);
            if (attrRef == null)
            {
                ALLog.Error($"[GGUISubWndPlayerHeroGainGridItem] refreshWnd attrRef is null, spec_attr_type: {heroRef.spec_attr_type}");
                return;
            }
            
            _m_heroIconWnd?.setTexture(heroRef.icon);
            _m_attrIconWnd?.setTexture(attrRef.icon);
            ALUGUICommon.setLabelTxt(wnd.txtHeroName, heroRef.transName);
            ALUGUICommon.setLabelTxt(wnd.txtAttrName, TextTranslate.instance.getLanguage(attrRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(_m_heroShowRef.unlock_desc, _m_heroShowRef.unlock_desc_params));
            wnd.setUnlockState(_m_heroShowRef.unlock_condition?.IsEnable(null) ?? true, NPPlayer.instance.heroComponent.getHeroInfo(_m_heroShowRef.hero_id) != null, _m_bIsFirstLock);

            //刷新红点，没有点击过前往按钮，或者满足解锁条件但未获得伙伴且满足获取条件
            bool isShowRedTip = !AccountSettingMgr.instance.accountSetting.isClickPlayerGainHeroJump(_m_heroShowRef.id) ||
                                (!NPPlayer.instance.heroComponent.isHeroUnlock(_m_heroShowRef.hero_id) && 
                                 (_m_heroShowRef.unlock_condition == null || _m_heroShowRef.unlock_condition.IsEnable(null)) && 
                                  (_m_heroShowRef.get_condition == null || _m_heroShowRef.get_condition.IsEnable(null)));
            ALUGUICommon.setGameObjEnable(wnd.goRedTip, isShowRedTip);
        }

        //点击前往
        private void _onJumpBtnClick(GameObject _btn)
        {
            if(_m_heroShowRef == null)
                return;

            //设置非强制红点已读
            AccountSettingMgr.instance.accountSetting.addClickPlayerGainHeroJumpId(_m_heroShowRef.id);
            NPPlayer.instance.playerInfo.refreshUnlockHeroRedTip();

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYER_HERO_GAIN);
            _m_heroShowRef?.jump_effect?.dealEffect(null);
        }

        // 点击未获得伙伴详情按钮
        private void _onClickLockDetail(GameObject _go)
        {
            if (_m_heroShowRef == null)
                return;

            HeroCardShowInfo showInfo = new HeroCardShowInfo(null, GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroShowRef.hero_id));
            QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(showInfo, null));
        }

        // 点击已获得伙伴详情按钮
        private void _onClickUnlockDetail(GameObject _go)
        {
            if (_m_heroShowRef == null)
                return;

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_heroShowRef.hero_id);
            HeroCardShowInfo showInfo = new HeroCardShowInfo(heroInfo, GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroShowRef.hero_id));
            QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(showInfo, null));
        }
    }
}