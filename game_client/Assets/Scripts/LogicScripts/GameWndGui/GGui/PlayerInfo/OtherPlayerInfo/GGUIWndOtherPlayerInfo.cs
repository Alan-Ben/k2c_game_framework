using UnityEngine;
using ALPackage;
using CommonEnum;

namespace GOE
{
    // 其他玩家的信息
    public class GGUIWndOtherPlayerInfo : _ANPGGUIBasicResBarWnd<GGUIMonoOtherPlayerInfo>
    {

        private static GGUIWndOtherPlayerInfo _g_instance;
        public static GGUIWndOtherPlayerInfo instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndOtherPlayerInfo();
                return _g_instance;
            }
        }

        //经验条
        private NPGGUIWndProgress _m_pExpProgress;
        //赚速条
        private NPGGUIWndProgress _m_pEarningsProgress;

        protected GGUIWndOtherPlayerInfo() : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoOtherPlayerInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoOtherPlayerInfo.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /// <summary>
        /// 玩家数据
        /// </summary>
        private NPCommonSimplePlayerInfo _m_playerInfo;

        /// <summary>
        /// 玩家信息
        /// </summary>
        private NPGGUIWndPlayerIcon _m_playerIconWnd;

        /// <summary>
        /// 玩家形象
        /// </summary>
        private NPGGUIWndCommonShowCase _m_playerShowcase;

        // /// <summary>
        // /// 称号总览
        // /// </summary>
        // private GGUIWndOtherPlayerTitleListItemGrid _m_titleGridWnd;

        /// <summary>
        /// 属性列表子窗口 - 临时先用骑士的
        /// </summary>
        // private GGUIWndHeroCommonAttr _m_wHeroAttrListWnd;

        /// <summary>
        /// 展开关闭属性列表的toggle
        /// </summary>
        private NPGGUIWndCommonToggleEx _m_toggleExWnd;

        /// <summary>
        /// 展开关闭称号总览列表的toggle
        /// </summary>
        private NPGGUIWndCommonToggleEx _m_allTitleToggleExWnd;

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.playerShowcase)
                _m_playerShowcase = new NPGGUIWndCommonShowCase(wnd.playerShowcase);
            if (null != wnd.playerIconMono)
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIconMono);
            // if (null != wnd.titleGridMono)
            //     _m_titleGridWnd = new GGUIWndOtherPlayerTitleListItemGrid(wnd.titleGridMono);
            // if (null != wnd.monoAttrList)
            //     _m_wHeroAttrListWnd = new GGUIWndHeroCommonAttr(wnd.monoAttrList);
            if (null != wnd.toggleExMono)
            {
                _m_toggleExWnd = new NPGGUIWndCommonToggleEx(wnd.toggleExMono);
                _m_toggleExWnd.clickDelegate += _onClickShowAttrList;
                _m_toggleExWnd.setSelected(false);
            }
            if (null != wnd.allTitleToggleExMono)
            {
                _m_allTitleToggleExWnd = new NPGGUIWndCommonToggleEx(wnd.allTitleToggleExMono);
                _m_allTitleToggleExWnd.clickDelegate += _onClickShowAllTitle;
                _m_allTitleToggleExWnd.setSelected(false);
            }
            if (null != wnd.expProgress)
                _m_pExpProgress = new NPGGUIWndProgress(wnd.expProgress);
            if (null != wnd.earningsProgress)
                _m_pEarningsProgress = new NPGGUIWndProgress(wnd.earningsProgress);

            ALUGUICommon.combineBtnClick(wnd.copyBtn, _onClickCopyBtn);
            ALUGUICommon.combineBtnClick(wnd.chatBtn, _onClickChatBtn);
            ALUGUICommon.combineBtnClick(wnd.deleteBtn, _onClickDeleteBtn);
            ALUGUICommon.combineBtnClick(wnd.addFriendBtn, _onClickAddFriendBtn);
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onClickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnShield, _onClickShieldBtn);
            ALUGUICommon.combineBtnClick(wnd.btnUnShield, _onClickUnShieldBtn);
            ALUGUICommon.combineBtnClick(wnd.likeBtn, _onBtnLike);
            ALUGUICommon.combineBtnClick(wnd.btnReport, _onClickReportBtn);
        }

        protected override void _onShowWnd()
        {
            _refresh();
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _refreshShield);
        }

        protected override void _onHideWnd()
        {
            if (_m_playerShowcase != null)
                _m_playerShowcase.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _refreshShield);
        }

        protected override void _onReset()
        {

        }
        protected override void _onDiscard()
        {
            // if (null != _m_titleGridWnd)
            //     _m_titleGridWnd.discard();
            // _m_titleGridWnd = null;

            if (null != _m_playerShowcase)
                _m_playerShowcase.discard();
            _m_playerShowcase = null;

            if (null != _m_playerIconWnd)
                _m_playerIconWnd.discard();
            _m_playerIconWnd = null;

            if (null != _m_toggleExWnd)
            {
                _m_toggleExWnd.clickDelegate -= _onClickShowAttrList;
                _m_toggleExWnd.discard();
            }
            _m_toggleExWnd = null;

            if (null != _m_allTitleToggleExWnd)
            {
                _m_allTitleToggleExWnd.clickDelegate -= _onClickShowAllTitle;
                _m_allTitleToggleExWnd.discard();
            }
            _m_allTitleToggleExWnd = null;

            if (null != _m_pExpProgress)
                _m_pExpProgress.discard();
            _m_pExpProgress = null;

            if (null != _m_pEarningsProgress)
                _m_pEarningsProgress.discard();
            _m_pEarningsProgress = null;
            // _m_wHeroAttrListWnd?.discard();
            // _m_wHeroAttrListWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.copyBtn, _onClickCopyBtn);
            ALUGUICommon.uncombineBtnClick(wnd.chatBtn, _onClickChatBtn);
            ALUGUICommon.uncombineBtnClick(wnd.deleteBtn, _onClickDeleteBtn);
            ALUGUICommon.uncombineBtnClick(wnd.addFriendBtn, _onClickAddFriendBtn);
            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onClickCloseBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnShield, _onClickShieldBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnUnShield, _onClickUnShieldBtn);
            ALUGUICommon.uncombineBtnClick(wnd.likeBtn, _onBtnLike);
            ALUGUICommon.uncombineBtnClick(wnd.btnReport, _onClickReportBtn);

        }

        public void setData(NPCommonSimplePlayerInfo _info)
        {
            if (null == _info)
                return;

            //重置一下选中状态
            _m_toggleExWnd?.setSelected(false, true, false);
            _m_playerInfo = _info;
            _refresh();
        }

        private void _refresh()
        {
            if (null == wnd || null == _m_playerInfo)
                return;

            _setPlayerInfo();

            // //刷新属性列表
            // if (null != _m_wHeroAttrListWnd && null != _m_playerInfo.powerDetailList && _m_playerInfo.powerDetailList.Count > (int)EBasicAttrType.LEAD)
            // {
            //     List<HeroAttrItemInfo> attrList = new List<HeroAttrItemInfo>();
            //     attrList.Add(new HeroAttrItemInfo(EBasicAttrType.STR, _m_playerInfo.powerDetailList[(int)EBasicAttrType.STR], false));
            //     attrList.Add(new HeroAttrItemInfo(EBasicAttrType.INT, _m_playerInfo.powerDetailList[(int)EBasicAttrType.INT], false));
            //     attrList.Add(new HeroAttrItemInfo(EBasicAttrType.POL, _m_playerInfo.powerDetailList[(int)EBasicAttrType.POL], false));
            //     attrList.Add(new HeroAttrItemInfo(EBasicAttrType.LEAD, _m_playerInfo.powerDetailList[(int)EBasicAttrType.LEAD], false));
            //     _m_wHeroAttrListWnd.setInfo(attrList, true);
            // }

            ALUGUICommon.setGameObjEnable(wnd.noUnionShowGoList, _m_playerInfo.guildId <= 0);
            ALUGUICommon.setGameObjEnable(wnd.noUnionHideGoList, _m_playerInfo.guildId > 0);

            ALUGUICommon.setGameObjEnable(wnd.likeClickShow, false);
            _refreshShield();
            _refreshLike();
        }    

        private void _refreshShield()
        {
            if (null == wnd || null == _m_playerInfo)
                return;
            bool isShield = NPPlayer.instance.friendsComp.isShield(_m_playerInfo.cid);
            
            ALUGUICommon.setGameObjEnable(wnd.canShieldShow, !isShield);
            ALUGUICommon.setGameObjEnable(wnd.btnUnShield, isShield);
        }  
        

        /// <summary>
        /// 刷新点赞相关
        /// </summary>
        private void _refreshLike()
        {
            // if (null == _m_beLikeShowInfo)
            //     return;
            // ALUGUICommon.setLabelTxt(wnd.txtLikeCount, _m_beLikeShowInfo.beLikeCount);
            // EPlayerLikeStat likeStat = _m_beLikeShowInfo.getLikeStat();
            // NPCommonEnumStatInfo<EPlayerLikeStat>.setStat(wnd.likesEnumStatInfos, likeStat);
        }

        /// <summary>
        /// 设置玩家信息
        /// </summary>
        private void _setPlayerInfo()
        {
            if (null == wnd || null == _m_playerInfo)
                return;

            if (null != _m_playerShowcase)
                _m_playerShowcase.showWnd(new ShowCaseCommonResUnitInfoObj(_m_playerInfo.skinRef?.td_show));
            if (null != _m_playerIconWnd)
                _m_playerIconWnd.setPlayerInfo(_m_playerInfo);
            bool isFriend = NPPlayer.instance.friendsComp.isFriend(_m_playerInfo.cid);
            ALUGUICommon.setGameObjEnable(wnd.isFriendShowGoList, isFriend);
            ALUGUICommon.setGameObjEnable(wnd.noFriendShowGoList, !isFriend);

            // if (null != _m_titleGridWnd)
            //     _m_titleGridWnd.setTitleIdList(_m_playerInfo.allTitleIdList);

            PlayerLvlRefObj nextLvlRefObj = GRefdataCoreMgr.instance.playerLvlCore.getRef(_m_playerInfo.level + 1);
            //设置玩家经验条，用经验值减去当前等级的初始经验
            if (null != _m_pExpProgress && _m_playerInfo.levelRef != null)
            {
                long showExp = _m_playerInfo.exp - _m_playerInfo.levelRef.exp;
                if (showExp < 0)
                    showExp = 0;

                //是否满级
                if(nextLvlRefObj != null)
                    _m_pExpProgress.setProgress(showExp, (nextLvlRefObj.exp - _m_playerInfo.levelRef.exp), EValueFormatType.NORMAL_NOT_LARGE_STR);
                else
                {
                    _m_pExpProgress.setProgress(showExp, showExp, EValueFormatType.NORMAL_NOT_LARGE_STR);
                    _m_pExpProgress.setProgressTxt(TextTranslate.instance.getLanguage(TransKeyConst.common_max_none), true);
                }
            }
            //设置玩家赚速条，用经验值减去当前等级的初始经验
            if (null != _m_pEarningsProgress)
            {
                //是否满级
                if (nextLvlRefObj != null)
                    _m_pEarningsProgress.setProgress(_m_playerInfo.earnings, nextLvlRefObj.earnings, EValueFormatType.GOLD, null);
                else
                {
                    _m_pEarningsProgress.setProgress(_m_playerInfo.earnings, _m_playerInfo.earnings, EValueFormatType.GOLD, null);
                    _m_pEarningsProgress.setProgressTxt(TextTranslate.instance.getLanguage(TransKeyConst.common_max_none), true);
                }
            }
        }

        #region 点击事件

        private void _onBtnLike(GameObject obj)
        {
            // if (null == _m_beLikeShowInfo)
            //     return;
            //
            // NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.collect_likes_fixed_id);
            // if (fixedCdInfo != null && fixedCdInfo.getCount() == 0)
            // {
            //     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.collect_likes_count_max));
            //     return;
            // }
            // EPlayerLikeStat likeStat = _m_beLikeShowInfo.getLikeStat();
            // if (likeStat == EPlayerLikeStat.HAS_LIKE)
            // {
            //     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.collect_likes_has_likeed));
            //     return;
            // }
            //
            // if (likeStat == EPlayerLikeStat.NONE)
            // {
            //     //不能给自己点赞
            //     return;
            // }
            //
            // _m_beLikeShowInfo?.dealLike(() =>
            // {
            //     ALUGUICommon.setGameObjEnable(wnd.likeClickShow, true);
            //     _refreshLike();
            // });
        }
        //点击复制cid
        private void _onClickCopyBtn(GameObject _btn)
        {
            if (null == _m_playerInfo)
                return;

            GUIUtility.systemCopyBuffer = _m_playerInfo.cid.ToString();
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_copySuc_str));
        }

        private void _onClickChatBtn(GameObject _btn)
        {
            if (null == _m_playerInfo)
                return;

            GCommon.jumpToPrivateChat(_m_playerInfo.cid);
        }

        private void _onClickDeleteBtn(GameObject _btn)
        {
            if (null == _m_playerInfo)
                return;

            //提示删除
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.friends_sure_delete_str, _m_playerInfo.name))
                  , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                  , null
                  , TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                  , () =>
                  {
                      NPPlayer.instance.friendsComp.reqRemoveFriend(_m_playerInfo.cid, () =>
                      {
                          //关闭本弹窗
                          QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_OtherPlayerInfoNode);
                          NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_delete_suc_none);
                      });
                  });
        }

        private void _onClickAddFriendBtn(GameObject _btn)
        {
            if (null == _m_playerInfo)
                return;

            FriendCommon.sendAddFriendRequest(_m_playerInfo.cid);
        }

        //点击展开关闭属性列表
        private void _onClickShowAttrList(NPGGUIWndCommonToggleEx _toggle)
        {
            if (null == _toggle)
                return;

            _m_toggleExWnd.setSelected(!_toggle.isOn);
        }

        private void _onClickShowAllTitle(NPGGUIWndCommonToggleEx _toggle)
        {
            if (null == _toggle)
                return;

            _m_allTitleToggleExWnd.setSelected(!_toggle.isOn);
        }

        private void _onClickCloseBtn(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_OtherPlayerInfoNode);
        }

        private void _onClickShieldBtn(GameObject _)
        {
            FriendCommon.setShieldPlayer(_m_playerInfo);
        }

        private void _onClickUnShieldBtn(GameObject _)
        {
            FriendCommon.setUnShieldPlayer(_m_playerInfo);
        }

        //点击举报按钮
        private void _onClickReportBtn(GameObject _)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerReportEditor.instance, () =>
            {
                GGUIWndPlayerReportEditor.instance.showWnd();
                GGUIWndPlayerReportEditor.instance.setInfo(_m_playerInfo.cid);
            }, UINodeTagConst_PlayerInfo.C_ADD_PLAYER_REPORT);
        }

        #endregion
    }
}
