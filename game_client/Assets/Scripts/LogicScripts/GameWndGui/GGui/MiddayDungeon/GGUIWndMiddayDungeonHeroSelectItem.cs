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
    public class GGUIWndMiddayDungeonHeroSelectItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMiddayDungeonHeroSelectItem>
    {
        private readonly Action<_IHeroCardShow> _m_onItemClick;

        private GGUIWndHeroIconItem _m_heroCard;
        
        private EMiddayDungeonSelectHeroType _m_selectHeroType;
        private _IHeroCardShow _m_heroInfo;
        private int _m_selectIndex;
        private bool _m_bIsValid;

        public GGUIWndMiddayDungeonHeroSelectItem(GGUIMonoMiddayDungeonHeroSelectItem _wnd, Action<_IHeroCardShow> _onItemClick)  : base(_wnd)
        {
            _m_onItemClick = _onItemClick;

            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_heroCard?.showWnd();
            
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_heroCard?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_heroCard?.resetWnd();
        }
        protected override void _onDiscard()
        {
            if (_m_heroCard != null)
            {
                _m_heroCard.discard();
                _m_heroCard = null;
            }
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onHeroClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHeroCard != null)
            {
                _m_heroCard = new GGUIWndHeroIconItem(wnd.monoHeroCard);
            }
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onHeroClick);
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void refreshWnd(_IHeroCardShow _heroInfo, int _selectIndex, EMiddayDungeonSelectHeroType _selectHeroType)
        {
            _m_selectHeroType = _selectHeroType;
            _m_heroInfo = _heroInfo;
            _m_selectIndex = _selectIndex;
            _m_bIsValid = false;
            switch (_m_selectHeroType)
            {
                case EMiddayDungeonSelectHeroType.Self:
                    if (_m_heroInfo != null)
                        _m_bIsValid = NPPlayer.instance.middayDungeonComp.getHeroLeftFightTimes(_m_heroInfo.id) > 0;
                    break;
                case EMiddayDungeonSelectHeroType.Guild:
                    if (_m_heroInfo is MiddayDungeonGuildHeroInfo guildHeroInfo)
                        _m_bIsValid = !NPPlayer.instance.middayDungeonComp.getGuildIsUsed(guildHeroInfo.cid);
                    break;
            }
            
            refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void refreshWnd()
        {
            if (null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtSelectNum, _m_selectIndex > 0 ? _m_selectIndex.ToString() : "");
            ALUGUICommon.setGameObjEnable(wnd.listSelectShow, _m_selectIndex > 0);
            ALUGUICommon.setGameObjEnable(wnd.listSelectHide, _m_selectIndex <= 0);
            _m_heroCard?.setData(_m_heroInfo);
            NPCommonEnumStatInfo<EMiddayDungeonSelectHeroType>.setStat(wnd.statInfos, _m_selectHeroType);
           
            switch (_m_selectHeroType)
            {
                case EMiddayDungeonSelectHeroType.Self:
                    if (_m_heroInfo != null)
                    {
                        int leftFightTimes = NPPlayer.instance.middayDungeonComp.getHeroLeftAndMaxFightTimes(_m_heroInfo.id, out int maxFightTimes);
                        ALUGUICommon.setLabelTxt(wnd.txtLeftFightTimes, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, leftFightTimes, maxFightTimes));
                        ALUGUICommon.setUIObjColor(wnd.txtLeftFightTimes, leftFightTimes > 0 ? wnd.hasFightTimesCol : wnd.noFightTimesCol);
                        ALUGUICommon.setGameObjEnable(wnd.listInValidShow, !_m_bIsValid);
                        ALUGUICommon.setGameObjEnable(wnd.listInValidHide, _m_bIsValid);
                    }
                    break;
                case EMiddayDungeonSelectHeroType.Guild:
                    if (_m_heroInfo is MiddayDungeonGuildHeroInfo guildHeroInfo)
                    {
                        guildHeroInfo.regPlayerInfo(_info =>
                        {
                            if (_info != null && wnd != null) 
                                ALUGUICommon.setLabelTxt(wnd.txtGuildPlayerName, _info.playerName);
                        });
                        int leftFightTimes = _m_bIsValid ? 1 : 0;
                        ALUGUICommon.setLabelTxt(wnd.txtLeftFightTimes, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, leftFightTimes, 1));
                        ALUGUICommon.setUIObjColor(wnd.txtLeftFightTimes, leftFightTimes > 0 ? wnd.hasFightTimesCol : wnd.noFightTimesCol);
                        ALUGUICommon.setGameObjEnable(wnd.listInValidShow, !_m_bIsValid);
                        ALUGUICommon.setGameObjEnable(wnd.listInValidHide, _m_bIsValid);
                    }

                    break;
            }
        }
        
        private void _onHeroClick(GameObject _)
        {
            if (wnd == null )
                return;
            if(!_m_bIsValid)
                return;
            _m_onItemClick?.Invoke(_m_heroInfo);
        }
    }
}
