using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟副本日志
    /// </summary>
    public class GGUIWndGuildDungeonSelectHero : _ATALBasicUIWnd<GGUIMonoGuildDungeonSelectHero>
    {
        private static GGUIWndGuildDungeonSelectHero _g_instance = new GGUIWndGuildDungeonSelectHero();
    
        public static GGUIWndGuildDungeonSelectHero instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonSelectHero();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildDungeonSelectHeroGrid _m_itemGridWnd;
        
        private GGUIWndHeroIconNullableItem _m_wCurSelectedHero;//骑士信息
        private NPGGUIWndCommonItem _m_wCostItem;//item
        
        private List<HeroInfo> _m_selfHeroList = new List<HeroInfo>();
        private HeroInfo _m_curSelectSelfHero; // 当前选中的大臣
        private Action<HeroInfo> _m_onConfirmSelctSelfHero; // 确定选择我的大臣回调

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildDungeonSelectHero() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonSelectHero.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonSelectHero.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            
            _m_wCurSelectedHero?.discard();
            _m_wCurSelectedHero = null;
            
            _m_selfHeroList.Clear();
            _m_curSelectSelfHero = null;
            _m_onConfirmSelctSelfHero = null;
            
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRecover, _onBtnRecoverClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnInValid, _onBtnInvalidClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGuildDungeonSelectHeroGrid(wnd.itemGrid, _onHeroSelect);
            
            if (null != wnd.curSelectedHero)
                _m_wCurSelectedHero = new GGUIWndHeroIconNullableItem(wnd.curSelectedHero);
            
            if (wnd.recoverCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.recoverCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnCancel, _onBtnCancelClick);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.combineBtnClick(wnd.btnRecover, _onBtnRecoverClick);
            ALUGUICommon.combineBtnClick(wnd.btnInValid, _onBtnInvalidClick);
        }

        public void setInfo(HeroInfo _selectHero, Action<HeroInfo> _onConfirmSelf)
        {
            _m_curSelectSelfHero = _selectHero;
            _m_onConfirmSelctSelfHero = _onConfirmSelf;

            _m_selfHeroList.Clear();
            // 遍历所有大臣，并按照满足条件进行排序
            NPPlayer.instance.heroComponent.dealAllHero(_info =>
            {
                if(null == _info)
                    return;
                _m_selfHeroList.Add(_info);
            });
            _m_selfHeroList.Sort(NPPlayer.instance.guildDungeonComp.sortHero);
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _m_itemGridWnd?.showItemList(_m_selfHeroList, _m_curSelectSelfHero);
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(GRefdataCoreMgr.instance.npGeneral.guild_dungeon_recover_hero_cost);
            }

            long totalPower = NPPlayer.instance.guildDungeonComp.getLeftHeroTotalPower();
            ALUGUICommon.setLabelTxt(wnd.txtLeftTotalPower, totalPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            _refreshCurSelectHeroState();
        }

        private void _refreshCurSelectHeroState()
        {
            if (_m_wCurSelectedHero != null)
            {
                _m_wCurSelectedHero.showWnd();
                _m_wCurSelectedHero.setData(_m_curSelectSelfHero);
            }
            
            if (_m_curSelectSelfHero != null && NPPlayer.instance.guildDungeonComp.getHeroLeftFightTimes(_m_curSelectSelfHero.id) > 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.listCanRecoverShow, false);
                ALUGUICommon.setGameObjEnable(wnd.listInValidShow, false);
                ALUGUICommon.setGameObjEnable(wnd.listCanFightShow, true);
            }
            else  if(_m_curSelectSelfHero != null && NPPlayer.instance.guildDungeonComp.canRecoverHero(_m_curSelectSelfHero.id))
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

        /// <summary>
        /// 点击选择大臣
        /// </summary>
        /// <param name="_heroInfo"></param>
        private void _onHeroSelect(_IHeroCardShow _heroInfo)
        {
            _m_curSelectSelfHero = _heroInfo as HeroInfo;
            _m_itemGridWnd?.showItemList(_m_selfHeroList, _m_curSelectSelfHero);
            _refreshWnd();
        }
        
        private void _onBtnConfirmClick(GameObject _)
        {
            if (_m_curSelectSelfHero == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_select_a_hero_tip);
                return;
            }

            // 如果是可恢复状态，弹恢复弹窗
            if (NPPlayer.instance.guildDungeonComp.getHeroLeftFightTimes(_m_curSelectSelfHero.id) <= 0)
            {
                if(NPPlayer.instance.guildDungeonComp.canRecoverHero(_m_curSelectSelfHero.id))
                    _recoverHero();
                else
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_select_hero_no_fight_times_tip);
                return;
            }
            _m_onConfirmSelctSelfHero?.Invoke(_m_curSelectSelfHero);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_SELECT_HERO);
        }
        private void _onBtnRecoverClick(GameObject _)
        {
            if (_m_curSelectSelfHero == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_select_a_hero_tip);
                return;
            }

            // 如果是可恢复状态，弹恢复弹窗
            if (NPPlayer.instance.guildDungeonComp.getHeroLeftFightTimes(_m_curSelectSelfHero.id) <= 0)
            {
                if(NPPlayer.instance.guildDungeonComp.canRecoverHero(_m_curSelectSelfHero.id))
                    _recoverHero();
            }
        }
        private void _onBtnInvalidClick(GameObject _)
        {
            if (_m_curSelectSelfHero == null)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_select_a_hero_tip);
                return;
            }

            // 如果是可恢复状态，弹恢复弹窗
            if (NPPlayer.instance.guildDungeonComp.getHeroLeftFightTimes(_m_curSelectSelfHero.id) <= 0 && !NPPlayer.instance.guildDungeonComp.canRecoverHero(_m_curSelectSelfHero.id))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_select_hero_no_fight_times_tip);
                return;
            }
        }
        private void _recoverHero()
        {
            NPCommonCostItem costItem = GRefdataCoreMgr.instance.npGeneral.guild_dungeon_recover_hero_cost;
            if (_m_curSelectSelfHero == null || costItem == null || !GCommon.isItemEnough(costItem, true))
                return;
                
            NPMesMgr.instance.showCostItemMes(costItem, () =>
                {
                    NPPlayer.instance.guildDungeonComp.reqRecoverHeroFight(_m_curSelectSelfHero.id, _suc => { _refreshWnd(); });
                }, null, TransKeyConst.guild_dungeon_recoverHeroFightWndTitle_none, 
                TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_recoverHeroFightWndContent_none, 
                    costItem.count, costItem.getItemName(), _m_curSelectSelfHero.heroRefObj.transName));
        }
        
        private void _onBtnCancelClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_SELECT_HERO);
        }

        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}