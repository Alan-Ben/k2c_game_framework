using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonEnum;
using Common.GuildDungeonObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟副本日志
    /// </summary>
    public class GGUIWndGuildDungeonLogGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildDungeonLogGridItem>
    {
        private GuildDungeonLog _m_data;

        private long _m_serializeOp;
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonLogGridItem(GGUIMonoGuildDungeonLogGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_serializeOp = ALSerializeOpMgr.next();
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        protected override void _resetGridItem()
        {
            
            
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(GuildDungeonLog _data)
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
            {
                return;
            }

            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.DateTime2StringHMS(TimeUtil.FromUTCSeconds(_m_data.timeS)));

            _m_serializeOp = ALSerializeOpMgr.next();
            long serializeOp = _m_serializeOp;
            GCommon.reqPlayerInfo(_m_data.cid, _info =>
            {
                if (serializeOp != _m_serializeOp || _info == null)
                    return;
                setContent(_info.name);
            });
          
        }

        private void setContent(string _playerName)
        {
            if(wnd == null || _m_data == null)
                return;
                
            switch (_m_data.logType)
            {
                case EGuildDungeon_LogType.NONE:

                    break;
                case EGuildDungeon_LogType.ATTACK:
                {
                    GuildDungeonMonsterRefObj monsterRefObj = GRefdataCoreMgr.instance.guildDungeonMonsterRefCore.getRef(_m_data.monsterId);
                    if (monsterRefObj != null)
                    {
                        GuildDungeonMonsterShowRefObj showRefObj = GRefdataCoreMgr.instance.guildDungeonMonsterShowRefCore.getRef(monsterRefObj.monster_show_id);
                        if (showRefObj != null)
                            ALUGUICommon.setLabelTxt(wnd.txtConten, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_log_attack_desc_string_string_num, _playerName, TextTranslate.instance.getLanguage(showRefObj.name), _m_data.damage));
                    }
                }
                break;
                case EGuildDungeon_LogType.KILL:
                {
                    GuildDungeonMonsterRefObj monsterRefObj = GRefdataCoreMgr.instance.guildDungeonMonsterRefCore.getRef(_m_data.monsterId);
                    if (monsterRefObj != null)
                    {
                        GuildDungeonMonsterShowRefObj showRefObj = GRefdataCoreMgr.instance.guildDungeonMonsterShowRefCore.getRef(monsterRefObj.monster_show_id);
                        if (showRefObj != null)
                        {
                            string monsterName = TextTranslate.instance.getLanguage(showRefObj.name);
                            ALUGUICommon.setLabelTxt(wnd.txtConten, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_log_kill_desc_string_string, _playerName, monsterName));
                        }
                    }
                }
                    break;
                case EGuildDungeon_LogType.START:
                    string costDesc = "";
                    if (_m_data.startType == EGuildDungeon_StartType.COMMON_ITEM)
                    {
                        GuildDungeonInfo dungeonInfo = NPPlayer.instance.guildDungeonComp.getDungeonInfoById(_m_data.dungeonId);
                        NPCommonCostItem startCostItem = dungeonInfo?.lvlRefObj?.star_cost_item;
                        costDesc = TextTranslate.instance.getLanguage(
                            TransKeyConst.guild_dungeon_log_start_cost_desc_num_name,_m_data.startCost, startCostItem?.getItemName());
                    }
                    else
                    {
                        NPCommonCostItem guildWealthCostItem = new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.guild_wealth_common_item, 1);
                        costDesc = TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_log_start_cost_desc_num_name, _m_data.startCost, guildWealthCostItem.getItemName());
                    }
                    GuildDungeonRefObj dungeonRefObj = GRefdataCoreMgr.instance.guildDungeonRefCore.getRef(_m_data.dungeonId);
                    ALUGUICommon.setLabelTxt(wnd.txtConten, 
                        TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_log_start_desc_string_string_string, _playerName, costDesc,TextTranslate.instance.getLanguage(dungeonRefObj?.name)));
                    break;
            }
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
