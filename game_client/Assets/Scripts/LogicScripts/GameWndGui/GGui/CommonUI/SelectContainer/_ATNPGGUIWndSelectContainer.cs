using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{

    /// <summary>
    /// 选择滚动条Wnd基类
    /// </summary>
    /// <typeparam name="_T_ITEM_MONO"></typeparam>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _ATNPGGUIWndSelectContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        : _ANPGGUIBasicSubWndContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _ANPGGUIMonoSelectContainerItem
        where _T_CONTAINER_MONO : _ATNPGGUIMonoSelectContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATNPGGUIWndSelectContainerItem<_T_ITEM_MONO>
    {
        protected _ATNPGGUIWndSelectContainer(_T_CONTAINER_MONO _containerMono) : base(_containerMono)
        {
            initWnd();
        }
        private enum EScrollType
        {
            NONE,
            HORIZONTAL,
            VERTICAL,
        }

        public event Action<_T_ITEM_WND> onSelectItem;//选择item回调

        private List<_T_ITEM_WND> _m_lItemList;//子窗体列表，只存放显示的窗体
        private _T_ITEM_WND _m_wCurrentItem;//当前选中的item

        private ALCommonEnableTaskController _m_ftScrollRectMonitorTask;//每帧任务控制对象
        private ALCommonEnableTaskController _m_ftSelectedMoveTask;//每帧任务控制对象

        private RectTransform _m_rtContentRect;//item父节点RectTransform
        private RectTransform _m_rtViewPortRect;//视口RectTransform
        private bool _m_bIsDraging;//是否正在拖动
        private EScrollType _m_eScrollType;//滚动条类型
        private NPScrollRect _m_srScrollRect;//滚动条

        private ALRealTimeFloatFadeController _m_cContentPosController;//容器位置控制器
        private float _m_fCurContentPos;//当前item父节点坐标，方便计算
        private float _m_fViewPortSize;//视口大小，方便计算

        //占位go，确保在弹性ScrollRect下每个item都可以居中
        private GameObject firstGo;
        private GameObject lastGo;


        protected sealed override void _onWndInitDone()
        {
            _onWndInitDoneEx();

            if (wnd == null
                || wnd.itemContainer == null)
                return;

            if (!(wnd.scrollRect is NPScrollRect))
            {
                Debug.LogError($"【{GetType()} Error】:滚动条配置类型错误，需要{typeof(NPScrollRect)}");
                return;
            }

            _m_lItemList = new List<_T_ITEM_WND>();
            _m_bIsDraging = false;

            //初始化滚动条相关配置
            _m_eScrollType = EScrollType.NONE;
            _m_rtContentRect = wnd.itemContainer.GetComponent<RectTransform>();
            _m_srScrollRect = wnd.scrollRect as NPScrollRect;
            if (_m_srScrollRect != null)
            {
                _m_rtViewPortRect = _m_srScrollRect.viewport;
                _m_srScrollRect.onStartDragDelegate += _onBeginDrag;
                _m_srScrollRect.onEndDragDelegate += _onEndDrag;

                if (_m_srScrollRect.horizontal)
                {
                    _m_eScrollType = EScrollType.HORIZONTAL;
                }
                else if (_m_srScrollRect.vertical)
                {
                    _m_eScrollType = EScrollType.VERTICAL;
                }

                //创建占位物体
                if (_m_eScrollType != EScrollType.NONE
                    && _m_rtViewPortRect != null)
                {
                    switch (_m_eScrollType)
                    {
                        case EScrollType.HORIZONTAL:
                            _m_fViewPortSize = _m_rtViewPortRect.rect.width;
                            break;

                        case EScrollType.VERTICAL:
                            _m_fViewPortSize = _m_rtViewPortRect.rect.height;
                            break;
                    }
                    firstGo = _createFakeItem(_m_rtContentRect, "First", wnd.itemSize, wnd.itemSpacing, _m_fViewPortSize);
                    lastGo = _createFakeItem(_m_rtContentRect, "Last", wnd.itemSize, wnd.itemSpacing, _m_fViewPortSize);
                }
            }
            _m_cContentPosController = new ALRealTimeFloatFadeController(1f, 1f, 1f, 1f, 1f);

            ALUGUICommon.combineBtnClick(wnd.btnPre, _onClickBtnPre);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickBtnNext);
        }

        protected sealed override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            _m_ftScrollRectMonitorTask.setDisable();
            _m_ftSelectedMoveTask.setDisable();
            
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            _m_lItemList?.Clear();

            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_lItemList != null)
            {
                _m_lItemList.Clear();
                _m_lItemList = null;
            }

            if (_m_srScrollRect != null)
            {
                _m_srScrollRect.onStartDragDelegate -= _onBeginDrag;
                _m_srScrollRect.onEndDragDelegate -= _onEndDrag;
            }

            ALUnityCommon.releaseGameObj(firstGo);
            ALUnityCommon.releaseGameObj(lastGo);

            onSelectItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnPre, _onClickBtnPre);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickBtnNext);

            _onDiscardEx();
        }

        protected sealed override void _onAddItemWnd(_T_ITEM_WND _itemWnd)
        {
            if (_itemWnd == null)
                return;

            _itemWnd.onClick += _onClickItem;
            _m_lItemList?.Add(_itemWnd);
            _onAddItemWndEx(_itemWnd);
            base._onAddItemWnd(_itemWnd);
        }

        protected sealed override void _discardItem(_T_ITEM_WND _itemWnd)
        {
            if (_itemWnd == null)
                return;

            //基类没有_onRemove方法，与add对应的就是discard
            _itemWnd.onClick -= _onClickItem;
            _m_lItemList?.Remove(_itemWnd);
            _discardItemEx(_itemWnd);
            base._discardItem(_itemWnd);
        }

        #region 子类窗体相关
        protected virtual void _onShowWndEx() { }
        protected virtual void _onHideWndEx() { }
        protected virtual void _onResetEx() { }
        protected virtual void _onDiscardEx() { }
        protected virtual void _onWndInitDoneEx() { }
        protected virtual void _onAddItemWndEx(_T_ITEM_WND _itemWnd) { }
        protected virtual void _discardItemEx(_T_ITEM_WND _itemWnd) { }
        #endregion


        #region 点击事件

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="_eventData"></param>
        private void _onBeginDrag(PointerEventData _eventData)
        {
            _m_bIsDraging = true;

            //开启任务检测移动状态，只有在拖拽结束才有可能需要开启任务
            _initScrollRectMonitorTask();
        }

        /// <summary>
        /// 结束拖拽
        /// </summary>
        /// <param name="_eventData"></param>
        private void _onEndDrag(PointerEventData _eventData)
        {
            _m_bIsDraging = false;
        }

        /// <summary>
        /// 点击item
        /// </summary>
        private void _onClickItem(int _index)
        {
            //终止监控任务，设置移动速度为0
            _m_ftScrollRectMonitorTask.setDisable();

            //设置选中对象
            _setCurrentItem(_index);

            //开启移动任务
            _moveToSelectedItem();
        }

        /// <summary>
        /// 点击跳转前一个item
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnPre(GameObject _go)
        {
            if (_m_wCurrentItem == null)
                return;

            scrollMoveTo(_m_wCurrentItem.itemIndex - 1);
        }

        /// <summary>
        /// 点击跳转后一个item
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnNext(GameObject _go)
        {
            if (_m_wCurrentItem == null)
                return;

            scrollMoveTo(_m_wCurrentItem.itemIndex + 1);
        }

        #endregion


        #region 任务

        /// <summary>
        /// 初始化每帧任务
        /// </summary>
        private void _initScrollRectMonitorTask()
        {
            //开始监控的时候需要将移动任务也关闭
            _m_ftScrollRectMonitorTask.setDisable();
            _m_ftSelectedMoveTask.setDisable();

            _m_ftScrollRectMonitorTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_scrollRectMoveMonitor);
        }
        private void _initSelectedItemMoveTask()
        {
            _m_ftSelectedMoveTask.setDisable();
            _m_ftSelectedMoveTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_moveSelectItemTick);
        }

        /// <summary>
        /// 在停止拖拽之后检测ScrollRect移动状态的监控任务
        /// </summary>
        private void _scrollRectMoveMonitor()
        {
            if (wnd == null
                || _m_rtContentRect == null
                || _m_lItemList == null)
                return;

            //刷新当前容器坐标
            _refreshContentPos();

            //判断是否还在惯性运动
            //这里注意 velocity 的单位是UI中的分辨率像素。一般小于1即会变为静止
            bool isInteriaMoving = (Mathf.Abs(_m_srScrollRect.velocity.x) > wnd.minScrollMoveSpeed || Mathf.Abs(_m_srScrollRect.velocity.y) > wnd.minScrollMoveSpeed);

            //刷新每个对象尺寸
            _refreshItemScale();

            //刷新当前选中的对象
            _setCurrentItem(_findCenterItemIndex());

            //如果还在拖拽或还在移动则不做后续处理
            if (_m_bIsDraging || isInteriaMoving)
                return;

            //终止监控任务
            _m_ftScrollRectMonitorTask.setDisable();

            //不在移动状态时，判断本对象选中item并开启居中操作
            _moveToSelectedItem();
        }

        /// <summary>
        /// 在将选中对象移动到中心的时候调用的移动Tick任务
        /// </summary>
        private void _moveSelectItemTick()
        {
            if (wnd == null
                || _m_rtContentRect == null
                || _m_lItemList == null)
                return;

            //刷新当前容器坐标
            _refreshContentPos();

            //刷新每个对象尺寸
            _refreshItemScale();

            //调用移动处理
            if (_moveToCurrentItem())
            {
                //如果已经移到到位置，则终止移动任务
                _m_ftSelectedMoveTask.setDisable();
                return;
            }
        }

        /// <summary>
        /// 检查并开启移动对象的任务
        /// </summary>
        protected void _moveToSelectedItem()
        {
            //设置速度为0
            if (_m_srScrollRect != null)
                _m_srScrollRect.velocity = Vector2.zero;

            //刷新位置信息
            _refreshPosController();

            //开启每帧的自动移动操作
            _initSelectedItemMoveTask();
        }

        /// <summary>
        /// 刷新当前每个Item尺寸
        /// </summary>
        protected void _refreshItemScale()
        {
            //计算每个Item的缩放
            _T_ITEM_WND itemWnd = null;
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                itemWnd = _m_lItemList[i];
                if (itemWnd == null)
                    return;

                itemWnd.setScale(_getItemScale(itemWnd.centerPos));
            }
        }

        #endregion


        #region 滚动条相关

        /// <summary>
        /// 获取缩放
        /// </summary>
        /// <param name="_centerPos"></param>
        /// <returns></returns>
        private Vector3 _getItemScale(float _centerPos)
        {
            if (wnd == null
                || wnd.scaleCurve == null
                || _m_fViewPortSize == 0f)
                return Vector3.one;

            //计算当前容器位置与item居中位置的偏移量
            float offset = _m_fCurContentPos - _centerPos;

            //如果完全超出ViewPort范围，重置缩放为1
            if (Mathf.Abs(offset) + wnd.itemSize / 2 > _m_fViewPortSize)
                return Vector3.one;

            //计算在曲线上的坐标，超出0和1的部分视为0和1
            float offsetPer = offset / _m_fViewPortSize;
            float curveX = 0.5f + offsetPer;
            if (curveX < 0f)
                curveX = 0f;
            else if (curveX > 1f)
                curveX = 1f;

            //获取曲线的值
            float value = wnd.scaleCurve.Evaluate(curveX);
            return new Vector3(value, value, 1);
        }

        /// <summary>
        /// 创建占位go
        /// </summary>
        /// <param name="_parent">父节点</param>
        /// <param name="_name">名字</param>
        /// <param name="_itemSize">其他item的大小</param>
        /// <param name="_spacing">item间距</param>
        /// <param name="_contentSize">容器大小</param>
        /// <returns></returns>
        private GameObject _createFakeItem(Transform _parent, string _name, float _itemSize, float _spacing, float _contentSize)
        {
            GameObject go = new GameObject();
            go.name = _name;
            go.transform.SetParent(_parent);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            RectTransform rect = go.AddComponent<RectTransform>();
            float width = 0f;
            float height = 0f;
            switch (_m_eScrollType)
            {
                case EScrollType.HORIZONTAL:
                    width = (_contentSize - _itemSize) / 2 - _spacing;
                    break;

                case EScrollType.VERTICAL:
                    height = (_contentSize - _itemSize) / 2 - _spacing;
                    break;
            }
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            return go;
        }

        /// <summary>
        /// 查找距离中心最近的item下标
        /// </summary>
        /// <returns></returns>
        private int _findCenterItemIndex()
        {
            if (_m_lItemList == null)
                return -1;

            int index = -1;
            _T_ITEM_WND item = null;
            float itemOffset = float.MinValue;//item中心偏差
            float minOffset = float.MaxValue;//当前最小item中心偏差

            //计算此时每个item的偏差
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                item = _m_lItemList[i];
                if (item == null)
                    continue;

                itemOffset = _getCenterPosOffset(item.centerPos);

                //偏差开始增大，说明已经找到距离中心最近的item
                if (minOffset < itemOffset)
                    break;

                //更新最小偏差
                minOffset = itemOffset;
                index = i;
            }

            return index;
        }

        /// <summary>
        /// 移动item父节点到当前item位置
        /// 返回是否成功移动到位置
        /// </summary>
        private bool _moveToCurrentItem()
        {
            if (_m_rtContentRect == null
                || _m_cContentPosController == null)
                return true;

            //插值移动item父节点
            switch (_m_eScrollType)
            {
                case EScrollType.HORIZONTAL:
                    _m_rtContentRect.anchoredPosition = new Vector2(_m_cContentPosController.curValue, _m_rtContentRect.anchoredPosition.y);
                    break;

                case EScrollType.VERTICAL:
                    _m_rtContentRect.anchoredPosition = new Vector2(_m_rtContentRect.anchoredPosition.x, _m_cContentPosController.curValue);
                    break;
            }

            return _m_cContentPosController.isDone;
        }

        /// <summary>
        /// 移动item父节点到当前item位置
        /// </summary>
        private void _setContentToCurrentItemPos()
        {
            if (_m_rtContentRect == null
                || _m_wCurrentItem == null)
                return ;

            _m_srScrollRect.StopMovement();// = Vector2.zero;
            //插值移动item父节点
            switch (_m_eScrollType)
            {
                case EScrollType.HORIZONTAL:
                    _m_rtContentRect.anchoredPosition = new Vector2(_m_wCurrentItem.centerPos, _m_rtContentRect.anchoredPosition.y);
                    break;

                case EScrollType.VERTICAL:
                    _m_rtContentRect.anchoredPosition = new Vector2(_m_rtContentRect.anchoredPosition.x, _m_wCurrentItem.centerPos);
                    break;
            }

            return ;
        }
        
        /// <summary>
        /// 重置控制器
        /// </summary>
        private void _refreshPosController()
        {
            if (wnd == null
                || _m_wCurrentItem == null
                || _m_cContentPosController == null)
                return;

            _m_cContentPosController.resetVariables(_m_fCurContentPos, _m_wCurrentItem.centerPos, 0, wnd.centeringTotalTime, wnd.centeringTotalTime * wnd.centeringAccTimeScale);
        }

        /// <summary>
        /// 刷新当前容器坐标
        /// </summary>
        private void _refreshContentPos()
        {
            switch (_m_eScrollType)
            {
                case EScrollType.HORIZONTAL:
                    _m_fCurContentPos = _m_rtContentRect.anchoredPosition.x;
                    break;

                case EScrollType.VERTICAL:
                    _m_fCurContentPos = _m_rtContentRect.anchoredPosition.y;
                    break;
            }
        }

        /// <summary>
        /// 更新item坐标参数
        /// </summary>
        private void _refreshItemPos()
        {
            if (wnd == null
                || _m_lItemList == null
                || _m_lItemList.Count <= 0
                || _m_lItemList[0] == null)
                return;

            //计算每个item居中时，对应父节点的位置
            void setItemScrollParam(Func<_T_ITEM_WND, float> _posSelector)
            {
                if (_posSelector == null)
                    return;

                float startPos = 0;
                _T_ITEM_WND itemWnd = null;
                startPos = _posSelector(_m_lItemList[0]);
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    itemWnd = _m_lItemList[i];
                    if (itemWnd == null)
                        continue;

                    itemWnd.setScrollParam(i, startPos - _posSelector(itemWnd));
                }
            }

            //水平或垂直不同处理
            switch (_m_eScrollType)
            {
                case EScrollType.HORIZONTAL:
                    setItemScrollParam((_item) =>
                    {
                        if (_item.rectTransform == null)
                            return 0f;
                        return _item.rectTransform.anchoredPosition.x;
                    });
                    break;

                case EScrollType.VERTICAL:
                    setItemScrollParam((_item) =>
                    {
                        if (_item.rectTransform == null)
                            return 0f;
                        return _item.rectTransform.anchoredPosition.y;
                    });
                    break;
            }
        }

        /// <summary>
        /// 设置当前item
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_notifySelect"></param>
        private void _setCurrentItem(int _index, bool _notifySelect = true)
        {
            //重复设置无效
            if (_m_wCurrentItem != null && _m_wCurrentItem.itemIndex == _index)
                return;

            if (_m_lItemList == null
                || _index < 0
                || _index >= _m_lItemList.Count)
                return;

            //切换选择item，并触发选择item事件
            _m_wCurrentItem?.setSelected(false);
            _m_wCurrentItem = _m_lItemList[_index];
            _m_wCurrentItem?.setSelected(true);

            //是否通知选择
            if (_notifySelect)
                onSelectItem?.Invoke(_m_wCurrentItem);

            //刷新按钮显示
            _refreshBtnShow();
        }

        /// <summary>
        /// 计算偏差值
        /// </summary>
        /// <param name="_pos"></param>
        /// <returns></returns>
        private float _getCenterPosOffset(float _pos)
        {
            return Mathf.Abs(_pos - _m_fCurContentPos);
        }

        /// <summary>
        /// 刷新按钮显示
        /// </summary>
        private void _refreshBtnShow()
        {
            if (_m_wCurrentItem == null
                || wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.btnPre, _m_wCurrentItem.itemIndex > 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, _m_lItemList != null && _m_wCurrentItem.itemIndex < _m_lItemList.Count - 1);
        }

        #endregion


        #region 功能方法

        /// <summary>
        /// 设置显示内容，子类可以不使用这个方法设置显示，但要调用refreshLayout刷新布局
        /// </summary>
        /// <typeparam name="_T_ITEM_DATA"></typeparam>
        /// <param name="_dataList"></param>
        /// <param name="_setDataAction"></param>
        public void setShowData<_T_ITEM_DATA>(List<_T_ITEM_DATA> _dataList, Action<_T_ITEM_WND, _T_ITEM_DATA> _setDataAction)
        {
            if (_dataList == null
                || _m_lItemList == null)
                return;

            _T_ITEM_WND itemWnd = null;
            _T_ITEM_DATA itemData = default(_T_ITEM_DATA);
            int count = 0;
            //遍历数据
            for (int i = 0; i < _dataList.Count; i++)
            {
                itemData = _dataList[i];
                if (null == itemData)
                    continue;

                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    //添加到_m_lItemList中不放在这处理，放在_onAddItemWnd中处理，保证子类也可以自定义方法设置显示数据
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                }
                //如果容器个数足够，则取出
                else
                {
                    itemWnd = _m_lItemList[i];

                }

                //刷新子窗体显示
                itemWnd.setSelected(false);
                itemWnd.showWnd();
                _setDataAction?.Invoke(itemWnd, itemData);

                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                //从_m_lItemList中移除不放在这处理，放在_onDiscardItem中处理，保证子类也可以自定义方法设置显示数据
                removeItemWnd(_m_lItemList[j]);
            }

            //设置显示数据完成
            refreshLayout();
        }

        /// <summary>
        /// 滚动到指定item下标处
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_isNotify">是否通知选择</param>
        public void scrollMoveTo(int _index, bool _isNotify = true)
        {
            _setCurrentItem(_index, _isNotify);
            _moveToSelectedItem();
        }

        /// <summary>
        /// 刷新布局，调用的时候要保证物体处于active
        /// </summary>
        public void refreshLayout(bool _setStartIndex = false, int _index = 0)
        {
            //占位go移动到头尾
            if (firstGo != null && firstGo.transform != null)
            {
                firstGo.transform.SetAsFirstSibling();
            }
            if (lastGo != null && lastGo.transform != null)
            {
                lastGo.transform.SetAsLastSibling();
            }

            //强制刷新一次布局，保证后续坐标计算正确
            LayoutRebuilder.ForceRebuildLayoutImmediate(_m_rtContentRect);

            //刷新当前容器坐标
            _refreshContentPos();

            //更新item坐标参数
            _refreshItemPos();

            //刷新每个对象尺寸
            _refreshItemScale();

            if (_setStartIndex)
            {
                _setCurrentItem(_index, false);
                _setContentToCurrentItemPos();
            }
            else
            {
                
                //刷新当前选中的对象
                _m_wCurrentItem = null;
                _setCurrentItem(_findCenterItemIndex());
            }
        }

        #endregion
    }
}
