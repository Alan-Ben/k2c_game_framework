using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝主页面
    /// </summary>
    public class GGUIWndTreasureHuntMain : _ANPGGUIBasicResBarWnd<GGUIMonoTreasureHuntMain>
    {
        private static GGUIWndTreasureHuntMain _g_instance;
        public static GGUIWndTreasureHuntMain instance { get { return _g_instance ??= new GGUIWndTreasureHuntMain(); } }

        private GGUISubWndTreasureHuntStationLevel _m_wStationLevel;
        private GGUIWndTreasureHuntSpecimenRoom _m_wSpecimenRoom; // 标本间窗口
        private GGUIWndTreasureHuntSpecimenRoom _m_wMaterialsRoom;// 材料室 
        private NPGGuiWndTexture _m_wCurInAreaIcon; // 当前所在区域图标纹理组件
        
        private TreasureHuntStationInfo _m_stationInfo; // 太空舱信息

        public GGUIWndTreasureHuntMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool showResBarBySelf { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoStationLevel != null)
                _m_wStationLevel = new GGUISubWndTreasureHuntStationLevel(wnd.monoStationLevel);

            // 初始化区域图标纹理组件
            if (wnd.curInAreaIcon != null)
                _m_wCurInAreaIcon = new NPGGuiWndTexture(wnd.curInAreaIcon);

            _m_wSpecimenRoom = new GGUIWndTreasureHuntSpecimenRoom(6805);
            _m_wMaterialsRoom = new GGUIWndTreasureHuntSpecimenRoom(6815);
            
            // 绑定所有按钮事件
            ALUGUICommon.combineBtnClick(wnd.btnLab, _onClickLab);
            ALUGUICommon.combineBtnClick(wnd.btnSpecimenRoom, _onClickSpecimenRoom);
            ALUGUICommon.combineBtnClick(wnd.btnGainEnergy, _onClickGainEnergy);
            ALUGUICommon.combineBtnClick(wnd.btnPlay, _onClickPlay);
            ALUGUICommon.combineBtnClick(wnd.btnCatalog, _onClickCatalog);
            ALUGUICommon.combineBtnClick(wnd.btnMaterialsRoom, _onClickMaterialsRoom);
            ALUGUICommon.combineBtnClick(wnd.btnCollectReward, _onClickCollectReward);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onClickReturn);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                // 解绑所有按钮事件
                ALUGUICommon.uncombineBtnClick(wnd.btnLab, _onClickLab);
                ALUGUICommon.uncombineBtnClick(wnd.btnSpecimenRoom, _onClickSpecimenRoom);
                ALUGUICommon.uncombineBtnClick(wnd.btnGainEnergy, _onClickGainEnergy);
                ALUGUICommon.uncombineBtnClick(wnd.btnPlay, _onClickPlay);
                ALUGUICommon.uncombineBtnClick(wnd.btnCatalog, _onClickCatalog);
                ALUGUICommon.uncombineBtnClick(wnd.btnMaterialsRoom, _onClickMaterialsRoom);
                ALUGUICommon.uncombineBtnClick(wnd.btnCollectReward, _onClickCollectReward);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onClickReturn);
            }

            _m_wStationLevel?.discard();
            _m_wStationLevel = null;

            _m_wCurInAreaIcon?.discardTexture();
            _m_wCurInAreaIcon = null;

            _m_wSpecimenRoom?.discard();
            _m_wSpecimenRoom = null;
            _m_wMaterialsRoom?.discard();
            _m_wMaterialsRoom = null;
            
            _m_stationInfo = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            // 注册消息监听
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_STATION_CHG, _onStationInfoChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_GAIN_ENERGY, _onBtnGainEnergy);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_PLAY, _onBtnPlay);
        }

        protected override void _onHideWnd()
        {
            // 取消消息监听
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_STATION_CHG, _onStationInfoChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_GAIN_ENERGY, _onBtnGainEnergy);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TREASURE_HUNT_MAIN_PLAY, _onBtnPlay);
            
            _m_wStationLevel?.hideWnd();
            _m_wSpecimenRoom?.hideWnd();
            _m_wMaterialsRoom?.hideWnd();
            _m_wCurInAreaIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wStationLevel?.resetWnd();
            _m_wSpecimenRoom?.resetWnd();
            _m_wMaterialsRoom?.resetWnd();
            _m_wCurInAreaIcon?.discardTexture();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            // 获取太空舱信息
            _m_stationInfo = NPPlayer.instance.treasureHuntComponent?.stationInfo;

            // 刷新太空舱等级信息
            if (_m_wStationLevel != null && _m_stationInfo != null)
            {
                _m_wStationLevel.showWnd();
                _m_wStationLevel.setData(_m_stationInfo);
            }
            else
            {
                _m_wStationLevel?.hideWnd();
            }

            _refreshInAreaShow();
        }

        /// <summary>
        /// 刷新所在区域显示
        /// </summary>
        private void _refreshInAreaShow()
        {
            if (wnd == null)
                return;
            
            TreasureHuntUtil.refreshInArea();
            long areaId = NPPlayer.instance.treasureHuntComponent?.saver?.getAreaId() ?? 0;
            
            TreasureHuntAreaRefObj areaRefObj = GRefdataCoreMgr.instance.treasureHuntAreaRefCore?.getRef(areaId);
            
            // 刷新区域图标
            if (_m_wCurInAreaIcon != null)
            {
                if (areaRefObj != null && areaRefObj.thumbnail_image != null)
                {
                    _m_wCurInAreaIcon.showWnd();
                    _m_wCurInAreaIcon.setTexture(areaRefObj.thumbnail_image);
                }
                else
                {
                    _m_wCurInAreaIcon.hideWnd();
                }
            }
            
            // 刷新区域名称
            if (wnd.txtCurInAreaName != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCurInAreaName, TextTranslate.instance.getLanguage(areaRefObj?.name ?? ""));
            }
        }
        
        #region 消息监听

        /// <summary>
        /// 太空舱信息变化
        /// </summary>
        private void _onStationInfoChg(params object[] _objects)
        {
            _refreshWnd();
        }

        #endregion

        #region 按钮事件处理

        /// <summary>
        /// 点击实验室按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLab(GameObject _go)
        {
            GNodeTreasureHuntLab.addNode(null);
        }

        /// <summary>
        /// 点击标本间按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickSpecimenRoom(GameObject _go)
        {
            if (_m_wSpecimenRoom == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(_m_wSpecimenRoom, () =>
            {
                _m_wSpecimenRoom?.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_SPECIMEN_ROOM);
        }

        private void _onBtnGainEnergy()
        {
            if (wnd != null) 
                _onClickGainEnergy(wnd.btnGainEnergy);
        }

        /// <summary>
        /// 点击获取能源按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGainEnergy(GameObject _go)
        {
            if(GCommon.getItemCount(ENPItemType.LAZY_CD, GRefdataCoreMgr.instance.npGeneral.treasure_hunt_lazy_cd_id) <= 0)
                return;
            
            // 请求领取捕捉能量
            NPPlayer.instance.treasureHuntComponent?.reqTreasureHuntDrawCapturePower((_isSucc, _msg) =>
            {
            });
        }
        private void _onBtnPlay()
        {
            if (wnd != null) 
                _onClickPlay(wnd.btnPlay);
        }
        /// <summary>
        /// 点击开始游玩按钮
        /// </summary>
        private void _onClickPlay(GameObject _go)
        {
            GNodeTreasureHuntGameMain.addNode();
        }
        
        /// <summary>
        /// 点击图鉴按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCatalog(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTreasureHuntCatalogMain.instance, UINodeTagConst.C_TREASURE_HUNT_CATALOG_MAIN, 0);
        }

        /// <summary>
        /// 点击材料室按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickMaterialsRoom(GameObject _go)
        {
            if (_m_wMaterialsRoom == null)
                return;
            
            QueueMgr.instance.addNode_InGame_SingleWnd(_m_wMaterialsRoom, () =>
            {
                _m_wMaterialsRoom?.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_SPECIMEN_ROOM);
        }

        /// <summary>
        /// 点击收集奖励按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCollectReward(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeTreasureHuntCollectAchieve());
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickReturn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_MAIN);
        }

        #endregion
    }
}