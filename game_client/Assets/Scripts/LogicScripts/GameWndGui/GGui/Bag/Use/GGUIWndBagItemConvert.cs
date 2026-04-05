using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    // 背包弹窗:合成物品
    public class GGUIWndBagItemConvert : _ANPGGUIBasicWnd<GGUIMonoBagItemConvert>
    {
        private static GGUIWndBagItemConvert _g_instance = new GGUIWndBagItemConvert();
        public static GGUIWndBagItemConvert instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBagItemConvert();

                return _g_instance;
            }
        }

        //物品id
        private long _m_iItemId = -1;
        //合成兑换数据
        private ItemConvertRefObj _m_convertRef;
        //合成数量
        private long _m_lCombineCount = 1;
        //目标物品列表
        private NPGGUIWndCommonItemContainer _m_wTargetItemContainer;
        //数量计数器
        private GGUIWndBagPopCounter _m_wCounter = null;
        //合成原料物品
        private NPGGUIWndCommonItem _m_wOriItem;
        //合成资源物品列表
        private NPGGUIWndCommonItemContainer _m_wResItemContainer;

        public GGUIWndBagItemConvert()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemConvert.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemConvert.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onDiscard()
        {
            _m_iItemId = -1;
            _m_lCombineCount = 1;

            if (_m_wTargetItemContainer != null)
                _m_wTargetItemContainer.discard();
            _m_wTargetItemContainer = null;

            if (_m_wCounter != null)
                _m_wCounter.discard();
            _m_wCounter = null;

            if (_m_wOriItem != null)
                _m_wOriItem.discard();
            _m_wOriItem = null;

            if (_m_wResItemContainer != null)
                _m_wResItemContainer.discard();
            _m_wResItemContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCombine, _onClickCombine);
            ALUGUICommon.uncombineBtnClick(wnd.btnNotEnough, _onClickCombine);
            ALUGUICommon.uncombineBtnClick(wnd.toAccessBtn, _onClickAccessBtn);

        }

        protected override void _onHideWnd()
        {
            _m_lCombineCount = 1;
        }

        protected override void _onReset()
        {
            if (_m_wTargetItemContainer != null)
                _m_wTargetItemContainer.resetWnd();
            if (_m_wCounter != null)
                _m_wCounter.resetWnd();
            if (_m_wOriItem != null)
                _m_wOriItem.resetWnd();
            if (_m_wResItemContainer != null)
                _m_wResItemContainer.resetWnd();
        }

        protected override void _onShowWnd()
        {
            if (_m_wTargetItemContainer != null)
                _m_wTargetItemContainer.showWnd();
            if (_m_wCounter != null)
                _m_wCounter.showWnd();
            if (_m_wOriItem != null)
                _m_wOriItem.showWnd();
            if (_m_wResItemContainer != null)
                _m_wResItemContainer.showWnd();

        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTargetItemContainer != null)
                _m_wTargetItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoTargetItemContainer);

            if (wnd.useCounter != null)
            {
                _m_wCounter = new GGUIWndBagPopCounter(wnd.useCounter);
                _m_wCounter.regCounterChangedEvent(_onCounterChanged);
            }

            if (wnd.monoOriItem != null)
                _m_wOriItem = new NPGGUIWndCommonItem(wnd.monoOriItem);

            if (wnd.monoResItemList != null)
                _m_wResItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoResItemList);

            //绑定按钮
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnCombine, _onClickCombine);
            ALUGUICommon.combineBtnClick(wnd.btnNotEnough, _onClickCombine);
            ALUGUICommon.combineBtnClick(wnd.toAccessBtn, _onClickAccessBtn);
        }

        // 初始化
        public void init(long _itemId)
        {
            if (null == wnd)
                return;

            _m_iItemId = _itemId;
            _m_convertRef = GRefdataCoreMgr.instance.itemConvertCore.getRef(_m_iItemId);
            long totalCount = GCommon.getItemCanCombineMaxCount(_m_iItemId);

            // 初始化计数器
            if (_m_wCounter != null)
            {
                _m_wCounter.showWnd();
                _m_wCounter.init(totalCount);//可合成：{0}/{1}
            }
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            _refreshTargetItemList();
            _refreshUseBtnState();
            _refreshOriItemList();
        }

        //刷新目标物品列表
        private void _refreshTargetItemList()
        {
            if (null == wnd || _m_convertRef == null)
                return;

            List<NPCommonCostItem> itemList = new List<NPCommonCostItem>();
            if (_m_convertRef.target_item != null)
            {
                long count = _m_lCombineCount;
                if (!wnd.isShowCombineCount)
                    count = GCommon.getItemCount(_m_convertRef.target_item);
                NPCommonCostItem item = new NPCommonCostItem(_m_convertRef.target_item, count);
                itemList.Add(item);
            }

            if (_m_wTargetItemContainer != null)
            {
                _m_wTargetItemContainer.showItemList(itemList);
            }
        }

        /// <summary>
        /// 刷新使用按钮状态 
        /// </summary>
        private void _refreshUseBtnState()
        {
            if (wnd == null)
                return;

            //置灰
            bool isEnable = GCommon.isItemCanCombine(_m_iItemId, _m_lCombineCount);
            if (!isEnable)
                GGameCommonInfo.grayImage(wnd.grayImgList);
            else
                GGameCommonInfo.disgrayImage(wnd.grayImgList);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughHideList, isEnable);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughShowList, !isEnable);
        }

        /// <summary>
        /// 刷新原材料列表
        /// </summary>
        private void _refreshOriItemList()
        {
            if (wnd == null || _m_convertRef == null)
                return;

            //原材料
            NPCommonCostItem oriCostItem = new NPCommonCostItem(ENPItemType.BAG_ITEM, _m_convertRef.bag_item_id, _m_convertRef.ori_item_num * _m_lCombineCount);

            //资源
            List<NPCommonCostItem> resItemList = new List<NPCommonCostItem>();
            if (_m_convertRef.cost_item_list != null)
            {
                for (int i = 0; i < _m_convertRef.cost_item_list.Count; i++)
                {
                    if (_m_convertRef.cost_item_list[i] == null)
                        continue;

                    NPCommonCostItem item = new NPCommonCostItem(_m_convertRef.cost_item_list[i]);    
                    item.setCount(_m_convertRef.cost_item_list[i].count * _m_lCombineCount);
                    resItemList.Add(item);
                }
            }

            //原材料
            if (_m_wOriItem != null)
            {
                _m_wOriItem.showWnd();
                _m_wOriItem.setItem(oriCostItem);
            }
            //原材料数量  已拥有/合成道具所需消耗数量
            ALUGUICommon.setLabelTxt(wnd.txtOriItemCount, GCommon.getItemCount(oriCostItem.item).ToLargeString(oriCostItem.getLargeStringType())+"/"+ oriCostItem.count);

            //资源列表
            if (_m_wResItemContainer != null)
            {
                _m_wResItemContainer.showItemList(resItemList);
            }
        }

        #region 点击事件

        // 响应关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Bag_ConvertItemNode);
        }

        //点击合成按钮
        private void _onClickCombine(GameObject _go)
        {
            if (_m_iItemId <= 0)
                return;

            if (!GCommon.isItemCanCombine(_m_iItemId, _m_lCombineCount))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_combineNotEnough_none);//材料不足
                return;
            }

            NPPlayer.instance.bagComp.reqItemConvert(_m_iItemId, _m_lCombineCount, () =>
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Bag_ConvertItemNode);
            });
        }

        //点击获取途径按钮
        private void _onClickAccessBtn(GameObject _go)
        {
            GCommon.popItemAccessWays(ENPItemType.BAG_ITEM,_m_iItemId);
        }
        #endregion

        // 响应计数器更改事件
        private void _onCounterChanged(long _newCount)
        {
            _m_lCombineCount = _newCount;
            _refreshWnd();
        }
    }
}
