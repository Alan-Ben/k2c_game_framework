using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonEnum;
using Common.GuildEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟PVE主界面
    /// </summary>
    public class GGUIWndGuildDungeonMainGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildDungeonMainGridItem>
    {
        private GuildDungeonInfo _m_data; // 数据
        // <AutoGen:WndDeclaration>
        private GGUIWndLongProgress _m_progressBloodWnd;  // 血条
        private NPGGuiWndTexture _m_bossIconWnd; // boss形象
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildDungeonMainGridItem(GGUIMonoGuildDungeonMainGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_progressBloodWnd?.hideWnd();
            _m_bossIconWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_progressBloodWnd?.resetWnd();
            _m_bossIconWnd?.hideWnd();
        }
    
        protected override void _onDiscard()
        {
            ALUGUICommon.uncombineBtnClick(wnd.btnEnter, _onClickbtnEnter);
            _m_progressBloodWnd?.discard();
            _m_progressBloodWnd = null;
            _m_bossIconWnd?.discard();
            _m_bossIconWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnEnter, _onClickbtnEnter);
            if (wnd.progressBlood != null)
                _m_progressBloodWnd = new GGUIWndLongProgress(wnd.progressBlood);
            if (wnd.bossIcon != null)
                _m_bossIconWnd = new NPGGuiWndTexture(wnd.bossIcon);
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

            if(_m_data == null || _m_data.dungeonRefObj == null)
                return;
            GuildDungeonLvlRefObj instanceLvlRef = _m_data.dungeonRefObj.getLvlRef(_m_data.instanceLvl);
            if(_m_progressBloodWnd != null)
            {
                long bossMaxHp = instanceLvlRef?.getMonsterHp(EGuildDungeon_MonsterType.BOSS) ?? 0;
                _m_progressBloodWnd.showWnd();
                _m_progressBloodWnd.initSld(0, bossMaxHp, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_progressBloodWnd.setNowValue(_m_data.instanceId > 0 && _m_data.bossMonster != null ? _m_data.bossMonster.hp: bossMaxHp);
            }
            ALUGUICommon.setLabelTxt(wnd.txtNum, _m_data.dungeonRefObj.id);
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_data.dungeonRefObj?.getFullTransName(_m_data.instanceLvl));
            ALUGUICommon.setLabelTxt(wnd.txtReward, instanceLvlRef?.finish_guild_exp ?? 0);
            ALUGUICommon.setLabelTxt(wnd.txtProcess, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _m_data.endMonsterCount, _m_data.totalMonsterCount));
            ALUGUICommon.setLabelTxt(wnd.txtLockCondition, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_unlockCondition_num, _m_data.dungeonRefObj.unlock_need_guild_lvl));
            if(_m_bossIconWnd != null)
            {
                _m_bossIconWnd.setTexture(_m_data.dungeonRefObj.icon);
                _m_bossIconWnd.showWnd();
            }
           

            foreach (GuildDungeonStateShow stateShow in wnd.stateShows)
            {
                if (stateShow != null) ALUGUICommon.setGameObjEnable(stateShow.showObjs, false);
            }
            foreach (GuildDungeonStateShow stateShow in wnd.stateShows)
            {
                if (stateShow != null && stateShow.state == _m_data.state)
                    ALUGUICommon.setGameObjEnable(stateShow.showObjs, true);
            }
            ALUGUICommon.setGameObjEnable(wnd.goShowOnReward, _m_data.hasRewardToGain);

            //显示红点
            long leftHeroTotalPower = NPPlayer.instance.guildDungeonComp.getLeftHeroTotalPower();
            ALUGUICommon.setGameObjEnable(wnd.goRedTip, (leftHeroTotalPower > 0 && _m_data.state == EGuildDungeonState.Started) || _m_data.hasRewardToGain);
        }
        
        // <AutoGen:Method>
        
        // 进入按钮点击事件
        private void _onClickbtnEnter(GameObject go)
        {
            if(_m_data == null)
                return;
            
            if (_m_data.state == EGuildDungeonState.WaitStart)
            {
                bool canUseGuildWealth = NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.USE_GUILD_WEALTH, false);

                if (!canUseGuildWealth && !NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.USE_ITEM_START_PVE, false))
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_no_permit_to_optn_tip));
                    return;
                }
                GGUIWndGuildDungeonStart.instance.setInfo(_m_data);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildDungeonStart.instance, GGUIWndGuildDungeonStart.instance.showWnd, UINodeTagConst.C_GUIlD_DUNGEON_START);
                return;
            }

            if (_m_data.state == EGuildDungeonState.Lock)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_lock_tip));
                return;
            }

            GGUIWndGuildDungeonMap.instance.setInfo(_m_data);
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildDungeonMap.instance, UINodeTagConst.C_GUILD_DUNGEON_MAP, null, null, 0);
        }
        // </AutoGen:Method>
        
        static string _getCommonSliderTxtStr(string _cur, string _max)
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
        }
    }
}
