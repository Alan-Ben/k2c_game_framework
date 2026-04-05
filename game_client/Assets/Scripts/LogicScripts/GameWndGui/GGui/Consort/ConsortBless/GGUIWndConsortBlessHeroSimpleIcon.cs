using ALPackage;

namespace GOE
{
    /// <summary>
    /// 妃子加护大臣icon
    /// </summary>
    public class GGUIWndConsortBlessHeroSimpleIcon : _ATALBasicUISubWnd<GGUIMonoConsortBlessHeroSimpleIcon>
    {
        private GGUIWndHeroConsortSimpleIconContainerItem _m_wHeroIcon;
        private CommonUISfxObj _m_oSfxObj;//特效物体

        private long _m_lConsortId;//妃子id
        private long _m_lHeroId;
        
        public GGUIWndConsortBlessHeroSimpleIcon(GGUIMonoConsortBlessHeroSimpleIcon _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoHeroIcon != null)
                _m_wHeroIcon = new GGUIWndHeroConsortSimpleIconContainerItem(wnd.monoHeroIcon);
        }
        
        protected override void _onDiscard()
        {
            _m_wHeroIcon?.discard();
            _m_wHeroIcon = null;

            _discardSfx();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroIcon?.hideWnd();
            
            _discardSfx();
        }

        protected override void _onReset()
        {
            _m_wHeroIcon?.resetWnd();
            
            _discardSfx();
        }

        public void setData(long _consortId, long _heroId)
        {
            _m_lConsortId = _consortId;
            _m_lHeroId = _heroId;

            refreshWnd();
        }

        public void refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            if (_m_lHeroId <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasHeroShow, false);
                ALUGUICommon.setGameObjEnable(wnd.noHeroShow, true);
                
                _m_wHeroIcon?.hideWnd();
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.hasHeroShow, true);
                ALUGUICommon.setGameObjEnable(wnd.noHeroShow, false);
                
                _m_wHeroIcon?.showWnd();
                _m_wHeroIcon?.setInfo(_m_lHeroId, EHeroConsortSimpleIconShowType.HERO);

                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);
                long addPower = consortInfo?.getBlessSkillHeroAddPower(_m_lHeroId) ?? 0;
                if (string.IsNullOrEmpty(wnd.txtAddPowerKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtAddPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, addPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtAddPower, TextTranslate.instance.getLanguage(wnd.txtAddPowerKey, addPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                }
            }
            
            _discardSfx();
        }

        #region 特效

        /// <summary>
        /// 播放特效
        /// </summary>
        public void playSfx(long _sfxId)
        {
            if (wnd == null || wnd.sfxParent == null)
                return;

            _discardSfx();
            
            _m_oSfxObj = PlaySfxMgr.instance.playUISfx(_sfxId, wnd.sfxParent);
        }

        /// <summary>
        /// 销毁特效
        /// </summary>
        private void _discardSfx()
        {
            _m_oSfxObj?.forceDiscard();
            _m_oSfxObj = null;
        }

        #endregion
    }
}