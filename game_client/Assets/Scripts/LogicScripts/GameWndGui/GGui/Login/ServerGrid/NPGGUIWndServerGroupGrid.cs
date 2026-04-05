using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ALPackage;
using JetBrains.Annotations;


namespace GOE
{
    /// <summary>
    /// 服务器组列表
    /// </summary>
    public class NPGGUIWndServerGroupGrid : _AALUGUIBasicGridSubWnd<NPGGUIMonoServerGroupGridItem, NPGGUIMonoServerGroupGrid, NPGGUIWndServerGroupGridItem>
    {
        private Action<string, ServerGroupShowData> _m_onSelectedGroup;//点击事件
        [NotNull]private List<ServerGroupShowData> _m_serverGroupList = new List<ServerGroupShowData>();//服务器组列表
        private ServerGroupShowData _m_curSelData;//当前选中的数据
        private string _m_sCurSelectAreaTag;//当前选中的大区标识

        public NPGGUIWndServerGroupGrid(NPGGUIMonoServerGroupGrid _containerWnd)
            : base(_containerWnd)
        {
            initWnd();
        }

        //当前选中group信息
        public Action<string, ServerGroupShowData> OnSelectedGroup { get { return _m_onSelectedGroup; } set { _m_onSelectedGroup = value; } }

        protected override NPGGUIWndServerGroupGridItem _createItemWnd(NPGGUIMonoServerGroupGridItem _itemMono)
        {
            return new NPGGUIWndServerGroupGridItem(_itemMono);
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        protected override void _refreshItemwnd(NPGGUIWndServerGroupGridItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_serverGroupList.Count)
            {
                return;
            }

            ServerGroupShowData groupInfo = _m_serverGroupList[_itemIdx];
            if (null == groupInfo)
                return;

            //设置信息
            _itemMono.setInfo(groupInfo, _onSelectedGroup);
            _itemMono.setSelected(_m_curSelData != null && _m_curSelData.groupName == groupInfo.groupName);
        }

        /// <summary>
        /// 显示服务器组列表
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_onSelectedGroup"></param>
        /// <param name="_selectedGroup"></param>
        public void showWnd(string _selectedAreaTag, List<ServerGroupShowData> groupShowDataList)
        {
            if (groupShowDataList == null)
                return;

            base.showWnd();
            _m_curSelData = groupShowDataList.SafeGet(0);
            _m_serverGroupList = groupShowDataList;
            _m_sCurSelectAreaTag = _selectedAreaTag;
            setItemCount(groupShowDataList.Count);
            moveToTop();
        }

        /// <summary>
        /// 点击选中服务器组的回调
        /// </summary>
        /// <param name="_item"></param>
        private void _onSelectedGroup(NPGGUIWndServerGroupGridItem _item)
        {
            if(null == _item || null == _item.groupData)
                return;

            _m_curSelData = _item.groupData;
            
            //刷新所有
            forceRefreshAllItem();
            
            //调用回调
            if (null != _m_onSelectedGroup)
                _m_onSelectedGroup(_m_sCurSelectAreaTag, _m_curSelData);
        }
    }
}