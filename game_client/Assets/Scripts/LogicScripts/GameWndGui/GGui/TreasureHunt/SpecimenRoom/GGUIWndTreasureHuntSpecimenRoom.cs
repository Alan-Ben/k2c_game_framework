using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 标本间
    /// </summary>
    public class GGUIWndTreasureHuntSpecimenRoom : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntSpecimenRoom>
    {
        private long _m_lUIPathId;//ui路径id

        private List<_ITreasureHuntOreInfo> _m_lPendingOreList;//待处理矿石列表
        
        private GGUIWndTreasureHuntOreItemGrid _m_wOreItemGrid;

        public GGUIWndTreasureHuntSpecimenRoom(long _uiPathId) : base(EALUIWndLayer.ADDITION)
        {
            _m_lUIPathId = _uiPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoOreItemGrid != null)
                _m_wOreItemGrid = new GGUIWndTreasureHuntOreItemGrid(wnd.monoOreItemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnDeal, _onClickDealBtn);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDeal, _onClickDealBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
            }
            
            _m_lPendingOreList?.Clear();
            _m_lPendingOreList = null;
            
            _m_wOreItemGrid?.discard();
            _m_wOreItemGrid = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lPendingOreList?.Clear();

            _m_wOreItemGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lPendingOreList?.Clear();

            _m_wOreItemGrid?.resetWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            if (_m_lPendingOreList == null)
                _m_lPendingOreList = new List<_ITreasureHuntOreInfo>();
            _m_lPendingOreList.Clear();
            NPPlayer.instance.treasureHuntComponent.getPendingOreList(_m_lPendingOreList);
            // 对_m_lPendingOreList按照品质高低优先排序，同品质根据ID从小到大进行排序
            _m_lPendingOreList.Sort((a, b) =>
            {
                if (b == null || b.oreRefObj == null) return -1;
                if (a == null || a.oreRefObj == null) return 1;
                if (a == b || a.oreRefObj == b.oreRefObj) return 0;
                
                if (a.oreRefObj.quality != b.oreRefObj.quality)
                    return b.oreRefObj.quality.CompareTo(a.oreRefObj.quality); // 品质高的在前
                
                return a.oreId.CompareTo(b.oreId); // 同品质按ID从小到大
            });

            if (_m_lPendingOreList.Count <= 0)
            {
                _m_wOreItemGrid?.hideWnd();
                
                ALUGUICommon.setGameObjEnable(wnd.hasOrePendingShow, false);
                ALUGUICommon.setGameObjEnable(wnd.noOrePendingShow, true);
            }
            else
            {
                if (_m_wOreItemGrid != null)
                {
                    _m_wOreItemGrid.showWnd();
                    _m_wOreItemGrid.setData(_m_lPendingOreList);
                }
                
                ALUGUICommon.setGameObjEnable(wnd.hasOrePendingShow, true);
                ALUGUICommon.setGameObjEnable(wnd.noOrePendingShow, false);
            }
        }
        
        /// <summary>
        /// 处理按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickDealBtn(GameObject _go)
        {
            if(_m_lPendingOreList == null || _m_lPendingOreList.Count <= 0)
                return;
            
            // 发送处理矿石请求
            NPPlayer.instance.treasureHuntComponent.reqTreasureHuntTransOre((_isSucc, _msg) =>
            {
                if (!_isSucc || _msg == null)
                    return;

                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureHuntSpecimenConvert.instance, () =>
                {
                    GGUIWndTreasureHuntSpecimenConvert.instance.setData(_msg);
                    GGUIWndTreasureHuntSpecimenConvert.instance.showWnd();
                }, UINodeTagConst.C_TREASURE_HUNT_SPECIMEN_CONVERT);

                // 刷新当前窗口
                _refreshWnd();
            });
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_SPECIMEN_ROOM);
        }
    }
}