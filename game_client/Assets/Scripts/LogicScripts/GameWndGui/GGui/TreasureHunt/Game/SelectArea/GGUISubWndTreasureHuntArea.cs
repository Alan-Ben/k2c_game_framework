using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝区域子窗口
    /// </summary>
    public class GGUISubWndTreasureHuntArea : _ANPGGUIBasicSubWnd<GGUISubMonoTreasureHuntArea>
    {
        private TreasureHuntAreaRefObj _m_rAreaRefObj;
        private bool _m_bSelected;//是否被选中
        
        private NPGGUIWndCommonRedTip _m_wUnlockRedTip;
        
        public GGUISubWndTreasureHuntArea(GGUISubMonoTreasureHuntArea _wnd) : base(_wnd)
        {
            initWnd();
        }

        public event Action<GGUISubWndTreasureHuntArea> onAreaClick;//当区域被点击

        public long areaId { get { return wnd == null ? 0 : wnd.areaId; } }
        public TreasureHuntAreaRefObj areaRefObj { get { return _m_rAreaRefObj; } }
        public bool isSelected { get { return _m_bSelected; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            _m_rAreaRefObj = GRefdataCoreMgr.instance.treasureHuntAreaRefCore.getRef(areaId);

            if(wnd.unlockRedTip != null)
                _m_wUnlockRedTip = new NPGGUIWndCommonRedTip(wnd.unlockRedTip);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickArea);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickArea);
            }

            onAreaClick = null;
            _m_rAreaRefObj = null;
            
            _m_wUnlockRedTip?.discard();
            _m_wUnlockRedTip = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wUnlockRedTip?.showWnd();
            
            _refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshRedTip);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RED_TIP_CHANGE, _refreshRedTip);
            
            _m_wUnlockRedTip?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wUnlockRedTip?.resetWnd();
        }

        public void setSelected(bool _selected)
        {
            if(_m_bSelected == _selected)
                return;
            
            _m_bSelected = _selected;
            
            _refreshSelectedState();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_rAreaRefObj == null || !isShow)
                return;

            // 设置区域名称
            ALUGUICommon.setLabelTxt(wnd.txtAreaName, TextTranslate.instance.getLanguage(_m_rAreaRefObj.name));
            
            bool isUnlock = _m_rAreaRefObj.isUnlock();
            ALUGUICommon.setGameObjEnable(wnd.unlockShowGoList, isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.lockShowGoList, !isUnlock);
            
            // 刷新选中状态
            _refreshSelectedState();

            // 刷新收集进度
            _refreshCollectionProgress();
            
            // 刷新红点
            _refreshRedTip();
        }

        /// <summary>
        /// 刷新选中状态
        /// </summary>
        private void _refreshSelectedState()
        {
            if(wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.selectedShowGoList, _m_bSelected);
            ALUGUICommon.setGameObjEnable(wnd.unSelectedShowGoList, !_m_bSelected);

            // 同步进度文本颜色
            if (wnd.txtCollectionProgress != null)
            {
                ALUGUICommon.setUIObjColor(wnd.txtCollectionProgress, _m_bSelected ? wnd.selectedProgressColor : wnd.unselectedProgressColor);
            }
        }

        private void _onClickArea(GameObject _go)
        {
            onAreaClick?.Invoke(this);
        }

        /// <summary>
        /// 刷新区域收集进度显示
        /// 进度公式: (当前拥有的矿石 + 当前拥有的特殊矿石 + 当前拥有的奇物) / (区域所有矿石 + 区域所有特殊矿石 + 区域所有奇物) * 100%
        /// 说明:
        ///     1. "特殊矿石" 定义为该矿石已达到高级条件 (TreasureHuntGotOreInfo.isAdvancedOre == true)
        ///     2. 每个矿石最多同时记为 1 个普通矿石 + 1 个特殊矿石(若达成高级), 与需求分子/分母的表达一致
        ///     3. 分母中 "区域所有特殊矿石" 数量等同于区域矿石总数 (每个矿石都可以升级为高级)
        /// </summary>
        private void _refreshCollectionProgress()
        {
            if (wnd == null || wnd.txtCollectionProgress == null || _m_rAreaRefObj == null)
                return;

            // 分母: 普通矿石总数 + 特殊矿石总数 + 奇物总数
            int totalOreCount = _m_rAreaRefObj.ore_list?.Count ?? 0;
            int totalAdvancedOreCount = totalOreCount; // 每种矿石都存在一个对应的高级形态
            int totalTreasureCount = _m_rAreaRefObj.treasure_list?.Count ?? 0;
            int denominator = totalOreCount + totalAdvancedOreCount + totalTreasureCount;
            if (denominator <= 0)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCollectionProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, 100));
                return;
            }

            int ownedOreCount = 0;            // 已拥有的矿石(普通形态)
            int ownedAdvancedOreCount = 0;    // 已拥有的高级矿石
            int ownedTreasureCount = 0;       // 已拥有的奇物

            // 统计矿石
            if (_m_rAreaRefObj.ore_list != null)
            {
                foreach (var oreId in _m_rAreaRefObj.ore_list)
                {
                    TreasureHuntGotOreInfo oreInfo = NPPlayer.instance.treasureHuntComponent.getGotOreInfo(oreId);
                    if (oreInfo == null)
                        continue;
                    ownedOreCount++;
                    if (oreInfo.oreState is ETreasureHuntOreState.NOT_ACTIVATE_ADVANCED or ETreasureHuntOreState.ACTIVATED_ADVANCED)
                        ownedAdvancedOreCount++;
                }
            }

            // 统计奇物
            if (_m_rAreaRefObj.treasure_list != null)
            {
                foreach (var treasureId in _m_rAreaRefObj.treasure_list)
                {
                    TreasureHuntGotTreasureInfo gotTreasureInfo = NPPlayer.instance.treasureHuntComponent.getGotTreasureInfo(treasureId);
                    if (gotTreasureInfo != null)
                        ownedTreasureCount++;
                }
            }

            int numerator = ownedOreCount + ownedAdvancedOreCount + ownedTreasureCount;
            int percent = (int)((numerator / (float)denominator) * 100f);
            ALUGUICommon.setLabelTxt(wnd.txtCollectionProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, percent));
        }

        /// <summary>
        /// 刷新区域红点显示
        /// </summary>
        private void _refreshRedTip()
        {
            if(_m_wUnlockRedTip == null || _m_rAreaRefObj == null)
                return;

            _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(_m_rAreaRefObj.unlock_red_tip_id);
            _m_wUnlockRedTip.showRedTipNum((redTipNode?.needShow() ?? false) ? 1 : 0);
        }
    }
}