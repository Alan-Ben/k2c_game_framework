using ALPackage;
using Common.GuildDungeonEnum;
using Common.GuildEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟PVE开启副本
    /// </summary>
    public class GGUIWndGuildDungeonStart : _ATALBasicUIWnd<GGUIMonoGuildDungeonStart>
    {
        private static GGUIWndGuildDungeonStart _g_instance = new GGUIWndGuildDungeonStart();
    
        public static GGUIWndGuildDungeonStart instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonStart();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        private NPGGUIWndCommonItem _m_itemGuildWnd;  // 联盟财富
        private NPGGUIWndCommonItem _m_itemGemWnd;  // 钻石
        // </AutoGen:WndDeclaration>
        
        private GuildDungeonInfo _m_dungeonInfo; // 当前副本信息
        
        public GGUIWndGuildDungeonStart() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonStart.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonStart.objName; }
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
            ALUGUICommon.uncombineBtnClick(wnd.btnGem, _onClickbtnGem);
            ALUGUICommon.uncombineBtnClick(wnd.btnGuildWealth, _onClickbtnGuildWealth);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            
            _m_itemGuildWnd?.discard();
            _m_itemGuildWnd = null;
            
            _m_itemGemWnd?.discard();
            _m_itemGemWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if(wnd.itemGem != null)
            {
                _m_itemGemWnd = new NPGGUIWndCommonItem(wnd.itemGem);
            }
            if(wnd.itemGuild != null)
            {
                _m_itemGuildWnd = new NPGGUIWndCommonItem(wnd.itemGuild);
            }
                
            ALUGUICommon.combineBtnClick(wnd.btnGem, _onClickbtnGem);
            ALUGUICommon.combineBtnClick(wnd.btnGuildWealth, _onClickbtnGuildWealth);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }

        public void setInfo(GuildDungeonInfo _dungeonId)
        {
            _m_dungeonInfo = _dungeonId;
            _refreshWnd();
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            bool canUseGuildWealth = NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.USE_GUILD_WEALTH, false);
            ALUGUICommon.setGameObjEnable(wnd.guildWealthShowGos, !canUseGuildWealth);
            ALUGUICommon.setGameObjEnable(wnd.guildWealthHideGos, canUseGuildWealth);
            if(!canUseGuildWealth)
                GGameCommonInfo.grayImage(wnd.guildWealthGrayGraphics);
            else
                GGameCommonInfo.disgrayImage(wnd.guildWealthGrayGraphics);
            
            bool cantUseGem = NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.USE_ITEM_START_PVE, false);
            ALUGUICommon.setGameObjEnable(wnd.cantUseGemShowGos, !cantUseGem);
            ALUGUICommon.setGameObjEnable(wnd.cantUseGemHideGos, cantUseGem);
            if(!cantUseGem)
                GGameCommonInfo.grayImage(wnd.cantUseGemGrayGraphics);
            else
                GGameCommonInfo.disgrayImage(wnd.cantUseGemGrayGraphics);
                
            if (_m_dungeonInfo != null && _m_dungeonInfo.lvlRefObj != null)
            {
                
                NPCommonCostItem guildWealthCostItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.guild_wealth_common_item, _m_dungeonInfo.lvlRefObj.star_cost_guild_wealth);
                _m_itemGemWnd?.setItem(_m_dungeonInfo.lvlRefObj.star_cost_item);
                _m_itemGuildWnd?.setItem(guildWealthCostItem);
            }
        }
        
        // <AutoGen:Method>
        // 使用联盟财富开启点击事件
        private void _onClickbtnGuildWealth(GameObject go)
        {
            if(!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.USE_GUILD_WEALTH , true))
                return;
                
            if (_m_dungeonInfo == null || _m_dungeonInfo.lvlRefObj == null) return;

            // 检查联盟财富资源是否足够
            NPCommonCostItem guildWealthCostItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.guild_wealth_common_item, _m_dungeonInfo.lvlRefObj.star_cost_guild_wealth);
            if (!GCommon.isItemEnough(guildWealthCostItem, true))
            {
                return;
            }

            NPMesMgr.instance.showCostItemMes(guildWealthCostItem, () =>
            {
                if (_m_dungeonInfo == null)
                {
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_START);
                    return;
                }
                NPPlayer.instance.guildDungeonComp.reqStartDungeon(_m_dungeonInfo.dungeonId, EGuildDungeon_StartType.ALLIANCE_WEALTH,
                    _suc =>
                    {
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_START);
                    });
            }, null, TransKeyConst.guild_dungeon_startSecondCheckWndTitle_none, 
                TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_startSecondCheckWndContent_none, 
                    guildWealthCostItem.count, guildWealthCostItem.getItemName(), TextTranslate.instance.getLanguage(_m_dungeonInfo.dungeonRefObj?.name)));
         
        }

        // 钻石开启点击事件
        private void _onClickbtnGem(GameObject go)
        {
            if(!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.USE_ITEM_START_PVE, true))
                return;
            if (_m_dungeonInfo == null || _m_dungeonInfo.lvlRefObj == null) return;

            NPCommonCostItem costItem = _m_dungeonInfo.lvlRefObj.star_cost_item;
            // 检查钻石资源是否足够
            if (costItem == null || !GCommon.isItemEnough(costItem, true))
            {
                return;
            }
            
            NPMesMgr.instance.showCostItemMes(costItem, () =>
                {
                    if (_m_dungeonInfo == null)
                    {
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_START);
                        return;
                    }
                    NPPlayer.instance.guildDungeonComp.reqStartDungeon(_m_dungeonInfo.dungeonId,
                        EGuildDungeon_StartType.COMMON_ITEM,
                        _suc => { QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_START); });
                }, null, TransKeyConst.guild_dungeon_startSecondCheckWndTitle_none,
                TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_startSecondCheckWndContent_none, 
                    costItem.count, costItem.getItemName(), TextTranslate.instance.getLanguage(_m_dungeonInfo.dungeonRefObj?.name)));
        }

        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_START);
        }
        // </AutoGen:Method>
    }
}