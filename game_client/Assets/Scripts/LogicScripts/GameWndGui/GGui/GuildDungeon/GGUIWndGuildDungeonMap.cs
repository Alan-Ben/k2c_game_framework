using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟PVE地图
    /// </summary>
    public class GGUIWndGuildDungeonMap : _ATALBasicUIWnd<GGUIMonoGuildDungeonMap>
    {
        private static GGUIWndGuildDungeonMap _g_instance = new GGUIWndGuildDungeonMap();
    
        public static GGUIWndGuildDungeonMap instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonMap();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        private GuildDungeonInfo _m_data; // 当前副本数据
        [NotNull]private List<long> _m_tagList = new List<long>(); // 当前副本地图上标记列表
        private bool _m_isInTagMode = false; // 是否处于标记模式

        private NPGGuiWndTexture _m_bgWnd;
        private GGUIGuildDungeonMapMgr _m_sceneMgr;

        public bool isInTagMode => _m_isInTagMode;
        
        
        
        public GGUIWndGuildDungeonMap() : base(EALUIWndLayer.NORMAL)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonMap.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonMap.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch => true;

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DUNGEON_RESET, _onGuildDungeonReset);
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
            _m_bgWnd?.hideWnd();
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DUNGEON_RESET, _onGuildDungeonReset);
        }
    
        protected override void _onReset()
        {
            _m_bgWnd?.discardTexture();
        }
    
        protected override void _onDiscard()
        {
            _m_sceneMgr?.discard();
            _m_sceneMgr = null;
            _m_bgWnd?.discard();
            _m_bgWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnLog, _onClickbtnLog);
            ALUGUICommon.uncombineBtnClick(wnd.btnMark, _onClickbtnMark);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.mapMono != null)
            {
                _m_sceneMgr = new GGUIGuildDungeonMapMgr();
                _m_sceneMgr.init(wnd.mapMono);
            }

            if (wnd.imgBg != null) 
                _m_bgWnd = new NPGGuiWndTexture(wnd.imgBg);

            ALUGUICommon.combineBtnClick(wnd.btnLog, _onClickbtnLog);
            ALUGUICommon.combineBtnClick(wnd.btnMark, _onClickbtnMark);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
     
        public void setInfo(GuildDungeonInfo data)
        {
            _m_data = data;
            _m_isInTagMode = false;
            _refreshWnd();
        }
        
        /// <summary>
        /// 更新曲线参数（运行时调用）
        /// </summary>
        public void updateCurve()
        {
            if (_m_sceneMgr != null)
            {
                _m_sceneMgr.updateCurve(); // 重新绘制所有连线
            }
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if(_m_data == null)
                return;
            
            _m_sceneMgr?.refreshMap(_m_data);

            if (_m_data.dungeonRefObj != null)
            {
                _m_bgWnd?.showWnd();
                _m_bgWnd?.setTexture(_m_data.dungeonRefObj.map_bg_img);
            }
            ALUGUICommon.setGameObjEnable(wnd.goShowOnTagMode, _m_isInTagMode);
            ALUGUICommon.setGameObjEnable(wnd.goHideOnTagMode, !_m_isInTagMode);
        }

        
        // 切换标记状态
        public void changeTagMonster(long _tagId)
        {
            if (_m_tagList.Contains(_tagId))
            {
                _m_tagList.Remove(_tagId);
            }
            else
            {
                _m_tagList.Add(_tagId);
            }
        }

        // 是否已标记
        public bool isTagMonster(long _monsterId)
        {
            return _m_tagList.Contains(_monsterId);
        }

        private void enterTagMode()
        {
            if (_m_data == null) 
                return;
            _m_isInTagMode = true;
            if (_m_tagList != null)
            {
                _m_tagList.Clear();
                if (_m_data.tagMonsterIdsList != null)
                    _m_tagList.AddRange(_m_data.tagMonsterIdsList);
            }
            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_enter_tag_mode_tip));

            ALUGUICommon.setGameObjEnable(wnd.goShowOnTagMode, _m_isInTagMode);
            ALUGUICommon.setGameObjEnable(wnd.goHideOnTagMode, !_m_isInTagMode);
        }
        
        private void endTagMode(bool _setTag = true)
        {
            _m_isInTagMode = false;
            if (_m_data == null) 
                return;
            if (_setTag)
            {
                NPPlayer.instance.guildDungeonComp.reqSetTagMonsterList(_m_data.instanceId, _m_tagList, _suc =>
                {
                });
            }
            else
            {
                WinMsg.SendMsg(WinMsgType.ON_GUILD_DUNGEON_TAG_MONSTER_CHG, _m_data.instanceId);
            }
            _m_tagList.Clear();
        
            ALUGUICommon.setGameObjEnable(wnd.goShowOnTagMode, _m_isInTagMode);
            ALUGUICommon.setGameObjEnable(wnd.goHideOnTagMode, !_m_isInTagMode);
        }

        /// <summary>
        /// 联盟PVE信息重置回调，关闭地图窗口
        /// </summary>
        private void _onGuildDungeonReset(object _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUILD_DUNGEON_MAP);
        }
        
        // <AutoGen:Method>
        // 日志按钮点击事件
        private void _onClickbtnLog(GameObject go)
        {
            endTagMode(false);
            GGUIWndGuildDungeonLog.instance.setInfo(_m_data);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildDungeonLog.instance, GGUIWndGuildDungeonLog.instance.showWnd, UINodeTagConst.C_GUIlD_DUNGEON_LOG);
        }

        // 标记按钮点击事件
        private void _onClickbtnMark(GameObject go)
        {
            if(!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.SET_PVE_MONSTER_TAG , true))
                return;            
            if(!_m_isInTagMode)
                enterTagMode();
            else
                endTagMode();
        }
        

        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            endTagMode(false);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUILD_DUNGEON_MAP);
        }
        // </AutoGen:Method>
    }
}