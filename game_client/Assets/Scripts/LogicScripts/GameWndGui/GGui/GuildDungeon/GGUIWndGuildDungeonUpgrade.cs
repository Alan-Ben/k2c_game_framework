using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟副本升级
    /// </summary>
    public class GGUIWndGuildDungeonUpgrade : _ATALBasicUIWnd<GGUIMonoGuildDungeonUpgrade>
    {
        private static GGUIWndGuildDungeonUpgrade _g_instance = new GGUIWndGuildDungeonUpgrade();
    
        public static GGUIWndGuildDungeonUpgrade instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonUpgrade();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildDungeonUpgradeGrid _m_itemGridWnd;

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildDungeonUpgrade() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonUpgrade.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonUpgrade.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_SET_CHG, _onGuildDungeonChg);
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_SET_CHG, _onGuildDungeonChg);
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGuildDungeonUpgradeGrid(wnd.itemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            _m_itemGridWnd?.showWnd();
            _m_itemGridWnd?.showItemList(NPPlayer.instance.guildDungeonComp.dungeonList);
        }

        /// <summary>
        /// 联盟PVE信息变化回调
        /// </summary>
        private void _onGuildDungeonChg(object _obj)
        {
            _refreshWnd();
        }
        
        // <AutoGen:Method>
        
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_UPGRADE);
        }
        // </AutoGen:Method>
    }
}