
using System.Collections.Generic;
using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 宝箱奖励容器
    /// </summary>
    public class GGUIWndNumMergeBoxRewardContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoNumMergeBoxRewardContainer, GGUIWndNumMergeBoxRewardContainerItem>
    {
        [NotNull] private readonly List<GGUIWndNumMergeBoxRewardContainerItem> _m_lItemList;


        public GGUIWndNumMergeBoxRewardContainer(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {
            _m_lItemList = new List<GGUIWndNumMergeBoxRewardContainerItem>();
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


        protected override GGUIWndNumMergeBoxRewardContainerItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            return new GGUIWndNumMergeBoxRewardContainerItem(_itemMono);
        }


        /// <summary>
        /// 刷新奖励显示（按品质分组）
        /// </summary>
        public void refreshWnd(List<QualityProbabilityInfo> _qualityProbList)
        {
            if (!_m_bIsShow || _qualityProbList == null)
                return;

            int count = 0;
            for (int i = 0; i < _qualityProbList.Count; i++)
            {
                GGUIWndNumMergeBoxRewardContainerItem itemWnd;

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
                itemWnd.refreshWnd(_qualityProbList[i]);
                count++;
            }

            // 隐藏多余的Item
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                removeItemWnd(_m_lItemList[j]);
                _m_lItemList.RemoveAt(j);
            }
        }
    }
}
