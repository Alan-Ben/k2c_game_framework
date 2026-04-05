using UnityEngine;
using System.Collections;

#if AL_UNITY_GUI
namespace ALPackage
{
    /***********************
     * the basic scroll bar
     **/
    public class ALGUIBaseScrollBar : ALGUIBaseWnd
    {
        /** 处理滑动块位置变更的事件回调函数 */
        public delegate void ScrollBarViewChg(float _totalRange, float _viewSize, float _viewTopPos);

        /** the scroll bar's total range size */
        private float _m_fTotalRange;
        /** current view top position in the range */
        private float _m_fCurViewTopPos;
        /** current view size, as the range size */
        private float _m_fCurViewSize;
        /** 一次点击上下按钮切换的视野区域范围 */
        private float _m_fChgViewSize;

        /** scroll bar style obj */
        private ALSOGUIScrollBarStyle _m_sScrollBarStyle;
        /** the back ground draw object */
        private SquaredTextureObj _m_toBackGroundDrawObj;

        /** the top button item object */
        private ALGUIBaseScrollBarBtnItem _m_biTopBtnItem;
        /** the bottom button item object */
        private ALGUIBaseScrollBarBtnItem _m_biBottomBtnItem;
        /** the drag button item object */
        private ALGUIBaseScrollBarDragBtn _m_dbDragBtnItem;

        /** 显示区域变更的相应函数 */
        private ScrollBarViewChg _m_dViewChgDelegate;

        /*******************
         * init function
         **/
        public ALGUIBaseScrollBar(ALSOGUIScrollBarStyle _style, ALGUIWndPositionStyle _posStyle)
            : base(_posStyle)
        {
            //显示区域信息
            _m_fTotalRange = 0;
            _m_fCurViewTopPos = 0;
            _m_fCurViewSize = 0;
            _m_fChgViewSize = 0;

            //初始化相关控件
            _initScrollBarByStyle(_style);

            ALGUIWndPrintActionDelegate = OnPain;
        }

        /****************
         * 显示区域变更回调处理
         **/
        public ScrollBarViewChg viewDelegate
        {
            get
            {
                return _m_dViewChgDelegate;
            }
            set
            {
                _m_dViewChgDelegate = value;
            }
        }

        /******************
         * only draw the back ground picture
         **/
        public void OnPain(ALGUIBaseWnd _wnd)
        {
            //绘制图片
            if (null != _m_toBackGroundDrawObj)
                _m_toBackGroundDrawObj.GUIDraw(new Rect(0, 0, width, height));
        }

        /***************
         * 设置显示区域信息
         **/
        public void setViewTotalRange(float _totalRange)
        {
            _m_fTotalRange = _totalRange;

            //检查当前显示区域信息是否正确
            _checkCurViewTopPosEnable();

            //刷新显示信息
            _refreshDragBtnPos();

            //调用显示区域变更事件触发函数
            if (null != viewDelegate)
                viewDelegate(_m_fTotalRange, _m_fCurViewSize, _m_fCurViewTopPos);
        }
        public void setCurViewAreaTopPos(float _topPos)
        {
            _m_fCurViewTopPos = _topPos;
            if (_m_fCurViewTopPos <= 0)
                _m_fCurViewTopPos = 0;

            //检查当前显示区域信息是否正确
            _checkCurViewTopPosEnable();

            //刷新显示信息
            _refreshDragBtnPos();

            //调用显示区域变更事件触发函数
            if (null != viewDelegate)
                viewDelegate(_m_fTotalRange, _m_fCurViewSize, _m_fCurViewTopPos);
        }
        public void setCurViewSize(float _curViewSize)
        {
            _m_fCurViewSize = _curViewSize;

            //检查当前显示区域信息是否正确
            _checkCurViewTopPosEnable();

            //刷新显示信息
            _refreshDragBtnPos();

            //调用显示区域变更事件触发函数
            if (null != viewDelegate)
                viewDelegate(_m_fTotalRange, _m_fCurViewSize, _m_fCurViewTopPos);
        }
        public void setCurViewInfo(float _totalRange, float _curViewSize)
        {
            _m_fTotalRange = _totalRange;
            _m_fCurViewSize = _curViewSize;

            //检查当前显示区域信息是否正确
            _checkCurViewTopPosEnable();

            //刷新显示信息
            _refreshDragBtnPos();

            //调用显示区域变更事件触发函数
            if (null != viewDelegate)
                viewDelegate(_m_fTotalRange, _m_fCurViewSize, _m_fCurViewTopPos);
        }
        public void setCurViewInfo(float _totalRange, float _curViewSize, float _curViewTopPos)
        {
            _m_fTotalRange = _totalRange;
            _m_fCurViewSize = _curViewSize;
            _m_fCurViewTopPos = _curViewTopPos;

            //检查当前显示区域信息是否正确
            _checkCurViewTopPosEnable();

            //刷新显示信息
            _refreshDragBtnPos();

            //调用显示区域变更事件触发函数
            if (null != viewDelegate)
                viewDelegate(_m_fTotalRange, _m_fCurViewSize, _m_fCurViewTopPos);
        }

        /******************
         * 点击按钮切换视野的区域
         **/
        public void setChgViewSize(float _chgViewSize)
        {
            _m_fChgViewSize = _chgViewSize;
        }

        /**********************
         * onsize event function
         **/
        protected void _onSizeEvent(ALGUIBaseWnd _wnd, Rect _oldRect, Rect _newRect)
        {
            //窗口大小改变时，重置滑动块位置信息
            _refreshDragBtnPos();
        }

        /***************
         * 根据当前滑动块位置设置当前显示区域信息
         **/
        protected void _refreshCurViewPos()
        {
            float dragPos = 0;
            float radioPos = 0;
            if (_m_sScrollBarStyle.scrollBarDirection == EALGUIScrollBarDirection.VERTICAL)
            {
                dragPos = _m_dbDragBtnItem.wndRect.y - _m_sScrollBarStyle.topBtnSize;

                //计算对应位置所在的比例位置
                radioPos = dragPos / (height - _m_sScrollBarStyle.topBtnSize - _m_sScrollBarStyle.bottomBtnSize - _m_dbDragBtnItem.positionStyle.height.num);
            }
            else
            {
                dragPos = _m_dbDragBtnItem.wndRect.x - _m_sScrollBarStyle.topBtnSize;

                //计算对应位置所在的比例位置
                radioPos = dragPos / (width - _m_sScrollBarStyle.topBtnSize - _m_sScrollBarStyle.bottomBtnSize - _m_dbDragBtnItem.positionStyle.width.num);
            }

            //超出最大比例为1
            if (1 - radioPos < 0.000001f)
            {
                radioPos = 1;
                _m_biBottomBtnItem.disable();
            }
            else
                _m_biBottomBtnItem.enable();

            //到顶设置无法向上
            if (0 == radioPos)
                _m_biTopBtnItem.disable();
            else
                _m_biTopBtnItem.enable();

            //根据位置比例计算实际显示的位置信息
            _m_fCurViewTopPos = (_m_fTotalRange - _m_fCurViewSize) * radioPos;

            //调用显示区域变更事件触发函数
            if (null != viewDelegate)
                viewDelegate(_m_fTotalRange, _m_fCurViewSize, _m_fCurViewTopPos);
        }

        /***************
         * 根据当前显示区域的总大小以及显示区域位置，刷新拖动滑块位置
         **/
        protected void _refreshDragBtnPos()
        {
            //判断是否显示区域超出总大小
            if (_m_fCurViewSize >= _m_fTotalRange)
            {
                //设置滑块不可见
                _m_dbDragBtnItem.hide();

                //设置上下按钮无效
                _m_biTopBtnItem.disable();
                _m_biBottomBtnItem.disable();

                return;
            }

            //设置滑块可见
            _m_dbDragBtnItem.show();

            if (_m_sScrollBarStyle.scrollBarDirection == EALGUIScrollBarDirection.VERTICAL)
            {
                //重新计算滑块大小
                float dragSizeRadio = _m_fCurViewSize / _m_fTotalRange;
                float dragSize = dragSizeRadio * (height - _m_sScrollBarStyle.topBtnSize - _m_sScrollBarStyle.bottomBtnSize);
                if (dragSize < (_m_sScrollBarStyle.dragBtnTopSize + _m_sScrollBarStyle.dragBtnBottomSize))
                    dragSize = (_m_sScrollBarStyle.dragBtnTopSize + _m_sScrollBarStyle.dragBtnBottomSize);

                //设置滑块大小
                _m_dbDragBtnItem.positionStyle.height.num = dragSize;

                if (0 == _m_fTotalRange)
                {
                    //无区域时，位置为0
                    _m_dbDragBtnItem.positionStyle.y = _m_sScrollBarStyle.topBtnSize;
                    //上下都无效
                    _m_biBottomBtnItem.disable();
                    _m_biTopBtnItem.disable();
                }
                else
                {
                    float posRadio = _m_fCurViewTopPos / (_m_fTotalRange - _m_fCurViewSize);
                    //超出最大比例为1
                    if (1 - posRadio < 0.000001f)
                    {
                        posRadio = 1;
                        _m_biBottomBtnItem.disable();
                    }
                    else
                        _m_biBottomBtnItem.enable();

                    //到顶设置无法向上
                    if (0 == posRadio)
                        _m_biTopBtnItem.disable();
                    else
                        _m_biTopBtnItem.enable();

                    //计算所在位置
                    float dragBtnPosY = (height - _m_sScrollBarStyle.topBtnSize - _m_sScrollBarStyle.bottomBtnSize - _m_dbDragBtnItem.positionStyle.height.num) * posRadio;

                    _m_dbDragBtnItem.positionStyle.y = _m_sScrollBarStyle.topBtnSize + dragBtnPosY;
                }
            }
            else
            {
                //重新计算滑块大小
                float dragSizeRadio = _m_fCurViewSize / _m_fTotalRange;
                float dragSize = dragSizeRadio * (width - _m_sScrollBarStyle.topBtnSize - _m_sScrollBarStyle.bottomBtnSize);
                if (dragSize < (_m_sScrollBarStyle.dragBtnTopSize + _m_sScrollBarStyle.dragBtnBottomSize))
                    dragSize = (_m_sScrollBarStyle.dragBtnTopSize + _m_sScrollBarStyle.dragBtnBottomSize);

                //设置滑块大小
                _m_dbDragBtnItem.positionStyle.width.num = dragSize;

                if (0 == _m_fTotalRange)
                {
                    //无区域时，位置为0
                    _m_dbDragBtnItem.positionStyle.x = _m_sScrollBarStyle.topBtnSize;
                    //上下都无效
                    _m_biBottomBtnItem.disable();
                    _m_biTopBtnItem.disable();
                }
                else
                {
                    float posRadio = _m_fCurViewTopPos / (_m_fTotalRange - _m_fCurViewSize);
                    //超出最大比例为1
                    if (posRadio >= 1)
                    {
                        posRadio = 1;
                        _m_biBottomBtnItem.disable();
                    }
                    else
                        _m_biBottomBtnItem.enable();

                    //到顶设置无法向上
                    if (0 == posRadio)
                        _m_biTopBtnItem.disable();
                    else
                        _m_biTopBtnItem.enable();

                    //计算所在位置
                    float dragBtnPosX = (width - _m_sScrollBarStyle.topBtnSize - _m_sScrollBarStyle.bottomBtnSize - _m_dbDragBtnItem.positionStyle.width.num) * posRadio;

                    _m_dbDragBtnItem.positionStyle.x = _m_sScrollBarStyle.topBtnSize + dragBtnPosX;
                }
            }

            //刷新窗口位置
            _m_dbDragBtnItem.ALGUIResetWndPos();
        }

        /********************
         * 处理滑动块的拖动操作的特殊操作函数
         **/
        protected void _dealDragBtnDragFunc(ALGUIBaseWnd _dragWnd, Vector2 _mouseMoveVector, Vector2 _mousePosToWnd)
        {
            if (null == _dragWnd)
                return;

            //move the wnd
            if (_m_sScrollBarStyle.scrollBarDirection == EALGUIScrollBarDirection.VERTICAL)
            {
                float minYMove = _m_sScrollBarStyle.topBtnSize - _m_dbDragBtnItem.positionStyle.y;
                float maxYMove = height - _m_dbDragBtnItem.positionStyle.height.num - _m_sScrollBarStyle.bottomBtnSize - _m_dbDragBtnItem.positionStyle.y;

                float moveY = _mouseMoveVector.y;
                if (moveY < minYMove)
                    moveY = minYMove;
                if (moveY > maxYMove)
                    moveY = maxYMove;

                _dragWnd.ALGUIMoveWnd(new Vector2(0, moveY));
            }
            else
            {
                float minXMove = _m_sScrollBarStyle.topBtnSize - _m_dbDragBtnItem.positionStyle.x;
                float maxXMove = width - _m_dbDragBtnItem.positionStyle.width.num - _m_sScrollBarStyle.bottomBtnSize - _m_dbDragBtnItem.positionStyle.x;

                float moveX = _mouseMoveVector.x;
                if (moveX < minXMove)
                    moveX = minXMove;
                if (moveX > maxXMove)
                    moveX = maxXMove;

                _dragWnd.ALGUIMoveWnd(new Vector2(moveX, 0));
            }

            //移动滑块后，根据滑块位置设置显示区域位置
            _refreshCurViewPos();
        }

        /*******************
         * the function to paint the top button
         **/
        protected internal void _onPainTopBtn(ALGUIBaseScrollBarBtnItem _item)
        {
            if (!_item.isEnable)
            {
                //disable
                _m_sScrollBarStyle.drawTopBtn(_m_sScrollBarStyle.disablePartIdx, _item);
            }
            else if (_item.isStat(ALGUIBaseMouseStat.OVER))
            {
                //mouse over
                _m_sScrollBarStyle.drawTopBtn(_m_sScrollBarStyle.mouseOverPartIdx, _item);
            }
            else if (_item.isStat(ALGUIBaseMouseStat.DOWN))
            {
                //mouse down
                _m_sScrollBarStyle.drawTopBtn(_m_sScrollBarStyle.mouseDownPartIdx, _item);
            }
            else
            {
                //normal
                _m_sScrollBarStyle.drawTopBtn(_m_sScrollBarStyle.normalPartIdx, _item);
            }
        }

        /*******************
         * the function to paint the bottom button
         **/
        protected internal void _onPainBottomBtn(ALGUIBaseScrollBarBtnItem _item)
        {
            if (!_item.isEnable)
            {
                //disable
                _m_sScrollBarStyle.drawBottomBtn(_m_sScrollBarStyle.disablePartIdx, _item);
            }
            else if (_item.isStat(ALGUIBaseMouseStat.OVER))
            {
                //mouse over
                _m_sScrollBarStyle.drawBottomBtn(_m_sScrollBarStyle.mouseOverPartIdx, _item);
            }
            else if (_item.isStat(ALGUIBaseMouseStat.DOWN))
            {
                //mouse down
                _m_sScrollBarStyle.drawBottomBtn(_m_sScrollBarStyle.mouseDownPartIdx, _item);
            }
            else
            {
                //normal
                _m_sScrollBarStyle.drawBottomBtn(_m_sScrollBarStyle.normalPartIdx, _item);
            }
        }

        /*******************
         * the function to paint the top button
         **/
        protected internal void _onPainDragBtn(ALGUIBaseScrollBarDragBtn _item)
        {
            if (!_item.isEnable)
            {
                //disable
                _m_sScrollBarStyle.drawDragBtn(_m_sScrollBarStyle.disablePartIdx, _item);
            }
            else if (_item.isStat(ALGUIBaseMouseStat.OVER))
            {
                //mouse over
                _m_sScrollBarStyle.drawDragBtn(_m_sScrollBarStyle.mouseOverPartIdx, _item);
            }
            else if (_item.isStat(ALGUIBaseMouseStat.DOWN))
            {
                //mouse down
                _m_sScrollBarStyle.drawDragBtn(_m_sScrollBarStyle.mouseDownPartIdx, _item);
            }
            else
            {
                //normal
                _m_sScrollBarStyle.drawDragBtn(_m_sScrollBarStyle.normalPartIdx, _item);
            }
        }

        /*********************
         * 根据滑动块的风格设置滑动块相关数据
         **/
        private void _initScrollBarByStyle(ALSOGUIScrollBarStyle _style)
        {

            _m_sScrollBarStyle = _style;

            if (null == _m_sScrollBarStyle)
                return;

            if (null != _m_sScrollBarStyle.scrollBarBakTexture)
            {
                _m_toBackGroundDrawObj = new SquaredTextureObj(_style.scrollBarBakTexture);
            }

            //create top button
            ALGUIWndPositionStyle topBtnPosStyle = new ALGUIWndPositionStyle();
            topBtnPosStyle.x = 0;
            topBtnPosStyle.y = 0;
            if (_m_sScrollBarStyle.scrollBarDirection == EALGUIScrollBarDirection.VERTICAL)
            {
                topBtnPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.MID_TOP;
                topBtnPosStyle.width.lengthType = ALGUILengthType.NUM;
                topBtnPosStyle.width.num = _m_sScrollBarStyle.scrollBarTexturePageSize;
                topBtnPosStyle.height.lengthType = ALGUILengthType.NUM;
                topBtnPosStyle.height.num = _m_sScrollBarStyle.topBtnSize;
            }
            else
            {
                topBtnPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_MID;
                topBtnPosStyle.width.lengthType = ALGUILengthType.NUM;
                topBtnPosStyle.width.num = _m_sScrollBarStyle.topBtnSize;
                topBtnPosStyle.height.lengthType = ALGUILengthType.NUM;
                topBtnPosStyle.height.num = _m_sScrollBarStyle.scrollBarTexturePageSize;
            }

            _m_biTopBtnItem = new ALGUIBaseScrollBarBtnItem(this, _onPainTopBtn, topBtnPosStyle);
            ALGUIRegChildWnd(_m_biTopBtnItem);

            //create bottom button
            ALGUIWndPositionStyle bottomBtnPosStyle = new ALGUIWndPositionStyle();
            bottomBtnPosStyle.x = 0;
            bottomBtnPosStyle.y = 0;
            if (_m_sScrollBarStyle.scrollBarDirection == EALGUIScrollBarDirection.VERTICAL)
            {
                bottomBtnPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.MID_BOTTOM;
                bottomBtnPosStyle.width.lengthType = ALGUILengthType.NUM;
                bottomBtnPosStyle.width.num = _m_sScrollBarStyle.scrollBarTexturePageSize;
                bottomBtnPosStyle.height.lengthType = ALGUILengthType.NUM;
                bottomBtnPosStyle.height.num = _m_sScrollBarStyle.bottomBtnSize;
            }
            else
            {
                bottomBtnPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.RIGHT_MID;
                bottomBtnPosStyle.width.lengthType = ALGUILengthType.NUM;
                bottomBtnPosStyle.width.num = _m_sScrollBarStyle.bottomBtnSize;
                bottomBtnPosStyle.height.lengthType = ALGUILengthType.NUM;
                bottomBtnPosStyle.height.num = _m_sScrollBarStyle.scrollBarTexturePageSize;
            }

            _m_biBottomBtnItem = new ALGUIBaseScrollBarBtnItem(this, _onPainBottomBtn, bottomBtnPosStyle);
            ALGUIRegChildWnd(_m_biBottomBtnItem);

            //create drag button
            ALGUIWndPositionStyle dragBtnPosStyle = new ALGUIWndPositionStyle();
            if (_m_sScrollBarStyle.scrollBarDirection == EALGUIScrollBarDirection.VERTICAL)
            {
                dragBtnPosStyle.x = 0;
                dragBtnPosStyle.y = _m_sScrollBarStyle.topBtnSize;
                dragBtnPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.MID_TOP;
                dragBtnPosStyle.width.lengthType = ALGUILengthType.NUM;
                dragBtnPosStyle.width.num = _m_sScrollBarStyle.scrollBarTexturePageSize;
                dragBtnPosStyle.height.lengthType = ALGUILengthType.NUM;
                dragBtnPosStyle.height.num = _m_sScrollBarStyle.dragBtnSize;
            }
            else
            {
                dragBtnPosStyle.x = _m_sScrollBarStyle.topBtnSize;
                dragBtnPosStyle.y = 0;
                dragBtnPosStyle.wndRectRefPositionType = ALGUIWndPositionSetting.LEFT_MID;
                dragBtnPosStyle.width.lengthType = ALGUILengthType.NUM;
                dragBtnPosStyle.width.num = _m_sScrollBarStyle.dragBtnSize;
                dragBtnPosStyle.height.lengthType = ALGUILengthType.NUM;
                dragBtnPosStyle.height.num = _m_sScrollBarStyle.scrollBarTexturePageSize;
            }

            _m_dbDragBtnItem = new ALGUIBaseScrollBarDragBtn(this, dragBtnPosStyle);
            _m_dbDragBtnItem.dragEnable = true;
            //修改滑动块的移动函数
            _m_dbDragBtnItem.chgDragDealFunc(_dealDragBtnDragFunc);
            ALGUIRegChildWnd(_m_dbDragBtnItem);

            //reg on size function
            ALGUIRegOnSizeDelegate(_onSizeEvent);

            //注册上下按钮操作
            _m_biTopBtnItem.ALGUIRegMouseClickDelegate(upButtonClick);
            _m_biBottomBtnItem.ALGUIRegMouseClickDelegate(downButtonClick);
        }

        /*******************
         * 检查当前显示区域顶部位置信息是否合法
         **/
        private void _checkCurViewTopPosEnable()
        {
            if (_m_fCurViewTopPos + _m_fCurViewSize > _m_fTotalRange)
            {
                _m_fCurViewTopPos = _m_fTotalRange - _m_fCurViewSize;
            }
        }

        /** 向上/左按钮点击 */
        public void upButtonClick(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            if (EALGUIOpButtonType.OP_BTN == _btnType)
            {
                //前移
                setCurViewAreaTopPos(_m_fCurViewTopPos - _m_fChgViewSize);
            }
        }
        /** 向下/右按钮点击 */
        public void downButtonClick(ALGUIBaseWnd _wnd, Vector2 _pos, EALGUIOpButtonType _btnType)
        {
            if (EALGUIOpButtonType.OP_BTN == _btnType)
            {
                //前移
                setCurViewAreaTopPos(_m_fCurViewTopPos + _m_fChgViewSize);
            }
        }
    }
}

#endif
