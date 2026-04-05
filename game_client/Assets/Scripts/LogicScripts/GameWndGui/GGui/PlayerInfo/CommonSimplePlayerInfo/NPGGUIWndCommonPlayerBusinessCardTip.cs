using System;
using ALPackage;
using Common.NpChatObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家的个人详情信息弹窗
    /// </summary>
    public class NPGGUIWndCommonPlayerBusinessCardTip:_ATNPGGUIWndCommonItemToolTip<NPGGUIMonoCommonPlayerBusinessCardTip>
    {
        // 单例
        private static NPGGUIWndCommonPlayerBusinessCardTip _m_gInstance;
        public static NPGGUIWndCommonPlayerBusinessCardTip instance
        {
            get
            {
                if(_m_gInstance == null)
                    _m_gInstance = new NPGGUIWndCommonPlayerBusinessCardTip();
                return _m_gInstance;
            }
        }
        

        private NPGGUIWndPlayerIcon _m_playerIcon;// 玩家相关信息
        private NPCommonSimplePlayerInfo _m_simplePlayerInfo;
        private NPGGuiWndTexture _m_texGender;//性别
        private RectTransform _m_rangRectTrans;//限制范围transform
        private Action _m_aOnCloseWnd;

        public NPGGUIWndCommonPlayerBusinessCardTip() : base(NPGGUIMonoCommonPlayerBusinessCardTip.assetPath,NPGGUIMonoCommonPlayerBusinessCardTip.objName)
        {
        }

        protected override string _monoAssetPath => NPGGUIMonoCommonPlayerBusinessCardTip.assetPath;
        protected override string _monoObjName => NPGGUIMonoCommonPlayerBusinessCardTip.objName;
        protected override _AALResourceCore _resourceCore=> GameResCore.instance;
        

        protected override void _onShowWnd()
        {
            base._onShowWnd();
            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _refreshWnd);
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.FRIENDS_SHIELD_CHG, _refreshWnd);
        }

        protected override void _onReset()
        {
            base._onReset();
            if (null != _m_playerIcon)
            {
                _m_playerIcon.resetWnd();
            }

            if (null != _m_texGender)
            {
                _m_texGender.discardTexture();
            }
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            if (null != _m_playerIcon)
            {
                _m_playerIcon.discard();
                _m_playerIcon = null;
            }
            
            if (null != _m_texGender)
            {
                _m_texGender.discard();
            }

            _m_simplePlayerInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            base._onWndInitDone();
            if (null != wnd.playerIcon)
            {
                _m_playerIcon = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            }

            if (null != wnd.texGender)
            {
                _m_texGender = new NPGGuiWndTexture(wnd.texGender);
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnReport,_onClickReport);
            ALUGUICommon.combineBtnClick(wnd.btnShield,_onClickShield);
            ALUGUICommon.combineBtnClick(wnd.btnUnShield,_onClickUnShield);
            ALUGUICommon.combineBtnClick(wnd.btnAddFriend,_onClickAddFriend);
            ALUGUICommon.combineBtnClick(wnd.btnDeleteFriend, _onClickDeleteFriend);
            ALUGUICommon.combineBtnClick(wnd.btnPersonalChat,_onClickPersonalChat);
            ALUGUICommon.combineBtnClick(wnd.btnDetailInfo,_onClickDetailInfo);
        }
        
        /// <summary>
        /// 显示玩家简要信息
        /// </summary>
        /// <param name="_simplePlayerInfo"></param>
        /// <param name="_targetTransRoot">跟随的目标</param>
        /// <param name="_interval">坐标偏移</param>
        /// <param name="_rangRectTrans"></param>
        /// <param name="_onClose"></param>
        public void showWnd(NPCommonSimplePlayerInfo _simplePlayerInfo,RectTransform _targetTransRoot,float _interval,RectTransform _rangRectTrans,Action _onClose = null)
        {
            _m_simplePlayerInfo = _simplePlayerInfo;
            _m_rangRectTrans = _rangRectTrans;
            _m_aOnCloseWnd = _onClose;
            base.showWnd();
            setPos(_targetTransRoot,_interval);
        }
        
        
        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            if(null == _m_simplePlayerInfo)
                return;

            if (_m_simplePlayerInfo.cid == NPPlayer.instance.playerInfo.CID)
            {
                ALUGUICommon.setGameObjEnable(wnd.selfShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.selfHideGoList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.selfShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.selfHideGoList, true);
            }
            
            if (null != _m_playerIcon)
            {
                _m_playerIcon.setPlayerInfoForChatPlayerCard(_m_simplePlayerInfo);
            }

            bool isFriend = NPPlayer.instance.friendsComp.isFriend(_m_simplePlayerInfo.cid);
            ALUGUICommon.setGameObjEnable(wnd.isFriendShowGoList, isFriend);
            ALUGUICommon.setGameObjEnable(wnd.noFriendShowGoList, !isFriend);

            string allianceName = _m_simplePlayerInfo.guildId > 0 ? TextTranslate.instance.getLanguage(TransKeyConst.guild_showName_simpleName_name, _m_simplePlayerInfo.guildSimpleName, _m_simplePlayerInfo.guildName) : TextTranslate.instance.getLanguage(TransKeyConst.chat_playerCard_allianceName_none);
            ALUGUICommon.setLabelTxt(wnd.txtAllinceName, allianceName);

            bool isShield = NPPlayer.instance.friendsComp.isShield(_m_simplePlayerInfo.cid);
            ALUGUICommon.setGameObjEnable(wnd.isShieldShowGoList, isShield);
            ALUGUICommon.setGameObjEnable(wnd.noShieldShowGoList, !isShield);

        }

        protected override void _onClose()
        {
            _m_aOnCloseWnd?.Invoke();
            _m_aOnCloseWnd = null;
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_COMMON_PLAYER_BUSINESS_CARD);
        }

        /// <summary>
        /// 举报
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickReport(GameObject _obj)
        {
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_sysUnOpen_none));
        }

        /// <summary>
        /// 屏蔽
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickShield(GameObject _obj)
        {
            FriendCommon.setShieldPlayer(_m_simplePlayerInfo, _onClose);
        }

        /// <summary>
        /// 取消屏蔽
        /// </summary>
        /// <param name="_"></param>
        private void _onClickUnShield(GameObject _)
        {
            FriendCommon.setUnShieldPlayer(_m_simplePlayerInfo);
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickAddFriend(GameObject _obj)
        {
            if (null == _m_simplePlayerInfo)
                return;

            FriendCommon.sendAddFriendRequest(_m_simplePlayerInfo.cid);
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickDeleteFriend(GameObject _obj)
        {
            if (null == _m_simplePlayerInfo)
                return;

            //提示删除
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.friends_sure_delete_str, _m_simplePlayerInfo.name))
                  , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                  , null
                  , TextTranslate.instance.getLanguage(TransKeyConst.confirm)
                  , () =>
                  {
                      NPPlayer.instance.friendsComp.reqRemoveFriend(_m_simplePlayerInfo.cid, () =>
                      {
                          NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_delete_suc_none);
                          _refreshWnd();
                      });
                  });
        }
        

        /// <summary>
        /// 私聊
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickPersonalChat(GameObject _obj)
        {
            if (null == _m_simplePlayerInfo)
                return;

            GCommon.jumpToPrivateChat(_m_simplePlayerInfo.cid);

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_COMMON_PLAYER_BUSINESS_CARD);
        }

        /// <summary>
        /// 详细个人空间
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickDetailInfo(GameObject _obj)
        {
            if (null == _m_simplePlayerInfo)
                return;

            FriendCommon.showPlayerInfo(_m_simplePlayerInfo);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_COMMON_PLAYER_BUSINESS_CARD);

        }

        /// <summary>
        /// 聊天玩家信息卡坐标定位
        /// </summary>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_interval"></param>
        public override void setPos(RectTransform _targetTransRoot, float _interval)
        {
            //如果没有传限制范围，就默认限制在全屏范围内
            if (_m_rangRectTrans == null)
                _m_rangRectTrans = Game.instance.mainCamera.uiRootRectTrans;

            //不需要跟随显示的wnd
            if (null == _targetTransRoot)
            {
                ALUGUICommon.setUIPos(rectTransform, Vector2.zero);
                return;
            }
            
            //判断是不是在目标上面显示，默认在目标上面显示
            if (!wnd.isHorizontalFollow) // 横向跟随
            {
                base.setPos(_targetTransRoot,_interval);
                return;
            }
            if(null == _targetTransRoot || null == _targetTransRoot.transform)
                return;
            
            //目标点的世界坐标
            Vector3 centerWorldPos = _targetTransRoot.transform.TransformPoint(_targetTransRoot.rect.center);
            //目标点的屏幕坐标
            Vector2 centerScreenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, centerWorldPos);

            //目标点的UGUI坐标
            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _m_rangRectTrans,
                centerScreenPos,
                Game.instance.mainCamera.uiCamera, 
                out uiPos);

            //限制范围的世界坐标
            Vector3 centerRangWorldPos = _m_rangRectTrans.transform.TransformPoint(_m_rangRectTrans.rect.center);
            //限制范围的屏幕坐标
            Vector2 centerRangScreenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, centerRangWorldPos);

            //限制范围的UGUI坐标
            Vector2 uiPosRang;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                Game.instance.mainCamera.uiRootRectTrans,
                centerRangScreenPos,
                Game.instance.mainCamera.uiCamera, 
                out uiPosRang);
            
            Vector2 tempPos = uiPos;

            //获取窗口宽高
            float selfWidth = getSelfWidth();
            float selfHeight = getSelfHeight();
            bool isleft = false;

            //窗口的中心点坐标X
            Vector2 wndCenterPos = GCommon.getUIRootPos(rectTransform);
            //目标左边界
            float targetLeftEdge = GCommon.getUIRootPos(_targetTransRoot).x - (_targetTransRoot.pivot.x * _targetTransRoot.rect.width) * _targetTransRoot.localScale.x - selfWidth - _interval;
            //目标右边界
            float targetRightEdge = GCommon.getUIRootPos(_targetTransRoot).x + ((1 - _targetTransRoot.pivot.x) * _targetTransRoot.rect.width) * _targetTransRoot.localScale.x + selfWidth + _interval;
            //限制范围左边界
            float rangeLeftEdge = GCommon.getUIRootPos(_m_rangRectTrans).x - (_m_rangRectTrans.pivot.x * _m_rangRectTrans.rect.width);
            //限制范围右边界
            float rangeRightEdge = GCommon.getUIRootPos(_m_rangRectTrans).x + ((1 - _m_rangRectTrans.pivot.x) * _m_rangRectTrans.rect.width);

            //判断边界
            if (targetRightEdge <= rangeRightEdge)
            {
                //右边界没有超出，展示在右边
                //窗口X坐标 = 目标右边界 - 窗口中心点偏移 - 窗口中心点屏幕坐标X
                tempPos.x = targetRightEdge - (1 - rectTransform.pivot.x) * selfWidth - wndCenterPos.x;
            }
            else if (targetLeftEdge >= rangeLeftEdge)
            {
                //左边界没有超出，展示在左边
                //窗口X坐标 = 目标左边界 + 窗口中心点偏移 - 窗口中心点屏幕坐标X
                tempPos.x = targetLeftEdge + (1 - rectTransform.pivot.x) * selfWidth - wndCenterPos.x;
                isleft = true;
            }
            else
            {
                //都超出了，就放在目标右边，且不超出限制范围
                //窗口X坐标 = 限制范围右边界 - 窗口中心点偏移 - 窗口中心点屏幕坐标X
                tempPos.x = rangeRightEdge - (1 - rectTransform.pivot.x) * selfWidth - wndCenterPos.x;
            }

            //看计算逻辑没啥问题，但是具体显示就是有偏差！！！！！！！！！！！！！！！
            if (-_m_rangRectTrans.rect.height / 2f > tempPos.y + _targetTransRoot.rect.height / 2 - selfHeight)
            {
                tempPos.y = tempPos.y  + uiPosRang.y - _targetTransRoot.rect.height / 2 + selfHeight * rectTransform.pivot.y; // 下对齐
            }
            else
            {
                tempPos.y = tempPos.y  + uiPosRang.y + _targetTransRoot.rect.height / 2 - selfHeight * rectTransform.pivot.y; // 常规上对齐
            }

            //设置窗口位置
            ALUGUICommon.setUIPos(rectTransform, tempPos);
            //设置跟随go的指针的位置
            _setFollowPointPos(centerWorldPos,false, isleft);
            
        }

        /// <summary>
        /// 窗口宽，
        /// </summary>
        /// <returns></returns>
        protected override float getSelfWidth()
        {
            if (null == wnd || null == _m_simplePlayerInfo)
                return 0;
            
            if (_m_simplePlayerInfo.cid == NPPlayer.instance.playerInfo.CID)
            {
                return wnd.selfWndWidth;
            }
            else
            {
                return wnd.otherWndWidth;
            }
        }

        /// <summary>
        /// 窗口高
        /// </summary>
        /// <returns></returns>
        protected override float getSelfHeight()
        {
            if (null == wnd || null == _m_simplePlayerInfo)
                return 0;
            if (_m_simplePlayerInfo.cid == NPPlayer.instance.playerInfo.CID)
            {
                return wnd.selfWndHeight;
            }
            else
            {
                return wnd.otherWndHeight;
            }
        }
    }

}