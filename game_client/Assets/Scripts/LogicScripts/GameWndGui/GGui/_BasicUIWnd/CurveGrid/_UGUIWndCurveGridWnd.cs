using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
        public abstract class _UGUIWndCurveGridWnd<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATALBasicUISubWnd<_T_CONTAINER_MONO>, _IALBasicRefreshUIWndInterface
        where _T_ITEM_MONO : _TALUGUIMonoGridItem
        where _T_CONTAINER_MONO : _UGUIMonoCurveGridWnd<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALUGUIBasicGridItemWnd<_T_ITEM_MONO>
    {
        /** 是否需要强制刷新 */
        private bool _m_bNeedForceRefresh;
        /** 是否需要强制重新计算窗口size */
        private bool _m_bNeedForceRefreshSize;
        /** 是否初始化强制刷新 */
        private bool _m_bInitForceRefresh = false;
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
        //是否刷新列表中岛中央显示
        private bool _m_isRefreshContentToCenter = false;

        private RectTransform _m_scrollRectRectTrans;
        private Vector2 _m_scrollRectDefaultSize;

        protected _UGUIWndCurveGridWnd(_T_CONTAINER_MONO _wnd)
            : base(_wnd)
        {
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
            if (null != _wnd && null != _wnd.scrollRect)
            {
                _m_scrollRectRectTrans = _wnd.scrollRect.GetComponent<RectTransform>();
                if (null != _m_scrollRectRectTrans)
                {
                    _m_scrollRectDefaultSize = new Vector2(_m_scrollRectRectTrans.rect.width,_m_scrollRectRectTrans.rect.height);
                }
            }
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
                pos.x = ((_idx / _m_iPerUnitItemCount) * (wnd.itemTemplate.width + _m_vCalSpace.x)) + _m_vCalSpace.x + wnd.paddingForSide.x;
                pos.y = -(((_idx % _m_iPerUnitItemCount) * (wnd.itemTemplate.height + _m_vCalSpace.y)) + _m_vCalSpace.y);
            }
            else
            {
                //纵向位置设置，注意加上预留位移
                pos.x = ((_idx % _m_iPerUnitItemCount) * (wnd.itemTemplate.width + _m_vCalSpace.x)) + _m_vCalSpace.x;
                pos.y = -(((_idx / _m_iPerUnitItemCount) * (wnd.itemTemplate.height + _m_vCalSpace.y)) + _m_vCalSpace.y + wnd.paddingForSide.x);
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

            _m_bInitForceRefresh = true;

            _m_isRefreshContentToCenter = true;
            
            ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, _m_iTotalCount == 0);
        }

        /****************
         * 使用对应的刷新函数刷新所有子对象
         **/
        public void refreshAllItem(Action<_T_ITEM_WND, int> _refreshAction)
        {
            if (null == _refreshAction || null == _m_lShowItemList)
                return;

            //释放物品格的对象
            for(int i = 0; i < _m_lShowItemList.Count; i++)
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
                if(i + _m_iStartItemIdx == _index)
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

                _T_ITEM_MONO wndMono = itemWnd.wnd;
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
            if(null == wnd || null == wnd.gameObject || !wnd.gameObject.activeInHierarchy)
                return;

            //检查窗口尺寸
            checkWndSize();

            _recalContentToCenter();

            //计算区域
            _recalculateCurRect();

            //在每帧刷新的时候调用的事件函数
            _onFrameRefresh();

            if (_m_bInitForceRefresh)//下一帧强制刷新一次，刷新itme的偏移值
            {
                _m_bNeedForceRefresh = true;
                _m_bInitForceRefresh = false;
            }
        }

        /// <summary>
        /// 刷新窗口到中间显示
        /// </summary>
        private void _recalContentToCenter()
        {
            if(null == _m_scrollRectRectTrans || null == wnd.gridAreaUIObj)
                return;
            if (!_m_isRefreshContentToCenter)
                return;
            if (!wnd.isContentToCenter)
                return;
            if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
            {
                if (wnd.gridAreaUIObj.rect.width < _m_scrollRectDefaultSize.x)
                {
                    _m_scrollRectRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,_m_scrollRectDefaultSize.x / 2 - wnd.gridAreaUIObj.rect.width / 2,wnd.gridAreaUIObj.rect.width);
                }
                else
                {
                    _m_scrollRectRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,0,_m_scrollRectDefaultSize.x);
                }
            }
            else
            {
                if (wnd.gridAreaUIObj.rect.height < _m_scrollRectDefaultSize.y)
                {
                    _m_scrollRectRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,_m_scrollRectDefaultSize.y / 2 - wnd.gridAreaUIObj.rect.height / 2,wnd.gridAreaUIObj.rect.height);
                }
                else
                {
                    _m_scrollRectRectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,0,_m_scrollRectDefaultSize.y);
                }
            }
            
            _m_isRefreshContentToCenter = false;
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
                
                if (wnd.itemTemplate.height <= 0 || wnd.itemTemplate.width <= 0) {
                    Debug.LogError("_AALUGUIBasicGridSubWnd CheckWndSize() wnd.itemTemplate.height==0 wnd.itemTemplate.width=0 ");
                    return;
                }

                //重新赋值间隔数据，保证使用原始数据计算不会在过程被更改
                _m_vCalSpace = wnd.spaceSize;

                //此时需要重新计算所有对象的数量
                if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    _m_iPerUnitItemCount = (int)(((_m_fHeight - _m_vCalSpace.y) / (wnd.itemTemplate.height + _m_vCalSpace.y)) + 0.001f);
                    _m_iShowLineCount = (int)(((_m_fWidth - _m_vCalSpace.x) / (wnd.itemTemplate.width + _m_vCalSpace.x)) + 0.001f) + 2;

                    if (0 != wnd.perLineItemCount && wnd.perLineItemCount < _m_iPerUnitItemCount)
                        _m_iPerUnitItemCount = wnd.perLineItemCount;

                    //最少每行一个
                    if (_m_iPerUnitItemCount <= 0)
                        _m_iPerUnitItemCount = 1;

                    //如果窗口需要自适应尺寸则在此进行计算
                    if (wnd.autoFixLine)
                    {
                        //重新计算高度
                        _m_vCalSpace.y = (_m_fHeight - (_m_iPerUnitItemCount * wnd.itemTemplate.height)) / (_m_iPerUnitItemCount + 1);
                    }
                }
                else
                {
                    _m_iPerUnitItemCount = (int)(((_m_fWidth - _m_vCalSpace.x) / (wnd.itemTemplate.width + _m_vCalSpace.x)) + 0.001f);
                    _m_iShowLineCount = (int)(((_m_fHeight - _m_vCalSpace.y) / (wnd.itemTemplate.height + _m_vCalSpace.y)) + 0.001f) + 2;

                    if (0 != wnd.perLineItemCount && wnd.perLineItemCount < _m_iPerUnitItemCount)
                        _m_iPerUnitItemCount = wnd.perLineItemCount;

                    //最少每行一个
                    if (_m_iPerUnitItemCount <= 0)
                        _m_iPerUnitItemCount = 1;

                    //如果窗口需要自适应尺寸则在此进行计算
                    if (wnd.autoFixLine)
                    {
                        //重新计算高度
                        _m_vCalSpace.x = (_m_fWidth - (_m_iPerUnitItemCount * wnd.itemTemplate.width)) / (_m_iPerUnitItemCount + 1);
                    }
                }

                _m_iShowTotlaCount = _m_iTotalCount;

                int totalLineCount = (_m_iShowTotlaCount + _m_iPerUnitItemCount - 1) / _m_iPerUnitItemCount;
                //设置区域对象的尺寸
                if(wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    //设置总宽度的时候需要附带padding
                    _m_fAllWidth = (totalLineCount * (wnd.itemTemplate.width + _m_vCalSpace.x)) + _m_vCalSpace.x + wnd.paddingForSide.x + wnd.paddingForSide.y;
                    _m_fAllHeight = (_m_iPerUnitItemCount * (wnd.itemTemplate.height + _m_vCalSpace.y)) + _m_vCalSpace.y;
                    _m_fScrollBarWidth = _m_fAllWidth - wnd.gridAreaMaskObj.rect.width;
                    _m_fScrollBarHeight = _m_fAllHeight - wnd.gridAreaMaskObj.rect.height;
                    wnd.gridAreaUIObj.sizeDelta = new Vector2(_m_fAllWidth, _m_fAllHeight);
                }
                else
                {
                    _m_fAllWidth = (_m_iPerUnitItemCount * (wnd.itemTemplate.width + _m_vCalSpace.x)) + _m_vCalSpace.x;
                    //设置总高度的时候需要附带padding
                    _m_fAllHeight = (totalLineCount * (wnd.itemTemplate.height + _m_vCalSpace.y)) + _m_vCalSpace.y + wnd.paddingForSide.x + wnd.paddingForSide.y;
                    _m_fScrollBarWidth = _m_fAllWidth - wnd.gridAreaMaskObj.rect.width;
                    _m_fScrollBarHeight = _m_fAllHeight - wnd.gridAreaMaskObj.rect.height;
                    wnd.gridAreaUIObj.sizeDelta = new Vector2(_m_fAllWidth, _m_fAllHeight);
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
                    if(null != tmpItem)
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
                    _m_iStartItemIdx = (int)((_m_vGridCurShowRect.x - wnd.paddingForSide.x) / (wnd.itemTemplate.width + _m_vCalSpace.x)) * _m_iPerUnitItemCount;

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
                    _m_iStartItemIdx = (int)((_m_vGridCurShowRect.y - wnd.paddingForSide.x) / (wnd.itemTemplate.height + _m_vCalSpace.y)) * _m_iPerUnitItemCount;

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
                if(_m_wTmpItem.itemIdx >= _m_iStartItemIdx && _m_wTmpItem.itemIdx <= _m_iFinalItemIdx)
                {
                    //此时设置显示对象对应索引的对象为本对象
                    _m_iTmpi2 = _m_wTmpItem.itemIdx - _m_iStartItemIdx;

                    //设置对应显示队列中对象
                    if(null == _m_lShowItemList[_m_iTmpi2])
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
                if(null == _m_lShowItemList[_m_iTmpi])
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
                if(_m_iTmpi2 < 0)
                {
                    _m_wTmpItem.resetGridItem();
                    continue;
                }

                //显示窗口
                _m_wTmpItem.showWnd();
                _m_wTmpItem.showGridItem();

                //>>>根据方向先设置对应方向的值，该值不受曲线影响<<<
                if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    float _posX = ((_m_iTmpi2 / _m_iPerUnitItemCount) * (wnd.itemTemplate.width + _m_vCalSpace.x)) + _m_vCalSpace.x + wnd.paddingForSide.x;
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , _posX
                        , ((RectTransform)_m_wTmpItem.wnd.transform).anchoredPosition.y);
                }
                else
                {

                    float _posY = -(((_m_iTmpi2 / _m_iPerUnitItemCount) * (wnd.itemTemplate.height + _m_vCalSpace.y)) + _m_vCalSpace.y + wnd.paddingForSide.x);
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , ((RectTransform)_m_wTmpItem.wnd.transform).anchoredPosition.x
                        , _posY);
                }

                //>>>再根据曲线及当前方向上的坐标值设置另一个坐标值<<<
                Vector2 _screenPos = RectTransformUtility.WorldToScreenPoint(MainCameraMono.instance.fullCanvas.worldCamera, _m_wTmpItem.rectTransform.position);
                //屏幕坐标转换到UGUI坐标
                Vector2 uiPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    wnd.curveAreaObj,
                    _screenPos,
                    MainCameraMono.instance.uiCamera,
                    out uiPos);

                // float _calcX = uiPos.x;  //最终相对坐标
                // float _calcY = uiPos.y;  //最终相对坐标
                float _calcX = wnd.curveAreaObj.rect.width / 2 + uiPos.x;  //最终相对坐标
                float _calcY = wnd.curveAreaObj.rect.height / 2 - uiPos.y;  //最终相对坐标
                float x = _calcX + _m_wTmpItem.wnd.width / 2f;   //增加item宽度的一半计算   TODO 因为ui上pivot配置的是0,1
                float y = _calcY + _m_wTmpItem.wnd.height / 2f;  //增加item高度的一半计算
                //设置显示对象位置
                if (wnd.layoutStyle == ALGUIListLayoutStyle.HORIZONTAL)
                {
                    
                    float proportion = wnd.curve.Evaluate(x / wnd.curveAreaObj.rect.width);   //最终比例
                    float deviation = (wnd.maxHeight - wnd.minHeight) * proportion + wnd.minHeight;    //最终高度
                    if (!wnd.isDefaultCurve)
                        deviation = -deviation;
                    float _posY = -(((_m_iTmpi2 % _m_iPerUnitItemCount) * (wnd.itemTemplate.height + _m_vCalSpace.y)) + _m_vCalSpace.y) + deviation;
                    //横向位置设置
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , ((RectTransform)_m_wTmpItem.wnd.transform).anchoredPosition.x
                        , _posY);
                }
                else
                {
                    float proportion = wnd.curve.Evaluate(y / wnd.curveAreaObj.rect.height);   //最终比例
                    float deviation = (wnd.maxHeight - wnd.minHeight) * proportion + wnd.minHeight;    //最终高度
                    if (wnd.isDefaultCurve)
                        deviation = -deviation;
                    float _posX = ((_m_iTmpi2 % _m_iPerUnitItemCount) * (wnd.itemTemplate.width + _m_vCalSpace.x)) + _m_vCalSpace.x + deviation;
                    //纵向位置设置
                    ALUGUICommon.setUIPos(
                        _m_wTmpItem.wnd.gameObject
                        , _posX
                        , ((RectTransform)_m_wTmpItem.wnd.transform).anchoredPosition.y);
                }

                //判断序号是否一致，并刷新显示
                if(_m_wTmpItem.itemIdx == _m_iTmpi2)
                {
                    //判断是否需要强制刷新，是则刷新显示项
                    if(isForceRefresh)
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
                return ;

            //实例化一个子窗口对象
            _T_ITEM_MONO itemWndObj = GameObject.Instantiate(wnd.itemTemplate) as _T_ITEM_MONO;
            if (null == itemWndObj)
                return ;

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
                return ;
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

            _T_ITEM_MONO wndMono = tmpWnd.wnd;
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
        protected abstract _T_ITEM_WND _createItemWnd(_T_ITEM_MONO _itemMono);
        /********************
         * 根据带入的窗口对象以及索引刷新相关的显示信息
         **/
        protected abstract void _refreshItemwnd(_T_ITEM_WND _itemMono, int _itemIdx);
    }
}