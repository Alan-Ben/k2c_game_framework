using ALPackage;
using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家卧室皮肤界面
    /// </summary>
    public class GGUIWndPlayerRoomSkin : _ATALBasicUIWnd<GGUIMonoPlayerRoomSkin>
    {
        private static GGUIWndPlayerRoomSkin _g_instance;
        public static GGUIWndPlayerRoomSkin instance { get { return _g_instance ??= new GGUIWndPlayerRoomSkin(); } }

        // 选中卧室皮肤背景图
        private NPGGuiWndTexture _m_wSelectedRoomSkinBg;
        // 卧室皮肤列表 Grid
        private GGUIWndPlayerRoomSkinItemGrid _m_roomSkinItemGridWnd;

        // 所有皮肤配表数据列表
        [NotNull] private List<PlayerRoomSkinRefObj> _m_allRoomSkinRefObjList = new List<PlayerRoomSkinRefObj>();
        // 皮肤拥有信息字典
        [NotNull] private Dictionary<PlayerRoomSkinRefObj, PlayerRoomSkinInfo> _m_roomSkinInfoDict = new Dictionary<PlayerRoomSkinRefObj, PlayerRoomSkinInfo>();
        // 皮肤状态字典
        [NotNull] private Dictionary<PlayerRoomSkinRefObj, EPlayerRoomSkinState> _m_roomSkinStateDict = new Dictionary<PlayerRoomSkinRefObj, EPlayerRoomSkinState>();
        
        // 当前选中的皮肤
        private PlayerRoomSkinRefObj _m_selectedRoomSkinRefObj;

        public GGUIWndPlayerRoomSkin() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlayerRoomSkin.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerRoomSkin.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建选中皮肤背景图
            if (wnd.selectedRoomSkinBg != null)
                _m_wSelectedRoomSkinBg = new NPGGuiWndTexture(wnd.selectedRoomSkinBg);

            // 构建皮肤列表 Grid
            if (wnd.monoRoomSkinItemGrid != null)
            {
                _m_roomSkinItemGridWnd = new GGUIWndPlayerRoomSkinItemGrid(wnd.monoRoomSkinItemGrid);
                _m_roomSkinItemGridWnd.onSelectRoomSkin += _onSelectRoomSkin;
            }

            // 绑定按钮
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onBtnReturnClick);
        }
        
        protected override void _onDiscard()
        {
            // 销毁子窗口
            _m_wSelectedRoomSkinBg?.discard();
            _m_wSelectedRoomSkinBg = null;

            if (_m_roomSkinItemGridWnd != null)
            {
                _m_roomSkinItemGridWnd.onSelectRoomSkin -= _onSelectRoomSkin;
                _m_roomSkinItemGridWnd.discard();
                _m_roomSkinItemGridWnd = null;                
            }

            // 解绑按钮
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onBtnReturnClick);
            }

            // 清空数据
            _m_allRoomSkinRefObjList.Clear();
            _m_roomSkinInfoDict.Clear();
            _m_roomSkinStateDict.Clear();
            _m_selectedRoomSkinRefObj = null;
        }

        
        protected override void _onShowWnd()
        {
            // 注册皮肤变化消息
            WinMsg.RegisterMsg(WinMsgType.ON_ROOM_SKIN_INFO_CHG, _onRoomSkinInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_ROOM_SKIN_ADD, _onAddRoomSkin);
            WinMsg.RegisterMsg(WinMsgType.ON_ROOM_SKIN_INVALID, _onRoomSkinInvalid);
            WinMsg.RegisterMsg(WinMsgType.ON_ROOM_SKIN_DEL, _onDelRoomSkin);
            WinMsg.RegisterMsgAct(WinMsgType.ON_IN_USE_ROOM_SKIN_CHG, _onCurInUseSkinChg);

            // 显示子窗口
            _m_wSelectedRoomSkinBg?.showWnd();
            _m_roomSkinItemGridWnd?.showWnd();

            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 解除消息注册
            WinMsg.UnregisterMsg(WinMsgType.ON_ROOM_SKIN_INFO_CHG, _onRoomSkinInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_ROOM_SKIN_ADD, _onAddRoomSkin);
            WinMsg.UnregisterMsg(WinMsgType.ON_ROOM_SKIN_INVALID, _onRoomSkinInvalid);
            WinMsg.UnregisterMsg(WinMsgType.ON_ROOM_SKIN_DEL, _onDelRoomSkin);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_IN_USE_ROOM_SKIN_CHG, _onCurInUseSkinChg);

            // 隐藏子窗口
            _m_wSelectedRoomSkinBg?.hideWnd();
            _m_roomSkinItemGridWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSelectedRoomSkinBg?.discardTexture();
            _m_roomSkinItemGridWnd?.resetWnd();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _refreshRoomSkinGrid();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void refreshData()
        {
            _m_allRoomSkinRefObjList.Clear();
            _m_allRoomSkinRefObjList.AddRange(GRefdataCoreMgr.instance.playerRoomSkinRefCore.refList);

            _refreshData();
        }

        private void _refreshData()
        {
            _m_roomSkinInfoDict.Clear();
            if (NPPlayer.instance.roomSkinComp.roomSkinList != null)
            {
                foreach (var skinInfo in NPPlayer.instance.roomSkinComp.roomSkinList)
                {
                    if(skinInfo == null || skinInfo.roomSkinRefObj == null)
                        continue;

                    _m_roomSkinInfoDict[skinInfo.roomSkinRefObj] = skinInfo;
                }
            }

            _refreshSkinStateData();
            _refreshSelectedRoomSkinData();
        }
        
        /// <summary>
        /// 刷新皮肤状态数据
        /// </summary>
        private void _refreshSkinStateData()
        {
            _m_roomSkinStateDict.Clear();
            foreach (var roomSkinRefObj in _m_allRoomSkinRefObjList)
            {
                if(roomSkinRefObj == null)
                    return;

                EPlayerRoomSkinState state = NPPlayer.instance.roomSkinComp.getSkinState(roomSkinRefObj, _m_roomSkinInfoDict.GetValueOrDefault(roomSkinRefObj));
                _m_roomSkinStateDict[roomSkinRefObj] = state;
            }
        }
        
        private void _refreshSelectedRoomSkinData()
        {
            _m_selectedRoomSkinRefObj = NPPlayer.instance.roomSkinComp.currentRoomSkinRefObj;
            if (_m_selectedRoomSkinRefObj == null)// 若没有找到当前选中皮肤, 默认选中配表第一个
                _m_selectedRoomSkinRefObj = _m_allRoomSkinRefObjList.SafeGet(0);
        }

        /// <summary>
        /// 刷新选中皮肤显示
        /// </summary>
        private void _refreshSelectedRoomSkinShow()
        {
            if (wnd == null || _m_selectedRoomSkinRefObj == null || !isShow)
                return;

            // 刷新背景图
            if (_m_wSelectedRoomSkinBg != null)
            {
                _m_wSelectedRoomSkinBg.showWnd();
                _m_wSelectedRoomSkinBg.setTexture(_m_selectedRoomSkinRefObj.bg_img);
            }

            // 刷新来源描述
            ALUGUICommon.setLabelTxt(wnd.txtSelectedRoomSkinSource, GCommon.getItemSource(ENPItemType.ROOM_SKIN, _m_selectedRoomSkinRefObj.id));
            
            // 刷新选中状态显示
            EPlayerRoomSkinState state = _m_roomSkinStateDict.GetValueOrDefault(_m_selectedRoomSkinRefObj);
            NPCommonEnumStatMutexShowInfo<EPlayerRoomSkinState>.setStat(wnd.selectedRoomSkinStateConfigList, state);
        }

        /// <summary>
        /// 刷新皮肤列表
        /// </summary>
        private void _refreshRoomSkinGrid()
        {
            if (_m_roomSkinItemGridWnd == null || !isShow)
                return;

            _m_roomSkinItemGridWnd.refreshWnd(_m_allRoomSkinRefObjList, _m_roomSkinInfoDict, _m_roomSkinStateDict);
            _m_roomSkinItemGridWnd.setSelectedRoomSkin(_m_selectedRoomSkinRefObj);
        }

        /// <summary>
        /// 选中皮肤回调
        /// </summary>
        private void _onSelectRoomSkin(PlayerRoomSkinRefObj _refObj)
        {
            if (_refObj == null)
                return;

            _m_selectedRoomSkinRefObj = _refObj;
            _refreshSelectedRoomSkinShow();
        }

        /// <summary>
        /// 当房间皮肤数据变化时
        /// </summary>
        private void _onRoomSkinInfoChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is PlayerRoomSkinInfo playerRoomSkinInfo) || playerRoomSkinInfo.roomSkinRefObj == null)
                return;

            _m_roomSkinInfoDict[playerRoomSkinInfo.roomSkinRefObj] = playerRoomSkinInfo;
            EPlayerRoomSkinState skinState = NPPlayer.instance.roomSkinComp.getSkinState(playerRoomSkinInfo.roomSkinRefObj, playerRoomSkinInfo);
            _m_roomSkinStateDict[playerRoomSkinInfo.roomSkinRefObj] = skinState;

            _m_roomSkinItemGridWnd?.forceRefreshSkinShow(playerRoomSkinInfo.roomSkinRefObj);
        }

        /// <summary>
        /// 当新增了皮肤时
        /// </summary>
        /// <param name="_objs"></param>
        private void _onAddRoomSkin(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is PlayerRoomSkinInfo playerRoomSkinInfo) || playerRoomSkinInfo.roomSkinRefObj == null)
                return;

            _m_roomSkinInfoDict[playerRoomSkinInfo.roomSkinRefObj] = playerRoomSkinInfo;
            EPlayerRoomSkinState skinState = NPPlayer.instance.roomSkinComp.getSkinState(playerRoomSkinInfo.roomSkinRefObj, playerRoomSkinInfo);
            _m_roomSkinStateDict[playerRoomSkinInfo.roomSkinRefObj] = skinState;

            _m_roomSkinItemGridWnd?.forceRefreshSkinShow(playerRoomSkinInfo.roomSkinRefObj);
        }

        /// <summary>
        /// 当前房间皮肤无效时
        /// </summary>
        /// <param name="_objs"></param>
        private void _onRoomSkinInvalid(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is PlayerRoomSkinInfo playerRoomSkinInfo) || playerRoomSkinInfo.roomSkinRefObj == null)
                return;

            _m_roomSkinInfoDict[playerRoomSkinInfo.roomSkinRefObj] = playerRoomSkinInfo;
            EPlayerRoomSkinState skinState = NPPlayer.instance.roomSkinComp.getSkinState(playerRoomSkinInfo.roomSkinRefObj, playerRoomSkinInfo);
            _m_roomSkinStateDict[playerRoomSkinInfo.roomSkinRefObj] = skinState;

            _m_roomSkinItemGridWnd?.forceRefreshSkinShow(playerRoomSkinInfo.roomSkinRefObj);
        }

        /// <summary>
        /// 当前删除房间皮肤时
        /// </summary>
        private void _onDelRoomSkin(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is PlayerRoomSkinInfo playerRoomSkinInfo) || playerRoomSkinInfo.roomSkinRefObj == null)
                return;

            _m_roomSkinInfoDict.Remove(playerRoomSkinInfo.roomSkinRefObj);
            EPlayerRoomSkinState skinState = NPPlayer.instance.roomSkinComp.getSkinState(playerRoomSkinInfo.roomSkinRefObj, null);
            _m_roomSkinStateDict[playerRoomSkinInfo.roomSkinRefObj] = skinState;

            _m_roomSkinItemGridWnd?.forceRefreshSkinShow(playerRoomSkinInfo.roomSkinRefObj);
        }

        /// <summary>
        /// 当前使用中的皮肤变化
        /// </summary>
        private void _onCurInUseSkinChg()
        {
            _refreshSkinStateData();//刷新皮肤状态数据
            _refreshSelectedRoomSkinData();//刷新选中皮肤数据
            if (_m_roomSkinItemGridWnd != null)
            {
                _m_roomSkinItemGridWnd.forceRefreshAllItem();
                _m_roomSkinItemGridWnd.setSelectedRoomSkin(_m_selectedRoomSkinRefObj);       
            }
        }
        
        /// <summary>
        /// 返回按钮点击
        /// </summary>
        private void _onBtnReturnClick(UnityEngine.GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_PLAYER_ROOM_SKIN);
        }
    }
}