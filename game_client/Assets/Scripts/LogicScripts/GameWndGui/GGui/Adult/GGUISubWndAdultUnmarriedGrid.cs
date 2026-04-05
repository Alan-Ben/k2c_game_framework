using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUISubWndAdultUnmarriedGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoAdultUnmarriedGridItem, GGUIMonoAdultUnmarriedGrid, GGUISubWndAdultUnmarriedGridItem>
    {
        private List<UnmarriedInfo> _m_unmarriedInfoList;
        //item点击申请组队按钮
        private Action<UnmarriedInfo> _m_aOnClickItemApply;

        /// <summary>
        /// item点击申请组队按钮
        /// </summary>
        public Action<UnmarriedInfo> onClickItemApply { get { return _m_aOnClickItemApply; } set { _m_aOnClickItemApply = value; } }

        public GGUISubWndAdultUnmarriedGrid(GGUIMonoAdultUnmarriedGrid _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
        }
        protected override void _onHideWnd()
        {
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
        }
        protected override void _onWndInitDone()
        {
        }
        protected override GGUISubWndAdultUnmarriedGridItem _createItemWnd(GGUIMonoAdultUnmarriedGridItem _itemMono)
        {
            GGUISubWndAdultUnmarriedGridItem item = new GGUISubWndAdultUnmarriedGridItem(_itemMono);
            item.onClickItemApply += _onClickItemApply;
            return item;
        }
        protected override void _onRefreshItemWnd(GGUISubWndAdultUnmarriedGridItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null || _m_unmarriedInfoList == null || _itemIdx < 0 || _itemIdx >= _m_unmarriedInfoList.Count)
                return;
            
            _itemMono.refreshWnd(_m_unmarriedInfoList[_itemIdx]);
        }
        

        public void refreshWnd(List<UnmarriedInfo> _unmarriedInfoList)
        {
            _m_unmarriedInfoList = _unmarriedInfoList;
            setItemCount(_m_unmarriedInfoList?.Count ?? 0);

            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _m_unmarriedInfoList == null || _m_unmarriedInfoList.Count == 0);
        }

        /// <summary>
        /// 点击申请组队按钮
        /// </summary>
        /// <param name="_info"></param>
        private void _onClickItemApply(UnmarriedInfo _info)
        {
            _m_aOnClickItemApply?.Invoke(_info);
        }
    }
}