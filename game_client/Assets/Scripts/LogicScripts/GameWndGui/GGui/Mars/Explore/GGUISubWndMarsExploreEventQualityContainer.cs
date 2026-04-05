using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndMarsExploreEventQualityContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsExploreEventQualityContainerItem, GGUIMonoMarsExploreEventQualityContainer, GGUISubWndMarsExploreEventQualityContainerItem>
    {
        [CanBeNull] private List<CommonQualityWeight> _m_qualityList;


        public GGUISubWndMarsExploreEventQualityContainer(GGUIMonoMarsExploreEventQualityContainer _containerMono)
            : base(_containerMono)
        {
            initWnd();
        }


        protected override GGUISubWndMarsExploreEventQualityContainerItem _createItemWnd(GGUIMonoMarsExploreEventQualityContainerItem _itemMono)
        {
            return new GGUISubWndMarsExploreEventQualityContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUISubWndMarsExploreEventQualityContainerItem _itemWnd, int _index)
        {
            CommonQualityWeight qualityWeight = _m_qualityList?.SafeGet(_index);
            _itemWnd.refreshWnd(qualityWeight);
        }


        public void refreshWnd([CanBeNull] List<CommonQualityWeight> _qualityList)
        {
            _m_qualityList = _qualityList;
            refreshWnd(_qualityList?.Count ?? 0);
        }
    }
}
