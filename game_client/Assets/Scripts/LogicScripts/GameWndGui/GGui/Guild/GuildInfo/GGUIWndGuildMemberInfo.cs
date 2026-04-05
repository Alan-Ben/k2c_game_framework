using System;
using ALPackage;
using Common.GuildEnum;

namespace GOE
{
    /// <summary>
    /// 联盟成员信息子窗口
    /// </summary>
    public class GGUIWndGuildMemberInfo : _ANPGGUIBasicSubWnd<GGUIMonoGuildMemberInfo>
    {
        //成员信息
        private GuildMemberInfo _m_memberInfo;
        
        //玩家信息
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //职位图标
        private NPGGuiWndTexture _m_wPosIcon;
        //职位banner图
        private GGuiWndSprite _m_wBannerIcon;
        
        //显示序列号
        private long _m_lShowSerialize;
        //是否需要获取七日最新贡献度
        private bool _m_bNeedGetNewContribute;
        
        public GGUIWndGuildMemberInfo(GGUIMonoGuildMemberInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action onPlayerIconClick;//当玩家头像被点击时
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.playerIcon != null)
            {
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
                _m_wPlayerIcon.onClickAction += _onPlayerIconClick;
            }

            if (wnd.imgBanner != null)
                _m_wBannerIcon = new GGuiWndSprite(wnd.imgBanner);

            if (wnd.imgPosIcon != null)
                _m_wPosIcon = new NPGGuiWndTexture(wnd.imgPosIcon);
        }
        
        protected override void _onDiscard()
        {
            onPlayerIconClick = null;
            
            if (_m_wPlayerIcon != null)
            {
                _m_wPlayerIcon.onClickAction -= _onPlayerIconClick;
                _m_wPlayerIcon.discard();
                _m_wPlayerIcon = null;    
            }
            
            _m_wBannerIcon?.discard();
            _m_wBannerIcon = null;
            
            _m_wPosIcon?.discard();
            _m_wPosIcon = null;

            if (wnd == null)
                return;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayerIcon?.hideWnd();
            _m_wBannerIcon?.hideWnd();
            _m_wPosIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
            _m_wBannerIcon?.discardTexture();
            _m_wPosIcon?.discardTexture();
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(GuildMemberInfo _info)
        {
            if (_info == null)
                return;

            _m_memberInfo = _info;
            _m_bNeedGetNewContribute = true;
            _refreshWnd(true);
        }

        //刷新窗口
        private void _refreshWnd(bool _showNewest)
        {
            _refreshPlayerInfo(_showNewest);
            _refreshOnlineState(_showNewest);
            _refreshIcon();
        }

        //刷新成员信息
        private void _refreshPlayerInfo(bool _showNewest)
        {
            if (wnd == null || _m_memberInfo == null)
                return;

            //先刷新一次展示
            _refreshPlayerInfo();

            //请求数据再刷新
            long serialize = _m_lShowSerialize;
            _m_memberInfo.getPlayerDetailInfo((info) =>
            {
                if (wnd == null || serialize != _m_lShowSerialize || _m_memberInfo == null || _m_memberInfo.playerDetailInfo == null)
                    return;

                _refreshPlayerInfo();
            }, _showNewest);
        }

        //刷新成员信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_memberInfo == null || _m_memberInfo.playerDetailInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd?.goGettingPlayerInfoHideList, false);
                ALUGUICommon.setGameObjEnable(wnd?.goGettingPlayerInfoShowList, true);
                return;
            }

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd?.goGettingPlayerInfoHideList, true);
            ALUGUICommon.setGameObjEnable(wnd?.goGettingPlayerInfoShowList, false);

            //设置玩家头像
            if (_m_wPlayerIcon != null)
            {
                _m_wPlayerIcon.showWnd();
                _m_wPlayerIcon.setPlayerInfo(_m_memberInfo.playerDetailInfo, false);
            }

            //先重置一下显示
            ALUGUICommon.setLabelTxt(wnd.txtSevenDayContribute, TextTranslate.instance.getLanguage(TransKeyConst.guild_sevenDayContribute_num, 0));
            ALUGUICommon.setLabelTxt(wnd.txtTotalContribute, TextTranslate.instance.getLanguage(TransKeyConst.guild_guildContribute_num, 0));

            //设置贡献度显示
            long serialize = _m_lShowSerialize;
            _m_memberInfo.getContributeInfo(_contributeInfo =>
            {
                if (serialize != _m_lShowSerialize || _contributeInfo == null || wnd == null)
                    return;

                _m_bNeedGetNewContribute = false;
                ALUGUICommon.setLabelTxt(wnd.txtSevenDayContribute,
                    TextTranslate.instance.getLanguage(TransKeyConst.guild_sevenDayContribute_num, _contributeInfo.getSevenDaysContribute()));
                ALUGUICommon.setLabelTxt(wnd.txtTotalContribute,
                    TextTranslate.instance.getLanguage(TransKeyConst.guild_guildContribute_num, _contributeInfo.getTotalContribute()));
            }, _m_bNeedGetNewContribute);
        }

        //刷新在线状态
        private void _refreshOnlineState(bool _showNewest)
        {
            if (wnd == null || _m_memberInfo == null)
                return;

            long serialize = _m_lShowSerialize;
            _m_memberInfo.getPlayerDetailInfo((info) =>
            {
                if (wnd == null || serialize != _m_lShowSerialize || _m_memberInfo == null || _m_memberInfo.playerDetailInfo == null)
                    return;
                
                if (wnd == null || !isShow || _m_memberInfo == null || _m_memberInfo.playerDetailInfo == null)
                    return;
                
                ALUGUICommon.setLabelTxt(wnd.txtOffLineTime, TextTranslate.instance.getLanguage(TransKeyConst.friends_offline_time_str, TimeUtil.getPassTimeShow(_m_memberInfo.playerDetailInfo.lastOfflineMs)));
                bool isOnline = _m_memberInfo.playerDetailInfo.isOnline;
                ALUGUICommon.setGameObjEnable(wnd.goOnlineShowList, isOnline);
                ALUGUICommon.setGameObjEnable(wnd.goOnlineHideList, !isOnline);
                
            }, _showNewest);
        }

        //刷新图标
        private void _refreshIcon()
        {
            if (wnd == null || _m_memberInfo == null)
                return;

            GuildPositionRefObj guildPositionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_m_memberInfo.positionId);
            if (guildPositionRef == null)
                return;

            if (_m_wPosIcon != null)
            {
                _m_wPosIcon.showWnd();
                _m_wPosIcon.setTexture(guildPositionRef.icon);
            }

            if (_m_wBannerIcon != null)
            {
                _m_wBannerIcon.showWnd();
                _m_wBannerIcon.setTexture(guildPositionRef.banner_spt);
            }
        }
        
        private void _onPlayerIconClick()
        {
            if(_m_memberInfo == null)
                return;
            
            if(onPlayerIconClick != null)
                onPlayerIconClick.Invoke();
            else
            {
                long serialize = _m_lShowSerialize;
                GCommon.reqPlayerInfo(_m_memberInfo.cid, (_info) =>
                {
                    if (serialize != _m_lShowSerialize || wnd == null)
                        return;

                    GCommon.showPlayerInfoWndTip(_info, _m_wPlayerIcon?.rectTransform, 0, Game.instance.mainCamera?.uiRootRectTrans);
                });
            }
        }
    }
}