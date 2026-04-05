
using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndAddPackInstallerContainer : _AGGUISubWndCommonContainer<GGUIMonoAddPackInstallerContainerItem, GGUIMonoAddPackInstallerContainer, GGUIWndAddPackInstallerContainerItem>
    {
        private IReadOnlyList<AddPackInstaller> _m_installerList;
        
        
        public GGUIWndAddPackInstallerContainer(GGUIMonoAddPackInstallerContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }


        public void setShowData(IReadOnlyList<AddPackInstaller> _installerList)
        {
            _m_installerList = _installerList;
            refreshWnd(_m_installerList?.Count ?? 0);
        }
        

        protected override GGUIWndAddPackInstallerContainerItem _createItemWnd(GGUIMonoAddPackInstallerContainerItem _itemMono)
        {
            return new GGUIWndAddPackInstallerContainerItem(_itemMono);
        }
        protected override void _refreshItemWnd(GGUIWndAddPackInstallerContainerItem _itemWnd, int _index)
        {
            if (_m_installerList == null)
                return;
            
            if (_index < 0 || _index >= _m_installerList.Count)
                return;
            
            _itemWnd.setInstaller(_m_installerList[_index]);
        }
    }
}