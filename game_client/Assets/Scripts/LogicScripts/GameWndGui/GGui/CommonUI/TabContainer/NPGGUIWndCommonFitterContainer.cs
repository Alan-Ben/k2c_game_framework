using System;
using System.Collections.Generic;
using System.Linq;

namespace GOE
{
    /// <summary>
    /// 通用的筛选i容器
    /// </summary>
    public class NPGGUIWndCommonFitterContainer<T> : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoCommonFitterTab, NPGGUIMonoCommonFitterContainer, NPGGUIWndCommonFitterTab<T>>
    {
        //显示列表
        private List<NPGGUIWndCommonFitterTab<T>> _m_lItemList;

        //点击回调
        private Action<NPGGUIWndCommonFitterTab<T>> _m_clickDelegate;
        private Func<T, bool> _m_curSelectedTypes;
        private List<NPGGUICommonFitterMono<T>> _m_monoList;

        public NPGGUIWndCommonFitterContainer(NPGGUIMonoCommonFitterContainer _mono, Action<NPGGUIWndCommonFitterTab<T>> _clickDelegate) : base(_mono)
        {
            _m_clickDelegate = _clickDelegate;
            initWnd();
        }

        //点击回调
        public Action<NPGGUIWndCommonFitterTab<T>> clickDelegate { get { return _m_clickDelegate; } }

        protected override NPGGUIWndCommonFitterTab<T> _createItemWnd(NPGGUIMonoCommonFitterTab _itemMono)
        {
            NPGGUIWndCommonFitterTab<T> itemWnd = new NPGGUIWndCommonFitterTab<T>(_itemMono,_onClickItem);
            return itemWnd;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lItemList = new List<NPGGUIWndCommonFitterTab<T>>();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;

            _m_clickDelegate = null;
        }

        public void showList(List<NPGGUICommonFitterMono<T>> _monoList)
        {
            _m_monoList = _monoList;
            _init();
        }

        private void _init()
        {
            if (null == wnd)
                return;
            if (null == _m_monoList)
                return;

            NPGGUIWndCommonFitterTab<T> itemWnd = null;
            NPGGUICommonFitterMono<T> item = null;

            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _m_monoList.Count; i++)
            {
                item = _m_monoList[i];
                if (null == item)
                    continue;
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                {
                    itemWnd = _m_lItemList[count];
                    if (null == itemWnd)
                        continue;
                }

                itemWnd.showWnd();
                itemWnd.setInfo(item);
                
                //默认选中
                if (null != _m_curSelectedTypes && _m_curSelectedTypes(item.type))
                {
                    itemWnd.setSelected(true);
                    // _onClickItem(itemWnd);
                }
                else
                {
                    itemWnd.setSelected(false);
                }

                count++;
            }
            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onClickItem(NPGGUIWndCommonFitterTab<T> _itemWnd)
        {
            if (null != _m_clickDelegate)
                _m_clickDelegate(_itemWnd);
        }

        /// <summary>
        /// 设置选中类型
        /// </summary>
        /// <param name="_isDefauliSelectedList"></param>
        /// <typeparam name="T"></typeparam>
        public void setDefaultSelected(Func<T, bool> _isDefauliSelectedList)
        {
            _m_curSelectedTypes = _isDefauliSelectedList;
        }

        /// <summary>
        /// 返回是否全部选中
        /// </summary>
        /// <returns></returns>
        public bool isAllSelected()
        {
            NPGGUICommonFitterMono<T> item = null;
            for (int i = 0; i < _m_monoList.Count; i++)
            {
                item = _m_monoList[i];
                if (null == item)
                    continue;
                //默认选中
                if (null != _m_curSelectedTypes && !_m_curSelectedTypes(item.type))
                {
                    return false;
                }
            }

            return true;
        }

        public void setAllSelected(bool _isOn)
        {
            NPGGUIWndCommonFitterTab<T> item = null;
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                item = _m_lItemList[i];
                if (null == item)
                    continue;
                //默认选中
                if (item.isOn != _isOn)
                {
                    item.setSelected(_isOn);
                }
            }
        }
    }
}
