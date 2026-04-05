
using System.Collections.Generic;
using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 宝箱奖励物品容器
    /// </summary>
    public class GGUIWndNumMergeBoxRewardItemContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoNumMergeBoxRewardItemContainer, GGUIWndNumMergeBoxRewardItemContainerItem>
    {
        [NotNull] private readonly List<GGUIWndNumMergeBoxRewardItemContainerItem> _m_lItemList;


        public GGUIWndNumMergeBoxRewardItemContainer(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {
            _m_lItemList = new List<GGUIWndNumMergeBoxRewardItemContainerItem>();
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
        protected override void _onWndInitDoneHotfix()
        {
        }


        protected override GGUIWndNumMergeBoxRewardItemContainerItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            return new GGUIWndNumMergeBoxRewardItemContainerItem(_itemMono);
        }


        /// <summary>
        /// 刷新物品列表
        /// </summary>
        /// <param name="_itemList">物品列表</param>
        /// <param name="_probList">概率列表（万分比）</param>
        public void refreshWnd(List<NPCommonCostItem> _itemList, List<int> _probList)
        {
            if (!_m_bIsShow || _itemList == null || _probList == null)
                return;

            int count = 0;
            int itemCount = _itemList.Count < _probList.Count ? _itemList.Count : _probList.Count;

            for (int i = 0; i < itemCount; i++)
            {
                GGUIWndNumMergeBoxRewardItemContainerItem itemWnd;

                // 如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (itemWnd == null)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                else
                    itemWnd = _m_lItemList[i];

                itemWnd.showWnd();
                itemWnd.refreshWnd(_itemList[i], _probList[i]);
                count++;
            }

            // 隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                removeItemWnd(_m_lItemList[j]);
                _m_lItemList.RemoveAt(j);
            }
        }
    }
}
