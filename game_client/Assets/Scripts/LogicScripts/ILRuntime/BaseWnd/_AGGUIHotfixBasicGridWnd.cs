using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _AGGUIHotfixBasicGridWnd<_T_ITEM_WND> : _ATALBasicUISubWnd<GGUIHotfixGridMono>, _IALBasicRefreshUIWndInterface where _T_ITEM_WND : _AGGUIHotfixBasicGridItemWnd
    {
        /** 是否需要强制刷新 */
        private bool _m_bNeedForceRefresh;
        /** 是否需要强制重新计算窗口size */
        private bool _m_bNeedForceRefreshSize;
        /** 窗口显示对象当前尺寸 */
        private float _m_fWidth;
        private float _m_fHeight;
        /** 所有内容对象的显示尺寸 */
        private float _m_fAllWidth;
        private float _m_fAllHeight;
        /** 滚动条对应的尺寸 */
        private float _m_fScrollBarWidth;
        private float _m_fScrollBarHeight;

        /** 总的对象数量 */
        private int _m_iTotalCount;
        /** 展示的对象总数量 */
        private int _m_iShowTotlaCount;
        /** 当行或当列的对象数量 */
        private int _m_iPerUnitItemCount;
        /** 行或列的数量 */
        private int _m_iShowLineCount;

        /** 中途Bar的队列 */
        private List<_AALUGUIGridBarController> _m_lBarList;
        /** 在计算显示数据的时候，当前显示的bar队列 */
        private List<_AALUGUIGridBarController> _m_lShowBarList;
        //临时检查窗口定位的处理队列
        private List<_AALUGUIGridBarController> _m_lTmpCheckSizeBarList;

        /** 当前显示范围的区域信息 */
        private Vector2 _m_vGridPreShowRect;
        private Vector2 _m_vGridCurShowRect;
        /** 当前用于实际显示的区域空间数据， 为了避免原始数据在调整尺寸的时候更改，实际显示使用本参数处理 */
        private Vector2 _m_vCalSpace;
        /** 起始到结束的对象位置索引信息 */
        private int _m_iStartItemIdx;
        private int _m_iFinalItemIdx;
        /** 根据当前窗口尺寸创建出来的需要显示的子对象队列 */
        private List<_T_ITEM_WND> _m_lShowItemList;
        /** 根据当前窗口尺寸创建出来的所有可能需要使用到的对象队列 */
        private List<_T_ITEM_WND> _m_lTotalItemList;

        private int _m_iTmpi;
        private int _m_iTmpi2;
        private int _m_iTmpIdx;
        private _T_ITEM_WND _m_wTmpItem;

        /** 显示操作序列号 */
        private int _m_iShowOpSerialize;

        protected _AGGUIHotfixBasicGridWnd(GGUIHotfixGridMono _wnd)
            : base(_wnd)
        {
            if (wnd == null)
                return;

            _m_bNeedForceRefresh = false;
            _m_fWidth = 0;
            _m_fHeight = 0;
            _m_fAllWidth = 0;
            _m_fAllHeight = 0;
            _m_fScrollBarWidth = 0;
            _m_fScrollBarHeight = 0;

            _m_vCalSpace = _wnd.spaceSize;

            _m_iTotalCount = 0;
            _m_iPerUnitItemCount = 0;
            _m_iStartItemIdx = 0;
            _m_iFinalItemIdx = 0;
            _m_lShowItemList = new List<_T_ITEM_WND>();
            _m_lTotalItemList = new List<_T_ITEM_WND>();

            _m_iShowOpSerialize = 1;

            _m_lBarList = new List<_AALUGUIGridBarController>();
            _m_lShowBarList = new List<_AALUGUIGridBarController>();
            _m_lTmpCheckSizeBarList = new List<_AALUGUIGridBarController>();
        }

        public int showOpSerialize { get { return _m_iShowOpSerialize; } }
        public float allWidth { get { return _m_fAllWidth; } }
        public float allHeight { get { return _m_fAllHeight; } }
        public float scrollBarWidth { get { return _m_fScrollBarWidth; } }
        public float scrollBarHeight { get { return _m_fScrollBarHeight; } }
        public int totalCount { get { return _m_iTotalCount; } }

        /******************
         * 显示本窗口
         **/
        public override void showWnd()
        {
            //刷新窗口尺寸
            refreshGridSize();

            base.showWnd();
        }
        public override void hideWnd()
        {
            _m_iShowOpSerialize++;

            base.hideWnd();
        }

        /// <summary>
        /// 初始化操作，初始化行为默认开启刷新尺寸任务
        /// </summary>
        protected override void _initWnd()
        {
            //刷新窗口尺寸
            refreshGridSize();

            base._initWnd();
        }

        /// <summary>
        /// 刷新窗口尺寸，重新计算显示数量
        /// </summary>
        public void refreshGridSize()
        {
            _m_iShowOpSerialize++;

            //设置朝向
            if (null != wnd && null != wnd.scrollRect)
            {
                if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    wnd.scrollRect.horizontal = true;
                    wnd.scrollRect.vertical = false;
                }
                else
                {
                    wnd.scrollRect.vertical = true;
                    wnd.scrollRect.horizontal = false;
                }
            }

            //开启刷新任务
            ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIRefreshMonoTask(this));
        }

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        public override void resetWnd()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGridSub][{this.GetType().Name}] resetWnd.");
            }
            hideWnd();

            _m_fWidth = 0;
            _m_fHeight = 0;

            //清除所有子窗口对象并释放
            clearAll();
            _m_iTotalCount = 0;
            _m_iPerUnitItemCount = 0;
            _m_iStartItemIdx = 0;
            _m_iFinalItemIdx = 0;

            //逐个bar释放
            for (int i = 0; i < _m_lBarList.Count; i++)
            {
                _m_lBarList[i].discard();
            }
            _m_lBarList.Clear();

            //调用reset事件函数
            _onReset();
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public override void discard()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIGridSub][{this.GetType().Name}] discard.");
            }
            //判断是否加载完成，是则隐藏本窗口
            hideWnd();

            _m_fWidth = 0;
            _m_fHeight = 0;

            //清除所有子窗口对象并释放
            clearAll();
            _m_iTotalCount = 0;
            _m_iPerUnitItemCount = 0;
            _m_iStartItemIdx = 0;
            _m_iFinalItemIdx = 0;

            //调用事件函数
            _onDiscard();

            //子窗口对象不释放资源
            //if (null != _m_monoWnd)
            //    ALUnityCommon.releaseGameObj(_m_monoWnd);
            _m_monoWnd = null;
        }

        /************
         * 获取对应对象的位置
         **/
        public Vector2 getItemPos(int _idx)
        {
            Vector2 pos;
            //设置显示对象位置
            if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
            {
                //横向位置设置，注意加上预留位移
                pos.x = ((_idx / _m_iPerUnitItemCount) * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x + wnd.paddingForSide.x;
                pos.y = -(((_idx % _m_iPerUnitItemCount) * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y);
            }
            else
            {
                //纵向位置设置，注意加上预留位移
                pos.x = ((_idx % _m_iPerUnitItemCount) * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x;
                pos.y = -(((_idx / _m_iPerUnitItemCount) * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y + wnd.paddingForSide.x);
            }

            return pos;
        }

        /**************
         * 设置对象数量
         **/
        public void setItemCount(int _count)
        {
            _m_iTotalCount = _count;

            //设置强制刷新
            _m_bNeedForceRefresh = true;
        }

        /// <summary>
        /// 强制刷新bar数据
        /// </summary>
        public void forceRefreshBar()
        {
            //设置强制刷新
            _m_bNeedForceRefreshSize = true;
        }

        /// <summary>
        /// 添加一个横栏对象
        /// </summary>
        /// <param name="_barController"></param>
        public void addBar(_AALUGUIGridBarController _barController)
        {
            if (null == _barController)
                return;

            //加入队列
            _m_lBarList.Add(_barController);

            //设置强制刷新
            _m_bNeedForceRefreshSize = true;
        }

        /// <summary>
        /// 删除一个横栏对象
        /// </summary>
        /// <param name="_barController"></param>
        public void removeBar(_AALUGUIGridBarController _barController)
        {
            if (null == _barController)
                return;

            //加入队列
            _m_lBarList.Remove(_barController);
            //此时直接隐藏
            _barController.hide();

            //设置强制刷新
            _m_bNeedForceRefreshSize = true;
        }

        /****************
         * 使用对应的刷新函数刷新所有子对象
         **/
        public void refreshAllItem(Action<_T_ITEM_WND, int> _refreshAction)
        {
            if (null == _refreshAction || null == _m_lShowItemList)
                return;

            //释放物品格的对象
            for (int i = 0; i < _m_lShowItemList.Count; i++)
            {
                _refreshAction(_m_lShowItemList[i], i + _m_iStartItemIdx);
            }
        }

        /****************
         * 强制刷新对应对象
         **/
        public void forceRefreshAllItem()
        {
            //刷新每个对象窗口
            refreshAllItem(_refreshItemwnd);
        }
        public void forceRefreshItem(int _index)
        {
            //释放物品格的对象
            for (int i = 0; i < _m_lShowItemList.Count; i++)
            {
                if (i + _m_iStartItemIdx == _index)
                    _refreshItemwnd(_m_lShowItemList[i], i + _m_iStartItemIdx);
            }
        }

        /****************
         * 清除所有子窗口
         **/
        public void clearAll()
        {
            //释放物品格的对象
            foreach (_T_ITEM_WND itemWnd in _m_lTotalItemList)
            {
                //使子窗口的位置可以正确显示
                itemWnd.wnd.transform.SetParent(null);

                GGUIHotfixCommonMono wndMono = itemWnd.wnd;
                //先调用析构函数
                itemWnd.discard();
                //删除对应的对象
                ALUnityCommon.releaseGameObj(wndMono);
            }
            //清空数据集
            _m_lShowItemList.Clear();
            _m_lTotalItemList.Clear();
        }

        /**********
         * 移动到最底层
         **/
        public void moveToLeft()
        {
            if (null == wnd || null == wnd.scrollRect)
                return;

            wnd.scrollRect.horizontalNormalizedPosition = 0;
        }
        public void moveToRight()
        {
            if (null == wnd || null == wnd.scrollRect)
                return;

            wnd.scrollRect.horizontalNormalizedPosition = 1;
        }
        public void moveToTop()
        {
            if (null == wnd || null == wnd.scrollRect)
                return;

            wnd.scrollRect.verticalNormalizedPosition = 1;
        }
        public void moveToBottom()
        {
            if (null == wnd || null == wnd.scrollRect)
                return;

            wnd.scrollRect.verticalNormalizedPosition = 0;
        }
        public void moveToHorizontalRate(float _rate)
        {
            if (null == wnd || null == wnd.scrollRect)
                return;

            float finalH = Mathf.Clamp(1 - _rate, 0f, 1f);
            wnd.scrollRect.horizontalNormalizedPosition = finalH;
        }
        public void moveToVerticalRate(float _rate)
        {
            if (null == wnd || null == wnd.scrollRect)
                return;

            float finalV = Mathf.Clamp(1 - _rate, 0f, 1f);
            wnd.scrollRect.verticalNormalizedPosition = finalV;
        }

        /***************
         * 每帧刷新操作
         **/
        public void frameRefresh()
        {
            if (null == wnd || null == wnd.gameObject || !wnd.gameObject.activeInHierarchy)
                return;

            //检查窗口尺寸
            checkWndSize();

            //计算区域
            _recalculateCurRect();

            //在每帧刷新的时候调用的事件函数
            _onFrameRefresh();
        }

        /// <summary>
        /// 每帧刷新的时候调用的事件函数，子类有需求则重写
        /// </summary>
        protected virtual void _onFrameRefresh()
        { }


        /// <summary>
        /// 检测窗口尺寸，在对应事件触发
        /// </summary>
        public void checkWndSize()
        {
            if (wnd.gridAreaMaskObj == null)
                return;

            //遮罩尺寸获取
            if (wnd.gridAreaMaskObj.rect.width != _m_fWidth || wnd.gridAreaMaskObj.rect.height != _m_fHeight
                || _m_iShowTotlaCount != _m_iTotalCount || _m_bNeedForceRefreshSize)
            {
                //重置标记
                _m_bNeedForceRefreshSize = false;

                //此时需要重新计算所有对象
                _m_fWidth = wnd.gridAreaMaskObj.rect.width;
                _m_fHeight = wnd.gridAreaMaskObj.rect.height;

                if (wnd.itemTemplateHeight <= 0 || wnd.itemTemplateWidth <= 0)
                {
                    Debug.LogError("_ANPGGUIHotfixBasicWnd CheckWndSize() wnd.itemTemplate.height==0 wnd.itemTemplate.width=0 ");
                    return;
                }

                //重新赋值间隔数据，保证使用原始数据计算不会在过程被更改
                _m_vCalSpace = wnd.spaceSize;

                //此时需要重新计算所有对象的数量
                if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    _m_iPerUnitItemCount = (int)(((_m_fHeight - _m_vCalSpace.y) / (wnd.itemTemplateHeight + _m_vCalSpace.y)) + 0.001f);
                    _m_iShowLineCount = (int)(((_m_fWidth - _m_vCalSpace.x) / (wnd.itemTemplateWidth + _m_vCalSpace.x)) + 0.001f) + 2;

                    if (0 != wnd.perLineItemCount && wnd.perLineItemCount < _m_iPerUnitItemCount)
                        _m_iPerUnitItemCount = wnd.perLineItemCount;

                    //最少每行一个
                    if (_m_iPerUnitItemCount <= 0)
                        _m_iPerUnitItemCount = 1;

                    //如果窗口需要自适应尺寸则在此进行计算
                    if (wnd.autoFixLine)
                    {
                        //重新计算高度
                        _m_vCalSpace.y = (_m_fHeight - (_m_iPerUnitItemCount * wnd.itemTemplateHeight)) / (_m_iPerUnitItemCount + 1);
                    }
                }
                else
                {
                    _m_iPerUnitItemCount = (int)(((_m_fWidth - _m_vCalSpace.x) / (wnd.itemTemplateWidth + _m_vCalSpace.x)) + 0.001f);
                    _m_iShowLineCount = (int)(((_m_fHeight - _m_vCalSpace.y) / (wnd.itemTemplateHeight + _m_vCalSpace.y)) + 0.001f) + 2;

                    if (0 != wnd.perLineItemCount && wnd.perLineItemCount < _m_iPerUnitItemCount)
                        _m_iPerUnitItemCount = wnd.perLineItemCount;

                    //最少每行一个
                    if (_m_iPerUnitItemCount <= 0)
                        _m_iPerUnitItemCount = 1;

                    //如果窗口需要自适应尺寸则在此进行计算
                    if (wnd.autoFixLine)
                    {
                        //重新计算高度
                        _m_vCalSpace.x = (_m_fWidth - (_m_iPerUnitItemCount * wnd.itemTemplateWidth)) / (_m_iPerUnitItemCount + 1);
                    }
                }

                _m_iShowTotlaCount = _m_iTotalCount;

                //根据是否有bar存在进行不同处理
                if (_m_lBarList.Count <= 0)
                {
                    int totalLineCount = (_m_iShowTotlaCount + _m_iPerUnitItemCount - 1) / _m_iPerUnitItemCount;
                    //设置区域对象的尺寸
                    if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                    {
                        //设置总宽度的时候需要附带padding
                        _m_fAllWidth = (totalLineCount * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x + wnd.paddingForSide.x + wnd.paddingForSide.y;
                        _m_fAllHeight = (_m_iPerUnitItemCount * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y;
                        _m_fScrollBarWidth = _m_fAllWidth - wnd.gridAreaMaskObj.rect.width;
                        _m_fScrollBarHeight = _m_fAllHeight - wnd.gridAreaMaskObj.rect.height;
                        wnd.gridAreaUIObj.sizeDelta = new Vector2(_m_fAllWidth, _m_fAllHeight);
                    }
                    else
                    {
                        _m_fAllWidth = (_m_iPerUnitItemCount * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x;
                        //设置总高度的时候需要附带padding
                        _m_fAllHeight = (totalLineCount * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y + wnd.paddingForSide.x + wnd.paddingForSide.y;
                        _m_fScrollBarWidth = _m_fAllWidth - wnd.gridAreaMaskObj.rect.width;
                        _m_fScrollBarHeight = _m_fAllHeight - wnd.gridAreaMaskObj.rect.height;
                        wnd.gridAreaUIObj.sizeDelta = new Vector2(_m_fAllWidth, _m_fAllHeight);
                    }
                }
                else
                {
                    //获取尺寸对应的size
                    int itemSize = wnd.itemTemplateWidth;
                    float space = _m_vCalSpace.x;
                    if (wnd.layoutStyle == ALGUIListLayoutStyle.VERTICAL)
                    {
                        itemSize = wnd.itemTemplateHeight;
                        space = _m_vCalSpace.y;
                    }

                    int startItemIndex = 0;
                    float startShowPos = 0;
                    //逐个bar进行缩放大小处理
                    _AALUGUIGridBarController controller = null;
                    _AALUGUIGridBarController preController = null;
                    //排序所有bar
                    _m_lBarList.Sort();
                    //清空临时列表
                    _m_lTmpCheckSizeBarList.Clear();
                    //逐个遍历进行缩放处理
                    for (int i = 0; i < _m_lBarList.Count; i++)
                    {
                        controller = _m_lBarList[i];
                        if (null == controller)
                            continue;

                        startShowPos = controller.onResizeWnd(_m_iPerUnitItemCount, startItemIndex, startShowPos, itemSize, space);

                        //判断当前bar的实际其实位置是否小于当前开始位置，是则需要将之前对象的起始位置设置为对应值
                        if (controller.realInsertItemIdx > startItemIndex)
                        {
                            _m_lTmpCheckSizeBarList.Clear();
                            _m_lTmpCheckSizeBarList.Add(controller);
                        }
                        else if (controller.realInsertItemIdx == startItemIndex)
                        {
                            //如位置相同则将对应对象加入临时队列，待之后处理
                            _m_lTmpCheckSizeBarList.Add(controller);
                        }
                        else
                        {
                            //如果实际位置小于当前位置，则需要将之前临时队列的所有bar的实际插入位置纠正
                            for (int n = 0; n < _m_lTmpCheckSizeBarList.Count; n++)
                            {
                                _m_lTmpCheckSizeBarList[n].setRealInsertIndex(controller.realInsertItemIdx);
                            }
                            _m_lTmpCheckSizeBarList.Clear();
                        }

                        //设置为实际开始位置
                        startItemIndex = controller.realInsertItemIdx;
                        //累加上显示的高度
                        startShowPos += controller.barHeight;

                        //设置前置数据的结尾序号
                        if (null != preController)
                            preController.setShowItemDataEndIndex(startItemIndex);

                        preController = controller;
                    }
                    //清空临时队列
                    _m_lTmpCheckSizeBarList.Clear();

                    if (startItemIndex < _m_iShowTotlaCount)
                    {
                        //重新计算剩余数量
                        //计算当前bar之前需要显示的数量
                        int showCount = _m_iShowTotlaCount - startItemIndex;
                        //计算当前bar需要显示的行数
                        int lineCount = (showCount + _m_iPerUnitItemCount - 1) / _m_iPerUnitItemCount;
                        //计算之前显示的高度
                        startShowPos = startShowPos + (lineCount * (itemSize + space)) + space;
                    }

                    //根据横向还是纵向做不同处理
                    if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                    {
                        //设置总宽度的时候需要附带padding
                        _m_fAllWidth = startShowPos + wnd.paddingForSide.x + wnd.paddingForSide.y;
                        _m_fAllHeight = (_m_iPerUnitItemCount * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y;
                        _m_fScrollBarWidth = _m_fAllWidth - wnd.gridAreaMaskObj.rect.width;
                        _m_fScrollBarHeight = _m_fAllHeight - wnd.gridAreaMaskObj.rect.height;
                        wnd.gridAreaUIObj.sizeDelta = new Vector2(_m_fAllWidth, _m_fAllHeight);
                    }
                    else
                    {
                        _m_fAllWidth = (_m_iPerUnitItemCount * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x;
                        //设置总高度的时候需要附带padding
                        _m_fAllHeight = startShowPos + wnd.paddingForSide.x + wnd.paddingForSide.y;
                        _m_fScrollBarWidth = _m_fAllWidth - wnd.gridAreaMaskObj.rect.width;
                        _m_fScrollBarHeight = _m_fAllHeight - wnd.gridAreaMaskObj.rect.height;
                        wnd.gridAreaUIObj.sizeDelta = new Vector2(_m_fAllWidth, _m_fAllHeight);
                    }
                }

                //计算需要的总数量
                int totalCacheItemCount = (_m_iPerUnitItemCount * _m_iShowLineCount);

                //初始化所有显示对象队列
                _T_ITEM_WND tmpItem = null;
                while (_m_lShowItemList.Count < totalCacheItemCount)
                    _m_lShowItemList.Add(null);
                while (_m_lShowItemList.Count > totalCacheItemCount)
                {
                    tmpItem = _m_lShowItemList[_m_lShowItemList.Count - 1];
                    if (null != tmpItem)
                        tmpItem.resetGridItem();

                    _m_lShowItemList.RemoveAt(_m_lShowItemList.Count - 1);
                }

                //根据需要的总数量从数据集合中删除多余，增加缺少的
                while (_m_lTotalItemList.Count < totalCacheItemCount)
                {
                    _addItemWnd();
                }
                //删除多余的
                while (_m_lTotalItemList.Count > totalCacheItemCount)
                {
                    _removeItemWnd(_m_lTotalItemList[_m_lTotalItemList.Count - 1]);
                }

                //设置强制刷新
                _m_bNeedForceRefresh = true;
            }
        }

        /// <summary>
        /// 重新计算当前区域相关信息，在拖动窗口时触发，或缩放等事件触发时触发
        /// </summary>
        protected void _recalculateCurRect()
        {
            if (_m_lBarList.Count <= 0)
            {
                _recalculateCurRectNoBar();
            }
            else
            {
                _recalculateCurRectForBar();
            }
        }
        /// <summary>
        /// 在没有插入Bar的情况下的原生刷新处理
        /// </summary>
        private void _recalculateCurRectNoBar()
        {
            if (null == wnd || null == wnd.itemTemplate)
                return;

            //设置上一次的区域
            _m_vGridPreShowRect = _m_vGridCurShowRect;
            //根据当前
            _m_vGridCurShowRect.x = -wnd.gridAreaUIObj.anchoredPosition.x;
            _m_vGridCurShowRect.y = wnd.gridAreaUIObj.anchoredPosition.y;

            if (!_m_bNeedForceRefresh && _m_vGridCurShowRect.x == _m_vGridPreShowRect.x && _m_vGridCurShowRect.y == _m_vGridPreShowRect.y)
                return;

            //设置强制刷新无效
            bool isForceRefresh = _m_bNeedForceRefresh;
            _m_bNeedForceRefresh = false;

            //根据显示区域计算开始显示的窗口索引
            if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
            {
                if (_m_vGridCurShowRect.x <= 0)
                    _m_iStartItemIdx = 0;
                else
                    _m_iStartItemIdx = (int)((_m_vGridCurShowRect.x - wnd.paddingForSide.x) / (wnd.itemTemplateWidth + _m_vCalSpace.x)) * _m_iPerUnitItemCount;

                _m_iFinalItemIdx = _m_iStartItemIdx + (_m_iPerUnitItemCount * _m_iShowLineCount) - 1;
                //判断最后的索引是否有效
                if (_m_iFinalItemIdx >= _m_iTotalCount)
                    _m_iFinalItemIdx = _m_iTotalCount - 1;
            }
            else
            {
                if (_m_vGridCurShowRect.y <= 0)
                    _m_iStartItemIdx = 0;
                else
                    _m_iStartItemIdx = (int)((_m_vGridCurShowRect.y - wnd.paddingForSide.x) / (wnd.itemTemplateHeight + _m_vCalSpace.y)) * _m_iPerUnitItemCount;

                _m_iFinalItemIdx = _m_iStartItemIdx + (_m_iPerUnitItemCount * _m_iShowLineCount) - 1;
                //判断最后的索引是否有效
                if (_m_iFinalItemIdx >= _m_iTotalCount)
                    _m_iFinalItemIdx = _m_iTotalCount - 1;
            }

            //根据显示的对象范围逐个调整对象位置
            //先清空所有显示队列
            for (_m_iTmpi = 0; _m_iTmpi < _m_lShowItemList.Count; _m_iTmpi++)
                _m_lShowItemList[_m_iTmpi] = null;
            //逐个将显示范围内的对象放入，此处处理的是可能的重复对象
            for (_m_iTmpi = 0; _m_iTmpi < _m_lTotalItemList.Count; _m_iTmpi++)
            {
                _m_wTmpItem = _m_lTotalItemList[_m_iTmpi];
                //判断显示对象是否在索引区域内，是则直接设置数据索引
                if (_m_wTmpItem.itemIdx >= _m_iStartItemIdx && _m_wTmpItem.itemIdx <= _m_iFinalItemIdx)
                {
                    //此时设置显示对象对应索引的对象为本对象
                    _m_iTmpi2 = _m_wTmpItem.itemIdx - _m_iStartItemIdx;

                    //设置对应显示队列中对象
                    if (null == _m_lShowItemList[_m_iTmpi2])
                    {
                        _m_lShowItemList[_m_iTmpi2] = _m_wTmpItem;
                    }
                    else
                    {
                        _m_wTmpItem.resetGridItem();
                    }
                }
                else
                {
                    _m_wTmpItem.resetGridItem();
                }
            }
            //两个队列分别逐个遍历，将未使用的对象放入显示队列对应空位
            _m_iTmpi2 = 0;  //此时设置为所有对象的索引位置
            for (_m_iTmpi = 0; _m_iTmpi < _m_lShowItemList.Count; _m_iTmpi++)
            {
                if (null == _m_lShowItemList[_m_iTmpi])
                {
                    //此时需要寻找对象并设置
                    while (_m_lTotalItemList[_m_iTmpi2].itemIdx != -1)
                    {
                        _m_iTmpi2++;
                    }

                    _m_wTmpItem = _m_lTotalItemList[_m_iTmpi2];
                    _m_iTmpi2++;
                    //此时设置对象
                    _m_lShowItemList[_m_iTmpi] = _m_wTmpItem;
                }
            }

            //将显示队列遍历，设置无效的显示信息，并刷新每个的新位置
            for (_m_iTmpi = 0; _m_iTmpi < _m_lShowItemList.Count; _m_iTmpi++)
            {
                _m_wTmpItem = _m_lShowItemList[_m_iTmpi];
                if (null == _m_wTmpItem)
                    continue;

                _m_iTmpi2 = _m_iTmpi + _m_iStartItemIdx;
                //超出范围则中断
                if (_m_iTmpi2 >= _m_iTotalCount)
                    break;

                //在0数据之前的数据都是无效数据
                if (_m_iTmpi2 < 0)
                {
                    _m_wTmpItem.resetGridItem();
                    continue;
                }

                //显示窗口
                _m_wTmpItem.showWnd();
                _m_wTmpItem.showGridItem();
                //设置显示对象位置
                if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    //横向位置设置
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , ((_m_iTmpi2 / _m_iPerUnitItemCount) * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x + wnd.paddingForSide.x
                        , -(((_m_iTmpi2 % _m_iPerUnitItemCount) * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y));
                }
                else
                {
                    //纵向位置设置
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , ((_m_iTmpi2 % _m_iPerUnitItemCount) * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x
                        , -(((_m_iTmpi2 / _m_iPerUnitItemCount) * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y + wnd.paddingForSide.x));
                }

                //判断序号是否一致，并刷新显示
                if (_m_wTmpItem.itemIdx == _m_iTmpi2)
                {
                    //判断是否需要强制刷新，是则刷新显示项
                    if (isForceRefresh)
                        _refreshItemwnd(_m_wTmpItem, _m_iTmpi2);
                    continue;
                }

                //设置索引
                _m_wTmpItem.setItemIdx(_m_iTmpi2);
                //刷新显示信息
                _refreshItemwnd(_m_wTmpItem, _m_iTmpi2);
            }
        }
        /// <summary>
        /// 在有插入Bar的情况下的刷新处理
        /// </summary>
        private void _recalculateCurRectForBar()
        {
            if (null == wnd || null == wnd.itemTemplate)
                return;

            //设置上一次的区域
            _m_vGridPreShowRect = _m_vGridCurShowRect;
            //根据当前
            _m_vGridCurShowRect.x = -wnd.gridAreaUIObj.anchoredPosition.x;
            _m_vGridCurShowRect.y = wnd.gridAreaUIObj.anchoredPosition.y;

            if (!_m_bNeedForceRefresh && _m_vGridCurShowRect.x == _m_vGridPreShowRect.x && _m_vGridCurShowRect.y == _m_vGridPreShowRect.y)
                return;

            //设置强制刷新无效
            bool isForceRefresh = _m_bNeedForceRefresh;
            _m_bNeedForceRefresh = false;

            //根据排版计算显示位置
            //起始位置注意要扣除预留空位
            float curShowStartPos = _m_vGridCurShowRect.x - wnd.paddingForSide.x;
            float curShowEndPos = curShowStartPos + wnd.gridAreaMaskObj.rect.width;
            int itemSize = wnd.itemTemplateWidth;
            float space = _m_vCalSpace.x;
            if (wnd.layoutStyle == ALGUIListLayoutStyle.VERTICAL)
            {
                curShowStartPos = _m_vGridCurShowRect.y - wnd.paddingForSide.x;
                curShowEndPos = curShowStartPos + wnd.gridAreaMaskObj.rect.height;
                itemSize = wnd.itemTemplateHeight;
                space = _m_vCalSpace.y;
            }

            //先清空所有显示队列
            for (_m_iTmpi = 0; _m_iTmpi < _m_lShowItemList.Count; _m_iTmpi++)
                _m_lShowItemList[_m_iTmpi] = null;

            //所有item中使用的索引
            int startShowIndex = 0;
            //是否已经超出范围
            bool isPassed = false;
            bool startShow = false;
            //根据排版计算各区域需要显示的信息
            float showPos = 0;
            int showStartIndex = 0;
            _AALUGUIGridBarController controller = null;
            _AALUGUIGridBarController preController = null;
            _AALUGUIGridBarController firstController = null;

            _m_lShowBarList.Clear();
            //开始计算显示的bar队列以及显示数据
            for (int i = 0; i < _m_lBarList.Count; i++)
            {
                controller = _m_lBarList[i];
                if (null == controller)
                    continue;

                //计算高度是否在区间内
                if (isPassed)
                {
                    //超出则直接隐藏
                    controller.hide();

                    preController = controller;
                    continue;
                }
                else if (controller.showEndPos < curShowStartPos)
                {
                    //此时直接隐藏即可
                    controller.hide();
                    //计算显示的开始位置
                    showPos = controller.showEndPos;
                    //设置开始计算的显示数据索引
                    showStartIndex = controller.realInsertItemIdx;

                    preController = controller;
                    continue;
                }

                //设置第一个显示对象的索引
                if (!startShow)
                {
                    firstController = preController;

                    //开始根据当前的区域进行显示数据的处理
                    //根据显示区域计算开始显示的窗口索引
                    if (curShowStartPos - showPos <= 0)
                        _m_iStartItemIdx = showStartIndex;
                    else
                        _m_iStartItemIdx = showStartIndex + ((int)((curShowStartPos - showPos) / (itemSize + space)) * _m_iPerUnitItemCount);

                    //如得到的开始位置大于当前bar的开始位置，则以当前bar开始位置为准
                    if (_m_iStartItemIdx > controller.realInsertItemIdx)
                        _m_iStartItemIdx = controller.realInsertItemIdx;

                    startShowIndex = _m_iStartItemIdx;

                    //设置开始显示
                    startShow = true;

                    _m_iFinalItemIdx = startShowIndex + (_m_iPerUnitItemCount * _m_iShowLineCount) - 1;

                    //判断最后的索引是否有效
                    if (_m_iFinalItemIdx >= _m_iShowTotlaCount)
                        _m_iFinalItemIdx = _m_iShowTotlaCount - 1;

                    //根据显示的对象范围逐个调整对象位置
                    //逐个将显示范围内的对象放入，此处处理的是可能的重复对象
                    for (int totalUseIndex = 0; totalUseIndex < _m_lTotalItemList.Count; totalUseIndex++)
                    {
                        _m_wTmpItem = _m_lTotalItemList[totalUseIndex];
                        //判断显示对象是否在索引区域内，是则直接设置数据索引
                        if (_m_wTmpItem.itemIdx >= startShowIndex && _m_wTmpItem.itemIdx <= _m_iFinalItemIdx)
                        {
                            //此时设置显示对象对应索引的对象为本对象
                            _m_iTmpi2 = _m_wTmpItem.itemIdx - startShowIndex;

                            //设置对应显示队列中对象
                            if (null == _m_lShowItemList[_m_iTmpi2])
                            {
                                _m_lShowItemList[_m_iTmpi2] = _m_wTmpItem;
                            }
                            else
                            {
                                _m_wTmpItem.resetGridItem();
                            }
                        }
                        else
                        {
                            _m_wTmpItem.resetGridItem();
                        }
                    }
                    //两个队列分别逐个遍历，将未使用的对象放入显示队列对应空位
                    _m_iTmpi2 = 0;  //此时设置为所有对象的索引位置
                    for (_m_iTmpi = 0; _m_iTmpi < _m_lShowItemList.Count; _m_iTmpi++)
                    {
                        if (null == _m_lShowItemList[_m_iTmpi])
                        {
                            //此时需要寻找对象并设置
                            while (_m_lTotalItemList[_m_iTmpi2].itemIdx != -1)
                            {
                                _m_iTmpi2++;
                            }

                            _m_wTmpItem = _m_lTotalItemList[_m_iTmpi2];
                            _m_iTmpi2++;
                            //此时设置对象
                            _m_lShowItemList[_m_iTmpi] = _m_wTmpItem;
                        }
                    }
                }
                else if (controller.showStartPos > curShowEndPos)
                {
                    //在开始位置已经超出结束范围时需要隐藏对应controller
                    controller.hide();
                }

                //添加到显示的bar队列
                _m_lShowBarList.Add(controller);

                //判断是否超出范围
                if (controller.showEndPos > curShowEndPos)
                {
                    isPassed = true;
                }
            }

            //判断是否全部bar都不在范围，此时以最后一个bar作为开始
            if (!startShow)
            {
                firstController = preController;

                //开始根据当前的区域进行显示数据的处理
                //根据显示区域计算开始显示的窗口索引
                if (curShowStartPos - showPos <= 0)
                    _m_iStartItemIdx = showStartIndex;
                else
                    _m_iStartItemIdx = showStartIndex + ((int)((curShowStartPos - showPos) / (itemSize + space)) * _m_iPerUnitItemCount);

                startShowIndex = _m_iStartItemIdx;

                //设置开始显示
                startShow = true;

                _m_iFinalItemIdx = startShowIndex + (_m_iPerUnitItemCount * _m_iShowLineCount) - 1;

                //判断最后的索引是否有效
                if (_m_iFinalItemIdx >= _m_iShowTotlaCount)
                    _m_iFinalItemIdx = _m_iShowTotlaCount - 1;

                //根据显示的对象范围逐个调整对象位置
                //逐个将显示范围内的对象放入，此处处理的是可能的重复对象
                for (int totalUseIndex = 0; totalUseIndex < _m_lTotalItemList.Count; totalUseIndex++)
                {
                    _m_wTmpItem = _m_lTotalItemList[totalUseIndex];
                    //判断显示对象是否在索引区域内，是则直接设置数据索引
                    if (_m_wTmpItem.itemIdx >= startShowIndex && _m_wTmpItem.itemIdx <= _m_iFinalItemIdx)
                    {
                        //此时设置显示对象对应索引的对象为本对象
                        _m_iTmpi2 = _m_wTmpItem.itemIdx - startShowIndex;

                        //设置对应显示队列中对象
                        if (null == _m_lShowItemList[_m_iTmpi2])
                        {
                            _m_lShowItemList[_m_iTmpi2] = _m_wTmpItem;
                        }
                        else
                        {
                            _m_wTmpItem.resetGridItem();
                        }
                    }
                    else
                    {
                        _m_wTmpItem.resetGridItem();
                    }
                }
                //两个队列分别逐个遍历，将未使用的对象放入显示队列对应空位
                _m_iTmpi2 = 0;  //此时设置为所有对象的索引位置
                for (_m_iTmpi = 0; _m_iTmpi < _m_lShowItemList.Count; _m_iTmpi++)
                {
                    if (null == _m_lShowItemList[_m_iTmpi])
                    {
                        //此时需要寻找对象并设置
                        while (_m_lTotalItemList[_m_iTmpi2].itemIdx != -1)
                        {
                            _m_iTmpi2++;
                        }

                        _m_wTmpItem = _m_lTotalItemList[_m_iTmpi2];
                        _m_iTmpi2++;
                        //此时设置对象
                        _m_lShowItemList[_m_iTmpi] = _m_wTmpItem;
                    }
                }
            }

            //从第一个bar显示对象
            controller = firstController;

            int showBarIdx = 0;
            if (_m_lShowBarList.Count > showBarIdx)
                firstController = _m_lShowBarList[showBarIdx];
            else
                firstController = null;

            float startPos = 0;
            if (null != controller)
                startPos = controller.showEndPos;

            //将显示队列遍历，设置无效的显示信息，并刷新每个的新位置
            for (_m_iTmpi = 0; _m_iTmpi < _m_lShowItemList.Count; _m_iTmpi++)
            {
                _m_wTmpItem = _m_lShowItemList[_m_iTmpi];
                if (null == _m_wTmpItem)
                    continue;

                //计算在这个组内的显示序号下标
                _m_iTmpi2 = _m_iTmpi + startShowIndex;

                //判断下标是否超出显示bar，是则调整到下一个bar
                while (null != firstController && _m_iTmpi2 >= firstController.realInsertItemIdx)
                {
                    //获取下一个bar
                    controller = firstController;
                    startPos = controller.showEndPos;

                    showBarIdx++;
                    if (showBarIdx < _m_lShowBarList.Count)
                        firstController = _m_lShowBarList[showBarIdx];
                    else
                        firstController = null;

                    //判断first是否需要显示
                    if (null != controller)
                    {
                        if (controller.showStartPos <= curShowEndPos)
                        {
                            controller.show();

                            //设置位置
                            if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                            {
                                //横向位置设置
                                controller.setPos(controller.showStartPos + wnd.paddingForSide.x, 0);
                            }
                            else
                            {
                                //横向位置设置
                                controller.setPos(0, -(controller.showStartPos + wnd.paddingForSide.x));
                            }
                        }
                        else
                        {
                            controller.hide();
                        }
                    }
                }

                //超出范围则中断
                if (_m_iTmpi2 >= _m_iTotalCount)
                {
                    _m_wTmpItem.resetGridItem();
                    continue;
                }

                //纠正相对索引
                int inControlIdx = _m_iTmpi2;
                if (null != controller)
                    inControlIdx = _m_iTmpi2 - controller.realInsertItemIdx;

                //显示窗口
                _m_wTmpItem.showWnd();
                _m_wTmpItem.showGridItem();
                //设置显示对象位置
                if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    //横向位置设置
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , startPos + ((inControlIdx / _m_iPerUnitItemCount) * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x + wnd.paddingForSide.x
                        , -(((inControlIdx % _m_iPerUnitItemCount) * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y));
                }
                else
                {
                    //纵向位置设置
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , ((inControlIdx % _m_iPerUnitItemCount) * (wnd.itemTemplateWidth + _m_vCalSpace.x)) + _m_vCalSpace.x
                        , -startPos - (((inControlIdx / _m_iPerUnitItemCount) * (wnd.itemTemplateHeight + _m_vCalSpace.y)) + _m_vCalSpace.y + wnd.paddingForSide.x));
                }

                //判断序号是否一致，并刷新显示
                if (_m_wTmpItem.itemIdx == _m_iTmpi2)
                {
                    //判断是否需要强制刷新，是则刷新显示项
                    if (isForceRefresh)
                        _refreshItemwnd(_m_wTmpItem, _m_iTmpi2);
                    continue;
                }

                //设置索引
                _m_wTmpItem.setItemIdx(_m_iTmpi2);
                //刷新显示信息
                _refreshItemwnd(_m_wTmpItem, _m_iTmpi2);
            }
        }

        /******************
         * 添加一个子窗口到容器中
         **/
        protected void _addItemWnd()
        {
            //判断对应数据是否有效
            if (null == wnd || null == wnd.gridAreaUIObj || null == wnd.itemTemplate)
                return;

            //实例化一个子窗口对象
            GGUIHotfixCommonMono itemWndObj = GameObject.Instantiate(wnd.itemTemplate) as GGUIHotfixCommonMono;
            if (null == itemWndObj)
                return;

            //将子窗口添加到容器中
            itemWndObj.transform.SetParent(wnd.gridAreaUIObj.transform);
            itemWndObj.transform.localPosition = Vector3.zero;
            itemWndObj.transform.localScale = Vector3.one;

            //创建一个子窗口管理对象
            _T_ITEM_WND itemWnd = _createItemWnd(itemWndObj);
            if (null == itemWnd)
            {
                //创建对象无效，删除创建对象资源并退出
                ALUnityCommon.releaseGameObj(itemWndObj);
                return;
            }

            //调用子窗口的初始化函数
            itemWnd.initWnd();
            //调用子窗口的显示函数
            itemWnd.showWnd();
            //重置数据
            itemWnd.resetGridItem();

            //添加到数据集合
            _m_lTotalItemList.Add(itemWnd);
        }

        /**************
        * 从数据集中删除对应的子窗口对象
        * */
        protected void _removeItemWnd(_T_ITEM_WND _itemWnd)
        {
            _T_ITEM_WND tmpWnd = null;
            //遍历查找，匹配成功则跳出循环
            for (int i = 0; i < _m_lTotalItemList.Count; i++)
            {
                tmpWnd = _m_lTotalItemList[i];
                if (null == tmpWnd)
                    continue;

                if (tmpWnd == _itemWnd)
                {
                    //移除对应位置节点
                    _m_lTotalItemList.RemoveAt(i);
                    break;
                }
            }

            //判断数据是否有效
            if (_itemWnd != tmpWnd)
                return;

            GGUIHotfixCommonMono wndMono = tmpWnd.wnd;
            //使子窗口的位置可以正确显示
            tmpWnd.wnd.transform.SetParent(null);
            //释放窗口对象
            tmpWnd.discard();

            //删除对应的对象
            ALUnityCommon.releaseGameObj(wndMono);
        }

        /*************
         * 根据带入的已经实例化的图标对象，创建一个对应子窗口的管理对象
         **/
        protected abstract _T_ITEM_WND _createItemWnd(GGUIHotfixCommonMono _itemMono);
        /********************
         * 根据带入的窗口对象以及索引刷新相关的显示信息
         **/
        protected abstract void _refreshItemwnd(_T_ITEM_WND _itemMono, int _itemIdx);
    }
}