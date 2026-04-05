using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;


namespace GOE
{
    public class NPGGUIWndTaskRewardGrid : _AALUGUIBasicGridSubWnd<NPGGUIMonoCommonItem, NPGGUIMonoRewardGrid, NPGGUIWndRewardItem>
    {
        private List<CommonItemData> _m_lDataList;

        public NPGGUIWndTaskRewardGrid(NPGGUIMonoRewardGrid _containerMono)
            : base(_containerMono)
        {
            _m_lDataList = new List<CommonItemData>();
        }

        protected override NPGGUIWndRewardItem _createItemWnd(NPGGUIMonoCommonItem _itemMono)
        {
            return new NPGGUIWndRewardItem(_itemMono);
        }

        /******************
        * 显示窗口的事件函数
        **/
        protected override void _onShowWnd()
        {
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {

        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            _m_lDataList.Clear();
            setItemCount(0);
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_lDataList.Clear();
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        protected override void _refreshItemwnd(NPGGUIWndRewardItem _item, int _itemIdx)
        {
            if(null == _item)
                return;

            if(_m_lDataList.Count >= _itemIdx)
                return;

            //获取数据
            CommonItemData data = _m_lDataList[_itemIdx];
            if(null == data)
                return;

            _item.setTaskRewardItem(data);
        }

        //设置数据队列
        public void setItemList(bool _showReceivedMark, CommonItemData[] _itemDataList)
        {
            _m_lDataList.Clear();
            if(null != _itemDataList)
            {
                _m_lDataList.AddRange(_itemDataList);
            }

            //设置数据
            setItemCount(_m_lDataList.Count);
        }
        public void setItemList(bool _showReceivedMark, List<CommonItemData> _itemDataList)
        {
            _m_lDataList.Clear();
            if(null != _itemDataList)
            {
                _m_lDataList.AddRange(_itemDataList);
            }

            //设置数据
            setItemCount(_m_lDataList.Count);
        }
    }
}

