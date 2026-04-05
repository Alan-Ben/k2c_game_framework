using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    // 单层的被合成界面
    public class GGUIWndBagItemBeCombined : _ANPGGUIBasicWnd<GGUIMonoBagItemBeCombined>
    {
        private static GGUIWndBagItemBeCombined _g_instance = new GGUIWndBagItemBeCombined();
        public static GGUIWndBagItemBeCombined instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBagItemBeCombined();

                return _g_instance;
            }
        }

        //被合成的物品id
        private long _m_beCombinedItemId = -1;
        //被合成的物品类型
        private ENPItemType _m_eBeCombinedItemType = ENPItemType.NONE;
        //合成兑换数据
        private ItemConvertRefObj _m_convertRef;
        //合成数量
        private long _m_lCombineCount = 0;
        //目标物品
        private NPGGUIWndCommonItem _m_wTargetItemWnd;
        //数量计数器
        private GGUIWndBagPopCounter _m_wCounter = null;
        //合成原料物品
        private NPGGUIWndCommonItem _m_wOriItem;
        //合成资源物品列表
        private NPGGUIWndCommonItemContainer _m_wResItemContainer;

        public GGUIWndBagItemBeCombined()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemBeCombined.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemBeCombined.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onDiscard()
        {
            _m_beCombinedItemId = -1;
            _m_eBeCombinedItemType = ENPItemType.NONE;
            _m_lCombineCount = 0;

            if (_m_wTargetItemWnd != null)
                _m_wTargetItemWnd.discard();
            _m_wTargetItemWnd = null;

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
        }

        protected override void _onReset()
        {
            if (_m_wTargetItemWnd != null)
                _m_wTargetItemWnd.resetWnd();
            if (_m_wCounter != null)
                _m_wCounter.resetWnd();
            if (_m_wOriItem != null)
                _m_wOriItem.resetWnd();
            if (_m_wResItemContainer != null)
                _m_wResItemContainer.resetWnd();
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTargetItem != null)
                _m_wTargetItemWnd = new NPGGUIWndCommonItem(wnd.monoTargetItem);

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

        /// <summary>
        /// 传入的是原材料id
        /// </summary>
        /// <param name="_oriItemId"></param>
        public void initOri(long _oriItemId)
        {
            if (null == wnd)
                return;

            ItemConvertRefObj convertRef = GRefdataCoreMgr.instance.itemConvertCore.getRef(_oriItemId);
            if (null == convertRef)
                return;

            _m_convertRef = convertRef;
            _m_eBeCombinedItemType = convertRef.target_item.itemType;
            _m_beCombinedItemId = convertRef.target_item.itemId;
            _m_lCombineCount = 1;
            // 初始化计数器
            if (_m_wCounter != null)
            {
                long totalCount = _getMaxCount();
                _m_wCounter.showWnd();
                _m_wCounter.init(totalCount);//可合成：{0}/{1}
            }
            _refreshWnd();
        }

        /// <summary>
        /// 传入的是目标材料id
        /// </summary>
        /// <param name="_beCombinedItemId"></param>
        public void init(ENPItemType _itemType, long _beCombinedItemId)
        {
            if (null == wnd)
                return;

            _m_eBeCombinedItemType = _itemType;
            _m_beCombinedItemId = _beCombinedItemId;
            _m_convertRef = null;
            ItemConvertRefObj temp = null;
            for(int i = 0;i < GRefdataCoreMgr.instance.itemConvertCore.refList.Count; i++)
            {
                temp = GRefdataCoreMgr.instance.itemConvertCore.refList[i];
                if (null == temp)
                    continue;

                if(temp.target_item.itemId == _beCombinedItemId && temp.target_item.itemType == _itemType)
                {
                    _m_convertRef = temp;
                    break;
                }
            }
            if (null == _m_convertRef)
                return;

            long totalCount = _getMaxCount();
            _m_lCombineCount = 1;
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
            if (_m_convertRef == null)
                return;

            if (_m_convertRef.target_item != null && _m_wTargetItemWnd != null)
            {
                long hasCount = GCommon.getItemCount(_m_convertRef.target_item);
                NPCommonCostItem item = new NPCommonCostItem(_m_convertRef.target_item, hasCount);
                _m_wTargetItemWnd.setItem(item);

                //目标物品数量 特殊显示key
                ALUGUICommon.setLabelTxt(wnd.txtTargetItemCount, TextTranslate.instance.getLanguage(wnd.txtTargetItemCountKey, hasCount.ToLargeString(item.getLargeStringType()),_m_lCombineCount));
                //目标物品纯数量
                ALUGUICommon.setLabelTxt(wnd.txtCombinedNum, _m_lCombineCount);
            }
        }

        /// <summary>
        /// 刷新使用按钮状态 
        /// </summary>
        private void _refreshUseBtnState()
        {
            if (wnd == null || _m_convertRef == null)
                return;

            //是否可以合成其他物品 并且 合成条件满足
            bool isEnable = GCommon.isItemCanCombine(_m_convertRef, 1);
            if (!isEnable)
                GGameCommonInfo.grayImage(wnd.grayImgList);
            else
                GGameCommonInfo.disgrayImage(wnd.grayImgList);

            ALUGUICommon.setGameObjEnable(wnd.oriNoEnoughShowGoList, !isEnable);
            ALUGUICommon.setGameObjEnable(wnd.oriEnoughShowGoList, isEnable);
        }

        /// <summary>
        /// 刷新原材料列表
        /// </summary>
        private void _refreshOriItemList()
        {
            if (wnd == null || _m_convertRef == null)
                return;

            //原材料消耗数量
            long oriCount = _m_convertRef.ori_item_num * _m_lCombineCount;

            NPCommonCostItem oriCostItem = new NPCommonCostItem(ENPItemType.BAG_ITEM, _m_convertRef.bag_item_id, oriCount);

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

            //资源列表
            if (_m_wResItemContainer != null)
            {
                _m_wResItemContainer.showItemList(resItemList);
            }
        }

        /// <summary>
        /// 获取合成数量上限
        /// </summary>
        /// <returns></returns>
        private long _getMaxCount()
        {
            if (_m_convertRef == null || _m_convertRef.target_item == null)
                return 0;

            long limitCount = GCommon.getItemCanCombineMaxCount(_m_convertRef.bag_item_id);
            long ownCount = GCommon.getItemCount(_m_convertRef.target_item);
            switch (_m_convertRef.target_item.itemType)
            {
                case ENPItemType.EQUIP:
                    //获取特殊藏品可获得数量上限
                    EquipRefObj equipRef = GRefdataCoreMgr.instance.equipRefCore.getRef(_m_convertRef.target_item.itemId);
                    if (equipRef != null && equipRef.num_limit > 0 && limitCount > equipRef.num_limit)
                    {
                        limitCount = equipRef.num_limit;
                        limitCount = limitCount - ownCount;
                    }
                    break;
            }
            return limitCount;
        }

        #region 点击事件

        // 响应关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
        }

        //点击合成按钮
        private void _onClickCombine(GameObject _go)
        {
            if (null == _m_convertRef)
                return;

            if (!GCommon.isItemCanCombine(_m_convertRef, _m_lCombineCount, true))
            {
                return;
            }

            NPPlayer.instance.bagComp.reqItemConvert(_m_convertRef.bag_item_id, _m_lCombineCount, () =>
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
            });
        }

        //点击获取途径按钮
        private void _onClickAccessBtn(GameObject _go)
        {
            GCommon.popItemAccessWays(ENPItemType.BAG_ITEM, _m_convertRef.bag_item_id);
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
