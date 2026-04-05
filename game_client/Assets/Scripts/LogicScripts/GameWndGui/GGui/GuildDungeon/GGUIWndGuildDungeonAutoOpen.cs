using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟PVE自动开启
    /// </summary>
    public class GGUIWndGuildDungeonAutoOpen : _ATALBasicUIWnd<GGUIMonoGuildDungeonAutoOpen>
    {
        private static GGUIWndGuildDungeonAutoOpen _g_instance = new GGUIWndGuildDungeonAutoOpen();
    
        public static GGUIWndGuildDungeonAutoOpen instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildDungeonAutoOpen();
                return _g_instance;
            }
        }
        
        private GGUIWndGuildDungeonAutoOpenGrid _m_itemGridWnd;
        private List<GuildDungeonInfo> _m_itemDataList = new List<GuildDungeonInfo>();
        private List<long> _m_autoStartDataList = new List<long>();

        private GGUIWndGuildDungeonTimeContainer _m_hourSelectContainer;
        private GGUIWndGuildDungeonTimeContainer _m_minuteSelectContainer;
        private int _m_curHour = 0; // 当前选择的小时
        private int _m_curMinute = 0; // 当前选择的分钟
        

        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>

        public GGUIWndGuildDungeonAutoOpen() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGuildDungeonAutoOpen.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGuildDungeonAutoOpen.objName; }
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
            _m_itemGridWnd?.discard();
            _m_itemGridWnd = null;
            _m_autoStartDataList?.Clear();
            if(_m_hourSelectContainer != null)
            {
                _m_hourSelectContainer.onSelectItem -= _onSelectHour;
                _m_hourSelectContainer.discard();
                _m_hourSelectContainer = null;
            }
            
            if(_m_minuteSelectContainer != null)
            {
                _m_minuteSelectContainer.onSelectItem -= _onSelectMinute;
                _m_minuteSelectContainer.discard();
                _m_minuteSelectContainer = null;
            }
            
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickbtnConfirm);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.itemGrid != null)
                _m_itemGridWnd = new GGUIWndGuildDungeonAutoOpenGrid(wnd.itemGrid, onDungeonItemClick);

            if (wnd.hourSelectContainer != null)
            {
                _m_hourSelectContainer = new GGUIWndGuildDungeonTimeContainer(wnd.hourSelectContainer);
                _m_hourSelectContainer.onSelectItem += _onSelectHour;
                List<int> hourList = new List<int>();
                for (int i = 0; i < 24; i++)
                {
                    hourList.Add(i);
                }
                _m_hourSelectContainer.setShowData(hourList, _setHourDataShow);
            }
            
            if(wnd.minuteSelectContainer != null)
            {
                _m_minuteSelectContainer = new GGUIWndGuildDungeonTimeContainer(wnd.minuteSelectContainer);
                _m_minuteSelectContainer.onSelectItem += _onSelectMinute;
                
                List<int> minuteList = new List<int>();
                for (int i = 0; i < 60; i++)
                {
                    minuteList.Add(i);
                }
                _m_minuteSelectContainer.setShowData(minuteList, _setMinuteDataShow);
            }
            
            
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickbtnConfirm);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
        }

        private void onDungeonItemClick(GuildDungeonInfo _obj)
        {
            if (_obj == null) 
                return;

            if (_m_autoStartDataList.Contains(_obj.dungeonId))
            {
                _m_autoStartDataList.Remove(_obj.dungeonId);
            }
            else
            {
                _m_autoStartDataList.Add(_obj.dungeonId);
            }

            if (_m_itemGridWnd != null)
            {
                _m_itemGridWnd.showItemList(NPPlayer.instance.guildDungeonComp.dungeonList, _m_autoStartDataList);
            }
            _updateGuildWealth();

        }


        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _m_autoStartDataList.Clear();
            NPPlayer.instance.guildDungeonComp.reqDungeonGlobalSet((_autoStartList, _hour, _min) =>
            {
                _m_autoStartDataList = _autoStartList;
                if (_m_itemGridWnd != null)
                {
                    _m_itemGridWnd.showWnd();
                    _m_itemGridWnd.showItemList(NPPlayer.instance.guildDungeonComp.dungeonList, _m_autoStartDataList);
                }


                if (_m_hourSelectContainer != null)
                {
                    _m_curHour = _hour;
                    ALCommonTaskController.CommonActionAddNextFrameTask(()=>
                    {
                        _m_hourSelectContainer.refreshLayout(true, _m_curHour);
                    });
                    // _m_hourSelectContainer.scrollMoveTo(_m_curHour, false);
                }
            
                if (_m_minuteSelectContainer != null)
                {
                    _m_curMinute = _min;
                    ALCommonTaskController.CommonActionAddNextFrameTask(()=>
                    {
                        _m_minuteSelectContainer.refreshLayout(true, _m_curMinute);
                    });
                    // _m_minuteSelectContainer.scrollMoveTo(_m_curMinute, false);
                }
                _updateGuildWealth();
            });
        }

        private void _updateGuildWealth()
        {
            if(wnd == null || wnd.txtGuildWealth == null)
                return;
            long needGuildWealth = 0;
            foreach (long dungeonId in _m_autoStartDataList)
            {
                GuildDungeonInfo dungeonInfo = NPPlayer.instance.guildDungeonComp.getDungeonInfoById(dungeonId);
                if (dungeonInfo != null) needGuildWealth += dungeonInfo.starCostGuildWealth;
            }
            ALUGUICommon.setLabelTxt(wnd.txtGuildWealth, TextTranslate.instance.getLanguage(TransKeyConst.guild_dungeon_auto_open_cost_desc_numnum, NPPlayer.instance.guildComp.guildInfo.guildWealth.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), needGuildWealth.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

        }

        private void _setHourDataShow(GGUIWndGuildDungeonTimeContainerItem _itemWnd, int _hour)
        {
            _itemWnd?.setInfo(_hour);
        }
        
        private void _setMinuteDataShow(GGUIWndGuildDungeonTimeContainerItem _itemWnd, int _minute)
        {
            _itemWnd?.setInfo(_minute);
        }

        private void _onSelectHour(GGUIWndGuildDungeonTimeContainerItem _obj)
        {
            if (_obj == null) 
                return;
            
            _m_curHour = _obj.value;
        }
        
        private void _onSelectMinute(GGUIWndGuildDungeonTimeContainerItem _obj)
        {
            if (_obj == null) 
                return;
            
            _m_curMinute = _obj.value;
        }
        
        // <AutoGen:Method>
        
        // 确定按钮点击事件
        private void _onClickbtnConfirm(GameObject go)
        {
            NPPlayer.instance.guildDungeonComp.reqSetAutoStartDungeon(_m_autoStartDataList, _m_curHour, _m_curMinute,
                _suc =>
                {
                    if (_suc)
                    { 
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_AUTO_OPEN);
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_dungeon_autoStart_set_suc_tip);
                    }
                } );
        }

        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_DUNGEON_AUTO_OPEN);
        }
        // </AutoGen:Method>
    }
}