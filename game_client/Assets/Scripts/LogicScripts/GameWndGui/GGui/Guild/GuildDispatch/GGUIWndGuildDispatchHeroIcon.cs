using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟派遣大臣item
    /// </summary>
    public class GGUIWndGuildDispatchHeroIcon : _ATALBasicUISubWnd<GGUIMonoGuildDispatchHeroIcon>
    {
        private Common.GuildObj.Guild_DispatchHeroInfo _m_iDispatchHeroInfo;//派遣大臣信息
        private _IHeroCardShow _m_iHeroCardShow;//大臣卡牌展示
        private GGUIWndHeroCommonCardItem _m_wHeroCard;//伙伴基础信息item
        private CommonUISfxObj _m_sfxObj;//特效对象

        public GGUIWndGuildDispatchHeroIcon(GGUIMonoGuildDispatchHeroIcon _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoHeroCard != null)
                _m_wHeroCard = new GGUIWndHeroCommonCardItem(wnd.monoHeroCard);
        }
        
        protected override void _onDiscard()
        {
            _m_wHeroCard?.discard();
            _m_wHeroCard = null;
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroCard?.hideWnd();
            _m_sfxObj?.forceDiscard();
            _m_sfxObj = null;
        }

        protected override void _onReset()
        {
            _m_wHeroCard?.resetWnd();
        }

        public void setData(Common.GuildObj.Guild_DispatchHeroInfo _dispatchHeroInfo)
        {
            _m_iDispatchHeroInfo = _dispatchHeroInfo;
            _m_iHeroCardShow = null;
            if (_m_iDispatchHeroInfo != null)
            {
                _m_iHeroCardShow = new HeroCardShowInfo(null, GRefdataCoreMgr.instance.heroRefCore.getRef(_m_iDispatchHeroInfo.getHeroId()));
            }

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            if (_m_iDispatchHeroInfo == null || _m_iHeroCardShow == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noHeroShow, true);
                _m_wHeroCard?.hideWnd();
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.noHeroShow, false);
         
                if (_m_wHeroCard != null)
                {
                    _m_wHeroCard.showWnd();
                    _m_wHeroCard.setInfo(_m_iHeroCardShow);
                }

                GuildMemberInfo memberInfo = NPPlayer.instance.guildComp.guildInfo?.getMemberInfo(_m_iDispatchHeroInfo.getCid());
                memberInfo?.getPlayerDetailInfo((_detailInfo) =>
                {
                    if(_detailInfo == null)
                        return;
                
                    ALUGUICommon.setLabelTxt(wnd.txtPlayerName, _detailInfo.name);
                }, false);

                ALUGUICommon.setLabelTxt(wnd.txtAddPro,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, (long)Math.Ceiling(_m_iDispatchHeroInfo.getAddPer() / 100d)));

                //播放派遣特效
                if (NPPlayer.instance.guildComp.recordLastSelectDispatchHeroId == _m_iDispatchHeroInfo.getHeroId() && NPPlayer.instance.playerInfo.CID == _m_iDispatchHeroInfo.getCid())
                {
                    //重置记录
                    NPPlayer.instance.guildComp.recordLastSelectDispatchHeroId = 0;

                    //先消耗掉之前的特效
                    _m_sfxObj?.forceDiscard();
                    _m_sfxObj = null;
                    //播放新的特效
                    if(wnd.dispatchSfxId > 0 && wnd.dispatchSfxParent != null)
                        _m_sfxObj = PlaySfxMgr.instance.playUISfx(wnd.dispatchSfxId, wnd.dispatchSfxParent);
                }
            }
        }
    }
}