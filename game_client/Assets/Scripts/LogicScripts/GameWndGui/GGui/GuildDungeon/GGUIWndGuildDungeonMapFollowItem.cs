using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Common.DinnerEnum;
using Common.GuildDungeonEnum;
using NPEnum;
using GOE.FollowItem;

namespace GOE
{
    /// <summary>
    /// 联盟PVE地图跟随窗口，
    /// </summary>
    public class GGUIWndGuildDungeonMapFollowItem : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoGuildDungeonMapFollowItem>
    {
        // <AutoGen:WndDeclaration>
        private GGUIWndLongProgress _m_progressBloodWnd;  // 血条
        private NPGGuiWndTexture _m_imgIconWnd; // 怪物图标
        // </AutoGen:WndDeclaration>

        private GuildDungeonMonster _m_monster;
        private long _m_resPathId = 6911;
        private Vector3 _m_rootPos;
        
        public GGUIWndGuildDungeonMapFollowItem(GuildDungeonMonster _monster, Vector3 _pos, Transform _root) : base(_root)
        {
            _m_monster = _monster;
            _m_resPathId = _m_monster != null && _m_monster.monsterRefObj != null && _m_monster.monsterRefObj.monster_type == EGuildDungeon_MonsterType.BOSS ? 6912 : 6911;
            _m_rootPos = _pos;
        }

        protected override string _monoAssetPath => UIResPathAssistant.getAssetPath(_m_resPathId);

        protected override string _monoObjName => UIResPathAssistant.getObjName(_m_resPathId);

        protected override _AALResourceCore _resourceCore => GameResCore.instance;
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_MONSTER_CHG, _onMonsterChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_TAG_MONSTER_CHG, _onMonsterTagChg);
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_MONSTER_CHG, _onMonsterChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_TAG_MONSTER_CHG, _onMonsterTagChg);
            _m_progressBloodWnd?.hideWnd();
            _m_imgIconWnd?.hideWnd();
        }
    
        protected override void _onReset()
        {
            _m_progressBloodWnd?.resetWnd();
            _m_imgIconWnd?.hideWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_progressBloodWnd?.discard();
            _m_progressBloodWnd = null;
            _m_imgIconWnd?.discard();
            _m_imgIconWnd = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnBattle, _onClickbtnBattle);
                ALUGUICommon.uncombineBtnClick(wnd.btnShowReward, _onClickShowReward);
            }
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
       
            wnd.transform.position = _m_rootPos;
            
            if (wnd.progressBlood != null)
                _m_progressBloodWnd = new GGUIWndLongProgress(wnd.progressBlood);
            if (wnd.imgIcon != null)
                _m_imgIconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            ALUGUICommon.combineBtnClick(wnd.btnBattle, _onClickbtnBattle);
            ALUGUICommon.combineBtnClick(wnd.btnShowReward, _onClickShowReward);
        }

      

        public void refreshWnd()
        {
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            if(_m_monster == null)
                return;
            if(_m_progressBloodWnd != null)
            {
                string _getCommonSliderTxtStr(string _cur, string _max)
                {
                    return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, _cur, _max);
                }
                _m_progressBloodWnd.showWnd();
                _m_progressBloodWnd.initSld(0, _m_monster.maxHp, _getCommonSliderTxtStr, EValueFormatType.NORMAL);
                _m_progressBloodWnd.setNowValue(_m_monster.hp);
            }
            if(_m_imgIconWnd != null)
            {
                _m_imgIconWnd.setTexture(_m_monster.icon);
                _m_imgIconWnd.showWnd();
            }
            GuildDungeonInfo dungeonInfo = _m_monster.dungeonInfo;


            if (dungeonInfo != null)
            {
                bool hasTag = dungeonInfo.hasTagMonster(_m_monster.monsterId);
                if (GGUIWndGuildDungeonMap.instance.isInTagMode)
                    hasTag = GGUIWndGuildDungeonMap.instance.isTagMonster(_m_monster.monsterId);
                
                ALUGUICommon.setGameObjEnable(wnd.listTagShowGos, hasTag);
                bool preHasDefeat = false;
                if (_m_monster.monsterRefObj.pre_monster_id_list.Count > 0)
                {
                    foreach (long preId in _m_monster.monsterRefObj.pre_monster_id_list)
                    {
                        GuildDungeonMonster preMonsterInfo = dungeonInfo.getMonsterInfo(preId);
                        if (preMonsterInfo != null && preMonsterInfo.hp <= 0)
                        {
                            preHasDefeat = true;
                            break;
                        }
                    }
                }
                else
                {
                    preHasDefeat = true; // 没有前置怪物, 则默认可以挑战
                }
                bool canFight = preHasDefeat && _m_monster.hp > 0;
                ALUGUICommon.setGameObjEnable(wnd.listCanFightShowGos, canFight);

                bool hasBox = _m_monster.isReward;
                bool isDefeat = _m_monster.hp <= 0;
                bool hasGainBox = NPPlayer.instance.guildDungeonComp.hasGainedRewardMonster(dungeonInfo.instanceId, _m_monster.monsterId);
                

                GuildDungeonMonsterState monsterState = GuildDungeonMonsterState.NotDefeat;
                if (isDefeat)
                {
                    if (hasBox)
                    {
                        monsterState = hasGainBox ? GuildDungeonMonsterState.GainedReward : GuildDungeonMonsterState.CanReward;
                    }
                    else
                    {
                        monsterState = GuildDungeonMonsterState.Defeat;
                    }
                }
                else
                {
                    monsterState = hasBox ? GuildDungeonMonsterState.NotDefeatReward : GuildDungeonMonsterState.NotDefeat;
                }

                if (wnd.monsterStateShowGos != null)
                {
                    foreach (var stateShow in wnd.monsterStateShowGos)
                    {
                        if (stateShow != null) ALUGUICommon.setGameObjEnable(stateShow.showGos, false);
                    }

                    foreach (var stateShow in wnd.monsterStateShowGos)
                    {
                        if (stateShow != null && stateShow.state == monsterState)
                            ALUGUICommon.setGameObjEnable(stateShow.showGos, true);
                    }
                }
            }
        }
        
        private void _onMonsterChg(params object[] _objs)
        {
            // 判断实例id一致才刷新界面
            if (_m_monster == null)
                return;
                
            if (_objs != null && _objs.Length > 1 && _objs[1] is long monsterId)
            {
                // 检查怪物ID是否匹配当前显示的怪物
                if (monsterId != _m_monster.monsterId)
                    return;
            }
            
            _refreshWnd();
        }
        
        private void _onMonsterTagChg(params object[] _objs)
        {
            // 判断实例id一致才刷新界面
            if (_m_monster == null)
                return;
                
            if (_objs != null && _objs.Length > 0 && _objs[0] is long instanceId)
            {
                if(_m_monster.dungeonInfo != null && instanceId != _m_monster.dungeonInfo.instanceId)
                    return;
            }
            
            _refreshWnd();
        }
        
        // <AutoGen:Method>
        // 战斗按钮点击事件
        private void _onClickbtnBattle(GameObject go)
        {
            if (_m_monster == null || _m_monster.monsterRefObj == null || _m_monster.monsterRefObj.pre_monster_id_list == null)
                return;
            
            GuildDungeonInfo dungeonInfo = _m_monster.dungeonInfo;

            if (GGUIWndGuildDungeonMap.instance.isInTagMode)
            {
                GGUIWndGuildDungeonMap.instance.changeTagMonster(_m_monster.monsterId);
                refreshWnd();
                return;
            }
            if (dungeonInfo == null)
                return;

            bool preHasDefeat = false;
            if (_m_monster.monsterRefObj.pre_monster_id_list.Count > 0)
            {
                foreach (long preId in _m_monster.monsterRefObj.pre_monster_id_list)
                {
                    GuildDungeonMonster preMonsterInfo = dungeonInfo.getMonsterInfo(preId);
                    if (preMonsterInfo != null && preMonsterInfo.hp <= 0)
                    {
                        preHasDefeat = true;
                        break;
                    }
                }
            }
            else
            {
                preHasDefeat = true; // 没有前置怪物, 则默认可以挑战
            }


            if (!preHasDefeat)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_dungeon_preMonsterHasNotDefeat_tip);
                return;
            }

            // 如果有奖励
            if (_m_monster.hp <= 0 )
            {
                if (_m_monster.isReward && !NPPlayer.instance.guildDungeonComp.hasGainedRewardMonster(dungeonInfo.instanceId, _m_monster.monsterId))
                {
                    NPPlayer.instance.guildDungeonComp.reqGainDungeonReward(dungeonInfo.instanceId, _m_monster.monsterId,
                        _suc =>
                        {
                            if(_suc)
                                refreshWnd();
                        });
                    return;
                }
                else
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_dungeon_monsterHasDefeat_tip);
                    return;
                }
           
            }
            

            GGUIWndGuildDungeonBattle.instance.setInfo(_m_monster);
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildDungeonBattle.instance, UINodeTagConst.C_GUIlD_DUNGEON_BATTLE, null, null, 0);
        }
        // </AutoGen:Method>

        private void _onClickShowReward(GameObject _go)
        {
            if (_m_monster == null) 
                return;
            GuildDungeonInfo dungeonInfo = _m_monster.dungeonInfo;

            if (dungeonInfo == null || dungeonInfo.dungeonRefObj == null || _m_monster.monsterRefObj == null || !_m_monster.isReward)
                return;
            // 如果有奖励
            bool hasGainedReward = NPPlayer.instance.guildDungeonComp.hasGainedRewardMonster(dungeonInfo.instanceId, _m_monster.monsterId);
            ECommonRewardType rewardType = ECommonRewardType.NONE;
            if (_m_monster.hp <= 0)
                rewardType = hasGainedReward ? ECommonRewardType.HAS_GET_REWARD : ECommonRewardType.CAN_GET_REWARD;
            else
                rewardType = ECommonRewardType.NOT_GET_REWARD;

            List<NPCommonCostItem> itemList = new List<NPCommonCostItem>();
            if (_m_monster.monsterRefObj.monster_type == EGuildDungeon_MonsterType.BOSS)
            {
                GCommon.itemTONoRewardItemList(dungeonInfo.dungeonRefObj.kill_boss_reward, itemList, 0);
            }
            else
            {
                GuildDungeonMonsterReward monsterReward = dungeonInfo.dungeonRefObj.getMonsterReward(_m_monster.monsterRefObj.monster_type);
                if (monsterReward != null) GCommon.itemTONoRewardItemList(monsterReward.reward, itemList, 0);
            }
            
            long uiPathId = wnd.boxRewardPreviewResPathId;
            string titleStr = string.IsNullOrEmpty(wnd.boxRewardPreviewTitleStrKey) ? "" : TextTranslate.instance.getLanguage(wnd.boxRewardPreviewTitleStrKey);
            
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Reward(
                uiPathId, 
                titleStr, 
                string.Empty, 
                itemList, 
                wnd.boxRewardPreviewRoot, 
                wnd.boxRewardPreviewToolTipOffset, 
                rewardType));
        }
    }
}