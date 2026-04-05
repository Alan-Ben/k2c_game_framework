using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟副本升级
    /// </summary>
    public class GGUIWndGuildDungeonUpgradeGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildDungeonUpgradeGridItem>
    {
        
        // <AutoGen:WndDeclaration>
        private NPGGuiWndTexture _m_icoBossWnd; // boss图标
        // </AutoGen:WndDeclaration>
        private GuildDungeonInfo _m_data; // 数据
        
        public GGUIWndGuildDungeonUpgradeGridItem(GGUIMonoGuildDungeonUpgradeGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_icoBossWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_icoBossWnd?.hideWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_icoBossWnd?.discard();
            _m_icoBossWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickbtnUpgrade);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.icoBoss != null)
                _m_icoBossWnd = new NPGGuiWndTexture(wnd.icoBoss);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickbtnUpgrade);
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(GuildDungeonInfo _data)
        {
            _m_data = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (_m_data == null)
                return;
            if(_m_icoBossWnd != null)
            {
                _m_icoBossWnd.setTexture(_m_data.dungeonRefObj?.icon);
                _m_icoBossWnd.showWnd();
            }
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_data.dungeonRefObj?.getFullTransName(_m_data.lvl));
            ALUGUICommon.setLabelTxt(wnd.txtOpenCost, _m_data.starCostGuildWealth);
            if (_m_data.dungeonRefObj != null)
            {
                GuildDungeonLvlRefObj lvlRefObj = _m_data.dungeonRefObj.getLvlRef(_m_data.lvl);
                long totalHp = 0;
                long bossHp = 0;
                if (lvlRefObj != null)
                {
                    bossHp = lvlRefObj.getMonsterHp(EGuildDungeon_MonsterType.BOSS);

                    if (_m_data.dungeonRefObj.monoster_list != null)
                        foreach (var monster in _m_data.dungeonRefObj.monoster_list)
                        {
                            if (monster != null) totalHp += lvlRefObj.getMonsterHp(monster.monster_type);
                        }

                    ALUGUICommon.setLabelTxt(wnd.txtBossBlood, bossHp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                }

                ALUGUICommon.setLabelTxt(wnd.txtTotalBlood, totalHp.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            }
            ALUGUICommon.setLabelTxt(wnd.txtLockCondition, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_unlockCondition_num, _m_data.dungeonRefObj?.unlock_need_guild_lvl));
            bool isLock = _m_data != null && _m_data.state == EGuildDungeonState.Lock;
            ALUGUICommon.setGameObjEnable(wnd.lockShowGos, isLock);
            ALUGUICommon.setGameObjEnable(wnd.lockHideGos, !isLock);
        }
        
        // <AutoGen:Method>
        
        // 升级按钮点击事件
        private void _onClickbtnUpgrade(GameObject go)
        {
            if (_m_data != null &&  _m_data.lvlRefObj != null)
            {
                NPCommonCostItem costItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.guild_wealth_common_item,
                    _m_data.lvlRefObj.upgrade_cost_guild_wealth);
                if (!GCommon.isItemEnough(costItem, true))
                {
                    return;
                }
                NPMesMgr.instance.showCostItemMes(costItem, () =>
                    {
                        int oldLvl = _m_data.lvl;
                        NPPlayer.instance.guildDungeonComp.reqUpgradeDungeonLvl(_m_data.dungeonId, oldLvl, (_suc, _msg) =>
                        {
                            if (_suc && _msg != null)
                            {
                                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_GuildDungeonUpgradeSuc(_m_data.dungeonRefObj, oldLvl, _msg.getUpgradeLvl()));
                            }
                            _refreshWnd();
                        });
                    }, null, TransKeyConst.guild_dungeon_upgradeSecondCheckWndTitle_none,
                    TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_upgradeSecondCheckWndContent_none, 
                        costItem.count, costItem.getItemName()));
            }
        }
        // </AutoGen:Method>
    }
}
