using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndConsortCGGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoConsortCGGridItem, GGUIMonoConsortCGGrid, GGUIWndConsortCGGridItem>
    {
        private List<ConsortCGRefObj> _m_lConsortCGRefObjList;//妃子CG数据列表
        
        public GGUIWndConsortCGGrid(GGUIMonoConsortCGGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        public Action<GGUIWndConsortCGGridItem> onClickDrawReward;//当点击领取奖励时的回调
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_ADD_CG, _onCgChg);
            WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CG_CHG, _onCgChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_ADD_CG, _onCgChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CG_CHG, _onCgChg);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            onClickDrawReward = null;
        }

        protected override void _onWndInitDone()
        {
        }

        protected override GGUIWndConsortCGGridItem _createItemWnd(GGUIMonoConsortCGGridItem _itemMono)
        {
            GGUIWndConsortCGGridItem itemWnd = new GGUIWndConsortCGGridItem(_itemMono);
            itemWnd.onClickDrawReward += onClickDrawReward;
            return itemWnd;
        }
        
        protected override void _onRefreshItemWnd(GGUIWndConsortCGGridItem _itemMono, int _itemIdx)
        {
            if(_itemMono == null || _m_lConsortCGRefObjList == null || _itemIdx < 0 || _itemIdx >= _m_lConsortCGRefObjList.Count)
                return;
            
            _itemMono.setData(_m_lConsortCGRefObjList[_itemIdx], _itemIdx);
        }
        
        public void setData(List<ConsortCGRefObj> _consortCgRefList)
        {
            _m_lConsortCGRefObjList = _consortCgRefList;

            if (_m_lConsortCGRefObjList == null || _m_lConsortCGRefObjList.Count <= 0)
            {
                if(wnd != null)
                    ALUGUICommon.setGameObjEnable(wnd.noItemShow, true);
                
                setItemCount(0);
                return;
            }

            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, false);
            
            setItemCount(_m_lConsortCGRefObjList.Count);
        }

        /// <summary>
        /// 有CG数据变化
        /// </summary>
        private void _onCgChg(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is ConsortCgInfo) || _m_lConsortCGRefObjList == null)
                return;
            
            ConsortCgInfo consortCgInfo = (ConsortCgInfo)_objs[0];
            if(consortCgInfo == null)
                return;
            
            int index = _m_lConsortCGRefObjList.FindIndex((_cgRef) => _cgRef != null && _cgRef.cg_id == consortCgInfo.cgId);
            
            forceRefreshItem(index);
        }
    }
}