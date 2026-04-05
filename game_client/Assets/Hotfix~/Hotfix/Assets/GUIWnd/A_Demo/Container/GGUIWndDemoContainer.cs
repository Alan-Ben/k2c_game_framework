using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 范例container窗口
    /// </summary>
    public class GGUIWndDemoContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoDemoContainer, GGUIWndDemoContainerItem>
    {
        //窗口容器
        protected List<GGUIWndDemoContainerItem> _m_lItemList;

        public GGUIWndDemoContainer(GGUIHotfixCommonMono containerWndMono) : base(containerWndMono)
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

        protected override GGUIWndDemoContainerItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            return new GGUIWndDemoContainerItem(_itemMono);
        }

        protected override void _onWndInitDoneHotfix()
        {
            _m_lItemList = new List<GGUIWndDemoContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_list"></param>
        public void showItemList(List<int> _list)
        {
            if (_list == null || _m_lItemList == null)
                return;

            GGUIWndDemoContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _list.Count; i++)
            {
                int num = _list[i];

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
                itemWnd.setInfo(num);
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