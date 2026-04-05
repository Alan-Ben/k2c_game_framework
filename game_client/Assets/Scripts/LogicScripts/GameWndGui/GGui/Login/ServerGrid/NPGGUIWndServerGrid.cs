using ALPackage;
using System.Collections.Generic;


namespace GOE
{
    /// <summary>
    /// 服务器列表
    /// </summary>
    public class NPGGUIWndServerGrid : _AALUGUIBasicGridSubWnd<NPGGUIMonoServerGridItem, NPGGUIMonoServerGrid, NPGGUIWndServerGridItem>
    {
        private List<ServerShowData> _m_serverList;//服务器列表数据
        private string _m_sAreaTag;//大区标识

        public NPGGUIWndServerGrid(NPGGUIMonoServerGrid _containerWnd)
            : base(_containerWnd)
        {
            initWnd();
        }

        protected override NPGGUIWndServerGridItem _createItemWnd(NPGGUIMonoServerGridItem _itemMono)
        {
            return new NPGGUIWndServerGridItem(_itemMono);
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        protected override void _refreshItemwnd(NPGGUIWndServerGridItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_serverList.Count)
                return;

            //获取数据
            ServerShowData data = _m_serverList[_itemIdx];
            if (null == data)
                return;

            _itemMono.setInfo(data);
            _itemMono.setSelected(false);

            //如果是当前大区，设置当前服务器为选中状态
            if (_m_sAreaTag == CDNSetting_AreaInfo.instance.areaTag)
            {
                GameInit_SelectServer.instance.getCurSelectServerItem(serverId =>
                {
                    _itemMono.setSelected(data.serverDataInfo.serverId == serverId);
                });
            }
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="_serverList"></param>
        /// <param name="_selectedServer"></param>
        public void showWnd(string _selectedAreaTag, List<ServerShowData> _serverList)
        {
            if (wnd == null)
                return;

            base.showWnd();
            _m_sAreaTag = _selectedAreaTag;
            _m_serverList = _serverList;

            ALUGUICommon.setGameObjEnable(wnd.emptyShowGo, _m_serverList.Count == 0);

            setItemCount(_m_serverList.Count);
            moveToTop();
        }
    }
}
