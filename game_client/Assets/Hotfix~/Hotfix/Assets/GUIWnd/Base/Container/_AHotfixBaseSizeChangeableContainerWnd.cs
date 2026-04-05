using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    public abstract class _AHotfixBaseSizeChangeableContainerWnd<_T_CONTAINER_MONO, _T_ITEM_WND> : _AHotfixBaseContainerWnd<_T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_CONTAINER_MONO : _AHotfixSizeChangeableContainerBaseMono, new()
        where _T_ITEM_WND : _AGGUIHotfixBasicSubWnd
    {
        private List<_T_ITEM_WND> _m_lSubWndList;

        protected _AHotfixBaseSizeChangeableContainerWnd(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {
        }

        public override void hideWnd(Action _delayDoneAction)
        {
            _hideAllSubWnd();
            
            base.hideWnd(_delayDoneAction);
        }
        
        public new void resetWnd()
        {
            // 因为父类的resetWnd会对所有子窗口进行销毁, 所以这里只要清除子窗口列表就行
            if(_m_lSubWndList != null)
                _m_lSubWndList.Clear();
            
            base.resetWnd();
        }
        
        public new void discard()
        {
            // 因为父类的resetWnd会对所有子窗口进行销毁, 所以这里只要清除子窗口列表就行
            if(_m_lSubWndList != null)
                _m_lSubWndList.Clear();
            
            base.discard();
        }

        protected override void _onAddItemWnd(_T_ITEM_WND _itemWnd)
        {
        }

        /// <summary>
        /// 隐藏所有子窗口
        /// </summary>
        protected void _hideAllSubWnd()
        {
            if (_m_lSubWndList == null)
                return;

            foreach (var itemWnd in _m_lSubWndList)
            {
                if(itemWnd != null)
                    itemWnd.hideWnd();
            }
        }
        
        /// <summary>
        /// 显示item数据列表
        /// </summary>
        /// <param name="_totalCount"></param>
        public virtual void showItemList(int _totalCount)
        {
            if (_m_lSubWndList == null)
                _m_lSubWndList = new List<_T_ITEM_WND>();

            _T_ITEM_WND itemWnd = null;
            int showWndCount = 0;
            
            if (_totalCount > 0)
            {
                //遍历数据
                for (int i = 0; i < _totalCount; i++)
                {
                    //如果容器内部个数不足则新增视图
                    if (showWndCount >= _m_lSubWndList.Count)
                    {
                        itemWnd = addItemWnd();
                        if (itemWnd != null)
                        {
                            _m_lSubWndList.Add(itemWnd);
                        }
                    }
                    //如果容器个数足够，则取出
                    else
                    {
                        itemWnd = _m_lSubWndList[showWndCount];
                        if (itemWnd == null)
                        {
                            itemWnd = addItemWnd();
                            _m_lSubWndList[showWndCount] = itemWnd;
                        }
                    }

                    if (itemWnd != null)
                    {
                        itemWnd.showWnd();
                        if (_refreshItemWnd(i, itemWnd))
                        {
                            showWndCount++;
                        }
                        else
                        {
                            itemWnd.hideWnd();
                        }
                    }
                }
            }
            
            if (hotfixWnd != null)
            {
                ALUGUICommon.setGameObjEnable(hotfixWnd.noItemShow, showWndCount <= 0);
            }

            //隐藏容器中多余的视图
            for (int j = _m_lSubWndList.Count - 1; j >= showWndCount; j--)
            {
                //移除窗口
                removeItemWnd(_m_lSubWndList[j]);
                //从队列删除
                _m_lSubWndList.RemoveAt(j);
            }

            //刷新布局
            refreshLayout();
        }

        /// <summary>
        /// 返回值代表是否成功对窗口设值, 若返回false, 会先将该item窗口隐藏, 然后进入下一轮遍历中使用, 返回true代表成功对窗口设值
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        protected abstract bool _refreshItemWnd(int _index, _T_ITEM_WND _itemWnd);
        
        /// <summary>
        /// 刷新布局
        /// </summary>
        public void refreshLayout()
        {
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                if (wnd == null || hotfixWnd == null || hotfixWnd.itemContainer == null)
                    return;

                RectTransform itemContainerRectTransform = hotfixWnd.itemContainer.GetComponent<RectTransform>();
                if(itemContainerRectTransform == null)
                    return;
                
                LayoutRebuilder.ForceRebuildLayoutImmediate(itemContainerRectTransform);
                
                if(hotfixWnd.chgSizeRectTransform == null)
                    return;

                float width = itemContainerRectTransform.rect.width;
                float height = itemContainerRectTransform.rect.height;

                if (hotfixWnd.widthChangeable)
                {
                    width += hotfixWnd.widthExpand;
                    if(hotfixWnd.widthRangeMin > -1 && width < hotfixWnd.widthRangeMin)
                        width = hotfixWnd.widthRangeMin;

                    if (hotfixWnd.widthRangeMax > -1 && width > hotfixWnd.widthRangeMax)
                        width = hotfixWnd.widthRangeMax;
                    
                    hotfixWnd.chgSizeRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                }
                
                if (hotfixWnd.heightChangeable)
                {
                    height += hotfixWnd.heightExpand;
                    if(hotfixWnd.heightRangeMin > -1 && height < hotfixWnd.heightRangeMin)
                        height = hotfixWnd.heightRangeMin;

                    if (hotfixWnd.heightRangeMax > -1 && height > hotfixWnd.heightRangeMax)
                        height = hotfixWnd.heightRangeMax;
                    
                    hotfixWnd.chgSizeRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                }
            });
        }

        /// <summary>
        /// 遍历所有正常显示中的窗口
        /// </summary>
        /// <param name="_func">传递的参数是显示中的窗口, 返回值表示是否继续查找</param>
        public void iterateShowItemWnd(Func<_T_ITEM_WND, bool> _func)
        {
            if (_func == null || _m_lSubWndList == null)
                return;

            foreach (var itemWnd in _m_lSubWndList)
            {
                if(itemWnd == null || !itemWnd.isShow)
                    return;

                if (!_func(itemWnd))
                    break;
            }
        }
    }
}