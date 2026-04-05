using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;

namespace Hotfix
{
    /// <summary>
    /// 热更工程通用Container基类
    /// </summary>
    /// <typeparam name="_T_CONTAINER_MONO"></typeparam>
    /// <typeparam name="_T_ITEM_WND"></typeparam>
    public abstract class _AHotfixBaseContainerWnd<_T_CONTAINER_MONO, _T_ITEM_WND> : _AHotfixBaseSubWnd<_T_CONTAINER_MONO>
        where _T_CONTAINER_MONO : _AHotfixContainerBaseMono, new()
        where _T_ITEM_WND : _AGGUIHotfixBasicSubWnd
    {
        /** 存储子窗口对象的队列 */
        private List<_T_ITEM_WND> _m_lItemWndList = new List<_T_ITEM_WND>();
        /** 刷新拖拽窗口尺寸的时间标记 */
        private float _m_fRefreshGridTimeTag;

        protected _AHotfixBaseContainerWnd(GGUIHotfixCommonMono _containerMono)
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
            //    ALCommon.releaseGameObj(_m_monoWnd);
            _m_monoWnd = null;
        }

        /******************
         * 获取第一个匹配的窗口
         **/
        public _T_ITEM_WND getFirstItemWnd(Func<_T_ITEM_WND, bool> _func)
        {
            if (null == _func)
                return null;

            _T_ITEM_WND tmpWnd = null;
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

            return null;
        }

        /******************
         * 添加一个子窗口到容器中
         **/
        public _T_ITEM_WND addItemWnd()
        {
            if (hotfixWnd == null)
                return null;

            return addItemWnd(hotfixWnd.itemTemplate);
        }
        public _T_ITEM_WND addItemWnd(GGUIHotfixCommonMono _templateWndMono)
        {
            //判断对应数据是否有效
            if (null == hotfixWnd || null == hotfixWnd.itemContainer || null == _templateWndMono)
                return null;

            //实例化一个子窗口对象
            GGUIHotfixCommonMono itemWndObj = _instantiateItemMono(_templateWndMono);
            //创建一个子窗口管理对象
            _T_ITEM_WND itemWnd = _createItemWnd(itemWndObj);
            if (null == itemWnd || null == itemWnd.wnd)
            {
                //创建对象无效，删除创建对象资源并退出
                _releaseItemMono(itemWndObj);
                return null;
            }

            //将子窗口添加到容器中
            itemWnd.wnd.transform.SetParent(hotfixWnd.itemContainer.transform);
            itemWnd.wnd.transform.localPosition = Vector3.zero;
            itemWnd.wnd.transform.localScale = Vector3.one;

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
            _T_ITEM_WND tmpWnd = null;
            //遍历查找，匹配成功则跳出循环
            for (int i = 0; i < _m_lItemWndList.Count; i++)
            {
                tmpWnd = _m_lItemWndList[i];
                if (null == tmpWnd)
                    continue;

                if (tmpWnd == _itemWnd)
                {
                    //移除对应位置节点
                    _m_lItemWndList.RemoveAt(i);
                    break;
                }
            }

            //判断数据是否有效
            if (_itemWnd != tmpWnd || null == tmpWnd)
                return;

            GGUIHotfixCommonMono wndMono = tmpWnd.wnd;
            //使子窗口的位置可以正确显示
            tmpWnd.wnd.transform.SetParent(null);
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
                //使子窗口的位置可以正确显示
                if (null != itemWnd.wnd && null != itemWnd.wnd)
                    itemWnd.wnd.transform.SetParent(null);

                GGUIHotfixCommonMono wndMono = itemWnd.wnd;
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
            if (0 <= _index && _index < _m_lItemWndList.Count)
            {
                return _m_lItemWndList[_index];
            }
            return null;
        }

        /**********
         * 移动到最底层
         **/
        public void moveToLeft()
        {
            if (null == hotfixWnd || null == hotfixWnd.scrollRect)
                return;

            hotfixWnd.scrollRect.horizontalNormalizedPosition = 0;
        }
        public void moveToRight()
        {
            if (null == hotfixWnd || null == hotfixWnd.scrollRect)
                return;

            hotfixWnd.scrollRect.horizontalNormalizedPosition = 1;
        }
        public void moveToTop()
        {
            if (null == hotfixWnd || null == hotfixWnd.scrollRect)
                return;

            hotfixWnd.scrollRect.verticalNormalizedPosition = 1;
        }
        public void moveToBottom()
        {
            if (null == hotfixWnd || null == hotfixWnd.scrollRect)
                return;

            hotfixWnd.scrollRect.verticalNormalizedPosition = 0;
        }
        public void moveToHorizontalRate(float _rate)
        {
            if (null == hotfixWnd || null == hotfixWnd.scrollRect)
                return;

            float finalH = Mathf.Clamp(1 - _rate, 0f, 1f);
            hotfixWnd.scrollRect.horizontalNormalizedPosition = finalH;
        }
        public void moveToVerticalRate(float _rate)
        {
            if (null == hotfixWnd || null == hotfixWnd.scrollRect)
                return;

            float finalV = Mathf.Clamp(1 - _rate, 0f, 1f);
            hotfixWnd.scrollRect.verticalNormalizedPosition = finalV;
        }

        /****************
         * 刷新grid的显示尺寸
         **/
        protected void _refreshGrid()
        {
            if (null == hotfixWnd || null == hotfixWnd.scrollRect)
                return;

            //判断时间标记
            if (_m_fRefreshGridTimeTag >= Time.realtimeSinceStartup)
                return;

            //设置时间标记
            _m_fRefreshGridTimeTag = Time.realtimeSinceStartup;
            //开启任务刷新
            if (hotfixWnd.itemContainer is VerticalLayoutGroup)
            {
                ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(hotfixWnd.itemContainer, GridLayoutGroup.Axis.Vertical));
            }
            else if (hotfixWnd.itemContainer is HorizontalLayoutGroup)
            {
                ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(hotfixWnd.itemContainer, GridLayoutGroup.Axis.Horizontal));
            }
            else if (hotfixWnd.itemContainer is GridLayoutGroup)
            {
                //横向布局是向下延申的，纵向布局是横向延申的
                //因此尺寸的设计是需要颠倒判断的
                GridLayoutGroup.Axis direction = ((GridLayoutGroup)hotfixWnd.itemContainer).startAxis;
                if (GridLayoutGroup.Axis.Horizontal == direction)
                    ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(hotfixWnd.itemContainer, GridLayoutGroup.Axis.Vertical));
                else
                    ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIWndContainerRefreshGridMonoTask(hotfixWnd.itemContainer, GridLayoutGroup.Axis.Horizontal));
            }
        }

        /***************
         * 创建本对象的脚本对象实例
         **/
        protected virtual GGUIHotfixCommonMono _instantiateItemMono(GGUIHotfixCommonMono _template)
        {
            if (null == _template)
                return null;

            //实例化一个子窗口对象
            return GameObject.Instantiate(_template) as GGUIHotfixCommonMono;
        }

        
        
        /***************
         * 创建本对象的脚本对象实例
         **/
        protected virtual void _releaseItemMono(GGUIHotfixCommonMono _mono)
        {
            if (null == _mono)
                return;

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
        protected abstract _T_ITEM_WND _createItemWnd(GGUIHotfixCommonMono _itemMono);

        /// <summary>
        /// 在添加了一个子窗口的时候调用的事件函数
        /// </summary>
        /// <param name="_itemWnd"></param>
        protected abstract void _onAddItemWnd(_T_ITEM_WND _itemWnd);
    }
}