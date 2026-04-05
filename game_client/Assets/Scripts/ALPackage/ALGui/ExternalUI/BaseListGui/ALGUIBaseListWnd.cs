using System.Collections.Generic;
using UnityEngine;

#if AL_UNITY_GUI
namespace ALPackage
{
    public abstract class _AALGUIBaseListWnd<T> : ALGUIBaseWnd
    {
        private int _m_iShowDataCount;
        /** 子控件显示对象队列 */
        private List<_AALGUIBaseListItem<T>> _m_lChildItemList;

        /** 布局朝向参数 */
        private ALGUIListLayoutStyle _m_eLayoutStyle;
        /** 是否一行或一列只允许存在一个子控件 */
        private bool _m_bSingleItemPerLine;

        /** 子控件外围固定预留的显示区域参数 */
        private int _m_iTopBorder;
        private int _m_iBottomBorder;
        private int _m_iLeftBorder;
        private int _m_iRightBorder;

        /** 子控件间预留的显示区域参数 */
        private int _m_iItemsHorizontalSpaceWidth;
        private int _m_iItemsVerticalSpaceHeight;

        /** 显示区域相关参数 */
        private int _m_iHorizontalMaxItemCount;
        private int _m_iVerticalMaxItemCount;

        /** 子控件存放的窗口对象 */
        private ALGUIBaseWnd _m_wItemsContainer;

        /** 容器的滚动条样式 */
        /** 横向 */
        private ALSOGUIScrollBarStyle _m_bsHScrollBarStyle;
        /** 纵向 */
        private ALSOGUIScrollBarStyle _m_bsVScrollBarStyle;

        /** 滚动条对象 */
        private ALGUIBaseScrollBar _m_sbScrollBarWnd;
        private float _m_fScrollBarTopPos;

        public _AALGUIBaseListWnd(Rect _wndRect, ALGUIListLayoutStyle _style)
            : base(_wndRect)
        {
            _m_iShowDataCount = 0;
            _m_lChildItemList = new List<_AALGUIBaseListItem<T>>();

            _m_eLayoutStyle = _style;
            _m_bSingleItemPerLine = false;

            _m_bsHScrollBarStyle = null;
            _m_bsVScrollBarStyle = null;

            _m_sbScrollBarWnd = null;
            _m_fScrollBarTopPos = 0;

            //设置容器对象
            _m_wItemsContainer = new ALGUIBaseWnd();
            this.ALGUIRegChildWnd(_m_wItemsContainer);

            //注册窗口尺寸变更事件
            this.ALGUIRegOnSizeDelegate(OnResize);
        }
        public _AALGUIBaseListWnd(ALGUIWndPositionStyle _posStyle, ALGUIListLayoutStyle _style)
            : base(_posStyle)
        {
            _m_iShowDataCount = 0;
            _m_lChildItemList = new List<_AALGUIBaseListItem<T>>();

            _m_eLayoutStyle = _style;
            _m_bSingleItemPerLine = false;

            _m_bsHScrollBarStyle = null;
            _m_bsVScrollBarStyle = null;

            _m_sbScrollBarWnd = null;
            _m_fScrollBarTopPos = 0;

            //设置容器对象
            _m_wItemsContainer = new ALGUIBaseWnd();
            this.ALGUIRegChildWnd(_m_wItemsContainer);

            //注册窗口尺寸变更事件
            this.ALGUIRegOnSizeDelegate(OnResize);
        }

        public int dataCount
        {
            get { return _m_iShowDataCount; }
        }
        public ALGUIListLayoutStyle layoutStyle
        {
            get { return _m_eLayoutStyle; }
            set { _m_eLayoutStyle = value; }
        }

        public bool singleItemPerLine
        {
            get { return _m_bSingleItemPerLine; }
            set { _m_bSingleItemPerLine = value; }
        }

        public int topBorder
        {
            get { return _m_iTopBorder; }
            set { _m_iTopBorder = value; }
        }

        public int bottomBorder
        {
            get { return _m_iBottomBorder; }
            set { _m_iBottomBorder = value; }
        }

        public int leftBorder
        {
            get { return _m_iLeftBorder; }
            set { _m_iLeftBorder = value; }
        }

        public int rightBorder
        {
            get { return _m_iRightBorder; }
            set { _m_iRightBorder = value; }
        }

        public int itemsHorizontalSpaceWidth
        {
            get { return _m_iItemsHorizontalSpaceWidth; }
            set { _m_iItemsHorizontalSpaceWidth = value; }
        }

        public int itemsVerticalSpaceHeight
        {
            get { return _m_iItemsVerticalSpaceHeight; }
            set { _m_iItemsVerticalSpaceHeight = value; }
        }

        /*****************
         * 设置窗口显示区域的上限区域
         **/
        public void setViewRangeTopPos(float _viewTopPos)
        {
            _m_fScrollBarTopPos = _viewTopPos;

            //刷新所有对象的显示位置
            _refreshAllItemPos();
        }
        protected void _setViewRangeTopPos(float _totalRange, float _viewSize, float _viewTopPos)
        {
            //设置显示区域
            setViewRangeTopPos(_viewTopPos);
        }

        /********************
         * 设置显示数据的总数量
         **/
        public void setShowDataCount(int _dataCount)
        {
            _m_iShowDataCount = _dataCount;

            //重新规划显示区域和现实数量
            refreshLayout();
        }

        /*******************
         * 刷新所有控件信息
         **/
        public void refreshAllChildItems()
        {
            if (null == _m_lChildItemList)
                return;

            for (int i = 0; i < _m_lChildItemList.Count; i++)
            {
                _AALGUIBaseListItem<T> item = _m_lChildItemList[i];

                item.initItem(item.getData());
            }
        }

        /*******************
         * 刷新指定位置控件信息
         **/
        public void refreshChildItem(int _index)
        {
            if (null == _m_lChildItemList)
                return;

            for (int i = 0; i < _m_lChildItemList.Count; i++)
            {
                _AALGUIBaseListItem<T> listItem = _m_lChildItemList[i];
                if (null == listItem)
                    continue;

                if (listItem.dataIndex == _index)
                {
                    listItem.initItem();
                    return;
                }
            }
        }

        /************************
         * 刷新所有控件的位置
         **/
        public void refreshLayout()
        {
            _relayoutItems(wndRect);
        }

        /** 大小更改事件 */
        public void OnResize(ALGUIBaseWnd _wnd, Rect _oldRect, Rect _newRect)
        {
            //重新计算所有布局
            _relayoutItems(_newRect);
        }

        /********************
         * 重新布局所有子控件对象
         **/
        protected void _relayoutItems(Rect _wndRect)
        {
            //清除所有子窗口对象
            _clearChildItems();

            //判断本窗口最多容纳显示对象数量
            int maxShowCount = 0;

            //根据新的窗口区域计算子控件队列
            //计算高度情况下最多的子控件数量
            _m_iVerticalMaxItemCount = (int)(((_wndRect.height - topBorder - bottomBorder) + _m_iItemsVerticalSpaceHeight) / (getItemWndHeight() + _m_iItemsVerticalSpaceHeight));
            //最少为1
            if (_m_iVerticalMaxItemCount <= 0)
                _m_iVerticalMaxItemCount = 1;

            //计算一列可排布多少子控件
            _m_iHorizontalMaxItemCount = (int)(((_wndRect.width - leftBorder - rightBorder) + _m_iItemsHorizontalSpaceWidth) / (getItemWndWidth() + _m_iItemsHorizontalSpaceWidth));
            //最少为1
            if (_m_iHorizontalMaxItemCount <= 0)
                _m_iHorizontalMaxItemCount = 1;

            //判断排版方向
            if (ALGUIListLayoutStyle.HORIZONTAL == _m_eLayoutStyle)
            {
                //单控件居中模式直接设置对应行/列只可存在一个控件
                if (singleItemPerLine)
                    _m_iVerticalMaxItemCount = 1;

                //计算一个屏幕内最多可完整显示的数量
                maxShowCount = _m_iVerticalMaxItemCount * _m_iHorizontalMaxItemCount;
            }
            else
            {
                //单控件居中模式直接设置对应行/列只可存在一个控件
                if (singleItemPerLine)
                    _m_iHorizontalMaxItemCount = 1;

                //计算一个屏幕内最多可完整显示的数量
                maxShowCount = _m_iVerticalMaxItemCount * _m_iHorizontalMaxItemCount;
            }

            //根据可显示数量是否超出显示数量表明是否需要创建滑动条
            bool needScroll = (maxShowCount < dataCount);
            if (needScroll)
            {
                //根据窗口布局方向，放置不同风格的滑动条
                if (ALGUIListLayoutStyle.HORIZONTAL == _m_eLayoutStyle)
                {
                    //获取横向滚动条样式
                    ALSOGUIScrollBarStyle hScrollBarStyle = _m_bsHScrollBarStyle;
                    if (null == hScrollBarStyle)
                        hScrollBarStyle = ALGUIDefaultSetting.defaultHScrollBarStyle;

                    //设置滚动条位置样式
                    ALGUIWndPositionStyle scrollPosStyle = new ALGUIWndPositionStyle();
                    scrollPosStyle.x = 0;
                    scrollPosStyle.y = 0;
                    scrollPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_BOTTOM;
                    scrollPosStyle.width.lengthType = ALGUILengthType.PARENT_PERCENT;
                    scrollPosStyle.height.lengthType = ALGUILengthType.NUM;
                    scrollPosStyle.width.num = 100;
                    scrollPosStyle.height.num = hScrollBarStyle.scrollBarTexturePageSize;

                    //横向，在底部放置滚动条
                    _m_sbScrollBarWnd = new ALGUIBaseScrollBar(hScrollBarStyle, scrollPosStyle);

                    //注册到本窗口
                    ALGUIRegChildWnd(_m_sbScrollBarWnd);

                    //重新计算纵向可放置对象个数
                    _m_iVerticalMaxItemCount = (int)(((_wndRect.height - topBorder - bottomBorder - hScrollBarStyle.scrollBarTexturePageSize) + _m_iItemsVerticalSpaceHeight) / (getItemWndHeight() + _m_iItemsVerticalSpaceHeight));
                    //最少为1
                    if (_m_iVerticalMaxItemCount <= 0)
                        _m_iVerticalMaxItemCount = 1;

                    //重新计算容器显示范围
                    _m_wItemsContainer.positionStyle.x = leftBorder;
                    _m_wItemsContainer.positionStyle.y = topBorder;
                    _m_wItemsContainer.positionStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;
                    _m_wItemsContainer.positionStyle.width.lengthType = ALGUILengthType.NUM;
                    _m_wItemsContainer.positionStyle.width.num = _wndRect.width - leftBorder - rightBorder;
                    _m_wItemsContainer.positionStyle.height.lengthType = ALGUILengthType.NUM;
                    _m_wItemsContainer.positionStyle.height.num = _wndRect.height - topBorder - bottomBorder - hScrollBarStyle.scrollBarTexturePageSize;
                    //重置窗口位置
                    _m_wItemsContainer.ALGUIResetWndPos();

                    //计算总的需要显示的区域大小
                    int rowCount = (dataCount + _m_iVerticalMaxItemCount - 1) / _m_iVerticalMaxItemCount;

                    //计算显示区域宽度
                    float totalWidth = rowCount * (itemsHorizontalSpaceWidth + getItemWndWidth()) - itemsHorizontalSpaceWidth;

                    //注册滚动条事件，先注册事件，在设置浏览信息时，将可能调整到视野区域
                    _m_sbScrollBarWnd.viewDelegate += _setViewRangeTopPos;

                    //设置滑动条的现实区域数据
                    _m_sbScrollBarWnd.setCurViewInfo(totalWidth, _m_wItemsContainer.wndRect.width, _m_fScrollBarTopPos);
                    _m_sbScrollBarWnd.setChgViewSize(getItemWndWidth() + itemsHorizontalSpaceWidth);
                }
                else
                {
                    //获取横向滚动条样式
                    ALSOGUIScrollBarStyle vScrollBarStyle = _m_bsVScrollBarStyle;
                    if (null == vScrollBarStyle)
                        vScrollBarStyle = ALGUIDefaultSetting.defaultVScrollBarStyle;

                    //设置滚动条位置样式
                    ALGUIWndPositionStyle scrollPosStyle = new ALGUIWndPositionStyle();
                    scrollPosStyle.x = 0;
                    scrollPosStyle.y = 0;
                    scrollPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.RIGHT_TOP;
                    scrollPosStyle.width.lengthType = ALGUILengthType.NUM;
                    scrollPosStyle.height.lengthType = ALGUILengthType.PARENT_PERCENT;
                    scrollPosStyle.width.num = vScrollBarStyle.scrollBarTexturePageSize;
                    scrollPosStyle.height.num = 100;

                    //横向，在底部放置滚动条
                    _m_sbScrollBarWnd = new ALGUIBaseScrollBar(vScrollBarStyle, scrollPosStyle);

                    //注册到本窗口
                    ALGUIRegChildWnd(_m_sbScrollBarWnd);

                    //重新计算横向可放置对象个数
                    _m_iHorizontalMaxItemCount = (int)(((_wndRect.width - leftBorder - rightBorder - vScrollBarStyle.scrollBarTexturePageSize) + _m_iItemsHorizontalSpaceWidth) / (getItemWndWidth() + _m_iItemsHorizontalSpaceWidth));
                    //最少为1
                    if (_m_iHorizontalMaxItemCount <= 0)
                        _m_iHorizontalMaxItemCount = 1;

                    //重新计算容器显示范围
                    _m_wItemsContainer.positionStyle.x = leftBorder;
                    _m_wItemsContainer.positionStyle.y = topBorder;
                    _m_wItemsContainer.positionStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;
                    _m_wItemsContainer.positionStyle.width.lengthType = ALGUILengthType.NUM;
                    _m_wItemsContainer.positionStyle.width.num = _wndRect.width - leftBorder - rightBorder - vScrollBarStyle.scrollBarTexturePageSize;
                    _m_wItemsContainer.positionStyle.height.lengthType = ALGUILengthType.NUM;
                    _m_wItemsContainer.positionStyle.height.num = _wndRect.height - topBorder - bottomBorder;
                    //重置窗口位置
                    _m_wItemsContainer.ALGUIResetWndPos();

                    //计算总的需要显示的区域大小
                    int lineCount = (dataCount + _m_iHorizontalMaxItemCount - 1) / _m_iHorizontalMaxItemCount;

                    //计算显示区域宽度
                    float totalHeight = lineCount * (itemsVerticalSpaceHeight + getItemWndHeight()) - itemsVerticalSpaceHeight;

                    //注册滚动条事件，先注册事件，在设置浏览信息时，将可能调整到视野区域
                    _m_sbScrollBarWnd.viewDelegate += _setViewRangeTopPos;

                    //设置滑动条的现实区域数据
                    _m_sbScrollBarWnd.setCurViewInfo(totalHeight, _m_wItemsContainer.wndRect.height, _m_fScrollBarTopPos);
                    _m_sbScrollBarWnd.setChgViewSize(getItemWndHeight() + itemsVerticalSpaceHeight);
                }
            }
            else
            {
                //直接使用窗口大小设置显示区域内对象
                //重新计算容器显示范围
                _m_wItemsContainer.positionStyle.x = leftBorder;
                _m_wItemsContainer.positionStyle.y = topBorder;
                _m_wItemsContainer.positionStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;
                _m_wItemsContainer.positionStyle.width.lengthType = ALGUILengthType.NUM;
                _m_wItemsContainer.positionStyle.width.num = _wndRect.width - leftBorder - rightBorder;
                _m_wItemsContainer.positionStyle.height.lengthType = ALGUILengthType.NUM;
                _m_wItemsContainer.positionStyle.height.num = _wndRect.height - topBorder - bottomBorder;
                //重置窗口位置
                _m_wItemsContainer.ALGUIResetWndPos();

                //设置显示区域
                _m_fScrollBarTopPos = 0;
            }

            //判断排版方向
            if (ALGUIListLayoutStyle.HORIZONTAL == _m_eLayoutStyle)
            {
                //计算一共几列
                int rowCount = (dataCount + _m_iVerticalMaxItemCount - 1) / _m_iVerticalMaxItemCount;

                //当列数量超过上限，按照上限算
                if (rowCount > (_m_iHorizontalMaxItemCount + 2))
                {
                    rowCount = _m_iHorizontalMaxItemCount + 2;
                }

                //计算显示区域内可排布的子控件队列
                //此时使用rowCount作为列数量
                for (int n = 0; n < rowCount; n++)
                {
                    for (int m = 0; m < _m_iVerticalMaxItemCount; m++)
                    {
                        //创建控件放在指定位置上
                        _AALGUIBaseListItem<T> item = _createShowItem();

                        //计算对应控件坐标
                        item.positionStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;
                        item.positionStyle.width.lengthType = ALGUILengthType.NUM;
                        item.positionStyle.height.lengthType = ALGUILengthType.NUM;
                        item.positionStyle.width.num = getItemWndWidth();
                        item.positionStyle.height.num = getItemWndHeight();

                        //设置位置信息
                        item._rowIdx = n;
                        item._lineIdx = m;
                        //设置父对象
                        item._parent = this;

                        //将对象添加到容器中
                        _m_wItemsContainer.ALGUIRegChildWnd(item);

                        //添加到队列中
                        _m_lChildItemList.Add(item);
                    }
                }
            }
            else
            {
                //计算一共几行
                int lineCount = (dataCount + _m_iHorizontalMaxItemCount - 1) / _m_iHorizontalMaxItemCount;

                //当列数量超过上限，按照上限算
                if (lineCount > (_m_iVerticalMaxItemCount + 2))
                {
                    lineCount = _m_iVerticalMaxItemCount + 2;
                }

                //计算显示区域内可排布的子控件队列
                //此时使用rowCount作为列数量
                for (int n = 0; n < _m_iHorizontalMaxItemCount; n++)
                {
                    for (int m = 0; m < lineCount; m++)
                    {
                        //创建控件放在指定位置上
                        _AALGUIBaseListItem<T> item = _createShowItem();

                        //计算对应控件坐标
                        item.positionStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_TOP;
                        item.positionStyle.width.lengthType = ALGUILengthType.NUM;
                        item.positionStyle.height.lengthType = ALGUILengthType.NUM;
                        item.positionStyle.width.num = getItemWndWidth();
                        item.positionStyle.height.num = getItemWndHeight();

                        //设置位置信息
                        item._rowIdx = n;
                        item._lineIdx = m;
                        //设置父对象
                        item._parent = this;

                        //将对象添加到容器中
                        _m_wItemsContainer.ALGUIRegChildWnd(item);

                        //添加到队列中
                        _m_lChildItemList.Add(item);
                    }
                }
            }

            //重置所有对象的位置
            _refreshAllItemPos();
        }

        /*******************
         * 清除所有子控件
         **/
        protected void _clearChildItems()
        {
            if (null == _m_lChildItemList)
                return;

            for (int i = 0; i < _m_lChildItemList.Count; i++)
            {
                _AALGUIBaseListItem<T> item = _m_lChildItemList[i];

                //注销窗口对象
                _m_wItemsContainer.ALGUIUnregChildWnd(item);

                //注销对象
                item.ALGUIRelease();
            }

            //清空列表
            _m_lChildItemList.Clear();

            //移除滚动条对象
            if (null != _m_sbScrollBarWnd)
            {
                ALGUIUnregChildWnd(_m_sbScrollBarWnd);
                _m_sbScrollBarWnd.ALGUIRelease();
                _m_sbScrollBarWnd = null;
            }
        }

        /*******************
         * 重刷所有子对象的显示位置信息
         **/
        protected void _refreshAllItemPos()
        {
            //获取容器窗口大小
            float containerWidth = _m_wItemsContainer.wndRect.width;
            float containerHeight = _m_wItemsContainer.wndRect.height;

            //判断排版方向
            if (ALGUIListLayoutStyle.HORIZONTAL == _m_eLayoutStyle)
            {
                //计算第一个对象的列数量
                int firstItemRowCount = ((int)_m_fScrollBarTopPos) / (getItemWndWidth() + itemsHorizontalSpaceWidth);
                //计算第一个显示对象的X坐标
                float firstShowItemXPos = (firstItemRowCount * (getItemWndWidth() + itemsHorizontalSpaceWidth)) - _m_fScrollBarTopPos;

                /** 第一个物件的Y坐标 */
                int firstItemInRowYPos = 0;
                /** 纵向物件之间的间隔 */
                int verticalSpaceHeight = itemsVerticalSpaceHeight;

                if (_m_iVerticalMaxItemCount <= 1)
                {
                    //单行方式，重新计算Y起始坐标
                    firstItemInRowYPos = (int)((containerHeight - getItemWndHeight()) / 2);
                }
                else
                {
                    //非单行方式，重新计算纵向间隔
                    verticalSpaceHeight = (int)((containerHeight - (_m_iVerticalMaxItemCount * getItemWndHeight())) / (_m_iVerticalMaxItemCount - 1));
                }

                //设置对应控件的显示位置
                for (int i = 0; i < _m_lChildItemList.Count; i++)
                {
                    _AALGUIBaseListItem<T> listItem = _m_lChildItemList[i];
                    if (null == listItem)
                        continue;

                    //计算对象位置
                    float x = firstShowItemXPos + (listItem._rowIdx * (getItemWndWidth() + itemsHorizontalSpaceWidth));
                    float y = firstItemInRowYPos + (listItem._lineIdx * (getItemWndHeight() + verticalSpaceHeight));

                    listItem.positionStyle.x = x;
                    listItem.positionStyle.y = y;

                    //超出上限，隐藏对象
                    int itemIdx = listItem.dataIndex;
                    if (itemIdx >= _m_iShowDataCount)
                    {
                        listItem.hide();
                    }
                    else
                    {
                        T data = _getShowData(itemIdx);
                        //显示对象
                        listItem.show();

                        //初始化控件数据
                        listItem.initItem(data);
                    }

                    //重置位置
                    listItem.ALGUIResetWndPos();
                }
            }
            else
            {
                //计算第一个对象的行数量
                int firstItemLineCount = ((int)_m_fScrollBarTopPos) / (getItemWndHeight() + itemsVerticalSpaceHeight);
                //计算第一个显示对象的X坐标
                float firstShowItemYPos = (firstItemLineCount * (getItemWndHeight() + itemsVerticalSpaceHeight)) - _m_fScrollBarTopPos;

                /** 第一个物件的X坐标 */
                int firstItemInRowXPos = 0;
                /** 横向对象的间隔 */
                int horizontalSpaceWidth = itemsHorizontalSpaceWidth;

                if (_m_iHorizontalMaxItemCount <= 1)
                {
                    //单列方式，重新计算X起始坐标
                    firstItemInRowXPos = (int)((containerWidth - getItemWndWidth()) / 2);
                }
                else
                {
                    //非单列方式，重新计算横向间隔
                    horizontalSpaceWidth = (int)((containerWidth - (_m_iHorizontalMaxItemCount * getItemWndWidth())) / (_m_iHorizontalMaxItemCount - 1));
                }

                //设置对应控件的显示位置
                for (int i = 0; i < _m_lChildItemList.Count; i++)
                {
                    _AALGUIBaseListItem<T> listItem = _m_lChildItemList[i];
                    if (null == listItem)
                        continue;

                    //计算对象位置
                    float x = firstItemInRowXPos + (listItem._rowIdx * (getItemWndWidth() + horizontalSpaceWidth));
                    float y = firstShowItemYPos + (listItem._lineIdx * (getItemWndHeight() + itemsVerticalSpaceHeight));

                    listItem.positionStyle.x = x;
                    listItem.positionStyle.y = y;

                    //超出上限，隐藏对象
                    int itemIdx = listItem.dataIndex;
                    if (itemIdx >= _m_iShowDataCount)
                    {
                        listItem.hide();
                    }
                    else
                    {
                        T data = _getShowData(itemIdx);
                        //显示对象
                        listItem.show();

                        //初始化控件数据
                        listItem.initItem(data);
                    }

                    //重置位置
                    listItem.ALGUIResetWndPos();
                }
            }
        }

        /*****************
         * 获取当前显示对象的对应数据的序列号
         **/
        protected internal int _getShowItemIndex(int _rowIdx, int _lineIdx)
        {
            //判断排版方向
            if (ALGUIListLayoutStyle.HORIZONTAL == _m_eLayoutStyle)
            {
                //计算第一个对象的列数量
                int firstItemRowCount = ((int)_m_fScrollBarTopPos) / (getItemWndWidth() + itemsHorizontalSpaceWidth);

                //根据原先的列数量计算第一个对象的序号
                int firstIdx = firstItemRowCount * _m_iVerticalMaxItemCount;

                //计算对应位置显示对象的数据
                int idx = firstIdx + ((_rowIdx * _m_iVerticalMaxItemCount) + _lineIdx);

                return idx;
            }
            else
            {
                //计算第一个对象的行数量
                int firstItemLineCount = ((int)_m_fScrollBarTopPos) / (getItemWndHeight() + itemsVerticalSpaceHeight);

                //根据原先的列数量计算第一个对象的序号
                int firstIdx = firstItemLineCount * _m_iHorizontalMaxItemCount;

                //计算对应位置显示对象的数据
                int idx = firstIdx + ((_lineIdx * _m_iHorizontalMaxItemCount) + _rowIdx);

                return idx;
            }
        }

        /*************************
         * 获取子控件绘制尺寸
         **/
        public abstract int getItemWndWidth();
        public abstract int getItemWndHeight();

        /*****************
         * 获取需要显示的制定位置的显示对象数据
         **/
        protected internal abstract T _getShowData(int _index);

        /*****************
         * 创建显示的子控件对象
         **/
        protected abstract _AALGUIBaseListItem<T> _createShowItem();
    }
}

#endif
