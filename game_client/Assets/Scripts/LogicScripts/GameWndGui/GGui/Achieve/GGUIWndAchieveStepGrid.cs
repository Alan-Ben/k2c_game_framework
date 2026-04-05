using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 成就步骤信息列表
    /// </summary>
    public class GGUIWndAchieveStepGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoAchieveStepGridItem, GGUIMonoAchieveStepGrid, GGUIWndAchieveStepGridItem>
    {
        [NotNull]private List<AchieveStepInfo> _m_showList;

        public GGUIWndAchieveStepGrid(GGUIMonoAchieveStepGrid _wnd)
            : base(_wnd)
        {
            _m_showList = new List<AchieveStepInfo>();
            initWnd();
        }

        protected override GGUIWndAchieveStepGridItem _createItemWnd(GGUIMonoAchieveStepGridItem _itemMono)
        {
            return new GGUIWndAchieveStepGridItem(_itemMono);
        }

        protected override void _refreshItemwnd(GGUIWndAchieveStepGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_showList.Count)
                return;

            AchieveStepInfo info = _m_showList[_itemIdx];

            _itemWnd.setInfo(info);
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

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_list"></param>
        public void setShowData(AchieveInfo _achieveInfo)
        {
            if(_achieveInfo == null || _achieveInfo.achieveRefObj == null || _achieveInfo.stepInfoList == null)
                return;

            AchieveTypeRefObj achieveTypeRef = GRefdataCoreMgr.instance.achieveTypeMap.getRef((long)_achieveInfo.achieveType);
            _m_showList.Clear();
            _m_showList.AddRange(_achieveInfo.stepInfoList);

            //排序
            if(achieveTypeRef != null && achieveTypeRef.need_reorder)
                _m_showList.Sort(_sortByState);
            else
                _m_showList.Sort(_sortByStepId);

            setItemCount(_m_showList.Count);

            //当前正在进行中的步骤id
            long curInProgressStepId = 0;
            if (_achieveInfo.curStepInfo != null)
                curInProgressStepId = _achieveInfo.curStepInfo.step;
            //列表移动的目标位置
            int targetIndex = 0;
            for (int i = 0; i < _m_showList.Count; i++)
            {
                if (_m_showList[i].step == curInProgressStepId)
                {
                    targetIndex = i;
                    break;
                }
            }
            //移动到目标位置
            _moveToTarget(targetIndex);
        }

        public void setShowData(List<AchieveStepInfo> _stepInfoList)
        {
            if (_stepInfoList == null)
            {
                setItemCount(0);
                return;
            }
            
            _m_showList.Clear();
            _m_showList.AddRange(_stepInfoList);
            setItemCount(_m_showList.Count);
        }

        //按stepId排序
        private int _sortByStepId(AchieveStepInfo _a, AchieveStepInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            return _a.step.CompareTo(_b.step);
        }

        //按领取状态排序
        private int _sortByState(AchieveStepInfo _a, AchieveStepInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            if (_a.getRewardState().CompareTo(_b.getRewardState()) != 0)
                return _a.getRewardState().CompareTo(_b.getRewardState());
            else 
                return _a.step.CompareTo(_b.step);
        }

        //移动到目标位置
        private void _moveToTarget(int _index)
        {
            if (wnd == null || null == wnd.itemTemplate)
                return;

            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                // 计算content的坐标活动范围
                if (wnd != null && 
                    wnd.itemTemplate != null && 
                    wnd.gridAreaUIObj != null && 
                    wnd.gridAreaUIObj.rect != null && 
                    wnd.gridAreaMaskObj != null && 
                    wnd.gridAreaMaskObj.rect != null)
                {
                    float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                    moveToVerticalRate((_index * (wnd.itemTemplate.height + wnd.spaceSize.y)) / contentRange);
                }
            });
        }

        /// <summary>
        /// 移动到第一个可领取奖励的步骤 或 第一个未达成的步骤
        /// </summary>
        public void moveToFirstCanDrawOrFirstUnReachedStep()
        {
            int firstUnReachedIndex = -1;//第一个未达成的步骤索引

            AchieveStepInfo stepInfo = null;
            for (int i = 0; i < _m_showList.Count; i++)
            {
                stepInfo = _m_showList[i];
                if(stepInfo == null)
                    continue;

                // 若还未记录第一个未达成的步骤, 且当前步骤未达成
                if (firstUnReachedIndex == -1 && !stepInfo.isFull())
                {
                    firstUnReachedIndex = i;
                }

                // 若当前步骤可以领取奖励, 不需要继续遍历了, 直接移动到这个步骤, 并返回
                if (stepInfo.getRewardState() == ENPCommonGetStat.CAN_GET)
                {
                    _moveToTarget(i);
                    return;
                }
            }
            
            // 代码运行到这里说明没有找到可领取奖励的步骤, 列表移动到第一个未达成的步骤
            if(firstUnReachedIndex > -1)
                _moveToTarget(firstUnReachedIndex);
        }
    }
}
