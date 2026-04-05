using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndMiddayDungeonSelectHero : _ATALBasicUIWnd<GGUIMonoMiddayDungeonSelectHero>
    {
        private static GGUIWndMiddayDungeonSelectHero _g_instance = new GGUIWndMiddayDungeonSelectHero();
    
        public static GGUIWndMiddayDungeonSelectHero instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndMiddayDungeonSelectHero();
                return _g_instance;
            }
        }

        private EMiddayDungeonSelectHeroType _m_selectHeroType = EMiddayDungeonSelectHeroType.Self;

        private long _m_lSerializeOp;
        
        private GGUIWndMiddayDungeonHeroSelectItemGrid _m_wHeroSelectGrid;//大臣选择窗口
        private NPGGUIWndCommonTab _m_tabMyHero;
        private NPGGUIWndCommonTab _m_tabGuildHero;        
        private GGUIWndHeroIconNullableItem _m_wCurSelectedHero;//骑士信息

        
        private List<_IHeroCardShow> _m_guildHeroList = new List<_IHeroCardShow>();
        private List<_IHeroCardShow> _m_selfHeroList = new List<_IHeroCardShow>();
        private HeroInfo _m_curSelectSelfHero; // 当前选中的我的大臣
        private MiddayDungeonGuildHeroInfo _m_curSelectGuildHero; // 当前选中的联盟派遣大臣
        
        private Action<HeroInfo> _m_onConfirmSelctSelfHero; // 确定选择我的大臣回调
        private Action<MiddayDungeonGuildHeroInfo> _m_onConfirmSelectGuildHero; // 确定选择联盟大臣回调
        
        public GGUIWndMiddayDungeonSelectHero() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoMiddayDungeonSelectHero.assetPath; }
        protected override string _monoObjName { get => GGUIMonoMiddayDungeonSelectHero.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            
            _m_wHeroSelectGrid?.showWnd();
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_wHeroSelectGrid?.hideWnd();
            _m_lSerializeOp = ALSerializeOpMgr.next();
        }
    
        protected override void _onReset()
        {
            _m_wHeroSelectGrid?.resetWnd();
            _m_wCurSelectedHero?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_wHeroSelectGrid?.discard();
            _m_wHeroSelectGrid = null;
            _m_tabMyHero?.discard();
            _m_tabMyHero = null;
            _m_tabGuildHero?.discard();
            _m_tabGuildHero = null;
            _m_wCurSelectedHero?.discard();
            _m_wCurSelectedHero = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.monoHeroSelectWnd != null)
            {
                _m_wHeroSelectGrid = new GGUIWndMiddayDungeonHeroSelectItemGrid(wnd.monoHeroSelectWnd, _onHeroSelect);
            }
            
            if (null != wnd.tabMyHero)
            {
                _m_tabMyHero = new NPGGUIWndCommonTab(wnd.tabMyHero);
                _m_tabMyHero.clickDelegate += _clickTabSelf;
            }
            if (null != wnd.tabGuildHero)
            {
                _m_tabGuildHero = new NPGGUIWndCommonTab(wnd.tabGuildHero);
                _m_tabGuildHero.clickDelegate += _clickTabGuild;
            }
            
            if (null != wnd.curSelectedHero)
            {
                _m_wCurSelectedHero = new GGUIWndHeroIconNullableItem(wnd.curSelectedHero);
            }
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        public void setInfo(EMiddayDungeonSelectHeroType _selectHeroType, Action<HeroInfo> _onConfirmSelf, Action<MiddayDungeonGuildHeroInfo> _onConfirmGuild, HeroInfo _curSelectSelfHero = null, MiddayDungeonGuildHeroInfo _curSelectGuildHero = null)
        {
            _m_onConfirmSelctSelfHero = _onConfirmSelf;
            _m_onConfirmSelectGuildHero = _onConfirmGuild;
            _m_selectHeroType = _selectHeroType;
            _m_curSelectSelfHero = _curSelectSelfHero;
            _m_curSelectGuildHero = _curSelectGuildHero;
            _checkDefaultSelectTab();
            _refreshWnd();
        }

        /// <summary>
        /// 检查需要选中的页签
        /// </summary>
        private void _checkDefaultSelectTab()
        {
            //自己的顾问是否还有可使用次数
            bool haveHeroCount = false;
            NPPlayer.instance.heroComponent.dealAllHero(_info =>
            {
                if (null == _info)
                    return;

                if (NPPlayer.instance.middayDungeonComp.getHeroLeftFightTimes(_info.id) > 0)
                    haveHeroCount = true;
            });

            //自己的顾问没有可使用次数并且有联盟援助次数时选中联盟援助页签
            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id);
            if (!haveHeroCount && fixedCdInfo != null && fixedCdInfo.getCount() > 0)
                _m_selectHeroType = EMiddayDungeonSelectHeroType.Guild;
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            NPPlayer.instance.middayDungeonComp.refreshRoundInfo();
            switch (_m_selectHeroType)
            {
                case EMiddayDungeonSelectHeroType.Self:
                    _updateSelfHero();
                    break;
                case EMiddayDungeonSelectHeroType.Guild:
                    _updateGuildHero();
                    break;
            }
        }
        
        /// <summary>
        /// 刷新我的大臣选择窗口
        /// </summary>
        private void _updateSelfHero()
        {
            if(null == wnd)
                return;
            _m_lSerializeOp = ALSerializeOpMgr.next();
            _m_tabMyHero?.setSelected(true);
            _m_tabGuildHero?.setSelected(false);
            _m_selfHeroList.Clear();
            // 遍历所有大臣，并按照满足条件进行排序
            NPPlayer.instance.heroComponent.dealAllHero(_info =>
            {
                if(null == _info)
                    return;
                _m_selfHeroList.Add(_info);
            });
            NPPlayer.instance.middayDungeonComp.refreshRoundInfo();
            _m_selfHeroList.Sort(NPPlayer.instance.middayDungeonComp.sortHero);
            _m_wHeroSelectGrid?.refreshWnd(_m_selfHeroList, _m_curSelectSelfHero, _m_selectHeroType);
            if (_m_wCurSelectedHero != null)
            {
                _m_wCurSelectedHero.showWnd();
                _m_wCurSelectedHero.setData(_m_curSelectSelfHero);
            }
            ALUGUICommon.setGameObjEnable(wnd.goShowInGuildHero, false);
            ALUGUICommon.setGameObjEnable(wnd.goShowInMyHero, true);

            ALUGUICommon.setGameObjEnable(wnd.goNoJoinGuildHideList, false);
            ALUGUICommon.setGameObjEnable(wnd.goNoJoinGuildShowList, false);
        }

        /// <summary>
        /// 刷新联盟大臣选择窗口
        /// </summary>
        private void _updateGuildHero()
        {             
            if(null == wnd)
                return;
            _m_tabMyHero?.setSelected(false);
            _m_tabGuildHero?.setSelected(true);
            _m_guildHeroList.Clear();
            _m_lSerializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_lSerializeOp;
            NPPlayer.instance.guildComp.reqGuildDispatchHeroList((_isSuc, _list) =>
            {
                if (serializeOp != _m_lSerializeOp)
                    return;
                if (_isSuc)
                {
                    foreach (Guild_DispatchHeroDetailInfo guidHero in _list)
                    {
                        // 过滤自己
                        if(guidHero == null || guidHero.getCid() == NPPlayer.instance.playerInfo.CID)
                            continue;
                        MiddayDungeonGuildHeroInfo guildHeroInfo = new MiddayDungeonGuildHeroInfo(guidHero);
                        _m_guildHeroList.Add(guildHeroInfo);
                    }
                    NPPlayer.instance.middayDungeonComp.refreshRoundInfo();
                    _m_guildHeroList.Sort(NPPlayer.instance.middayDungeonComp.sortGuildHero);
                    _m_wHeroSelectGrid?.refreshWnd(_m_guildHeroList, _m_curSelectGuildHero, _m_selectHeroType);
                }
            });
            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id);
            
            // 刷新借用大臣剩余次数
            if (fixedCdInfo != null)
                ALUGUICommon.setLabelTxt(wnd.txtGuildAssistCount, TextTranslate.instance.getLanguage(
                    TransKeyConst.midddayDungeon_guildAssit_times_str,
                    fixedCdInfo.getCount(),
                    fixedCdInfo.getMaxCount()));
            _m_wHeroSelectGrid?.refreshWnd(_m_guildHeroList, _m_curSelectGuildHero, _m_selectHeroType);

            if (_m_wCurSelectedHero != null)
            {
                _m_wCurSelectedHero.showWnd();
                _m_wCurSelectedHero.setData(_m_curSelectGuildHero);
            }
            
            ALUGUICommon.setGameObjEnable(wnd.goShowInGuildHero, true);
            ALUGUICommon.setGameObjEnable(wnd.goShowInMyHero, false);

            bool isJoinGuild = NPPlayer.instance.guildComp.isJoinGuild();
            ALUGUICommon.setGameObjEnable(wnd.goNoJoinGuildHideList, isJoinGuild);
            ALUGUICommon.setGameObjEnable(wnd.goNoJoinGuildShowList, !isJoinGuild);
        }
        
        private void _clickTabSelf(bool obj)
        {
            _m_selectHeroType = EMiddayDungeonSelectHeroType.Self;
            _refreshWnd();
        }

        private void _clickTabGuild(bool obj)
        {
            _m_selectHeroType = EMiddayDungeonSelectHeroType.Guild;
            _refreshWnd();
        }

        /// <summary>
        /// 点击选择大臣
        /// </summary>
        /// <param name="_heroInfo"></param>
        private void _onHeroSelect(_IHeroCardShow _heroInfo)
        {
            switch (_m_selectHeroType)
            {
                case EMiddayDungeonSelectHeroType.Self:
                    _m_curSelectSelfHero = _heroInfo as HeroInfo;
                    _m_wHeroSelectGrid?.refreshWnd(_m_selfHeroList, _m_curSelectSelfHero, _m_selectHeroType);
                    _m_onConfirmSelctSelfHero?.Invoke(_m_curSelectSelfHero);
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_SELECT_HERO);
                    break;
                case EMiddayDungeonSelectHeroType.Guild:
                    NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(GRefdataCoreMgr.instance.npGeneral.midday_dungeon_can_borrow_guild_hero_num_fixed_cd_id);
                    if (fixedCdInfo != null && fixedCdInfo.getCount() <= 0)
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_no_left_borrow_guid_hero_times);
                        return;
                    }
                    _m_curSelectGuildHero = _heroInfo as MiddayDungeonGuildHeroInfo;
                    _m_wHeroSelectGrid?.refreshWnd(_m_guildHeroList, _m_curSelectGuildHero, _m_selectHeroType);
                    _m_onConfirmSelectGuildHero?.Invoke(_m_curSelectGuildHero);
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_SELECT_HERO);
                    break;
            }
            _refreshWnd();
        }

        private void _onBtnConfirmClick(GameObject _)
        {
            switch (_m_selectHeroType)
            {
                case EMiddayDungeonSelectHeroType.Self:
                    if (_m_curSelectSelfHero == null)
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_select_a_hero_tip);
                        return;
                    }
                    _m_onConfirmSelctSelfHero?.Invoke(_m_curSelectSelfHero);
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_SELECT_HERO);
                    break;
                case EMiddayDungeonSelectHeroType.Guild:
                    if (_m_curSelectGuildHero == null)
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_select_a_hero_tip);
                        return;
                    }
                    _m_onConfirmSelectGuildHero?.Invoke(_m_curSelectGuildHero);
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_SELECT_HERO);
                    break;
            }        
          
        }

        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MIDDAY_DUNGEON_SELECT_HERO);
        }
    }
}