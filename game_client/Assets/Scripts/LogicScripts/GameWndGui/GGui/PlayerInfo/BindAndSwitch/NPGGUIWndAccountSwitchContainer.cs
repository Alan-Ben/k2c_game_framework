using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 账号切换方式列表
    /// </summary>
    public class NPGGUIWndAccountSwitchContainer : _ANPGGUIBasicSubWndContainer<NPGGUIMonoAccountSwitchContainerItem, NPGGUIMonoAccountSwitchContainer, NPGGUIWndAccountSwitchContainerItem>
    {
        protected List<NPGGUIWndAccountSwitchContainerItem> _m_lItemList;

        public NPGGUIWndAccountSwitchContainer(NPGGUIMonoAccountSwitchContainer _containerMono) : base(_containerMono)
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
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<NPGGUIWndAccountSwitchContainerItem>();
        }

        protected override NPGGUIWndAccountSwitchContainerItem _createItemWnd(NPGGUIMonoAccountSwitchContainerItem _itemMono)
        {
            return new NPGGUIWndAccountSwitchContainerItem(_itemMono);
        }

        public void setInfo(List<NPLoginWayRefObj> _list)
        {
            if (_m_lItemList == null)
                return;

            int count = 0;
            //遍历数据
            for (int i = 0; i < _list.Count; i++)
            {
                NPLoginWayRefObj itemInfo = _list[i];
                if (itemInfo == null)
                    continue;

                NPGGUIWndAccountSwitchContainerItem itemWnd = null;
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (itemWnd == null)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.setInfo(itemInfo);
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
        }
    }
}
