using System;
using ALPackage;
using Common.DinnerEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndDinnerMainJoinerItem : _ATALBasicUISubWnd<GGUIMonoDinnerMainJoinerItem>
    {
        private GDinnerJoinerInfo _m_joinerInfo;
        private bool _m_showAnim;
        private NPGGUIWndPlayerIcon _m_playerInfo; //玩家信息
        private GGUIWndHeroIconItem _m_heroInfo; //大臣信息
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndDinnerMainJoinerItem(GGUIMonoDinnerMainJoinerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_joinerInfo = null;
            _m_heroInfo?.discard();
            _m_heroInfo = null;
            _m_playerInfo?.discard();
            _m_playerInfo = null;
            
            wnd.talkAnim.Stop(wnd.talkShowAnimName);
            wnd.talkAnim.Sample(wnd.talkShowAnimName, 0.0f);
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if(wnd.playerIcon != null)
                _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            if(wnd.heroIcon != null)
                _m_heroInfo = new GGUIWndHeroIconItem(wnd.heroIcon);
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);

        }

        public void setInfo(GDinnerJoinerInfo _data, bool _showAni = false)
        {
            _m_joinerInfo = _data;
            _m_showAnim = _showAni;
            if(wnd == null)
                return;
            if (_m_joinerInfo == null)
            {
                //如果没有数据，则显示等待加入
                ALUGUICommon.setGameObjEnable(wnd.waitJoinShowGos, true);
                ALUGUICommon.setGameObjEnable(wnd.waitJoinHideGos, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.waitJoinShowGos, false);
                ALUGUICommon.setGameObjEnable(wnd.waitJoinHideGos, true);
            }

            _m_joinerInfo?.regDetailInfo(_info =>
            {
                _refreshWnd();
            });
            wnd.talkAnim.Stop(wnd.talkShowAnimName);
            wnd.talkAnim.Sample(wnd.talkShowAnimName, 0.0f);
        }

        public void setTalkShow(string _talkText)
        {
            ALUGUICommon.setLabelTxt(wnd.txtTalk, _talkText);
            if (wnd != null && wnd.talkAnim != null) 
                wnd.talkAnim.Play(wnd.talkShowAnimName);
        }

        private void _refreshWnd()
        {
            if (null == wnd )
                return;
            if (_m_joinerInfo == null)
            {
                //如果没有数据，则显示等待加入
                ALUGUICommon.setGameObjEnable(wnd.waitJoinShowGos, true);
                ALUGUICommon.setGameObjEnable(wnd.waitJoinHideGos, false);
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.textName, _m_joinerInfo.name);
                string popularityStr = TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_score, _m_joinerInfo.score);
                ALUGUICommon.setLabelTxt(wnd.txtPopularity, popularityStr);
                ALUGUICommon.setLabelTxt(wnd.txtPopularity2, popularityStr);

                bool isFriend = _m_joinerInfo.joinerType == EDinnerJoinerType.PLAYER && NPPlayer.instance.friendsComp.isFriend(_m_joinerInfo.joinerId);
                ALUGUICommon.setGameObjEnable(wnd.friendShowGos, isFriend);
                bool isSelf = _m_joinerInfo.joinerType == EDinnerJoinerType.PLAYER && NPPlayer.instance.playerInfo.CID == _m_joinerInfo.joinerId;
                ALUGUICommon.setGameObjEnable(wnd.selfJoinShowGos, isSelf);
                
                if (_m_joinerInfo.joinerType == EDinnerJoinerType.PLAYER)
                {
                    if (_m_playerInfo != null)
                    {
                        _m_playerInfo.showWnd();
                        _m_playerInfo.setPlayerInfo(_m_joinerInfo.playerInfo);
                    }
                    if (_m_heroInfo != null)
                    {
                        _m_heroInfo.hideWnd();
                    }
                }
                else
                {
                    if (_m_playerInfo != null)
                    {
                        _m_playerInfo.hideWnd();
                    }
                    if (_m_heroInfo != null)
                    {
                        HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_joinerInfo.joinerId);
                        _m_heroInfo.showWnd();
                        _m_heroInfo.setData(new HeroCardShowInfo(null, heroRefObj));
                    }
                }

                if (_m_showAnim && _m_joinerInfo.joinerType == EDinnerJoinerType.PLAYER && wnd.showAnim != null)
                    wnd.showAnim.Play(isSelf ? wnd.playerSelfShowAnimName : wnd.showAnimName);
            }
            
        }
        private void _onClick(GameObject obj)
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            GTDDinnerMainSceneMgr.instance.dealClickSeatItem(_m_joinerInfo);

            if (_m_joinerInfo == null)
                return;
            if (_m_joinerInfo.joinerType == EDinnerJoinerType.HERO)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.dinner_joiner_is_my_hero_tip);
                return;
            }

            long serialize = _m_lShowSerialize;
            GCommon.reqPlayerInfo(_m_joinerInfo.joinerId, (_info) =>
            {
                if (serialize != _m_lShowSerialize || wnd == null)
                    return;

                GCommon.showPlayerInfoWndTip(_info, wnd.playerDetailParent, 0, Game.instance.mainCamera?.uiRootRectTrans);
            });
        }
    }
}
