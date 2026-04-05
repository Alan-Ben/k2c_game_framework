using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonEnum;
using Common.GuildDungeonObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟副本日志
    /// </summary>
    public class GGUIWndGuildDungeonLog : _ATALBasicUIWnd<GGUIMonoGuildDungeonLog>
    {
        private static GGUIWndGuildDungeonLog _g_instance = new GGUIWndGuildDungeonLog();
    
        public static GGUIWndGuildDungeonLog instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonLog();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildDungeonLogGrid _m_itemGridWnd;
        private List<GuildDungeonLog> _m_itemDataList = new List<GuildDungeonLog>();
        private GuildDungeonInfo _m_dungeonInfo;
        
        private bool _m_isReqPageListIng = false;
        private int _m_refreshSerialize = -1;
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildDungeonLog() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonLog.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonLog.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_MONSTER_CHG, _onGuildDungeonMonsterChg);
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_MONSTER_CHG, _onGuildDungeonMonsterChg);
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            _m_itemDataList?.Clear();
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.itemGrid != null)
            {
                _m_itemGridWnd = new GGUIWndGuildDungeonLogGrid(wnd.itemGrid);
            }
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_LOG);
        }

        public void setInfo(GuildDungeonInfo _dungeonInfo)
        {
            _m_dungeonInfo = _dungeonInfo;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _refreshList();
        }
        
        /// <summary>
        /// 刷列表
        /// </summary>
        /// <param name="_pageStartSerial"></param>
        private void _refreshList()
        {
            if(_m_dungeonInfo == null)
                return;
            
            if (_m_isReqPageListIng)
                return;
            int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            _m_refreshSerialize = ALSerializeOpMgr.next();
            int refreshSerialize = _m_refreshSerialize;
            _m_isReqPageListIng = true;
            NPPlayer.instance.guildDungeonComp.reqGetDungeonLogList(_m_dungeonInfo.instanceId, (_msg) =>
            {
                if (refreshSerialize != _m_refreshSerialize || _msg == null)
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                    return;
                }
                _m_itemDataList.Clear();
                foreach (GuildDungeon_Log log in _msg.getLogList())
                {
                    if(log == null)
                        continue;
                    EGuildDungeon_LogType logType = log.getLogType();
                    if (logType == EGuildDungeon_LogType.ATTACK)
                    {
                        GuildDungeonLog lastLog = _m_itemDataList.GetLast();
                        GuildDungeon_LogAttack logAttack = new GuildDungeon_LogAttack();
                        logAttack.readPackage(log.getInfo());
                        if (lastLog != null && lastLog.logType == EGuildDungeon_LogType.ATTACK && lastLog.cid == logAttack.getCid() && lastLog.monsterId == logAttack.getMonsterId())
                        {
                            // 合并攻击日志
                            lastLog.combineAttackLog( logAttack.getDamage(), log.getCreatedAt());
                        }
                        else
                        {
                            GuildDungeonLog logInfo = new GuildDungeonLog(logType, log.getCreatedAt());
                            logInfo.initAttackInfo(logAttack.getCid(),  logAttack.getMonsterId(), logAttack.getDamage());
                            _m_itemDataList.Add(logInfo);
                        }
                    }
                    else if(logType == EGuildDungeon_LogType.KILL)
                    {
                        GuildDungeon_LogKill logKill = new GuildDungeon_LogKill();
                        logKill.readPackage(log.getInfo());
                        GuildDungeonLog logInfo = new GuildDungeonLog(logType, log.getCreatedAt());
                        logInfo.initKillInfo(logKill.getCid(),  logKill.getMonsterId());
                        _m_itemDataList.Add(logInfo);
                    }
                    else if(logType == EGuildDungeon_LogType.START)
                    {
                        GuildDungeon_LogStart logStart = new GuildDungeon_LogStart();
                        logStart.readPackage(log.getInfo());
                        GuildDungeonLog logInfo = new GuildDungeonLog(logType, log.getCreatedAt());
                        logInfo.initStartInfo(logStart.getDungeonId(), logStart.getCid(), logStart.getStartType(), logStart.getStartCost());
                        _m_itemDataList.Add(logInfo);
                    }
                }
             
                _refreshGrid();
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    _m_isReqPageListIng = false;
                    MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
                }, 0.2f);
            });
        }
        private void _refreshGrid()
        {
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(_m_itemDataList);
            if (wnd != null) 
                ALUGUICommon.setGameObjEnable(wnd.emptyShowGos, _m_itemDataList?.Count == 0);
        }

        /// <summary>
        /// 联盟PVE怪物信息变化回调
        /// </summary>
        private void _onGuildDungeonMonsterChg(params object[] _obj)
        {
            // 判断实例id一致才刷新界面
            if (_m_dungeonInfo == null)
                return;
                
            if (_obj != null && _obj.Length > 0 && _obj[0] is long dungeonInstanceId)
            {
                if ( _m_dungeonInfo.instanceId != dungeonInstanceId)
                    return;
            }
            
            _refreshWnd();
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}