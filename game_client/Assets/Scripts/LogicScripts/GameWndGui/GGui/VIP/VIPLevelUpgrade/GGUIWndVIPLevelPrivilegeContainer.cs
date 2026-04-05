using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// VIP等级特权描述列表
    /// </summary>
    public class GGUIWndVIPLevelPrivilegeContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoVIPLevelPrivilegeContainerItem, GGUIMonoVIPLevelPrivilegeContainer, GGUIWndVIPLevelPrivilegeContainerItem>
    {
        //item列表
        protected List<GGUIWndVIPLevelPrivilegeContainerItem> _m_lItemList;

        public GGUIWndVIPLevelPrivilegeContainer(GGUIMonoVIPLevelPrivilegeContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndVIPLevelPrivilegeContainerItem _createItemWnd(GGUIMonoVIPLevelPrivilegeContainerItem _itemMono)
        {
            GGUIWndVIPLevelPrivilegeContainerItem item = new GGUIWndVIPLevelPrivilegeContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndVIPLevelPrivilegeContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<VIPUpgradePrivilegeShowData> _showDataList)
        {
            if (wnd == null || _showDataList == null || _m_lItemList == null)
                return;

            GGUIWndVIPLevelPrivilegeContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _showDataList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.setInfo(_showDataList[i]);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }

            ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, _showDataList.Count > 0);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _showDataList.Count <= 0);
        }
    }
}
