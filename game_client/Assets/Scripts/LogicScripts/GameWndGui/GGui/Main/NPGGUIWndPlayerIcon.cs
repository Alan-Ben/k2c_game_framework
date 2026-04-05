using UnityEngine;
using System;
using ALPackage;
using Common.NpChatObj;
using NPCommon;
using NPEnum;


namespace GOE
{
    public class NPGGUIWndPlayerIcon : _ANPGGUIBasicSubWnd<NPGGUIMonoPlayerIcon>
    {
        // 头像
        private NPGGuiWndTexture _m_wtIconWnd;

        // 头像资源
        private GGUIWndPrefabSubDressItem _m_iconPrefabSubDressItem;

        // 头像框
        private NPGGuiWndTexture _m_wIconBgkWnd;

        // 头像框资源
        private GGUIWndPrefabSubDressItem _m_iconBgkPrefabSubDressItem;

        // 半身像
        private NPGGuiWndTexture _m_wCardWnd;

        // 称号
        private GGUIWndSubPlayerTitle _m_playerTitleWnd;
        //等级
        private GGUIWndPlayerLv _m_playerLvWnd;

        //数据
        private NPCommonSimplePlayerInfo _m_playerInfo;
        
        private long _m_lShowSerial;

        public Action onClickAction;

        public NPGGUIWndPlayerIcon(NPGGUIMonoPlayerIcon _wnd)
           : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerial = ALSerializeOpMgr.next();
            
            if (null != _m_wtIconWnd)
                _m_wtIconWnd.showWnd();

            if (_m_wIconBgkWnd != null)
                _m_wIconBgkWnd.showWnd();
            
            if (_m_wCardWnd != null)
                _m_wCardWnd.showWnd();

            if (null != _m_playerTitleWnd)
                _m_playerTitleWnd.showWnd();

            if (null != _m_playerLvWnd)
                _m_playerLvWnd.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerial = ALSerializeOpMgr.next();

            if (null != _m_wtIconWnd)
                _m_wtIconWnd.hideWnd();

            if (_m_wIconBgkWnd != null)
                _m_wIconBgkWnd.hideWnd();
            
            if (_m_wCardWnd != null)
                _m_wCardWnd.hideWnd();

            if (null != _m_playerTitleWnd)
                _m_playerTitleWnd.hideWnd();

            if (null != _m_playerLvWnd)
                _m_playerLvWnd.hideWnd();

            _m_iconPrefabSubDressItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_iconPrefabSubDressItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(null != _m_wtIconWnd)
                _m_wtIconWnd.discard();
            _m_wtIconWnd = null;

            if(_m_wIconBgkWnd != null)
                _m_wIconBgkWnd.discard();
            _m_wIconBgkWnd = null;
            
            if (_m_wCardWnd != null)
                _m_wCardWnd.discard();

            if (null != _m_playerTitleWnd)
                _m_playerTitleWnd.discard();
            _m_playerTitleWnd = null;

            if (null != _m_playerLvWnd)
                _m_playerLvWnd.discard();
            _m_playerLvWnd = null;

            _m_iconPrefabSubDressItem?.discard();
            _m_iconPrefabSubDressItem = null;

            _m_iconBgkPrefabSubDressItem?.discard();
            _m_iconBgkPrefabSubDressItem = null;

            onClickAction = null;
            _m_playerInfo = null;
            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickInfo);
            ALUGUICommon.uncombineBtnClick(wnd.btnToPlayerDetail, _onClickToPlayerDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnEarningsDetail, _onClickEarningsDetail);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            // 头像
            if(null != wnd.imgIcon)
                _m_wtIconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            // 头像框
            if(wnd.imgIconBgk != null)
                _m_wIconBgkWnd = new NPGGuiWndTexture(wnd.imgIconBgk);
            
            // 半身像
            if (wnd.imgCard != null)
                _m_wCardWnd = new NPGGuiWndTexture(wnd.imgCard);

            //称号
            if (null != wnd.monoSubPlayerTitle)
                _m_playerTitleWnd = new GGUIWndSubPlayerTitle(wnd.monoSubPlayerTitle);

            //等级
            if (null != wnd.playerLvMono)
                _m_playerLvWnd = new GGUIWndPlayerLv(wnd.playerLvMono);

            ALUGUICommon.combineBtnClick(wnd.btnClick,_onClickInfo);
            ALUGUICommon.combineBtnClick(wnd.btnToPlayerDetail, _onClickToPlayerDetail);
            ALUGUICommon.combineBtnClick(wnd.btnEarningsDetail, _onClickEarningsDetail);

        }

        /// <summary>
        /// 点击信息按钮
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickInfo(GameObject _obj)
        {
            if (null != onClickAction)
            {
                onClickAction();
            }
        }

        /// <summary>
        /// 点击跳转玩家详情
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickToPlayerDetail(GameObject _go)
        {
            FriendCommon.showPlayerInfo(_m_playerInfo);
        }

        /// <summary>
        /// 点击查看赚速详情
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickEarningsDetail(GameObject _go)
        {
            if (wnd == null || _go == null)
                return;

            QueueMgr.instance.AddNode(new GNodePlayerEarningsDetailToolTip((RectTransform)_go.transform, wnd.earningDetailToolTipInterval.x, wnd.earningDetailToolTipInterval.y));
        }

        /// <summary>
        /// 设置玩家信息
        /// </summary>
        /// <param name="_info"></param>
        public void setPlayerInfo(NPCommonSimplePlayerInfo _info,bool _isSelf = true)
        {
            if (null == _info || wnd == null)
                return;

            _m_playerInfo = _info;

            // 设置头像和头像框            
            _setIcon(GRefdataCoreMgr.instance.playerIconCore.getRef(_info.icon), _isSelf);
            _setIconBgk(GRefdataCoreMgr.instance.iconBgkCore.getRef(_info.icon_bgk));
            _setCard(_info.skinRef?.card_image);
            _setPlayerTitle(_info.curTitleInfo, _info.isShowTitle);
            _setPlayerLv(_info.level);

            // 设置玩家名字
            setName(_info.name, _info.cid);

            // 设置实力
            setPower(_info.totalPower);

            // 设置玩家id
            ALUGUICommon.setLabelTxt(wnd.txtId, TextTranslate.instance.getLanguage(TransKeyConst.playerID_num, _info.cid));

            //国力
            ALUGUICommon.setLabelTxt(wnd.txtPower, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_allNationPower_num, _info.earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.numPower, _info.earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));

            //服务器信息展示
            ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, ""));//默认展示空
            GCommon.getServerNameByCId(_info.cid, _serverName =>
            {
                if (wnd != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, _serverName));
                    ALUGUICommon.setLabelTxt(wnd.txtServerEx, _serverName);
                }
            });
            
            ALUGUICommon.setLabelTxt(wnd.txtVipLv, TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_vip_num, _info.vipLvl));
            ALUGUICommon.setGameObjEnable(wnd.zeroVipHideGoList, _info.vipLvl != 0);
            GGameCommonInfo.grayImage(wnd.zeroVipGrayList, _info.vipLvl == 0);
            if(_info.guildId == 0)
                ALUGUICommon.setLabelTxt(wnd.txtUnion, TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_noalliancename_des));
            else
                ALUGUICommon.setLabelTxt(wnd.txtUnion, TextTranslate.instance.getLanguage(TransKeyConst.guild_showName_simpleName_name, _info.guildSimpleName, _info.guildName));
            
            ALUGUICommon.setGameObjEnable(wnd.onlineShowList, _info.isOnline);
            ALUGUICommon.setGameObjEnable(wnd.offlineShowList, !_info.isOnline);
        }

        /// <summary>
        /// 设置玩家信息
        /// </summary>
        /// <param name="_info"></param>
        public void setPlayerInfoForChatPlayerCard(NPCommonSimplePlayerInfo _info)
        {
            if(null == _info)
                return;
            setPlayerInfo(_info,_info.cid == NPPlayer.instance.playerInfo.CID);
        }

        // 设置玩家信息
        public void setPlayerInfo(NPCommon_ChatPlayerContent _chatPlayer)
        {
            if (_chatPlayer == null || wnd == null)
                return;

            NPCommonSimplePlayerInfo info = new NPCommonSimplePlayerInfo(_chatPlayer);
            setPlayerInfo(info, info.cid == NPPlayer.instance.playerInfo.CID);
        }

        /// <summary>
        /// 设置显示玩家
        /// </summary>
        /// <param name="_cid"></param>
        public void setPlayer(long _cid, Action _onGetInfo = null)
        {
            long serial = _m_lShowSerial;
            _m_playerInfo = null;
            GCommon.reqPlayerInfo(_cid, (_playerInfo) =>
            {
                if (serial != _m_lShowSerial)
                    return;

                _onGetInfo?.Invoke();
                setPlayerInfo(_playerInfo, _cid == NPPlayer.instance.playerInfo.CID);
            });
        }
        
        /// <summary>
        /// 设置玩家自己的信息,主界面mainplayer使用
        /// </summary>
        public void setSelfInfo()
        {
            if(wnd == null)
                return;
            
            PlayerInfo playerInfo = NPPlayer.instance.playerInfo;

            _m_playerInfo = new NPCommonSimplePlayerInfo();
            _m_playerInfo.cid = playerInfo.CID;

            // 设置头像+头像框
            _setSelfHead();

            // _setPlayerTitle(NPPlayer.instance.playerInfo.getCurrentTitleId());

            setName(playerInfo.PlayerName, playerInfo.CID);

            setPower(NPPlayer.instance.heroComponent.totalPower);

            ALUGUICommon.setLabelTxt(wnd.txtId, TextTranslate.instance.getLanguage(TransKeyConst.playerID_num, playerInfo.CID));

            //服务器信息展示
            ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, ""));//默认展示空
            GCommon.getServerNameByCId(playerInfo.CID, _serverName =>
            {
                if (wnd != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverName_str, _serverName));
                    ALUGUICommon.setLabelTxt(wnd.txtServerEx, _serverName);
                }
            });

            //等级名称
            if (null != _m_playerLvWnd)
                _m_playerLvWnd.setLvRefObj(playerInfo.curLevelRef);

            // vip
            ALUGUICommon.setLabelTxt(wnd.txtVipLv, TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_vip_num, playerInfo.getCurrentVIPLvl()));
            ALUGUICommon.setGameObjEnable(wnd.zeroVipHideGoList, playerInfo.getCurrentVIPLvl() != 0);
            GGameCommonInfo.grayImage(wnd.zeroVipGrayList, playerInfo.getCurrentVIPLvl() == 0);

            //国力
            long allPower = NPPlayer.instance.specialItemComp.goldData.earnings;
            ALUGUICommon.setLabelTxt(wnd.txtPower, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_allNationPower_num, allPower.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.numPower, allPower.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));

            //亲密度
            long allIntimacy = NPPlayer.instance.consortComp.allIntimacyNum;
            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_allIntimacy_num, allIntimacy.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.numIntimacy, allIntimacy.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            //联盟
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null || guildInfo.guildId <= 0)
                ALUGUICommon.setLabelTxt(wnd.txtUnion, TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_noalliancename_des));
            else
                ALUGUICommon.setLabelTxt(wnd.txtUnion, TextTranslate.instance.getLanguage(TransKeyConst.guild_showName_simpleName_name, guildInfo.simpleName, guildInfo.name));
            
            // 因为显示的是玩家自己信息，所以在线状态显示在线
            ALUGUICommon.setGameObjEnable(wnd.onlineShowList, true);
            ALUGUICommon.setGameObjEnable(wnd.offlineShowList, false);
        }

        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_cid"></param>
        public void setName(string _name, long _cid)
        {
            if (wnd == null)
                return;

            if (string.IsNullOrEmpty(wnd.txtNameKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtName, _name);
                ALUGUICommon.setLabelTxt(wnd.txtNameTmp, _name);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(wnd.txtNameKey, _name));
                ALUGUICommon.setLabelTxt(wnd.txtNameTmp, TextTranslate.instance.getLanguage(wnd.txtNameKey, _name));
            }

            //设置带服务器名的名称
            ALUGUICommon.setLabelTxt(wnd.txtNameWithServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverNamePlayerName_str_str, "", _name));//默认展示空服务器名
            GCommon.getServerNameByCId(_cid, _serverName =>
            {
                if (wnd != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtNameWithServer, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_serverNamePlayerName_str_str, _serverName, _name));
                }
            });
        }

        /// <summary>
        /// 设置赚速
        /// </summary>
        public void setEarnings(long _earnings)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtPower, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_allNationPower_num, _earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.numPower, _earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
        }

        /// <summary>
        /// 设置实力
        /// </summary>
        public void setPower(long _power)
        {
            if (wnd == null)
                return;

            //战力
            ALUGUICommon.setLabelTxt(wnd.txtFight, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_allFight_num, _power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
            ALUGUICommon.setLabelTxt(wnd.numFight, _power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }

        // 设置玩家自己的头像+头像框+称号
        private void _setSelfHead()
        {
            NPPlayerIconItem curIcon = NPPlayer.instance.playerInfo.curIcon;
            if(null != curIcon)
                _setIcon(curIcon.playerIconRef, true);

            PlayerIconBgkItem bgkItem = NPPlayer.instance.playerInfo.curIconBgk;
            if(null != bgkItem)
                _setIconBgk(bgkItem.iconBgkRef);

            PlayerSkinRefObj skin = NPPlayer.instance.playerInfo.curSkinRef;
            if (null != skin)
                _setCard(skin.card_image);

            _setPlayerTitle(NPPlayer.instance.titleComp.curTitle, NPPlayer.instance.titleComp.isShowOthers);
        }

        // 设置头像
        private void _setIcon(PlayerIconRefObj _refObj, bool _isSelf)
        {
            if(_refObj == null)
                return;

            NPGTextureIndex iconIndex = GCommon.getItemTexIcon(ENPItemType.ICON, _refObj.id);
            if (null != _m_wtIconWnd)
                _m_wtIconWnd.setTexture(iconIndex);

            GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_iconPrefabSubDressItem, _refObj.asset_path_id, wnd.iconParPos, (_item) =>
            {
                _m_iconPrefabSubDressItem = _item;
                _m_iconPrefabSubDressItem?.showWnd();
                _m_iconPrefabSubDressItem?.setIcon(iconIndex);
            });

            //根据配置，判断是否修改头像的朝向
            if (null == _m_wtIconWnd || null == _m_wtIconWnd.rawImage)
                return;
            Vector3 scale = _m_wtIconWnd.rawImage.transform.localScale;
            scale.x = Math.Abs(scale.x);
            if (wnd.isOverturnOtherPlayerIcon && !_isSelf)
            {
                scale.x = -Math.Abs(scale.x);
            }
            else
            {
                scale.x = Math.Abs(scale.x);
            }
            _m_wtIconWnd.rawImage.transform.localScale = scale;
        }

        /// <summary>
        /// 设置半身像
        /// </summary>
        /// <param name="_cardImg"></param>
        private void _setCard(NPGTextureIndex _cardImg)
        {
            if (_m_wCardWnd != null) 
                _m_wCardWnd.setTexture(_cardImg);
        }

        /// <summary>
        /// 设置称号
        /// </summary>
        /// <param name="_curTitleInfo"></param>
        private void _setPlayerTitle(PlayerInfo_CurTitle _curTitleInfo, bool _isShowTitle)
        {
            if (wnd == null)
                return;

            if (_curTitleInfo != null)
            {
                _m_playerTitleWnd?.showWnd();
                _m_playerTitleWnd?.setInfo(_curTitleInfo);
            }

            ALUGUICommon.setGameObjEnable(wnd.goNoOrPrivateTitleHideList, _isShowTitle && _curTitleInfo != null && _curTitleInfo.getType() != ENPPlayerTitleType.NONE && _curTitleInfo.getInfo() != null);
        }

        //设置等级
        private void _setPlayerLv(long _lv)
        { 
            if (null != _m_playerLvWnd)
                _m_playerLvWnd.setLv(_lv);
        }

        // 设置头像框
        private void _setIconBgk(PlayerIconBgkRefObj _refObj)
        {
            if( _refObj == null)
                return;

            NPGTextureIndex iconBgkIndex = GCommon.getItemTexIcon(ENPItemType.ICON_BGK, _refObj.id);
            if (null != _m_wIconBgkWnd)
                _m_wIconBgkWnd.setTexture(iconBgkIndex);

            GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_iconBgkPrefabSubDressItem, _refObj.asset_path_id, wnd.iconBgkParPos, (_item) =>
            {
                _m_iconBgkPrefabSubDressItem = _item;
                _m_iconBgkPrefabSubDressItem?.showWnd();
                _m_iconBgkPrefabSubDressItem?.setIcon(iconBgkIndex);
            });
        }
    }
}
