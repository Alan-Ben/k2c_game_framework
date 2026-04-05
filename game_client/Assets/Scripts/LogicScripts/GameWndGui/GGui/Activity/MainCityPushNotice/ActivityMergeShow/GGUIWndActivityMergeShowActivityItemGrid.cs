using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 活动合并展示主城推送弹窗活动Item列表
    /// </summary>
    public class GGUIWndActivityMergeShowActivityItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoActivityMergeShowActivityItem, GGUIMonoActivityMergeShowActivityItemGrid, GGUIWndActivityMergeShowActivityItem>
    {
        // 展示数据：key为活动合并展示配表数据，value为活动信息
        private List<KeyValuePair<PushNoticeActivityMergeRefObj, _ABaseActivityInfo>> _m_lDataList;
        private ALCommonEnableTaskController _m_secTask;

        /// <summary>
        /// 点击前往按钮事件
        /// </summary>
        public event Action<GGUIWndActivityMergeShowActivityItem> onGotoBtnClick;


        public GGUIWndActivityMergeShowActivityItemGrid(GGUIMonoActivityMergeShowActivityItemGrid _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onWndInitDone()
        {
        }

        protected override void _onShowWnd()
        {
            _createSecTask();
        }

        protected override void _onHideWnd()
        {
            _discardSecTask();
        }

        protected override void _onReset()
        {
            _discardSecTask();
        }

        protected override void _onDiscard()
        {
            _discardSecTask();
            
            onGotoBtnClick = null;
            _m_lDataList = null;
        }

        protected override GGUIWndActivityMergeShowActivityItem _createItemWnd(GGUIMonoActivityMergeShowActivityItem _itemMono)
        {
            GGUIWndActivityMergeShowActivityItem itemWnd = new GGUIWndActivityMergeShowActivityItem(_itemMono);
            itemWnd.onGotoBtnClick += _onItemGotoBtnClick;
            return itemWnd;
        }
        
        protected override void _onRefreshItemWnd(GGUIWndActivityMergeShowActivityItem _itemWnd, int _itemIdx)
        {
            if (_itemWnd == null || _m_lDataList == null || _itemIdx < 0 || _itemIdx >= _m_lDataList.Count)
                return;

            KeyValuePair<PushNoticeActivityMergeRefObj, _ABaseActivityInfo> data = _m_lDataList[_itemIdx];
            _itemWnd.refreshWnd(data.Value, data.Key);
        }

        /// <summary>
        /// 设置数据并刷新列表
        /// </summary>
        /// <param name="_dataDict">数据字典：key为活动合并展示配表数据，value为活动信息</param>
        public void setData(List<KeyValuePair<PushNoticeActivityMergeRefObj, _ABaseActivityInfo>> _dataDict)
        {
            _m_lDataList = _dataDict;
            int itemCount = _m_lDataList?.Count ?? 0;
            
            setItemCount(itemCount);
        }

        /// <summary>
        /// Item前往按钮点击回调
        /// </summary>
        private void _onItemGotoBtnClick(GGUIWndActivityMergeShowActivityItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            onGotoBtnClick?.Invoke(_itemWnd);
        }

        #region 每秒任务

        private void _discardSecTask()
        {
            _m_secTask.setDisable();
        }
        
        private void _createSecTask()
        {
            _discardSecTask();

            _m_secTask = CommonTaskController.CommonEnableDurationActionAddMonoTask(() =>
            {
                refreshAllItem((_itemWnd, _index) =>
                {
                    _itemWnd?.secTick();
                });
            }, 1f);
        }

        #endregion
    }
}
