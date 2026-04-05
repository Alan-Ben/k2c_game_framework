using ALPackage;
using NPCommon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用获得道具窗体基类
    /// </summary>
    public abstract class _ANPGGUIWndBaseGetItem<T> : _ANPGGUIBasicWnd<T>
        where T : NPGGUIMonoGetItem
    {
        private NPGGUIWndGetItemContainer _m_wItemContainer;
        private int _m_itemCount;

        protected _ANPGGUIWndBaseGetItem() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override _AALResourceCore _resourceCore
        {
            get { return GameResCore.instance; }
        }


        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            _m_wItemContainer?.hideWnd();
            _m_itemCount = 0;
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wItemContainer != null)
            {
                _m_wItemContainer.onShowItem -= _onShowItem;
                _m_wItemContainer.discard();
            }
            _m_wItemContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickBtnClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //物品列表
            if (wnd.monoItemContainer != null)
            {
                _m_wItemContainer = new NPGGUIWndGetItemContainer(wnd.monoItemContainer);
                _m_wItemContainer.onShowItem += _onShowItem;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickBtnClose);
        }


        #region 点击事件

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnClose(GameObject _go)
        {
            //如果正在显示，第一次点击先显示全部物品
            if (_m_wItemContainer != null && _m_wItemContainer.curShowIndex < _m_wItemContainer.totalItemCount)
            {
                _m_wItemContainer.showAllItem();
                _m_wItemContainer.scrollMoveToBottom();
            }
            else
            {
                //退出节点
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GET_ITEM);
            }
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 当显示了一个item
        /// </summary>
        private void _onShowItem()
        {
            if (wnd == null || _m_wItemContainer == null || _m_wItemContainer.wnd == null)
                return;

            //超出了显示范围，移动到底部
            if (_m_wItemContainer.curShowIndex >= wnd.maxLine * _m_wItemContainer.wnd.content.constraintCount)
            {
                _m_wItemContainer.scrollMoveToBottom();
            }
        }

        /// <summary>
        /// 计算窗体大小
        /// </summary>
        /// <param name="_itemCount"></param>
        private void _calWndSize(int _itemCount)
        {
            if (wnd == null || _m_wItemContainer == null || _m_wItemContainer.wnd == null)
                return;

            float containerHeight = _getContainerHeight(_itemCount);

            //计算容器高度
            if (wnd.contentRectTrans != null)
            {
                wnd.contentRectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, containerHeight);
            }

            //计算外层窗体高度
            if (wnd.wndRect != null)
            {
                wnd.wndRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, containerHeight + wnd.additionHeight);
            }

            //单行情况，居中对齐逐个显示；多行情况，从左开始逐个显示
            if (_m_wItemContainer.wnd.content != null)
            {
                if (_itemCount > _m_wItemContainer.wnd.content.constraintCount)
                {
                    _m_wItemContainer.wnd.content.childAlignment = TextAnchor.MiddleLeft;
                }
                else
                {
                    _m_wItemContainer.wnd.content.childAlignment = TextAnchor.MiddleCenter;
                }
            }
        }

        /// <summary>
        /// 获取容器的高度
        /// </summary>
        /// <param name="_itemCount"></param>
        /// <returns></returns>
        private float _getContainerHeight(int _itemCount)
        {
            if (wnd == null || _m_wItemContainer == null || _m_wItemContainer.wnd == null)
                return 0f;

            //计算总共的行数
            int lineCount = (int)Math.Ceiling((float)_itemCount / _m_wItemContainer.wnd.content.constraintCount);

            //超过最大行数，按最大行数计算，拖动展示所有物品
            if (lineCount > wnd.maxLine)
            {
                lineCount = wnd.maxLine;
            }

            return lineCount * (_m_wItemContainer.wnd.content.cellSize.y + _m_wItemContainer.wnd.content.spacing.y);
        }


        #endregion


        #region 外部调用

        /// <summary>
        /// 设置数据并展示
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_titleKey"></param>
        public void setItemListAndShow(List<NPCommon_ItemInfo> _itemList, string _titleKey = TransKeyConst.common_getreward_tip)
        {
            if (wnd == null || _itemList == null)
                return;

            //标题
            if (string.IsNullOrEmpty(_titleKey))
                _titleKey = TransKeyConst.common_getreward_tip;
            
            if(wnd.titleTxt != null)
                wnd.titleTxt.text = TextTranslate.instance.getLanguage(_titleKey);

            _m_itemCount = _itemList.Count;
            //窗体大小
            _calWndSize(_m_itemCount);

            //显示物品列表
            _m_wItemContainer?.showItemList(_itemList);

            showWnd();
        }
        
        /// <summary>
        /// 设置数据并展示
        /// </summary>
        /// <param name="_itemList"></param>
        /// <param name="_titleKey"></param>
        public void addItemListAndShow(List<NPCommon_ItemInfo> _itemList)
        {
            if (wnd == null || _itemList == null)
                return;

            //窗体大小
            _m_itemCount += _itemList.Count;
            //窗体大小
            _calWndSize(_m_itemCount);

            //显示物品列表
            _m_wItemContainer?.addItemList(_itemList);

            showWnd();
        }

        #endregion
    }
}
