using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonObj;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟PVE主界面
    /// </summary>
    public class GGUIWndGuildDungeonMain : _ATALBasicUIWnd<GGUIMonoGuildDungeonMain>
    {
        private static GGUIWndGuildDungeonMain _g_instance = new GGUIWndGuildDungeonMain();
    
        public static GGUIWndGuildDungeonMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonMain();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildDungeonMainGrid _m_itemGridWnd;

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        private int _m_timeDownSer;
        
        public GGUIWndGuildDungeonMain() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_INSTANCE_CHG, _onGuildDungeonInstanceChg);
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_RESET, _onGuildDungeonReset);
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_SET_CHG, _onGuildDungeonSetChg);

            _refreshWnd();

            //设置红点已读
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_DUNGEON_UNLOCK, 0);
            AccountSettingMgr.instance.accountSetting.addAlreadyReadGuildRedTip(RedTipConst.RED_GUILD_DUNGEON_UNLOCK);
        }
    
        protected override void _onHideWnd()
        {
            _m_timeDownSer = ALSerializeOpMgr.next();

            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_INSTANCE_CHG, _onGuildDungeonInstanceChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_RESET, _onGuildDungeonReset);
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_SET_CHG, _onGuildDungeonSetChg);
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            _m_timeDownSer = ALSerializeOpMgr.next();

            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickBtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnAutoStart, _onClickbtnAutoStart);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrate, _onClickbtnUpgrate);
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onClickbtnRank);
            ALUGUICommon.uncombineBtnClick(wnd.btnClaimAll, _onClickbtnClaimAll);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGuildDungeonMainGrid(wnd.itemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickBtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnAutoStart, _onClickbtnAutoStart);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrate, _onClickbtnUpgrate);
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onClickbtnRank);
            ALUGUICommon.combineBtnClick(wnd.btnClaimAll, _onClickbtnClaimAll);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            List<GuildDungeonInfo> dungeonList = NPPlayer.instance.guildDungeonComp.dungeonList;
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(dungeonList);
            
            ALUGUICommon.setLabelTxt(wnd.txtResetTime,  TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_aotu_settle_time, TimeUtil.millisecondsToTime_hms(GRefdataCoreMgr.instance.npGeneral.guild_dungeon_auto_settle_time.getNextRefreshLastTimeMs())));
            _m_timeDownSer = ALSerializeOpMgr.next();
            _refreshTimeDown(_m_timeDownSer);
            
            
            ALUGUICommon.setGameObjEnable(wnd.autoStartShowGos, NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.SET_PVE_AUTO_STAR , false));
            ALUGUICommon.setGameObjEnable(wnd.upgradeShowGos, NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.USE_GUILD_WEALTH , false));
            
            bool hasReward = false;
            if (dungeonList != null)
                foreach (GuildDungeonInfo dungeonInfo in dungeonList)
                {
                    if (dungeonInfo != null && dungeonInfo.hasRewardToGain)
                    {
                        hasReward = true;
                        break;
                    }
                }
            ALUGUICommon.setGameObjEnable(wnd.hasRewardShowGos, hasReward);
        }

        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;
           
            ALUGUICommon.setLabelTxt(wnd.txtResetTime,  TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_aotu_settle_time, TimeUtil.millisecondsToTime_hms(GRefdataCoreMgr.instance.npGeneral.guild_dungeon_auto_settle_time.getNextRefreshLastTimeMs())));
            if (_timeDownSer != _m_timeDownSer)
                return;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }

        /// <summary>
        /// 联盟PVE实例信息变化回调
        /// </summary>
        private void _onGuildDungeonInstanceChg(params object[] _obj)
        {
            _refreshWnd();
        }

        /// <summary>
        /// 联盟PVE信息重置回调
        /// </summary>
        /// <summary>
        /// 联盟PVE信息重置回调
        /// </summary>
        private void _onGuildDungeonReset(params object[] _obj)
        {
            _refreshWnd();
        }
        
        /// <summary>
        /// 联盟PVE信息设置回调
        /// </summary>
        private void _onGuildDungeonSetChg(params object[] _obj)
        {
            _refreshWnd();
        }
        // 自动开启点击事件
        private void _onClickBtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_MAIN);
        }
        
        // <AutoGen:Method>
        
        // 自动开启点击事件
        private void _onClickbtnAutoStart(GameObject go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildDungeonAutoOpen.instance, GGUIWndGuildDungeonAutoOpen.instance.showWnd, UINodeTagConst.C_GUIlD_DUNGEON_AUTO_OPEN);
        }

        // 副本升级点击事件
        private void _onClickbtnUpgrate(GameObject go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildDungeonUpgrade.instance, GGUIWndGuildDungeonUpgrade.instance.showWnd, UINodeTagConst.C_GUIlD_DUNGEON_UPGRADE);
        }

        // 战报排行点击事件
        private void _onClickbtnRank(GameObject go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildDungeonRank.instance, GGUIWndGuildDungeonRank.instance.showWnd, UINodeTagConst.C_GUIlD_DUNGEON_RANK);
        }

        // 一键领取点击事件
        private void _onClickbtnClaimAll(GameObject go)
        {
            NPPlayer.instance.guildDungeonComp.reqGainAllDungeonReward(_suc =>
            {
                _refreshWnd();
            });
        }
        // </AutoGen:Method>
    }
}