using System;
using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟成员列表item
    /// </summary>
    public class GGUIWndGuildMemberListGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoGuildMemberListGridItem>
    {
        //成员信息
        private GuildMemberInfo _m_memberInfo;
        //玩家信息
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //半身像
        private NPGGuiWndTexture _m_wBody;
        //职位图标
        private NPGGuiWndTexture _m_wPosIcon;
        //职位banner图
        private GGuiWndSprite _m_wBannerIcon;
        //选择勾选框
        private NPGGUIWndCommonToggleEx _m_wCheckToggle;
        //是否是选择模式
        private bool _m_bIsInSelectMode;
        //显示序列号
        private long _m_lShowSerialize;
        //选择回调
        private Action<bool, GuildMemberInfo> _m_aSelectAction;
        //是否需要获取七日最新贡献度
        private bool _m_bNeedGetNewContribute;

        /// <summary>
        /// 选择回调
        /// </summary>
        public Action<bool, GuildMemberInfo> onSelectAction { get { return _m_aSelectAction; } set { _m_aSelectAction = value; } }


        public GGUIWndGuildMemberListGridItem(GGUIMonoGuildMemberListGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bNeedGetNewContribute = true;
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayerIcon?.hideWnd();
            _m_wBannerIcon?.hideWnd();
            _m_wBody?.hideWnd();
            _m_wPosIcon?.hideWnd();
            _m_wCheckToggle?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
            _m_wBannerIcon?.discardTexture();
            _m_wBody?.discardTexture();
            _m_wPosIcon?.discardTexture();
            _m_wCheckToggle?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wPlayerIcon?.resetWnd();
            _m_wBannerIcon?.discardTexture();
            _m_wBody?.discardTexture();
            _m_wPosIcon?.discardTexture();
            _m_wCheckToggle?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wPlayerIcon != null)
            {
                _m_wPlayerIcon.discard();
                _m_wPlayerIcon.onClickAction += _onPlayerIconClick;
            }
            _m_wPlayerIcon = null;
            _m_wBannerIcon?.discard();
            _m_wBannerIcon = null;
            _m_wBody?.discard();
            _m_wBody = null;
            _m_wPosIcon?.discard();
            _m_wPosIcon = null;
            _m_wCheckToggle?.discard();
            _m_wCheckToggle = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnManage, _onClickManage);
        }

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

            if (wnd.imgBody != null)
                _m_wBody = new NPGGuiWndTexture(wnd.imgBody);

            if (wnd.imgPosIcon != null)
                _m_wPosIcon = new NPGGuiWndTexture(wnd.imgPosIcon);

            if (wnd.monoCheckToggle != null)
            {
                _m_wCheckToggle = new NPGGUIWndCommonToggleEx(wnd.monoCheckToggle);
                _m_wCheckToggle.clickDelegate += _onClickCheckToggle;
            }

            ALUGUICommon.combineBtnClick(wnd.btnManage, _onClickManage);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(GuildMemberInfo _info, bool _selectMode = false)
        {
            if (wnd == null)
                return;

            _m_memberInfo = _info;
            _m_bIsInSelectMode = _selectMode;

            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _m_memberInfo == null);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, _m_memberInfo != null);

            if (_m_memberInfo != null)
                _refreshWnd();
        }

        /// <summary>
        /// 设置是否选中勾选框
        /// </summary>
        /// <param name="_isCheck"></param>
        public void setCheckMark(bool _isCheck)
        {
            if (_m_wCheckToggle == null || _m_memberInfo == null)
                return;

            _m_wCheckToggle.setState(_isCheck);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshPlayerInfo();
            _refreshOnlineState();
            _refreshIcon();
            _refreshState();
        }

        //刷新成员信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_memberInfo == null)
                return;

            if (_m_wPlayerIcon != null)
            {
                _m_wPlayerIcon.showWnd();
                _m_wPlayerIcon.setPlayerInfo(_m_memberInfo.playerDetailInfo, false);
            }

            if (_m_wBody != null)
            {
                _m_wBody.showWnd();
                _m_wBody.setTexture(_m_memberInfo.playerDetailInfo?.skinRef?.card_image);
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
        private void _refreshOnlineState()
        {
            if (wnd == null || _m_memberInfo == null || _m_memberInfo.playerDetailInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtOffLineTime, TextTranslate.instance.getLanguage(TransKeyConst.friends_offline_time_str, TimeUtil.getPassTimeShow(_m_memberInfo.playerDetailInfo.lastOfflineMs)));
            bool isOnline = _m_memberInfo.playerDetailInfo.isOnline;
            ALUGUICommon.setGameObjEnable(wnd.goOnlineShowList, isOnline);
            ALUGUICommon.setGameObjEnable(wnd.goOnlineHideList, !isOnline);
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

        //刷新状态
        private void _refreshState()
        {
            if (wnd == null)
                return;

            if (_m_memberInfo == null)
                return;

            //如果是自己则不展示勾选框
            if (_m_memberInfo.cid == NPPlayer.instance.playerInfo.CID && wnd.needHideSelfCheckMark)
                _m_wCheckToggle?.hideWnd();
            else
                _m_wCheckToggle?.showWnd();

            ALUGUICommon.setGameObjEnable(wnd.goSelectModeShowList, _m_bIsInSelectMode);
            ALUGUICommon.setGameObjEnable(wnd.goSelectModeHideList, !_m_bIsInSelectMode);

            //设置管理按钮显隐
            bool canShowManageBtn = false;
            if (_m_memberInfo.cid == NPPlayer.instance.playerInfo.CID)
            {
                //是自己不显示管理按钮
                canShowManageBtn = false;
            }
            else
            {
                GuildPositionRefObj positionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_m_memberInfo.positionId);
                GuildPositionRefObj selfPositionRef = NPPlayer.instance.guildComp.guildInfo?.getSelfPositionRef();

                if (positionRef != null)
                {
                    //是否有权限
                    canShowManageBtn = NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.TRANSFER_LEADER) ||//有转让盟主权限
                                       NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.KICK_OUT) ||//有提出联盟权限
                                       (NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.POSITION_APPOINT) &&//有任命的权限
                                        selfPositionRef != null &&
                                        selfPositionRef.can_appoint_position_list != null &&
                                        selfPositionRef.can_appoint_position_list.Contains(positionRef.type));
                }

            }
            ALUGUICommon.setGameObjEnable(wnd.btnManage, canShowManageBtn);
        }

        //点击选中框
        private void _onClickCheckToggle(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null || _m_wCheckToggle == null)
                return;

            _m_wCheckToggle.setSelected(!_toggle.isOn);
            _m_aSelectAction?.Invoke(_m_wCheckToggle.isOn, _m_memberInfo);
        }

        //点击管理按钮
        private void _onClickManage(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildAppoint.instance, () =>
            {
                GGUIWndGuildAppoint.instance.showWnd();
                GGUIWndGuildAppoint.instance.setInfo(_m_memberInfo);
            }, UINodeTagConst_Guild.C_GUILD_APPOINT);
        }

        //点击玩家头像
        private void _onPlayerIconClick()
        {
            if (_m_memberInfo == null)
                return;

            long serialize = _m_lShowSerialize;
            GCommon.reqPlayerInfo(_m_memberInfo.cid, (_info) =>
            {
                if (serialize != _m_lShowSerialize || wnd == null || _m_wPlayerIcon == null || _m_wPlayerIcon.wnd == null)
                    return;

                RectTransform targetRectTransform = (null != _m_wPlayerIcon.wnd.locateRectTrans) ? _m_wPlayerIcon.wnd.locateRectTrans : _m_wPlayerIcon?.rectTransform;
                GCommon.showPlayerInfoWndTip(_info, targetRectTransform, 0, Game.instance.mainCamera?.uiRootRectTrans);
            });
        }
    }
}
