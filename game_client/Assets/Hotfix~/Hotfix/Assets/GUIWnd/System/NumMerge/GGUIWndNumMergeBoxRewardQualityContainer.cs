
using System.Collections.Generic;
using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    /// <summary>
    /// 宝箱奖励品质容器
    /// </summary>
    public class GGUIWndNumMergeBoxRewardQualityContainer : _AHotfixBaseShowAnimContainerWnd<GGUIMonoNumMergeBoxRewardQualityContainer, GGUIWndNumMergeBoxRewardQualityContainerItem>
    {
        [NotNull] private readonly List<GGUIWndNumMergeBoxRewardQualityContainerItem> _m_lItemList;


        public GGUIWndNumMergeBoxRewardQualityContainer(GGUIHotfixCommonMono _containerMono) : base(_containerMono)
        {
            _m_lItemList = new List<GGUIWndNumMergeBoxRewardQualityContainerItem>();
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


        protected override GGUIWndNumMergeBoxRewardQualityContainerItem _createItemWnd(GGUIHotfixCommonMono _itemMono)
        {
            return new GGUIWndNumMergeBoxRewardQualityContainerItem(_itemMono);
        }


        /// <summary>
        /// 刷新品质概率显示
        /// </summary>
        /// <param name="_qualityProbList">品质概率列表</param>
        /// <param name="_compareQualityProbList">用于对比的品质概率列表（下一级用来对比当前级）</param>
        public void refreshWnd(List<QualityProbabilityInfo> _qualityProbList, List<QualityProbabilityInfo> _compareQualityProbList = null)
        {
            if (!_m_bIsShow || _qualityProbList == null)
                return;

            int count = 0;
            for (int i = 0; i < _qualityProbList.Count; i++)
            {
                GGUIWndNumMergeBoxRewardQualityContainerItem itemWnd;

                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (itemWnd == null)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                else
                    itemWnd = _m_lItemList[i];

                QualityProbabilityInfo info = _qualityProbList[i];

                // 判断是否有提升（对比概率）
                bool isUpgrade = false;
                if (_compareQualityProbList != null)
                {
                    foreach (QualityProbabilityInfo compareInfo in _compareQualityProbList)
                    {
                        if (compareInfo.quality == info.quality)
                        {
                            isUpgrade = info.probability > compareInfo.probability;
                            break;
                        }
                    }
                }

                itemWnd.showWnd();
                itemWnd.refreshWnd(info.quality, info.probability, isUpgrade);
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
