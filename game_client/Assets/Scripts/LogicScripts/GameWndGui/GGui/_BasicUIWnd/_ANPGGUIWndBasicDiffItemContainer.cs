using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public interface _IBasicDiffItemContainerItemWnd
    {
        _AALBasicUIWndMono getWndMono();

        void discard();

        void initWnd();

        void showWnd();
        
        void hideWnd();
    }
    
    public abstract class _ANPGGUIWndBasicDiffItemContainer<_T_CONTAINER_MONO, _T_ITEM_WND> : _ATALBasicUISubWnd<_T_CONTAINER_MONO>
        where _T_CONTAINER_MONO : _ATNPGGUIMonoBasicDiffItemContainer
        where _T_ITEM_WND : _IBasicDiffItemContainerItemWnd
    {
        /** 存储子窗口对象的队列 */
        private List<_T_ITEM_WND> _m_lItemWndList = new List<_T_ITEM_WND>();
        /** 刷新拖拽窗口尺寸的时间标记 */
        private float _m_fRefreshGridTimeTag;

        protected _ANPGGUIWndBasicDiffItemContainer(_T_CONTAINER_MONO _containerMono)
            : base(_containerMono)
        {
            _m_fRefreshGridTimeTag = 0;
        }

        /***************
         * 重置窗口数据，代替原先的discard函数
         **/
        public new void resetWnd()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIContainer][{this.GetType().Name}] resetWnd.");
            }
            
            hideWnd();

            //清除所有子窗口对象并释放
            clearAll();

            //调用reset事件函数
            _onReset();
        }

        /***************
         * 释放窗口资源相关对象
         **/
        public new void discard()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UIContainer][{this.GetType().Name}] discard.");
            }
            
            //判断是否加载完成，是则隐藏本窗口
            hideWnd();

            //清除所有子窗口对象并释放
            clearAll();

            //调用事件函数
            _onDiscard();

            //子窗口不释放资源对象
            //if (null != _m_monoWnd)
            //    ALUnityCommon.releaseGameObj(_m_monoWnd);
            _m_monoWnd = null;
        }

        /******************
         * 获取第一个匹配的窗口
         **/
        public _T_ITEM_WND getFirstItemWnd(Func<_T_ITEM_WND, bool> _func)
        {
            if (null == _func)
                return default;

            _T_ITEM_WND tmpWnd = default(_T_ITEM_WND);
            //遍历查找，匹配成功则跳出循环
            for (int i = 0; i < _m_lItemWndList.Count; i++)
            {
                tmpWnd = _m_lItemWndList[i];
                if (null == tmpWnd)
                    continue;

                if (_func(tmpWnd))
                {
                    return tmpWnd;
                }
            }

            return default;
        }
        
        public _T_ITEM_WND addItemWnd(_AALBasicUIWndMono _templateWndMono)
        {
            //判断对应数据是否有效
            if(null == wnd || null == wnd.itemContainer || null == _templateWndMono)
                return default;

            //实例化一个子窗口对象
            _AALBasicUIWndMono itemWndObj = _instantiateItemMono(_templateWndMono);
            //创建一个子窗口管理对象
            _T_ITEM_WND itemWnd = _createItemWnd(itemWndObj);
            _AALBasicUIWndMono wndMono = itemWnd?.getWndMono();
            if(null == itemWnd || null == wndMono)
            {
                //创建对象无效，删除创建对象资源并退出
                _releaseItemMono(itemWndObj);
                return default;
            }

            //将子窗口添加到容器中
            wndMono.transform.SetParent(wnd.itemContainer.transform);
            wndMono.transform.localPosition = Vector3.zero;
            wndMono.transform.localScale = Vector3.one;
            wndMono.transform.localRotation = Quaternion.identity;

            //调用子窗口的初始化函数
            itemWnd.initWnd();
            //调用子窗口的显示函数
            itemWnd.showWnd();

            //添加到数据集合
            _m_lItemWndList.Add(itemWnd);

            //在添加了窗口的时候调用的对应事件函数
            _onAddItemWnd(itemWnd);

            //刷新拖拽区域大小
            _refreshGrid();

            return itemWnd;
        }

        /**************
        * 从数据集中删除对应的子窗口对象
        * */
        public void removeItemWnd(_T_ITEM_WND _itemWnd)
        {
            _T_ITEM_WND tmpWnd = default(_T_ITEM_WND);
            //遍历查找，匹配成功则跳出循环
            for (int i = 0; i < _m_lItemWndList.Count; i++)
            {
                tmpWnd = _m_lItemWndList[i];
                if (null == tmpWnd)
                    continue;

                if (EqualityComparer<_T_ITEM_WND>.Default.Equals(tmpWnd, _itemWnd))
                {
                    //移除对应位置节点
                    _m_lItemWndList.RemoveAt(i);
                    break;
                }
            }

            //判断数据是否有效
            if (!EqualityComparer<_T_ITEM_WND>.Default.Equals(tmpWnd, _itemWnd) || null == tmpWnd)
                return;

            _AALBasicUIWndMono wndMono = tmpWnd.getWndMono();
            //使子窗口的位置可以正确显示
            if(wndMono != null)
                wndMono.transform.SetParent(null);
            //释放窗口对象
            _discardItem(tmpWnd);

            //刷新拖拽区域大小
            _refreshGrid();

            //删除对应的对象
            _releaseItemMono(wndMono);
        }

        /****************
         * 使用对应的刷新函数刷新所有子对象
         **/
        public void refreshAllItem(Action<_T_ITEM_WND> _refreshAction)
        {
            if (null == _refreshAction)
                return;

            //释放物品格的对象
            foreach (_T_ITEM_WND itemWnd in _m_lItemWndList)
            {
                //处理刷新函数
                _refreshAction(itemWnd);
            }
        }

        /****************
         * 清除所有子窗口
         **/
        public void clearAll()
        {
            //释放物品格的对象
            foreach (_T_ITEM_WND itemWnd in _m_lItemWndList)
            {
                if (itemWnd == null)
                    continue;
                
                _AALBasicUIWndMono wndMono = itemWnd.getWndMono();
                
                //使子窗口的位置可以正确显示
                if (null != wndMono)
                    wndMono.transform.SetParent(null);

                //先调用析构函数
                _discardItem(itemWnd);
                //删除对应的对象
                _releaseItemMono(wndMono);
            }
            //清空数据集
            _m_lItemWndList.Clear();

            //刷新拖拽区域大小
            _refreshGrid();
        }

        /*****************
         * 获取对应位置对象对应的实际显示对象
         **/
        public _T_ITEM_WND getItemWnd(int _index)
        {
            if(0 <= _index && _index < _m_lItemWndList.Count)
            {
                return _m_lItemWndList[_index];
            }
            return default(_T_ITEM_WND);
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

        /****************
         * 刷新grid的显示尺寸
         **/
        protected void _refreshGrid()
        {
            if (null == wnd || null == wnd.scrollRect)
                return;

            //判断时间标记
            if (_m_fRefreshGridTimeTag >= Time.realtimeSinceStartup)
                return;

            //设置时间标记
            _m_fRefreshGridTimeTag = Time.realtimeSinceStartup;
            //开启任务刷新
            if (wnd.itemContainer is VerticalLayoutGroup)
            {
                ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(wnd.itemContainer, GridLayoutGroup.Axis.Vertical));
            }
            else if (wnd.itemContainer is HorizontalLayoutGroup)
            {
                ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(wnd.itemContainer, GridLayoutGroup.Axis.Horizontal));
            }
            else if (wnd.itemContainer is GridLayoutGroup)
            {
                //横向布局是向下延申的，纵向布局是横向延申的
                //因此尺寸的设计是需要颠倒判断的
                GridLayoutGroup.Axis direction = ((GridLayoutGroup)wnd.itemContainer).startAxis;
                if (GridLayoutGroup.Axis.Horizontal == direction)
                    ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(wnd.itemContainer, GridLayoutGroup.Axis.Vertical));
                else
                    ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(wnd.itemContainer, GridLayoutGroup.Axis.Horizontal));
            }
        }

        /***************
         * 创建本对象的脚本对象实例
         **/
        protected virtual _AALBasicUIWndMono _instantiateItemMono (_AALBasicUIWndMono _template) 
        {
            if(null == _template)
                return null;

            //实例化一个子窗口对象
            return GameObject.Instantiate(_template) as _AALBasicUIWndMono;
        }

        /***************
         * 创建本对象的脚本对象实例
         **/
        protected virtual void _releaseItemMono (_AALBasicUIWndMono _mono) 
        {
            if(null == _mono)
                return ;

            //删除对应的对象
            ALUnityCommon.releaseGameObj(_mono);
        }

        /**************
         * 释放单个窗口资源的函数
         **/
        protected virtual void _discardItem(_T_ITEM_WND _itemWnd)
        {
            if (null == _itemWnd)
                return;

            _itemWnd.discard();
        }

        /*************
         * 根据带入的已经实例化的图标对象，创建一个对应子窗口的管理对象
         **/
        protected abstract _T_ITEM_WND _createItemWnd(_AALBasicUIWndMono _itemMono);

        /// <summary>
        /// 在添加了一个子窗口的时候调用的事件函数
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected abstract void _onAddItemWnd(_T_ITEM_WND _itemWnd);
    }
}
