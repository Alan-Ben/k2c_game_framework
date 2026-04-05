using Common.StageGoalObj;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 阶段目标到达时代之巅详情弹窗列表
    /// </summary>
    public class GGUIWndStageGoalReachPeakDetailContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoStageGoalReachPeakDetailContainerItem, GGUIMonoStageGoalReachPeakDetailContainer, GGUIWndStageGoalReachPeakDetailContainerItem>
    {
        //item列表
        protected List<GGUIWndStageGoalReachPeakDetailContainerItem> _m_lItemList;

        public GGUIWndStageGoalReachPeakDetailContainer(GGUIMonoStageGoalReachPeakDetailContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndStageGoalReachPeakDetailContainerItem _createItemWnd(GGUIMonoStageGoalReachPeakDetailContainerItem _itemMono)
        {
            GGUIWndStageGoalReachPeakDetailContainerItem item = new GGUIWndStageGoalReachPeakDetailContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _m_lItemList[i]?.hideWnd();
                }
            }
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
            _m_lItemList = new List<GGUIWndStageGoalReachPeakDetailContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void showItemList(List<StageGoal_BigStepFirstReachInfo> _infoList)
        {
            if (_infoList == null || _m_lItemList == null)
                return;

            //按时间从早到晚排序
            _infoList.Sort((_a,_b)=>_a.getReachTimeMs().CompareTo(_b.getReachTimeMs()));

            GGUIWndStageGoalReachPeakDetailContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _infoList.Count; i++)
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

                count++;
                itemWnd.showWnd();
                itemWnd.setInfo(_infoList[i], count);
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
