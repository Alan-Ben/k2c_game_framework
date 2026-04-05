using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndDinnerDetailLogItem : _ANPGGUIBasicGridItemWnd<GGUIMonoDinnerDetailLogItem>
    {
        private DinnerDetailLogInfo _m_itemData;
        private NPGGUIWndPlayerIcon _m_playerInfo;
        private NPGGuiWndTexture _m_texHeadIcon;//头像

        public GGUIWndDinnerDetailLogItem(GGUIMonoDinnerDetailLogItem _wnd) : base(_wnd)
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
            _m_playerInfo?.discard();
            _m_playerInfo = null;
            _m_texHeadIcon?.discard();
            _m_texHeadIcon = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if(wnd.playerIcon != null)
                _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            if(wnd.headIcon != null)
                _m_texHeadIcon = new NPGGuiWndTexture(wnd.headIcon);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(DinnerDetailLogInfo _data)
        {
            _m_itemData = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if(_m_itemData == null)
                return;
            if (_m_itemData.joinerType == EDinnerJoinerType.PLAYER)
            {
                GCommon.reqPlayerInfo(_m_itemData.joinerId, _info =>
                {
                    if (wnd == null)
                        return;

                    if (_m_playerInfo != null)
                    {
                        _m_playerInfo.showWnd();
                        _m_playerInfo.setPlayer(_m_itemData.joinerId);
                    }
                    ALUGUICommon.setLabelTxt(wnd.txtContent, _m_itemData.makeLogContent(_info.name));
                });
                ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.getChatTimeShow(_m_itemData.timeMs));
                ALUGUICommon.setGameObjEnable(wnd.heroJoinerShowGos, false);
            }
            else
            {
                HeroRefObj heroRefObj = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_itemData.joinerId);
                if (heroRefObj != null)
                {
                    _m_texHeadIcon?.setTexture(heroRefObj.icon);
                    ALUGUICommon.setLabelTxt(wnd.txtContent, _m_itemData.makeLogContent(heroRefObj.transName));
                }

                ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.getChatTimeShow(_m_itemData.timeMs));
                ALUGUICommon.setGameObjEnable(wnd.heroJoinerShowGos, true);
            }
        }
    }
}
