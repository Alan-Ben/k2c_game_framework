using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Common.DinnerEnum;
using NPEnum;
using GOE.FollowItem;

namespace GOE
{
    /// <summary>
    /// 宴会席位的跟随窗口，
    /// </summary>
    public class GGUIWndDinnerSeatFollowItem : _ATALGGUIWndCommonFollowItem<GGUIMonoDinnerSeatFollowItem>
    {
        private NPGGuiWndTexture _m_cardIcon;
        private GDinnerJoinerInfo _m_joinerInfo;
        //显示序列号
        private long _m_lShowSerialize;
        
        public GGUIWndDinnerSeatFollowItem(GGUIMonoDinnerSeatFollowItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_cardIcon?.discardTexture();
        }

        protected override void _onReset()
        {
            _m_cardIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_cardIcon?.discard();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
            {
                return;
            }
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
            if(wnd.imgCardIcon != null)
                _m_cardIcon = new NPGGuiWndTexture(wnd.imgCardIcon);
        }

        /// <summary>
        /// 刷新item显示
        /// </summary>
        /// <param name="_joinerInfo"></param>
        public void setInfo(GDinnerJoinerInfo _joinerInfo)
        {
            _m_joinerInfo = _joinerInfo;
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_joinerInfo)
            {
                return;
            }
            ALUGUICommon.setLabelTxt(wnd.textName, _m_joinerInfo.name);
            bool isFriend = _m_joinerInfo.joinerType == EDinnerJoinerType.PLAYER && NPPlayer.instance.friendsComp.isFriend(_m_joinerInfo.joinerId);
            ALUGUICommon.setGameObjEnable(wnd.friendShowGos, isFriend);

            if (_m_cardIcon != null)
            {
                if (_m_joinerInfo.joinerType == EDinnerJoinerType.PLAYER)
                {
                    //根据player的数据做显示
                    GCommon.reqPlayerInfo(_m_joinerInfo.joinerId, (playerInfo) =>
                    {
                        _m_cardIcon.setTexture(playerInfo?.skinRef?.card_image);
                    });
                }
                else if(_m_joinerInfo.joinerType == EDinnerJoinerType.HERO)
                {
                    var heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_joinerInfo.joinerId);
                    if (heroRefObj != null) _m_cardIcon.setTexture(heroRefObj.card_image);
                }
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