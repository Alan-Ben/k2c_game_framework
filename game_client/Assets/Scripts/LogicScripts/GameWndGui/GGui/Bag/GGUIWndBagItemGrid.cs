using ALPackage;
using ClientEnum;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;


namespace GOE
{
    // 背包物品容器
    public class GGUIWndBagItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoBagGridItem, GGUIMonoBagItemGrid, GGUIWndBagGridItem>
    {
        // 物品列表
        private List<BagItemShowInfo> _m_lItemList = new List<BagItemShowInfo>();

        //选中下标
        private int _m_iSelectedIdx;

        //选中对象展示的bar数据
        private _ABasicBagItemBar _m_selectedBar;

        //多个不同类型的bar对象
        private BagItemDetailBar _m_dbDetailBar;
        private BagItemUseBar _m_dbUseBar;
        private BagItemConvertBar _m_dbConvertBar;
        private BagItemUseAndConvertBar _m_dbUseConvertBar;
        private long _m_typeBarPathId;
        private Dictionary<NPEnum.ENPBagItemType, int> _m_typeItemCount;
        private List<BagItemTypeBarController> _m_typeBarList;
        private List<ENPBagItemType> _m_tabTypeList;
        private bool _m_isShowTypeBar = false;

        public GGUIWndBagItemGrid(GGUIMonoBagItemGrid _containerMono) : base(_containerMono)
        {
            _m_iSelectedIdx = -1;
            _m_selectedBar = null;

            _m_dbDetailBar = new BagItemDetailBar(this);
            _m_dbUseBar = new BagItemUseBar(this);
            _m_dbConvertBar = new BagItemConvertBar(this);
            _m_dbUseConvertBar = new BagItemUseAndConvertBar(this);
            _m_typeItemCount = new Dictionary<ENPBagItemType, int>();
            _m_typeBarList = new List<BagItemTypeBarController>();
            initWnd();
        }

        #region override方法
        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.BAG_ITEM_GRID_SCROLL_MOVE_TO, _onScrollMoveToItem);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_BAG_ITEM_GRID_ITEM, _onSimulateClickBagItem);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.BAG_ITEM_GRID_SCROLL_MOVE_TO, _onScrollMoveToItem);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_BAG_ITEM_GRID_ITEM, _onSimulateClickBagItem);
            //窗口记录红点已读跟着窗口隐藏一起设置为已读
            _setReadWndRedTip();
        }
        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_lItemList.Clear();
            _m_iSelectedIdx = -1;
            _m_selectedBar = null;

            if (null != _m_dbDetailBar)
                _m_dbDetailBar.discard();
            _m_dbDetailBar = null;

            if (null != _m_dbUseBar)
                _m_dbUseBar.discard();
            _m_dbUseBar = null;

            if (null != _m_dbConvertBar)
                _m_dbConvertBar.discard();
            _m_dbConvertBar = null;

            if (null != _m_dbUseConvertBar)
                _m_dbUseConvertBar.discard();
            _m_dbUseConvertBar = null;
            
            _m_typeItemCount?.Clear();
            _m_typeItemCount = null;

            _clearTypeBars();
            _m_typeBarList = null;
        }
        // 创建对象
        protected override GGUIWndBagGridItem _createItemWnd(GGUIMonoBagGridItem _itemMono)
        {
            // 创建对象
            GGUIWndBagGridItem gridItem = new GGUIWndBagGridItem(_itemMono);
            gridItem.setSelectDelegate(setSelectIndex);

            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndBagGridItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_lItemList.Count)
                return;

            // 刷新物品UI
            _itemMono.refreshItem(_m_lItemList[_itemIdx], _m_iSelectedIdx == _itemIdx);
        }
        #endregion

        /// <summary>
        /// 设置物品列表
        /// </summary>
        /// <param name="_tabTypeList"></param>
        /// <param name="_typeBarPathId"></param>
        public void setItemListWithBar(List<NPEnum.ENPBagItemType> _tabTypeList, long _typeBarPathId)
        {
            _m_tabTypeList = _tabTypeList;
            _addTypeBars(_m_tabTypeList, _typeBarPathId);
            _refreshAllItemByBar();
        }

        private void _refreshAllItemByBar()
        {
            _getItemListWithBar(_m_tabTypeList);
            //排序
            _m_isShowTypeBar = true;
            _m_lItemList.Sort(_getTypeSort);
            //设置无对象被选中
            _m_iSelectedIdx = -1;
            _onSelectItem(_m_iSelectedIdx);
            // 刷新物品
            _refreshItems();
            _refreshBarIndex();
            forceRefreshBar();
        }

        private void _refreshBarIndex()
        {
            int count = 0;
            BagItemTypeBarController typeBar = null;
            for (int i=0;i< _m_typeBarList.Count;i++)
            {
                typeBar = _m_typeBarList[i];
                if (null == typeBar)
                    continue;
                typeBar.setInsertIndex(count);
                count += _m_typeItemCount.ContainsKey(typeBar.type) ? _m_typeItemCount[typeBar.type] : 0;
            }
        }

        /// <summary>
        /// 设置物品列表
        /// </summary>
        /// <param name="_tabType"></param>
        public void setItemList(List<NPEnum.ENPBagItemType> _tabTypeList, EBagMainTabMonoType _tabType)
        {
            _m_tabTypeList = _tabTypeList;
            _clearTypeBars();
            getItemList(_tabTypeList, _tabType);
            _m_typeBarPathId = 0;
            //排序
            _m_isShowTypeBar = false;
            _m_lItemList.Sort(_getSort);
            //设置无对象被选中
            _m_iSelectedIdx = -1;
            _onSelectItem(_m_iSelectedIdx);
            // 刷新物品
            _refreshItems();
            forceRefreshBar();
        }

        private void _addTypeBars(List<NPEnum.ENPBagItemType> _tabTypeList, long _typeBarPathId)
        {
            if (_m_typeBarPathId != _typeBarPathId)//资源id不同时，清理原有bar
            {
                _clearTypeBars();
                _m_typeBarPathId = _typeBarPathId;
            }

            BagItemTypeBarController barWnd;
            ENPBagItemType _type;
            for (int i = 0; i < _tabTypeList.Count; i++)
            {
                _type = _tabTypeList[i];
                if (_m_typeBarList.Count > i)
                {
                    barWnd = _m_typeBarList[i];
                }
                else
                {
                    barWnd = new BagItemTypeBarController(_m_typeBarPathId, wnd.gridAreaUIObj, _onBarSelected);
                    _m_typeBarList.Add(barWnd);
                    addBar(barWnd);
                }

                barWnd.reset();//先重置，显示默认
                barWnd.setBarData(_type, _getTypeStr(_type));
            }
            
            
            //移除多余bar
            if (_m_typeBarList.Count > _tabTypeList.Count)
            {
                for (int i = _tabTypeList.Count; i < _m_typeBarList.Count; i++)
                {
                    removeBar(_m_typeBarList[i]);
                    _m_typeBarList[i].discard();
                }

                _m_typeBarList.RemoveRange(_tabTypeList.Count, _m_typeBarList.Count - _tabTypeList.Count);
            }
            
        }

        /// <summary>
        /// 获得类型文本
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        private string _getTypeStr(ENPBagItemType _type)
        {
            string key = _type.ToString();
            foreach (NPBagItemTypeKeyConfig config in wnd.typeKeyConfig)
            {
                if (config.type == _type)
                    key = config.typeKey;
            }

            return TextTranslate.instance.getLanguage(key);
        }

        /// <summary>
        /// bar选中装填变化
        /// </summary>
        /// <param name="_obj"></param>
        private void _onBarSelected(BagItemTypeBarController _obj)
        {
            _refreshAllItemByBar();
        }

        private void _clearTypeBars()
        {
            if (null != _m_typeBarList)
            {
                foreach (BagItemTypeBarController typeBar in _m_typeBarList)
                {
                    removeBar(typeBar);
                    typeBar.discard();
                }
                _m_typeBarList.Clear();
            }
        }

        /// <summary>
        /// 根据页签获取对应数据
        /// </summary>
        /// <param name="_tabType"></param>
        /// <summary>
        public void _getItemListWithBar(List<NPEnum.ENPBagItemType> _tabTypeList)
        {
            if (null == _m_lItemList)
                return;
            //取出数据库的数据
            List<BagItem> flagItemList = new List<BagItem>();
            NPPlayer.instance.bagComp.getItemList(flagItemList);
            //需要的数据
            List<BagItem> needItemList = new List<BagItem>();

            //筛选类型
            BagItemTypeBarController _typeBar = null;
            for (int i = 0; i < flagItemList.Count; ++i)
            {
                BagItem tmpBagItem = flagItemList[i];
                if (null == tmpBagItem)
                    continue;
                if (_tabTypeList.Contains(tmpBagItem.bagItemType))
                    needItemList.Add(tmpBagItem);
            }

            _m_lItemList.Clear();
            _m_typeItemCount.Clear();
            //转成展示数据
            for (int i = 0; i < needItemList.Count; i++)
            {
                BagItem item = needItemList[i];
                if (null == item || null == item.itemRefObj)
                    continue;

                //过滤隐藏是数据
                if (item.itemRefObj.is_hiding)
                    continue;

                _typeBar = _getTypeBar(item.bagItemType);
                if (null == _typeBar || !_typeBar.isShowItem) //是否显示，显示才把item加入显示列表
                    continue;
                
                
                BagItemShowInfo itemShowInfo = new BagItemShowInfo(item);
                _m_lItemList.Add(itemShowInfo);
                _refreshTypeItemCount();
            }

            //设置查看过新物品
            _setReadNewBagItem();
        }

        private void _refreshTypeItemCount()
        {
            if(null == _m_typeItemCount)
                _m_typeItemCount = new Dictionary<ENPBagItemType, int>();
            else 
                _m_typeItemCount.Clear();
            foreach (BagItemShowInfo showInfo in _m_lItemList)
            {
                if (!_m_typeItemCount.ContainsKey(showInfo.bagItem.bagItemType))
                {
                    _m_typeItemCount.Add(showInfo.bagItem.bagItemType, 0);
                }
                _m_typeItemCount[showInfo.bagItem.bagItemType]++;
            }
        }

        private BagItemTypeBarController _getTypeBar(ENPBagItemType _type)
        {
            foreach (BagItemTypeBarController typeBar in _m_typeBarList)
            {
                if (typeBar.type == _type)
                    return typeBar;
            }
            return null;
        }

        /// <summary>
        /// 根据页签获取对应数据
        /// </summary>
        /// <param name="_tabType"></param>
        /// <summary>
        public void getItemList(List<NPEnum.ENPBagItemType> _tabTypeList, EBagMainTabMonoType _tabType)
        {
            if (null == _m_lItemList)
                return;
            //取出数据库的数据
            List<BagItem> flagItemList = new List<BagItem>();
            NPPlayer.instance.bagComp.getItemList(flagItemList);
            //需要的数据
            List<BagItem> needItemList = new List<BagItem>();

            //筛选类型
            for (int i = 0; i < flagItemList.Count; ++i)
            {
                BagItem tmpBagItem = flagItemList[i];
                if (null == tmpBagItem)
                    continue;
                if (_tabTypeList.Contains(tmpBagItem.bagItemType) && 
                    ((_tabType == EBagMainTabMonoType.USE_ITEM && (tmpBagItem.itemRefObj.show_type == EBagClickShowView.USE || tmpBagItem.itemRefObj.show_type == EBagClickShowView.USE_AND_CONVERT)) || 
                     (_tabType != EBagMainTabMonoType.USE_ITEM && tmpBagItem.itemRefObj.show_type != EBagClickShowView.USE && tmpBagItem.itemRefObj.show_type != EBagClickShowView.USE_AND_CONVERT)))
                    needItemList.Add(tmpBagItem);
            }

            _m_lItemList.Clear();
            //转成展示数据
            for (int i = 0; i < needItemList.Count; i++)
            {
                BagItem item = needItemList[i];
                if (null == item || null == item.itemRefObj)
                    continue;

                //过滤隐藏是数据
                if (item.itemRefObj.is_hiding)
                    continue;

                BagItemShowInfo itemShowInfo = new BagItemShowInfo(item);
                _m_lItemList.Add(itemShowInfo);
            }

            //设置红点数据
            _setReadNewBagItem();
        }

        #region 红点相关

        //设置查看过新物品
        private void _setReadNewBagItem()
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] != null && (_m_lItemList[i].isNew || _m_lItemList[i].isNewAddItem))
                    NPPlayer.instance.bagComp.setItemIsViewed(_m_lItemList[i].bagItem.itemId);
            }
        }

        //设置窗口记录红点已读
        private void _setReadWndRedTip()
        {
            //设置窗口红点已读
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _setReadWndRedTip(_m_lItemList[i]);
            }
        }

        //设置窗口记录红点已读
        private void _setReadWndRedTip(BagItemShowInfo _info)
        {
            if (_info == null)
                return;

            AccountSettingMgr.instance.bagItemWndRedTipSaver.setReadRedTipByType(_info.bagItem.itemId, EBagItemRedTipType.ADD, EBagItemRedTipType.NEW, EBagItemRedTipType.COMBINE);
        }

        #endregion

        private int _getTypeSort(BagItemShowInfo _x, BagItemShowInfo _y)
        {
            int _xIndex=  _m_tabTypeList.IndexOf(_x.bagItem.bagItemType);
            int _yIndex=  _m_tabTypeList.IndexOf(_y.bagItem.bagItemType);
            if (_xIndex < _yIndex)
                return -1;
            if (_xIndex > _yIndex)
                return 1;            
            return _getSort(_x,_y);
        }

        /// <summary>
        /// 排序
        /// </summary>
        private int _getSort(BagItemShowInfo _x, BagItemShowInfo _y)
        {
            //优先排序id从小到大排序
            int res = _x.bagItem.sortId.CompareTo(_y.bagItem.sortId);
            if (res != 0)
                return res;

            //按照品质高到低排序
            res = -_x.bagItem.quality.CompareTo(_y.bagItem.quality);
            if (res != 0)
                return res;

            if (_x.isNew && !_y.isNew)
                return -1;
            if (!_x.isNew && _y.isNew)
                return 1;

            //判断是否可合成，根据合成状态排序
            if (_x.bagItem.itemRefObj.show_type == EBagClickShowView.CONVERT && _y.bagItem.itemRefObj.show_type == EBagClickShowView.CONVERT)
            {
                bool _xCanCombine = NPPlayer.instance.bagComp.judgeItemCanCombineMaxCount(_x.bagItem);
                bool _yCanCombine = NPPlayer.instance.bagComp.judgeItemCanCombineMaxCount(_y.bagItem);
                if(_xCanCombine && !_yCanCombine)
                    return -1;
                if(!_xCanCombine && _yCanCombine)
                    return 1;
            }

            return _x.bagItem.itemId.CompareTo(_y.bagItem.itemId);
        }

        /// <summary>
        /// 设置选中的对象序列号
        /// </summary>
        /// <param name="_index"></param>
        public void setSelectIndex(int _index)
        {
            int preIdx = _m_iSelectedIdx;
            if (_m_iSelectedIdx == _index)
                _m_iSelectedIdx = -1;
            else
                _m_iSelectedIdx = _index;

            //设置查看过新物品 需要在点击事件之前，因为点击事件会修改isNew
            if (_m_iSelectedIdx >= 0)
            {
                BagItemShowInfo item = _m_lItemList[_m_iSelectedIdx];
                if (null != item && (item.isNew || item.isNewAddItem))
                {
                    NPPlayer.instance.bagComp.setItemIsViewed(item.bagItem.itemId);
                }

            }
            //处理选中事件
            _onSelectItem(_m_iSelectedIdx);

            //刷新两个数据对象
            forceRefreshItem(preIdx);
            forceRefreshItem(_m_iSelectedIdx);

        }

        /// <summary>
        /// 物品增加
        /// </summary>
        /// <param name="_item"></param>
        public void addItem(BagItem _item)
        {
            // 判断物品是否存在，存在则刷新物品
            int addIdx = _getItemIdx(_item);
            if (addIdx >= 0)
            {
                BagItemShowInfo tmpInfo = _m_lItemList[addIdx];
                if (null != tmpInfo)
                    tmpInfo.setBagItem(_item);
                forceRefreshItem(addIdx);

                return;
            }
            
            //创建展示数据
            BagItemShowInfo info = new BagItemShowInfo(_item);

            //添加到队列，并重新排序
            _m_lItemList.Add(info);

            //获取之前的选中下标
            int oldSelectIdx = _m_iSelectedIdx;
            //如果之前点击的物品还存在 找到之前的物品
            BagItemShowInfo oldItem = null;

            //有选中对象则获取刷新信息
            if (_m_iSelectedIdx >= 0)
                oldItem = _m_lItemList[_m_iSelectedIdx];

            //排序，此操作可能引发选中序号变更
            if (_m_isShowTypeBar)
            {
                ///刷新一次类型数量
                _refreshTypeItemCount();
                _m_lItemList.Sort(_getTypeSort);
            }
            else
            {
                _m_lItemList.Sort(_getSort);
            }


            //如果存在旧的选中对象，进行数据校验
            if (null != oldItem)
            {
                //获取数据改变后的位置下标
                int idx = _m_lItemList.IndexOf(oldItem);
                //重新点击下标
                if (idx > 0 && idx != oldSelectIdx)
                    setSelectIndex(idx);
            }

            //设置查看过新物品
            _setReadNewBagItem();

            // 刷新物品
            _refreshItems();
            _refreshBarIndex();
            forceRefreshBar();
        }

        // 不移除物品 数量置为0
        public void removeItem(BagItem _removeItem)
        {
            // 修改物品状态
            int idx = _getItemIdx(_removeItem);
            if (idx >= 0 && idx < _m_lItemList.Count)
            {
                BagItemShowInfo info = _m_lItemList[idx];
                if (null != info)
                    info.setCount(0);
                // 刷新物品
                forceRefreshItem(idx);
            }

        }

        // 刷新物品
        public void updateItem(BagItem _item)
        {
            // 刷新物品
            int idx = _getItemIdx(_item);
            if (idx == -1)
                return;

            BagItemShowInfo info = _m_lItemList[idx];
            if (null != info)
                info.setBagItem(_item);

            //设置查看过新物品
            _setReadNewBagItem();
            forceRefreshItem(idx);
        }

        /// <summary>
        /// 获取物品下标
        /// </summary>
        private int _getItemIdx(BagItem _item)
        {
            int idx = -1;

            if (null == _item)
                return idx;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                BagItemShowInfo info = _m_lItemList[i];
                if (null == info)
                    continue;
                if (info.bagItem.itemId == _item.itemId)
                {
                    idx = i;
                    break;
                }
            }

            return idx;
        }

        // 刷新物品列表
        private void _refreshItems()
        {
            // 空物品提示
            if (null != wnd.noneItemsTips)
            {
                ALUGUICommon.setUIObjScale(wnd.noneItemsTips, _m_lItemList.Count <= 0 ? 1 : 0);
            }

            // 刷新grid
            setItemCount(_m_lItemList.Count);
            //这里调用一下动画
            showWnd();

            ALCommonActionMonoTask.addNextFrameLaterTask(() => 
            {
                //列表加载完成
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.BAG_LIST_LOAD_DONE);
            });
        }

        /// <summary>
        /// 选中指定序列数据时的处理函数
        /// </summary>
        protected void _onSelectItem(int _selectIdx)
        {
            BagItemShowInfo selectItem = null;
            if (_selectIdx >= 0 && _selectIdx < _m_lItemList.Count)
                selectItem = _m_lItemList[_selectIdx];

            //先删除旧的bar
            removeBar(_m_selectedBar);
            //隐藏对应bar
            if (null != _m_selectedBar)
                _m_selectedBar.hide();
            _m_selectedBar = null;

            //判断是否无选中，如无则清空bar数据
            if (null == selectItem)
                return;

            //获取对应的Bar数据
            _m_selectedBar = _getItemBar(selectItem.bagItem);
            _m_selectedBar.setInsertIndex(_selectIdx + 1);
            _m_selectedBar.setItem(selectItem.bagItem);
            //开始加载
            _m_selectedBar.init((null));
            //加入bar
            addBar(_m_selectedBar);

            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                // forceRefreshBar();
                _m_selectedBar.show();
            });

            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                _checkMoveToBarBottom(_selectIdx);
            });

            //设置窗口记录红点已读
            _setReadWndRedTip(selectItem);
        }

        /// <summary>
        /// 滚动到指定下标的item
        /// </summary>
        /// <param name="_itemIdx">item下标</param>
        /// <param name="_type">滚动类型：Center居中，TOP_OR_RIGHT顶部，BOTTOM_OR_LEFT底部，NearestEdge最近边缘</param>
        /// <param name="_needFade">是否需要平滑滚动动画</param>
        public void scrollToItem(int _itemIdx, EScrollToItemType _type = EScrollToItemType.Center, bool _needFade = true, float _smoothTime = 0.25f)
        {
            if (null == wnd || null == wnd.gridAreaUIObj || null == wnd.gridAreaMaskObj || null == wnd.itemTemplate || null == wnd.scrollRect || _m_lItemList == null)
                return;

            // 检查下标有效性
            if (_itemIdx < 0 || _itemIdx >= _m_lItemList.Count)
                return;

            // 获取每行item数量
            int perLineCount = wnd.perLineItemCount > 0 ? wnd.perLineItemCount : 1;

            // 获取滑动的范围
            float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
            if (contentRange <= 0)
                return;

            // 计算item所在行
            int row = _itemIdx / perLineCount;

            // 计算item顶部位置（从内容区域顶部开始计算）
            float itemTopY = wnd.paddingForSide.y + row * (wnd.itemTemplate.height + wnd.spaceSize.y);
            // 计算item底部位置
            float itemBottomY = itemTopY + wnd.itemTemplate.height;

            // 计算类型bar的高度（如果有TypeBar显示的话）
            float typeBarHeightBefore = 0;
            if (_m_isShowTypeBar && null != _m_tabTypeList && null != _m_typeBarList && null != _m_typeItemCount)
            {
                int accumulatedCount = 0;
                for (int i = 0; i < _m_tabTypeList.Count; i++)
                {
                    if (accumulatedCount <= _itemIdx)
                    {
                        typeBarHeightBefore += wnd.spaceSize.y;
                        typeBarHeightBefore += _m_typeBarList.Count > i ? _m_typeBarList[i].barHeight : 0;
                    }
                    accumulatedCount += _m_typeItemCount.ContainsKey(_m_tabTypeList[i]) ? _m_typeItemCount[_m_tabTypeList[i]] : 0;
                    if (accumulatedCount > _itemIdx)
                        break;
                }
            }

            // 加上TypeBar的高度
            itemTopY += typeBarHeightBefore;
            itemBottomY += typeBarHeightBefore;

            int selectItemRow = _m_iSelectedIdx / perLineCount;//选中item所处行数
            if (selectItemRow < row && _m_selectedBar != null)// 加上选中item的bar高度
            {
                itemTopY += _m_selectedBar.barHeight;
                itemBottomY += _m_selectedBar.barHeight;
            }
            
            // 当前滑动的距离
            float curScrollY = (1 - wnd.scrollRect.verticalNormalizedPosition) * contentRange;

            // 视口高度
            float viewportHeight = wnd.gridAreaMaskObj.rect.height;

            // 可视区域
            float visibleTop = curScrollY;
            float visibleBottom = curScrollY + viewportHeight;

            // 计算目标滚动位置
            float targetScrollY;
            switch (_type)
            {
                case EScrollToItemType.Center:
                    // 将item居中显示
                    targetScrollY = itemTopY + wnd.itemTemplate.height * 0.5f - viewportHeight * 0.5f;
                    break;
                case EScrollToItemType.TOP_OR_RIGHT:
                    // 将item显示在顶部
                    targetScrollY = itemTopY;
                    break;
                case EScrollToItemType.BOTTOM_OR_LEFT:
                    // 将item显示在底部
                    targetScrollY = itemBottomY - viewportHeight;
                    break;
                case EScrollToItemType.NearestEdge:
                default:
                    // 滚动到最近的边缘
                    if (itemBottomY < visibleBottom)
                    {
                        // item在可视区域下面
                        targetScrollY = itemTopY;
                    }
                    else if(itemTopY > visibleBottom)
                    {
                        // item在可视区域上面
                        targetScrollY = itemBottomY - viewportHeight;
                    }
                    else
                    {
                        float distToStart = Mathf.Abs(itemTopY - visibleTop);
                        float distToEnd = Mathf.Abs(itemBottomY - visibleBottom);
                        targetScrollY = distToStart < distToEnd ? itemTopY : itemBottomY - viewportHeight;
                    }
                    break;
            }

            // 限制在有效范围内
            targetScrollY = Mathf.Clamp(targetScrollY, 0, contentRange);

            // 计算滚动比例
            float rate = targetScrollY / contentRange;
            rate = 1 - rate;

            // 执行滚动
            if (_needFade)
            {
                new ScrollerSmoothMoveTaskVertical(this, rate, _smoothTime).deal();
            }
            else
            {
                // 直接设置滚动位置（无动画）
                wnd.scrollRect.verticalNormalizedPosition = rate;
            }
        }

        /// <summary>
        /// 检查是否需要滑动到展开bar的底部
        /// </summary>
        private void _checkMoveToBarBottom(int _itemIdx)
        {
            if (null == wnd || null == wnd.gridAreaUIObj || null == wnd.gridAreaMaskObj || null == wnd.itemTemplate)
                return;

            //获取滑动的范围
            float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;

            //根据选中下标获取item的底部值
            int selectItemRow = _itemIdx / wnd.perLineItemCount;
            float selectItemTop = wnd.paddingForSide.y + selectItemRow * (wnd.itemTemplate.height + wnd.spaceSize.y);
            float selectItemBottom = wnd.paddingForSide.y + (selectItemRow + 1) * (wnd.itemTemplate.height + wnd.spaceSize.y);

            //当前滑动的距离
            float curScrollY = (1 - wnd.scrollRect.verticalNormalizedPosition) * contentRange;

            //计算类型bar的高度
            float typeBarHeight = 0;
            if (_m_isShowTypeBar)
            {
                int max = 0;
                for (int i = 0; i < _m_tabTypeList.Count; i++)
                {
                    if (max <= _itemIdx)
                    {
                        typeBarHeight += wnd.spaceSize.y;
                        typeBarHeight += _m_typeBarList.Count > i ? _m_typeBarList[i].barHeight : 0;
                    }
                    max += _m_typeItemCount.ContainsKey(_m_tabTypeList[i]) ? _m_typeItemCount[_m_tabTypeList[i]] : 0;
                    if (max > _itemIdx)
                        break;
                }
            }
            selectItemTop += typeBarHeight;
            selectItemBottom += typeBarHeight;
            
            //如果item的底部值加上bar的高度大于可视区域的底部值 或 item的底部值加上bar的高度小于可视区域顶部值，则需要滑动
            if (selectItemBottom + _m_selectedBar.barHeight > curScrollY + wnd.gridAreaMaskObj.rect.height || 
                selectItemBottom + _m_selectedBar.barHeight < curScrollY)
            {
                //需要滑动的距离
                float needScrollY = selectItemBottom + _m_selectedBar.barHeight - wnd.gridAreaMaskObj.rect.height;
                float rate = needScrollY / contentRange;
                moveToVerticalRate(rate);
            }
        }

        /// <summary>
        /// 获取对应数据的选中bar数据
        /// </summary> 
        /// <param name="_bagItem"></param>
        /// <returns></returns>
        protected _ABasicBagItemBar _getItemBar(BagItem _bagItem)
        {
            if (null == _bagItem)
                return null;

            if (_bagItem.itemRefObj.show_type == EBagClickShowView.USE)
                return _m_dbUseBar;
            if (_bagItem.itemRefObj.show_type == EBagClickShowView.CONVERT)
                return _m_dbConvertBar;
            if (_bagItem.itemRefObj.show_type == EBagClickShowView.USE_AND_CONVERT)
                return _m_dbUseConvertBar;
            else
                return _m_dbDetailBar;
        }

        /// <summary>
        /// 滚动到指定类型的物品
        /// 参数：_objs[0] 为 EGGUIMonoBagItemGridTargetItemType 枚举类型或其字符串形式
        /// 参数：_objs[1] 为附加参数字符串，格式根据类型不同而不同
        /// </summary>
        private void _onScrollMoveToItem(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null)
                return;

            // 解析目标类型枚举
            EGGUIMonoBagItemGridTargetItemType _targetItemType;
            if (_objs[0] is EGGUIMonoBagItemGridTargetItemType)
                _targetItemType = (EGGUIMonoBagItemGridTargetItemType)_objs[0];
            else if (_objs[0] is string)
                ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemGridTargetItemType), (string)_objs[0], out _targetItemType);
            else
                return;

            // 解析附加参数
            string paramsStr = _objs.Length >= 2 && _objs[1] is string ? (string)_objs[1] : string.Empty;
            string[] paramsArr = paramsStr?.Split(':');

            switch (_targetItemType)
            {
                case EGGUIMonoBagItemGridTargetItemType.ITEM_ID:
                {
                    // 参数格式: itemIdList#scrollType#needFade#smoothTime
                    // itemIdList: 物品ID列表，多个ID用逗号分隔（必须），优先匹配第一个找到的
                    // smoothTime: 平滑滚动时间（可选，默认 0.25f）
                    // scrollType: EScrollToItemType 枚举（可选，默认 Center）
                    if (paramsArr == null || paramsArr.Length < 1)
                        break;

                    // 解析物品ID列表
                    string[] itemIdStrArr = paramsArr[0]?.Split(',');
                    if (itemIdStrArr == null || itemIdStrArr.Length < 1)
                        break;

                    // 查找物品下标，优先匹配列表中第一个找到的物品
                    int itemIdx = -1;
                    long foundItemId = 0;
                    for (int i = 0; i < itemIdStrArr.Length; i++)
                    {
                        if (!long.TryParse(itemIdStrArr[i], out long itemId))
                            continue;

                        int idx = _getItemIdxById(itemId);
                        if (idx >= 0)
                        {
                            itemIdx = idx;
                            foundItemId = itemId;
                            break;
                        }
                    }

                    if (itemIdx < 0)
                    {
                        Debug.LogError_EditorOnly($"[GGUIWndBagItemGrid _onScrollMoveToItem] 未找到物品ID列表中的任何物品：{paramsArr[0]}");
                        break;
                    }
                    
                    // 解析平滑滚动时间
                    float smoothTime = 0f;
                    if (paramsArr.Length >= 2 && !string.IsNullOrEmpty(paramsArr[1]))
                    {
                        ALCommon.TryParseFloat(paramsArr[1], out smoothTime);
                    }
                    
                    // 解析滚动类型
                    EScrollToItemType scrollType = EScrollToItemType.Center;
                    if (paramsArr.Length >= 3 && !string.IsNullOrEmpty(paramsArr[2]))
                    {
                        if (ALCommon.TryEnumParse(typeof(EScrollToItemType), paramsArr[2], out EScrollToItemType parsedScrollType))
                        {
                            scrollType = parsedScrollType;
                        }
                    }

                    // 使用父类接口方法执行滚动
                    scrollToItem(itemIdx, scrollType, smoothTime > 0f, smoothTime);
                    break;
                }
                case EGGUIMonoBagItemGridTargetItemType.IMPROVE_TARGET_TYPE_ITEM:
                {
                    // 参数格式: EImproveTargetType:smoothTime:scrollType
                    // EImproveTargetType: 使用道具可提升的目标类型，优先匹配第一个找到的
                    // smoothTime: 平滑滚动时间（可选，默认 0.25f）
                    // scrollType: EScrollToItemType 枚举（可选，默认 Center）
                    if (paramsArr == null || paramsArr.Length < 1)
                        break;

                    // 解析提升目标类型
                    EImproveTargetType improveTargetType = EImproveTargetType.NONE;
                    if (ALCommon.TryEnumParse(typeof(EImproveTargetType), paramsArr[0], out EImproveTargetType parsedImproveTargetType))
                        improveTargetType = parsedImproveTargetType;

                    // 查找物品下标，优先匹配列表中第一个找到的物品
                    int itemIdx = -1;
                    GRefdataCoreMgr.instance.bagItemUseCore.dealAllRef(_ref =>
                    {
                        if (improveTargetType != EImproveTargetType.NONE && 
                            _ref != null && 
                            _ref.improve_target_type_list != null &&
                            _ref.improve_target_type_list.Count > 0 &&
                            _ref.improve_target_type_list.Contains(improveTargetType) &&
                            itemIdx == -1)
                        {
                            itemIdx = _getItemIdxById(_ref.id);
                        }
                    });
                    if (itemIdx < 0)
                    {
                        Debug.LogError_EditorOnly($"[GGUIWndBagItemGrid _onScrollMoveToItem] 未找到目标提升类型的任何物品：{paramsArr[0]}");
                        break;
                    }

                    // 解析平滑滚动时间
                    float smoothTime = 0f;
                    if (paramsArr.Length >= 2 && !string.IsNullOrEmpty(paramsArr[1]))
                    {
                        ALCommon.TryParseFloat(paramsArr[1], out smoothTime);
                    }

                    // 解析滚动类型
                    EScrollToItemType scrollType = EScrollToItemType.Center;
                    if (paramsArr.Length >= 3 && !string.IsNullOrEmpty(paramsArr[2]))
                    {
                        if (ALCommon.TryEnumParse(typeof(EScrollToItemType), paramsArr[2],
                                out EScrollToItemType parsedScrollType))
                        {
                            scrollType = parsedScrollType;
                        }
                    }

                    // 使用父类接口方法执行滚动
                    scrollToItem(itemIdx, scrollType, smoothTime > 0f, smoothTime);
                    break;
                }
                default:
                    Debug.LogError_EditorOnly($"[GGUIWndBagItemGrid _onScrollMoveToItem] 未实现滚动目标类型：{_targetItemType}");
                    break;
            }
        }

        /// <summary>
        /// 根据物品ID获取物品下标
        /// </summary>
        /// <param name="_itemId">物品ID</param>
        /// <returns>物品下标，未找到返回-1</returns>
        private int _getItemIdxById(long _itemId)
        {
            if (_m_lItemList == null)
                return -1;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                BagItemShowInfo info = _m_lItemList[i];
                if (info != null && info.bagItem != null && info.bagItem.itemId == _itemId)
                {
                    return i;
                }
            }

            return -1;
        }

        #region 获取ItemWnd

        /// <summary>
        /// 根据物品ID获取ItemWnd
        /// </summary>
        /// <param name="_itemId">物品ID</param>
        /// <returns>对应的ItemWnd，未找到返回null</returns>
        private GGUIWndBagGridItem getItemWndByItemId(long _itemId)
        {
            if (_itemId <= 0)
                return null;

            int itemIdx = _getItemIdxById(_itemId);
            if (itemIdx < 0)
                return null;

            GGUIWndBagGridItem targetItem = null;
            refreshAllItem((_itemWnd, _idx) =>
            {
                if (_idx == itemIdx && _itemWnd != null)
                {
                    targetItem = _itemWnd;
                }
            });

            return targetItem;
        }

        #endregion

        #region 获取RectTransform

        /// <summary>
        /// 通过指定的目标物品类型获取RectTransform
        /// </summary>
        /// <param name="_targetItemType">目标物品类型</param>
        /// <param name="_paramsStr">附加参数字符串（如物品ID）</param>
        /// <returns>对应的RectTransform，未找到返回null</returns>
        public RectTransform getRectTransformByTargetItemType(EGGUIMonoBagItemGridTargetItemType _targetItemType, string _paramsStr)
        {
            string[] paramsArr = _paramsStr?.Split(':');
            
            switch (_targetItemType)
            {
                case EGGUIMonoBagItemGridTargetItemType.ITEM_ID:
                    // 参数格式: itemIdList，多个ID用逗号分隔，优先匹配第一个找到的
                    if (paramsArr != null && paramsArr.Length > 0)
                    {
                        // 解析物品ID列表
                        string[] itemIdStrArr = paramsArr[0]?.Split(',');
                        if (itemIdStrArr == null || itemIdStrArr.Length < 1)
                            return null;

                        // 查找物品，优先匹配列表中第一个找到的物品
                        for (int i = 0; i < itemIdStrArr.Length; i++)
                        {
                            if (!long.TryParse(itemIdStrArr[i], out long itemId))
                                continue;

                            GGUIWndBagGridItem itemWnd = getItemWndByItemId(itemId);
                            if (itemWnd != null)
                            {
                                return itemWnd.rectTransform;
                            }
                        }
                    }
                    return null;
                case EGGUIMonoBagItemGridTargetItemType.IMPROVE_TARGET_TYPE_ITEM:
                    // 参数格式: EImproveTargetType
                    if (paramsArr != null && paramsArr.Length > 0)
                    {
                        // 解析物品ID列表
                        EImproveTargetType improveTargetType = EImproveTargetType.NONE;
                        if (ALCommon.TryEnumParse(typeof(EImproveTargetType), paramsArr[0], out EImproveTargetType parsedImproveTargetType))
                            improveTargetType = parsedImproveTargetType;

                        // 查找物品，优先匹配列表中第一个找到的物品
                        GGUIWndBagGridItem itemWnd = null;
                        GRefdataCoreMgr.instance.bagItemUseCore.dealAllRef(_ref =>
                        {
                            if (improveTargetType != EImproveTargetType.NONE && 
                                _ref != null &&
                                _ref.improve_target_type_list != null &&
                                _ref.improve_target_type_list.Count > 0 &&
                                _ref.improve_target_type_list.Contains(improveTargetType) &&
                                itemWnd == null)
                            {
                                itemWnd = getItemWndByItemId(_ref.id);
                            }
                        });
                        return itemWnd?.rectTransform;
                    }
                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取使用物品bar使用按钮的RectTransform
        /// </summary>
        /// <returns></returns>
        public RectTransform getUseBagPageUseSimpleBarUseBtnRectTransform()
        {
            if (_m_selectedBar != _m_dbUseBar)
            {
                Debug.LogError($"[GGUIWndBagItemGrid getUseBagPageUseSimpleBarUseBtnRectTransform] 当前选中bar不是使用物品bar");
                return null;
            }
            
            return _m_dbUseBar.getUseBtnRectTransform();
        }   
        
        #endregion

        #region 模拟点击

        /// <summary>
        /// 模拟点击背包物品（消息处理）
        /// 参数：_objs[0] 为 EGGUIMonoBagItemGridTargetItemType 枚举类型或其字符串形式
        /// 参数：_objs[1] 为附加参数字符串，格式根据类型不同而不同
        /// </summary>
        private void _onSimulateClickBagItem(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null)
                return;

            // 解析目标类型枚举
            EGGUIMonoBagItemGridTargetItemType targetItemType;
            if (_objs[0] is EGGUIMonoBagItemGridTargetItemType)
                targetItemType = (EGGUIMonoBagItemGridTargetItemType)_objs[0];
            else if (_objs[0] is string)
                ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemGridTargetItemType), (string)_objs[0], out targetItemType);
            else
                return;

            // 解析附加参数
            string paramsStr = _objs.Length >= 2 && _objs[1] is string ? (string)_objs[1] : string.Empty;

            // 执行模拟点击
            simulateClickByTargetItemType(targetItemType, paramsStr);
        }

        /// <summary>
        /// 通过指定的目标物品类型模拟点击
        /// </summary>
        /// <param name="_targetItemType">目标物品类型</param>
        /// <param name="_paramsStr">附加参数字符串</param>
        public void simulateClickByTargetItemType(EGGUIMonoBagItemGridTargetItemType _targetItemType, string _paramsStr)
        {
            string[] paramsArr = _paramsStr?.Split(':');

            switch (_targetItemType)
            {
                case EGGUIMonoBagItemGridTargetItemType.ITEM_ID:
                {
                    // 参数格式: itemIdList，多个ID用逗号分隔，优先匹配第一个找到的
                    if (paramsArr != null && paramsArr.Length > 0)
                    {
                        // 解析物品ID列表
                        string[] itemIdStrArr = paramsArr[0]?.Split(',');
                        if (itemIdStrArr == null || itemIdStrArr.Length < 1)
                            break;

                        // 查找物品下标，优先匹配列表中第一个找到的物品
                        int itemIdx = -1;
                        long foundItemId = 0;
                        for (int i = 0; i < itemIdStrArr.Length; i++)
                        {
                            if (!long.TryParse(itemIdStrArr[i], out long itemId))
                                continue;

                            int idx = _getItemIdxById(itemId);
                            if (idx >= 0)
                            {
                                itemIdx = idx;
                                foundItemId = itemId;
                                break;
                            }
                        }

                        if (itemIdx >= 0)
                        {
                            setSelectIndex(-1); // 先清除当前选中状态
                            setSelectIndex(itemIdx);
                        }
                        else
                        {
                            Debug.LogError_EditorOnly($"[GGUIWndBagItemGrid simulateClickByTargetItemType] 未找到物品ID列表中的任何物品：{paramsArr[0]}");
                        }
                    }
                    break;
                }
                case EGGUIMonoBagItemGridTargetItemType.IMPROVE_TARGET_TYPE_ITEM:
                {
                    // 解析提升目标类型
                    EImproveTargetType improveTargetType = EImproveTargetType.NONE;
                    if (ALCommon.TryEnumParse(typeof(EImproveTargetType), paramsArr[0], out EImproveTargetType parsedImproveTargetType))
                        improveTargetType = parsedImproveTargetType;

                    // 查找物品下标，优先匹配列表中第一个找到的物品
                    int itemIdx = -1;
                    GRefdataCoreMgr.instance.bagItemUseCore.dealAllRef(_ref =>
                    {
                        if (improveTargetType != EImproveTargetType.NONE &&
                            _ref != null &&
                            _ref.improve_target_type_list != null &&
                            _ref.improve_target_type_list.Count > 0 &&
                            _ref.improve_target_type_list.Contains(improveTargetType) &&
                            itemIdx == -1)
                        {
                            itemIdx = _getItemIdxById(_ref.id);
                        }
                    });
                    if (itemIdx >= 0)
                    {
                        setSelectIndex(-1); // 先清除当前选中状态
                        setSelectIndex(itemIdx);
                    }
                    else
                    {
                        Debug.LogError_EditorOnly($"[GGUIWndBagItemGrid simulateClickByTargetItemType] 未找到对应提升目标类型的物品：{paramsArr[0]}");
                    }
                    break;
                }
                default:
                    Debug.LogError_EditorOnly($"[GGUIWndBagItemGrid simulateClickByTargetItemType] 未实现目标类型：{_targetItemType}");
                    break;
            }
        }

        #endregion

        // 响应物品点击事件
        //private void _onItemClick(int _itemIdx)
        //{
        //    if (_itemIdx < 0 || _itemIdx >= _m_lItemList.Count)
        //        return;

        //    // 设置小红点
        //    NPBagRedTipMgr.instance.onItemVisited(_m_lItemList[_itemIdx]);

        //    // 标记该物品为已浏览
        //    _m_lItemList[_itemIdx].isDetailVisited = true;

        //    // 刷新红点
        //    NPGGUIWndBag.instance.refreshTabRedTip();

        //    // 当前物品
        //  //  _m_iCurSelectItem = _m_lItemList[_itemIdx];

        //    // 刷新详情
        //    _refreshDetail();
        //    // 刷新物品
        //    _refreshItems();
        //}
    }
}
