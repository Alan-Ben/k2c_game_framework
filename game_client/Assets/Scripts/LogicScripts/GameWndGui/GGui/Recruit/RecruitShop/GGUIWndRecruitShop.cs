using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 招聘商城
    /// </summary>
    public class GGUIWndRecruitShop : _ANPGGUIBasicWnd<GGUIMonoRecruitShop>
    {
        private RecruitShopInfo _m_iShopInfo;//招募商店ID
        private long _m_lUIResPathId;
        
        private CommonItemData _m_iRecruitCostItemData;//招募消耗物品数据
        
        private NPGGUIWndCommonItem _m_wRecruitCostItem;
        private GGUIWndRecruitShopCardItemContainer _m_wRecruitShopCardItemContainer;
        
        public GGUIWndRecruitShop(RecruitShopInfo _shopInfo, long _uiResPathId) : base(EALUIWndLayer.NORMAL)
        {
            _m_iShopInfo = _shopInfo;
            _m_lUIResPathId = _uiResPathId;

            if (_m_iShopInfo != null && _m_iShopInfo.recruitShopRefObj != null && _m_iShopInfo.recruitShopRefObj.recruit_cost_show_item != null)
                _m_iRecruitCostItemData = new CommonItemData(_m_iShopInfo.recruitShopRefObj.recruit_cost_show_item, 0);
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.monoRecruitCostItem != null)
                _m_wRecruitCostItem = new NPGGUIWndCommonItem(wnd.monoRecruitCostItem);

            if (wnd.monoRecruitShopCardItemContainer != null)
                _m_wRecruitShopCardItemContainer = new GGUIWndRecruitShopCardItemContainer(wnd.monoRecruitShopCardItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_wRecruitCostItem?.discard();
            _m_wRecruitCostItem = null;
            
            _m_wRecruitShopCardItemContainer?.discard();
            _m_wRecruitShopCardItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);
            
            _m_wRecruitCostItem?.hideWnd();
            
            _m_wRecruitShopCardItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRecruitCostItem?.resetWnd();
            
            _m_wRecruitShopCardItemContainer?.resetWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null || _m_iShopInfo == null || _m_iShopInfo.recruitShopRefObj == null)
                return;

            _refreshRecruitCostItem();

            if (_m_wRecruitShopCardItemContainer != null)
            {
                _m_wRecruitShopCardItemContainer.showWnd();
                _m_wRecruitShopCardItemContainer.setData(_m_iShopInfo, _m_iShopInfo.recruitShopRefObj.recruitRefObjList);
            }
        }
        
        private void _refreshRecruitCostItem()
        {
            if(wnd == null)
                return;
            
            if (_m_iRecruitCostItemData != null)
            {
                _m_iRecruitCostItemData.setCount(GCommon.getItemCount(_m_iRecruitCostItemData.getItemType(), _m_iRecruitCostItemData.subId));
                if (_m_wRecruitCostItem != null)
                {
                    _m_wRecruitCostItem.showWnd();
                    _m_wRecruitCostItem.setItem(_m_iRecruitCostItemData);
                }
            }
            else
            {
                _m_wRecruitCostItem?.hideWnd();
            }
        }

        /// <summary>
        /// 兑换成功消息
        /// </summary>
        private void _onRecruitExchangeSucc()
        {
            // 刷新窗口
            _refreshWnd();
        }
        
        /// <summary>
        /// 当背包物品变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onBagItemChg(params object[] _objs)
        {
            if(_m_iRecruitCostItemData == null || _m_iRecruitCostItemData.getItemType() != ENPItemType.BAG_ITEM)
                return;
            
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem))
                return;

            BagItem chgBagItem = (BagItem) _objs[0];
            if (chgBagItem != null && chgBagItem.itemId == _m_iRecruitCostItemData.subId)
            {
                _refreshRecruitCostItem();
            }
        }
        
        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RECRUIT_SHOP);
        }
    }
}