using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndTowerChapterItem : _ANPGGUIBasicGridItemWnd<GGUIMonoTowerChapterItem>
    {
        private TowerLevelInfo _m_towerLevelInfo;
        private NPGGuiWndTexture _m_towerIcon;
        private NPGGuiWndTexture _m_towerBg;
        private NPGGUIWndPlayerIcon _m_playerInfo; //玩家信息
        private NPCommonSimplePlayerInfo _m_simplePlayerInfo;
        private Action<TowerLevelInfo> _m_onBtnChallengeClick; //挑战按钮点击回调
        private int _m_refreshSerialize = -1;
        private int _m_lShowSerialize = -1;

        public GGUIWndTowerChapterItem(GGUIMonoTowerChapterItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            _m_towerIcon?.hideWnd();
            _m_towerBg?.hideWnd();
            _m_refreshSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_towerIcon?.discardTexture();
            _m_towerBg?.discardTexture();
            _m_refreshSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onDiscard()
        {
            _m_towerIcon?.discard();
            _m_towerIcon = null;
            _m_towerBg?.discard();
            _m_towerBg = null;

            _m_simplePlayerInfo = null;
            if (_m_playerInfo != null)
            {
                _m_playerInfo.discard();
            }
            _m_playerInfo = null;
            _m_refreshSerialize = ALSerializeOpMgr.next();
            ALUGUICommon.uncombineBtnClick(wnd.btnChallenge, _onBtnChallengeClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnInfoDetail, _onPlayerIconClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (wnd.texIcon != null)
                _m_towerIcon = new NPGGuiWndTexture(wnd.texIcon);
            if (wnd.texBg != null)
                _m_towerBg = new NPGGuiWndTexture(wnd.texBg);
            if (wnd.playerInfo != null)
            {
                _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            }
            ALUGUICommon.combineBtnClick(wnd.btnChallenge, _onBtnChallengeClick);
            ALUGUICommon.combineBtnClick(wnd.btnInfoDetail, _onPlayerIconClick);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(TowerLevelInfo _data, Action<TowerLevelInfo> _onBtnChallengeClick = null)
        {
            _m_towerLevelInfo = _data;
            _m_onBtnChallengeClick = _onBtnChallengeClick;
            _m_refreshSerialize = ALSerializeOpMgr.next();

            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || _m_towerLevelInfo == null)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtLevelName, _m_towerLevelInfo.levelName);
            ALUGUICommon.setLabelTxt(wnd.txtDailyCoins, TextTranslate.instance.getLanguage(TransKeyConst.tower_daily_coin_get_num,_m_towerLevelInfo.towerCoinCount));
            ALUGUICommon.setLabelTxt(wnd.txtTotalLevel, TextTranslate.instance.getLanguage(TransKeyConst.tower_common_total_level_desc, _m_towerLevelInfo.totalLevel));
            ALUGUICommon.setGameObjEnable(wnd.curLevelShowGos, _m_towerLevelInfo.isMyCurTowerLevel);
            ALUGUICommon.setUIObjColor(wnd.playerColorGraphic, _m_towerLevelInfo.isMyCurTowerLevel ? wnd.myPlayerColor : wnd.otherPlayerColor);
            ALUGUICommon.setUIObjColor(wnd.bgColorGraphic, _m_towerLevelInfo.isMyCurTowerLevel ? wnd.bgMyPlayerColor : _m_towerLevelInfo.level % 2 == 1 ? wnd.bgCommonColor1 : wnd.bgCommonColor2);
            ALUGUICommon.setUIObjColor(wnd.txtColorGraphic, _m_towerLevelInfo.isMyCurTowerLevel ? wnd.txtMyPlayerColor : wnd.txtCommonColor);

            // 设置驻守玩家信息
            bool hasPlayerInfo = false;
            
            if (_m_towerLevelInfo.isMyCurTowerLevel)
            {
                if (_m_playerInfo != null)
                {
                    _m_playerInfo.showWnd();
                    _m_playerInfo.setSelfInfo();
                }
                ALUGUICommon.setLabelTxt(wnd.txtPower, NPPlayer.instance.heroComponent.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                hasPlayerInfo = true;
                _m_simplePlayerInfo = null;
            }
            else
            {
                if (_m_towerLevelInfo.playerCid != 0)
                {
                    long refreshSerialize = _m_refreshSerialize;
                    GCommon.reqPlayerInfo(_m_towerLevelInfo.playerCid, _playerInfo =>
                    {
                        if(refreshSerialize != _m_refreshSerialize)
                            return; //如果刷新序列号不一致，说明已经被刷新了，直接返回
                        if (_playerInfo != null)
                        {
                            _m_simplePlayerInfo = _playerInfo;
                            if (_m_playerInfo != null)
                            {
                                _m_playerInfo.showWnd();
                                _m_playerInfo.setPlayerInfo(_playerInfo);
                            }
                            ALUGUICommon.setLabelTxt(wnd.txtPower, _playerInfo.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                        }
                        else
                        {
                            _m_simplePlayerInfo = null;
                            _m_playerInfo?.hideWnd();
                        }
                    });
                    hasPlayerInfo = true;
                }
                else
                {
                    _m_simplePlayerInfo = null;
                    _m_playerInfo?.hideWnd();
                    ALUGUICommon.setLabelTxt(wnd.txtPower, _m_towerLevelInfo.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                    ALUGUICommon.setLabelTxt(wnd.txtBossName, _m_towerLevelInfo.bossName);
                }
            }

            _m_towerBg?.showWnd();
            _m_towerBg?.setTexture(_m_towerLevelInfo.banner_tex);
            _m_towerIcon?.showWnd();
            _m_towerIcon?.setTexture(_m_towerLevelInfo.boss_icon);
            
            ALUGUICommon.setGameObjEnable(wnd.hasPlayerInfoShowGos, hasPlayerInfo);
            ALUGUICommon.setGameObjEnable(wnd.hasPlayerInfoHideGos, !hasPlayerInfo);
            
            
            TowerResearchRefObj researchRef = _m_towerLevelInfo.researchRefObj;
            if (researchRef != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtResearchEarnBonus,
                    TextTranslate.instance.getLanguage(TransKeyConst.tower_earn_bonus_add_num, researchRef.building_profit_add_per / 100f));
            }
            ALUGUICommon.setGameObjEnable(wnd.researchShowGos, researchRef != null);

            string totalLevelDesc = TextTranslate.instance.getLanguage(TransKeyConst.tower_common_total_level_desc, _m_towerLevelInfo.totalLevel);
            if (wnd.chapterItemShows != null)
                foreach (var chapterItem in wnd.chapterItemShows)
                {
                    if (chapterItem == null) continue;
                    ALUGUICommon.setLabelTxt(chapterItem.txtTotalLevel, totalLevelDesc);
                    ALUGUICommon.setGameObjEnable(chapterItem.showGos, chapterItem.chapterId == _m_towerLevelInfo.chapter);
                }
            if (wnd.txtChapterNameList != null && _m_towerLevelInfo.chapterRefObj != null)
                foreach (var chapterNameTxt in wnd.txtChapterNameList)
                {
                    ALUGUICommon.setLabelTxt(chapterNameTxt, TextTranslate.instance.getLanguage(_m_towerLevelInfo.chapterRefObj.name));
                }
        }

        private void _onBtnChallengeClick(GameObject _go)
        {
            _m_onBtnChallengeClick?.Invoke(_m_towerLevelInfo);
        }
        
        private void _onPlayerIconClick(GameObject _go)
        {
            if ( wnd != null && _m_towerLevelInfo != null)
            {
                if(_m_towerLevelInfo.isMyCurTowerLevel )
                {
                    QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text_Text(5318,
                        NPPlayer.instance.playerInfo.PlayerName,
                        NPPlayer.instance.heroComponent.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT),
                        wnd.detailTipTarget, wnd.playerTipInterval));        
                }
                else if (_m_towerLevelInfo.playerCid != 0 && _m_playerInfo != null && _m_simplePlayerInfo != null)
                {
                    QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text_Text(5318,
                        _m_simplePlayerInfo.name,
                        _m_simplePlayerInfo.totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT),
                        wnd.detailTipTarget, wnd.playerTipInterval));        
                }
                else
                {
                    QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text_Text(5318,
                        _m_towerLevelInfo.bossName,
                        _m_towerLevelInfo.power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT),
                        wnd.detailTipTarget, wnd.playerTipInterval));        
                }
                // GCommon.showPlayerInfoWndTip(_m_simplePlayerInfo, _m_playerInfo?.rectTransform, 0, Game.instance.mainCamera?.uiRootRectTrans);
            }
        }
    }
}
