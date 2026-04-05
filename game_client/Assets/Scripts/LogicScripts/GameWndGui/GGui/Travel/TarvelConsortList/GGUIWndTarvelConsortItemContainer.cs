
using System;
using System.Collections.Generic;


namespace GOE
{
    //情报使用背包物品 容器
    public class GGUIWndTarvelConsortItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoTarvelConsortItem, GGUIMonoTarvelConsortItemContainer, GGUIWndTarvelConsortItem>
    {
        public List<GGUIWndTarvelConsortItem> _m_lItemGroupList;//子控件列表
        public GGUIWndTarvelConsortItemContainer(GGUIMonoTarvelConsortItemContainer _containerMono)
            : base(_containerMono)
        {
            initWnd();
        }

        /// <summary>
        /// item被点击回调
        /// </summary>
        public event Action<GGUIWndTarvelConsortItem> onItemClick;
        
        protected override GGUIWndTarvelConsortItem _createItemWnd(GGUIMonoTarvelConsortItem _itemMono)
        {
            GGUIWndTarvelConsortItem itemWnd = new GGUIWndTarvelConsortItem(_itemMono);
            itemWnd.onItemClick += _onItemClick;
            return itemWnd;
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
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            onItemClick = null;
            
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }
        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndTarvelConsortItem>();
        }
        
        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList">物品显示数据列表</param>
        public void showItemList(List<_IConsortShowInfo> _itemDataList, Dictionary<long, ETravelConsortUnlockStat> _travelConsortUnlockStatDic)
        {
            _IConsortShowInfo tempData = null;
            GGUIWndTarvelConsortItem tempItemWnd = null;
            int count = 0;
            for(int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if(tempData == null)
                    continue;

                if(i >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if(tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[i];
                }

                ETravelConsortUnlockStat tempStat = ETravelConsortUnlockStat.UNLOCK;
                if (_travelConsortUnlockStatDic == null || !_travelConsortUnlockStatDic.TryGetValue(tempData.consortId, out tempStat))
                {
                    tempStat = NPPlayer.instance.travelComp.getTravelConsortUnlockStat(tempData.consortId);
                }
                
                tempItemWnd.setInfo(tempData, tempStat);
                count++;
            }
            for(int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }
        }
        
        private void _onItemClick(GGUIWndTarvelConsortItem _itemWnd)
        {
            onItemClick?.Invoke(_itemWnd);
        }
    }
}
