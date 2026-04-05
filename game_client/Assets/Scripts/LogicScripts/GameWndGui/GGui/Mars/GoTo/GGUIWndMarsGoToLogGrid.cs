using JetBrains.Annotations;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 火星航行日志列表
    /// </summary>
    public class GGUIWndMarsGoToLogGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMarsGoToLogGridItem, GGUIMonoMarsGoToLogGrid, GGUIWndMarsGoToLogGridItem>
    {
        //总的数据列表
        [NotNull] private List<MarsStageLogInfo> _m_lInfoList = new List<MarsStageLogInfo>();

        public GGUIWndMarsGoToLogGrid(GGUIMonoMarsGoToLogGrid _containerMono) : base(_containerMono)
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
            _m_lInfoList?.Clear();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override GGUIWndMarsGoToLogGridItem _createItemWnd(GGUIMonoMarsGoToLogGridItem _itemMono)
        {
            // 创建对象
            GGUIWndMarsGoToLogGridItem gridItem = new GGUIWndMarsGoToLogGridItem(_itemMono);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndMarsGoToLogGridItem _itemMono, int _itemIdx)
        {
            if (_m_lInfoList.Count <= _itemIdx)
                return;

            MarsStageLogInfo info = _m_lInfoList[_itemIdx];
            _itemMono?.setInfo(info);
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void setInfo(List<MarsStageLogInfo> _infoList)
        {
            if (_infoList == null)
                return;

            _m_lInfoList = _infoList;
            setItemCount(_infoList.Count);
        }
    }
}
