using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public abstract class _ANPGGUIBasicSubWndControlContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND> : _ATALUGUISubWndBasicContainer<_T_ITEM_MONO, _T_CONTAINER_MONO, _T_ITEM_WND>
        where _T_ITEM_MONO : _AALBasicUIWndMono
        where _T_CONTAINER_MONO : _ATNPGGUIMonoControlContainer<_T_ITEM_MONO>
        where _T_ITEM_WND : _ATALBasicUISubWnd<_T_ITEM_MONO>
    {
        private List<_T_ITEM_WND> _m_lSubWndList;
        
        protected _ANPGGUIBasicSubWndControlContainer(_T_CONTAINER_MONO _containerMono)
                : base(_containerMono)
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
        
        /// <summary>
        /// 在添加了一个子窗口的时候调用的事件函数
        /// </summary>
        /// <param name="_itemWnd"></param>
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
        /// <param name="_dataList"></param>
        /// <param name="_setDataAction"></param>
        /// <typeparam name="_T_ITEM_DATA">返回值代表是否成功对窗口设值, 若返回false, 会先将该item窗口隐藏, 然后进入下一轮遍历中使用</typeparam>
        public virtual void showItemList<_T_ITEM_DATA>(List<_T_ITEM_DATA> _dataList, Func<_T_ITEM_WND, _T_ITEM_DATA, bool> _setDataAction)
        {
            if (_dataList == null || _dataList.Count <= 0)
            {
                if(wnd != null)
                    ALUGUICommon.setGameObjEnable(wnd.noItemShow, true);
                
                _hideAllSubWnd();
                return;
            }

            if (_m_lSubWndList == null)
                _m_lSubWndList = new List<_T_ITEM_WND>();

            _T_ITEM_WND itemWnd = null;
            _T_ITEM_DATA itemData = default(_T_ITEM_DATA);
            int showWndCount = 0;
            //遍历数据
            for (int i = 0; i < _dataList.Count; i++)
            {
                itemData = _dataList[i];
                if (null == itemData)
                    continue;

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
                    if (_setDataAction != null)
                    {
                        if (_setDataAction.Invoke(itemWnd, itemData))
                        {
                            showWndCount++;
                        }
                        else
                        {
                            itemWnd.hideWnd();
                        }
                    }
                    else
                    {
                        showWndCount++;
                    }
                }
            }

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, showWndCount <= 0);
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
        /// 显示item数据列表
        /// </summary>
        /// <param name="_totalCount"></param>
        /// <param name="_setDataAction"></param>
        /// <typeparam name="_T_ITEM_DATA">返回值代表是否成功对窗口设值, 若返回false, 会先将该item窗口隐藏, 然后进入下一轮遍历中使用</typeparam>
        public virtual void showItemList(int _totalCount, Func<int, _T_ITEM_WND, bool> _setDataAction)
        {
            if (_totalCount <= 0)
            {
                _hideAllSubWnd();
                return;
            }

            if (_m_lSubWndList == null)
                _m_lSubWndList = new List<_T_ITEM_WND>();

            _T_ITEM_WND itemWnd = null;
            int showWndCount = 0;
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
                    if (_setDataAction != null)
                    {
                        if (_setDataAction.Invoke(i, itemWnd))
                        {
                            showWndCount++;
                        }
                        else
                        {
                            itemWnd.hideWnd();
                        }
                    }
                    else
                    {
                        showWndCount++;
                    }
                }
            }
            
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, showWndCount <= 0);
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
        /// 刷新布局
        /// </summary>
        public void refreshLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
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
