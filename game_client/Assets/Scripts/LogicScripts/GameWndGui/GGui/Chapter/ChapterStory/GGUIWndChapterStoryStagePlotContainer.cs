using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndChapterStoryStagePlotContainer : _ATNPGGUIWndSizeChangeableContainer<GGUIMonoChapterStoryStagePlotItem, GGUIMonoChapterStoryStagePlotContainer, GGUIWndChapterStoryStagePlotItem>
    {
        private List<ChapterStoryStagePlotShowInfo> _m_lPlotShowInfoList;
        
        public GGUIWndChapterStoryStagePlotContainer(GGUIMonoChapterStoryStagePlotContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<ChapterStoryStagePlotShowInfo> onClickDrawRewardBtn;
        
        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            onClickDrawRewardBtn = null;
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

        protected override GGUIWndChapterStoryStagePlotItem _createItemWnd(GGUIMonoChapterStoryStagePlotItem _itemMono)
        {
            GGUIWndChapterStoryStagePlotItem itemWnd = new GGUIWndChapterStoryStagePlotItem(_itemMono);
            itemWnd.onClickDrawRewardBtn += _onClickDrawRewardBtn;
            return itemWnd;
        }

        protected override void _discardItem(GGUIWndChapterStoryStagePlotItem _itemWnd)
        {
            if(_itemWnd != null)
                _itemWnd.onClickDrawRewardBtn -= _onClickDrawRewardBtn;
            
            base._discardItem(_itemWnd);
        }

        protected override bool _refreshItemWnd(int _index, GGUIWndChapterStoryStagePlotItem _itemWnd)
        {
            if(_itemWnd == null || _m_lPlotShowInfoList == null || _index < 0 || _index >= _m_lPlotShowInfoList.Count)
                return false;
            
            _itemWnd.setData(_m_lPlotShowInfoList[_index]);
            return true;
        }
        
        public void setData(List<ChapterStoryStagePlotShowInfo> _plotShowInfoList)
        {
            _m_lPlotShowInfoList = _plotShowInfoList;

            showItemList(_m_lPlotShowInfoList?.Count ?? 0);
        }
        
        private void _onClickDrawRewardBtn(ChapterStoryStagePlotShowInfo _plotShowInfo)
        {
            onClickDrawRewardBtn?.Invoke(_plotShowInfo);
        }
    }
}