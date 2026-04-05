using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星基地民意信件网格列表窗口
    /// </summary>
    public class GGUIWndMarsPopularWillLetterItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsPopularWillLetterItem, GGUIMonoMarsPopularWillLetterItemGrid, GGUIWndMarsPopularWillLetterItem>
    {
        private List<_IMarsPeopleWillLetter> _m_lLetterList;
        
        public GGUIWndMarsPopularWillLetterItemGrid(GGUIMonoMarsPopularWillLetterItemGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        protected override void _onDiscard()
        {
            _m_lLetterList = null;
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
        
        protected override void _onRefreshItemWnd(GGUIWndMarsPopularWillLetterItem _itemMono, int _itemIdx)
        {
            if (_itemMono == null || _m_lLetterList == null || _itemIdx < 0 || _itemIdx >= _m_lLetterList.Count)
                return;
                
            _itemMono.setData(_m_lLetterList[_itemIdx]);
        }

        protected override GGUIWndMarsPopularWillLetterItem _createItemWnd(GGUIMonoMarsPopularWillLetterItem _itemMono)
        {
            GGUIWndMarsPopularWillLetterItem itemWnd = new GGUIWndMarsPopularWillLetterItem(_itemMono);
            return itemWnd;
        }
        
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_letterList">民意信件数据列表</param>
        public void setData(List<_IMarsPeopleWillLetter> _letterList)
        {
            _m_lLetterList = _letterList;
            
            int count = _m_lLetterList?.Count ?? 0;
            setItemCount(count);
            
            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, count <= 0);
        }
    }
}