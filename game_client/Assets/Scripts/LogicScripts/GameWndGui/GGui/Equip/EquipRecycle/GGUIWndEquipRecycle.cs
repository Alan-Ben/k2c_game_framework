using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品分解界面
    /// </summary>
    public class GGUIWndEquipRecycle : _ANPGGUIBasicWnd<GGUIMonoEquipRecycle>
    {
        private static GGUIWndEquipRecycle _g_instance;
        public static GGUIWndEquipRecycle instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndEquipRecycle();
                return _g_instance;
            }
        }

        //藏品列表
        private GGUIWndEquipRecycleSelectGrid _m_wEquipGrid;
        //批量选择页签
        private GGUIWndEquipRecycleBatchSelect _m_wRecycleBatchSelect;
        //选中隐藏切换控件
        private NPGGUIWndCommonToggleEx _m_wToggle;
        //获得道具图标列表
        private List<NPGGuiWndTexture> _m_lGainItemIconList;
        //是否需要显示红点
        private bool _m_bNeedShowRedTip;

        public GGUIWndEquipRecycle() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEquipRecycle.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEquipRecycle.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_EQUIP_REMOVE, _onEquipRemove);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_EQUIP_RECYCLE_ITEM_INDEX, _onSimulateClickItem);

            //获取是否需要展示红点
            _ARedTipNode redTipNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_EQUIP_RECYCLE);
            _m_bNeedShowRedTip = redTipNode != null && redTipNode.needShow();

            //设置选择页签默认状态
            _m_wToggle?.showWnd();
            _m_wToggle?.setSelected(false, true, false);
            _m_wRecycleBatchSelect?.showWnd();
            _m_wRecycleBatchSelect?.initState(_m_bNeedShowRedTip ? EEquipRecycleBatchSelectTabType.BLUE_AND_BELOW : EEquipRecycleBatchSelectTabType.GREEN_AND_BELOW);

            //刷新界面
            _refreshWnd();

            //如果有红点，则设置默认选中
            if (_m_bNeedShowRedTip)
                _onClickSelectAllButton(_m_wToggle);

            //设置红点已读
            if (redTipNode != null && redTipNode.needShow())
            {
                AccountSettingMgr.instance.dailyTagSaver.setSaveToday(DailyTagConst.EQUIP_RECYCLE);
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_EQUIP_RECYCLE, 0);
            }
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_EQUIP_REMOVE, _onEquipRemove);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_EQUIP_RECYCLE_ITEM_INDEX, _onSimulateClickItem);
            _m_wToggle?.hideWnd();
            _m_wRecycleBatchSelect?.hideWnd();
            _m_wEquipGrid?.hideWnd();
            _m_bNeedShowRedTip = false;

            if (_m_lGainItemIconList != null)
            {
                foreach (NPGGuiWndTexture item in _m_lGainItemIconList)
                {
                    item?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_wToggle?.resetWnd();
            _m_wRecycleBatchSelect?.resetWnd();
            _m_wEquipGrid?.resetWnd();

            if (_m_lGainItemIconList != null)
            {
                foreach (NPGGuiWndTexture item in _m_lGainItemIconList)
                {
                    item?.discardTexture();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_wToggle?.discard();
            _m_wToggle = null;
            _m_wRecycleBatchSelect?.discard();
            _m_wRecycleBatchSelect = null;
            _m_wEquipGrid?.discard();
            _m_wEquipGrid = null;

            if (_m_lGainItemIconList != null)
            {
                foreach (NPGGuiWndTexture item in _m_lGainItemIconList)
                {
                    item?.discard();
                }
                _m_lGainItemIconList.Clear();
                _m_lGainItemIconList = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnRecycle, _onClickConfirm);//点击分解确认按钮
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭按钮
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoSelectAllToggle != null)
            {
                _m_wToggle = new NPGGUIWndCommonToggleEx(wnd.monoSelectAllToggle);
                _m_wToggle.clickDelegate += _onClickSelectAllButton;
            }

            if (wnd.monoBatchSelect != null)
            {
                _m_wRecycleBatchSelect = new GGUIWndEquipRecycleBatchSelect(wnd.monoBatchSelect);
                _m_wRecycleBatchSelect.onSortChanged += _onClickSelectTabType;
            }

            if (wnd.monoEquipGrid != null)
            {
                _m_wEquipGrid = new GGUIWndEquipRecycleSelectGrid(wnd.monoEquipGrid);
                _m_wEquipGrid.clickDelegate += _onClickEquipItem;
            }

            _m_lGainItemIconList = new List<NPGGuiWndTexture>();
            if (wnd.recycleGainItemPreviewList != null)
            {
                for (int i = 0; i < wnd.recycleGainItemPreviewList.Count; i++)
                {
                    if (wnd.recycleGainItemPreviewList[i] != null &&
                        wnd.recycleGainItemPreviewList[i].imgIcon != null)
                    {
                        NPGGuiWndTexture itemIcon = new NPGGuiWndTexture(wnd.recycleGainItemPreviewList[i].imgIcon);
                        _m_lGainItemIconList.Add(itemIcon);
                    }
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnRecycle, _onClickConfirm);//点击分解确认按钮
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭按钮
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshGrid();
            _refreshGainItem();
            _refreshRedTip();
        }

        //刷新藏品列表
        private void _refreshGrid()
        {
            if (wnd == null)
                return;

            List<EquipInfo> equipList = new List<EquipInfo>();
            NPPlayer.instance.equipComp.getEquipInfoList(equipList);

            //移除不能分解的藏品
            for (int i = equipList.Count - 1; i >= 0; i--)
            {
                if(equipList[i] == null || equipList[i].wearHeroId > 0 || equipList[i].isLock)
                    equipList.RemoveAt(i);
            }

            equipList.Sort(_sortList);

            if (_m_wEquipGrid != null)
            {
                _m_wEquipGrid.showWnd();
                _m_wEquipGrid.setInfo(equipList);
            }
        }

        //刷新可获得道具
        private void _refreshGainItem()
        {
            if (wnd == null || wnd.recycleGainItemPreviewList == null)
                return;

            if (_m_lGainItemIconList == null || _m_lGainItemIconList.Count != wnd.recycleGainItemPreviewList.Count)
                return;

            List<NPCommonCostItem> itemList = _getCanGainItemList();
            for (int i = 0; i < wnd.recycleGainItemPreviewList.Count; i++)
            {
                GGUIEquipRecycleGainItemPreview previewItem = wnd.recycleGainItemPreviewList[i];
                if(previewItem == null || previewItem.bagItemId <= 0)
                    continue;

                long itemCount = 0;
                for (int j = 0; j < itemList.Count; j++)
                {
                    if (itemList[j] != null && itemList[j].subId == previewItem.bagItemId)
                    {
                        itemCount = itemList[j].getCount();
                        break;
                    }
                }

                ALUGUICommon.setLabelTxt(previewItem.txtCount, itemCount);
                _m_lGainItemIconList[i]?.showWnd();
                _m_lGainItemIconList[i]?.setTexture(GCommon.getItemTexIcon(ENPItemType.BAG_ITEM, previewItem.bagItemId));
            }
        }

        //刷新红点
        private void _refreshRedTip()
        {
            if (wnd == null)
                return;

            List<long> dbIdList = _getSelectDbIdList();
            ALUGUICommon.setGameObjEnable(wnd.goRecycleRedTip, _m_bNeedShowRedTip && dbIdList != null && dbIdList.Count > 0);
        }

        //获取可获得的道具列表
        private List<NPCommonCostItem> _getCanGainItemList()
        {
            if (_m_wEquipGrid == null)
                return null;

            List<NPCommonCostItem> itemList = new List<NPCommonCostItem>();
            List<EquipInfo> equipList = new List<EquipInfo>();
            equipList.AddRange(_m_wEquipGrid.alreadySelectList);

            for (int i = 0; i < equipList.Count; i++)
            {
                if (equipList[i] != null && equipList[i].equipRef != null && equipList[i].equipRef.disassemble_get_item_list != null)
                {
                    //添加基础返还道具
                    itemList.AddRange(equipList[i].equipRef.disassemble_get_item_list);
                    //添加升级消耗道具
                    if (equipList[i].level > 1 && equipList[i].equipRef.cost_item != null)
                    {
                        NPCommonCostItem upgradeCostItem = new NPCommonCostItem(equipList[i].equipRef.cost_item);
                        upgradeCostItem.count = upgradeCostItem.count * (equipList[i].level - 1);
                        if(upgradeCostItem.count > 0)
                            itemList.Add(upgradeCostItem);
                    }
                }
            }

            itemList = GCommon.getCombineItemList(itemList);
            return itemList;
        }

        //获取已选中的数据id
        private List<long> _getSelectDbIdList()
        {
            if (_m_wEquipGrid == null)
                return null;

            List<long> dbIdList = new List<long>();
            for (int i = 0; i < _m_wEquipGrid.alreadySelectList.Count; i++)
            {
                if(_m_wEquipGrid.alreadySelectList[i] != null && _m_wEquipGrid.alreadySelectList[i].equipInfo != null)
                    dbIdList.Add(_m_wEquipGrid.alreadySelectList[i].equipInfo.dbId);
            }

            return dbIdList;
        }

        //排序：
        //品质从低到高排序
        //藏品觉醒等级从低到高排序
        //藏品等级从低到高排序
        //藏品额外等级从低到高排序
        //ID排序
        private int _sortList(EquipInfo _a, EquipInfo _b)
        {
            if (_a == null || _b == null || _a.equipRef == null || _b.equipRef == null)
                return 0;

            EQuality qualityA = GCommon.getItemQuality(ENPItemType.EQUIP, _a.equipId);
            EQuality qualityB = GCommon.getItemQuality(ENPItemType.EQUIP, _b.equipId);
            int qualityCompare = qualityA.CompareTo(qualityB);
            if (qualityCompare != 0)
                return qualityCompare;

            if (_a.level != _b.level)
                return _a.level.CompareTo(_b.level);

            if (qualityA == EQuality.RED && qualityB == EQuality.RED)
                return _a.equipRef.addition_quality_level.CompareTo(_b.equipRef.addition_quality_level);

            return _a.equipId.CompareTo(_b.equipId);
        }

        #region 消息事件

        //藏品移除事件
        private void _onEquipRemove()
        {
            _refreshWnd();
        }

        //模拟点击藏品回收列表 item
        private void _onSimulateClickItem(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long index = (long)_objects[0];
            _m_wEquipGrid?.setClickItemByIndex((int)index);
        }

        #endregion

        #region 点击事件

        //点击列表选择类型
        private void _onClickSelectTabType(EEquipRecycleBatchSelectTabType _type)
        {
            if (_m_wToggle != null && _m_wToggle.isOn)
            {
                _m_wEquipGrid?.setSelectAllByType(_type);
                _refreshGainItem();
            }
        }

        //点击藏品列表item
        private void _onClickEquipItem(GGUIWndEquipRecycleSelectGridItem _item)
        {
            _refreshGainItem();
            _refreshRedTip();
        }

        //点击全部选中开关
        private void _onClickSelectAllButton(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null)
                return;

            _m_wToggle?.setSelected(!_toggle.isOn, true);

            if (_m_wToggle != null && _m_wToggle.isOn)
            {
                EEquipRecycleBatchSelectTabType selectType = _m_wRecycleBatchSelect != null ? _m_wRecycleBatchSelect.curSelectType : EEquipRecycleBatchSelectTabType.NONE;
                _m_wEquipGrid?.setSelectAllByType(selectType);
                _refreshGainItem();
                _refreshRedTip();
            }
        }

        //点击确定分解
        private void _onClickConfirm(GameObject _go)
        {
            List<NPCommonCostItem> itemList = _getCanGainItemList();
            List<long> dbIdList = _getSelectDbIdList();
            if(dbIdList == null || dbIdList.Count <= 0)
                return;

            //设置不显示红点
            _m_bNeedShowRedTip = false;
            _refreshRedTip();

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndEquipRecycleConfirm.instance, () =>
            {
                GGUIWndEquipRecycleConfirm.instance.showWnd();
                GGUIWndEquipRecycleConfirm.instance.setInfo(dbIdList, itemList);
            }, UINodeTagConst.C_EQUIP_RECYCLE_CONFIRM);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EQUIP_RECYCLE);
        }

        #endregion
    }
}