using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndInnMainSignboard : _ATALBasicUISubWnd<GGUIMonoInnMainSignboard>
    {
        private GGUISubWndInnLevelIconContainer _m_starIconContainerWnd;
        private NPGGuiWndTexture _m_medalIconWnd;
        
        public GGUISubWndInnMainSignboard(GGUIMonoInnMainSignboard _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            _m_starIconContainerWnd?.showWnd();
            _m_medalIconWnd?.showWnd();
            refreshWnd();
            
            NPPlayer.instance.innComp.onLevelChg += _onInnLevelChg;
            NPPlayer.instance.innComp.onPopularityChg += _onPopularityChg;
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MEDAL_DETAIL_BUTTON, _onSimulateClickDetailBtn);
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.innComp.onPopularityChg -= _onPopularityChg;
            NPPlayer.instance.innComp.onLevelChg -= _onInnLevelChg;
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_INN_MEDAL_DETAIL_BUTTON, _onSimulateClickDetailBtn);
            
            _m_starIconContainerWnd?.hideWnd();
            _m_medalIconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_starIconContainerWnd?.resetWnd();
            _m_medalIconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_starIconContainerWnd?.discard();
            _m_starIconContainerWnd = null;
            _m_medalIconWnd?.discard();
            _m_medalIconWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoStarIconContainer != null)
                _m_starIconContainerWnd = new GGUISubWndInnLevelIconContainer(wnd.monoStarIconContainer);
            
            if (wnd.imgMedalIcon != null)
                _m_medalIconWnd = new NPGGuiWndTexture(wnd.imgMedalIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            InnLevelRefObj levelRef = NPPlayer.instance.innComp.levelRef;
            InnLevelRefObj nextLevelRef = NPPlayer.instance.innComp.nextLevelRef;
            InnMedalLevelRefObj medalLevelRef = NPPlayer.instance.innComp.medalLevelRef;
            long popularity = NPPlayer.instance.innComp.popularity;

            ALUGUICommon.setLabelTxt(wnd.txtName, levelRef?.nameTranslated);
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, levelRef?.level ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtMedalLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, medalLevelRef?.level ?? 0));
            ALUGUICommon.setLabelTxt(wnd.txtLevelProgress,
                TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num,
                    popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT),
                    nextLevelRef?.need_popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT) ?? popularity.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            if (wnd.sldLevelProgress != null)
            {
                wnd.sldLevelProgress.minValue = levelRef?.need_popularity ?? 0;
                wnd.sldLevelProgress.maxValue = nextLevelRef?.need_popularity ?? levelRef?.need_popularity ?? 0;
                wnd.sldLevelProgress.value = popularity;
            }
            
            wnd.setIsZeroLevel(levelRef == null);
            wnd.setLevelMax(nextLevelRef == null);
            
            // 刷新奖牌图标
            _m_medalIconWnd?.setTexture(levelRef?.medal_icon);
            // 刷新星级图标容器
            _m_starIconContainerWnd?.refreshWnd(levelRef);
        }


        private void _onBtnDetailClick(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnMedalInfo.instance, GGUIWndInnMedalInfo.instance.showWnd, UINodeTagConst.C_INN_MEDAL_INFO);
        }
        private void _onInnLevelChg()
        {
            refreshWnd();
        }
        private void _onPopularityChg()
        {
            refreshWnd();
        }
        /// <summary>
        /// 模拟点击详情按钮
        /// </summary>
        private void _onSimulateClickDetailBtn()
        {
            _onBtnDetailClick(null);
        }
    }
}