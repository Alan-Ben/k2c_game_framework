using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndSummonPublicRollRecordGrid : _ATNPGGUIWndRefreshGrid<GGUIMonoSummonPublicRollRecordGridItem, GGUIMonoSummonPublicRollRecordGrid, GGUIWndSummonPublicRollRecordGridItem>
    {
        private long _m_lPoolId;
        private List<Common.GachaObj.Gacha_PublicRecordInfo> _m_lRecordInfoList;

        private ALCommonEnableTaskController _m_reqRecordInfoTask;//请求公告召唤记录信息任务
        
        public GGUIWndSummonPublicRollRecordGrid(GGUIMonoSummonPublicRollRecordGrid _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndSummonPublicRollRecordGridItem _createItemWnd(GGUIMonoSummonPublicRollRecordGridItem _itemMono)
        {
            GGUIWndSummonPublicRollRecordGridItem itemWnd = new GGUIWndSummonPublicRollRecordGridItem(_itemMono);
            return itemWnd;
        }

        protected override void _onRefreshItemWnd(GGUIWndSummonPublicRollRecordGridItem _itemMono, int _itemIdx)
        {
            if(_itemMono == null || _itemIdx < 0 || _m_lRecordInfoList == null || _m_lRecordInfoList.Count <= 0 || _itemIdx >= _m_lRecordInfoList.Count)
                return;
            
            _itemMono.showWnd();
            _itemMono.setData(_m_lRecordInfoList[_m_lRecordInfoList.Count - _itemIdx - 1]);
        }

        protected override void _refreshData(Action<bool> _complete)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();

            _discardReqRecordInfoTask();
            
            _m_lRecordInfoList?.Clear();
            _m_lRecordInfoList = null;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lRecordInfoList?.Clear();
            
            _discardReqRecordInfoTask();
        }

        protected override void _onReset()
        {
        }

        public void setData(long _poolId)
        {
            _m_lPoolId = _poolId;

            _refreshWnd();
            _initReqRecordInfoTask();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            if (_m_lRecordInfoList == null || _m_lRecordInfoList.Count <= 0 || wnd.showItemCount <= 0)
            {
                setItemCount(0);
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, true);
                
                return;
            }
            
            if(_m_lRecordInfoList.Count > wnd.showItemCount)
                _m_lRecordInfoList.RemoveRange(0, _m_lRecordInfoList.Count - wnd.showItemCount);
            
            ALUGUICommon.setGameObjEnable(wnd.noItemShow, false);
            setItemCount(_m_lRecordInfoList.Count);
        }
        
        /// <summary>
        /// 初始化请求公告召唤记录信息任务
        /// </summary>
        private void _initReqRecordInfoTask()
        {
            _discardReqRecordInfoTask();
            if(wnd == null || wnd.showItemCount <= 0 || wnd.reqNewDataInterval <= 0 || _m_lPoolId <= 0)
                return;
            
            _m_reqRecordInfoTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_dealAction, wnd.reqNewDataInterval);
        }

        private void _dealAction()
        {
            if(_m_lPoolId <= 0 || wnd == null)
                return;
            
            if (_m_lRecordInfoList == null)
                _m_lRecordInfoList = new List<Common.GachaObj.Gacha_PublicRecordInfo>();
         
            NPPlayer.instance.gachaComp.reqGachaPublicRollRecord(_m_lPoolId, _m_lRecordInfoList.GetLast()?.getDbId() ?? 0,
                (_msg) =>
                {
                    if(_msg == null || _m_lRecordInfoList == null)
                        return;

                    List<Common.GachaObj.Gacha_PublicRecordInfo> recordList = _msg.getRecordList();
                    if (recordList == null || recordList.Count <= 0)
                        return;
                    
                    recordList.Sort(_sortGacha_PublicRecordInfo);
                    _m_lRecordInfoList.AddRange(recordList);
                    _refreshWnd();
                }, null);
        }
        
        /// <summary>
        /// 销毁请求公告召唤记录信息任务
        /// </summary>
        private void _discardReqRecordInfoTask()
        {
            _m_reqRecordInfoTask.setDisable();
        }

        /// <summary>
        /// 对记录数据排序(dbId从小到大)
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private int _sortGacha_PublicRecordInfo(Common.GachaObj.Gacha_PublicRecordInfo _a,
            Common.GachaObj.Gacha_PublicRecordInfo _b)
        {
            if (ReferenceEquals(_a, _b))
                return 0;
            if (_b == null)
                return -1;
            if (_a == null)
                return 1;

            return _a.getDbId().CompareTo(_b.getDbId());
        }
    }
}