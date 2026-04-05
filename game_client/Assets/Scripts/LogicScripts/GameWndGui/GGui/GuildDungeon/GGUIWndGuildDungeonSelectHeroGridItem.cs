using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟副本日志
    /// </summary>
    public class GGUIWndGuildDungeonSelectHeroGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildDungeonSelectHeroGridItem>
    {
        private readonly Action<HeroInfo> _m_onItemClick;
        // <AutoGen:WndDeclaration>
        private GGUIWndHeroCommonCardItem _m_heroCardWnd;  // 大臣信息
        // </AutoGen:WndDeclaration>
        
        private HeroInfo _m_data;
        private bool _m_isSelected; // 是否选中
        
        public GGUIWndGuildDungeonSelectHeroGridItem(GGUIMonoGuildDungeonSelectHeroGridItem _wnd, Action<HeroInfo> _onItemClick) : base(_wnd)
        {
            _m_onItemClick = _onItemClick;
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }


        protected override void _onHideWnd()
        {
            _m_heroCardWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_heroCardWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            if (_m_heroCardWnd != null)
            {
                _m_heroCardWnd.ClickAction -= _onClickHeroCard;
                _m_heroCardWnd.discard();
            }
            _m_heroCardWnd = null;
            _m_data = null;
            _m_isSelected = false;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnRecover, _onBtnRecoverClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.heroCard != null)
            {
                _m_heroCardWnd = new GGUIWndHeroCommonCardItem(wnd.heroCard);
                _m_heroCardWnd.ClickAction = _onClickHeroCard;
            }
            ALUGUICommon.combineBtnClick(wnd.btnRecover, _onBtnRecoverClick);
            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onBtnSelectClick);
        }

   

        protected override void _resetGridItem()
        {
            _m_data = null;
            _m_isSelected = false;
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(HeroInfo _data, bool _selected)
        {
            _m_data = _data;
            _m_isSelected = _selected;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            if(_m_data == null)
                return;

            if(_m_heroCardWnd != null)
            {
                _m_heroCardWnd.showWnd();
                _m_heroCardWnd.setInfo(_m_data);
            }
            // UI切换：选中时显示列表代表可点击/可出战；不可用显示列表用于已出战
            ALUGUICommon.setGameObjEnable(wnd.listSelectShow, _m_isSelected);
            ALUGUICommon.setGameObjEnable(wnd.listSelectHide, !_m_isSelected);
            
            if (NPPlayer.instance.guildDungeonComp.getHeroLeftFightTimes(_m_data.id) > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.listCanRecoverShow, false);
                ALUGUICommon.setGameObjEnable(wnd.listInValidShow, false);
                ALUGUICommon.setGameObjEnable(wnd.listCanFightShow, true);
            }
            else
            {
                if(NPPlayer.instance.guildDungeonComp.canRecoverHero(_m_data.id))
                {
                    ALUGUICommon.setGameObjEnable(wnd.listInValidShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.listCanFightShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.listCanRecoverShow, true);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.listCanFightShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.listCanRecoverShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.listInValidShow, true);
                }
            }

        }
        
        private void _onBtnSelectClick(GameObject _obj)
        {
            _m_onItemClick?.Invoke(_m_data);
        }
        private void _onBtnRecoverClick(GameObject _obj)
        {
            _m_onItemClick?.Invoke(_m_data);
        }
        // <AutoGen:Method>

        // </AutoGen:Method>

        private void _onClickHeroCard(GGUIWndHeroCommonCardItem _)
        {
            if (_m_data == null)
                return;
        }
    }
}
