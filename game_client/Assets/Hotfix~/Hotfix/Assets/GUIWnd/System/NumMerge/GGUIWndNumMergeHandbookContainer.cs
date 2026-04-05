using System.Collections.Generic;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 2048 图鉴container
    /// </summary>
    public class GGUIWndNumMergeHandbookContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoNumMergeHandbookContainer, GGUIWndNumMergeHandbookContainerItem>
    {
        //窗口容器
        protected List<GGUIWndNumMergeHandbookContainerItem> _m_lItemList;

        public GGUIWndNumMergeHandbookContainer(GGUIHotfixCommonMono containerWndMono) : base(containerWndMono)
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

        protected override GGUIWndNumMergeHandbookContainerItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            return new GGUIWndNumMergeHandbookContainerItem(_itemMono);
        }

        protected override void _onWndInitDoneHotfix()
        {
            _m_lItemList = new List<GGUIWndNumMergeHandbookContainerItem>();
        }

        /// <summary>
        /// 显示图鉴列表
        /// </summary>
        public void showItemList()
        {
            if (_m_lItemList == null)
                return;

            //获取所有棋子配置
            List<NumMergeBlockRefObj> blockList = HotfixRefdataCoreMgr.instance.numMergeBlockRefCore.refList;
            if (blockList == null)
                return;

            GGUIWndNumMergeHandbookContainerItem itemWnd = null;
            int count = 0;

            //遍历所有棋子
            for (int i = 0; i < blockList.Count; i++)
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
                itemWnd.refreshWnd(blockList[i]);
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
