using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIWndTreasureHuntAkeyCaptureResultContainer : _AGGUISubWndCommonContainer<GGUIMonoTreasureHuntAkeyCaptureResultItem, GGUIMonoTreasureHuntAkeyCaptureResultContainer, GGUIWndTreasureHuntAkeyCaptureResultItem>
    {
        private List<TreasureHuntCaptureResultBase> _m_lCaptureResultList;

        public GGUIWndTreasureHuntAkeyCaptureResultContainer(GGUIMonoTreasureHuntAkeyCaptureResultContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onDiscard()
        {
            _m_lCaptureResultList = null;//因为列表数据源不是这里的, 所以这里只要将列表置为空, 不需要清除
            
            base._onDiscard();
        }

        protected override GGUIWndTreasureHuntAkeyCaptureResultItem _createItemWnd(GGUIMonoTreasureHuntAkeyCaptureResultItem _itemMono)
        {
            GGUIWndTreasureHuntAkeyCaptureResultItem itemWnd = new GGUIWndTreasureHuntAkeyCaptureResultItem(_itemMono);
            return itemWnd;
        }

        protected override void _refreshItemWnd(GGUIWndTreasureHuntAkeyCaptureResultItem _itemWnd, int _index)
        {
            if (_m_lCaptureResultList == null || _index < 0 || _index >= _m_lCaptureResultList.Count)
                return;

            TreasureHuntCaptureResultBase captureResult = _m_lCaptureResultList.SafeGet(_index);
            _itemWnd.setData(captureResult);
        }

        /// <summary>
        /// 设置捕获结果数据
        /// </summary>
        /// <param name="_captureResultList">捕获结果列表</param>
        public void setData(List<TreasureHuntCaptureResultBase> _captureResultList)
        {
            _m_lCaptureResultList = _captureResultList;

            int itemCount = _m_lCaptureResultList?.Count ?? 0;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShow, itemCount <= 0);
            }

            refreshWnd(itemCount);
        }
    }
}